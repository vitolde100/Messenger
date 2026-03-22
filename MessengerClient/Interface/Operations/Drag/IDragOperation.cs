namespace MessengerClient.Interface
{
    internal interface IDragOperation
    {
        public void OnMouseMove(Point GlobalPos);

        public void OnMouseUp();
    }
}
