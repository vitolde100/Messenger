using MessengerClient.Client.Protocol;
using MessengerClient.Client.Services;

namespace MessengerClient
{
    public partial class ChatForm : Form
    {
        IProtocol _protocol;
        NetworkService _networkService;

        const int WINDOW_PADDING_X = 15;
        const int WINDOW_PADDING_Y = 39;

        private Size currentSize;

        public ChatForm(IProtocol prototcol, NetworkService networkService)
        {
            InitializeComponent();
        }

        private void ChatForm_Load(object sender, EventArgs e)
        {
            MainPanel.Size = new Size(Size.Width - WINDOW_PADDING_X, Size.Height - WINDOW_PADDING_Y);
            
        }

        private void ChatForm_SizeChanged(object sender, EventArgs e)
        {
            MainPanel.Size = new Size(Size.Width - WINDOW_PADDING_X, Size.Height - WINDOW_PADDING_Y);
        }
    }
}
