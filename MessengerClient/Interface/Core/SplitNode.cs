namespace MessengerClient.Interface
{
    public class SplitNode : LayoutNode
    {
        public float Ratio { get; private set; }
        public bool isHorisontal { get; private set; }

        private LayoutNode _first;
        private LayoutNode _second;

        public SplitNode(RenderData data, LayoutNode first, LayoutNode second, float ratio, LayoutNode parent = null) : base(data, parent)
        {
            isHorisontal = true;
            Ratio = ratio;

            _first = first;
            _second = second;
            first.SetParent(this);
            second.SetParent(this);

            Visual.SetBackColor(Color.FromArgb(0, 255, 0));

            UpdateChildrens();
        }

        public void SetRatio(float ratio)
        {
            Ratio = ratio;
            UpdateChildrens();
        }

        public void SetOrientation(bool orient)
        {
            isHorisontal = orient;
            UpdateChildrens();
        }

        public override void SetSize(Size size)
        {
            base.SetSize(size);
            UpdateChildrens();
        }

        public void UpdateChildrens()
        {
            Point firstPosition = new Point(0, 0);
            if (isHorisontal)
            {
                int firstWidth = (int)((float)Data.Bounds.Size.Width * Ratio);
                
                
                _first.SetPosition(firstPosition);
                _first.SetSize(new Size(firstWidth, Data.Bounds.Size.Height));

                _second.SetPosition(new Point(firstWidth + firstPosition.X, 0));
                _second.SetSize(new Size(Data.Bounds.Size.Width - firstPosition.X - firstWidth, Data.Bounds.Size.Height));
            }
            else
            {
                int firstHeight = (int)((float)Data.Bounds.Size.Height * Ratio);

                _first.SetPosition(firstPosition);
                _first.SetSize(new Size(Data.Bounds.Size.Width, firstHeight));

                _second.SetPosition(new Point(0, firstHeight));
                _second.SetSize(new Size(Data.Bounds.Size.Width, Data.Bounds.Size.Height - firstHeight));
            }
            MarkDirty(DirtyFlags.Layout);
        }

        public override RenderData GetRenderData()
        {
            UpdateChildrens();
            return base.GetRenderData();
        }
    }
}
