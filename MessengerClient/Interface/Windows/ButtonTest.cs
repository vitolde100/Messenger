namespace MessengerClient.Interface.Windows
{
    internal class ButtonTest : ContentNode
    {
        Button _sendButton;

        public event Action<ContentNode> onControlsChanged;

        public ButtonTest(int id, string title) : base(id, title)
        {

        }

        protected override void InitializeComponents()
        {
            Controls = new List<Control>();
            _sendButton = new Button();
            //
            //m_sendButton
            //
            _sendButton.Size = new Size(120, 65);
            _sendButton.Location = new Point(15, 15);
            _sendButton.Text = "Send";
            _sendButton.BackColor = Color.FromArgb(125, 125, 125);
            _sendButton.FlatStyle = FlatStyle.Flat;
            //
            //Controls
            //
            Controls.Add(_sendButton);
            //
            onControlsChanged?.Invoke(this);
        }

        public override void SetSize(Size Size)
        {

        }
    }
}
