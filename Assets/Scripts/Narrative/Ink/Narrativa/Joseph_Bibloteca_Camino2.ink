=== Camino_2_Joseph_Biblioteca ===
#setflag:Camino_2_Joseph_Biblioteca

#sprite:joseph_sad
Es inútil, Sophia... por más que lo intente, estas palabras son solo ruido para mí.

#sprite:sophia_sad
Veo el esfuerzo en tus ojos, pero también veo que tu mente se bloquea.

#sprite:joseph_sad
No es falta de ganas... simplemente no tengo las herramientas para procesar esto.

#sprite:sophia_thinking
A veces insistir no es virtud, sino una forma de tortura.

#sprite:joseph_sad
Mis compañeros ya terminaron... y yo sigo en la introducción.

#sprite:sophia_neutral
Quizás estamos forzando una cerradura que no tiene llave para ti.

#sprite:joseph_sad
Es doloroso admitirlo... pero este es mi límite.

#sprite:sophia_sad
Forzarlo más solo te va a romper.

#sprite:joseph_neutral
Tienes razón... voy a cerrar el libro.

#sprite:sophia_neutral
#setvar:ruta:decision_biblioteca_3
#setflag:decision_biblioteca_3
No tiene sentido pelear una batalla perdida.
-> END

=== HandleDecision_Biblioteca_3 ===
{
- GetVar("ruta") == "ultimo_esfuerzo": -> Camino_1_2_Joseph_Biblioteca
- GetVar("ruta") == "rendicion_final": -> Camino_2_2_Joseph_Biblioteca
- GetVar("ruta") == "decision_biblioteca_3": -> decision_biblioteca_3
}
=== decision_biblioteca_3 ===
#sprite:sophia_thinking
¿Qué debería hacer con Joseph?
+[Intentarlo otra vez]
#sprite:sophia_neutral
#setvar:ruta:ultimo_esfuerzo
No debería rendirmetan fácil
-> END
+[Aceptar la derrota]
#setvar:ruta:rendicion_final
#sprite:sophia_sad
Este es un caso perdido
-> END
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
¿Te rindes conmigo? Pensé que eras la única que creía que podía lograrlo.

#sprite:sophia_thinking
Creer en lo imposible también puede ser un error.

#sprite:sophia_neutral
Seguir aquí solo va a desgastarte.

#sprite:joseph_sad
Entonces... ¿soy un caso perdido?

#sprite:sophia_neutral
Para este camino, sí. Y seguir insistiendo sería perder más de tu vida.

#sprite:joseph_sad
Se siente como un vacío... pero también como si me quitaras un peso enorme.

#sprite:sophia_thinking
Tu energía debería ir a un lugar donde no luches contra tu propia naturaleza.

#sprite:joseph_neutral
Estoy cansado de pelear contra una pared que no se mueve.

#sprite:sophia_neutral
Entonces vámonos. Este lugar ya no tiene nada más para ti.

#sprite:joseph_sad
#setvar:ruta:Epilogo4
#setflag:Final_Alcanzado
Adiós a los libros... supongo que este nunca fue mi camino.
-> END

{ GetVar("ruta") == "Epilogo4": -> epilogo_nietzsche }