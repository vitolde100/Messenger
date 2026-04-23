/*namespace MessengerClient.Interface
{
    internal abstract class ContentNode
    {
        public List<Control> Controls { get; protected set; }
        protected LayoutNode? _owner;
        public Color BackColor { get; protected set; }

        public ContentNode(LayoutNode? owner = null) 
        { 
            _owner = owner;
            Controls = new List<Control>();
            InitializeComponents();
            _owner?.MarkDirty(DirtyFlags.Visual);
        }

        public virtual void SetOwner(LayoutNode owner)
        {
            _owner = owner;
            _owner.MarkDirty(DirtyFlags.Visual);
        }

        public void SetBackColor(Color color)
        {
            BackColor = color;
        }

        protected abstract void InitializeComponents();

        public abstract void SetSize(Size Size);
    }
}*/