using MessengerClient.Client.Services;
using MessengerClient.Client.Transport;
using MessengerClient.Client;
using MessengerShared.Requests;
using MessengerShared.API;
using System.Text.Json;

namespace MessengerClient
{
    public partial class Hello : Form
    {
        public bool ServerDone = false;
        public bool ClientDone = false;
        public bool Registrating = true;
        private ITransport _transport;
        private NetworkService _networkService;
        public Hello(NetworkService network, ITransport transport)
        {
            _transport = transport;
            _networkService = network;
            InitializeComponent();
        }

        private void Hello_Load(object sender, EventArgs e)
        {

        }

        private void sel1_Selecting(object sender, TabControlCancelEventArgs e)
        {
            if (e.TabPage == Account && !ServerDone)
            {
                e.Cancel = true;
            }
            if (e.TabPage == Server && ServerDone)
            {
                e.Cancel = true;
            }
        }

        private async void serverConnect_Click(object sender, EventArgs e)
        {
            try
            {
                State.IP = ipBox.Text;
                State.Port = int.Parse(portBox.Text);
                await _transport.ConnectAsync(State.IP, State.Port);
                ServS();
            }
            catch (Exception ex) { ServF(); }
        }

        public void ServS() { ServerDone = true; sErrLable.Hide(); sel1.TabPages[1].Focus(); }

        public void ServF() { sErrLable.Show(); }

        private void ApplyData()
        {
            if (LoginBox.Text == "" || PasswordBox.Text == "")
            {
                LogInErr.Text = "Login and password cannot be empty.";
                LogInErr.Show();
                throw new Exception("Empty");
            }

            State.Login = LoginBox.Text;
            State.Password = PasswordBox.Text;
            LogInErr.Hide();
        }

        private async void SingInBut_Click(object sender, EventArgs e)
        {
            try
            {
                ApplyData();
            }
            catch { return; }

            Responce responce = await _networkService.Login();

            if (responce.Success)
            {
                ClientDone = true;
                State.Session = JsonSerializer.Deserialize<Session>(JsonSerializer.Serialize(responce.Data));
                this.Close();
            }
            else
            {
                if (responce.Error == ServerCodes.WrongPassword)
                    LogInErr.Text = "Login failed. Wrong Password.";
                if (responce.Error == ServerCodes.NoTargetUser)
                    LogInErr.Text = "Login failed. No such user.";
                LogInErr.Show();
            }
        }

        private async void SingUpBut_Click(object sender, EventArgs e)
        {
            try
            {
                ApplyData();
            }
            catch { return; }

            Responce responce = await _networkService.Registrate();

            if (responce.Success)
            {
                ClientDone = true;
                State.Session = JsonSerializer.Deserialize<Session>(JsonSerializer.Serialize(responce.Data));
                this.Close();
            }
            else
            {
                if (responce.Error == ServerCodes.ClientAlreadyExist)
                    LogInErr.Text = "Registration failed. User already exists.";
                LogInErr.Show();
            }
        }
    }
}
