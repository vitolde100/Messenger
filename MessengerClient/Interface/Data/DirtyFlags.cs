namespace MessengerClient.Interface
{
    [Flags]
    public enum DirtyFlags
    {
        None = 0,
        Layout = 1,
        Visual = 2,
    }
}
