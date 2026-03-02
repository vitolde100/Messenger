using MessengerClient.Windows;
using MessengerShared;
namespace MessengerClient
{
    public partial class Test_Form : Form
    {
        ChatWindow m_chat = new ChatWindow("chat", new Point(0, 0), new Size(500, 500), Program.client);
        ClientList m_clientList = new ClientList("clientList", new Point(500, 0), new Size(200, 500), Program.client);
        public Test_Form()
        {
            InitializeComponent();
        }

        private void Test_Form_Load(object sender, EventArgs e)
        {
            Controls.Add(m_chat.WindowPanel);
            Controls.Add(m_clientList.WindowPanel);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            m_chat.SetTarget(textBox1.Text);
        }

        private void vScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            TestLable.Text = vScrollBar1.Value.ToString();
            TestLable.Location = new Point(TestLable.Location.X, vScrollBar1.Value * 3);
        }
    }
}