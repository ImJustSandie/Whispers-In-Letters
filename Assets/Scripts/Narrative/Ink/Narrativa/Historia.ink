
INCLUDE Joseph_Bibloteca.ink
INCLUDE Epilogos.ink
INCLUDE Joseph_Arcade..ink
INCLUDE Joseph_Arcade_Camino2.ink
INCLUDE Objects.ink
INCLUDE Joseph_Bibloteca_Camino2.ink




// Declarar funciones externas para conectar con el GameManager de Unity
EXTERNAL GetFlag(flagName)
EXTERNAL GetVar(varName)
// (Estos fallbacks son solo para que Inky no marque error de sintaxis)
=== function GetFlag(flagName) ===
~ return false
=== function GetVar(varName) ===
~ return ""

=== Joseph1_Prologo ===
#setflag:Joseph1_Prologo
#sprite:sophia_happy
 Hola Joseph, que milagro en verte por aquí tan temprano.
 #sprite:joseph_neutral
 Ah, hola Sophia, que bueno encontrarte.
 #sprite:joseph_sad
  La verdad no. Es lo mismo de siempre, pero más pesado...
   #setvar:ruta:desicion_prologo
  -> Joseph1_Prologo_Reencuentro
  === Joseph1_Prologo_Reencuentro ===
  #setflag:Joseph1_Prologo_Reencuentro



  ¿Seguro que tienes tiempo para escuchar mis dramas?
  
    +[No Escuhar a joseph]
    #setvar:ruta:desicion_prologo
     #sprite:sophia_euforic
     no tengo tiempo, pero espero que te vaya bien
     #sprite:joseph_sad
     No hay problema, nos vemos
     ->END
     
    +[Si Escuhar a joseph]
    #setvar:ruta:aprobacion
     #sprite:sophia_happy
     Para los amigos siempre hay tiempo, cuéntame que te tiene tan preocupado
     ->Joseph2_Prologo
   
{ GetVar("ruta") == "desicion_prologo": -> Joseph1_Prologo_Reencuentro }

=== Joseph2_Prologo ===
 #setflag:Joseph2_Prologo
#setvar:ruta:desicion
#sprite:joseph_happy
 Gracias por escucharme.
 #sprite:joseph_sad
 Es que... no sé qué estoy haciendo con mi vida. Mi familia siempre me ha dado todo lo económico, pero me siento solo, ¿sabes?
  #sprite:joseph_sad
  pero me siento solo, ¿sabes? Esta es la cuarta carrera que empiezo y no sé si voy a terminarla. Siento que solo estudio por la presión de ellos y por miedo a fracasar otra vez.
  #sprite:sophia_sad
  No digas eso, no te des tan duro. Todos tenemos nuestro ritmo y talento, solo que a veces tarda en aparecer.
  #sprite:sophia_neutral
  Somos jóvenes, este es el momento para cometer errores y volverlo a intentar, ¿no crees?
  #sprite:joseph_sad
  Eso dicen todos, pero yo siento que el tiempo se me escapa.
  #sprite:joseph_neutral
  Dicen que soy "libre" de elegir, pero cada elección se siente como una celda nueva.
  #sprite:sophia_thinking
  ¿Por qué estás tan empeñado en completar una carrera universitaria?
  #sprite:joseph_sad
  Porque no quiero decepcionar a mi familia
  #sprite:sophia_thinking
  Entiendo. y ¿Qué carrera escogiste esta vez?
   #sprite:joseph_neutral
  Derecho, No soy muy fanático de la idea, pero al menos
  #sprite:joseph_happy
  si quiero terminar una carrera, quiero hacerlo con algo que disfrute.
   #sprite:sophia_thinking
  ¿Y crees que puedes completarla?
  #sprite:joseph_happy
  Eso espero, estoy estudiando con dedicación. Me gusta leer, debatir, y ¿ayudar a la gente, supongo...?
  #sprite:joseph_neutral
   Y aunque no soy muy fanático de la idea de hacerme abogado,
  #sprite:joseph_happy
  si quiero terminar una carrera, quiero hacer algo que disfrute.
   #sprite:sophia_neutral
  Claro, ya estoy entendiendo tu postura. Pero a veces, para encontrar la salida, hay que cambiar de perspectiva.
  #sprite:joseph_happy
  A veces hablas como si te hubieras tragado una enciclopedia, ¿lo sabías?
  #sprite:sophia_euforic
  Y tú hablas como si pensar fuera un deporte extremo. 
  #sprite:sophia_euforic
  Oye Joseph, ¿a qué hora entras a clases hoy?
  #sprite:joseph_neutral
  En unas dos horas. ¿Por qué?
  #sprite:sophia_euforic
  #setvar:ruta:desicion_Decamino
  #setflag:Desicion_DeCamino 
  Dos horas es tiempo suficiente para cambiar el rumbo del día. ¡Vamos, camina!
  -> END
