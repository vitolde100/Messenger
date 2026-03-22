namespace MessengerClient.Interface
{
    internal class Divider : Panel
    {
        private bool isDragging = false;
        private Point startMouse;
        private SplitNode parentSplitter;
        private SplitterDragOperation? currentDrag;

        public Divider(SplitNode splitter)
        {
            parentSplitter = splitter;
            BackColor = Color.Gray; // для наглядности

            MouseDown += Divider_MouseDown;
            MouseMove += Divider_MouseMove;
            MouseUp += Divider_MouseUp;
        }

        private void Divider_MouseDown(object sender, MouseEventArgs e)
        {
            isDragging = true;
            startMouse = PointToScreen(e.Location);
            // Создаём drag operation
            currentDrag = new SplitterDragOperation(parentSplitter, startMouse);
            Capture = true; // мышь захвачена
            parentSplitter.isPreviewDivider = true;
        }

        private void Divider_MouseMove(object sender, MouseEventArgs e)
        {
            if (!isDragging) return;

            Point globalMouse = PointToScreen(e.Location);
            currentDrag?.OnMouseMove(globalMouse);
        }

        private void Divider_MouseUp(object sender, MouseEventArgs e)
        {
            if (!isDragging) return;

            Point globalMouse = PointToScreen(e.Location);

            isDragging = false;
            Capture = false;

            parentSplitter.isPreviewDivider = false;

            currentDrag?.OnMouseUp();
            currentDrag = null;
        }
    }
}
