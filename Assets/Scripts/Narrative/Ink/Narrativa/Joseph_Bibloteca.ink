==== Joseph_Bibloteca_Prologo===
#setflag:Joseph_Bibloteca_Prologo
#sprite:sophia_neutral
Joseph, deberías aprovechar para repasar o prepararte. Si quieres que esta vez sea diferente, necesitas comprometerte de verdad.

#sprite:joseph_sad
¿Pretendes que llene mi vacío existencial con más libros? Sophia, eso es lo que me ha estado asfixiando toda la vida.

#sprite:sophia_thinking
Te asfixia porque lo ves como una imposición, no como una herramienta para tu propia liberación.

#sprite:joseph_sad
No entiendo cómo leer sobre contratos o leyes puede hacerme sentir "libre".

#sprite:sophia_neutral
La ignorancia es la mayor de las cárceles. Ven conmigo, vamos a la biblioteca.

#sprite:joseph_sad
Es el lugar que más odio en este campus. El silencio allí dentro me grita mis fracasos.

#sprite:joseph_thinking
Siento que si respiro muy fuerte, un bibliotecario del siglo XVIII va a salir de entre los estantes a regañarme.

#sprite:sophia_neutral
Pues hoy vamos a entrar para que ese silencio se convierta en concentración.

#sprite:joseph_sad
He pasado por tres carreras antes que esta, y en todas las bibliotecas me sentí como un intruso.

#sprite:sophia_happy
Esta vez es diferente porque no vas a estar solo. Vamos a vencer ese miedo juntos.

#sprite:sophia_neutral
Míralo como un entrenamiento para tu espíritu. Vamos, Joseph, antes de que pierdas el valor.
// Sophia y Joseph caminan hacia la biblioteca


-> END
==== Joseph_Bibloteca===
#setflag:Joseph_Bibloteca
#sprite:sophia_neutral
Ya estamos aquí. Mira este lugar… el silencio casi se puede tocar.

#sprite:joseph_neutral
Es más imponente de lo que recordaba.

#sprite:joseph_neutral
Esas computadoras… parecen de otra época. Como si el tiempo se hubiera detenido para que nadie deje de estudiar.

#sprite:sophia_thinking
Es un refugio. Aquí el ruido del exterior no existe.

#sprite:sophia_neutral
Solo estás tú… y lo que decidas aprender.

#sprite:joseph_sad
(Suspira)
Solo espero que este “refugio” no termine siendo otra celda.

#sprite:sophia_neutral
Empieza por algo pequeño. No tienes que entenderlo todo hoy.

#sprite:joseph_neutral
…Supongo que puedo intentarlo.

#sprite:joseph_neutral
voy a tomar un libro de la estantería, lo observare un momento

#sprite:joseph_sad
Hace tiempo que no abro uno sin sentir presión…

#sprite:sophia_thinking
Entonces no lo hagas por presión.

#sprite:sophia_neutral
Hazlo por curiosidad. Solo una página.

#sprite:joseph_neutral
…Está bien. Una página.
#setvar:ruta:Decision_Biblioteca_1
#setflag:Decision_Biblioteca_1
#setflag:Desicion_DeCamino
-> Decision_Biblioteca_1 

=== HandleDecision_Biblioteca_1 ===
{
- GetVar("ruta") == "estrategia": -> Camino_1_Joseph_Biblioteca
- GetVar("ruta") == "rendirse": -> Camino_2_Joseph_Biblioteca
- GetVar("ruta") == "Decision_Biblioteca_1": -> Decision_Biblioteca_1
}
=== Decision_Biblioteca_1 ===
#sprite:sophia_thinking
¿Qué debería hacer con Joseph?
+[Crear estrategia]
#sprite:sophia_euforic
#setvar:ruta:estrategia
voy a ayudar a joseph
-> END
+[Decirle que no puede]
#sprite:sophia_sad
#setvar:ruta:rendirse
mejor me rindo
-> END
+[Pensar después]
#sprite:sophia_thinking
#setvar:ruta:Decision_Biblioteca_1
-> END


=== Camino_1_Joseph_Biblioteca ===
#setflag:Camino_1_Joseph_Biblioteca

#sprite:joseph_sad
Lo sabía. Siempre me pasa. No soy brillante, Sophia... mi cerebro no funciona como el tuyo o el de mis compañeros.

