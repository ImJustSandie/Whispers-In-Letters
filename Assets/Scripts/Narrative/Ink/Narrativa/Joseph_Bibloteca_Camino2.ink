=== Camino_2_Joseph_Biblioteca ===
#setflag:Camino_2_Joseph_Biblioteca

#sprite:joseph_sad
#sonido:joseph_suspiro
Es inútil, Sophia... por más que lo intente, estas palabras son solo ruido para mí.

#sprite:sophia_sad
Veo el esfuerzo en tus ojos… pero también cómo tu mente se bloquea cada vez que intentas avanzar.

#sprite:joseph_sad
No es falta de ganas... simplemente no tengo las herramientas para procesarlo.

#sprite:sophia_thinking
A veces insistir no es una virtud… sino una forma de lastimarte sin darte cuenta.

#sprite:joseph_sad
Mis compañeros ya terminaron… y yo sigo atrapado en la introducción.

#sprite:joseph_sad
Es como si todos estuvieran leyendo el mismo idioma… y yo estuviera mirando símbolos que no encajan en nada.

#sprite:sophia_neutral
Quizás no es que no puedas… sino que estás intentando entenderlo de la forma equivocada.

#sprite:sophia_thinking
No todas las cerraduras están hechas para la misma llave.

#sprite:joseph_sad
Es doloroso admitirlo… pero este es mi límite.

#sprite:joseph_sad
Y cada vez que lo intento más, solo me siento más atrás.

#sprite:sophia_neutral
Forzarlo más no te va a hacer avanzar… solo te va a romper.

#sprite:joseph_sad
Tienes razón… ya no sé si estoy aprendiendo o solo sobreviviendo a esto.

#sprite:joseph_sad
Voy a cerrar el libro.

#sprite:joseph_sad
#sonido:joseph_suspiro
Se siente extraño… como rendirme en silencio.

#sprite:sophia_neutral
No es rendirse.

#sprite:sophia_neutral
Es dejar de pelear una batalla que no está hecha para ganarse así.

#setflag:Obj_Biblio_3
#setflag:Obj_Activ_Biblioteca
#setvar:ruta:decision_biblioteca_3
#setflag:decision_biblioteca_3

#sprite:sophia_neutral
No tiene sentido seguir rompiéndote contra algo que no responde.
#sprite:joseph_sad
Aunque si realmente quiero ayudar a joseph deberia estudiar su tema.

-> END

=== HandleDecision_Biblioteca_3 ===
{
- GetVar("ruta") == "ultimo_esfuerzo": -> Camino_1_2_Joseph_Biblioteca
- GetVar("ruta") == "rendicion_final": -> Camino_2_2_Joseph_Biblioteca
- GetVar("ruta") == "decision_biblioteca_3":
    {
        - GetFlag("Activ_Biblio_3") == true: -> decision_biblioteca_3
        - else: -> decision_biblioteca_3_Gated
    }
}
=== decision_biblioteca_3 ===
#sprite:sophia_thinking
¿Qué debería hacer con Joseph?
+[Intentarlo otra vez]
#sprite:sophia_neutral
#setvar:ruta:ultimo_esfuerzo
No debería rendirme tan fácil
-> Camino_1_2_Joseph_Biblioteca
+[Aceptar la derrota]
#setvar:ruta:rendicion_final
#sprite:sophia_sad
Este es un caso perdido
-> Camino_2_2_Joseph_Biblioteca
+[Decidir después]
#sprite:sophia_thinking
#setvar:ruta:decision_biblioteca_3
Necesito pensarlo
-> END

=== decision_biblioteca_3_Gated ===
#sprite:sophia_thinking
Aún no estoy lista para decidir. Debería explorar un poco más antes.
+[Aceptar la derrota]
#setvar:ruta:rendicion_final
#sprite:sophia_sad
Este es un caso perdido
-> Camino_2_2_Joseph_Biblioteca
+[Decidir después]
#sprite:sophia_thinking
#setvar:ruta:decision_biblioteca_3
Necesito pensarlo
-> END

=== Camino_2_2_Joseph_Biblioteca ===
#setflag:Camino_2_2_Joseph_Biblioteca

#sprite:sophia_neutral
Joseph, detente. No vamos a leer ni una sola línea más.

#sprite:joseph_sad
#sonido:joseph_sorpresa
¿Te rindes conmigo? Pensé que eras la única que creía que podía lograrlo.

#sprite:sophia_thinking
Creer en lo imposible también puede ser un error.

#sprite:sophia_neutral
Seguir aquí no te está formando… te está desgastando.

#sprite:joseph_sad
Entonces... ¿soy un caso perdido?

#sprite:sophia_neutral
No. Pero este camino no es para ti. Y seguir insistiendo solo te va a vaciar más.

#sprite:joseph_sad
He invertido tanto tiempo… tantos intentos…
Dejarlo ahora se siente como confirmar que todos tenían razón sobre mí.

#sprite:sophia_thinking
O como dejar de demostrarles que no la tenían.

#sprite:joseph_sad
Estoy cansado… de estudiar sin entender, de avanzar sin sentir nada.

#sprite:joseph_sad
Es como golpear una pared que no se rompe… y empezar a romperme yo.

#sprite:sophia_neutral
Entonces deja de golpearla.

#sprite:sophia_thinking
Tu energía debería ir a un lugar donde no luches contra ti mismo.

#sprite:joseph_neutral
Se siente vacío… pero también más ligero.

#sprite:sophia_neutral
Eso no es derrota. Es espacio.

#sprite:joseph_sad
Nunca pensé que rendirme se sentiría así…

#setvar:ruta:Epilogo4
#setflag:Final_Alcanzado
#deleteflag:exploracion

#sprite:joseph_sad
Adiós a los libros… supongo que este nunca fue mi camino.
-> Sophia_Cansada

{ GetVar("ruta") == "Epilogo4": -> epilogo_nietzsche }

===Joseph_Final_4====
#setflag:Joseph_Final_4
#sprite:joseph_neutral
Gracias Sophia por ayudarme, 
Me siento mas relajado ahora que no tengo presiones
->Sophia_Cansada