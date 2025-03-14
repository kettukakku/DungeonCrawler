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

    public override bool Equals(object obj)
        => obj is Vector2Int other && Equals(other);

    public bool Equals(Vector2Int other)
        => x == other.x && y == other.y;

    //Utility
    public override string ToString()
        => $"({x}, {y})";

    public override int GetHashCode()
        => x.GetHashCode() ^ y.GetHashCode() << 2;

    //Putting this here until I convert the remaining tuples into structs
    public static Vector2Int operator +(Vector2Int a, (int x, int y) b)
        => new Vector2Int(a.x + b.x, a.y + b.y);

    public static Vector2Int operator +((int x, int y) a, Vector2Int b)
        => new Vector2Int(a.x + b.x, a.y + b.y);
}
