namespace MessengerClient.Interface
{
    internal abstract class ContentNode
    {
        public int ID;
        public string Title;

        public List<Control> Controls { get; protected set; }

        public Color BackColor { get; protected set; }

        public ContentNode(int id, string title) 
        { 
            ID = id;
            Title = title;
            Controls = new List<Control>();
            InitializeComponents();
        }

        public void SetBackColor(Color color)
        {
            BackColor = color;
        }

        protected abstract void InitializeComponents();

        public abstract void SetSize(Size Size);
    }
}
