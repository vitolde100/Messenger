using MessengerClient.Elements;

namespace MessengerClient.Windows
{

    internal class ClientList : IWindow
    {
        internal struct User
        {
            public string UserName { get; set; }
        }

        List<User> m_users = new List<User>();
        List<UserPanel> m_userPanels = new List<UserPanel>();

        Label Label = new Label();
        ScrollBar m_scrollBar;

        Size m_userPanelSize;
        Color m_backColor = Color.FromArgb(125, 125, 125);

        int offsetMultiplier = 5;
        int lastOffset = 0;
        int offset = 0;

        

        public ClientList(string name, Point location, Size size, Client client) : base(name, location, size) 
        {
            client.MessageReceived += Client_MessageReceived;
        }


        public ClientList(string name, Point location, Size size, Client client, List<User> users) : base(name, location, size)
        {
            m_users = users ?? new List<User>();
        }
        private void Client_MessageReceived(MessengerShared.ChatMessage obj)
        {
            //throw new NotImplementedException();
        }

        public override void InitializeComponents()
        {
            m_scrollBar = new VScrollBar();
            //
            //WindowPanel
            //
            WindowPanel.BackColor = m_backColor;
            WindowPanel.MouseWheel += Element_Scroll;
            //
            //Users
            //
            if (!(m_users == null || m_users.Count == 0))
            { 
                for (int i = 0; i < m_users.Count; i++)
                {
                    string userName = m_users[i].UserName ?? string.Empty;
                    m_userPanels.Add(new UserPanel(
                        userName,
                        new Point(0, m_userPanelSize.Height * i),
                        m_userPanelSize));

                    m_userPanels.Last().Panel.Location = new Point(0, i * m_userPanelSize.Height);
                    m_userPanels.Last().Panel.Size = m_userPanelSize;
                }
            }
            //
            //ScrollBar
            //
            m_scrollBar.Size = new Size(15, m_size.Height);
            m_scrollBar.Location = new Point(m_size.Width - m_scrollBar.Width, 0);
            m_scrollBar.MouseWheel += Element_Scroll;
            m_scrollBar.ValueChanged += ScrollBar_ValueChanged;
            //
            //UserPanel
            //
            m_userPanelSize = new Size(m_size.Width, 55);
            //
            //lable
            //
            Label.Text = $"Users: {m_users.Count}";
            Label.AutoSize = true;
            Label.Location = new Point(0, m_size.Height - 50);
            //
            //Add Controls
            //
            WindowPanel.Controls.Add(Label);
            WindowPanel.Controls.Add(m_scrollBar);

        }

        private void ScrollBar_ValueChanged(object? sender, EventArgs e)
        {
            int offset = m_scrollBar.Value * offsetMultiplier;
            for (int i = 0; i < m_userPanels.Count; i++)
            {
                Point newPosition = new Point(m_userPanels[i].Panel.Location.X, m_userPanels[i].Panel.Location.Y - (offset - lastOffset));
                m_userPanels[i].SetPosition(newPosition);
            }
            lastOffset = offset;
        }

        private void Element_Scroll(object? sender, MouseEventArgs e)
        {
            int Factor = e.Delta / Math.Abs(e.Delta);
            int offset = -Factor * offsetMultiplier;
            if ((m_scrollBar.Value + offset) >= m_scrollBar.Minimum &&
                (m_scrollBar.Value + offset) <= m_scrollBar.Maximum)
                m_scrollBar.Value += offset;
        }

        public void FindOrAdd(User user)
        {
            AddUser(user);
        }

        public void AddUser(User user)
        {
            m_users.Add(user);
            m_userPanels.Add(new UserPanel(
                user.UserName,
                new Point(
                    0, m_userPanelSize.Height * (m_users.Count - 1) - (offset - lastOffset)),
                    m_userPanelSize));
            WindowPanel.Controls.Add(m_userPanels.Last().Panel);
        }

        public override void SetSize(Size newSize)
        {
            WindowPanel.Size = newSize;
        }
    }
}