using MessengerClient.Client.Services;
using MessengerClient.Client.Protocol;
using MessengerClient.Client.Transport;
using MessengerShared.API;
using MessengerClient.Interface.Forms;
using MessengerClient.Properties;
using MessengerShared.Requests.Enums;
using MessengerShared.Requests;

namespace MessengerClient
{
    public partial class HelloForm : Form
    {
        private string Password;

        public bool ServerDone = false;
        public bool ClientDone = false;
        public bool Registrating = true;
        private ITransport _transport = Program.AppContext.Transport;
        private IProtocol _protocol = Program.AppContext.Protocol;
        private NetworkService _networkService = Program.AppContext.NetworkService;
        public HelloForm()
        {
            InitializeComponent();
            if (_transport.IsConnected)
            {
                ServS();
            }
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
                Program.state.IP = ipBox.Text;
                Program.state.Port = int.Parse(portBox.Text);
                await _transport.ConnectAsync(Program.state.IP, Program.state.Port);
                new Thread(() => _protocol.RunRecieveloop()).Start();
                ServS();
            }
            catch (Exception ex) { ServF(); }
        }

        public void ServS() { ServerDone = true; sErrLable.Hide(); sel1.SelectTab(1); }

        public void ServF() { sErrLable.Show(); }

        private void ApplyData()
        {
            if (LoginBox.Text == "" || PasswordBox.Text == "")
            {
                LogInErr.Text = "Login and password cannot be empty.";
                LogInErr.Show();
                throw new Exception("Empty");
            }

            Program.state.Login = LoginBox.Text;
            Password = PasswordBox.Text;
            LogInErr.Hide();
        }

        private async void SingInBut_Click(object sender, EventArgs e)
        {
            try
            {
                ApplyData();
            }
            catch { return; }

            Response responce = await _networkService.Login(Program.state.Login, Password);

            if (responce.Success)
            {
                ClientDone = true;
                var session = (Session)responce.Data;
                Program.state.UserID = session.userID;
                Program.state.Session = session;
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

            Response responce = await _networkService.Registrate(Program.state.Login, Password);

            if (responce.Success)
            {
                ClientDone = true;

                var session = (Session)responce.Data;
                Program.state.UserID = session.userID;
                Program.state.Session = session;
                this.Close();
            }
            else
            {
                if (responce.Error == ServerCodes.ClientAlreadyExist)
                    LogInErr.Text = "Registration failed. User already exists.";
                LogInErr.Show();
            }
        }

        private void ipBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            pictureBox3.Enabled = false;
            SingInBut.Enabled = true;
            SingUpBut.Enabled = true;
            pictureBox3.Image = Resources.cS;
        }
    }
}
