extends Node2D

var selection := 1

func _unhandled_input(event: InputEvent) -> void:
	
	if event.is_action_pressed("ui_down"):
		selection = 2
		$Sprite.position.y = 155
	elif event.is_action_pressed("ui_up"):
		selection = 1
		$Sprite.position.y = 132
	
	if event.is_action_pressed("ui_accept"):
		if selection == 1:
			get_tree().change_scene("res://StartScreen/Instructions.tscn")
		else:
			get_tree().change_scene("res://StartScreen/Credits.tscn")
