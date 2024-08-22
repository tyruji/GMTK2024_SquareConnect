using Godot;
using System;

public partial class ScreenBounds : Node2D
{
    public Vector2 ScreenSize => _multiplier * ( Vector2 ) DisplayServer.ScreenGetSize();

    private Player _player = null;

    private SquareHandler _squareHandler = null;

    public event Action OnScreenResize;

    private float _multiplier = .25f;

    public override void _Ready()
    {
        _player = GetNode<Player>( "../Player" );
        _squareHandler = GetNode<SquareHandler>( "../SquareHandler" );        
        _squareHandler.OnSquareCaptured += CheckScreenResize;
    }

    public override void _ExitTree()
    {
        if( _squareHandler == null ) return;
        
        _squareHandler.OnSquareCaptured -= CheckScreenResize;
    }

    private void CheckScreenResize( Square sqr )
    {
        Vector2 offset = sqr.GlobalPosition - _player.GlobalPosition;
        if( Mathf.Abs( offset.X ) < ScreenSize.X * .416f && 
            Mathf.Abs( offset.Y ) < ScreenSize.Y * .33f ) return;

        _multiplier *= 1.25f;

        OnScreenResize?.Invoke();
    }

    public Vector2 GetRandomPointOnScreenEdge() => RandomPointOnSquare( .5f * ScreenSize );

    private Vector2 RandomPointOnSquare( Vector2 size ) => RandomPointOnSquare( size.X, size.Y );
    
    private Vector2 RandomPointOnSquare( float size_x, float size_y )
    {
        float angle = GD.Randf() * Mathf.Pi * .5f;
        float tan = Mathf.Tan( angle );

        Vector2 result = Vector2.Zero;

        if( tan > size_x / size_y )
        {
            result.Y = size_y;
            result.X = size_y / tan;
        }
        else
        {
            result.X = size_x;
            result.Y = size_x * tan;
        }

        result.X *= 2 * GD.RandRange( 0, 1 ) - 1;
        result.Y *= 2 * GD.RandRange( 0, 1 ) - 1;

        return result;
    }
}
