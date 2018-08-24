public struct Vector2Int
{
    public readonly int x;
    public readonly int y;

    public Vector2Int(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public override string ToString()
    {
        return "(" + x + ", " + y + ")";
    }
}
