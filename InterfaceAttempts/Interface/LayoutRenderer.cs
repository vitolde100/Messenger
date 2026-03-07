namespace InterfaceAttempts.Interface
{
    internal class LayoutRenderer
    {
        private Panel _rootPanel;

        private Dictionary<LayoutNode, Panel> Tree = new();
        
        public LayoutRenderer(Panel panel)
        {
            _rootPanel = panel;
        }

        public void RenderTree()
        {
            if (Tree != null)
            {
                foreach (var pair in Tree)
                {
                    _rootPanel.Controls.Add(Render(pair.Key));
                }
            }
            else throw new Exception();
        }

        public Panel Render(LayoutNode node)
        {
            RenderData data = node.GetRenderData();
            Panel panel;

            if (Tree[node] == null) 
            {  
                panel = new Panel();
                Tree[node] = panel;
            }
            else panel = Tree[node];
            panel.BackColor = Color.FromArgb(125, 125, 125);
            PreparePanel(data, ref panel);

            if (data.Childrens != null)
                foreach (var child in data.Childrens)
                    panel.Controls.Add(Render(child));

            return panel;
        }
        
        public void Add(LayoutNode node)
        {
            Tree.Add(node, null);
        }

        private void PreparePanel(RenderData data, ref Panel panel)
        {
            panel.Location = data.Bounds.Position;
            panel.Size = data.Bounds.Size;
            if (data.Controls != null)
                foreach (var control in data.Controls) panel.Controls.Add(control);
        }
    }
}
