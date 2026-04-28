using MessengerClient.Client.Services;
using MessengerShared.Requests.Data;
using System.Text.Encodings.Web;
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
            Program.AppContext.Protocol.OnMessageReceived += MessageReceived;
        }

        private readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };

        private async void TestForm_Load(object sender, EventArgs e)
        {

        }

        private void AuthService_OnReloginRequired()
        {
            Application.Exit();
        }

        private void MessageReceived(ChatMessageData obj)
        {
            if (TextTextBox.InvokeRequired)
            {
                TextTextBox.Invoke(new Action(() =>
                {
                    TextTextBox.Text = JsonSerializer.Serialize(
                        obj,
                        _jsonOptions
                    );
                }));
                return;
            }

            TextTextBox.Text = JsonSerializer.Serialize(
                obj,
                _jsonOptions
            );
        }

        private async void SendButton_Click(object sender, EventArgs e)
        {
            var message = new ChatMessageData(TargetBox.Text, MessageTextBox.Text);
            var Response = await _networkService.SendMessage(message);
            if (!Response.Success) { MSGErrorLable.Text = Response.Error.ToString(); }
            TextTextBox.Text = JsonSerializer.Serialize(Response, _jsonOptions);
        }

        private async void GetButton_Click(object sender, EventArgs e)
        {
            var Response = await _networkService.GetContact(LoginBox.Text);
            if (!Response.Success) { UserListError.Text = Response.Error.ToString(); }
            UserList.Text = JsonSerializer.Serialize(Response, _jsonOptions);
        }

        private async void AddButton_Click(object sender, EventArgs e)
        {
            var Response = await _networkService.AddToChat(UIDBox.Text, GroupIDBox.Text);
            if (!Response.Success) { GroupErrorLable.Text = Response.Error.ToString(); }
            ChatsBox.Text = JsonSerializer.Serialize(Response, _jsonOptions);
        }

        private async void CreateButton_Click(object sender, EventArgs e)
        {
            var Response = await _networkService.CreateChat(true, NameBox.Text);
            if (!Response.Success) { GroupErrorLable.Text = Response.Error.ToString(); }
            ChatsBox.Text = JsonSerializer.Serialize(Response, _jsonOptions);
        }
    }
}
