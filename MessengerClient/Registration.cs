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
            string[] fragments = IPBox.Text.Split(':', 2);
            if (!string.IsNullOrEmpty(NameBox.Text) && fragments.Length == 2)
            {
                Program.NickName = NameBox.Text;
                Program.IP = fragments[0];
                Program.Port = Convert.ToInt32(fragments[1]);
                Program.client.SetName(Program.NickName);
                Program.client.Connect(Program.IP, Program.Port);
                Close();
            } 
        }
       
    }
}