=== HandleDesicion_DeCamino ===
{
    - GetVar("ruta") == "arcade": -> Joseph_Arcade_Prologo
    - GetVar("ruta") == "biblioteca": -> Joseph_Bibloteca_Prologo
    - GetVar("ruta") == "desicion_Decamino": -> Desicion_DeCamino
}
=== Desicion_DeCamino ===
#setvar:ruta:desicion_Decamino
#sprite:sophia_thinking
Que deberia hacer con joseph
  + [Ir al arcade]
     #setvar:ruta:arcade
     Le dire a joseph de ir al arcade
     -> END
  + [Ir a la biblioteca]
     #setvar:ruta:biblioteca
     Le dire a joseph de ir a la Biblioteca
     -> END
  + [Mas tarde decir]
     #setvar:ruta:desicion_Decamino
     -> END
     
=== Habitacion_Bloqueada ===
#sprite:sophia_neutral
Aun no deberia ir a mi habitacion, tengo cosas que hacer.
-> END

=== Salida_Bloqueada ===
#sprite:sophia_neutral
Estoy muy cansada, no creo que quiera salir mas por hoy
-> END


=== Cama_Bloqueada ===
#sprite:sophia_neutral
Aun no me puedo ir a dormir, tengo cosas que hacer.
-> END


=== Reflexion_Final ===
#sprite:sophia_neutral
(Depuración) Esta es una reflexión genérica. No se encontró un nudo específico para la carta leída.
-> Final_Del_Juego

=== Final_Del_Juego ===
Es el momento de cerrar los ojos y dejar que el pensamiento descanse... hasta que volvamos a despertar. #fade_out
-> END


=== Confirmacion_Dormir ===
#sprite:sophia_neutral
Estoy exhausta... Siento que si me duermo ahora, este día finalmente habrá terminado. 
¿Estoy lista para dejarlo todo atrás por hoy?
+ [Sí, es momento de descansar]
    -> Reflexion_Final_Selector
+ [No, todavía hay algo que me inquieta]
    -> END

=== Reflexion_Final_Selector ===
{
    - GetVar("proxima_reflexion") == "reflexion_schopenhauer": -> reflexion_schopenhauer
    - GetVar("proxima_reflexion") == "reflexion_hegel": -> reflexion_hegel
    - GetVar("proxima_reflexion") == "reflexion_estoicos": -> reflexion_estoicos
    - GetVar("proxima_reflexion") == "reflexion_nietzsche": -> reflexion_nietzsche
    - else: -> Reflexion_Final
}


=== reflexion_schopenhauer ===
#sprite:sophia_thinking
(Depuración) Has leído la carta de Schopenhauer. Sophia reflexiona sobre la voluntad y el deseo...
-> Final_Del_Juego

=== reflexion_hegel ===
#sprite:sophia_thinking
(Depuración) Has leído la carta de Hegel. Sophia reflexiona sobre el progreso de la razón y el deber...
-> Final_Del_Juego

=== reflexion_estoicos ===
#sprite:sophia_thinking
(Depuración) Has leído la carta de los Estoicos. Sophia reflexiona sobre el control interno y la paz...
-> Final_Del_Juego

=== reflexion_nietzsche ===
#sprite:sophia_thinking
(Depuración) Has leído la carta de Nietzsche. Sophia reflexiona sobre el superhombre y la creación de valores...
-> Final_Del_Juego


