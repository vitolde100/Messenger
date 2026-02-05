namespace MessengerClient.Windows
{
    public class ChatWindow
    {
        string m_name;
        public Point m_position;
        public Size m_size;

        Color m_backColor = Color.FromArgb(64, 64, 64);

        public Panel ChatPanel;
        Panel UserPanel;
        ScrollBar ChatScrollBar;

        public ChatWindow(string name, Point position, Size size)
        {
            m_name = name;
            m_position = position;
            m_size = size;
            InitializeInterface();
        }

        private void InitializeInterface()
        {
            ChatPanel = new Panel();
            UserPanel = new Panel();
            ChatScrollBar = new VScrollBar();
            ChatPanel.SuspendLayout();
            //
            //ChatPanel
            //
            ChatPanel.Location = m_position;
            ChatPanel.Size = m_size;
            ChatPanel.BackColor = m_backColor;
            ChatPanel.Controls.Add(UserPanel);
            ChatPanel.Controls.Add(ChatScrollBar);
            //
            //UserPanel
            //
            UserPanel.Location = new Point(0, 0);
            UserPanel.Size = new Size(m_size.Width, 40);
            UserPanel.BackColor = Color.FromArgb(125, 125, 125);
            //
            //ChatScrollBar
            //
            ChatScrollBar.Location = new Point(m_size.Width - 25, UserPanel.Size.Height);
            ChatScrollBar.Size = new Size(15, m_size.Height - UserPanel.Size.Height);

        }

        public void SetSize(Size newSize)
        {
            m_size = newSize;
            ChatPanel.Size = m_size;
            UserPanel.Size = new Size(m_size.Width, UserPanel.Size.Height);
            ChatScrollBar.Location = new Point(m_size.Width - 20, UserPanel.Size.Height);
            ChatScrollBar.Size = new Size(ChatScrollBar.Size.Width, m_size.Height - UserPanel.Size.Height);
        }
        
        public void SetPosition(Point newPosition)
        {
            m_position = newPosition;
            ChatPanel.Location = m_position;
        }
    }
}
