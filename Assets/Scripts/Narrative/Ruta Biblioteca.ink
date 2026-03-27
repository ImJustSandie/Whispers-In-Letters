// ==========================================
// CAMINO BIBLIOTECA - V1
// Rama condensada de dialogos_historia_3D_V3.ink
// Integra el sistema de flags y vars de History test.ink
// ==========================================

// ==========================================
// INICIO: Reencuentro o primer encuentro
// ==========================================

=== inicio_biblioteca ===

// Si ya hubo un reencuentro previo, saltar directo al estado guardado
{ GetVar("ruta") == "disciplina":
    -> reencuentro_disciplina
}
{ GetVar("ruta") == "rendicion":
    -> reencuentro_rendicion
}

// --- PRIMER ENCUENTRO ---

#sprite:sophia_happy
#setflag: Encuentro Biblioteca
Sophia: Joseph deberías aprovechar para repasar o prepararte. Si quieres que esta vez sea diferente, necesitas comprometerte de verdad.

#sprite:joseph_neutral
Joseph: ¿Pretendes que llene mi vacío existencial con más libros? Sophia, eso es lo que me ha estado asfixiando toda la vida.

#sprite:sophia_happy
Sophia: Te asfixia porque lo ves como una imposición, no como una herramienta para tu propia liberación.

#sprite:joseph_neutral
Joseph: No entiendo cómo leer sobre contratos o leyes puede hacerme sentir "libre".

#sprite:sophia_happy
Sophia: La ignorancia es la mayor de las cárceles. Ven conmigo, vamos a la biblioteca.

#sprite:joseph_neutral
Joseph: Es el lugar que más odio en este campus. El silencio allí dentro me grita mis fracasos.

#sprite:sophia_happy
Sophia: Pues hoy vamos a entrar para que ese silencio se convierta en concentración.

-> decision_entrada_biblioteca


// ==========================================
// DECISIÓN DE ENTRADA
// ==========================================

=== decision_entrada_biblioteca ===

* [Diseñar una estrategia de estudio - Él no es un estudiante de molde]
    #setvar:ruta:estrategia
    -> biblioteca_estrategia

* [Me doy por vencida - Tienes serias limitantes académicas]
    #setvar:ruta:rendicion
    -> biblioteca_rendicion_final


// ==========================================
// RAMA DISCIPLINA: Estrategia de estudio
// ==========================================

=== biblioteca_estrategia ===

#setflag: Estrategia Planteada
#sprite:joseph_neutral
Joseph: Lo sabía. Siempre me pasa. No soy brillante, Sophia, mi cerebro no funciona como el tuyo.

#sprite:sophia_happy
Sophia: No se trata de ser un genio. Noto que te cuesta conectar conceptos fundamentales, así que busquemos otro método.

#sprite:joseph_neutral
Joseph: ¿Qué estrategia puede servirle a alguien que olvida el inicio del párrafo al llegar al final?

#sprite:sophia_happy
Sophia: Olvida la memoria literal. Asocia cada concepto a algo concreto y a ejemplos reales, no a esquemas abstractos.

#sprite:joseph_neutral
Joseph: ¿Crees que fragmentar la información realmente me ayude a avanzar?

#sprite:sophia_happy
Sophia: Es la única forma: avanzar lento pero seguro. No compitas contra el reloj de otros, solo contra tu propio ritmo.

#sprite:joseph_neutral
Joseph: Está bien. Nada se pierde, peor es no intentarlo.

-> decision_avance_estrategia


// ------------------------------------------
// DECISIÓN: ¿Progresó con la estrategia?
// ------------------------------------------

=== decision_avance_estrategia ===

* [Joseph ha mejorado - Hay que ser constante y disciplinado]
    #setvar:ruta:disciplina
    -> biblioteca_disciplina

* [No avanzaste casi nada - Este reto es muy duro]
    #setvar:ruta:rendicion
    -> biblioteca_rendicion_final


// ==========================================
// RAMA FINAL: Disciplina y constancia (Epílogo estoico)
// ==========================================

