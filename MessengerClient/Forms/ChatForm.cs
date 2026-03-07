using MessengerClient.Windows;

namespace MessengerClient
{
    public partial class ChatForm : Form
    {
        ChatWindow m_chatWindow;
        const int WINDOW_PADDING_X = 15;
        const int  WINDOW_PADDING_Y = 39;

        public ChatForm()
        {
            InitializeComponent();
        }
        private void ChatForm_Load(object sender, EventArgs e)
        {
            TabBar.Size = new Size(Width, TabBar.Height);
            ToolBar.Size = new Size(ToolBar.Width, Height);

            m_chatWindow = new ChatWindow(
                "General",
                new Point(ToolBar.Width, TabBar.Height),
                new Size(Width - WINDOW_PADDING_X - ToolBar.Width, Height - WINDOW_PADDING_Y - TabBar.Height),
                Program.client
            );
            Controls.Add(m_chatWindow.WindowPanel);
        }

        private void Chat_Resize(object sender, EventArgs e)
        {
            TabBar.Size = new Size(Width, TabBar.Height);
            ToolBar.Size = new Size(ToolBar.Width, Height);
            m_chatWindow.SetSize(new Size(
                Width - WINDOW_PADDING_X - ToolBar.Width, 
                Height - WINDOW_PADDING_Y - TabBar.Height));
        }

        private void HideButton_Click(object sender, EventArgs e)
        {

        }
    }
}
