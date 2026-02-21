using MessengerShared;
using System.Text;

namespace MessengerClient.Elements
{
    public class Message
    {
        const int MAX_LINE_LENGTH = 50;
        const int MAX_WIDTH = 300;
        const int PADDING_X = 20;
        const int PADDING_Y = 14;
        const int PANEL_RADIUS = 20;

        public RoundedPanel Panel = new RoundedPanel();
        Label m_label = new Label();

        public ChatMessage message = new ChatMessage();
        Point m_location;
        Size m_size;
        Size m_textSize;

        Color m_backColor = Color.FromArgb(125, 125, 125);

        public Message(ChatMessage msg, Point location)
        {
            FormatMessage(msg.Text);
            m_size = new Size(
                m_textSize.Width + PADDING_X + PANEL_RADIUS,
                m_textSize.Height + PADDING_Y + PANEL_RADIUS);
            if (location.X > 0) m_location.X = location.X - m_size.Width;
            m_location.Y = location.Y;
            InitializeComponents();
        }

        private void FormatMessage(string msg)
        {
            List<string> lines = new List<string>();
            StringBuilder current = new StringBuilder();

            string[] words = msg.Split(' ');
            if (words.Length > 0)
            {
                foreach (string word in words)
                {
                    if (word.Length > MAX_LINE_LENGTH)
                    {
                        if (current.Length > 0)
                        {
                            lines.Add(current.ToString());
                            current.Clear();
                        }
                        for (int i = 0; i < word.Length; i += MAX_LINE_LENGTH)
                            lines.Add(word.Substring(i, Math.Min(MAX_LINE_LENGTH, word.Length - i)));
                    }
                    else
                    {
                        if (current.Length + word.Length + 1 > MAX_LINE_LENGTH)
                        {
                            lines.Add(current.ToString());
                            current.Clear();
                        }
                        if (current.Length > 0)
                            current.Append(' ');

                        current.Append(word);
                    }
                }

                if (current.Length > 0)
                    lines.Add(current.ToString());
            }
            else
                lines.Add(msg); 
            message.Text = string.Join(Environment.NewLine, lines);
            m_textSize = TextRenderer.MeasureText(
                this.message.Text,
                SystemFonts.DefaultFont,
                new Size(MAX_WIDTH, int.MaxValue),
                TextFormatFlags.WordBreak
            );
        }

        private void InitializeComponents()
        {
            //
            //Panel
            //
            Panel.Location = m_location;
            Panel.Width = m_textSize.Width + PADDING_X + PANEL_RADIUS;
            Panel.Height = m_textSize.Height + PADDING_Y + PANEL_RADIUS;
            Panel.Radius = PANEL_RADIUS;
            Panel.BackColor = m_backColor;
            Panel.BorderColor = Color.FromArgb(85, 85, 85);
            //
            //Lable
            //
            m_label.AutoSize = true;
            m_label.Location = new Point(5, 5);
            m_label.MaximumSize = new Size(MAX_WIDTH, 0);
            m_label.TextAlign = ContentAlignment.TopLeft;
            m_label.Text = message.Text;
            //
            //Add controls
            //
            Panel.Controls.Add(m_label);
        }

        public void SetPosition(Point newPosition)
        {
            m_location = newPosition;
            Panel.Location = m_location;
        }
    }
}
