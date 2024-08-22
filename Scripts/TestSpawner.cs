using Godot;
using Godot.Collections;
using System;

public partial class TestSpawner : Node2D
{
    [Export]
    private Array<Color> Colours = new Array<Color>();

    [Export]
    private PackedScene _SquarePrefab = null;

    [Export]
    private Timer _Timer = null;

    public override void _Ready()
    {
        _Timer.Timeout += SpawnRandomSquare;
    }

    public override void _ExitTree()
    {
        if( _Timer == null ) return;
        
        _Timer.Timeout -= SpawnRandomSquare;
    }

    private void SpawnRandomSquare()
    {
        var square = _SquarePrefab.Instantiate<Square>();
        AddChild( square );

        square.CanCollide = false;

        Vector2 point = new Vector2( GD.RandRange( 1036,1848 ),GD.RandRange( -32,-161 ) );
        square.GlobalPosition = point;
        square.Velocity = Vector2.Down * ( 1.0f + GD.Randf() )  * 20f * square.Speed;

        square.Scale = Vector2.One * ( 1.0f + GD.Randf() );

        Color color = Colours[ GD.RandRange( 0, Colours.Count - 1 ) ];
        square.Color = color;
    }
}
