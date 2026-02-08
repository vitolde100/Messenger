using MessengerClient.Windows;
using MessengerShared;
using System.Windows.Forms;
namespace MessengerClient
{
    public partial class Test_Form : Form
    {
        ChatWindow chat = new ChatWindow("chat", new Point(0, 0), new Size(600, 300));
        public Test_Form()
        {
            InitializeComponent();
        }

        private void Test_Form_Load(object sender, EventArgs e)
        {
            Controls.Add(chat.WindowPanel);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageArguments message = new MessageArguments();
            message.Data = "Hello World!HHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHH";
            message.Sender = null;
            chat.AddMessage(message);
        }
    }
}