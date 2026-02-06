using MessengerClient.Windows;

namespace MessengerClient
{
    public partial class ChatForm : Form
    {
        ChatWindow m_chatWindow;
        public bool isListShown = true;
        int ListWidthMax = 600;
        int ListWidthMin = 10;

        public ChatForm()
        { 
            InitializeComponent();
            TabBar.Size = new Size(Width, TabBar.Height);
            ListPanel.Size = new Size(ListPanel.Width, Height);
            ToolBar.Size = new Size(ToolBar.Width, Height);

            /*m_chatWindow = new ChatWindow(
                "General", 
                new Point(ToolBar.Width + ListPanel.Width, TabBar.Height), 
                new Size(Width - ToolBar.Width - ListPanel.Width, Height - TabBar.Height - 39
            ));*/

            if (isListShown) HideButton.Text = "H";
            else HideButton.Text = "S";

            //Controls.Add(m_chatWindow.WindowPanel);
        }

        private void Chat_Resize(object sender, EventArgs e)
        {
            TabBar.Size = new Size(Width, TabBar.Height);
            ToolBar.Size = new Size(ToolBar.Width, Height);
            ListPanel.Size = new Size(ListPanel.Width, Height - TabBar.Height);
            //m_chatWindow.SetSize(new Size(Width - ListPanel.Width - ToolBar.Width - 10, Height - TabBar.Height - 39));
        }

        private void HideButton_Click(object sender, EventArgs e)
        {
            if (isListShown) HideButton.Text = "S";
            else HideButton.Text = "H";
            Thread SizeChanger = new Thread(() => ChangeListVisibility()); //ChangeListVisibility()
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
            if (ListPanel.Width == ListWidthMax)
            {
                for (double i = Math.PI / 2; i >= 0; i -= 0.1f)
                {
                    if (ListPanel.Width <= ListWidthMin)
                    {
                        Invoke(() => ListPanel.Width = ListWidthMin);
                        break;
                    }
                    Invoke(() => ListPanel.Size = new Size((int)(ListPanel.Width * Math.Sin(i)), ListPanel.Height));
                    Thread.Sleep(1);
                }
                Invoke(() => ListPanel.Width = ListWidthMin);
                isListShown = false;
            }
            else if (ListPanel.Width == ListWidthMin)
            {
                for (double i = 0; i <= Math.PI / 2; i += 0.1f)
                {
                    if (ListPanel.Width >= ListWidthMax)
                    {
                        Invoke(() => ListPanel.Width = ListWidthMax);
                        break;
                    }
                    Invoke(() => ListPanel.Size = new Size((int)(ListWidthMax * Math.Sin(i)), ListPanel.Height));
                    Thread.Sleep(1);
                }
                Invoke(() => ListPanel.Width = ListWidthMax);
                isListShown = true;
            }
        }    
    }
}
