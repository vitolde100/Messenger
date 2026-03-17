namespace MessengerClient.Interface
{
    public class VisualStyle
    {
        private LayoutNode _owner;
        public bool isDirty { get; set; }
        public Color BackColor { get; private set; }
        
        public VisualStyle(LayoutNode owner) 
        { 
            _owner = owner;
        }

        public void SetBackColor(Color backColor) 
        { 
            BackColor = backColor;
            _owner.MarkDirty(DirtyFlags.Visual);
        }
    }
}
