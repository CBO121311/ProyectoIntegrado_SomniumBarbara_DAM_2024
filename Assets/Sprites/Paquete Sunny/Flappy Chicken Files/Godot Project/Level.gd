extends Node2D


var eggs_eateng_by_tick := -1
var max_eggs := 20
var eggs := max_eggs

onready var eating_cursor = $UI/EatingCursor
onready var eggsBar = $UI/EggsBar


# Position the UI elements
func _ready() -> void:
	eggsBar.rect_size.x = eggs * 10
	eating_cursor.position.x = (eggs + 1) * 10

# One eggs is consumed everytime
func _on_Timer_timeout() -> void:
	update_bar(eggs_eateng_by_tick)

# Update the UI when an egg is taken
func update_bar(amount):
	eggs += amount
	eggsBar.rect_size.x = eggs * 10
	eating_cursor.position.x = (eggs + 1) * 10
	
