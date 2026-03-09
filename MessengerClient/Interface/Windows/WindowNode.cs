using MessengerClient.Interface;

namespace MessengerClient
{
    internal class WindowNode : LayoutNode
    {
        public ContentNode? Content { get; private set; }
        
        public WindowNode(RenderData data, ContentNode content) : base(data) 
        {
            Content = content;
            Content.SetSize(Data.Bounds.Size);
        }

        public override RenderData GetRenderData()
        {
            if (Content != null) Data.Controls = Content.Controls;
            return this.Data;
        }
    
        public void SetSize(Size size)
        {
            Data.Bounds.Size = size;
            Content.SetSize(size);
        }
    }
}
