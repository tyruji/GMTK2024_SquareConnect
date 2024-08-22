using Godot;
using System;

[GlobalClass]
public partial class Wave : Resource
{
    [Export]
    public float Duration { get; set; } = 10;

    [Export]
    public float Frequency { get; set; } = 3;

    [Export]
    public int ColorCount { get; set; } = 2;

    [Export]
    public float MinSpeed { get; set; } = 20f;

    [Export]
    public float MaxSpeed { get; set; } = 30f;
}