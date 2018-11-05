/// <summary>
///     Integer Vector2
/// </summary>
public struct Vector2Int
{
    /// <summary>
    ///     X coordinate
    /// </summary>
    public readonly int X;

    /// <summary>
    ///     Y coordinate
    /// </summary>
    public readonly int Y;

    public Vector2Int(int x, int y)
    {
        X = x;
        Y = y;
    }

    public override string ToString()
    {
        return "(" + X + ", " + Y + ")";
    }
}