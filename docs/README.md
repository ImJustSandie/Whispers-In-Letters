# Documentación Técnica — Whispers in Letters

> Repositorio principal: [`Manual Tecnico.md`](../Manual%20Tecnico.md) (~4500 líneas, 8 capítulos)

## Tabla de Sistemas

| Sistema | Capítulo | Archivos clave (C#) | SOs clave |
|---------|----------|---------------------|-----------|
| **Arquitectura General** | [Ch.1](../Manual%20Tecnico.md#capitulo-1-arquitectura-general) | `GameManager`, `StoryManager`, `LevelManager`, `AudioManager`, `PrologueManager`, `SaveSystem` | `GameStateSO` |
| **Sistema Narrativo** | [Ch.2](../Manual%20Tecnico.md#capitulo-2-sistema-narrativo) | `DialogueUIController`, `DialogueTagProcessor`, `CharacterPortraitAnimator` | `CharacterPortraitData`, `NarrativeImageDatabase`, `PhilosopherCardDatabase`, `CollectableDuckDatabase` |
| **Sistema de Interacción** | [Ch.3](../Manual%20Tecnico.md#capitulo-3-sistema-de-interaccion) | `PlayerInteraction`, `InteractableObject`, `AdvancedInteractableObject`, `PrologueItemInteractable`, `SceneTransitionTrigger` | `InteractableData`, `AdvancedInteractableData` |
| **Escenas y Niveles** | [Ch.4](../Manual%20Tecnico.md#capitulo-4-escenas-y-niveles) | `LevelManager`, `SpawnPoint`, `PrologueManager`, `SceneMusicStarter` | — |
| **Sistema de UI** | [Ch.5](../Manual%20Tecnico.md#capitulo-5-sistema-de-ui) | `MenuInicio`, `CarruselNiveles`, `UIAjustes`, `TutorialCarrusel`, `GameSummaryUI`, `BotonSalirMenu` | — |
| **Sistema de Persistencia** | [Ch.6](../Manual%20Tecnico.md#capitulo-6-sistema-de-persistencia) | `SaveSystem`, `GameStateSO`, `PlayerPrefsKeys` | `GameStateSO` |
| **Guía de Creación de Contenido** | [Ch.7](../Manual%20Tecnico.md#capitulo-7-guia-de-creacion-de-contenido) | — (prácticas y pipelines) | — |
| **Convenciones Técnicas** | [Ch.8](../Manual%20Tecnico.md#capitulo-8-convenciones-tecnicas-y-reglas-arquitectonicas) | — (estándares) | — |

## Roadmap de Contenido

| Hito | Estado | Dependencias |
|------|--------|-------------|
| Prólogo jugable (Parque → Arcade/Biblioteca → Parque) | ✅ Implementado | PrologueManager, PrologueItemInteractable, SceneTransitionTrigger |
| 5 escenas base | ✅ Implementado | LevelManager, SpawnPoint, SceneMusicStarter |
| Diálogos narrativos básicos | ✅ Implementado | Ink, StoryManager, InteractableObject |
| Objetos condicionales (Joseph) | ✅ Implementado | AdvancedInteractableObject |
| Cartas de filósofos (epílogo) | ✅ Implementado | InteractableTable, CardPanelController, FinalReflectionInteractable |
| Coleccionables (patos) | ✅ Implementado | InteractableObject (isCollectable), CollectableDuckDatabase |
| Guardado/Carga completo | ✅ Implementado | SaveSystem, GameStateSO |
| Menú principal + carrusel | ✅ Implementado | MenuInicio, CarruselNiveles |
| 4 finales (Schopenhauer, Hegel, Estoicos, Nietzsche) | ✅ Implementado | PlayerPrefsKeys, GameSummaryManager |
| Sistema de audio por escena | ✅ Implementado | SceneMusicStarter, AudioManager |

## Convenciones del Proyecto

Ver [Capítulo 8](../Manual%20Tecnico.md#capitulo-8-convenciones-tecnicas-y-reglas-arquitectonicas) para:
- Convenciones de código C# ([§8.2](../Manual%20Tecnico.md#-82-convenciones-de-codigo))
- Convenciones de Ink ([§8.3](../Manual%20Tecnico.md#-83-convenciones-de-ink))
- Convenciones de escenas y assets ([§8.4](../Manual%20Tecnico.md#-84-convenciones-de-escenas-y-assets))
- Buenas prácticas transversales ([§8.5](../Manual%20Tecnico.md#-85-buenas-practicas-transversales))

## Riesgos por Categoría

Ver [§7.11](../Manual%20Tecnico.md#-711-riesgos-frecuentes) para la tabla unificada de riesgos.

## Checklist Unificada

Ver [§7.10](../Manual%20Tecnico.md#-710-checklist-general-de-creacion-de-contenido) para la checklist de validación de contenido.

## Deuda Técnica y Áreas Frágiles

Ver [§8.6](../Manual%20Tecnico.md#-86-deuda-tecnica) (deuda técnica), [§8.7](../Manual%20Tecnico.md#-87-areas-frágiles) (áreas frágiles) y [§8.8](../Manual%20Tecnico.md#-88-areas-con-alto-acoplamiento) (alto acoplamiento).