#sprite:sophia_neutral
No se trata de ser un genio, pero noto que te cuesta conectar conceptos fundamentales.

#sprite:joseph_sad
Hay personas que entienden todo muy rápido... yo puedo leerlo diez veces y mi mente sigue en blanco.

#sprite:sophia_thinking
Tu velocidad de procesamiento es distinta. Te enredas en la terminología técnica.

#sprite:joseph_sad
Es humillante darme cuenta... quizás no es flojera, sino falta de capacidad.

#sprite:sophia_neutral
No digas eso. Pero sí es evidente que el camino académico será difícil para ti.

#sprite:sophia_thinking
Tal vez necesitamos una estrategia diferente.

#sprite:joseph_sad
¿Pero qué estrategia le sirve a alguien que olvida el inicio del párrafo al llegar al final?

#sprite:sophia_happy
Olvida la memoria literal. Asocia cada concepto con algo real.

#sprite:sophia_neutral
Usa ejemplos, no solo teoría.

#sprite:joseph_neutral
¿Crees que fragmentar la información me ayude?

#sprite:sophia_happy
Es la única forma: avanzar lento, pero seguro.

#sprite:joseph_sad
Me tomará el triple de tiempo...

#sprite:sophia_neutral
#setvar:ruta:decision_biblioteca_2
#setflag:decision_biblioteca_2
No compitas con otros. Compite con tu propio ritmo.
#sprite:joseph_neutral
Está bien... peor es no intentarlo.
-> END
=== HandleDecision_Biblioteca_2 ===
{
- GetVar("ruta") == "disciplina": -> Camino_1_2_Joseph_Biblioteca
- GetVar("ruta") == "rendirse_2": -> Camino_2_2_Joseph_Biblioteca
- GetVar("ruta") == "decision_biblioteca_2": -> decision_biblioteca_2
}
=== decision_biblioteca_2 ===
#sprite:sophia_thinking
¿Qué debería hacer con Joseph?
+[Seguir disciplinado]
#sprite:sophia_neutral
#setvar:ruta:disciplina
Joseph ha mejorado, pero hay que ser constante y disciplinado - Hay mucho camino por recorrer
-> END
+[Rendirse]
#setvar:ruta:rendirse_2
No ha avanzado casi nada - Este es un reto muy duro
-> END
+[Decidir después]
#sprite:sophia_thinking
#setvar:ruta:decision_biblioteca_2
Voy a pensarlo
-> END

=== Camino_1_2_Joseph_Biblioteca ===
#setflag:Camino_1_2_Joseph_Biblioteca
#sprite:joseph_neutral
Llevo dos horas con la misma página, pero no he sentido ganas de rendirme ni una sola vez.

#sprite:sophia_happy
Eso es porque dejaste de pelear contra tu naturaleza.

#sprite:joseph_neutral
Antes creía que ser libre era aprobar sin esfuerzo... ahora veo que es mantenerme firme aquí.

#sprite:sophia_neutral
La disciplina es la forma más pura de libertad.

#sprite:joseph_sad
Mi mente sigue siendo un terreno difícil...

#sprite:sophia_happy
Pero tu voluntad es la que manda ahora.

#sprite:joseph_neutral
Es extraño... al aceptar que soy lento, ya no me pesa lo que piensan los demás.

#sprite:sophia_thinking
Porque te enfocas en lo que puedes controlar.

#sprite:joseph_neutral
Seguiré con el siguiente párrafo. No busco rapidez, busco constancia.

#sprite:sophia_happy
Esa es tu verdadera victoria.

#sprite:joseph_sad
Sophia... siento que la cabeza me va a estallar. ¿Y si no nací para esto?

#sprite:sophia_neutral
El cansancio no decide por ti.

#sprite:joseph_sad
Quiero cerrar el libro...

#sprite:sophia_neutral
Eso es volver a la misma prisión.

#sprite:joseph_neutral
Si me rindo ahora... todo habrá sido igual.

#sprite:sophia_happy
Hazlo por tu propia voluntad, no por los demás.

#sprite:joseph_neutral
Está bien... tomaré agua y seguiré.

#sprite:sophia_euforic
Tu libertad está en no rendirte.
#setvar:ruta:Epilogo3
#setflag:Final_Alcanzado
-> END

{ GetVar("ruta") == "Epilogo3": -> epilogo_estoicos}