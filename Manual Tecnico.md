# Manual Técnico — Whispers in Letters

> **Proyecto:** Adaptación a videojuego del libro *El Mundo de Sofía*
> **Motor:** Unity (3D, third-person) | **Narrativa:** Ink (Inkle Studios) | **Input:** Unity Input System

---

## Índice General

| Cap | Contenido |
|-----|-----------|
| [**1**](#capítulo-1-arquitectura-general) | Arquitectura General — Managers, dependencias, ciclo de vida |
| [**2**](#capítulo-2-sistema-narrativo) | Sistema Narrativo — Ink, StoryManager, diálogo, retratos, prólogo |
| [**3**](#capítulo-3-sistema-de-interacción) | Sistema de Interacción — PlayerInteraction, InteractableObject, triggers |
| [**4**](#capítulo-4-escenas-y-niveles) | Escenas y Niveles — LevelManager, SpawnPoint, transiciones |
| [**5**](#capítulo-5-sistema-de-ui) | Sistema de UI — Menú, carrusel, ajustes, HUD |
| [**6**](#capítulo-6-sistema-de-persistencia) | Sistema de Persistencia — SaveSystem, GameStateSO, PlayerPrefs |
| [**7**](#capítulo-7-guía-de-creación-de-contenido) | Guía de Creación de Contenido — Pipelines, checklists, riesgos |
| [**8**](#capítulo-8-convenciones-técnicas-y-reglas-arquitectónicas) | Convenciones Técnicas — Código, Ink, buenas prácticas, deuda técnica |

> **Documentación externa:** [`docs/README.md`](docs/README.md) — tabla de sistemas, roadmap.

---

## Capítulo 1: Arquitectura General

Patrón **Singleton-based centralizado con ScriptableObjects como datos**. Managers persistentes (`DontDestroyOnLoad`) en `Game Manager.prefab`. Comunicación: referencias directas a singleton, eventos C# nativos, funciones externas vinculadas desde Ink.

```mermaid
graph TB
    subgraph Persistentes["Managers (DontDestroyOnLoad)"]
        GM[GameManager] --> GSO[GameStateSO]
        GM --> SS[SaveSystem]
        LM[LevelManager]
        SM[StoryManager]
        AM[AudioManager]
        PM[PrologueManager]
    end
    subgraph Escena["Por escena"]
        PLM[PlayerMovement] --> SM
        PIN[PlayerInteraction] --> SM
        IObj[InteractableObject] --> SM & GM
        AObj[AdvancedInteractableObject] --> SM & GM
        STT[SceneTransitionTrigger] --> SM & LM
        DUC[DialogueUIController]
        DTP[DialogueTagProcessor] --> GM & LM
    end
    subgraph Menu
        MI[MenuInicio] --> GM
        CN[CarruselNiveles] --> GM
        UIA[UIAjustes] --> AM
    end
    SM --> DUC --> DTP
    linkStyle default stroke:#666;stroke-width:1px
```

| Manager | Dependencias |
|---------|-------------|
| `GameManager` | GameStateSO, SaveSystem |
| `LevelManager` | GameManager |
| `StoryManager` | GameManager, DialogueUIController, inkJSON |
| `AudioManager` | 4 AudioSource, PlayerPrefs |
| `PrologueManager` | GameManager, StoryManager |

**Ciclo de inicio:**
1. GameManager.Awake() → SaveSystem.Load() → ¿clearStateOnStart? → GameStateSO.LoadFrom(data) o ClearState()
2. AudioManager.Awake() → LoadVolumes() desde PlayerPrefs
3. StoryManager.Awake() → InitializeStory() + BindExternalFunction(GetFlag, GetVar)
4. LevelManager.Awake() → Configura fade canvas (apagado)
5. PrologueManager.Awake() → DontDestroyOnLoad
6. GameManager.UpdateUIVisibility("Menu")

**Ciclo de vida al cargar escena:** SceneManager.sceneLoaded dispara en paralelo:
- GameManager: UpdateUIVisibility() (HUD on/off según escena)
- StoryManager: RefreshUIReferences()
- LevelManager: ResetFade() (si no está en transición)
- PrologueManager: HandleSceneRoutine() (evalúa flags del prólogo)

**Comunicación entre sistemas:**
1. **Eventos C# nativos** — DialogueTagProcessor → DialogueUIController, OnDialogueStateChanged (GameSummaryManager, AdvancedInteractableObject), OnInteractableChanged (InteractionIndicatorConnector)
2. **Referencias directas a singleton** — Patrón dominante (~10-13 clases por manager)
3. **Funciones externas de Ink** — StoryManager vincula GetFlag/GetVar al GameManager
4. **Tags Ink como comandos** — #scene:, #setflag:, #sonido: → DialogueTagProcessor → manager destino

**Dependencias fuertes:**
- GameManager: referenciado por ~13 clases (God Object incipiente)
- StoryManager: referenciado por ~10 clases, evento OnDialogueStateChanged con 3 suscriptores
- PlayerMovement: acoplado a StoryManager.IsDialogueActive para bloquear movimiento
- Transiciones: todas pasan por LevelManager.ChangeScene() que orquesta fade + auto-save + carga + spawn

---

## Capítulo 2: Sistema Narrativo

Arquitectura en 4 capas: Ink (.ink → .json) → StoryManager (motor Unity) → DialogueUIController (UI) → Trigger Narrativo (gameplay).

### Archivos Ink

| Archivo | Líneas | Knots | Propósito |
|---------|--------|-------|-----------|
| `Historia.ink` | 262 | ~20 | Entry point, INCLUDEs, prólogo, finales |
| `Prologo.ink` | 84 | 9 | Prólogo jugable |
| `Objects.ink` | 310 | ~45 | Interacciones con objetos, NPCs, patos |
| `Joseph_Arcade.ink` | 307 | ~15 | Ruta Arcade |
| `Joseph_Arcade_Camino2.ink` | 157 | ~8 | Segunda ruta Arcade |
| `Joseph_Bibloteca.ink` | 303 | ~15 | Ruta Biblioteca |
| `Joseph_Bibloteca_Camino2.ink` | 174 | ~8 | Segunda ruta Biblioteca |
| `Epilogos.ink` | 75 | 8 | Cartas de filósofos |

Todos incluidos vía `INCLUDE` desde `Historia.ink`, compilado a `Historia.json`.

### Tags de Ink

| Tag | Ejemplo | Acción |
|-----|---------|--------|
| `#sprite:` | `#sprite:sophia_happy` | Cambia retrato + identifica personaje |
| `#setflag:` | `#setflag:conocio_sophia` | Activa flag en GameState |
| `#deleteflag:` | `#deleteflag:exploracion` | Desactiva flag |
| `#setvar:` | `#setvar:ruta:Epilogo1` | Asigna variable narrativa |
| `#incrementvar:` | `#incrementvar:contador:1` | Incrementa variable numérica |
| `#sonido:` | `#sonido:joseph_suspiro` | Reproduce SFX |
| `#scene:` | `#scene:Biblioteca` | Cambia de escena |
| `#fade_out` | `#fade_out` | Fundido a negro al terminar diálogo |
| `#small_image:` | `#small_image:img1,img2` | Muestra imágenes (máx 3) |
| `#big_image:` | `#big_image:Control_Arcade` | Muestra imagen grande |

### StoryManager

`Assets/Scripts/Narrative/Narrative Logic/StoryManager.cs`. Puente entre Ink y Unity. Inicializa `Ink.Runtime.Story` desde `Historia.json`, vincula funciones externas, gestiona el ciclo del diálogo.

**API pública:**
| Método | Propósito |
|--------|-----------|
| `StartStory(knot)` | Elige path en Ink, inicia UI de diálogo |
| `AdvanceStory()` | Avanza a la siguiente línea |
| `EndStory()` | Finaliza diálogo, notifica a suscriptores |
| `IsDialogueActive` | Propiedad bool para consulta de estado |
| `OnDialogueStateChanged` | Evento Action(bool): true=inicio, false=fin |

**Flujo de StartStory(knot):** story.ChoosePathString(knot) → SetStory(story) en DialogueUIController → DisplayNextLine() → typewriter → jugador avanza → story.Continue() → loop hasta END.

### Ink External Functions (puente Ink ↔ C#)

Vinculadas en `SetExternalFunctionBindings()`. Se llaman desde Ink como funciones globales:

| Función Ink | Firme C# | Propósito |
|-------------|----------|-----------|
| `GetFlag(name)` | `GameManager.GetStoryFlag(string)` | Consulta flags desde Ink |
| `SetFlag(name)` | `GameManager.SetStoryFlag(string, bool)` | Setea flags desde Ink (alternativa a tag) |
| `GetVar(key)` | `GameManager.GetStoryVariable(string)` | Consulta variables de juego |
| `SetVar(key, val)` | `GameManager.SetStoryVariable(string, object)` | Setea variables de juego |
| `IncrementVar(key, increment)` | `GameManager.IncrementStoryVariable(string, int)` | Incrementa variables |
| `Not(pred)` | `!pred` en C# | Negación lógica (delegada) |
| `ArrayHasFlag(arrayName, flagName)` | `GameManager.StoryFlags.Contains(...)` | Verifica flag en array |

**Uso en Ink:**
```ink
=== puerta_bloqueada ===
{GetFlag("tiene_llave"):
  - #scene:Torreon
  - No tienes la llave.
  -> END
}
```

### Variables observadas (ObserveVariables)

StoryManager observa variables Ink y sincroniza a GameStateSO:
```csharp
story.ObserveVariable("ruta", (varName, newValue) => {
    GameManager.SetStoryVariable("ruta", newValue);
});
```
Variables observadas: `ruta` (elige epílogo).

### Cómo fluye un diálogo típico

1. **Inicio:** PlayerInteraction → InteractableObject.Interact() → StoryManager.StartStory("knot")
2. **Ink evalúa:** condiciones con `{GetFlag("...")}` y ramifica
3. **Presentación:** typewriter en DialogueUIController, decide si esperar clic o continuar
4. **Tags:** DialogueTagProcessor ejecuta `#setflag:`, `#sonido:`, etc.
5. **Opciones:** si hay `+ [Opción]`, se muestran como botones. El jugador elige → story.ChooseChoice()
6. **Repite** 3-5 hasta END → StoryManager.EndStory() → OnDialogueStateChanged(false)

### DialogueUIController

`DialogueUIController.cs`. Controla la interfaz del diálogo: texto typewriter, botones de opciones, retratos animados con blend shapes, voces por personaje, SFX e imágenes. Asigna `DialogueTagProcessor` para reaccionar a los tags de Ink.

### DialogueTagProcessor

`DialogueTagProcessor.cs`. Procesa tags de Ink y los convierte en acciones:
- `#setflag:` / `#deleteflag:` → GameManager.SetStoryFlag()
- `#setvar:` / `#incrementvar:` → GameManager.SetStoryVariable() / IncrementStoryVariable()
- `#scene:` → LevelManager.ChangeScene()
- `#sonido:` → DialogueUIController.HandleSoundRequested()
- `#sprite:` → DialogueUIController cambio de retrato
- `#small_image:` / `#big_image:` → DialogueUIController muestra imagen
- `#fade_out` → programa fade al terminar el diálogo

### PrologueManager

`Assets/Scripts/Core/PrologueManager.cs`. Orquesta prólogo a través de 3 escenas (Parque → Arcade/Biblioteca → Parque final). Evalúa flags y dispara knots al cargar escena.

| Flag | Cuándo se setea |
|------|-----------------|
| `prologue_arcade_visited` | Al llegar al Arcade |
| `prologue_arcade_item_collected` | Al recoger objeto en Arcade |
| `prologue_library_visited` | Al llegar a Biblioteca |
| `prologue_library_item_collected` | Al recoger objeto en Biblioteca |
| `prologue_completed` | Seteado por Ink al terminar |
| `prologue_final_seen` | Diálogo final visto |

### Triggers narrativos (cómo se inicia un diálogo)

| Trigger | Componente | Modo |
|---------|-----------|------|
| Objeto interactuable | `InteractableObject` + `InteractableData` SO | Al presionar botón |
| Objeto condicional | `AdvancedInteractableObject` + `AdvancedInteractableData` | Condiciones AND |
| Objeto de prólogo | `PrologueItemInteractable` | Al presionar botón, auto-desactiva |
| Puerta/transición | `SceneTransitionTrigger` | Trigger automático + opcional confirmación |
| Automático al cargar | `PrologueManager` | Evalúa flags en sceneLoaded |

### Persistencia narrativa

GameStateSO almacena `List<string> flags`, `List<StoryVariable> variables`. El auto-save ocurre en `LevelManager.ChangeScene()` antes de cargar la nueva escena.

### ScriptableObjects narrativos

| Asset | Tipo | Propósito |
|-------|------|-----------|
| `Estado.asset` | `GameStateSO` | Estado global del juego |
| `Images.asset` | `NarrativeImageDatabase` | Mapea IDs de imagen → Sprite |
| `Portrait Data Base.asset` | `CharacterPortraitData` | Mapea IDs de sprite → Animation State |
| `Cartas.asset` | `PhilosopherCardDatabase` | Datos de cartas de filósofos |
| `Patos DB.asset` | `CollectableDuckDatabase` | Datos de patos coleccionables |

### Mapa narrativo (árbol de decisión)

Arcade → Schopenhauer/Hegel. Biblioteca → Estoicos/Nietzsche. Cada ruta tiene 2 caminos con 2 finales cada uno (aceptación/reproche) = 4 finales totales + 1 epílogo por filósofo. ~45 objetos interactuables, 4 NPCs, 6 patos coleccionables.

---

## Capítulo 3: Sistema de Interacción

Arquitectura en 4 capas: Collider Trigger → PlayerInteraction (IInteractable) → Implementaciones → Datos+Narrativa.

### IInteractable

Interfaz raíz en `Assets/Scripts/Narrative/Interactions/Logic/IInteractable.cs`. Métodos: `void Interact()`, `string GetInteractionName()`.

### PlayerInteraction

`Assets/Scripts/Player data/PlayerInteraction.cs`. Singleton en Player.prefab. Detecta IInteractable via OnTriggerEnter/Exit. Enlaza input (Tecla E / Gamepad South). Si hay diálogo activo, llama AdvanceStory() en lugar de Interact(). Expone evento `OnInteractableChanged(Action<IInteractable>)`.

### Implementaciones

| Clase | Archivo | SO de datos | Propósito |
|-------|---------|-------------|-----------|
| `InteractableObject` | `Interactions/SO/SO Logic/` | `InteractableData` | Objeto genérico (narrativo o coleccionable) |
| `AdvancedInteractableObject` | `Interactions/SO/Joseph/` | `AdvancedInteractableData` | Objeto condicional con lista de InteractionEntry |
| `PrologueItemInteractable` | `Interactions/Logic/` | Ninguno (campos directos) | Objeto de prólogo autocontenido |
| `SceneTransitionTrigger` | `Interactions/Logic/` | Ninguno | Transición de escenas (NO implementa IInteractable) |
| `InteractableTable` | `Interactions/Logic/` | Ninguno | Cartas de filósofos (epílogo) |
| `FinalReflectionInteractable` | `Narrative Logic/` | `PhilosopherCardDatabase` | Reflexión final (4 etapas) |

### InteractableObject (objeto genérico)

Implementa IInteractable. Usa `InteractableData` SO. Dos modos:
- **Narrativo** (`isCollectable = false`): llama `StartStory(inkKnot)`. Si `requiredFlag` no se cumple, usa `fallbackKnot`.
- **Coleccionable** (`isCollectable = true`): al cargar escena, si `flagToSetOnCollect` ya existe → se desactiva (ya recogido). Al recoger: dispara Ink, setea flag, incrementa variable, guarda, reproduce sonido, se desactiva.

**Componentes requeridos:** Collider IsTrigger=true + InteractableObject script + InteractableData SO. Opcional: PulseAnimation hijo, autoTriggerOnEnter.

### AdvancedInteractableObject (objeto condicional)

Implementa IInteractable. Usa `AdvancedInteractableData` SO con lista de `InteractionEntry`. Evalúa condiciones en orden; la primera que cumple gana.
- **Visibility Conditions:** si fallan en Start(), el objeto se oculta.
- **Disappear After Dialogue:** se suscribe a OnDialogueStateChanged. Al terminar, si la condición se cumple → fade-out → desactiva → fade-in.
- **NPC Rotation:** al interactuar, rota el NPC hacia el jugador.

### InteractableData SO

Creado desde `Create > Narrative > Interactable`. Campos: `interactionName`, `inkKnot`, `isCollectable`, `flagToSetOnCollect`, `variableToIncrementOnCollect`, `incrementAmount`.

### AdvancedInteractableData SO

Creado desde `Create > Narrative > Advanced Interactable`. Lista de `InteractionEntry` evaluadas en orden. Cada entrada: `inkKnot`, `conditions` (AND). Tipos: `Ninguna`, `RequerirFlag`, `RequerirVariable`.

### SceneTransitionTrigger (puertas)

NO implementa IInteractable. Usa OnTriggerEnter directamente. Tres modos:
1. **Directo:** `confirmationKnot` vacío → `LevelManager.ChangeScene(destinationSceneName)`.
2. **Con flag requerido:** evalúa `requiredFlag` + pila de `TransitionCondition`. La primera que falle reproduce `fallbackKnot` + NaturalPushBackRoutine().
3. **Con confirmación:** dispara knot de Ink. El script Ink usa `#scene:NombreEscena` para la transición.

### PrologueItemInteractable (prólogo)

Autocontenido (no requiere SO). En Start(): si prólogo completado u objeto ya recogido → se desactiva. Al recoger: dispara knot, marca flag, guarda checkpoint, reproduce sonido, se desactiva. Tiene su propio sistema de pulso (anterior a PulseAnimation).

### Feedback Visual

| Componente | Propósito |
|-----------|-----------|
| `InteractionIndicator` | Billboard world-space sobre objeto interactuable |
| `InteractionIndicatorConnector` | Bridge: PlayerInteraction.OnInteractableChanged → Indicator |
| `PulseAnimation` | Pulso de emisión en Renderer (extraído de PrologueItemInteractable) |
| `InteractUI` | Legacy toggle simple de icono (coexiste con Indicator) |

### Prefabs Base

| Prefab | Componentes de interacción |
|--------|---------------------------|
| `Player.prefab` | PlayerInteraction, InteractUI |
| `Cambio de escena.prefab` | SceneTransitionTrigger |
| `IDEL JOSEPHIdle.prefab` | AdvancedInteractableObject |
| `JOSEPH.prefab` | AdvancedInteractableObject |
| `Gamepad_Classic.prefab` | InteractableObject |

### Dependencias

Todos los interactables dependen de `GameManager.Instance` (flags/variables), `StoryManager.Instance` (StartStory, IsDialogueActive) y opcionalmente `LevelManager.Instance` (ChangeScene).

### Inventario de assets (~65 SOs)

| Ubicación | Cantidad | Tipo |
|-----------|----------|------|
| `SO/Joseph/` | 3 | AdvancedInteractableData |
| `SO/NPCs/` | 4 | InteractableData |
| `SO/Escenarios/Arcade/` | 10 | InteractableData |
| `SO/Escenarios/Biblioteca/` | 9 | InteractableData |
| `SO/Escenarios/Cuarto/` | 4 | InteractableData |
| `SO/Escenarios/Parque/` | 11 | InteractableData |

---

## Capítulo 4: Escenas y Niveles

### Escenas registradas

| Índice | Escena | Propósito |
|--------|--------|-----------|
| 0 | `Menu.unity` | Menú principal, carrusel, ajustes, créditos |
| 1 | `Arcade.unity` | Ruta Schopenhauer/Hegel |
| 2 | `Biblioteca.unity` | Ruta Estoicos/Nietzsche |
| 3 | `Cuarto.unity` | Reflexión final, epílogo |
| 4 | `Parque.unity` | Prólogo, encuentro social |

**Flujo:** Menu → Parque → (Arcade ↔ Parque ↔ Biblioteca) → Cuarto → Menu.

### LevelManager

Singleton persistente en `Game Manager.prefab`. API:
- `ChangeScene(sceneName)` — fade out → auto-save → carga asíncrona → HandlePlayerSpawn() → fade in
- `FadeToBlack()` / `FadeToClear()` — fundidos inmediatos
- `FadeToBlackRoutine()` / `FadeToClearRoutine()` — corrutinas para uso externo

**Flujo ChangeScene:** Cerrar ajustes → Play transitionSFX → FadeOut → actualizar previousSceneName → SaveGame() → SceneManager.LoadSceneAsync(destino) → HandlePlayerSpawn() → FadeIn.

### SpawnPoints

Cada escena contiene SpawnPoints con `fromSceneName`. PlayerManager.HandlePlayerSpawn() lee previousSceneName del GameStateSO, busca coincidencia (case-insensitive), desactiva CharacterController, teletransporta, reactiva.

### Fuentes de transición

| Origen | Mecanismo |
|--------|-----------|
| CarruselNiveles | `GameManager.RequestLoadLevel("Parque")` |
| SceneTransitionTrigger | `LevelManager.ChangeScene(destino)` |
| Botón "Salir al menú" | `LevelManager.ChangeScene("Menu")` |
| Tag `#scene:nombre` en Ink | `DialogueTagProcessor` → `LevelManager.ChangeScene()` |
| GameSummaryUI | `LevelManager.ChangeScene("Menu")` |

### SceneTransitionTrigger

Tres modos: (1) **Directo** — `ChangeScene()` inmediato. (2) **Con flag requerido** — evalúa requiredFlag + pila de condiciones, muestra fallback si no cumple. (3) **Con confirmación** — dispara knot de Ink que usa `#scene:` para la transición.

### SceneMusicStarter

Cada escena debe tener un GameObject con este componente. Reproduce música y ambiente al iniciar la escena.

### Flujo del prólogo entre escenas

El prólogo es una secuencia guiada pero jugable a través de 3 escenas:

1. **Parque (inicio):** `prologo_parque_inicio` se dispara al cargar.
2. **Arcade o Biblioteca:** el jugador elige puerta → `prologue_arcade_visited` / `prologue_library_visited` → recoge objeto → `prologue_arcade_item_collected` / `prologue_library_item_collected`.
3. **Parque (final):** al volver, Joseph aparece → `Joseph1_Prologo_Reencuentro` → `Joseph2_Prologo` → `Decision_de_Camino` → `prologue_completed`.

**Transiciones:** todas usan SceneTransitionTrigger con confirmationKnot o cambio directo. PrologueManager.HandleSceneRoutine() evalúa flags en cada sceneLoaded y dispara el knot correspondiente.

### Dependencias por escena

| Escena | Managers | Componentes de escena |
|--------|----------|----------------------|
| Menu | GameManager, AudioManager | MenuInicio, CarruselNiveles, UIAjustes |
| Parque | Todos | Player, SceneTransitionTrigger(xN), SceneMusicStarter, SpawnPoint(xN) |
| Arcade | Todos | Player, SceneTransitionTrigger, SpawnPoint(x2), AdvancedInteractableObject (Joseph) |
| Biblioteca | Todos | Player, SceneTransitionTrigger, SpawnPoint(x2), AdvancedInteractableObject (Joseph) |
| Cuarto | Todos | Player, SceneMusicStarter, SpawnPoint, InteractableTable(x2), FinalReflectionInteractable |

---

## Capítulo 5: Sistema de UI

Tres subsistemas independientes: **Menu UI** (menú principal), **Dialogue UI** (diálogos y cartas, ver Cap. 2), **Interaction UI** (indicador de interacción, ver Cap. 3). No hay UIManager centralizado.

### Canvases

| Canvas | Render Mode | Orden | Propósito |
|--------|-------------|-------|-----------|
| Menu Canvas | ScreenSpace Overlay | 0 | Paneles del menú |
| Settings Canvas | ScreenSpace Overlay | 1 | Ajustes |
| Dialogue Canvas | ScreenSpace Overlay | 2 | Diálogos, opciones, retratos |
| Card Canvas | ScreenSpace Overlay | 3 | Cartas de filósofos |
| Interaction Canvas | WorldSpace | N/A | Billboard de interacción |
| Fade Canvas | ScreenSpace Overlay | 10 | Fundido a negro |
| Summary Canvas | ScreenSpace Overlay | 4 | Resumen final |

### CardPanelController (Cartas de filósofos)

`CardPanelController.cs` en `Narrative/Narrative Logic/`. Gestiona la visualización de cartas coleccionables en el epílogo. Recibe `PhilosopherCardData` desde `PhilosopherCardDatabase` y las muestra en el Card Canvas.

**Flujo de colección:**
1. Jugador encuentra carta en `InteractableTable` (mesa del Cuarto)
2. `InteractableTable` dispara Ink con datos de la carta
3. Ink setea flag via `#setflag:carta_filosofo_X`
4. Al activar `FinalReflectionInteractable`, se muestran 4 etapas de reflexión
5. Cada etapa es un knot de Ink + una carta visual en pantalla

**PhilosopherCardDatabase SO:** contiene lista de `PhilosopherCardData` con: ID, nombre, descripción, sprite frontal/trasero, flag asociado.

**CollectableDuckDatabase SO:** similar pero para los 6 patos coleccionables escondidos en las escenas.

### GameManager y HUD

GameManager controla visibilidad de `hudObjects[]`. Tecla Q abre/cierra panel de ajustes (pausa Time.timeScale).

### Menú Principal

`MenuInicio.cs`: Jugar → CarruselNiveles, Créditos, Ajustes, Salir. `CarruselNiveles.cs`: navegación A/D, muestra hasta 4 estrellas por final desbloqueado vía PlayerPrefs. `UIAjustes.cs`: 5 sliders (Música, SFX, UI, Ambiente, Master) vinculados a AudioManager.

### Clases de UI

| Clase | Archivo | Responsabilidad |
|-------|---------|-----------------|
| `MenuInicio` | `Menu/MenuInicio.cs` | Navegación del menú principal |
| `CarruselNiveles` | `Menu/CarruselNiveles.cs` | Selector de nivel con estrellas |
| `UIAjustes` | `Menu/UIAjustes.cs` | Panel de configuración de volumen |
| `BotonSalirMenu` | `Menu/BotonSalirMenu.cs` | Auto-save + ChangeScene("Menu") |
| `TutorialCarrusel` | `Menu/TutorialCarrusel.cs` | Tutorial interactivo |
| `GameSummaryUI` | `Core/GameSummaryUI.cs` | Pantalla final del juego |
| `GameSummaryManager` | `Core/GameSummaryManager.cs` | Lógica del resumen final |

---

## Capítulo 6: Sistema de Persistencia

### Arquitectura

```
GameManager (API pública: SaveGame, LoadGame, SetStoryFlag, GetStoryFlag, etc.)
  → GameStateSO (estado en runtime: flags, variables, escena, posición)
  → GameSaveData (POCO serializable a JSON)
  → SaveSystem (static: I/O en disco o PlayerPrefs en WebGL)
```

### Flujo Save/Load

```
SaveGame(): GameStateSO → GameSaveData → SaveSystem.Save(data) → save.json en persistentDataPath
LoadGame(): SaveSystem.Load() → GameSaveData → GameStateSO.LoadFrom(data)
RequestLoadLevel(): ¿Save existe? → Sí: ¿currentSceneName válido? → ChangeScene(guardada). No: ResetGameState → ChangeScene(base).
```

### Cuándo se guarda

- **Auto-save:** en cada `LevelManager.ChangeScene()`, antes de cargar la nueva escena.
- **Coleccionables:** tras recoger objeto (en InteractableObject y PrologueItemInteractable).
- **Salir al menú:** BotonSalirMenu llama SaveGame() antes de ChangeScene("Menu").
- **Ink:** puede disparar guardado indirectamente al setear flags que activan otros sistemas.

### GameStateSO

`Assets/Scripts/Player data/GameStateSO.cs`. ScriptableObject creado desde `Core/Game State`. Almacena: `currentSceneName`, `previousSceneName`, `playerPosition`, `playerRotation`, `flags` (List<string>), `variables` (List<StoryVariable> clave-valor).

### SaveSystem

Clase estática en `Assets/Scripts/Core/SaveSystem.cs`. Solo GameManager lo invoca. Escribe/lee `save.json` en `Application.persistentDataPath` (o PlayerPrefs en WebGL).

### Datos persistidos

| Dato | Origen | Destino |
|------|--------|---------|
| Flags narrativos | GameStateSO | save.json |
| Variables narrativas | GameStateSO | save.json |
| Escena actual | GameStateSO | save.json |
| Posición del jugador | GameStateSO | save.json |
| Volúmenes de audio | AudioManager | PlayerPrefs |
| Finales desbloqueados | GameSummaryManager | PlayerPrefs |

### PlayerPrefs Keys

`PlayerPrefsKeys.cs` — constantes centralizadas. Volúmenes: `MusicVolume`, `SFXVolume`, `UIVolume`, `AmbientVolume`, `MasterVolume`. Finales: `EndingUnlocked_Schopenhauer`, `EndingUnlocked_Hegel`, `EndingUnlocked_Estoicos`, `EndingUnlocked_Nietzsche`.

### Eventos de guardado

`OnDialogueStateChanged(false)` → GameSummaryManager evalúa si mostrar resumen; AdvancedInteractableObject evalúa si desaparecer.

---

## Capítulo 7: Guía de Creación de Contenido

### 7.1 Visión General

Este capítulo unifica pipelines, checklists y riesgos de los capítulos anteriores. Es la referencia única para creación de nuevo contenido.

### 7.2 Cómo crear un nuevo objeto interactuable

1. Crear `InteractableData` SO (Assets/Create/Narrative/Interactable) con `interactionName`, `inkKnot`
2. GameObject con Collider (IsTrigger=true) + `InteractableObject` component
3. Arrastrar SO a `data`
4. Opcional: `requiredFlag`, `fallbackKnot`, `autoTriggerOnEnter`

**Coleccionable:** marcar `isCollectable`, configurar `flagToSetOnCollect`. Se desactiva automáticamente si el flag ya existe al cargar.

### 7.3 Cómo crear una secuencia interactiva (puzzle)

1. Crear `AdvancedInteractableData` SO (Assets/Create/Narrative/Advanced Interactable)
2. Agregar `InteractionEntry` por cada knot posible con condiciones AND
3. GameObject con Collider + `AdvancedInteractableObject`
4. Opcional: `visibilityConditions` (ocultar si no se cumplen), `disappearAfterDialogue`

### 7.4 Cómo crear una nueva interacción narrativa

1. Escribir knot en archivo .ink (o crear nuevo archivo + INCLUDE en Historia.ink). Ejemplo:
   ```ink
   === mi_nuevo_knot ===
   #sprite:sophia_neutral
   #setflag:exploracion_iniciada
   Sophia: ¿Has visto esto? Es fascinante.
   + [Sí, cuéntame más]
     #sonido:ui_click
     -> mas_detalle_knot
   + [No me interesa]
     -> END
   ```
2. Usar tags `#sprite:`, `#setflag:`, `#sonido:`, etc. según necesidad:
   - `#sprite:` con ID existente en CharacterPortraitData
   - `#setflag:` en snake_case (ej: `exploracion_iniciada`)
   - `#sonido:` con ID existente en DialogueUIController.dialogueSounds
3. Compilar .json desde Inky (Ctrl+S genera automáticamente)
4. En Unity, asignar el knot desde el Inspector del interactuable correspondiente
5. Verificar en consola: `[StoryManager] StartStory llamado con knot: 'mi_nuevo_knot'`
6. Probar: acercarse al interactuable → presionar E → verificar que el diálogo se muestra correctamente

### 7.5 Cómo crear un nuevo nivel

1. Crear escena en `Assets/Scenes/`
2. Registrar en Build Settings (verificar índice)
3. Agregar `SceneMusicStarter`, `SpawnPoint`s, Player prefab
4. Configurar `SceneTransitionTrigger` para entrada/salida
5. Si es parte del prólogo: agregar handler en `PrologueManager.HandleSceneRoutine()`
6. Agregar interactuables según la narrativa

### 7.6 Cómo crear un interactuable condicional

1. Crear `AdvancedInteractableData` SO
2. Agregar `InteractionEntry` por cada knot, en orden de prioridad
3. Configurar condiciones: `RequerirFlag` o `RequerirVariable`
4. Asignar SO al `AdvancedInteractableObject` en escena
5. Probar cada rama condicional

### 7.7 Cómo reutilizar prefabs

- Prefab base = configuración mínima funcional (Collider trigger + script)
- Variantes: arrastrar a escena y asignar SO específico, o crear Prefab Variant
- Para Joseph/NPC condicional: usar prefab existente, cambiar `AdvancedInteractableData` SO

### 7.8 Cómo registrar un nuevo sistema (manager)

1. Crear MonoBehaviour con patrón singleton (`Instance + DontDestroyOnLoad`)
2. Agregar al `Game Manager.prefab`
3. Si necesita acceso global: exponer API pública y eventos C# si aplica
4. Si necesita reaccionar a cambios de escena: suscribirse a `SceneManager.sceneLoaded`
5. Si necesita reaccionar al diálogo: suscribirse a `StoryManager.OnDialogueStateChanged`

### 7.9 Cómo conectar gameplay y narrativa

**Unity → Ink:** `StoryManager.Instance.StartStory("knot_name")` desde cualquier IInteractable. El knot debe existir en archivo .ink compilado. Ejemplo de implementación:
```csharp
// En InteractableObject.Interact():
StoryManager.Instance.StartStory(data.inkKnot);
```

**Ink → Unity:** Tags procesados por `DialogueTagProcessor`:
```
#setflag:mision_completada   → GameManager.SetStoryFlag("mision_completada", true)
#setvar:ruta:Epilogo1       → GameManager.SetStoryVariable("ruta", "Epilogo1")
#scene:Biblioteca            → LevelManager.ChangeScene("Biblioteca")
```

**Transición desde Ink:** El `SceneTransitionTrigger` con `confirmationKnot` dispara un diálogo. El script Ink usa `#scene:NombreEscena` para completar la transición:
```ink
=== confirmar_salida ===
¿Estás seguro de que quieres ir a la biblioteca?
+ [Sí, vamos]
  #scene:Biblioteca
+ [Mejor no]
  -> END
```

**Transición condicional por narrativa:** SceneTransitionTrigger.requiredFlag = "mision_completada" con fallbackKnot = "aun_no_puedes_pasar". El flag se setea desde Ink mediante `#setflag:mision_completada`. Al cumplir la misión, el flag se activa y la puerta se desbloquea.

### 7.10 Checklist General de Creación de Contenido

#### Pre-creación
- [ ] El knot tiene nombre único en todo el proyecto
- [ ] El archivo .ink está INCLUDE en `Historia.ink`
- [ ] El .json fue recompilado desde Inky
- [ ] Tags con sintaxis correcta (`#tag:valor`, snake_case para flags)
- [ ] `#sprite:` existe en `CharacterPortraitData`
- [ ] `#sonido:` existe en `DialogueUIController.dialogueSounds`
- [ ] `#small_image:` / `#big_image:` existe en `NarrativeImageDatabase`

#### Implementación
- [ ] Collider con IsTrigger=true en el GameObject
- [ ] InteractableObject/AdvancedInteractableObject con SO asignado
- [ ] `inkKnot` coincide exactamente con knot en Ink
- [ ] Si usa `requiredFlag`: el flag se setea en el momento correcto
- [ ] Si usa condiciones: no hay conflictos ni bucles infinitos
- [ ] `flagToSetOnCollect` único en todo el proyecto (coleccionables)
- [ ] `destinationSceneName` coincide con Build Settings (transiciones)

#### Persistencia
- [ ] Flags/variables se persisten al cambiar de escena
- [ ] Objetos ya recogidos no aparecen al recargar escena
- [ ] auto-save se llama antes de transicionar

#### Integración
- [ ] El diálogo inicia correctamente al interactuar
- [ ] Opciones llevan al knot correcto
- [ ] El prólogo fluye correctamente entre escenas
- [ ] Al terminar el diálogo, el panel se oculta y el personaje puede moverse
- [ ] Los managers persistentes no se duplican al recargar Menu

### 7.11 Riesgos Frecuentes

| # | Riesgo | Mitigación |
|---|--------|------------|
| 1 | **Knot mal escrito en C# o SO** | Usar constantes de strings. Verificar coincidencia exacta |
| 2 | **Tag malformado (#setflag:flag name)** | Los espacios internos no se corrigen. Usar snake_case |
| 3 | **INCLUDE faltante** | Verificar que el .ink está en Historia.ink |
| 4 | **No recompilar .json** | Configurar auto-compilar en Inky |
| 5 | **GameState no persiste** | Verificar que transiciones pasan por LevelManager.ChangeScene() |
| 6 | **Diálogo bloqueado** | Verificar `!IsDialogueActive` antes de StartStory |
| 7 | **Collider mal posicionado** | Usar Gizmos en el Editor |
| 8 | **Condiciones en conflicto (Advanced)** | Revisar orden de entradas. Usar condiciones excluyentes |
| 9 | **confirmationKnot sin tag #scene:** | El script Ink debe incluir `#scene:NombreEscena` |
| 10 | **SpawnPoint sin fromSceneName** | Cada escena debe tener SpawnPoint para cada origen |
| 11 | **Fade atascado en negro** | LevelManager.OnSceneLoaded resetea el fade |
| 12 | **Managers duplicados** | Usan DontDestroyOnLoad + singleton check en Awake |

### 7.12 Cómo añadir un coleccionable (carta de filósofo o pato)

1. **PhilosopherCardDatabase / CollectableDuckDatabase:** Agregar entrada con ID único, nombre, sprites y flag asociado
2. **Ink:** Crear knot para la recogida (ej: `carta_filosofo_zenon`). Tag `#setflag:carta_zenon_obtenida`
3. **Escena:** Ubicar `InteractableTable` (para cartas) o crear nuevo interactuable (para patos)
4. **Probar:** Recoger objeto → flag se setea → carta aparece en el epílogo

### 7.13 Cómo funciona el epílogo

1. Al completar una ruta narrativa, `ruta` se setea (ej: `Epilogo1` = Schopenhauer aceptación)
2. `LevelManager.ChangeScene("Cuarto")` lleva al cuarto de reflexión
3. En el cuarto: `InteractableTable` muestra cartas de filósofos disponibles (según flags de colección)
4. `FinalReflectionInteractable` con 4 interacciones narrativas → cada etapa avanza la reflexión
5. Al completar la 4ª etapa → `GameSummaryManager` recopila datos → `GameSummaryUI` muestra resumen con estrellas
6. **Estrellas:** cada final desbloqueado incrementa un contador en PlayerPrefs (visible en CarruselNiveles)

### 7.14 Referencia rápida de archivos

| Archivo | Ruta |
|---------|------|
| `GameManager.cs` | `Assets/Scripts/Core/` |
| `StoryManager.cs` | `Assets/Scripts/Narrative/Narrative Logic/` |
| `DialogueUIController.cs` | `Assets/Scripts/Narrative/Narrative Logic/` |
| `DialogueTagProcessor.cs` | `Assets/Scripts/Narrative/Narrative Logic/` |
| `LevelManager.cs` | `Assets/Scripts/Menu/` |
| `SaveSystem.cs` | `Assets/Scripts/Core/` |
| `GameStateSO.cs` | `Assets/Scripts/Player data/` |
| `PlayerInteraction.cs` | `Assets/Scripts/Player data/` |
| `InteractableObject.cs` | `Assets/Scripts/Narrative/Interactions/SO/SO Logic/` |
| `AdvancedInteractableObject.cs` | `Assets/Scripts/Narrative/Interactions/SO/Joseph/` |
| `PrologueManager.cs` | `Assets/Scripts/Core/` |
| `PrologueItemInteractable.cs` | `Assets/Scripts/Narrative/Interactions/Logic/` |
| `SceneTransitionTrigger.cs` | `Assets/Scripts/Narrative/Interactions/Logic/` |
| `AudioManager.cs` | `Assets/Scripts/Audio/` |
| `SceneMusicStarter.cs` | `Assets/Scripts/Audio/` |
| `MenuInicio.cs` | `Assets/Scripts/Menu/` |
| `CarruselNiveles.cs` | `Assets/Scripts/Menu/` |
| `GameManager.prefab` | `Assets/Prefabs/` |
| `Player.prefab` | `Assets/Prefabs/` |
| `Historia.json` | `Assets/Scripts/Narrative/Ink/` |

---

## Capítulo 8: Convenciones Técnicas y Reglas Arquitectónicas

### 8.1 Convenciones de Código

- **Lenguaje:** C# en Unity, scripts Ink para narrativa
- **Nombres:** PascalCase para clases/métodos públicos, camelCase para params/vars privadas, `_camelCase` para campos serializados
- **Scripts:** un archivo por clase, nombre = clase
- **Namespaces:** no se usan (no hay colisiones significativas)
- **Eventos:** `Action<T>` para eventos simples, `event EventHandler<T>` para eventos estándar. Sufijo `Changed` para notificaciones de estado
- **Archivos Ink:** archivo raíz `Historia.ink` con INCLUDEs. Un archivo por ruta narrativa. snake_case para nombres de knot
- **Tags Ink:** `#tag:valor` sin espacios. `#setflag:` usa snake_case
- **SO:** usar `CreateAssetMenu` con rutas tipo `Narrative/Interactable`

### 8.2 Estructura de Carpetas

```
Assets/
  Scripts/
    Core/               — GameManager, SaveSystem, GameStateSO, GameSummaryManager, PrologueManager
                          GameSaveData, PlayerPrefsKeys, GameSummaryUI, FinalRoomManager
    Player data/        — PlayerManager, PlayerMovement, PlayerInteraction, GameStateSO
                          (GameStateSO también está aquí, no en Core)
    Audio/              — AudioManager, AudioEvent, SceneMusicStarter
    Menu/               — LevelManager, MenuInicio, CarruselNiveles, UIAjustes, BotonSalirMenu,
                          MenuBotones, TutorialCarrusel
    Narrative/
      Narrative Logic/  — StoryManager, DialogueUIController, DialogueTagProcessor,
                          CardPanelController, FinalReflectionInteractable,
                          PhilosopherCardDatabase, CollectableDuckDatabase,
                          NarrativeImageDatabase, CharacterPortraitData
      Ink/              — Historia.ink (entry point), Historia.json (compilado),
                          *.ink (Prologo, Objects, Joseph_*, Epilogos)
      Interactions/
        Logic/          — IInteractable, InteractableData, AdvancedInteractableData,
                          SceneTransitionTrigger, PrologueItemInteractable,
                          InteractableTable, InteractUI, InteractionIndicator,
                          InteractionIndicatorConnector, PulseAnimation, PlayerControls
        SO/
          SO Logic/     — InteractableObject (lógica), NarrativeImageDatabase,
                          CharacterPortraitData, CollectableDuckDatabase
          Joseph/       — AdvancedInteractableObject
          Escenarios/   — SOs de datos por escena (Arcade, Biblioteca, Cuarto, Parque, NPCs)
  Prefabs/              — Game Manager.prefab, Player.prefab, Cambio de escena.prefab,
                          IDEL JOSEPHIdle.prefab, JOSEPH.prefab, Gamepad_Classic.prefab
  Scenes/               — Menu.unity, Parque.unity, Arcade.unity, Biblioteca.unity, Cuarto.unity
```

### 8.3 Reglas de Dependencias

- Solo `GameManager` invoca `SaveSystem`
- `PlayerMovement` no debe llamar a managers directamente (actualmente rompe esta regla)
- Todos los interactables se comunican solo via `IInteractable` y sus SO
- La UI de diálogo depende de `StoryManager`, no de `GameManager`
- `AudioManager` solo usa PlayerPrefs, no `GameStateSO`
- Nuevos managers: agregar a `Game Manager.prefab` + patrón singleton + DontDestroyOnLoad
- Nuevos interactables: implementar `IInteractable`, no heredar

### 8.4 Buenas Prácticas

1. **Usar SO, no datos hardcodeados** — permiten reutilizar lógica con diferentes datos
2. **Extraer PulseAnimation** a hijo del interactuable (desacoplado del script principal)
3. **Usar Prefab Variants** en lugar de modificar prefabs base directamente
4. **Constantes para flags y knots** — strings literales son frágiles
5. **Probar todas las ramas condicionales** — cada InteractionEntry debe probarse
6. **Eventos para comunicación entre sistemas** en lugar de referencias directas (futuro)
7. **Mantener interacciónName descriptivo** (se usa en logs de depuración)

### 8.5 Flujo Recomendado de Git

1. Crear rama desde `develop`: `feature/nueva-interaccion`
2. Implementar contenido Ink + configuración Unity
3. Probar en Unity Play mode
4. Commit: mensaje descriptivo con prefijo (`[Narrativa]`, `[Interacción]`, `[UI]`)
5. Push + PR a `develop` con descripción de qué se probó

### 8.6 Prácticas Seguras para Modificar Sistemas

1. **StoryManager:** modificar solo para cambiar lógica de BindExternalFunction o ciclo de diálogo. No tocar si solo se agrega contenido Ink
2. **GameManager:** punto más sensible. Cualquier cambio requiere probar: inicio de partida, continuación, cambio de escena, guardado/carga
3. **LevelManager:** cambios en transiciones afectan todas las escenas. Probar fade, spawn, auto-save
4. **AdvancedInteractableObject:** cambios en condiciones requieren verificar que no rompen la lógica de Joseph

### 8.7 Reglas para Nuevos Desarrolladores

1. Leer este manual (Cap. 1-4 para entender el sistema, Cap. 7 para crear contenido)
2. Para agregar narrativa: solo tocar archivos .ink (no código C#)
3. Para agregar objetos: seguir pipeline del §7.2
4. No modificar managers sin entender el flujo completo
5. Probar siempre: partida nueva → interactuar → cambiar escena → guardar → cargar
6. Verificar logs en consola ante cualquier comportamiento extraño

### 8.8 Deuda Técnica Identificada

| ID | Deuda | Impacto | Mitigación |
|----|-------|---------|------------|
| DT1 | PlayerMovement acoplado a StoryManager.IsDialogueActive | Medio | Migrar a evento OnDialogueStateChanged |
| DT2 | GameManager como God Object (~13 dependencias) | Alto | Introducir event channels vía SO |
| DT3 | StoryManager como segundo hub (~10 dependencias) | Alto | Event channels |
| DT4 | Lógica de spawn duplicada en LevelManager y PlayerManager | Bajo | Unificar en un solo servicio |
| DT5 | GameObject.Find() para localizar jugador | Bajo | Sistema de referencias por escena |
| DT6 | PrologueItemInteractable con su propio sistema de pulso (antes de extraer PulseAnimation) | Bajo | Ya migrado |
| DT7 | Falta de tests automatizados | Medio | Agregar tests de integración para flujos críticos |
| DT8 | No hay versionado de save.json | Medio | Agregar schema version |
| DT9 | InteractUI legacy coexistiendo con InteractionIndicator | Bajo | Deprecar InteractUI |
| DT10 | Nombres de archivos inconsistentes (espacios vs sin espacios) | Bajo | Estandarizar |

### 8.9 Áreas Frágiles

| Área | Riesgo | Por qué es frágil |
|------|--------|-------------------|
| `LevelManager.ChangeScene()` | Alto | Orquesta fade, auto-save, carga asíncrona y spawn. Cualquier error deja el juego en estado inconsistente |
| `StoryManager.StartStory()` | Alto | Depende de inkJSON compilado, BindExternalFunction, DialogueUIController y GameState |
| `AdvancedInteractableObject` condiciones | Medio | La evaluación en orden es correcta pero frágil si se agregan nuevas condiciones sin revisar las existentes |
| `GameManager.RequestLoadLevel()` | Alto | Decide entre partida nueva, continuación o reinicio. Error aquí = bucle de carga |
| `PrologueManager.HandleSceneRoutine()` | Medio | Depende de flags y nombres de escena hardcodeados. Nueva escena de prólogo = nuevo handler |

### 8.10 Mejoras Futuras Recomendadas

1. **Bus de eventos desacoplado** (ScriptableObject event channels) para reducir acoplamiento a GameManager/StoryManager
2. **Eliminar duplicación de spawn** entre LevelManager y PlayerManager
3. **Migrar GameObject.Find()** a SceneReferences por escena
4. **Migrar consultas directas a singleton** (PlayerMovement → StoryManager) a suscripciones a eventos
5. **Agregar tests de integración** para flujos críticos (inicio → interactuar → transición → guardar → cargar)
6. **Versionado de save** para evitar corrupción entre versiones

---

### Apéndice A: Solución de problemas comunes

| Síntoma | Causa probable | Solución |
|---------|---------------|----------|
| El diálogo no inicia al presionar E | IsDialogueActive es true (diálogo previo no cerró) | Llamar EndStory() forzado o reiniciar escena |
| El objeto interactuable no aparece | VisibilityCondition falló en Start() | Verificar condiciones en AdvancedInteractableData |
| La transición de escena no funciona | destinationSceneName mal escrito o no en Build Settings | Verificar nombre exacto y Build Settings index |
| Las opciones de Ink no aparecen | story.Continue() no devolvió choices | Verificar estructura Ink (debe tener `+ [Opción]`) |
| El auto-save no carga la escena correcta | sceneName en save corrupto | Borrar save manualmente en persistentDataPath |
| La carta de filósofo no aparece en epílogo | PhilosopherCardDatabase no tiene la entrada | Verificar ID en DB |
| FadeOut y no vuelve | LevelManager.OnSceneLoaded no se ejecutó | Verificar que ChangeScene carga escena correcta |
| Managers duplicados al volver al menú | DontDestroyOnLoad + escena Menu tiene su propio GM | Verificar que Menu no incluya GameManager |

### Apéndice B: Palabras reservadas y convenciones Ink

**Convenciones de nomenclatura:**
- `snake_case` para flags: `prologo_arcade_visited`, `carta_zenon_obtenida`
- `PascalCase` para knots: `Joseph1_Prologo_Reencuentro`, `Decision_de_Camino`
- Tags siempre en minúscula: `#setflag:`, `#sonido:`, `#scene:`

**Palabras reservadas Ink:** `END`, `DONE`, `->`, `===`, `+`, `{`, `}`, `~`, `VAR`, `CONST`, `LIST`, `INCLUDE`, `EXTERNAL`.

### Apéndice C: Comandos Unity útiles para debug

| Propósito | Menú / Atajo |
|-----------|-------------|
| Resetear flags/variables | GameManager Inspector → ClearStoryFlags |
| Ver estado de juego | GameStateSO Inspector (runtime) |
| Verificar knot en Ink | Play Mode → consola: `[StoryManager] StartStory llamado con knot: '...'` |
| Ver auto-save | `%APPDATA%/WhispersInLetters/save.json` |
| Buscar SO por escena | Tool: InteractableTable.SceneInteractablesByScene |

> **Fin del manual.** Documentación externa: [`docs/README.md`](docs/README.md) — tabla de sistemas y roadmap.
