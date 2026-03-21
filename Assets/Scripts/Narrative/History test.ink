
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

#sprite:sophia_happy
Hola

#sprite:joseph_neutral
¿Asi que esta es la biblioteca? no se si me siento comodo aquí, pero creo que me iré acostumbrando.

Voy a hacer mi mayor esfuerzo por mejorar, de verdad quiero lograrlo esta vez...

+ [Motivarlo]
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
    #sprite:sophia_happy
    Ya has dicho eso antes. ¿Qué va a ser diferente esta vez?

    #sprite:joseph_neutral
    ...lo sé. No tengo una gran respuesta.

    #sprite:sophia_happy
    Entonces empieza por algo pequeño. Demuéstralo con acciones.

    #sprite:joseph_neutral
    Supongo que tienes razón... hablar es fácil.

    -> Joseph1_Cuestionado


=== Joseph1_Motivado ===

#sprite:joseph_neutral
Tal vez podría empezar revisando algunos libros básicos...

#sprite:sophia_happy
Buena idea. Paso a paso.

#sprite:joseph_neutral
Gracias. De verdad.

-> END


=== Joseph1_Cuestionado ===

#sprite:joseph_neutral
Quizá debería dejar de prometer tanto y simplemente hacer algo.

#sprite:sophia_happy
Exactamente.

#sprite:joseph_neutral
Entonces... empezaré ahora.

-> END