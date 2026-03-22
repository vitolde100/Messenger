namespace MessengerClient.Interface
{
    internal class SplitNode : LayoutNode
    {
        public bool Capture;
        public float Ratio {  get; private set; }
        public Orientation Orientation { get; private set; }
        public bool isPreviewDivider;

        private Divider _Divider;
        private Divider _PreviewDivider;
        private int _DividerWidth = 6;

        private LayoutNode _first;
        private LayoutNode _second;

        public Size Size 
        { 
            get { return Data.Size; }
        }

        public SplitNode(RenderData data, LayoutNode first, LayoutNode second, float ratio = 0.5f, Orientation orient = Orientation.Horizontal, LayoutNode parent = null) : base(data, parent)
        {
            Orientation = orient;
            Ratio = ratio;
            Data.Controls = new();

            _PreviewDivider = new Divider(this);
            Data.Controls.Add(_PreviewDivider);
            _PreviewDivider.BackColor = Color.DarkGray;
            _PreviewDivider.Visible = false;

            _Divider = new Divider(this);
            Data.Controls.Add(_Divider);
            _Divider.BackColor = Color.White;


            _first = first;
            _second = second;
            first.SetParent(this);
            second.SetParent(this);

            UpdateLayout();
            MarkDirty(DirtyFlags.Layout | DirtyFlags.Visual);
        }

        public void UpdateLayout()
        {
            if (!isPreviewDivider)
            {
                _PreviewDivider.Visible = false;
                Point firstPosition = new Point(0, 0);
                if (Orientation == Orientation.Horizontal)
                {
                    _Divider.Size = new Size(_DividerWidth, Data.Size.Height);
                    _Divider.Location = new Point((int)(Data.Size.Width * Ratio - _Divider.Size.Width / 2), 0);
                    _Divider.Cursor = Cursors.SizeWE;

                    int firstWidth = (int)((float)Data.Bounds.Size.Width * Ratio - _Divider.Width / 2);

                    _first.SetPosition(firstPosition);
                    _first.SetSize(new Size(firstWidth, Data.Bounds.Size.Height));

                    _second.SetPosition(new Point(firstWidth + firstPosition.X + _Divider.Width / 2, 0));
                    _second.SetSize(new Size(Data.Bounds.Size.Width - firstPosition.X - firstWidth, Data.Bounds.Size.Height));
                }
                else
                {
                    _Divider.Size = new Size(Data.Size.Width, _DividerWidth);
                    _Divider.Location = new Point(0, (int)(Data.Size.Height * Ratio - _Divider.Size.Height / 2));
                    _Divider.Cursor = Cursors.SizeNS;

                    int firstHeight = (int)((float)Data.Bounds.Size.Height * Ratio - _Divider.Height / 2);

                    _first.SetPosition(firstPosition);
                    _first.SetSize(new Size(Data.Bounds.Size.Width, firstHeight));

                    _second.SetPosition(new Point(0, firstHeight + firstPosition.Y + _Divider.Height / 2));
                    _second.SetSize(new Size(Data.Bounds.Size.Width - firstPosition.Y, Data.Bounds.Size.Height - firstHeight));
                    
                    MarkDirty(DirtyFlags.Layout);
                }
            }
            else
            {
                _PreviewDivider.Visible = true;
                if (Orientation == Orientation.Horizontal)
                {
                    _PreviewDivider.Size = new Size(_DividerWidth, Data.Size.Height);
                    _PreviewDivider.Location = new Point((int)(Data.Size.Width * Ratio - _Divider.Size.Width / 2), 0);
                    _PreviewDivider.Cursor = Cursors.SizeWE;
                }
                else
                {
                    _PreviewDivider.Size = new Size(Data.Size.Width, _DividerWidth);
                    _PreviewDivider.Location = new Point(0, (int)(Data.Size.Height * Ratio - _Divider.Size.Height / 2));
                    _PreviewDivider.Cursor = Cursors.SizeNS;
                }
            }
        }

        public void SetRatio(float ratio)
        {
            Ratio = ratio;
            UpdateLayout();
        }

        public override void SetSize(Size size)
        {
            Data.Size = size;
            UpdateLayout();
        }

        public void SetOrientation(Orientation orient)
        {
            Orientation = orient;
            UpdateLayout();
        }

    }
}
