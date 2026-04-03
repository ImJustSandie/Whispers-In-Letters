===Camino_2_Joseph_Arcade===
    #setflag:Camino_2_Joseph_Arcade
    #sprite:sophia_neutral
    Estuvo bien, pero ya es hora de irnos, Joseph. No podemos quedarnos aquí para siempre.
    
    #sprite:joseph_sad
    ¿Ya? Apenas estaba empezando en serio… siempre tiene que acabarse justo cuando mejor se pone.
    
    #sprite:sophia_thinking
    Precisamente por eso. No se trata de escapar, sino de saber cuándo parar.
    
    #sprite:joseph_sad
    Es fácil decirlo… aquí al menos siento que soy bueno en algo. Afuera… todo es distinto.
    
    #sprite:sophia_neutral
    Y eso no va a cambiar si te quedas escondido aquí. Lo que encontraste, te lo llevas contigo.
    
    #setvar:ruta:desicion3
    #setflag:Desicion3_Arcade
    #sprite:joseph_sad
    …Supongo que tienes razón. No puedo quedarme aquí para siempre, aunque quisiera.
    -> END
    
    === HandleDesicion3_Arcade ===
#setvar:ruta:desicion3
{
- GetVar("ruta") == "Oportunidad": -> Camino_1_2_Joseph_Arcade
- GetVar("ruta") == "sacarlo": -> Camino_2_2_Joseph_Arcade
- GetVar("ruta") == "desicion3": -> Desicion3_Arcade
}
=== Desicion3_Arcade ===
Que deberia hacer con joseph
+[Motivar a joseph a seguir su sueños]
#setvar:ruta:Oportunidad
#sprite:sophia_euforic
Aquí joseph encontro la razón de vida despues de todo No es un fracasado, solo tenías otro enfoque
-> END
+[Eso no vale para nada]
#setvar:ruta:sacarlo
#sprite:sophia_neutral
debo detener
-> Camino_2_2_Joseph_Arcade
+[Decidir luego]
#setvar:ruta:desicion3
#sprite:sophia_thinking
Dare una vuelta y luego sigo hablando con joseph
-> END
    
    ===Camino_2_2_Joseph_Arcade ===
    #setflag:Camino_2_2_Joseph_Arcade
    #sprite:sophia_euforic
    ¡Joseph, suelta ese mando ahora mismo! Llevamos horas aquí y ya perdiste la noción del tiempo.
    
    #sprite:joseph_sad
    ¡Solo cinco minutos más! Estoy a punto de romper otro récord, no puedes interrumpir ahora.
    
    #sprite:sophia_euforic
    ¡¿A quién le importa un récord en una máquina?! Hace una hora que me dijiste que ya terminabas, no voy a permitir que te hundas en este antro.
    
    #sprite:joseph_sad
    ¡Déjame en paz, Sophia! Aquí es donde soy feliz, no en esas clases.
    
    #sprite:sophia_euforic
    ¡Se acabó! Te levantas de esa silla o te dejo aquí solo.
    
    #sprite:joseph_sad
    ¡Me estás arruinando el momento! ¡Casi lo tenía!
    
    #sprite:sophia_neutral
    Te estoy evitando que te pierdas aquí; camina hacia la puerta.
    
    #sprite:joseph_sad
    Eres una dictadora... me traes para distraerme y ahora me arrastras como a un niño.
    
    #sprite:sophia_neutral
    Alguien tiene que ser el adulto hoy; muévete antes de que esto vaya a peor.
    
    #sprite:joseph_sad
    Está bien, ya voy... pero no esperes que te agradezca esto.
    
    #sprite:sophia_thinking
    Estás reaccionando así porque te costó soltarlo. Pero escucha: la libertad no es hacer lo que uno quiere todo el tiempo.
    
    #sprite:sophia_neutral
    Muévete, Joseph. Tienes clase. Tu futuro no se va a construir aquí dentro.
    
    #sprite:joseph_sad
    Oye, no me empujes… puedo salir solo.
    
    // Sophia reflexiona y cambia tono
    
    #sprite:sophia_thinking
    Joseph… mírame. Lo siento, no debí traerte aquí.
    
    #sprite:joseph_sad
    No es tu culpa… fui yo el que se dejó llevar.
    
    #sprite:sophia_neutral
    Fui irresponsable. Pensé que te ayudaría y solo te confundí más.
    
    #sprite:joseph_sad
    Por un momento sentí que todo tenía sentido… y ahora volver duele más.
    
    #sprite:sophia_thinking
    Me duele verte así… no era la idea.
    
    #sprite:joseph_sad
    Es que aquí todo era simple… y afuera todo pesa otra vez.
    
    #sprite:sophia_neutral
    Perdóname… esto no era una salida real.
    
    #sprite:joseph_sad
    Tal vez solo querías ayudar… pero ahora se siente como si hubiera sido mentira.
    
    #sprite:sophia_happy
    Entonces hagámoslo bien esta vez… enfrentémoslo juntos, paso a paso.
    #sprite:joseph_neutral
    No te culpes tanto… al menos ahora sé que, aunque cueste, mi lugar está allá afuera.
    #setvar:ruta:epilogo2 
{ GetVar("ruta") == "epilogo2": -> epilogo_hegel }
    ->END