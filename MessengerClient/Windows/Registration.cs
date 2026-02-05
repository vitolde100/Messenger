namespace MessengerClient
{
    public partial class Registration : Form
    {
        public Registration()
        {
            InitializeComponent();
        }

        void ConnectButton_Click(object sender, EventArgs e)
        {
            string[] Adressfragments = IPBox.Text.Split(':', 2);
            if (string.IsNullOrEmpty(NameBox.Text))
            {
                ConnectionExLable.Text = "Invalid Nickname";
                ConnectionExLable.Visible = true;
                return;
            }
            else
            {
                Program.NickName = NameBox.Text;
            }

            if (Adressfragments.Length != 2)
            {
                ConnectionExLable.Text = "Invalid IP Adress";
                ConnectionExLable.Visible = true;
                return;
            }
            else
            {
                Program.IP = Adressfragments[0];
                try
                {
                    Program.Port = Convert.ToInt32(Adressfragments[1]);
                }
                catch
                {
                    ConnectionExLable.Text = "Invalid IP Adress";
                    return;
                }
            }

            Program.client.SetName(Program.NickName);

            Exception ex = null;
            Thread ConnectThread = new Thread(() =>
            {
                try
                {
                    Program.client.TryConnect(Program.IP, Program.Port);
                }
                catch (Exception e)
                {
                    ex = e;
                }
            });
            ConnectThread.Start();
            if (!ConnectThread.Join(5000))
            {
                ex = new TimeoutException("Connection timed out");
            }

            if (ex != null)
            {
                ConnectionExLable.Text = ex.Message;
                ConnectionExLable.Visible = true;
                return;
            }
            else
            {
                Program.isConnected = true;
                Close();
            }
        }
    }
}
