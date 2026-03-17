namespace MessengerClient.Interface
{ 
    public class LayoutNode
    {

        public VisualStyle Visual;

        protected RenderData Data;
        public DirtyFlags Dirty { get; private set; }
        public LayoutNode? Parent { get; private set; }
        public List<LayoutNode>? Childrens { get; } = new(); 

        public LayoutNode(RenderData data, LayoutNode? parent = null)
        {
            Visual = new(this);
            Data = data;
            SetParent(parent);
            if(Childrens != null)
                foreach (var child in Childrens) child.SetParent(this);
            MarkDirty(DirtyFlags.Layout);
        }
        
        //For Render
        public void SetPosition(Point point)
        {
            Data.Bounds.Position = point;
            MarkDirty(DirtyFlags.Layout);
        }

        public virtual void SetSize(Size size)
        {
            Data.Bounds.Size = size;
        }

        public virtual RenderData GetRenderData()
        {
            return Data;
        }
      
        //For Tree
        public void MarkDirty(DirtyFlags flag)
        {
            Dirty |= flag;
            Parent?.MarkDirty(flag);
        }

        public void ClearDirty()
        {
            Dirty = DirtyFlags.None;
        }

        public void SetParent(LayoutNode? parent = null)
        {
            MarkDirty(DirtyFlags.Layout);
            Parent?.RemoveChildren(this);
            Parent = parent;
            Parent?.AddChildren(this);
            MarkDirty(DirtyFlags.Layout);
        }

        public void AddChildren(LayoutNode Node)
        {
            if (!Childrens.Contains(Node))
                Childrens.Add(Node);
        }

        public void RemoveChildren(LayoutNode Node)
        {
            if(Childrens == null) return;
            if(Childrens.Contains(Node)) 
                Childrens.Remove(Node);   
        }

    }
}
