namespace MessengerClient.Windows
{
    public class ChatWindow : IWindow
    {
        string m_name;
        public Point m_position;
        public Size m_size;

        Color m_backColor = Color.FromArgb(64, 64, 64);

        Panel m_userPanel;
        ScrollBar m_chatScrollBar;

        public ChatWindow(string name, Point position, Size size) : base(name, position, size)
        {
            m_name = name;
            m_position = position;
            m_size = size;
            InitializeInterface();
        }

        public override void InitializeInterface()
        {
            m_userPanel = new Panel();
            m_chatScrollBar = new VScrollBar();
            WindowPanel.SuspendLayout();
            //
            //ChatPanel
            //
            WindowPanel.Location = m_position;
            WindowPanel.Size = m_size;
            WindowPanel.BackColor = m_backColor;
            WindowPanel.Controls.Add(m_userPanel);
            WindowPanel.Controls.Add(m_chatScrollBar);
            //
            //UserPanel
            //
            m_userPanel.Location = new Point(0, 0);
            m_userPanel.Size = new Size(m_size.Width, 40);
            m_userPanel.BackColor = Color.FromArgb(125, 125, 125);
            //
            //ChatScrollBar
            //
            m_chatScrollBar.Location = new Point(m_size.Width - 25, m_userPanel.Size.Height);
            m_chatScrollBar.Size = new Size(15, m_size.Height - m_userPanel.Size.Height);
        }

        public override void SetSize(Size newSize)
        {
            m_size = newSize;
            WindowPanel.Size = m_size;
            m_userPanel.Size = new Size(m_size.Width, m_userPanel.Size.Height);
            m_chatScrollBar.Location = new Point(m_size.Width - 20, m_userPanel.Size.Height);
            m_chatScrollBar.Size = new Size(m_chatScrollBar.Size.Width, m_size.Height - m_userPanel.Size.Height);
        }
    }
}
