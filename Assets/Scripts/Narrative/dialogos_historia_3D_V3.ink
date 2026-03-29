// ==========================================
// DIÁLOGOS - Historia 3D V3
// Ink script para integración en Unity
// ==========================================

-> prologo


// ==========================================
// PRÓLOGO
// ==========================================

=== prologo ===
// Sophia recibe una carta sin remitente a través de un servicio de mensajería,
// la abre, la lee y se queda pensativa.
// Sophia va caminando hacia la universidad y ve a su compañero Joseph
// sentado en una banca, cabizbajo. Lo saluda.

Sophia: Hola Joseph, que milagro en verte por aquí tan temprano.
Joseph: Ah, hola Sophia, que bueno encontrarte.
Sophia: Igual yo. Pero… te noto algo preocupado. ¿estás bien? ¿pasó algo en tu casa?
Joseph: La verdad no. Es lo mismo de siempre, pero más pesado... ¿Seguro que tienes tiempo para escuchar mis dramas?

* [No, no tengo tiempo, pero espero que te vaya bien]
    Joseph: No hay problema, nos vemos
    -> prologo
* [Para los amigos siempre hay tiempo, cuéntame que te tiene tan preocupado]
    -> dialogo_inicial


// ==========================================
// DIÁLOGO INICIAL
// ==========================================

=== dialogo_inicial ===
Sophia: Para los amigos siempre hay tiempo, cuéntame que te tiene tan preocupado.
Joseph: Gracias por escucharme.
Joseph: Es que... no sé qué estoy haciendo con mi vida. Mi familia siempre me ha dado todo lo económico, pero me siento solo, ¿sabes? Esta es la cuarta carrera que empiezo y no sé si voy a terminarla. Siento que solo estudio por la presión de ellos y por miedo a fracasar otra vez.
Sophia: No digas eso, no te des tan duro. Todos tenemos nuestro ritmo y talento, solo que a veces tarda en aparecer. Somos jóvenes, este es el momento para cometer errores y volverlo a intentar, ¿no crees?
Joseph: Eso dicen todos, pero yo siento que el tiempo se me escapa. Dicen que soy "libre" de elegir, pero cada elección se siente como una celda nueva.
Sophia: ¿Por qué estás tan empeñado en completar una carrera universitaria?
Joseph: Porque no quiero decepcionar a mi familia
Sophia: Entiendo. y ¿Qué carrera escogiste esta vez?
Joseph: Derecho, No soy muy fanático de la idea, pero al menos, si quiero terminar una carrera, quiero hacerlo con algo que disfrute.
Sophia: ¿Y crees que puedes completarla?
Joseph: Eso espero, estoy estudiando con dedicación. Me gusta leer, debatir, y ¿ayudar a la gente, supongo...?
Joseph: Y aunque no soy muy fanático de la idea de hacerme abogado, si quiero terminar una carrera, quiero hacer algo que disfrute.
Sophia: Claro, ya estoy entendiendo tu postura.
Sophia: Oye Joseph, ¿a qué hora entras a clases hoy?
Joseph: En unas dos horas. ¿Por qué?
Sophia: Dos horas es tiempo suficiente para cambiar el rumbo del día
// Sophia piensa: ¿Tendrá esto que ver con la carta sobre la libertad que recibí hoy?
-> eleccion_camino


// ==========================================
// ELECCIÓN DE CAMINO PRINCIPAL
// ==========================================

=== eleccion_camino ===
* [conozco un lugar para que encuentres tu libertad]
    -> camino_1_inicio
* [Vamos a la biblioteca para que estudiemos juntos]
    -> camino_2_inicio


// ==========================================
// CAMINO 1: LA TENTACIÓN Y EL EXCESO
// ==========================================

