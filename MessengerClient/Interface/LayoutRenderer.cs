using MessengerClient.Interface;

namespace MessengerClient
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
                    _rootPanel.Controls.Add(Render(pair.Key));
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

            panel.Location = data.Bounds.Position;
            panel.Size = data.Bounds.Size;
            panel.BackColor = Color.FromArgb(125, 125, 125);
            if (data.Controls != null)
                foreach (Control control in data.Controls) panel.Controls.Add(control);

            if (data.Childrens != null)
                foreach (LayoutNode child in data.Childrens)
                    panel.Controls.Add(Render(child));

            return panel;
        }
        
        public void Add(LayoutNode node)
        {
            Tree.Add(node, null);
        }
    }
}
