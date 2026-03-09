using InterfaceAttempts.Interface;
using InterfaceAttempts.Windows;

namespace InterfaceAttempts
{
    public partial class Form1 : Form
    {
        Panel MainPanel = new Panel();
        LayoutRenderer _Renderer;
        public Form1()
        {
            InitializeComponent();
            MainPanel.Size = new Size(Width, Height);
            MainPanel.BackColor = Color.Red;
            _Renderer = new LayoutRenderer(MainPanel);
            Controls.Add(MainPanel);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ButtonTest but1 = new ButtonTest(0, "HelloWorld");
            Bounds bounds = new Bounds(new Point(0, 0), new Size(Width, Height));

            WindowNode win1 = new WindowNode(new RenderData(bounds), but1);

            _Renderer.Add(win1);

            _Renderer.RenderTree();
        }
    }
}
