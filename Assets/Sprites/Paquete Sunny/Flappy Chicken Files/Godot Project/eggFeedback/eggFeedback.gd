extends AnimatedSprite

func _ready() -> void:
	play("New Anim")
	


func _on_eggFeedback_animation_finished() -> void:
	queue_free()
	
