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

public class RenderData
{
    public Bounds Bounds;
    public List<LayoutNode>? Childrens;
    public List<Control>? Controls;
    
    public RenderData(Bounds bound, List<LayoutNode>? child = null, List<Control>? cont = null) 
    {
        Bounds = bound;
        Childrens = child;
        Controls = cont;
    }
}
