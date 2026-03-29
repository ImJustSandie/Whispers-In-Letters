
=== Joseph_Arcade_Prologo==
#setflag:Joseph_Arcade_Prologo
#sprite:sophia_thinking
Oye, he estado pensando en lo que me dijiste y.… a veces nos exigimos demasiado por lo que otros esperan. ¿Y si hoy mandas todo a volar y te relajas?

#sprite:joseph_sad
¿A qué te refieres con "mandar todo a volar"?

#sprite:sophia_neutral
Pues a relajarte, a no pensar tanto y simplemente vivir.

#sprite:joseph_sad
No estoy de humor para una caminata filosófica por el parque.

#sprite:sophia_thinking
Ven conmigo, conozco un lugar que es como un refugio, donde nadie te va a juzgar ni a pedir resultados. Es un sitio para que seas tú, sin la voz de tus papás o de la universidad en la cabeza. Tómalo, solo por hoy. Úsalo para encontrarte.

#sprite:joseph_neutral
Suena peligrosamente tentador... pero ¿y la clase de hoy?

#sprite:sophia_neutral
La clase estará ahí mañana, y el próximo semestre también. Pero hoy, hoy necesitas respirar sin pedir permiso.

#sprite:joseph_sad
No necesito un refugio, necesito un título para que mi padre deje de mirarme como si fuera un error.

#sprite:sophia_thinking
Ese título es tu jaula. Vamos a un lugar donde puedas ser tú, sin la voz de otros diciéndote qué hacer.

#sprite:joseph_sad
Es una locura... si pierdo esta clase, el profesor me pondrá en su lista negra.

#sprite:sophia_neutral
El profesor ni siquiera recordará tu nombre en un año. Pero tú recordarás este día como el momento en que dijiste "basta".

#sprite:joseph_sad
No lo sé... esto se siente muy mal. Muy mal.

#sprite:sophia_thinking
Así no es, ven conmigo. Confía en mí por una sola vez.

#sprite:joseph_sad
Si esto sale mal... si alguien me ve fuera del campus a esta hora...

#sprite sophia_happy
Nadie lo va a saber. Seremos invisibles. Solo por hoy. Solo por esta vez intenta no pensar en las consecuencias.

#sprite:joseph_neutral
Está bien. Total, ya me siento como un extraño en mi propia vida.

#sprite:sophia_happy
Esa es la actitud que quería ver. Vamos, antes de que el arrepentimiento te alcance en la puerta del salón.

#sprite:joseph_neutral
Solo por hoy. Solo por esta vez intentaré no pensar en las consecuencias.
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
#sprite:joseph_happy
Voy a jugar mi primera partida.

// Juega por primera vez
#setvar:ruta:desicion
-> END

=== Desicion1_Arcade ===
#setflag:Desicion1_Arcade

  { GetVar("ruta") == "motivacion":
    -> Camino_1_Joseph_Arcade
}
{ GetVar("ruta") == "desmotivarlo":
    -> Camino_2_Joseph_Arcade
}
{ GetVar("ruta") == "desicion":
    -> Desicion1_Arcade
}
#sprite: sophia_thinking
  Que deberia hacer con joseph
  
    +[Dejarlo y que siga jugando]
    #setvar:ruta:motivacion
     #sprite:sophia_euforic
     joseph se merece romper esa rutina y encontrar un respiro de la universidad

     ->END
  
    +[llevarlo a la universidad]
    #setvar:ruta:desmotivarlo
     #sprite:sophia_sad
     Que le hice a joseph debo  sacarlo
     ->END
     
    +[Decidir luego]
     #setvar:ruta:desicion
     #sprite:sophia_thinking
     Dare una vuelta y luego sigo hablando con joseph
     ->END
     
=== Camino_1_Joseph_Arcade ===
#setflag:Camino_1_Joseph_Arcade
#setvar:ruta:desicion2
#sprite:joseph_happy
Mira Sophia. ¡Le acabo de ganar al récord de la semana! ¡Soy el rey de este paraíso!

#sprite:joseph_happy
Quiero ver qué hay en el siguiente nivel, y en el siguiente, y en el que sigue después de ese. Quiero llegar al final. No importa si tardo toda la noche. Quiero ver qué hay detrás del último nivel.

#sprite:joseph_happy
Sophia, mira esto... ¡el puntaje está subiendo tan rápido.

#sprite:sophia_happy
No sabía que eras tan bueno, deberías ser un streamer de videojuegos o un jugador profesional.

