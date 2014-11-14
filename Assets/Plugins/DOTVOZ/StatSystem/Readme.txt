Version: 1.0

---
Stats can be either a float/int or an array.
Array stats are for setting: Current, Minimum and Maximum values.

You can write "min" or "max" inside the array for float.MinValue or float.MaxValue respectively.



Stat samples:

	"health": [100, 0, 100] -> Current health will be equal to 100, minimum to 0 and maximum to 100.

	"movementSpeed": 10 -> Current movementSpeed will be equal to 10, minimum to float.MinValue and maximum to float.MaxValue.

	"level": [1, 1, "max"] -> Current level will be equal to 1, minimum to 1, maximum to float.MaxValue.

	"resistance": 0.25 -> 25% Global Resistance. Damage is multiplied by (1 - this value + element resistance). Does nothing for Heal and Physical damage types.

	"resistanceFire": 0.25 -> 25% Fire Resistance. Damage is multiplied by (1 - resistance + this value).

	"resistanceHeal": -0.25 -> 25% More Healing. Damage is multiplied by (1 + this value). Use negative value for increased damage or healing.

---

StatEffects are made for changing the Stat values (Current values).

You can easily create Abilities for RPG games using this.



List of current StatEffect properties (You can have an array of effects for a single StatEffect, see SampleEffects.json):

	stat -> Stat id.

	damageType -> Damage type. default: "NoResistance".

	value -> Value.
	duration -> Duration of the effect. default: 0 (infinite).

	delay -> (seconds) Delay before starting the effect.

	tick -> (seconds) Tick between effect applies.

	return -> If the Stat value should return to the value before the effect. default: true.

	set -> If the value should set (=) instead of incrementing (+). default: false.
	multiply -> If the value should multiply (*) instead of incrementing (+). default: false.

	child -> If the effects should apply to a child of the initial target.


Warning: Having Stat "health" and value below 0 will remove functionality of "return", "set" and "multiply". Value above 0 and Damage Type "Heal" will also remove it (With Unit).

Warning: Damage Type only works with "health" Stat (With Unit).

---
Error troubleshooting:
If you have Errors inside JSONLoader, check if the JSON file path is correct or you might get it from a syntax error inside the JSON because it could not parse it.
If you have Errors inside Init function of StatObject and/or StatEffect take a look at how the JSON is written because almost everything there is JSON-to-Script.