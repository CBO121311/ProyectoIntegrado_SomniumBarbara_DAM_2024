extends Area2D

const FEEDBACK:PackedScene = preload("res://eggFeedback/eggFeedback.tscn")

var speed := 80.0
var direction := Vector2(1, 1)

var life_span_counter := 900

onready var screen_size = get_viewport_rect().size

func _ready() -> void:
	direction.x = (randi() & 2) - 1

func _process(delta: float) -> void:
	
	
	# Movement
	position.x += speed * direction.x * delta
	position.y += speed * .5 * direction.y * delta
	
	# boundaries
	if position.x < 0:
		#position.x = screen_size.x
		direction.x  = 1
	elif position.x > screen_size.x:
		#position.x = 0
		direction.x  = -1
		
	if position.y < 0:
		direction.y  = 1
	elif position.y > screen_size.y:
		direction.y = -1
		
	# Remove after a while
	if life_span_counter == 300:
		$AnimationPlayer.play("flick")
	if life_span_counter <= 0:
		queue_free()
	life_span_counter -= 1
	
# visual feedback for the player when it picks an egg
func spawn_feedback()->void:
	var my_feedback = FEEDBACK.instance()
	get_parent().call_deferred("add_child", my_feedback)
	my_feedback.position = position
	

# When chicken touches an egg, update the egg counter 
func _on_Egg_body_entered(body: Node) -> void:
	spawn_feedback()
	get_parent().get_parent().update_bar(1)
	queue_free()
