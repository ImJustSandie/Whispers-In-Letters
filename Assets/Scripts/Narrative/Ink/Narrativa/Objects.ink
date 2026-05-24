=== lampara ===
#sprite:sophia_euforic
Si me acuerdo de esto, aun me duele la cabeza
-> END

===minotauro===
#sprite:Tablos
#sonido:Escencia
"La felicidad de tu vida depende de la calidad de tus pensamientos". — Marco Aurelio
->END

===minotauro_fallback ===
#sprite: sophia_sad
Siento que me olvidé de algo muy importante...
No creo tener sufience escencia.
->END

=== ArcadeBloqueadoPrologo ===
#sprite: sophia_thinking
Necesito ir al arcade primero a buscar mi control
-> END

=== LibroParque ===
#sprite:sophia_happy
Suputamadre me deje este libro aqui
Que bien que lo encontré
-> END

=== Libro2 ===
#sprite:sophia_happy
Asumakina, sabía que me faltaba otro libro
-> END

=== Libro2Fall===
#sprite: sophia_thinking
¿Este libro es mio?
Creo que no, mejor lo dejo aqui
-> END

// ==========================================
//Objetos- Habitacion de sofia
// ==========================================

===Televisor===
#sprite:sophia_neutral
A las 6:00 pm “La Venganza de Nova, película de terror”
A las 9:00 pm “Mire maestro, el documental de arte”
A las 12:00 am “Como no quedarse calvo, siendo ingeniero en Multimedia”
->END

===Biblioteca_Sophia===
#sprite:sophia_neutral
Sophia en el país de las maravillas.
Sophia y la piedra filosofal.
Sophia y la balada de la tortuga y los koalas.
Sophia y la comunidad filosófica.
#sprite: sophia_thinking
La promoción decía ‘pague 4 y lleve 5’, 
pero no sabía que se referían a los cactus.
->END

// ==========================================
// Objetos-Arcade
// ==========================================

===Maquinarecreativa1===
#sprite:sophia_happy
Aquí puedo jugar: The legend of Sophia
->END

===Maquinarecreativa2===
#sprite:sophia_happy
Aquí puedo jugar: The league of Sophias
->END

===Maquinarecreativa3===
#sprite:sophia_happy
Esta máquina está en otro Arcade
->END

===Maquinarecreativa4===
#sprite:sophia_happy
En esta puedo jugar Whispers in Letters
->END

===Animatronicos===
#sprite:sophia_euforic
Son Alfredo y el Bonifacio Amarillo mis favoritos.
->END

===Mesacomida1===
#sprite:sophia_thinking
Quien pidió pizza con piña, esto solo le gusta a la gente rara.
->END

===Mesacomida2===
#sprite:sophia_euforic
De comer estas hamburguesas, no se nos ven los pies.
->END

===AvisoCinta===
#sprite: sophia_neutral
Esta zona esta bloqueada, supongo que está en mantenimiento
-> END

// ==========================================
// Objetos-Parque
// ==========================================
===Fuente==
#sprite:sophia_neutral
Ya no quedan monedas en la fuente, misteriosamente han desaparecido.
->END
===AvisoArcade===
#sprite:sophia_neutral
Arcade Playtown
->END

===AvisoBiblioteca===
#sprite:sophia_neutral
 Biblioteca Miguel Olivares
 -> END
 
===AvisoProhibidopasar===
#sprite:sophia_neutral
Prohibido pasar
#sprite: sophia_thinking
Hace dos años que esto debió haberse inaugurado. 
->END

===AvisoCasa===
#sprite: sophia_neutral
Conjunto Residencial Diabulus Berchello
-> END

// ==========================================
// Objetos-Bibloteca
// ==========================================

===Estantelibros1===
#sprite:sophia_neutral
Hasta aquí llega el olor a viejo con estos libros.
->END

===Estantelibros2===
#sprite:sophia_neutral
Esto está lleno de libros, de los cuales ninguno voy a leer.
->END

===EstanteLibros3===
#sprite:sophia_neutral
No veo nada de interés aquí.
-> END

===Pergamino===
#sprite:sophia_neutral
Un pergamino, en esta época del año, a esta hora del día, en esta parte
del mundo y ubicada específicamente en esta biblioteca. 
->END

===Computador1===
#sprite:sophia_neutral
Estas computadoras son de la prehistoria
->END

