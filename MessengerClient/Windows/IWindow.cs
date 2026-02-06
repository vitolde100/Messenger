namespace MessengerClient.Windows
{
    public abstract class IWindow
    {
        string m_name;
        Point m_position;
        Size m_size;

        public Panel WindowPanel = new Panel();

        public IWindow(string name, Point position, Size size) 
        {
            m_name = name;
            m_position = position;
            m_size = size;
            InitializeInterface();
        }

        abstract public void InitializeInterface();

        abstract public void SetSize(Size newSize);

        public void SetPosition(Point newPosition)
        {
            m_position = newPosition;
            WindowPanel.Location = m_position;
        }

        public void Show()
        {
            for (double i = Math.PI; i >= 0; i -= 0.1f)
            {
                SetSize(new Size(
                    (int)(Math.Cos(m_size.Width * i)),
                    (int)(Math.Sin(m_size.Height * i))
                ));
                Thread.Sleep(1);
            }
        }

        public void Hide()
        {
            for (double i = Math.PI; i >= 0; i += 0.1f)
            {
                SetSize(new Size(
                    (int)(Math.Cos(m_size.Width * i)),
                    (int)(Math.Sin(m_size.Height * i))
                ));
                Thread.Sleep(1);
            }
        }
    } 
}
