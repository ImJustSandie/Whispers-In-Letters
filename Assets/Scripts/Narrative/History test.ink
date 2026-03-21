
// Declarar funciones externas para conectar con el GameManager de Unity
EXTERNAL GetFlag(flagName)
EXTERNAL GetVar(varName)
// (Estos fallbacks son solo para que Inky no marque error de sintaxis)
=== function GetFlag(flagName) ===
~ return false
=== function GetVar(varName) ===
~ return ""

=== inicio ===
#sprite:sophia_happy
Este es un dialogo de prueba

-> END



=== obj1 ===
#sprite:sophia_happy
Esta es una libreria

A veces me da mucho sueño cuando estoy en la biblioteca

-> END


=== obj2 ===
#sprite:sophia_happy
Hola wenas

-> END


=== Joseph1 ===

// --- REENCUENTRO INMEDIATO (si ya habló antes en esta misma sesión o poco después) ---
{ GetVar("actitud_joseph") == "motivado":
    -> Joseph1_Motivado_Reencuentro
}
{ GetVar("actitud_joseph") == "cuestionado":
    -> Joseph1_Cuestionado_Reencuentro
}

// --- PRIMER ENCUENTRO ---

#sprite:sophia_happy
Hola...

#sprite:joseph_neutral
¿Así que esta es la biblioteca? No sé si me siento cómodo aquí... pero creo que me iré acostumbrando.

#sprite:joseph_neutral
Voy a hacer mi mayor esfuerzo por mejorar... de verdad quiero lograrlo esta vez...

    + [Motivarlo]
        #setvar:actitud_joseph:motivado

        #sprite:sophia_happy
        Me alegra escuchar eso. Este puede ser un buen comienzo si te lo tomas en serio.

        #sprite:joseph_neutral
        Eso intento... aunque no siempre es fácil mantenerme enfocado.

        #sprite:sophia_happy
        No tienes que hacerlo perfecto, solo constante.

        #sprite:joseph_neutral
        Supongo que eso suena más alcanzable...

        -> Joseph1_Motivado

    + [Cuestionarlo]
        #setvar:actitud_joseph:cuestionado

        #sprite:sophia_happy    
        Ya has dicho eso antes. ¿Qué va a ser diferente esta vez?

        #sprite:joseph_neutral
        ...lo sé. No tengo una gran respuesta.

        #sprite:sophia_happy
        Entonces empieza por algo pequeño. Demuéstralo con acciones.

        #sprite:joseph_neutral
        Supongo que tienes razón... hablar es fácil.

        -> Joseph1_Cuestionado


// --- RAMA MOTIVADO (PRIMERA VEZ) ---
=== Joseph1_Motivado ===

#sprite:joseph_neutral
Tal vez podría empezar revisando algunos libros básicos...

#sprite:sophia_happy
Buena idea. Paso a paso.

#sprite:joseph_neutral
Gracias. De verdad.

-> END


// --- RAMA CUESTIONADO (PRIMERA VEZ) ---
=== Joseph1_Cuestionado ===

#sprite:joseph_neutral
Quizá debería dejar de prometer tanto y simplemente hacer algo.

#sprite:sophia_happy
Exactamente.

#sprite:joseph_neutral
Entonces... empezaré ahora.

-> END


// --- REENCUENTRO SI LO MOTIVASTE ---
=== Joseph1_Motivado_Reencuentro ===

#sprite:joseph_neutral
Oh... Sophia.

#sprite:joseph_neutral
He estado pensando en lo que dijiste... eso de ser constante.

#sprite:joseph_neutral
No hice mucho todavía, pero... al menos abrí un libro.

#sprite:sophia_happy
Eso ya es un avance.

#sprite:joseph_neutral
Supongo que sí... antes ni siquiera lo intentaba.

-> END


// --- REENCUENTRO SI LO CUESTIONASTE ---
=== Joseph1_Cuestionado_Reencuentro ===

#sprite:joseph_neutral
...Sophia.

#sprite:joseph_neutral
No pude dejar de pensar en lo que dijiste.

#sprite:joseph_neutral
Tenías razón... hablo mucho, pero hago poco.

#sprite:sophia_happy
¿Y ahora?

#sprite:joseph_neutral
...ahora al menos me incomoda seguir igual.

#sprite:joseph_neutral
Supongo que eso es un inicio.

-> END