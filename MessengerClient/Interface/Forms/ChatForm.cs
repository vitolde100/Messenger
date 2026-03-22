using MessengerClient.Interface;

namespace MessengerClient
{
    public partial class ChatForm : Form
    {
        System.Windows.Forms.Timer frame = new System.Windows.Forms.Timer();
        Panel _MainPanel = new Panel();

        LayoutRenderer _Renderer;

        const int WINDOW_PADDING_X = 15;
        const int WINDOW_PADDING_Y = 39;

        private Size currentSize;

        public ChatForm()
        {
            _Renderer = new LayoutRenderer(_MainPanel);
            frame.Tick += (_, _) => _Renderer.RunFrame();
            frame.Interval = 8;

            _MainPanel.BackColor = SystemColors.Window;
            InitializeComponent();
            Controls.Add(_MainPanel);

        }

        private void ChatForm_Load(object sender, EventArgs e)
        {
            currentSize = new Size(Size.Width - WINDOW_PADDING_X, Size.Height - WINDOW_PADDING_Y);

            ContentNode content = new ChatWindow(Program.client);
            ContentNode content1 = new ClientList(Program.client);
            WindowNode window = new WindowNode(new RenderData(),content);
            WindowNode window1 = new WindowNode(new RenderData(), content1);
            
            _Renderer.Add(new SplitNode(
                new RenderData(new Bounds()),
                window1,
                window,
                0.2f
            ));


            _Renderer.SetSize(Size);
            frame.Start();
        }

        private void ChatForm_SizeChanged(object sender, EventArgs e)
        {
            currentSize = new Size(Size.Width - WINDOW_PADDING_X, Size.Height - WINDOW_PADDING_Y);
            _Renderer.SetSize(currentSize);
        }
    }
}
