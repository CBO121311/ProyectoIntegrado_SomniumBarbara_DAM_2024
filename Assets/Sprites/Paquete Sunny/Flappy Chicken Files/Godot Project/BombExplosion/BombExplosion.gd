extends AnimatedSprite


func _ready() -> void:
	play("default")
	
	


func _on_BombExplosion_animation_finished() -> void:
	queue_free()
