extends Node2D


onready var scroll = $Scroll
var init = 392.0

# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	init = scroll.position.y


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta: float) -> void:
	scroll.global_position.y -= 0.40
	
	if scroll.global_position.y <= -770:
		scroll.global_position.y = init

func _unhandled_input(event: InputEvent) -> void:

	if event.is_action_pressed("ui_accept"):
		get_tree().change_scene("res://StartScreen/StartScreen.tscn")
