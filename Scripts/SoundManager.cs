using Godot;
using System;

public partial class SoundManager : Node2D
{
    private AudioStreamPlayer _captureSound = null;
    private AudioStreamPlayer _rainbowCaptureSound = null;
    private AudioStreamPlayer _bounceSound = null;
    private AudioStreamPlayer _destroySound = null;
    private SquareHandler _squareHandler = null;

    public override void _Ready()
    {
        _captureSound = GetNode<AudioStreamPlayer>( "capture" );
        _rainbowCaptureSound = GetNode<AudioStreamPlayer>( "rainbow_capture" );
        _destroySound = GetNode<AudioStreamPlayer>( "destroy" );
        _bounceSound = GetNode<AudioStreamPlayer>( "bounce" );
        _squareHandler = GetNode<SquareHandler>( "../SquareHandler" );       
        _squareHandler.OnSquareCaptured += PlayCapture;
        _squareHandler.OnSquareDestroyed += PlayDestroy;
        _squareHandler.OnSquareBounced += PlayBounce;
    }

    public override void _ExitTree()
    {
        if( _squareHandler == null ) return;

        _squareHandler.OnSquareCaptured -= PlayCapture;
        _squareHandler.OnSquareDestroyed -= PlayDestroy;
        _squareHandler.OnSquareBounced -= PlayBounce;
    }

    private void PlayCapture( Square sqr )
    {
        if( sqr is RainbowSquare )
        {
            _rainbowCaptureSound.Play();
            return;
        }
        _captureSound.Play();
    }
    private void PlayDestroy( Square _ ) => _destroySound.Play();
    private void PlayBounce( Square _, Square __ ) => _bounceSound.Play();
}
