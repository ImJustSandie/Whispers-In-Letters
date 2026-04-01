
INCLUDE Joseph_Bibloteca.ink
INCLUDE Epilogos.ink
INCLUDE Joseph_Arcade..ink
INCLUDE Joseph_Arcade_Camino2.ink
INCLUDE Objects.ink




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
   
  { GetVar("ruta") == "desicion_prologo":
    -> Joseph1_Prologo_Reencuentro
}

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
  Dos horas es tiempo suficiente para cambiar el rumbo del día. ¡Vamos, camina!
  #setvar:ruta:desicion_Decamino
  -->END
  
  === HandleDesicion_DeCamino ===
#setflag:Desicion_DeCamino 
{ GetVar("ruta") == "arcade":
    -> Joseph_Arcade_Prologo
}

{ GetVar("ruta") == "biblioteca":
 -> Joseph_Bibloteca_Prologo
    
}
{ GetVar("ruta") == "desicion_Decamino":
    -> Desicion_DeCamino
}

-> END


=== Desicion_DeCamino ===

    #setvar:ruta:desicion_Decamino
    #sprite:sophia_thinking
    Que deberia hacer con joseph
  + [Ir al arcade]
     Le dire a joseph de ir al arcade
        #setvar:ruta:arcade
  -> END
  + [Ir a la biblioteca]
        Le dire a joseph de ir a la Biblioteca
         #setvar:ruta:biblioteca
  -> END
  + [Mas tarde decir]
  #setvar:ruta:desicion_Decamino
  -> END