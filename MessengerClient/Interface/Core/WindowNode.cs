namespace MessengerClient.Interface
{
    internal class WindowNode : LayoutNode
    {
        public ContentNode Content { get; private set; }

        public WindowNode(RenderData data, ContentNode content, LayoutNode? parent = null) : base(data, parent) 
        {
            Content = content;
            Content.SetOwner(this);
            Content.SetSize(Data.Bounds.Size);
        }
    
        public override void SetSize(Size size)
        {
            base.SetSize(size);
            Content?.SetSize(size);
            MarkDirty(DirtyFlags.Layout);
        }

        public override RenderData GetRenderData()
        {
            if (Content.Controls != null) Data.Controls = Content.Controls;
            return Data;
        }
    }
}
