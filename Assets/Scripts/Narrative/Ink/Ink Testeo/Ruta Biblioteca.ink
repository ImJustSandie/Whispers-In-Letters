// ==========================================
// CAMINO BIBLIOTECA - V1
// Rama condensada de dialogos_historia_3D_V3.ink
// Integra el sistema de flags y vars de History test.ink
// ==========================================


// ==========================================
// PRÓLOGO: Primer encuentro con Joseph
// El jugador decide si mandarlo a estudiar o a relajarse.
// Knot de entrada principal para este arco narrativo.
// ==========================================

=== prologo_joseph ===

// Si ya se tomó una decisión sobre Joseph, no repetir el prólogo
{ GetFlag("Prologo Joseph") == true:
    -> inicio_biblioteca
}

#setflag: Prologo Joseph
#sprite:joseph_neutral
Joseph: Sophia... hoy es uno de esos días en que no sé si seguir o simplemente desaparecer un rato.

#sprite:sophia_happy
Sophia: ¿Qué quieres decir con eso, Joseph?

#sprite:joseph_neutral
Joseph: Tengo un parcial la próxima semana. Ni siquiera sé por dónde empezar. Me siento paralizado.

#sprite:sophia_happy
Sophia: Ese bloqueo lo conozco. La pregunta es qué hacemos con él.

#sprite:joseph_neutral
Joseph: No sé... a veces siento que necesito despejarme primero. Otras, que si no estudio ahora nunca lo haré.

* [Llévalo a la biblioteca - Ahora es el momento, la claridad viene al empezar]
    #setvar:ruta:entrada_pendiente

    #sprite:sophia_happy
    Sophia: El bloqueo no se va esperando, Joseph. Se rompe actuando. Vamos a la biblioteca.

    #sprite:joseph_neutral
    Joseph: ...Está bien. Contigo al menos siento que no voy solo.

    #sprite:sophia_happy
    Sophia: Nunca vas solo. Vamos.

    -> END

* [Mándalo al arcade - Necesita descansar antes de poder concentrarse]
    #setvar:ruta:arcade

    #sprite:sophia_happy
    Sophia: De acuerdo, a veces la mente necesita soltar antes de poder agarrar algo nuevo.

    #sprite:joseph_neutral
    Joseph: ¿Lo dices en serio? ¿No me vas a regañar?

    #sprite:sophia_happy
    Sophia: Esta vez no. Pero que sea un descanso real, no una huida. Cuando vuelvas, hablamos.

    #sprite:joseph_neutral
    Joseph: Prometido. Gracias, Sophia.

    -> END


// ==========================================
// INICIO: Reencuentro o primer encuentro en biblioteca
// (Se activa cuando el jugador entra con Joseph a la biblioteca)
// ==========================================

=== inicio_biblioteca ===

// Redirigir según el estado actual de la ruta
{ GetVar("ruta") == "entrada_pendiente":
    -> decision_entrada_biblioteca
}
{ GetVar("ruta") == "estrategia_pendiente":
    -> biblioteca_estrategia
}
{ GetVar("ruta") == "avance_pendiente":
    -> decision_avance_estrategia
}
{ GetVar("ruta") == "disciplina_pendiente":
    -> biblioteca_disciplina
}
{ GetVar("ruta") == "rendicion_pendiente":
    -> biblioteca_rendicion_final
}
{ GetVar("ruta") == "disciplina":
    -> reencuentro_disciplina
}
{ GetVar("ruta") == "rendicion":
    -> reencuentro_rendicion
}

// --- PRIMER ENCUENTRO (solo si no hay estado previo) ---

#setflag: Encuentro Biblioteca
#sprite:sophia_happy
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

// Marcar que la primera conversación terminó y hay una decisión pendiente
#setvar:ruta:entrada_pendiente

-> END


// ==========================================
// DECISIÓN DE ENTRADA
// (El jugador vuelve a hablar con Sophia/Joseph
//  para tomar la decisión de cómo proceder)
// ==========================================

=== decision_entrada_biblioteca ===

{ GetVar("ruta") != "entrada_pendiente":
    -> END
}

