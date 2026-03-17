namespace MessengerClient.Interface;

public class Bounds
{
    public Point Position;
    public Size Size;

    public Bounds(Point position, Size size)
    {
        Position = position;
        Size = size;
    }

    public Bounds(Point position) : this(position, new Size(100, 100)) { }
}
