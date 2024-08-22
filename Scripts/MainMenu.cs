using Godot;
using System;

public partial class MainMenu : Control
{
    [Export]
    private PackedScene _GameScene = null;

    [Export]
    private PackedScene _TutorialScene = null;

    [Export]
    private ColorRect _ScreenDarken = null;

    private bool _transitioning = false;

    public override void _Ready()
    {
        var tween = CreateTween().SetTrans( Tween.TransitionType.Cubic );
        tween.TweenProperty( _ScreenDarken, "modulate", new Color( 0,0,0,0 ), .5f );
    }

    private void StartGame()
    {
        if( _transitioning ) return;
        _transitioning = true;

        var tween = CreateTween().SetTrans( Tween.TransitionType.Cubic );
        tween.TweenProperty( _ScreenDarken, "modulate", Colors.White, .5f );
        tween.TweenCallback( Callable.From( 
            () => { GetTree().ChangeSceneToPacked( _GameScene ); } ) );
    }
    
    private void StartTutorial()
    {
        if( _transitioning ) return;
        _transitioning = true;

        var tween = CreateTween().SetTrans( Tween.TransitionType.Cubic );
        tween.TweenProperty( _ScreenDarken, "modulate", Colors.White, .5f );
        tween.TweenCallback( Callable.From( 
            () => { GetTree().ChangeSceneToPacked( _TutorialScene ); } ) );
    }

}
