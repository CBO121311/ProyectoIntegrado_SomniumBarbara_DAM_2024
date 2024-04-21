extends KinematicBody2D

const GRAVITY := 10.0
const MAX_FALL_VEL := 100
const FLAP_POWER := 200.0
const move_speed := 100.0

const DEATH:PackedScene = preload("res://Player/DeathPlayer.tscn")

var motion := Vector2.ZERO

onready var screen_size = get_viewport_rect().size

func _process(delta: float) -> void:
	
	# input flap
	if Input.is_action_just_pressed("ui_accept"):
		motion.y = 0
		motion.y -= FLAP_POWER
		$FlapAudio.play()
		
		
	# input direction
	var input_x = Input.get_axis("ui_left", "ui_right")
	motion.x = input_x * move_speed
	
	# apply gravity
	motion.y += GRAVITY
	
	# Max Speed limiter
	if motion.y > MAX_FALL_VEL:
		motion.y = MAX_FALL_VEL
	
	motion = move_and_slide(motion)
	
	# change sprite direction
	if motion.x > 0:
		$AnimatedSprite.flip_h = true
	elif motion.x < 0:
		$AnimatedSprite.flip_h = false
	
	# boundaries
	# keep the chicken in the box of the viewport
	if position.x < 0:
		position.x = 0
	elif position.x > screen_size.x:
		position.x = screen_size.x
	
	if position.y < 0:
		position.y = 0
	elif position.y > screen_size.y:
		position.y = screen_size.y
		bounce()

		
	# debug 
	if Input.is_action_just_pressed("ui_cancel"):
		get_tree().reload_current_scene()
		
# bounce the chicken a little when it hits a chick
func bounce()->void:
	motion.y = 0
	motion.y -= 150
	
# Spawn an sprite of a death chicken
func spawn_death_player_image()->void:
	var my_image = DEATH.instance()
	get_parent().call_deferred("add_child", my_image)
	my_image.position = position
		
# kill player
func kill()->void:
	$DeathAudio.play()
	spawn_death_player_image()
	position = Vector2(210, -16)
