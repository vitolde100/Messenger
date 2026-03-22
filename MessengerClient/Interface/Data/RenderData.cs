namespace MessengerClient.Interface
{
    internal class RenderData
    {
        
        public Bounds Bounds;
        public List<Control>? Controls;

        public Size Size
        {
            get { return Bounds.Size; }
            set { Bounds.Size = value; }
        }
        public Point Location
        {
            get { return Bounds.Position; }
            set { Bounds.Position = value; }
        }

        public RenderData(Bounds? bound = null, List<Control>? cont = null)
        {
            Bounds = bound != null ? bound : new Bounds();
            Controls = cont;
        }
    }
}
