using Godot;
using System;

public partial class CameraHandler : Camera2D
{
    private ScreenBounds _screenBounds = null;

    public override void _Ready()
    {
        _screenBounds = GetNode<ScreenBounds>( "../ScreenBounds" );       
        _screenBounds.OnScreenResize += ResizeZoom;
    }

    public override void _ExitTree()
    {
        if( _screenBounds == null ) return;
        
        _screenBounds.OnScreenResize -= ResizeZoom;
    }

    private void ResizeZoom()
    {
        var tween = CreateTween().SetTrans( Tween.TransitionType.Spring );
        
            // Create a sort of animation that lasts 1.5 seconds.
        tween.TweenProperty( this, "zoom", Zoom / 1.25f, 1.5f );
    }
}