#sprite:sophia_neutral
¿Ves? En la universidad te hacían sentir como un fracasado, pero aquí eres el arquitecto de tu propia victoria.

#sprite:joseph_happy
¡Increíble…Tres niveles en menos de diez minutos!

#sprite:sophia_euforic
No te detengas ahora, Joseph. El mundo exterior merece conocer este talento.

#sprite:joseph_happy
Me siento invencible... es como si toda la frustración de estos años se estuviera convirtiendo en energía para mis dedos.

#sprite:sophia_happy
Eso es lo que pasa cuando dejas de ser quien tus padres quieren y te conviertes en quien realmente eres: un conquistador.

#sprite:joseph_happy
¡Otra partida! Siento que ahora mismo nada puede salir mal.

#sprite:sophia_euforic
Hazlo, Joseph. Explora, juega, sé el dueño de este pequeño universo.
#setvar:ruta:desicion2
-> END


=== Desicion2_Arcade ===
#setflag:Desicion2_Arcade
  { GetVar("ruta") == "vocacion":
    -> Camino_1_2_Joseph_Arcade
}

{ GetVar("ruta") == "universidad":
    -> Camino_2_Joseph_Arcade
}
{ GetVar("ruta") == "desicion2":
    -> Desicion2_Arcade
}
  Que deberia hacer con joseph
  
    +[Darle otra oportunidad]
    #setvar:ruta:vocacion
     #sprite:sophia_euforic
     Tienes potencial para esto de los videojuegos
     ->END
  
    +[Eso no vale para nada]
    #setvar:ruta:universidad
     #sprite:sophia_neutral
     debo detener 
     ->END
     
    +[Decidir luego]
     #setvar:ruta:desicion2
     #sprite:sophia_thinking
     Dare una vuelta y luego sigo hablando con joseph
     ->END

=== Camino_1_2_Joseph_Arcade ===
#setflag:Camino_1_2_Joseph_Arcade
#sprite:joseph_neutral
Me gusta este lugar... no hay preguntas difíciles, solo obstáculos que puedo saltar o destruir. aquí es donde realmente pertenezco. Además, acabo de descubrir algo, no soy un mal estudiante, soy un profesional fuera de lugar

#sprite:sophia_happy
Te lo dije. Solo necesitabas un entorno diferente. Tienes una coordinación asombrosa. He visto a gente jugar años y no tienen tu precisión.

#sprite:joseph_neutral
Es que esto no es un juego para mí ahora. Es arquitectura de datos en movimiento. Es estrategia pura.

#sprite:sophia_thinking
Parece que por fin encontraste un idioma que tu cerebro sí quiere hablar.

#sprite:joseph_sad
Por primera vez en mi vida, no me siento como un "fracasado" que decepciona a sus padres

#sprite:sophia_neutral
Me alegra que lo veas así, Joseph. Esa es la confianza que te faltaba.

#sprite:joseph_happy
Me siento... útil. Siento que, si me dedico a esto profesionalmente, podría ser el mejor. No uno del montón, sino el mejor.

#sprite:joseph_happy
Se acabó el intentar encajar en un molde que me rompe. Voy a construir mi propio molde aquí mismo.

#sprite:sophia_thinking
Me pregunto si esta "libertad" que encontraste no es solo otra forma de presión que te estás imponiendo.

// Joseph decae un momento

#sprite:joseph_sad
Quizás esto es una locura, Sophia... mis padres nunca aceptarán que esta es mi nueva vida.

#sprite:sophia_neutral
Mírate Joseph; estás otra vez ansioso.

#sprite:joseph_sad
Es que en este lugar no soy el fracasado que ellos ven, aquí soy alguien importante.

#sprite:sophia_happy
Entonces no dejes que el miedo de ellos apague tu chispa; vuelve a ese juego.

#sprite:joseph_neutral
¿Me estás diciendo que ignore las llamadas que mis padres me han estado haciendo y me quede?

#sprite:sophia_thinking
Te digo que prefiero verte como un jugador apasionado que como un ente.

#sprite:joseph_happy
Tienes razón, si vuelvo ahora al salón, el torneo aún no habrá terminado.

#sprite:sophia_euforic
Ve y demuestra que esta es tu verdadera razón de ser, no una simple escapatoria.

#sprite:joseph_happy
Gracias, Sophia... es la primera vez que siento que alguien valida lo que realmente soy.

#sprite:sophia_neutral
Solo no mires atrás; corre antes de que la duda te alcance de nuevo.

#setvar:ruta:Epilogo1
-> END

{ GetVar("ruta") == "Epilogo1":
    -> epilogo_schopenhauer
}