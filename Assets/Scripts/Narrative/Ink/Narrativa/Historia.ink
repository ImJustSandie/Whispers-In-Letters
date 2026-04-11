
INCLUDE Joseph_Bibloteca.ink
INCLUDE Epilogos.ink
INCLUDE Joseph_Arcade..ink
INCLUDE Joseph_Arcade_Camino2.ink
INCLUDE Objects.ink
INCLUDE Joseph_Bibloteca_Camino2.ink
INCLUDE Prologo.ink





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
No sé qué estoy haciendo con mi vida.
Mi familia me ha dado todo, pero me siento solo.

#sprite:joseph_sad
Esta es la cuarta carrera que empiezo…
y siento que solo estudio por presión.

#sprite:sophia_sad
No te castigues tanto.
Cada quien encuentra su camino a su ritmo.

#sprite:joseph_neutral
Eso dicen… pero siento que el tiempo se me escapa.

#sprite:joseph_neutral
Se supone que soy libre, pero cada decisión pesa.

#sprite:sophia_thinking
Entonces, ¿por qué insistir en una carrera?

#sprite:joseph_sad
Porque no quiero decepcionar a mi familia.

#sprite:sophia_thinking
¿Y qué estás estudiando ahora?

#sprite:joseph_neutral
Derecho.
No me apasiona, pero…

#sprite:joseph_happy
me gusta leer, debatir… ayudar a la gente.

#sprite:sophia_neutral
Entonces no suena tan mal.

#sprite:sophia_thinking
Tal vez no necesitas cambiar de camino,
sino de perspectiva.

#sprite:joseph_happy
A veces hablas como enciclopedia.

#sprite:sophia_euforic
Y tú como si pensar doliera.

#sprite:sophia_euforic
Oye, ¿a qué hora entras?

#sprite:joseph_neutral
En dos horas.

#sprite:sophia_euforic
Perfecto.
Dos horas bastan para cambiar el rumbo del día.

#setvar:ruta:desicion_Decamino
¡Vamos, camina!
  -> Desicion_DeCamino
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
     -> Joseph_Arcade_Prologo
  + [Ir a la biblioteca]
     #setvar:ruta:biblioteca
     Le dire a joseph de ir a la Biblioteca
     -> Joseph_Bibloteca_Prologo
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
Lo sabía… ahora entiendo mejor por qué llevar a Joseph al Arcade fue lo correcto en ese momento.
#sprite:sophia_thinking
En un mundo que a veces parece puro sufrimiento, ese alivio momentáneo era la única libertad real que se podía alcanzar.
-> Final_Del_Juego

=== reflexion_hegel ===
#sprite:sophia_thinking
Esto me ha hecho ver que mi impulso de llevar a Joseph a la universidad no fue una imposición, sino un acto de verdadera libertad.
#sprite:sophia_thinking
No se trata de hacer lo que uno quiera, ni de seguir reglas porque sí, sino de hacer que nuestras acciones tengan un sentido dentro de la sociedad.
-> Final_Del_Juego

=== reflexion_estoicos ===
#sprite:sophia_thinking
Pensar en que hay cosas que se pueden controlar y otras que no me crea una dualidad.
#sprite:sophia_thinking
Es lo mismo que sucede con Joseph: no puede controlar lo que sus padres esperan de él, pero sí cómo se siente respecto a eso.
#sprite:sophia_thinking
Definitivamente hay que vencer esa dualidad y buscar el control. Eso fue lo que logré con Joseph al mantenerlo en la biblioteca
#sprite:sophia_thinking
no fue un castigo, sino una forma de enseñarle a dominar sus impulsos y su...
-> Final_Del_Juego

=== reflexion_nietzsche ===
#sprite:sophia_thinking
Es cierto… vivir condicionado por el “tú debes” de la obediencia y las tradiciones que nos imponen es una carga pesada que no nos deja elegir el camino.
#sprite:sophia_thinking
Hice bien al incitar a Joseph a romper sus límites. Él tenía que pensar en lo que realmente quiere, dejando de lado las verdades de otros para empezar a inventar su propia vida.
-> Final_Del_Juego


