using Godot;
using System;

public partial class Square : Area2D
{
    [Export]
    public float Speed = 25f;

    public Color Color { get => Modulate; set => Modulate = value; }

    public bool Captured { get; protected set; } = false;

    public Vector2 Velocity { get; set; } = Vector2.Zero;

    public bool CanCollide { get; set; } = true;

    public bool Destroyed { get; private set; } = false;

    public SquareHandler squareHandler = null;

    public override void _Process(double delta)
    {
        if( squareHandler != null && squareHandler.GameEnded && !Destroyed )
        {
            Destroy();
        }
        
        if( Captured ) return;

        GlobalPosition += ( float ) delta * Velocity;
    }

    public virtual bool CompareColours( Square square )
    {
        return this.Color == square.Color;
    }

        /// Connect a new square to the square mass.
    public virtual void JoinSquare( Square square, Vector2 offset )
    {
        square.GetParent().RemoveChild( square );
        GetParent().AddChild( square );

        square.GlobalPosition = GlobalPosition + offset;

        var tween = CreateTween();//.SetTrans( Tween.TransitionType.Cubic );
        tween.TweenProperty( square, "modulate", 4 * Colors.White, .05f );
        tween.TweenProperty( square, "modulate", square.Color, .05f );
    }

    public void Destroy( bool takePoints = true )
    {
        if( !takePoints )
        {
            QueueFree();
            return;
        }
        
        Destroyed = true;
        squareHandler?.NotifyDestroyed( this );
        var tween = CreateTween();
        tween.TweenProperty( GetNode( "Sprite2D" ), "modulate", new Color( 0,0,0,0 ), .25f );
        GetNode<GpuParticles2D>( "deathparticles" ).Emitting = true;
        tween.TweenCallback( Callable.From( QueueFree ) ).SetDelay( .25f );
    }

    private void OnAreaEntered( Area2D area )
    {
        //GD.Print( "Collided with, ", area );
        
        if( Destroyed || !CanCollide || Captured
         || area is not Square square || square.Destroyed ) return;

        if( !square.Captured )
        {
                // If the square we collided with 
                // is not captured, bounce off of it.

            Vector2 dir = ( GlobalPosition - square.GlobalPosition ).Normalized();
            Velocity = dir * Speed;

            squareHandler?.NotifyBounced( this, square );

            return;
        }
            // We collided with a captured square.

            // If our colours match, we are captured by the player.
        if( square.CompareColours( this ) || CompareColours( square ) )
        {
            Captured = true;

                // We calculate the offset here, becuase 
                // by the time the join method is called, the
                // positions might change.
            Vector2 pos_offset = GlobalPosition - square.GlobalPosition;
            // float sqr_size = 32;
            // if( pos_offset.LengthSquared() < sqr_size * sqr_size )
            // {
            //     pos_offset = pos_offset.Normalized() * sqr_size;
            // }

            square.CallDeferred( nameof( JoinSquare ), this, pos_offset );

            squareHandler?.NotifyCaptured( this );

            return;
        }

            // Colors dont match, this square is destroyed.
        Destroy();
    }
}
