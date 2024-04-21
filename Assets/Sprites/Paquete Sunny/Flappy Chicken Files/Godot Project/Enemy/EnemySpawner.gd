extends Node2D

const ENEMY: PackedScene = preload("res://Enemy/Enemy.tscn")
const BOMB: PackedScene = preload("res://Bomb/Bomb.tscn")

var max_enemies := 7

onready var timer := $Timer
onready var enemies_container = $EnemiesContainer

func spawn_enemy()->void:
	var my_enemy = ENEMY.instance()
	enemies_container.call_deferred("add_child", my_enemy)
	my_enemy.position = Vector2.ZERO

func spawn_fireball()->void:
	var my_bomb = BOMB.instance()
	get_parent().call_deferred("add_child", my_bomb)
	my_bomb.position = Vector2.ZERO

func _on_Timer_timeout() -> void:
	
	if enemies_container.get_child_count() > max_enemies:
		return
		
	var random_value = randi() % 4
	if random_value == 0:
		spawn_fireball()
	else:
		spawn_enemy()

	
		
	
