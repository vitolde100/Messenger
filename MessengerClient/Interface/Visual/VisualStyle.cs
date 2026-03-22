namespace MessengerClient.Interface
{
    internal class VisualStyle
    {
        private LayoutNode _owner;
        public bool isDirty { get; private set; }
        public Color BackColor { get; private set; }
        
        public VisualStyle(LayoutNode owner) 
        { 
            _owner = owner;
            BackColor = Color.Black;
        }

        public void SetBackColor(Color backColor) 
        { 
            BackColor = backColor;
            _owner.MarkDirty(DirtyFlags.Visual);
        }
    }
}
