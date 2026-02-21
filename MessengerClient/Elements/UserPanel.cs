namespace MessengerClient.Elements
{
    class UserPanel
    {
        public Panel Panel = new Panel();
        Label m_label = new Label();

        string m_userName;
        Point m_position = new Point();
        Size m_size = new Size();

        int scrolledOffset = 0;

        public UserPanel(string userName, Point position, Size size)
        {
            m_userName = userName;
            m_position = position;
            m_size = size;
            InitializeComponents();
        }

        public void InitializeComponents()
        {
            //
            //Panel
            //
            Panel.Location = m_position;
            Panel.Size = m_size;
            Panel.BackColor = Color.FromArgb(32,32,32);
            //
            //Label
            //
            m_label.ForeColor = Color.White;
            m_label.Text = "UserName";
            m_label.Location = new Point(10, 10);
            //
            //Add controls
            //
            Panel.Controls.Add(m_label);
        }

        public void SetSize(Size newSize)
        {
            Panel.Size = newSize;
        }

        public void SetPosition(Point newPosition)
        {
            m_position = newPosition;
            Panel.Location = newPosition;
        }
    }
}