=== camino_1_inicio ===
Sophia: Oye, he estado pensando en lo que me dijiste y.… a veces nos exigimos demasiado por lo que otros esperan. ¿Y si hoy mandas todo a volar y te relajas?
Joseph: ¿A qué te refieres con "mandar todo a volar"?
Sophia: Pues a relajarte, a no pensar tanto y simplemente vivir.
Joseph: No estoy de humor para una caminata filosófica por el parque.
Sophia: Ven conmigo, conozco un lugar que es como un refugio, donde nadie te va a juzgar ni a pedir resultados. Es un sitio para que seas tú, sin la voz de tus papás o de la universidad en la cabeza. Tómalo, solo por hoy. Úsalo para encontrarte.
Joseph: Suena peligrosamente tentador... pero ¿y la clase de hoy?
Sophia: La clase estará ahí mañana, y el próximo semestre también. Pero hoy, Joseph, hoy necesitas respirar sin pedir permiso.
Joseph: No necesito un refugio, necesito un título para que mi padre deje de mirarme como si fuera un error.
Sophia: Ese título es tu jaula. Vamos a un lugar donde puedas ser tú, sin la voz de otros diciéndote que hacer.
Joseph: Es una locura... si pierdo esta clase, el profesor me pondrá en su lista negra.
Sophia: El profesor ni siquiera recordará tu nombre en un año. Pero tú recordarás este día como el momento en que dijiste "basta".
Joseph: No lo sé... esto se siente muy mal, Sophia. Muy mal.
Sophia: Así no es, ven conmigo. Confía en mí por una sola vez.
Joseph: Si esto sale mal... si alguien me ve fuera del campus a esta hora...
Sophia: Nadie lo va a saber. Seremos invisibles. Solo por hoy, Joseph. Solo por esta vez intenta no pensar en las consecuencias.
Joseph: Está bien. Total, ya me siento como un extraño en mi propia vida.
Sophia: Esa es la actitud que quería ver. Vamos, antes de que el arrepentimiento te alcance en la puerta del salón.
Joseph: Solo por hoy, Sophia. Solo por esta vez intentaré no pensar en las consecuencias.
// Sophia y Joseph llegan al lugar donde están los videojuegos/arcade
Sophia: ¡Bienvenido al templo del presente! Aquí el placer es la única norma.
Joseph: Ay Caramba... esto me encanta, es el paraíso.
Sophia: ¡Mira ese juego! Se ve genial.  Olvida la carrera y el "llegar a ser alguien". Pásatela bien y ya.
Joseph: Hace siglos que no jugaba uno de estos. Mi padre siempre decía que esto era perder el tiempo.
Sophia: ¡Pruébalo! Deja que tus manos decidan por ti. No pienses en el examen de mañana, solo en el oponente de la pantalla.
Joseph: Es increíble, …Me siento como si estuviera en otra dimensión. Una donde no decepciono a nadie. Podría quedarme aquí horas... o días enteros si me lo permitieran.
Sophia: ¿Ves? La libertad no estaba escondida en los libros, estaba en este lugar.
Joseph: Gracias por traerme, Sophia.
Sophia: Disfrútalo, Joseph. Te lo mereces después de tanto tiempo viviendo para los demás.
Joseph: Voy a jugar mi primera partida.
// Joseph juega por primera vez
-> decision_arcade_1


// ------------------------------------------
// DECISIÓN A: Primer momento en el arcade
// ------------------------------------------

=== decision_arcade_1 ===
// Sophia piensa en qué decisión debe tomar ahí
* [Motivar a Joseph - Debe romper esa rutina y encontrar un respiro]
    -> arcade_motivar
* [Mejor deja así - Vete a la universidad a estudiar]
    -> arcade_dejar_ir


// ------------------------------------------
// CAMINO 1 / OPCIÓN 1: Motivar a Joseph
// ------------------------------------------

