using MessengerClient.Client;
using MessengerClient.Client.Services;
using MessengerClient.Client.Transport;

namespace MessengerClient
{
    public partial class WelcomeForm : Form
    {
        ITransport _transport;
        NetworkService _networkService;

        public WelcomeForm(NetworkService networkService, ITransport transport)
        {
            InitializeComponent();
            _transport = transport;
            _networkService = networkService;
        }

        private void SingUp_Click(object sender, EventArgs e)
        {
            OpenEnterForm(true);
        }

        private void SingIn_Click(object sender, EventArgs e)
        {
            OpenEnterForm(false);
        }

        private void OpenEnterForm(bool isSignUp)
        {
            using (var form = new Enter(_networkService, _transport, isSignUp))
            {
                this.Hide();

                var result = form.ShowDialog();

                if (result == DialogResult.OK && _transport.isConnected && State.isLoggedIn)
                {
                    this.Close();
                    return;
                }

                this.Show();
            }
        }
    }
}
