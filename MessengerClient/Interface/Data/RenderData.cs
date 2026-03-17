namespace MessengerClient.Interface
{
    public class RenderData
    {
        public Bounds Bounds;
        public List<Control>? Controls;

        public RenderData(Bounds bound, List<Control>? cont = null)
        {
            Bounds = bound;
            Controls = cont;
        }
    }
}
