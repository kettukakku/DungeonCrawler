using Godot;
using System;

public struct Vector2Int
{
    public int x;
    public int y;

    public Vector2Int(int nx, int ny)
    {
        x = nx;
        y = ny;
    }

    //Arithmetic

    public static Vector2Int operator +(Vector2Int a, Vector2Int b)
        => new Vector2Int(a.x + b.x, a.y + b.y);

    public static Vector2Int operator -(Vector2Int a, Vector2Int b)
        => new Vector2Int(a.x - b.x, a.y - b.y);

    public static Vector2Int operator *(Vector2Int a, Vector2Int b)
        => new Vector2Int(a.x * b.x, a.y * b.y);

    public static Vector2Int operator /(Vector2Int a, Vector2Int b)
        => new Vector2Int(a.x / b.x, a.y / b.y);

    //Comparison

    public static bool operator ==(Vector2Int a, Vector2Int b)
        => a.x == b.x && a.x == b.x;
    public static bool operator !=(Vector2Int a, Vector2Int b)
        => !(a == b);

    //Utility
    public override string ToString()
        => $"({x}, {y})";

    //Putting this here until I convert the remaining tuples into structs

    public static Vector2Int operator +(Vector2Int a, (int x, int y) b)
        => new Vector2Int(a.x + b.x, a.y + b.y);

    public static Vector2Int operator +((int x, int y) a, Vector2Int b)
        => new Vector2Int(a.x + b.x, a.y + b.y);
}
