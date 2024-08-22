using Godot;
using System;

public partial class StartingSquare : RainbowSquare
{
    public override void _Ready()
    {
        Captured = true;
    }

    public override void _Process(double delta) {}
}
