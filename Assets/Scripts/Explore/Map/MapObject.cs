public class MapObject
{
    public int x;
    public int y;
    public int color;
    public string type;
    public bool isXDirection;

    public MapObject(int x, int y, int color, string type, bool isXDirection = false)
    {
        this.x = x;
        this.y = y;
        this.color = color;
        this.type = type;
        this.isXDirection = isXDirection;

    }
}