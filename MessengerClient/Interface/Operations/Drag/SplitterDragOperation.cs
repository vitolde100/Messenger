namespace MessengerClient.Interface
{
    internal class SplitterDragOperation : IDragOperation
    {
        private readonly SplitNode splitter;
        private readonly Point startMouse;
        private readonly float startRatio;

        public SplitterDragOperation(SplitNode splitter, Point mouseGlobal)
        {
            this.splitter = splitter;
            startMouse = mouseGlobal;
            startRatio = splitter.Ratio;
        }

        public void OnMouseMove(Point globalMouse)
        {
            float newRatio;
            if (splitter.Orientation == Orientation.Horizontal)
            {
                int delta = globalMouse.X - startMouse.X;
                newRatio = startRatio + (float)delta / splitter.Size.Width;
            }
            else
            {
                int delta = globalMouse.Y - startMouse.Y;
                newRatio = startRatio + (float)delta / splitter.Size.Height;
            }
            newRatio = Math.Clamp(newRatio, 0.1f, 0.9f);
            splitter.SetRatio(newRatio);
        }

        public void OnMouseUp()
        {
            splitter.UpdateLayout();
        }
    }
}