=== arcade_motivar ===
// Joseph continúa jugando descontroladamente
Joseph: Mira Sophia. ¡Le acabo de ganar al récord de la semana! ¡Soy el rey de este paraíso!
Joseph: Quiero ver qué hay en el siguiente nivel, y en el siguiente, y en el que sigue después de ese. Quiero llegar al final. No importa si tardo toda la noche. Quiero ver qué hay detrás del último nivel.
Joseph: Sophia, mira esto... ¡el puntaje está subiendo tan rápido.
Sophia: No sabía que eras tan bueno, deberías ser un streamer de videojuegos o un jugador profesional.
Sophia: ¿Ves? En la universidad te hacían sentir como un fracasado, pero aquí eres el arquitecto de tu propia victoria.
Joseph: ¡Increíble…Tres niveles en menos de diez minutos!
Sophia: No te detengas ahora, Joseph. El mundo exterior merece conocer este talento.
Joseph: Me siento invencible... es como si toda la frustración de estos años se estuviera convirtiendo en energía para mis dedos.
Sophia: Eso es lo que pasa cuando dejas de ser quien tus padres quieren y te conviertes en quien realmente eres: un conquistador.
Joseph: ¡Otra partida! Siento que ahora mismo nada puede salir mal.
Sophia: Hazlo, Joseph. Explora, juega, sé el dueño de este pequeño universo.
-> decision_arcade_2


// ------------------------------------------
// DECISIÓN B: Después de jugar más
// ------------------------------------------

=== decision_arcade_2 ===
// Sophia piensa en qué decisión debe tomar ahí
* [Aquí encontraste la razón de vida - No eres un fracasado, solo tenías un enfoque diferente]
    -> arcade_vocacion
* [Ya es mucho juego - Esto no aporta nada a la vida]
    -> arcade_dejar_ir


// ------------------------------------------
// CAMINO 1 / OPCIÓN 1-1: Vocación como gamer
// ------------------------------------------

=== arcade_vocacion ===
// Joseph está frente a una de las máquinas de competencia. La motivación que le
// dio Sophia ha encendido una chispa diferente; ya no quiere jugar por diversión,
// sino con una mirada analítica y profesional.
Joseph: Me gusta este lugar... no hay preguntas difíciles, solo obstáculos que puedo saltar o destruir. aquí es donde realmente pertenezco. Además, acabo de descubrir algo, no soy un mal estudiante, soy un profesional fuera de lugar
Sophia: Te lo dije. Solo necesitabas un entorno diferente. Tienes una coordinación asombrosa. He visto a gente jugar años y no tienen tu precisión.
Joseph: Es que esto no es un juego para mí ahora. Es arquitectura de datos en movimiento. Es estrategia pura.
Sophia: Parece que por fin encontraste un idioma que tu cerebro sí quiere hablar.
Joseph: Por primera vez en mi vida, no me siento como un "fracasado" que decepciona a sus padres.
Sophia: Me alegra que lo veas así, Joseph. Esa es la confianza que te faltaba.
Joseph: Me siento... útil. Siento que, si me dedico a esto profesionalmente, podría ser el mejor. No uno del montón, sino el mejor.
Joseph: Se acabó el intentar encajar en un molde que me rompe. Voy a construir mi propio molde aquí mismo.
Sophia: Me pregunto si esta "libertad" que encontraste no es solo otra forma de presión que te estás imponiendo.
// Joseph ha decaído por un momento pensando en sus padres, pero Sophia,
// al verlo tan infeliz fuera de las máquinas, decide impulsarlo a seguir
// con su nueva vocación.
Joseph: Quizás esto es una locura, Sophia... mis padres nunca aceptarán que esta es mi nueva vida.
Sophia: Mírate Joseph; estas otra vez ansioso.
Joseph: Es que en este lugar no soy el fracasado que ellos ven, aquí soy alguien importante.
Sophia: Entonces no dejes que el miedo de ellos apague tu chispa; vuelve a ese juego.
Joseph: ¿Me estás diciendo que ignore las llamadas que mis padres me han estado haciendo y me quede?
Sophia: Te digo que prefiero verte como un jugador apasionado que como un ente.
Joseph: Tienes razón, si vuelvo ahora al salón, el torneo aún no habrá terminado.
Sophia: Ve y demuestra que esta es tu verdadera razón de ser, no una simple escapatoria.
Joseph: Gracias, Sophia... es la primera vez que siento que alguien valida lo que realmente soy.
Sophia: Solo no mires atrás; corre antes de que la duda te alcance de nuevo.
// Epílogo: Joseph está convencido: ser profesional es su salida definitiva al fracaso,
// pero Sophia nota que su euforia es solo una nueva cadena.
-> epilogo_schopenhauer


