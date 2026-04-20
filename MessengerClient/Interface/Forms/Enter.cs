using MessengerClient.Client;
using MessengerClient.Client.Services;
using MessengerClient.Client.Transport;
using MessengerShared.API;
using MessengerShared.Requests;
using System.Text.Json;
namespace MessengerClient
{

    public partial class Enter : Form
    {
    ITransport _transport;
    NetworkService _networkService;
    bool _isSignUp;

    public Enter(NetworkService networkService, ITransport transport, bool isSignUp)
    {
        InitializeComponent();
        _transport = transport;
        _networkService = networkService;
        _isSignUp = isSignUp;
    }

    private async void ConnectButton_Click(object sender, EventArgs e)
    {
        ErrorLable.Text = "";

        if (string.IsNullOrWhiteSpace(LoginBox.Text))
        {
            ErrorLable.Text = "Invalid Login";
            return;
        }

        if (string.IsNullOrWhiteSpace(PasswordBox.Text))
        {
            ErrorLable.Text = "Invalid Password";
            return;
        }

        if (string.IsNullOrWhiteSpace(IPBox.Text))
        {
            ErrorLable.Text = "Invalid Address";
            return;
        }

        var parts = IPBox.Text.Split(':');
        if (parts.Length != 2 || !int.TryParse(parts[1], out int port))
        {
            ErrorLable.Text = "Invalid Address";
            return;
        }

        State.Login = LoginBox.Text;
        State.Password = PasswordBox.Text;
        State.IP = parts[0];
        State.Port = port;

        try
        {
            await _transport.ConnectAsync(State.IP, State.Port);

            Responce response;

            if (_isSignUp)
                response = await _networkService.Registrate();
            else
                response = await _networkService.Login();

            if (!response.Success)
            {
                ErrorLable.Text = response.Error.ToString() ?? "Request failed";
                return;
            }

            Session? session = null;

            try
            {
                session = JsonSerializer.Deserialize<Session>(JsonSerializer.Serialize(response.Data));
            }
            catch
            {
                ErrorLable.Text = "Session parse error";
                return;
            }

            if (session == null)
            {
                ErrorLable.Text = "Session is null";
                return;
            }

            State.Session = session;
            State.UserID = session.userID;
            State.isLoggedIn = true;

            DialogResult = DialogResult.OK;
            this.Close();
        }
        catch (Exception ex)
        {
            ErrorLable.Text = "Connection error";
            Console.WriteLine(ex.Message);
        }
    }
    }
}