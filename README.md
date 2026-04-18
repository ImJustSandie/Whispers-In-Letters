# ✉️ Whispers in Letters (Susurros en Letras)

![Unity](https://img.shields.io/badge/Unity-2022.3+-black?style=for-the-badge&logo=unity)
![Ink](https://img.shields.io/badge/Narrative-Ink-orange?style=for-the-badge)
![License](https://img.shields.io/badge/License-MIT-blue?style=for-the-badge)

**Whispers in Letters** es una experiencia narrativa interactiva basada en el célebre libro *"El Mundo de Sofía"*. El objetivo principal del proyecto es transformar complejos **dilemas filosóficos** en situaciones cotidianas y comprensibles para estudiantes, permitiéndoles explorar las consecuencias de sus decisiones bajo la lupa de grandes pensadores.

---

## 🌟 Visión del Proyecto

La filosofía no tiene por qué ser abstracta o aburrida. En este juego, los estudiantes de colegios y universidades pueden enfrentarse a problemas reales a través de los ojos de los personajes, aplicando conceptos de:

- **Arthur Schopenhauer** (La voluntad y el pesimismo)
- **Friedrich Nietzsche** (El superhombre y la voluntad de poder)
- **Georg Wilhelm Friedrich Hegel** (La dialéctica y la historia)
- **Los Estoicos** (El control emocional y la virtud)

El juego busca que el aprendizaje sea **vivencial**, donde cada elección no solo cambia el diálogo, sino que altera el destino de los personajes y el epílogo de la historia.

---

## 🎭 Personajes y Ambientación

### Joseph
Es el eje central de las primeras narrativas. Un joven enfrentado a las presiones de la vida moderna, la vocación y la disciplina. A través de Joseph, el jugador explorará rutas que lo llevarán a la **Biblioteca** (el camino de la academia y la introspección) o al **Arcade** (el camino de la distracción y la gratificación inmediata).

### El Entorno
Desde parques tranquilos hasta bibliotecas imponentes, cada escenario está diseñado para evocar una atmósfera específica que refuerza el dilema en cuestión.

---

## 🧱 Arquitectura Modular

Uno de los pilares técnicos de **Whispers in Letters** es su **Estructura Modular**. Hemos diseñado el motor del juego para que sea altamente escalable:

- **Sistema de Niveles Desacoplado**: Gracias al `LevelManager` y al sistema de persistencia, añadir un nuevo dilema filosófico es tan sencillo como crear una nueva escena y un archivo Ink.
- **Narrativa Plug-and-Play**: El motor de Ink permite redactar historias complejas sin tocar una sola línea de código C#.
- **Persistencia Centralizada**: El `GameManager` orquesta todo el flujo, asegurando que las decisiones tomadas en el "Nivel 1" puedan repercutir en el final del juego de forma automática.

Esta arquitectura permite que el proyecto crezca orgánicamente, añadiendo más filósofos y más dilemas con un esfuerzo técnico mínimo.

---

## 🚀 Tecnologías Utilizadas

- **Unity**: Motor principal para el renderizado 3D y la lógica de juego.
- **Ink Messaging System**: Un robusto parser para manejar diálogos ramificados complejos.
- **ScriptableObjects**: Para una gestión de datos eficiente y sin dependencias pesadas.
- **JSON & Persistence**: Sistema de guardado personalizado para asegurar que el progreso educativo sea continuo.

---

## 📖 Cómo Empezar

1. Clona el repositorio.
2. Abre el proyecto en **Unity 2022.3** o superior.
3. Carga la escena `Menu` en `Assets/Scenes/`.
4. ¡Empieza a explorar el mundo de la filosofía!

---

*Desarrollado con ❤️ para transformar la educación a través de la interactividad.*
