using Godot;
using System;

public partial class Player : Node2D
{
    [Export]
    private float _Speed = 8.0f;

    public bool MouseControlled { get; set; } = true;

    public Vector2 TargetPosition { get; set; }

    public override void _Process(double delta)
    {
        Vector2 target = TargetPosition;
        
        if( MouseControlled )
        {
            target = GetGlobalMousePosition();
        }

        GlobalPosition = GlobalPosition.Lerp( target, ( float ) delta * _Speed );
    }
}
