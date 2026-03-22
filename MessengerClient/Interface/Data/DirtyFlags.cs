namespace MessengerClient.Interface
{
    [Flags]
    internal enum DirtyFlags
    {
        None = 0,
        Layout = 1,
        Visual = 2,
    }
}
