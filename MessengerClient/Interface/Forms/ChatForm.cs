using MessengerClient.Interface;

namespace MessengerClient
{
    public partial class ChatForm : Form
    {
        private System.Windows.Forms.Timer frame = new System.Windows.Forms.Timer();
        private Panel _MainPanel = new Panel();
        private ChatWindow _ChatContent;
        private WindowNode _ChatWindow;
        private ChatWindow _ChatContent1;
        private WindowNode _ChatWindow1;
        private LayoutRenderer _Renderer;
        private SplitNode _SplitNode;

        const int WINDOW_PADDING_X = 15 +15; //Delete +15 LATER
        const int  WINDOW_PADDING_Y = 39 +15;

        public ChatForm()
        {
            
            frame.Tick += (_, _) => _Renderer.RunFrame();

            _ChatContent = new ChatWindow(0, "ChatWindow", Program.client);
            _ChatWindow = new WindowNode(new RenderData(new Bounds(new Point(0, 0))), _ChatContent);
            _ChatContent1 = new ChatWindow(0, "ChatWindow", Program.client);
            _ChatWindow1 = new WindowNode(new RenderData(new Bounds(new Point(0, 0))), _ChatContent1);

            _SplitNode = new SplitNode(new RenderData(new Bounds(new Point(0, 0), new Size(1000,500))), _ChatWindow, _ChatWindow1, 0.5f, null);

            _MainPanel.Location = new Point(30, 30);
            InitializeComponent();
            Controls.Add(_MainPanel);
            _Renderer = new LayoutRenderer(_MainPanel);
        }

        private void ChatForm_Load(object sender, EventArgs e)
        {
            _MainPanel.Size = new Size(Width - WINDOW_PADDING_X - _MainPanel.Location.X, Height - WINDOW_PADDING_Y - _MainPanel.Location.Y);
            TabBar.Size = new Size(Width, TabBar.Height);
            ToolBar.Size = new Size(ToolBar.Width, Height);

            _Renderer.Add(_SplitNode);

            frame.Start();
        }

        private void Chat_Resize(object sender, EventArgs e)
        {
            _MainPanel.Size =new Size(
                Width - _MainPanel.Location.X - WINDOW_PADDING_X, 
                Height - _MainPanel.Location.Y - WINDOW_PADDING_Y);
            TabBar.Size = new Size(Width, TabBar.Height);
            ToolBar.Size = new Size(ToolBar.Width, Height);
        }

        private void HideButton_Click(object sender, EventArgs e)
        {

        }
    }
}
