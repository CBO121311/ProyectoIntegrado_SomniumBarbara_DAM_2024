extends Area2D

var direction := 1
const speed := 100
const POSSIBLE_HEIGHTS := [60, 92, 124, 156, 188, 220]

const BOMB_EXPLOSION: PackedScene = preload("res://BombExplosion/BombExplosion.tscn")

onready var screen_size = get_viewport_rect().size

# put this on a random position
func _ready() -> void:
	set_random_direction()
	set_random_position()
	
# Select a random vertical position 
func set_random_position()->void:
	position.y = POSSIBLE_HEIGHTS[randi() % POSSIBLE_HEIGHTS.size()]

# either move it right or left
func set_random_direction()->void:
	direction = (randi() & 2) - 1
	if direction == -1:
		position.x = screen_size.x


func _process(delta: float) -> void:
	# move it at a liner speed
	position.x += speed * delta * direction

	# wrap it around boundaries
	if position.x > screen_size.x:
		queue_free()
	elif position.x < 0:
		queue_free()
	
# Spawn the explosion animation
func spawn_explosion()->void:
	var my_explosion = BOMB_EXPLOSION.instance()
	get_parent().call_deferred("add_child", my_explosion)
	my_explosion.position = position

# if the player touches it kill the chicken
func _on_Bomb_body_entered(body: Node) -> void:
	spawn_explosion()
	body.kill()
	queue_free()
