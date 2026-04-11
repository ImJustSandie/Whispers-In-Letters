
=== Joseph_Arcade_Prologo==
#setflag:Joseph_Arcade_Prologo
#sprite:sophia_thinking
He estado pensando en lo que dijiste… a veces nos exigimos demasiado. ¿Y si hoy simplemente te das un respiro?

#sprite:joseph_sad
¿"Darme un respiro" cómo?

#sprite:sophia_neutral
Dejar de pensar tanto. Solo vivir un rato.

#sprite:joseph_sad
No estoy para caminatas filosóficas.

#sprite:sophia_thinking
No es eso. Conozco un lugar donde nadie te juzga. Un refugio.

#sprite:joseph_neutral
Suena tentador… ¿y la clase?

#sprite:sophia_neutral
La clase seguirá ahí. Tú necesitas parar hoy.

#sprite:joseph_sad
No necesito un refugio… necesito un título para que mi padre deje de verme como un error.

#sprite:sophia_thinking
¿Y si ese título es la jaula?

#sprite:joseph_sad
Tal vez… pero es la única forma que tengo de no fallar.

#sprite:joseph_sad
Y si falto, el profesor me va a marcar.

#sprite:sophia_neutral
En un año no recordará tu nombre. Tú sí recordarás este momento.

#sprite:joseph_sad
No lo sé… esto se siente mal.

#sprite:sophia_thinking
Confía en mí. Solo esta vez.

#sprite:joseph_sad
Si alguien me ve fuera del campus…

#sprite:sophia_happy
Nadie lo sabrá. Solo por hoy, deja de pensar en consecuencias.

#sprite:joseph_neutral
Está bien… igual ya me siento fuera de lugar.

#sprite:sophia_happy
Vamos, antes de que te arrepientas.

#sprite:joseph_neutral
#setflag:Desicion_DeCamino
Solo por hoy.

->END

=== Joseph_Arcade_===
#setflag:Joseph_Arcade
#setvar:ruta:arcade
// Llegan al arcade
#sprite:sophia_euforic
¡Bienvenido al templo del presente! Aquí el placer es la única norma.
#sprite:joseph_happy
Ay caramba... esto me encanta, es el paraíso.
#sprite:sophia_happy
¡Mira ese juego! Se ve genial. Olvida la carrera y el "llegar a ser alguien". Pásatela bien y ya.
#sprite:joseph_neutral
Hace siglos que no jugaba uno de estos. Mi padre siempre decía que esto era perder el tiempo.
#sprite:sophia_happy
¡Pruébalo! Deja que tus manos decidan por ti. No pienses en el examen de mañana, solo en el oponente de la pantalla.
#sprite:joseph_happy
Es increíble… Me siento como si estuviera en otra dimensión. Una donde no decepciono a nadie. Podría quedarme aquí horas... o días enteros si me lo permitieran.
#sprite:sophia_happy
¿Ves? La libertad no estaba escondida en los libros, estaba en este lugar.
#sprite:joseph_happy
Gracias por traerme.
#sprite:sophia_neutral
Disfrútalo. Te lo mereces después de tanto tiempo viviendo para los demás.
#setvar:ruta:desicion_Arcade_1
#setflag:Desicion1_Arcade
Voy a jugar mi primera partida.
-> Desicion1_Arcade

=== HandleDesicion1_Arcade ===
{
- GetVar("ruta") == "motivacion": -> Camino_1_Joseph_Arcade
- GetVar("ruta") == "desmotivarlo": -> Camino_2_Joseph_Arcade
- GetVar("ruta") == "desicion_Arcade_1": -> Desicion1_Arcade
}
=== Desicion1_Arcade ===
#sprite: sophia_thinking
Que deberia hacer con joseph
+[Dejarlo y que siga jugando]
#setvar:ruta:motivacion
#sprite:sophia_euforic
joseph se merece romper esa rutina y encontrar un respiro de la universidad
->  Camino_1_Joseph_Arcade
+[llevarlo a la universidad]
#setvar:ruta:desmotivarlo
#sprite:sophia_sad
Que le hice a joseph debo sacarlo
-> Camino_2_Joseph_Arcade
+[Decidir luego]
#setvar:ruta:desicion_Arcade_1
#sprite:sophia_thinking
Dare una vuelta y luego sigo hablando con joseph
-> END

