INCLUDE global.ink

    {firstTimeButterfly:
        Muy buenas, Mariposa. ¿Podría preguntarte algo? #speaker:Barbara #portrait:barbara_neutral #layout:left
        Estoy aquí para ayudarte en lo que necesites. #speaker:Mariposa #portrait:butterfly_neutral #layout:left
    - else:
        ¡Hola de nuevo, Mariposa! ¿Te puedo preguntar? #speaker:Barbara #portrait:barbara_neutral #layout:left
        
        Por supuesto. #speaker:Mariposa #portrait:butterfly_neutral #layout:left
    }

    ~ firstTimeButterfly= false

-> main

=== repeat ===
¿Necesitas saber alguna pregunta más?

-> main

=== main ===

+ [No tengo preguntas]
    Entiendo. Hasta luego, Mariposa. #speaker:Barbara #portrait:barbara_neutral #layout:left
    Mariposa: Adiós, que no tengas pesadillas. #speaker:Mariposa #portrait:butterfly_neutral #layout:left
   
    -> END

+ [Portal blanco]
    ¿A dónde lleva este portal blanco? #speaker:Barbara #portrait:barbara_neutral #layout:left

     Este portal te llevaría a una serie de fases de vuelo, donde controlarías a una mariposa, un murciélago y un águila.  #speaker:Mariposa #portrait:butterfly_neutral #layout:left
    Cada fase presenta desafíos únicos, como evitar ser atrapado, navegar utilizando ultrasonido o incluso defenderse.
-> repeat  