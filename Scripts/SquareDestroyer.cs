using Godot;
using System;

public partial class SquareDestroyer : Area2D
{
    private void OnAreaEntered( Area2D area )
    {
        if( area is not Square sqr ) return;

        sqr.Destroy( false );
    }
}
