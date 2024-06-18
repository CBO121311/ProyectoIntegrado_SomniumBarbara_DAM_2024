INCLUDE global.ink

    {firstTimeSquirrel:
        Muy buenas, ardilla. ¿Podría preguntarte algo? #speaker:Barbara #portrait:barbara_neutral #layout:left
        Buenas Bárbara. #speaker:Ardilla #portrait:squirrel_happy #layout:left
    - else:
        ¡Hola de nuevo, ardilla! ¿Puedo preguntar? #speaker:Barbara #portrait:barbara_neutral #layout:left
        
        Por ti lo que sea. #speaker:Ardilla #portrait:squirrel_neutral #layout:left

    }

    ~ firstTimeSquirrel= false

-> main

=== repeat ===
¿Alguna otra pregunta?

-> main

=== main ===

Claro, ¿deseas hacerme alguna pregunta? #speaker:Ardilla #portrait:squirrel_neutral #layout:left

+ [No tengo más preguntas]
    Entiendo. ¡Gracias y adiós!. #speaker:Barbara #portrait:barbara_neutral #layout:left
    ¡Adiós, Bárbara! ¡No me olvides!. #speaker:Ardilla #portrait:squirrel_sad #layout:left
    -> END

+ [Portal verde]
    Este portal te llevaría a una serie de fases de plataformas, donde controlarías a una ardilla, un conejo y quizás en un futuro, un dinosaurio. #speaker:Ardilla #portrait:squirrel_neutral #layout:left
    Dependiendo de cuál elijas, podrás realizar diferentes movimientos para acceder a distintos lugares. #speaker:Ardilla #portrait:squirrel_neutral #layout:left

    -> main