extends AnimatedSprite

func _ready() -> void:
	play("New Anim")
	




func _on_Explosion_animation_finished() -> void:
	queue_free()

