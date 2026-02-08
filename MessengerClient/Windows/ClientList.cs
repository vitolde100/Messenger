namespace MessengerClient.Windows
{
    internal class ClientList : IWindow
    {
        public ClientList(string name, Point location, Size size) : base(name, location, size) { }
        
        public override void InitializeInterface()
        {
        }

        public override void SetSize(Size newSize)
        {
            WindowPanel.Size = newSize;
        }
    }
}
