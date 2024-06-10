INCLUDE global.ink

    {firstTimeKiwi:
        Hola Kiwi. #speaker:Barbara #portrait:barbara_neutral #layout:left
        ¡Hola Bárbara! #speaker:Kiwi #portrait:kiwi_neutral #layout:left
        ¿Me conoces? #speaker:Barbara #portrait:barbara_neutral #layout:left
        Lo extraño sería que no te conociéramos. #speaker:Kiwi #portrait:kiwi_neutral #layout:left
    - else:
        Hola otra vez. #speaker:Barbara #portrait:barbara_neutral #layout:left
        ¡Hola de nuevo, Bárbara! #speaker:Kiwi #portrait:kiwi_neutral #layout:left
    }
    
    ~ firstTimeKiwi= false
    ¿Deseas hacerme alguna pregunta? #speaker:Kiwi #portrait:kiwi_neutral #layout:left
-> main

=== repeat ===
¿Alguna otra pregunta?

-> main


=== main ===



+ [No tengo más preguntas]
    Entonces, ¡hasta pronto! #speaker:Kiwi #portrait:kiwi_neutral #layout:left
    Adiós. #speaker:Barbara #portrait:barbara_neutral #layout:left
    -> END

+ [Objetivo]
    ¿Hay alguna razón para estar aquí? #speaker:Barbara #portrait:barbara_neutral #layout:left
    Ve a los distintos mundos y lo descubrirás. #speaker:Kiwi #portrait:kiwi_neutral #layout:left
    -> main 

+ [¿Quién son los otros?]
    Veo que hay otros animales por aquí. #speaker:Barbara #portrait:barbara_neutral #layout:left
    Son compañeros que podrías manejar si comenzaras sus mundos... #speaker:Kiwi #portrait:kiwi_neutral #layout:left
    Puedes hablar con ellos y preguntarles.
    -> main 

+ [Este lugar]
    ¿Dónde nos encontramos? #speaker:Barbara #portrait:barbara_neutral #layout:left
    En una prisión creada por ti. #speaker:Kiwi #portrait:kiwi_neutral #layout:left
    ¿Por mí? #speaker:Barbara #portrait:barbara_neutral #layout:left
    Sí. ¿Hay alguien más aquí? #speaker:Kiwi #portrait:kiwi_neutral #layout:left
    -> main  

+ [¿Quién eres?]
    Soy Barbara. #speaker:Barbara #portrait:barbara_neutral #layout:left
    Muy bien. #speaker:Kiwi #portrait:kiwi_neutral #layout:left
    ... #speaker:Barbara #portrait:barbara_neutral #layout:left
    Soy el cobrador de deudas. Si alguien te debe algo, dímelo e iré a cobrar esa deuda. #speaker:Kiwi #portrait:kiwi_neutral #layout:left
    -> main