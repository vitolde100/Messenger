using System.Numerics;

namespace MessengerClient.Windows
{
    public abstract class IWindow
    {
        private struct AnimationData
        {
            public Size TargetSize;
            public Vector2D Progress;
            public double MaxProgress;
            public Vector2 Factor;

            public struct Vector2D
            {
                public double X;
                public double Y;

                public Vector2D(double x, double y)
                {
                    X = x;
                    Y = y;
                }
            }
            public void FindFactor(Size currentSize)
            {
                Vector2 Factor;
                Factor.X = TargetSize.Width - currentSize.Width;
                Factor.Y = TargetSize.Height - currentSize.Height;
            }
        }
        private const int FrameRate = 8;

        protected string m_name;
        protected Point m_position;
        protected Size m_size;

        public Panel WindowPanel = new Panel();

        private AnimationData m_animationData;
        private System.Windows.Forms.Timer m_animationTimer;

        public IWindow(string name, Point position, Size size)
        {
            m_name = name;
            m_position = position;
            m_size = size;

            WindowPanel.Location = m_position;
            WindowPanel.Size = m_size;
            InitializeComponents();
        }

        abstract public void InitializeComponents();

        public void SetPosition(Point newPosition)
        {
            m_position = newPosition;
            WindowPanel.Location = m_position;
        }

        public Point GetPosition()
        {
            return m_position;
        }

        public void ChangeSize(Size TargetSize)//<-- WIP
        {
            m_animationData = new AnimationData
            {
                TargetSize = TargetSize,
                Progress = new AnimationData.Vector2D(0,0),
                MaxProgress = Math.PI / 2
            };
            m_animationData.FindFactor(m_size);

            m_animationTimer?.Stop();
            m_animationTimer = new System.Windows.Forms.Timer();
            m_animationTimer.Interval = FrameRate;

            m_animationTimer.Tick += (s, e) =>
            {
                if (m_animationData.Progress.X < m_animationData.MaxProgress )
                {
                    SetSize(new Size(

                        ));
                }
                else
                {
                    SetSize(TargetSize);
                    m_animationTimer.Stop();
                }
            };
            m_animationTimer.Start();
        }

        abstract public void SetSize(Size newSize);
    } 
}
