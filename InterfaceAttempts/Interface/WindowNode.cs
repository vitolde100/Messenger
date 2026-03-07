
using InterfaceAttempts.Windows;

namespace InterfaceAttempts.Interface
{
    internal class WindowNode : LayoutNode
    {
        public ContentNode? Content { get; private set; }
        
        public WindowNode(RenderData data, ContentNode content) : base(data) 
        {
            Content = content;
        }

        public override RenderData GetRenderData()
        {
            if (Content != null) Data.Controls = Content.Controls;
            return this.Data;
        }
    }
}
