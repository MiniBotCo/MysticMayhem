using Godot;
using System;
using System.Collections.Generic;

public partial class Door : Area2D, ISpawnable
{

	[Export]
    public Spawn spawner { get; set; }


}
