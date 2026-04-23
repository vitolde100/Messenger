using MessengerClient.Client.Services;
using MessengerShared.Requests.Data;
using System.Text.Json;

namespace MessengerClient.Interface.Forms
{
    public partial class TestForm : Form
    {
        private NetworkService _networkService = Program.AppContext.NetworkService;
        public TestForm()
        {
            InitializeComponent();
            SessionTextBox.Text = JsonSerializer.Serialize(Program.state.Session, new JsonSerializerOptions { WriteIndented = true });
            Program.AppContext.AuthService.OnReloginRequired += AuthService_OnReloginRequired;
        }
        private async void TestForm_Load(object sender, EventArgs e)
        {
            await Program.AppContext.AuthService.Auth();
        }

        private void AuthService_OnReloginRequired()
        {
            Application.Exit();
        }

        private async void SendButton_Click(object sender, EventArgs e)
        {
            var message = new ChatMessageData(TargetBox.Text, MessageTextBox.Text);
            var Responce = await _networkService.SendMessage(message);
            if (!Responce.Success) { MSGErrorLable.Text = Responce.Error.ToString(); }
            else { TextTextBox.Text = JsonSerializer.Serialize(Program.state.Session, new JsonSerializerOptions { WriteIndented = true }); }
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            var Responce = await _networkService.GetContact(UIDBox.Text);
            if (!Responce.Success) { GroupErrorLable.Text = Responce.Error.ToString(); }
            else { TextTextBox.Text = Responce.Error.ToString(); }
        }

    }
}