===Computador2===
#sprite:sophia_neutral
Alguien dejó su historial de búsqueda abierto, será mejor no mirarlo.
->END

// ==========================================
// Patos
// ==========================================

===Pato_Parque1===
#sprite: sophia_happy
!Un pato! es muy bonito, me lo llevaré como recuerdo
-> END

===Pato_Parque2===
#sprite: sophia_happy
Este pato se ve muy tranquilo, ¡Me traerá tranquilidad en mi vida!
-> END

===Pato_Arcade===
#sprite: sophia_neutral
#sonido: sophia_suspira
Este pato se ve algo amargado, !Un arcade no es un sitio para un pato asi!
-> END

===Pato_Biblioteca===
#sprite: sophia_happy
#sonido: sophia_risa
!Este pato se ve muy concentrado! Estoy segura que me ayudará a estudiar
-> END

===Pato_Habitacion===
#sprite: sophia_neutral
#sonido: sophia_suspira
Siempre me sorprenderá lo mucho que me parezco a este pato...
Creo que quiero guardarlo por ahora
-> END


// ==========================================
// NPC
// ==========================================

=== NPC_01 ===
#sprite: sophia_euforic
¡No puede ser! Otra vez se quedó a uno de vida y me morí
-> END

=== NPC_02 ===
#sprite: sophia_euforic
Ojalá el autor de este libro estuviera vivo para que me explicara qué rayos quiso decir.
-> END

=== NPC_03 ===
#sprite: sophia_euforic
Negrito, eso es una pregunta capciosa, tienes que leerte el libro.
-> END

=== NPC_04 ===
#sprite: sophia_euforic
! Si, Gane!, voy 1 victoria y 99 derrotas el día de hoy
-> END


// ==========================================
// ACTIVIDADES ARCADE
// ==========================================

===Activ_Pizza_Arcade===
#sprite:sophia_happy
#sonido:sophia_risa
Aquí está la pizza. Joseph se pondrá contento.
#setflag:Activ_Arcade_1
-> END

===Activ_Pizza_Arcade_Fail===
#sprite:sophia_thinking
Esta pizza se ve deliciosa..
-> END

===Maquina_Juego_Arcade_Fail===
#sprite: sophia_neutral
Este juego se ve interesante, creo que me podría gustar...
-> END

===Activ_Maquina_Juego_FB===
#sprite:sophia_thinking
Joseph aún no me ha pedido que juegue con él. Mejor esperar a que me lo pida.
-> END

===Activ_Maquina_Juego===
{
    - GetVar("ruta") == "desicion3":
        #sprite: sophia_happy
        Una partida más para entender bien lo que Joseph ve en esto.
        #setflag:Activ_Arcade_3
        -> END
    - GetVar("ruta") == "desicion2":
        #sprite:sophia_happy
        Voy a jugar una partida para entender lo que Joseph siente aquí.
        #setflag:Activ_Arcade_2
        -> END
    - else:
        #sprite:sophia_thinking
        No es momento de jugar. Joseph me necesita.
        -> END
}

// ==========================================
// ACTIVIDADES BIBLIOTECA
// ==========================================

===Activ_Libro_Leyes===
#sprite:sophia_happy
Aquí está el libro de derecho. Joseph lo pidió, esto le ayudará a familiarizarse con la materia.
#setflag:Activ_Biblio_1
-> END

===Activ_Libro_Leyes_Fail===
#sprite:sophia_neutral
Este libro es sobre introducción al derecho
-> END


===Activ_Estudio_Biblioteca_FB===
#sprite:sophia_thinking
La república, el príncipe, leviatán... Es curioso como los filosofos hablaban también mucho sobre el estado y política de su época.
-> END

===Activ_Estudio_Biblioteca===
{
    - GetVar("ruta") == "decision_biblioteca_3":
        #sprite:sophia_neutral
        Repasaré estos apuntes una vez más antes de decidir.
        #setflag:Activ_Biblio_3
        -> END
    - GetVar("ruta") == "decision_biblioteca_2":
        #sprite:sophia_neutral
        Revisaré estos apuntes para entender mejor cómo ayudar a Joseph con su estudio.
        #setflag:Activ_Biblio_2
        -> END
    - else:
        #sprite:sophia_thinking
        No tengo por qué estudiar esto ahora. Mejor acompañar a Joseph primero.
        -> END
}
