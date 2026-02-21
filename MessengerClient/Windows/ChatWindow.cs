using MessengerClient.Elements;
using MessengerShared;

namespace MessengerClient.Windows
{
    internal partial class ChatWindow : IWindow
    {
        List<ChatMessage> m_messages = new List<ChatMessage>();
        List<Elements.Message> m_messagePanels = new List<Elements.Message>();

        Panel m_userBar;
        ScrollBar m_scrollBar;
        TextBox m_inputBox;
        Button m_sendButton;

        int m_messagesLength = 0;
        int m_lastOffset = 0;
        int m_offsetMultiplier = 5;
        const int PADDING_Y = 5;

        string m_target;

        Color m_backColor = Color.FromArgb(64, 64, 64);
        private Client m_client;

        public ChatWindow(string name, Point position, Size size, Client client) : base(name, position, size)
        {
            m_client = client;
            m_client.MessageReceived += AddMessage;
        }

        public override void InitializeComponents()
        {
            m_userBar = new Panel();
            m_scrollBar = new VScrollBar();
            m_inputBox = new TextBox();
            m_sendButton = new Button();
            //
            //WindowPanel
            //
            WindowPanel.BackColor = m_backColor;
            WindowPanel.MouseWheel += Element_Scroll;
            //
            //UserPanel
            //
            m_userBar.Location = new Point(0, 0);
            m_userBar.Size = new Size(m_size.Width, 40);
            m_userBar.BackColor = Color.FromArgb(125, 125, 125);
            //
            //ChatScrollBar
            //
            m_scrollBar.Size = new Size(15, m_size.Height - m_userBar.Size.Height);
            m_scrollBar.Location = new Point(m_size.Width - m_scrollBar.Width, m_userBar.Size.Height);
            m_scrollBar.MouseWheel += Element_Scroll;
            m_scrollBar.ValueChanged += ScrollBar_ValueChanged;
            //
            //InputBox
            //
            m_inputBox.Size = new Size(m_size.Width - 80, 30);
            m_inputBox.Location = new Point(0, m_size.Height - m_inputBox.Height);
            //
            //SendButton
            //
            m_sendButton.Size = new Size(65, m_inputBox.Height);
            m_sendButton.Location = new Point(m_inputBox.Size.Width, m_inputBox.Location.Y);
            m_sendButton.Text = "Send";
            m_sendButton.BackColor = Color.FromArgb(125, 125, 125);
            m_sendButton.FlatStyle = FlatStyle.Flat;
            m_sendButton.Click += SendButton_Click;
            //
            //Add
            //
            WindowPanel.Controls.Add(m_userBar);
            WindowPanel.Controls.Add(m_scrollBar);
            WindowPanel.Controls.Add(m_inputBox);
            WindowPanel.Controls.Add(m_sendButton);
        }

        public void AddMessage(ChatMessage message)
        {
            m_messages.Add(message);
            int offset = m_scrollBar.Value * m_offsetMultiplier;
            int YPos = (m_userBar.Height + m_messagesLength + (PADDING_Y * m_messages.Count) - offset);
            int XPos = 0;
            if (message.Target == m_target) XPos = 0;
            else XPos = m_size.Width - m_scrollBar.Width;
            m_messagePanels.Add(new Elements.Message(message, new Point(XPos,YPos)));
            WindowPanel.Controls.Add(m_messagePanels.Last().Panel);
            m_messagesLength += m_messagePanels.Last().Panel.Height;
        }
 
        private async void SendButton_Click(object? sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(m_inputBox.Text))
            {
                ChatMessage message = new ChatMessage();
                DateTime utcNow = DateTime.UtcNow;
                DateTime unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                message.SendTime = utcNow - unixEpoch;
                message.Target = m_target;
                message.Sender = Program.NickName;
                message.Text = m_inputBox.Text;
                AddMessage(message);
                await m_client.SendMessage(message);
                m_inputBox.Clear();
            }
        }

        public void SetTarget(string target)
        {
            m_userBar.Controls.Clear();
            Label nameLabel = new Label();
            nameLabel.AutoSize = true;
            nameLabel.Location = new Point(5, 10);
            nameLabel.Text = target;
            m_userBar.Controls.Add(nameLabel);
        }

        public override void SetSize(Size newSize)
        {
            m_size = newSize;
            WindowPanel.Size = m_size;

            m_userBar.Width = m_size.Width;

            m_scrollBar.Location = new Point(
                m_size.Width - m_scrollBar.Width,
                m_userBar.Height + m_userBar.Location.Y
            );
            m_scrollBar.Height = m_size.Height - m_userBar.Height - m_userBar.Location.Y;

            m_inputBox.Location = new Point(m_inputBox.Location.X, m_size.Height - m_inputBox.Height);
            m_inputBox.Width = (m_size.Width - m_inputBox.Location.X) - (m_sendButton.Width + m_scrollBar.Width);

            m_sendButton.Location = new Point(m_inputBox.Width + m_inputBox.Location.X, m_inputBox.Location.Y);

            foreach (Elements.Message message in m_messagePanels)
            {
                message.SetPosition(new Point(m_size.Width - m_scrollBar.Size.Width - message.Panel.Width, message.Panel.Location.Y));
            }
        }

        private void Element_Scroll(object? sender, MouseEventArgs e)
        {
            int Factor = e.Delta / Math.Abs(e.Delta);
            int offset = -Factor * m_offsetMultiplier;
            if ((m_scrollBar.Value + offset) >= m_scrollBar.Minimum &&
                (m_scrollBar.Value + offset) <= m_scrollBar.Maximum)
                m_scrollBar.Value += offset;
        }

        private void ScrollBar_ValueChanged(object? sender, EventArgs e)
        {
            int offset = m_scrollBar.Value * m_offsetMultiplier;
            for (int i = 0; i < m_messages.Count; i++)
            {
                Point newPosition = new Point(m_messagePanels[i].Panel.Location.X, m_messagePanels[i].Panel.Location.Y - (offset - m_lastOffset));
                m_messagePanels[i].SetPosition(newPosition);
            }
            m_lastOffset = offset;
        }

    }
}