// ------------------------------------------
// CAMINO 1 / OPCIÓN 1-2 y OPCIÓN 2:
// Salir del arcade (nodo compartido)
// Alcanzado desde Decisión A[2] o Decisión B[2]
// ------------------------------------------

=== arcade_dejar_ir ===
// Sophia ha decidido que es mejor salir de ese lugar de videojuegos
Sophia: Estuvo genial, pero... ya nos tenemos que ir Joseph. El mundo real nos está esperando.
Joseph: cálmate, solo nos estábamos dando un respiro.
Sophia: No vas a huir, vas a volver con la cabeza fría y el control que recuperaste aquí.
Joseph: Es verdad, una partida no soluciona mis problemas, solo los pospone.
Sophia: Así se habla; recoge tus cosas y vamos directo a clase antes que empiece la clase.
Joseph: Gracias por sacarme. necesitaba este golpe de realidad.
Sophia: La libertad también es saber cuándo terminar la diversión para cumplir con el deber.
Joseph: Vamos, si corremos llegaremos a tiempo.
-> decision_salida_arcade


// ------------------------------------------
// DECISIÓN C: Momento de salida del arcade
// ------------------------------------------

=== decision_salida_arcade ===
// Sophia piensa en qué decisión debe tomar ahí
* [Te voy a dar otra oportunidad - Tienes potencial para esto de los videojuegos]
    -> arcade_salida_forzada
* [Que mala idea tuve al llevarte a ese lugar - Es hora de abandonar esta aventura]
    -> arcade_salida_forzada


// ------------------------------------------
// CAMINO 1 / SALIDA FORZADA
// ------------------------------------------

=== arcade_salida_forzada ===
// Sophia tiene un momento de reflexión y arrepentimiento: se da cuenta que si
// deja a Joseph en ese lugar no lo está liberando, sino destruyendo, entonces lo
// rescata de su desorden emocional para encaminarlo hacia la responsabilidad.
// Joseph está sumergido en el juego y no quiere irse.
// Sophia pierde la paciencia y lo obliga a salir.
Sophia: ¡Joseph, suelta ese mando ahora mismo! Llevamos horas aquí y ya perdiste la noción del tiempo.
Joseph: ¡Solo cinco minutos más! Estoy a punto de romper otro récord, no puedes interrumpir ahora.
Sophia: ¡A quién le importa un récord en una máquina!  Hace una hora que me dijiste que ya terminabas, no voy a permitir que te hundas en este antro
Joseph: ¡Déjame en paz, Sophia! Aquí es donde soy feliz, no en esas clases.
Sophia: ¡Se acabó! Te levantas de esa silla o te dejo aquí solo.
Joseph: ¡Me estás arruinando el momento! ¡Casi lo tenía!
Sophia: Te estoy salvando la vida de convertirte en un fantasma de este lugar; camina hacia la puerta.
Joseph: Eres una dictadora... me traes para distraerme y ahora me arrastras como a un niño.
Sophia: Alguien tiene que ser el adulto hoy; muévete antes de que llame a tus padres.
Joseph: Está bien, ya voy... pero no esperes que te agradezca este mal rato.
Sophia: Estás exagerando. Te traje para que vieras que la evasión no es la respuesta, pero ahora es momento de que veas cuál es la verdadera salida. Escucha, Joseph: la libertad no es hacer lo que uno quiere en cada momento, eso es solo un capricho.
Sophia: ¡Muévete, Joseph! Tienes clase y hay que ser responsables. Tu futuro no se va a construir solo en un videojuego.
Joseph: ¡Oye, no me empujes!, yo puedo salir solo.
// Sophia observa a Joseph confundido y se siente mal por haberlo llevado
// al salón de los videojuegos.
Sophia: Joseph, mírame... lo siento mucho, nunca debí sugerirte que viniéramos a este lugar.
Joseph: No es tu culpa, yo fui el que se dejó llevar por los videojuegos.
Sophia: Fui irresponsable; pensé que te ayudaría y solo logré confundirte más sobre tu futuro.
Joseph: Me hiciste creer que podía ser otra persona, y ahora la realidad me golpea el doble.
Sophia: Me siento fatal viendo cómo te cuesta volver a la normalidad después de esto.
Joseph: Es que aquí todo era fácil, y afuera todo vuelve a ser tan difícil.
Sophia: Perdóname por darte una falsa salida; las soluciones reales no están en un videojuego.
Joseph: Quizás solo querías verme sonreír, pero ahora esa sonrisa me parece una mentira.
Sophia: Prometo que de ahora en adelante te ayudaré a enfrentar tu realidad y a salir adelante.
Joseph: No te culpes tanto, al menos ahora sé que mi lugar, por difícil que sea, está allá afuera.
// Sophia y Joseph salen del salón de videojuegos.
// Epílogo: Sophia observa a Joseph quien ha entendido decidido que su lugar
// está en la universidad, aceptando la disciplina sobre el placer inmediato.
-> epilogo_hegel


