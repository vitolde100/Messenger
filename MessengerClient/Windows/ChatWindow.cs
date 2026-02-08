using MessengerShared;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http.Headers;
using System.Numerics;
using System.Windows.Forms;

namespace MessengerClient.Windows
{
    class Message
    {
        MessageArguments m_message;

        public RoundedPanel Panel = new RoundedPanel();
        Label m_label = new Label();

        List<string> lines = new List<string>();

        Point m_position;
        Size m_size;

        Color m_backColor = Color.FromArgb(125, 125, 125);

        public Message(MessageArguments message, Point position)
        {
            m_position = position;
            FindSize(message.Data);
            InitializeInterface();
        }

        private void FindSize(string message)
        {
            int lineCount = 0;
            int letterCount = 0;
            int lineLength = 50;

            m_message.Data = message;

            string[] words = message.Split(' ');
            foreach (string word in words)
            {
                if (letterCount + word.Length + 1 > lineLength)
                { 
                    if (letterCount == 0)
                    { 
                        string[] splitWord = SplitByLength(lineLength, word);
                        foreach(string part in splitWord)
                        {
                            if (part.Length + 1 > lineLength)
                            {
                                FindOrCreate((part + Environment.NewLine).Substring(0, lineLength), lineCount);
                                lineCount++;
                            }
                            else
                            {
                                FindOrCreate((part).Substring(0, lineLength), lineCount);
                                letterCount += part.Length + 1;
                            }   
                        }
                    }
                    else
                    {
                        m_message.Data = m_message.Data.Insert(letterCount, Environment.NewLine);
                        letterCount = 0;
                        lineCount++;
                    }
                }
                else
                {
                    letterCount += word.Length + 1;
                    FindOrCreate(word + " ", lineCount);
                }
            }
            m_label.AutoSize = true;
            m_label.Text = m_message.Data;
            m_size = new Size(m_label.Size.Width + 20, m_label.Size.Height + 20);
        }

        private void InitializeInterface()
        {
            //
            //Panel
            //
            Panel.Location = m_position;
            Panel.BackColor = m_backColor;
            Panel.BorderColor = Color.FromArgb(85, 85, 85);
            //
            //Lable
            //
            m_label.Location = new Point(10, 10);
            //m_label.Size = new Size(m_size.Width - 20, m_size.Height - 20);
            m_label.BackColor = Color.FromArgb(0, 255, 0);

            Panel.Controls.Add(m_label);
        }

        public void SetPosition(Point newPosition)
        {
            m_position = newPosition;
            Panel.Location = m_position;
        }

        public static string[] SplitByLength(int Length, string str)
        {
            string[] strings = new string[str.Length / Length];
            for (int i = 0; i < strings.Length; i++)
            {
                strings[i] = str.Substring(i * Length, Length);
            }
            return strings;
        }

        private void FindOrCreate(string word, int index)
        { 
            if (index >= 0 && index > lines.Count)
            {
                lines.Add(word);
            }
            else
            {
                lines[index] += word;
            }
        }
    }

    public partial class ChatWindow : IWindow
    {
        Color m_backColor = Color.FromArgb(64, 64, 64);

        Panel m_userPanel;
        ScrollBar m_chatScrollBar;
        List<Message> m_messages = new List<Message>();

        public ChatWindow(string name, Point position, Size size) : base(name, position, size) { }

        public override void InitializeInterface()
        {
            m_userPanel = new Panel();
            m_chatScrollBar = new VScrollBar();
            //
            //ChatPanel
            //
            WindowPanel.BackColor = m_backColor;
            WindowPanel.Controls.Add(m_userPanel);
            WindowPanel.Controls.Add(m_chatScrollBar);
            //
            //UserPanel
            //
            m_userPanel.Location = new Point(0, 0);
            m_userPanel.Size = new Size(m_size.Width, 40);
            m_userPanel.BackColor = Color.FromArgb(125, 125, 125);
            //
            //ChatScrollBar
            //
            m_chatScrollBar.Location = new Point(m_size.Width - 15, m_userPanel.Size.Height);
            m_chatScrollBar.Size = new Size(15, m_size.Height - m_userPanel.Size.Height);
        }

        public override void SetSize(Size newSize)
        {
            m_size = newSize;
            WindowPanel.Size = m_size;
            m_userPanel.Size = new Size(m_size.Width, m_userPanel.Size.Height);
            m_chatScrollBar.Location = new Point(m_size.Width - 15, m_userPanel.Size.Height);
            m_chatScrollBar.Size = new Size(m_chatScrollBar.Size.Width, m_size.Height - m_userPanel.Size.Height);
        }

        public void AddMessage(MessageArguments message)
        {
            m_messages.Add(new Message(message, new Point(0,20 + m_userPanel.Height)));
            WindowPanel.Controls.Add(m_messages.Last().Panel);
        }
    }
}