=== biblioteca_disciplina ===

#setflag: Disciplina Alcanzada
#sprite:joseph_neutral
Joseph: Llevo dos horas con la misma página, pero no he sentido ganas de rendirme ni una sola vez.

#sprite:sophia_happy
Sophia: Eso es porque has dejado de pelear contra tu naturaleza y has aceptado tu ritmo.

#sprite:joseph_neutral
Joseph: Antes creía que ser libre era aprobar sin esfuerzo. Ahora veo que es mantenerme firme aquí.

#sprite:sophia_happy
Sophia: La disciplina es la forma más pura de libertad, Joseph. Tú decides no ser esclavo de tu pereza.

#sprite:joseph_neutral
Joseph: Sophia, siento que la cabeza me va a estallar... ¿y si simplemente no nací para esto?

#sprite:sophia_happy
Sophia: El cansancio es solo una impresión de tus sentidos. No dejes que dicte lo que vas a hacer.

#sprite:joseph_neutral
Joseph: Tienes razón. Si me rindo ahora, le doy la razón a todos los que dijeron que no podía.

#sprite:sophia_happy
Sophia: No lo hagas por ellos. Hazlo por la dignidad de tu propia voluntad.

#sprite:joseph_neutral
Joseph: Está bien, tomaré un poco de agua y seguiré. No voy a permitir que el cansancio me venza.

-> END


// ==========================================
// RAMA FINAL: Rendición (Epílogo Nietzsche)
// ==========================================

=== biblioteca_rendicion_final ===

#setflag: Rendicion Aceptada
#sprite:joseph_neutral
Joseph: Es inútil, Sophia. Por más que lo intente, estas palabras son solo ruido para mí.

#sprite:sophia_happy
Sophia: Veo el esfuerzo en tus ojos, pero también veo que tu mente se bloquea por completo.

#sprite:joseph_neutral
Joseph: No es falta de ganas, es que simplemente no tengo las herramientas para procesar esto.

#sprite:sophia_happy
Sophia: A veces, insistir en lo imposible no es virtud, sino una forma de tortura innecesaria.

#sprite:joseph_neutral
Joseph: Es doloroso admitirlo, pero mi límite académico está aquí.

#sprite:sophia_happy
Sophia: Tu energía debe ir a otro lugar donde no tengas que luchar contra tu propia naturaleza.

#sprite:joseph_neutral
Joseph: Adiós a los libros... es hora de aceptar que este camino nunca fue el mío.

-> END


// ==========================================
// REENCUENTROS (si el jugador vuelve a hablar
// con Joseph después de una sesión previa)
// ==========================================

=== reencuentro_disciplina ===

#setvar:ruta:Neutro
#sprite:joseph_neutral
Joseph: Sophia... he seguido con el método que diseñamos.

#sprite:joseph_neutral
Joseph: Es lento, pero ya no me frustro tanto con las páginas.

#sprite:sophia_happy
Sophia: Eso es todo lo que importa. La constancia es tu verdadera victoria.

#sprite:joseph_neutral
Joseph: Gracias por no rendirte conmigo.

-> END


=== reencuentro_rendicion ===

#setvar:ruta:Neutro
#sprite:joseph_neutral
Joseph: Sophia... después de lo que pasó en la biblioteca, estuve pensando.

#sprite:joseph_neutral
Joseph: Quizás tenías razón. No se puede forzar una cerradura que no tiene llave.

#sprite:sophia_happy
Sophia: ¿Y ahora qué vas a hacer?

#sprite:joseph_neutral
Joseph: Todavía no lo sé. Pero al menos ya no me rompo contra lo mismo.

-> END


// ==========================================
// FIN DE HISTORIA
// ==========================================

=== fin_historia ===

* [Yo sé que hice lo correcto. La libertad no es elegir todos los caminos, es comprometerse con uno.]
    -> END
* [Tal vez pude haber escogido otro camino... Ese es el precio de ser humanos y tener el poder de decidir.]
    -> END
