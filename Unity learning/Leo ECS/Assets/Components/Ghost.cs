using Leopotam.Ecs;
using UnityEngine;

struct Ghost
{
    public float Radius;
    public Vector2 Position;
    public Vector2 Velocity;
    public Color Color;
}

struct NewGhost {}

struct WorldBounds
{
    public Rect Bounds;
}