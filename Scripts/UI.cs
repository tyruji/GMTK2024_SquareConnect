using Godot;
using System;

public partial class UI : Control
{
    [Export]
    private string _MainMenuScenePath = null;

    [Export]
    private ColorRect _ScreenDarken = null;

    public int Score { get; private set; }

    public int Lives { get; private set; } = 9;

    private SquareHandler _squareHandler = null;

    private bool _transitioning = false;

    public override void _Ready()
    {
        _squareHandler = GetNode<SquareHandler>( "../../SquareHandler" );
        _squareHandler.OnSquareCaptured += AddScore;
        _squareHandler.OnSquareDestroyed += DecreaseLife;

        var tween = CreateTween().SetTrans( Tween.TransitionType.Cubic );
        tween.TweenProperty( _ScreenDarken, "modulate", new Color( 0,0,0,0 ), .5f );
    }

    public override void _ExitTree()
    {
        if( _squareHandler == null ) return;

        _squareHandler.OnSquareCaptured -= AddScore;
        _squareHandler.OnSquareDestroyed -= DecreaseLife;
    }

    private void MainMenu()
    {
        if( _transitioning ) return;
        _transitioning = true;

        var tween = CreateTween().SetTrans( Tween.TransitionType.Cubic );
        tween.TweenProperty( _ScreenDarken, "modulate", Colors.White, .5f );
        tween.TweenCallback( Callable.From( 
            () => { GetTree().ChangeSceneToFile( _MainMenuScenePath ); } ) );
    }

    private void DecreaseLife( Square _ )
    {
        if( Lives <= 0 ) return;

        --Lives;
        var tween = CreateTween();
        var life = GetNode( "HBoxContainer" ).GetChild( Lives );
        tween.TweenProperty( life, "modulate", new Color( 0,0,0,0 ), .5f );
        tween.TweenCallback( Callable.From( life.QueueFree ) );

        if( Lives != 0 ) return;

        _squareHandler.StopSpawning();
        var end_screen = GetNode<CenterContainer>( "EndScreen" );
        end_screen.GetNode<Label>( "VBoxContainer/HBoxContainer/Score" ).Text = Score.ToString();
        end_screen.Visible = true;
        tween.TweenProperty( end_screen, "modulate", Colors.White, .5f );
    }

    private void AddScore( Square _ )
    {
        Score += 100;
        GetNode<Label>( "Score/score" ).Text = Score.ToString();
    }


}
