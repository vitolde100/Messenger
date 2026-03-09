using MessengerClient.Interface;

namespace MessengerClient
{
    public partial class ChatForm : Form
    {
        private Panel _MainPanel = new Panel();
        private ChatWindow _ChatContent;
        private WindowNode _ChatWindow;

        private LayoutRenderer _Renderer;
        const int WINDOW_PADDING_X = 15;
        const int  WINDOW_PADDING_Y = 39;

        public ChatForm()
        {
            _MainPanel.Location = new Point(30,30);
            _MainPanel.Size = new Size(Width - WINDOW_PADDING_X, Height - WINDOW_PADDING_Y);

            _ChatContent = new ChatWindow(0, "ChatWindow", Program.client);
            _ChatWindow = new WindowNode(new RenderData(new Bounds(new Point(0, 0))), _ChatContent);

            InitializeComponent();
            Controls.Add(_MainPanel);
            _Renderer = new LayoutRenderer(_MainPanel);
        }

        private void ChatForm_Load(object sender, EventArgs e)
        {

            _Renderer.Add(_ChatWindow);
            _Renderer.RenderTree();
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