// ==========================================
// CAMINO 2: EL DESPERTAR DE LA CONCIENCIA
// ==========================================

=== camino_2_inicio ===
Sophia: Joseph deberías aprovechar para repasar o prepararte. Si quieres que esta vez sea diferente, necesitas comprometerte de verdad.
Joseph: ¿Pretendes que llene mi vacío existencial con más libros? Sophia, eso es lo que me ha estado asfixiando toda la vida.
Sophia: Te asfixia porque lo ves como una imposición, no como una herramienta para tu propia liberación.
Joseph: No entiendo cómo leer sobre contratos o leyes puede hacerme sentir "libre".
Sophia: La ignorancia es la mayor de las cárceles. Ven conmigo, vamos a la biblioteca.
Joseph: Es el lugar que más odio en este campus. El silencio allí dentro me grita mis fracasos.
Sophia: Pues hoy vamos a entrar para que ese silencio se convierta en concentración.
Joseph: He pasado por tres carreras antes que esta, y en todas las bibliotecas me sentí como un intruso.
Sophia: Esta vez es diferente porque no vas a estar solo, vamos a vencer ese miedo juntos. Míralo como un entrenamiento para tu espíritu. Vamos, Joseph, antes de que pierdas el valor.
// Sophia y Joseph caminan hacia la biblioteca
-> decision_biblioteca_1


// ------------------------------------------
// DECISIÓN D: Primer momento en la biblioteca
// ------------------------------------------

=== decision_biblioteca_1 ===
// Sophia piensa en qué decisión debe tomar ahí
* [Diseñar una estrategia de estudio para Joseph - Él no es un estudiante de molde]
    -> biblioteca_estrategia
* [Mejor dejemos esto así - Me doy por vencida, tienes serias limitantes académicas]
    -> biblioteca_opcion2


// ------------------------------------------
// CAMINO 2 / OPCIÓN 1: Diseñar estrategia
// ------------------------------------------

