// ============================================================
// PRÓLOGO — Whispers in Letters
// Archivo: Prologo.ink
// Incluido desde Historia.ink
// Si vas a cambiar la historia no cambies el NPC_03
//
// KNOTS DISPONIBLES:
//   prologo_parque_inicio          → Sophia llega al parque (primera vez)
//   prologo_arcade_llegada         → Sophia llega al Arcade
//   prologo_arcade_recoger_objeto  → Sophia recoge el objeto del Arcade
//   prologo_arcade_falta_objeto    → Fallback: intenta salir sin recoger
//   prologo_biblioteca_llegada     → Sophia llega a la Biblioteca
//   prologo_biblioteca_recoger_obj → Sophia recoge el objeto de la Biblioteca
//   prologo_biblioteca_falta_objeto→ Fallback: intenta salir sin recoger
//   prologo_parque_final           → Joseph aparece, prólogo termina
// ============================================================

=== prologo_parque_inicio ===
#sprite:sophia_neutral
#sonido:sophia_suspira
Espera... creo que dejé el control en el Arcade.
#sprite:sophia_thinking
#sonido:sophia_sorpresa
#setflag:exploracion
Debería ir a buscarlo antes de que se haga tarde.
-> END

=== prologo_arcade_llegada ===
#sprite:sophia_neutral
El arcade, tambien llamado el templo del presente. 
Déjame revisar dónde lo dejé...
#big_image: Control_Arcade
Creo que ya lo vi...
-> END

=== prologo_arcade_recoger_objeto ===
#sprite:sophia_euforic
#sonido:sophia_risa
¡Aquí está! Menos mal.
#sprite:sophia_thinking
#sonido:sophia_sorpresa
Aunque... también recuerdo que dejé mi libro favorito en la biblioteca.
Creo que debería pasarme por allá antes de volver.
-> END

=== prologo_arcade_falta_objeto ===
#sprite:sophia_neutral
Todavía no he encontrado lo que vine a buscar.
Debería seguir mirando por aquí.
-> END

=== prologo_biblioteca_llegada ===
#sprite:sophia_neutral
La biblioteca. Siempre tan tranquila a esta hora.
#sprite:sophia_thinking
Vamos, tiene que estar por aquí en algún lado...
#big_image: Libro_Biblioteca_01
Creo que ya lo vi...
-> END

=== prologo_biblioteca_recoger_obj ===
#sprite: sophia_euforic
#sonido: sophia_risa
¡Lo encontré!
#sprite: sophia_neutral
#setflag:prologue_completed
#setvar:ruta:Inicio
Bien. Ya tengo todo lo que necesitaba.
Supongo que puedo volver al parque ahora.
-> END

=== prologo_biblioteca_falta_objeto ===
#sprite:sophia_neutral
Aún no encuentro lo que vine a buscar aquí.
Debería revisarlo bien antes de irme.
-> END

=== prologo_parque_final ===
#sprite:sophia_neutral
#sonido:sophia_sorpresa
... 
#sprite:sophia_thinking
Ese es Joseph.
#sprite:sophia_euforic
¡Joseph! Espera, voy a hablar con él.
-> END
