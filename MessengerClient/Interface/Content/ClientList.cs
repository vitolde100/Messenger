/*using MessengerShared.Requests.Data;
using MessengerClient.Interface.Elements;
using MessengerClient.Data;
using MessengerClient.Client.Transport;
using MessengerClient.Client.Protocol;

namespace MessengerClient.Interface
{
    internal class ClientList : ContentNode
    {
        List<ClientData> m_users = new List<ClientData>();
        List<UserPanel> m_userPanels = new List<UserPanel>();

        Label Label = new Label();
        ScrollBar m_scrollBar;

        Size m_size = new Size();
        Size m_userPanelSize;

        int offsetMultiplier = 5;
        int lastOffset = 0;
        int offset = 0;

        private IProtocol _client;

        public event Action<ClientData> onUserChanged;

        public ClientList(IProtocol client) : base() 
        {
            _client = client;
            client.MessageReceived += onMessageReceived;
        }

        public ClientList(IProtocol client, List<ClientData> users) : base()
        {
            _client = client;
            client.MessageReceived += onMessageReceived;
            m_users = users ?? new List<ClientData>();
        }

        public override void SetOwner(LayoutNode owner)
        {
            base.SetOwner(owner);
            _owner?.Visual.SetBackColor(Color.White);
        }

        protected override void InitializeComponents()
        {
            m_scrollBar = new VScrollBar();
            //
            //UserPanel
            //
            m_userPanelSize = new Size(m_size.Width, 55);
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
            //lable
            //
            Label.Text = $"Users: {m_users.Count}";
            Label.AutoSize = true;
            Label.Location = new Point(0, m_size.Height - 50);
            //
            //Add
            //
            Controls.Add(m_scrollBar);
            Controls.Add(Label);

        }

        public void AddUser(ClientData user)
        {
            m_users.Add(user);
            m_userPanels.Add(new UserPanel(
                user.UserName,
                new Point(
                    0, m_userPanelSize.Height * (m_users.Count - 1) - (offset - lastOffset)),
                    m_userPanelSize));
            Controls.Add(m_userPanels.Last().Panel);
            _owner?.MarkDirty(DirtyFlags.Visual);
        }

        public override void SetSize(Size newSize)
        {
            m_size = newSize;
        }

        private void onMessageReceived(ChatMessageData obj)
        {
            //throw new NotImplementedException();
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
    }
}*/