=== biblioteca_estrategia ===
// En la biblioteca, Joseph se ve frustrado, no entiende los libros,
// tiene dificultades para comprender y procesar la información.
// Sophia piensa en un método que se adapte a Joseph.
Joseph: Lo sabía. Siempre me pasa. No soy brillante, Sophia, mi cerebro no funciona como el tuyo o el de mis compañeros.
Sophia: No se trata de ser un genio, pero noto que te cuesta conectar conceptos que son fundamentales.
Joseph: Hay personas que entienden todo muy rápido, mientras que yo puedo leerlo hasta diez veces y mi mente sigue siendo un lienzo en blanco.
Sophia: Es cierto que tu velocidad de procesamiento es distinta, te enredas en la terminología técnica.
Joseph: Es humillante darme cuenta, frente a ti, de que quizás mi falta de éxito no es solo flojera, sino falta de capacidad.
Sophia: No digas eso, pero sí es evidente que el camino académico va a ser una cuesta dura para ti. Pero tal vez quizás necesitemos una estrategia diferente, porque el método tradicional te está costando.
Joseph: pero qué estrategia puede servirle a alguien que olvida el inicio del párrafo al llegar al final
Sophia: Olvida la memoria literal; lo que necesitas es asociar cada concepto a algo que puedas tocar y a ejemplos en lugar de esquemas.
Joseph: ¿Crees que fragmentar la información realmente me ayude a avanzar?
Sophia: Es la única forma: avanzar lento pero seguro.
Joseph: Me tomará el triple de tiempo que a los demás terminar.
Sophia: No compitas contra el reloj de otros, sino contra tu propio ritmo.
Joseph: Está bien, nada se pierde, peor es no intentarlo.
-> decision_biblioteca_2


// ------------------------------------------
// DECISIÓN E: Después de intentar la estrategia
// ------------------------------------------

=== decision_biblioteca_2 ===
// Sophia piensa en qué decisión debe tomar ahí
* [Joseph ha mejorado, pero hay que ser constante y disciplinado - Hay mucho camino por recorrer]
    -> biblioteca_disciplina
* [No avanzaste casi nada - Este es un reto muy duro]
    -> biblioteca_opcion2


// ------------------------------------------
// CAMINO 2 / OPCIÓN 1-1: Disciplina y constancia
// ------------------------------------------

=== biblioteca_disciplina ===
// Joseph empieza a aplicar el método y, aunque le cuesta, es consciente
// que debe ser disciplinado y constante.
// Su libertad está en ser el dueño de su propio esfuerzo.
Joseph: Llevo dos horas con la misma página, pero no he sentido ganas de rendirme ni una sola vez.
Sophia: Eso es porque has dejado de pelear contra tu naturaleza y has aceptado tu ritmo.
Joseph: Antes creía que ser libre era aprobar sin esfuerzo; ahora veo que es mantenerme firme aquí.
Sophia: La disciplina es la forma más pura de libertad, Joseph; tú decides no ser esclavo de tu pereza.
Joseph: Mi mente sigue siendo un terreno difícil, pero mi voluntad es la que lleva las riendas ahora.
Sophia: Has entendido que no controlas tu capacidad innata, pero sí controlas cuánto tiempo te quedas sentado.
Joseph: Es extraño, al aceptar que soy lento, el peso del juicio de los demás simplemente desapareció.
Sophia: Es que no hay que luchar contra la corriente, se trata de no rendirse y enfocarse en lo que depende de ti y soltar lo que no se puede cambiar.
Joseph: Seguiré con el siguiente párrafo; no busco el éxito rápido, busco la constancia de mi carácter.
Sophia: Esa constancia es tu verdadera victoria; hoy eres el dueño de tus actos, no de tus miedos.
// Joseph tiene un momento de fatiga física, pero Sophia lo guía para que
// su mente se imponga sobre el cansancio.
Joseph: Sophia, siento que la cabeza me va a estallar... ¿y si simplemente no nací para esto?
Sophia: El cansancio es solo una impresión de tus sentidos; no dejes que dicte lo que vas a hacer.
Joseph: Me dan ganas de cerrar el libro y volver a la comodidad de no intentar nada.
Sophia: Recuerda que la comodidad es otra forma de prisión; rendirse es volver a ser un esclavo.
Joseph: Tienes razón, si me rindo ahora, le doy la razón a todos los que dijeron que no podía.
Sophia: No lo hagas por ellos, hazlo por la dignidad de tu propia voluntad; mantente en tu puesto.
Joseph: Es una lucha constante entre lo que mi cuerpo quiere y lo que mi razón me dicta.
Sophia: Esa lucha es la que te hace un hombre libre; cada minuto extra aquí es una cadena rota.
Joseph: Está bien, tomaré un poco de agua y seguiré; no voy a permitir que el cansancio me venza.
Sophia: Así se habla; tu libertad no está en el diploma, sino en no rendirte ante tu propia debilidad.
// Epílogo: La libertad no reside en cambiar el intelecto, sino en aceptar
// la realidad y cumplir con el deber sin quejas.
-> epilogo_estoicos


