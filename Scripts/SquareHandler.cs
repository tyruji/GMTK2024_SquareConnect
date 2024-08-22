using Godot;
using Godot.Collections;
using System;

public partial class SquareHandler : Node2D
{
    [Export]
    private Array<Color> Colours = new Array<Color>();

    [Export]
    private Array<Wave> Waves = new Array<Wave>();

    [Export]
    private PackedScene _SquarePrefab = null;

    [Export]
    private PackedScene _RainbowSquarePrefab = null;

    [Export]
    public Timer Timer { get; private set; } = null;

    [Export]
    private ScreenBounds _ScreenBounds = null;

    public bool GameEnded { get; private set; } = false;

    public event Action<Square> OnSquareCaptured;

    public event Action<Square> OnSquareDestroyed;

    public event Action<Square, Square> OnSquareBounced;

    public event Action<Square> OnSquareSpawned;

    private int _currentWaveIdx = -1;

    private float _timeElapsed = .0f; 

    public override void _Ready()
    {
        _ScreenBounds = GetNode<ScreenBounds>( "../ScreenBounds" );

        Timer.Timeout += SpawnRandomSquare;

        NextWave();
    }

    public override void _Process( double delta )
    {
        _timeElapsed += ( float ) delta;

        if( _timeElapsed < Waves[ _currentWaveIdx ].Duration ) return;
        NextWave();
    }

    public override void _ExitTree()
    {
        if( Timer == null ) return;

        Timer.Timeout -= SpawnRandomSquare;
    }

    private void NextWave()
    {
        if( _currentWaveIdx == Waves.Count - 1 )
        {
                // you win
            return;
        }
        var wave = Waves[ ++_currentWaveIdx ];
        Timer.Stop();
        Timer.WaitTime = wave.Frequency;
        Timer.Start();
        _timeElapsed = .0f;
    }

    public void StopSpawning()
    {
        Timer.Stop();
        GameEnded = true;
    }

    public void NotifyCaptured( Square square ) => OnSquareCaptured?.Invoke( square );
    public void NotifyDestroyed( Square square ) => OnSquareDestroyed?.Invoke( square );
    public void NotifyBounced( Square sqr1, Square sqr2 ) => OnSquareBounced?.Invoke( sqr1, sqr2 );

    public void SpawnRandomSquare()
    {
        bool isRainbow = GD.Randf() < .1f;

        var square = ( isRainbow ? _RainbowSquarePrefab : _SquarePrefab ).Instantiate<Square>();
        AddChild( square );

        Vector2 point = _ScreenBounds.GetRandomPointOnScreenEdge();
        square.GlobalPosition = point;

        var wave = Waves[ _currentWaveIdx ];

        float speed = ( float ) GD.RandRange( wave.MinSpeed, wave.MaxSpeed );

        square.Velocity = -point.Normalized() * speed;

        square.squareHandler = this;

        if( isRainbow ) return;

        int max_colours = wave.ColorCount;
        Color color = Colours[ GD.RandRange( 0, Mathf.Min( max_colours, Colours.Count ) - 1 ) ];
        square.Color = color;

        OnSquareSpawned?.Invoke( square );
    }

    public void SpawnSquare( Vector2 position, Vector2 direction, 
            Color color = default, bool isRainbow = false )
    {
        var square = ( isRainbow ? _RainbowSquarePrefab : _SquarePrefab ).Instantiate<Square>();
        AddChild( square );

        square.GlobalPosition = position;
        square.Velocity = direction * square.Speed;

        square.squareHandler = this;

        if( isRainbow ) return;

        square.Color = color;

        OnSquareSpawned?.Invoke( square );
    }
}