#sprite:sophia_happy
Sophia: Bien, Joseph. Ahora que estamos aquí, ¿cuál es tu plan?

* [Diseñar una estrategia de estudio - Él no es un estudiante de molde]
    #setvar:ruta:estrategia_pendiente

    #sprite:sophia_happy
    Sophia: Perfecto. No todos aprendemos igual, así que encontraremos tu método.

    #sprite:joseph_neutral
    Joseph: Está bien... voy a intentarlo. Aunque no prometo nada.

    #sprite:sophia_happy
    Sophia: Con eso es suficiente por ahora. Vuelve cuando estés listo para empezar.

    -> END

* [Me doy por vencida - Tienes serias limitantes académicas]
    #setvar:ruta:rendicion_pendiente

    #sprite:sophia_happy
    Sophia: Joseph... no tiene sentido seguir forzando algo que te causa tanto daño.

    #sprite:joseph_neutral
    Joseph: ¿Lo dices en serio? ¿Me estás diciendo que abandone?

    #sprite:sophia_happy
    Sophia: Te digo que tu energía merece un camino que no te destruya. Piénsalo.

    -> END


// ==========================================
// RAMA ESTRATEGIA: Diseñar un método de estudio
// (Knot separado, el jugador lo retoma después)
// ==========================================

=== biblioteca_estrategia ===

{ GetVar("ruta") != "estrategia_pendiente":
    -> END
}

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

// Guardar estado para que el jugador pueda explorar y volver
#setvar:ruta:avance_pendiente

-> END


// ------------------------------------------
// DECISIÓN: ¿Progresó con la estrategia?
// (El jugador vuelve después de haber explorado)
// ------------------------------------------

=== decision_avance_estrategia ===

{ GetVar("ruta") != "avance_pendiente":
    -> END
}

#sprite:sophia_happy
Sophia: Joseph, han pasado unos días desde que empezaste el método. ¿Cómo te fue?

* [Joseph ha mejorado - Hay que ser constante y disciplinado]
    #setvar:ruta:disciplina_pendiente

    #sprite:joseph_neutral
    Joseph: Creo que... algo está cambiando. No entiendo todo, pero ya no me bloqueo igual.

    #sprite:sophia_happy
    Sophia: Eso es exactamente lo que esperaba. La constancia está dando frutos.

    #sprite:joseph_neutral
    Joseph: Todavía me cuesta. Pero seguiré.

    #sprite:sophia_happy
    Sophia: Ven a contarme más cuando estés listo. Yo estaré por aquí.

    -> END

* [No avanzaste casi nada - Este reto es muy duro]
    #setvar:ruta:rendicion_pendiente

    #sprite:joseph_neutral
    Joseph: No puedo mentirte, Sophia. Apenas pude con una página. No sirvo para esto.

    #sprite:sophia_happy
    Sophia: Tu esfuerzo fue real, aunque el resultado no fuera el esperado.

    #sprite:joseph_neutral
    Joseph: ¿Y de qué sirve el esfuerzo si no lleva a ningún lado?

    #sprite:sophia_happy
    Sophia: Quizás hay otro lado al que debería llevarte. Piénsalo. Hablaremos pronto.

    -> END


// ==========================================
// RAMA FINAL: Disciplina y constancia
// (Knot separado, retomado después de explorar)
// ==========================================

=== biblioteca_disciplina ===

{ GetVar("ruta") != "disciplina_pendiente":
    -> END
}

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

// Conversación completa — resetear para futuros reencuentros
#setvar:ruta:disciplina
#setflag: Ruta Terminada

-> END


// ==========================================
// RAMA FINAL: Rendición
// (Knot separado, retomado desde estado pendiente)
// ==========================================

=== biblioteca_rendicion_final ===

{ GetVar("ruta") != "rendicion_pendiente":
    -> END
}

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

// Conversación completa — resetear para futuros reencuentros
#setvar:ruta:rendicion
#setflag: Ruta Terminada

-> END


// ==========================================
// REENCUENTROS (el jugador vuelve a hablar
// con Joseph/Sophia después de concluir la ruta)
// ==========================================

=== reencuentro_disciplina ===

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
