namespace MessengerClient.Interface
{
    internal class LayoutRenderer
    {
        private Panel _rootPanel;
        private Dictionary<LayoutNode, Panel> _panels = new();
        private LayoutNode _tree = new LayoutNode(null);
        public bool isRunning = true;
        public LayoutRenderer(Panel panel)
        {
            _rootPanel = panel;
            _rootPanel.BackColor = Color.Blue;
            _panels.Add(_tree, _rootPanel);
        }

        public void RunFrame()
        {
            Render(_tree);
        }

        public void Render(LayoutNode node)
        {
            if (node.Dirty == DirtyFlags.None) return;

            if ((node.Dirty & DirtyFlags.Layout) != 0)
            {
                RenderData? data = node.GetRenderData();

                if (!_panels.ContainsKey(node)) _panels.Add(node, null);

                if (data != null)
                {
                    Panel panel;
                    if (_panels[node] == null) panel = new Panel();
                    else panel = _panels[node];

                    panel.Location = data.Bounds.Position;
                    panel.Size = data.Bounds.Size;

                    _panels[node] = panel;

                    if (data.Controls != null)
                        foreach (Control control in data.Controls)
                        {
                            if (!_panels[node].Controls.Contains(control)) _panels[node].Controls.Add(control);
                        }
                }
            }

            if ((node.Dirty & DirtyFlags.Visual) != 0)
            {
                _panels[node].BackColor = node.Visual.BackColor;
            }

            if (node.Childrens != null)
                foreach (LayoutNode child in node.Childrens)
                {
                    Render(child);
                    if (!_panels[node].Controls.Contains(_panels[child]))
                        _panels[node].Controls.Add(_panels[child]);
                }

            node.ClearDirty();
        }
        
        public void Add(LayoutNode node)
        {
            node.SetParent(_tree);
        }
    }
}
