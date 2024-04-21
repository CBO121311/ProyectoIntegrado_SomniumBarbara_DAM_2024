extends RigidBody2D


func _process(delta: float) -> void:
	if position.y > 300:
		queue_free()
