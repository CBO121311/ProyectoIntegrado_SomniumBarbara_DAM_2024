extends Area2D


const speed := 100
const POSSIBLE_HEIGHTS := [60, 92, 124, 156, 188, 220]
const EXPLOSION: PackedScene = preload("res://Explosion/Explosion.tscn")
const EGG: PackedScene = preload("res://Egg/Egg.tscn")

var direction := 1

onready var screen_size = get_viewport_rect().size

# when it enters the stage put it in a random position
func _ready() -> void:
	set_random_direction()
	set_random_position()
	

func set_random_position()->void:
	position.y = POSSIBLE_HEIGHTS[randi() % POSSIBLE_HEIGHTS.size()]


func set_random_direction()->void:
	direction = (randi() & 2) - 1
	if direction == -1:
		position.x = screen_size.x

func _process(delta: float) -> void:
	
	# move it in a constant speed
	position.x += speed * delta * direction
	
	# facing direction of the sprite
	if direction == 1:
		$AnimatedSprite.flip_h = true
	else:
		$AnimatedSprite.flip_h = false
		
	# wrap around boundaries
	if position.x > screen_size.x:
		position.x = 0
		set_random_position()
	elif position.x < 0 :
		position.x = screen_size.x 
		set_random_position()


func _on_Enemy_body_entered(body: Node) -> void:
	
	# if player is over enemy. Kill enemy... else kill player
	if body.position.y < position.y:
		body.bounce()
		spawn_explosion()
		spawn_egg()
		queue_free()
	else:
		body.kill()
		
func spawn_explosion()->void:
	var my_explosion = EXPLOSION.instance()
	get_parent().call_deferred("add_child", my_explosion)
	my_explosion.position = position

# When killed spawn a pickable egg
func spawn_egg()->void:
		
	var my_egg = EGG.instance()
	get_parent().get_parent().call_deferred("add_child", my_egg)
	my_egg.position = Vector2(position.x, position.y + 16)

# change the direction of the enemy it it touches another chick (enemy)
func _on_Enemy_area_entered(area: Area2D) -> void:
	direction *= -1