// ------------------------------------------
// CAMINO 2 / OPCIÓN 1-2 y OPCIÓN 2:
// Rendirse (nodo compartido)
// Alcanzado desde Decisión D[2] o Decisión E[2]
// ------------------------------------------

=== biblioteca_opcion2 ===
// Joseph y Sophia continúan en la sala de lectura de la biblioteca,
// miran un libro con desesperanza. Ambos aceptan que el abismo entre
// su capacidad y el requisito académico es demasiado ancho.
Joseph: Es inútil, Sophia; por más que lo intente, estas palabras son solo ruido para mí.
Sophia: Veo el esfuerzo en tus ojos, pero también veo que tu mente se bloquea por completo.
Joseph: No es falta de ganas, es que simplemente no tengo las herramientas para procesar esto.
Sophia: A veces, insistir en lo imposible no es virtud, sino una forma de tortura innecesaria.
Joseph: Mis compañeros ya terminaron el manual y yo no he pasado de la introducción.
Sophia: Quizás estamos forzando una cerradura que simplemente no tiene llave para ti.
Joseph: Es doloroso admitirlo, pero mi límite académico está aquí; no puedo dar ni un paso más.
Sophia: Forzarlo más solo te llevará a un colapso; a veces, retirarse es la opción más cuerda.
Joseph: Tienes razón, voy a cerrar este libro; no voy a seguir rompiéndome contra él.
Sophia: Guardemos las cosas; no tiene sentido pelear una batalla que ya está perdida de antemano.
-> decision_biblioteca_3


// ------------------------------------------
// DECISIÓN F: Frente al límite académico
// ------------------------------------------

=== decision_biblioteca_3 ===
// Sophia piensa en qué decisión debe tomar ahí
* [No me debo rendir tan fácil - Este reto no me va a vencer]
    -> biblioteca_ultimo_esfuerzo
* [Confirmado, este es un caso perdido - Es mejor que no gastemos energías en esto]
    -> biblioteca_rendicion_final


// ------------------------------------------
// CAMINO 2 / OPCIÓN 2-1: Último esfuerzo fallido
// ------------------------------------------

=== biblioteca_ultimo_esfuerzo ===
// Sophia y Joseph intentan un último esfuerzo,
// pero Joseph retrocede en lugar de avanzar. La frustración es total.
Sophia: Joseph, deja de mirar el reloj. Llevamos media hora y no has pasado de la introducción.
Joseph: Es que me duele la cabeza. Estas frases son tan largas y olvido el inicio antes de llegar al punto final.
Sophia: Te traje aquí para estudiar, no para quejarte de los autores.
Joseph: ¡No es queja, es incapacidad! Siento que me falta un tornillo para este tipo de pensamiento lógico.
Sophia: Todos tenemos capacidad, pero me estoy dando cuenta de que tu comprensión lectora es muy limitada para este nivel.
Joseph: ¡Lo sé! Por eso fracasé en las otras tres carreras. No soy "inteligente". No sé qué hacemos aquí, tal vez perder el tiempo viendo cómo me frustro.
Sophia: No es el sistema, eres tú; este reto te queda demasiado grande y no puedes superarlo.
Sophia: Hay que reconocer que no avanzamos, ese es el primer paso para dejar de sufrir por esto.
-> biblioteca_rendicion_final


