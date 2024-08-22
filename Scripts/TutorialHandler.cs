using Godot;
using Godot.Collections;
using System;

public partial class TutorialHandler : Node2D
{
    [Export]
    public Array<string> Dialogues = new Array<string>();

    private int _index = 0;

    private SquareHandler _squareHandler = null;

    private VBoxContainer _text = null;

    private Button _nextButton = null;

    private Player _player = null;

    public override void _Ready()
    {
        _player = GetNode<Player>( "../Player" );
        _squareHandler = GetNode<SquareHandler>( "../SquareHandler" );

        _squareHandler.Timer.Stop();

        _text = GetNode<VBoxContainer>( "../CanvasLayer/ui/text" );
        _text.Modulate = new Color( 0,0,0,0 );

        _nextButton = _text.GetNode<Button>( "Button" );
        
        NextDialogue();
    }

    public void NextDialogue()
    {
        var tween = CreateTween();
        tween.TweenCallback( Callable.From( () => _nextButton.Disabled = true) );
        tween.TweenProperty( _text, "modulate", new Color( 0,0,0,0 ), .5f );

        Color red = new Color( 1, 0, 0, 1 );
        Color blue = new Color( 0, 0, 1, 1 );
        Color green = new Color( 0, 1, 0, 1 );

        if( _index == 2 )
        {
            
            tween.TweenCallback( Callable.From( 
                () => _squareHandler.SpawnSquare( new Vector2( -101, -170 ), 
                    5 * Vector2.Down, red ) ) );

            tween.TweenCallback( Callable.From(
                () => _player.MouseControlled = false ) ).SetDelay( .25f );

            tween.TweenCallback( Callable.From( 
                () => _player.TargetPosition = new Vector2( -100, 0 ) ) );
            
            tween.TweenCallback( Callable.From(
                () => _player.MouseControlled = true ) ).SetDelay( 1f );
        }

        if( _index == 3 )
        {
            
            tween.TweenCallback( Callable.From( 
                () => _squareHandler.SpawnSquare( new Vector2( -97, -170 ), 
                    5 * Vector2.Down, red ) ) );

            tween.TweenCallback( Callable.From(
                () => _player.MouseControlled = false ) ).SetDelay( .25f );

            tween.TweenCallback( Callable.From( 
                () => _player.TargetPosition = new Vector2( -100, 0 ) ) );
            
            tween.TweenCallback( Callable.From(
                () => _player.MouseControlled = true ) ).SetDelay( 1f );
        }

        if( _index == 4 )
        {
            tween.TweenCallback( Callable.From( 
                () => _squareHandler.SpawnSquare( new Vector2( -100, -170 ), 
                    4 * Vector2.Down, blue ) ) );

            tween.TweenCallback( Callable.From(
                () => _player.MouseControlled = false ) ).SetDelay( .25f );

            tween.TweenCallback( Callable.From( 
                () => _player.TargetPosition = new Vector2( -100, 0 ) ) );
            
            tween.TweenCallback( Callable.From(
                () => _player.MouseControlled = true ) ).SetDelay( 1f );
        }

        if( _index == 5 )
        {
            var lifes = GetNode<HBoxContainer>( "../CanvasLayer/ui/HBoxContainer" );
            lifes.Modulate = new Color( 0,0,0,0 );
            tween.TweenCallback( Callable.From( () => lifes.Visible = true ) );
            tween.TweenProperty( lifes, "modulate", Colors.White, 1.0f );

            tween.TweenCallback( Callable.From( 
                () => _squareHandler.SpawnSquare( new Vector2( -100, -170 ), 
                    4 * Vector2.Down, blue ) ) );

            tween.TweenCallback( Callable.From(
                () => _player.MouseControlled = false ) ).SetDelay( .25f );

            tween.TweenCallback( Callable.From( 
                () => _player.TargetPosition = new Vector2( -100, 0 ) ) );
            
            tween.TweenCallback( Callable.From(
                () => _player.MouseControlled = true ) ).SetDelay( 1f );
        }

        if( _index == 6 )
        {
            var score = GetNode<HBoxContainer>( "../CanvasLayer/ui/Score" );
            score.Modulate = new Color( 0,0,0,0 );
            tween.TweenCallback( Callable.From( () => score.Visible = true ) );
            tween.TweenProperty( score, "modulate", Colors.White, .5f );

            tween.TweenCallback( Callable.From( 
                () => _squareHandler.SpawnSquare( new Vector2( -271, 0 ), 
                    4 * Vector2.Right, blue ) ) );

            tween.TweenCallback( Callable.From(
                () => _player.MouseControlled = false ) ).SetDelay( .25f );

            tween.TweenCallback( Callable.From( 
                () => _player.TargetPosition = new Vector2( -100, -5 ) ) );
            
            tween.TweenCallback( Callable.From(
                () => _player.MouseControlled = true ) ).SetDelay( 1.25f );
        }

        if( _index == 7 )
        {
            tween.TweenCallback( Callable.From( 
                () => _squareHandler.SpawnSquare( new Vector2( -271, 0 ), 
                    4 * Vector2.Right, blue ) ) );

            tween.TweenCallback( Callable.From( 
                () => _squareHandler.SpawnSquare( new Vector2( -100, -170 ), 
                    4 * Vector2.Right, red ) ) );

            tween.TweenCallback( Callable.From( 
                () => _squareHandler.SpawnSquare( new Vector2( 280, -20 ), 
                    7 * Vector2.Left, green ) ) );

            tween.TweenCallback( Callable.From( 
                () => _squareHandler.SpawnSquare( new Vector2( -85, 190 ), 
                    5 * Vector2.Up, isRainbow: true ) ) );
        }

        if( _index == 9 )
        {
            tween.TweenCallback( Callable.From( _squareHandler.StopSpawning ) ).SetDelay( .5f );
            return;
        }

        tween.TweenCallback( Callable.From( NextText ) );
        tween.TweenProperty( _text, "modulate", Colors.White, .5f );
        tween.TweenCallback( Callable.From( () => _nextButton.Disabled = false ) );
    }

    private void NextText() => _text.GetNode<Label>( "Label" ).Text = Dialogues[ _index++ ];
}
