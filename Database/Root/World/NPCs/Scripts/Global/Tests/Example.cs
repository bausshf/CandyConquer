/*
 * A script file may contain multiple script entries.
 * A script file may contain classes, enums, structs etc.
 * A script file may contain methods that does not implement the ScriptEntry attribute.
 * A script file can access all methods declared in other script files.
 * 
 * Methods that implement the ScriptEntry attribute must have the Entry property declared.
 * The entry must be the npc id.
 * 
 * All types and methods not declared with the ScriptEntry attribute should be declared private.
 * There's no reason for them to be declared public, it will only let them be iterated during compilation,
 * which we want to avoid since they can't be called directly from outside the engine.
 */

[ScriptEntry(Entry = 0)]
public static void ExampleCall(Player player, byte option)
{
	switch (option)
	{
		case 0: ExampleCall_Initial(player); break;
		case 1: ExampleCall_Option1(player); break;
		
		default: ExampleCall_Default(player); break;
	}
}

private static void ExampleCall_Initial(Player player)
{
	// Option 0
}

private static void ExampleCall_Option1(Player player)
{
	// Option 1
}

private static void ExampleCall_Default(Player player)
{
	// Anything ...
}