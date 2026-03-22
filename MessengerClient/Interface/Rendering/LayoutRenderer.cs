namespace MessengerClient.Interface
{
    internal class LayoutRenderer
    {
        private Panel _rootPanel;
        private Dictionary<LayoutNode, Panel> _panels = new();
        private LayoutNode _tree = new LayoutNode(null);

        public LayoutRenderer(Panel panel)
        {
            _tree.Visual.SetBackColor(panel.BackColor);
            _rootPanel = panel;
            _rootPanel.BackColor = Color.Blue;
            _panels.Add(_tree, _rootPanel);
        }

        public void SetSize(Size newSize)
        {
            _tree.SetSize(newSize);
        }

        public void RunFrame()
        {
            Render(_tree);
        }

        public void Render(LayoutNode node)
        {
            if (node.Dirty == DirtyFlags.None) return;
                
            if (!_panels.ContainsKey(node)) _panels.Add(node, null);
            
            RenderData? data = node.GetRenderData();
            Panel panel;

            if (_panels[node] == null) panel = new Panel();
            else panel = _panels[node];

            if ((node.Dirty & DirtyFlags.Layout) != 0)
            {
                if (data != null)
                {
                    panel.Location = data.Bounds.Position;
                    panel.Size = data.Bounds.Size;
                }
            }

            if ((node.Dirty & DirtyFlags.Visual) != 0)
            {
                if (panel.BackColor != node.Visual.BackColor) 
                    panel.BackColor = node.Visual.BackColor;

                if (data?.Controls != null)
                    foreach (Control control in data.Controls)
                    {
                        if (!panel.Controls.Contains(control)) panel.Controls.Add(control);
                    }
            }

            if (node.Childrens != null)
                foreach (LayoutNode child in node.Childrens)
                {
                    Render(child);
                    if (!panel.Controls.Contains(_panels[child]))
                        panel.Controls.Add(_panels[child]);
                }

            _panels[node] = panel;

            node.ClearDirty();
        }
        
        public void Add(LayoutNode node)
        {
            node.SetParent(_tree);
        }
    }
}
