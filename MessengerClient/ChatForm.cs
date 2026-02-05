using MessengerClient.Windows;

namespace MessengerClient
{
    public partial class ChatForm : Form
    {
        ChatWindow m_chatWindow;
        public bool isListShown = true;
        int ListWidthMax = 290;
        int ListWidthMin = 10;

        public ChatForm()
        { 
            InitializeComponent();
            TabBar.Size = new Size(Width, TabBar.Height);
            ListPanel.Size = new Size(ListPanel.Width, Height);
            ToolBar.Size = new Size(ToolBar.Width, Height);
            m_chatWindow = new ChatWindow(
                "General", 
                new Point(ToolBar.Width + ListPanel.Width, TabBar.Height), 
                new Size(Width - ToolBar.Width - ListPanel.Width, Height - TabBar.Height - 39
                ));

            if (isListShown) HideButton.Text = "H";
            else HideButton.Text = "S";

            Controls.Add(m_chatWindow.ChatPanel);
        }

        private void Chat_Resize(object sender, EventArgs e)
        {
            TabBar.Size = new Size(Width, TabBar.Height);
            ToolBar.Size = new Size(ToolBar.Width, Height);
            ListPanel.Size = new Size(ListPanel.Width, Height - TabBar.Height);
            m_chatWindow.SetSize(new Size(Width - ListPanel.Width - ToolBar.Width - 10, Height - TabBar.Height - 39));
        }

        private void HideButton_Click(object sender, EventArgs e)
        {
            if (isListShown) HideButton.Text = "S";
            else HideButton.Text = "H";
            Thread SizeChanger = new Thread(() => ChangeListVisibility());
            SizeChanger.Start();
        }

        private void ListPanel_Click(object sender, EventArgs e)
        {
            if (isListShown == false)
            {
                Thread SizeChanger = new Thread(() => ChangeListVisibility());
                SizeChanger.Start();
            }
        }

        private void ChangeListVisibility()
        {
            if (isListShown)
            {
                if (ListPanel.Width != ListWidthMax) return;
                for (int i = ListPanel.Width; ListPanel.Width > ListWidthMin; i--)
                {
                    Thread.Sleep(1);
                    int x = ListPanel.Size.Width * i / ListWidthMax + 1;
                    Invoke(() => ListPanel.Size = new Size(x, ListPanel.Height));
                    if (ListPanel.Width <= ListWidthMin)
                    {
                        Invoke(() => ListPanel.Width = ListWidthMin);
                    }
                }
                isListShown = false;
            }
            else
            {
                if (ListPanel.Width != ListWidthMin) return;
                int x = 0;
                while (ListPanel.Width < ListWidthMax)
                {
                    Thread.Sleep(1);
                    int y = ListPanel.Width;
                    Invoke(() => ListPanel.Size = new Size(x + y / 3, ListPanel.Height));
                    if (ListPanel.Width >= ListWidthMax)
                    {
                        Invoke(() => ListPanel.Width = ListWidthMax);
                    }
                    x = y;
                }
                isListShown = true;
            }
        }
    }
}