=== Camino_1_Joseph_Arcade ===
#setflag:Camino_1_Joseph_Arcade
#setvar:ruta:desicion2
#sprite:joseph_happy
¡Sophia! ¡Le acabo de ganar al récord de la semana! ¡Soy el rey de este lugar!

#sprite:joseph_happy
Quiero ver qué hay en el siguiente nivel… y en el último. No importa cuánto tarde.

#sprite:joseph_happy
¡Mira! El puntaje está subiendo rapidísimo.

#sprite:sophia_happy
No sabía que eras tan bueno. Deberías ser streamer o jugador profesional.

#sprite:sophia_neutral
¿Ves? Allá te hacían sentir como un fracasado… aquí construyes tu propia victoria.

#sprite:joseph_happy
¡Tres niveles en menos de diez minutos!

#sprite:sophia_euforic
No te detengas ahora, Joseph.

#sprite:joseph_happy
Me siento invencible… como si toda la frustración se volviera energía.

#sprite:sophia_happy
Eso pasa cuando dejas de vivir para otros… y empiezas a ser tú.

#setvar:ruta:desicion2
#setflag:Desicion2_Arcade
Hazlo, Joseph. Explora, juega, sé el dueño de este pequeño universo.
-> END

=== HandleDesicion2_Arcade ===
{
- GetVar("ruta") == "vocacion": -> Camino_1_2_Joseph_Arcade
- GetVar("ruta") == "universidad": -> Camino_2_2_Joseph_Arcade
- GetVar("ruta") == "desicion2": -> Desicion2_Arcade
}
=== Desicion2_Arcade ===
Que deberia hacer con joseph
+[Darle otra oportunidad]
#setvar:ruta:vocacion
#sprite:sophia_euforic
Tienes potencial para esto de los videojuegos
-> Camino_1_2_Joseph_Arcade
+[Eso no vale para nada]
#setvar:ruta:universidad
#sprite:sophia_neutral
debo detener 
-> Camino_2_2_Joseph_Arcade
+[Decidir luego]
#setvar:ruta:desicion2
#sprite:sophia_thinking
Dare una vuelta y luego sigo hablando con joseph
-> END

=== Camino_1_2_Joseph_Arcade ===
#setflag:Camino_1_2_Joseph_Arcade
#sprite:joseph_neutral
Me gusta este lugar... no hay preguntas difíciles, solo obstáculos que puedo superar. Aquí es donde pertenezco.
Además, acabo de entender algo: no soy un mal estudiante… solo estaba en el lugar equivocado.

#sprite:sophia_happy
Te lo dije. Solo necesitabas el entorno correcto. Tienes una precisión increíble.

#sprite:joseph_neutral
Esto ya no es un juego para mí… es estrategia, es control.

#sprite:sophia_thinking
Parece que por fin encontraste un idioma que tu mente quiere hablar.

#sprite:joseph_sad
Por primera vez… no me siento como un fracaso.

#sprite:sophia_neutral
Esa es la confianza que te faltaba.

#sprite:joseph_happy
Me siento útil. Siento que podría ser realmente bueno en esto… no uno más, sino el mejor.

#sprite:joseph_happy
Se acabó intentar encajar en algo que no soy. Voy a hacer mi propio camino.

#sprite:sophia_thinking
¿Y si esta “libertad” es solo otra presión distinta?

// Joseph decae un momento

#sprite:joseph_sad
Tal vez esto es una locura… mis padres nunca aceptarían esto.

#sprite:sophia_neutral
Mírate, vuelves a dudar.

#sprite:joseph_sad
Aquí no soy ese fracaso… aquí soy alguien.

#sprite:sophia_happy
Entonces no dejes que ese miedo te detenga. Vuelve al juego.

#sprite:joseph_neutral
¿Me estás diciendo que ignore sus llamadas?

#sprite:sophia_thinking
Te digo que elijas quién quieres ser ahora.

#sprite:joseph_happy
Tienes razón… el torneo sigue.

#sprite:sophia_euforic
Ve. Demuestra que esto no es una huida.

#sprite:joseph_happy
Gracias, Sophia… nunca me había sentido así de validado.

#setvar:ruta:Epilogo1
#setflag:Final_Alcanzado

#sprite:sophia_neutral
No mires atrás. Corre antes de que la duda vuelva.
-> END

{ GetVar("ruta") == "Epilogo1": -> epilogo_schopenhauer }