namespace InterfaceAttempts.Interface
{ 
    public abstract class LayoutNode
    {
        public LayoutNode? Parent { get; private set; }
        public RenderData Data { get; private set; }

        public LayoutNode(RenderData data)
        {
            Data = data;
            if(Data.Childrens != null)
                foreach (var child in Data.Childrens) child.SetParent(this);
        }

        public void SetParent(LayoutNode parent)
        {
            Parent = parent;
        }

        public abstract RenderData GetRenderData();
    }
}