// ------------------------------------------
// CAMINO 2 / RENDICIÓN FINAL
// ------------------------------------------

=== biblioteca_rendicion_final ===
// Sophia toma la decisión final de detenerse. Entiende que seguir
// insistiendo es un desperdicio de energía vital para ambos.
Sophia: Joseph, detente. No vamos a leer ni una sola línea más de este libro.
Joseph: ¿Te rindes conmigo? Pensé que eras la única que creía que podía lograrlo.
Sophia: Creer en lo imposible es un error; gastar más energía aquí es quemarte por nada.
Joseph: Entonces, ¿soy oficialmente un caso perdido para la universidad?
Sophia: Eres un caso perdido para esta carrera, y seguir insistiendo es un desperdicio de tu vida.
Joseph: Se siente como un vacío inmenso, pero también como si me quitaras un camión de encima.
Sophia: Tu energía debe ir a otro lugar donde no tengas que luchar contra tu propia naturaleza.
Joseph: Tienes razón; estoy agotado de pelear contra una pared que no se va a mover.
Sophia: Vámonos de aquí; esta biblioteca no tiene nada más que ofrecerte más que frustración.
Joseph: Adiós a los libros... es hora de aceptar que este camino nunca fue el mío.
// Joseph se levanta de la mesa y deja el libro abierto, como quien abandona
// un campo de batalla donde ya no queda nada por lo que luchar.
// Sophia y Joseph se van de la biblioteca.
-> epilogo_nietzsche


// ==========================================
// EPÍLOGOS FILOSÓFICOS
// ==========================================

=== epilogo_schopenhauer ===
// Sophia recibe una carta con la postura de Schopenhauer
Sophia: (Carta - Schopenhauer) "Joseph cree que es libre, pero su voluntad irracional lo domina. Puede hacer lo que quiera, pero no puede dejar de desear nuevas distracciones. Es preso de su propio aburrimiento".
-> epilogo_final

=== epilogo_hegel ===
// Sophia recibe una nueva carta con la postura de Hegel
Sophia: (Carta - Hegel) "La libertad no es el libre albedrío del capricho, sino la realización del individuo en sus instituciones". No se trata de luchar contra la corriente del sistema, sino encontrar tu vocación dentro de él y cumplir un rol, alinearse con la razón universal".
-> epilogo_final

=== epilogo_estoicos ===
// Sophia recibe otra carta con la postura de los filósofos estoicos
Sophia: (Carta - Estoicos) "Los seres humanos no son libres respecto a lo que les sucede, pero sí lo son respecto a cómo reaccionan ante ello". Se trata de llegar a la Ataraxia, es decir, la imperturbabilidad del alma ante la adversidad.
-> epilogo_final

=== epilogo_nietzsche ===
// Llega una carta para Sophia con la postura filosófica de Nietzsche
Sophia: (Carta - Nietzsche) "La libertad es un proceso de creación de uno mismo. El individuo da forma a su carácter. La libertad es una afirmación continua del propio ser".
-> epilogo_final


// ==========================================
// EPÍLOGO FINAL
// ==========================================

=== epilogo_final ===
// Sophia se encuentra en su casa y le llegan tres cartas.
// Son las de los filósofos que no se escogieron.
// Todas dicen: ¿Por qué no escogiste otro camino?
// Sophia reflexiona.

* [Yo sé que hice lo correcto. No se puede vivir en la duda perpetua; elegí basándome en lo que Joseph necesitaba en ese instante. La libertad no es elegir todos los caminos, es tener el valor de comprometerse con uno solo y aceptar sus consecuencias.]
    -> END
* [Tal vez pude haberme arrepentido]
    -> END
* [Tal vez pude haber escogido otro camino ... El universo es infinito y nosotros solo vemos una parte. Nunca dejaremos de preguntarnos "qué habría pasado si...". Ese es el precio de ser humanos y tener el poder de decidir.]
    -> END
