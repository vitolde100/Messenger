namespace MessengerClient.Interface
{
    internal class WindowNode : LayoutNode
    {
        public ContentNode? Content { get; private set; }

        public WindowNode(RenderData data, ContentNode content, LayoutNode? parent = null) : base(data, parent) 
        {
            Content = content;
            Content.SetSize(Data.Bounds.Size);

            Visual.SetBackColor(Color.FromArgb(76, 76, 76));
        }
    
        public override void SetSize(Size size)
        {
            base.SetSize(size);
            Content?.SetSize(size);
            MarkDirty(DirtyFlags.Layout);
        }

        public override RenderData GetRenderData()
        {
            if (Content != null) Data.Controls = Content.Controls;
            return Data;
        }
    }
}
