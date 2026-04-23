namespace MessengerClient.Interface.Forms
{
    partial class TestForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            SendButton = new Button();
            MessageTextBox = new TextBox();
            GetButton = new Button();
            UIDBox = new TextBox();
            CreateButton = new Button();
            NameButton = new TextBox();
            TargetBox = new TextBox();
            button1 = new Button();
            UserList = new RichTextBox();
            UserLable = new Label();
            MSGErrorLable = new Label();
            GroupErrorLable = new Label();
            ChatsLable = new Label();
            MSGLable = new Label();
            TextTextBox = new RichTextBox();
            SessionTextBox = new RichTextBox();
            label1 = new Label();
            SuspendLayout();
            // 
            // SendButton
            // 
            SendButton.Location = new Point(155, 39);
            SendButton.Name = "SendButton";
            SendButton.Size = new Size(75, 52);
            SendButton.TabIndex = 0;
            SendButton.Text = "Send";
            SendButton.UseVisualStyleBackColor = true;
            SendButton.Click += SendButton_Click;
            // 
            // MessageTextBox
            // 
            MessageTextBox.Location = new Point(10, 68);
            MessageTextBox.Name = "MessageTextBox";
            MessageTextBox.PlaceholderText = "Message";
            MessageTextBox.Size = new Size(139, 23);
            MessageTextBox.TabIndex = 1;
            // 
            // GetButton
            // 
            GetButton.Location = new Point(711, 148);
            GetButton.Name = "GetButton";
            GetButton.Size = new Size(75, 45);
            GetButton.TabIndex = 2;
            GetButton.Text = "Get";
            GetButton.UseVisualStyleBackColor = true;
            // 
            // UIDBox
            // 
            UIDBox.Location = new Point(520, 68);
            UIDBox.Name = "UIDBox";
            UIDBox.PlaceholderText = "UserID";
            UIDBox.Size = new Size(185, 23);
            UIDBox.TabIndex = 3;
            // 
            // CreateButton
            // 
            CreateButton.Location = new Point(711, 39);
            CreateButton.Name = "CreateButton";
            CreateButton.Size = new Size(75, 52);
            CreateButton.TabIndex = 4;
            CreateButton.Text = "Create";
            CreateButton.UseVisualStyleBackColor = true;
            // 
            // NameButton
            // 
            NameButton.Location = new Point(520, 39);
            NameButton.Name = "NameButton";
            NameButton.PlaceholderText = "GroupName";
            NameButton.Size = new Size(185, 23);
            NameButton.TabIndex = 5;
            // 
            // TargetBox
            // 
            TargetBox.Location = new Point(10, 39);
            TargetBox.Name = "TargetBox";
            TargetBox.PlaceholderText = "Target";
            TargetBox.Size = new Size(139, 23);
            TargetBox.TabIndex = 6;
            // 
            // button1
            // 
            button1.Location = new Point(711, 97);
            button1.Name = "button1";
            button1.Size = new Size(75, 45);
            button1.TabIndex = 7;
            button1.Text = "Add";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // UserList
            // 
            UserList.Location = new Point(520, 148);
            UserList.Name = "UserList";
            UserList.Size = new Size(185, 290);
            UserList.TabIndex = 8;
            UserList.Text = "";
            // 
            // UserLable
            // 
            UserLable.AutoSize = true;
            UserLable.Font = new Font("Segoe UI", 14F);
            UserLable.Location = new Point(520, 117);
            UserLable.Name = "UserLable";
            UserLable.Size = new Size(78, 25);
            UserLable.TabIndex = 9;
            UserLable.Text = "UserList";
            // 
            // MSGErrorLable
            // 
            MSGErrorLable.AutoSize = true;
            MSGErrorLable.ForeColor = Color.Red;
            MSGErrorLable.Location = new Point(10, 97);
            MSGErrorLable.Name = "MSGErrorLable";
            MSGErrorLable.Size = new Size(32, 15);
            MSGErrorLable.TabIndex = 10;
            MSGErrorLable.Text = "Error";
            // 
            // GroupErrorLable
            // 
            GroupErrorLable.AutoSize = true;
            GroupErrorLable.ForeColor = Color.Red;
            GroupErrorLable.Location = new Point(520, 97);
            GroupErrorLable.Name = "GroupErrorLable";
            GroupErrorLable.Size = new Size(32, 15);
            GroupErrorLable.TabIndex = 11;
            GroupErrorLable.Text = "Error";
            // 
            // ChatsLable
            // 
            ChatsLable.AutoSize = true;
            ChatsLable.Font = new Font("Segoe UI", 14F);
            ChatsLable.Location = new Point(578, 10);
            ChatsLable.Name = "ChatsLable";
            ChatsLable.Size = new Size(127, 25);
            ChatsLable.TabIndex = 12;
            ChatsLable.Text = "Groups/Chats";
            // 
            // MSGLable
            // 
            MSGLable.AutoSize = true;
            MSGLable.Font = new Font("Segoe UI", 14F);
            MSGLable.Location = new Point(136, 9);
            MSGLable.Name = "MSGLable";
            MSGLable.Size = new Size(94, 25);
            MSGLable.TabIndex = 13;
            MSGLable.Text = "Messages";
            // 
            // TextTextBox
            // 
            TextTextBox.Location = new Point(10, 115);
            TextTextBox.Name = "TextTextBox";
            TextTextBox.Size = new Size(220, 323);
            TextTextBox.TabIndex = 14;
            TextTextBox.Text = "";
            // 
            // SessionTextBox
            // 
            SessionTextBox.Location = new Point(236, 39);
            SessionTextBox.Name = "SessionTextBox";
            SessionTextBox.Size = new Size(278, 154);
            SessionTextBox.TabIndex = 15;
            SessionTextBox.Text = "";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 14F);
            label1.Location = new Point(320, 9);
            label1.Name = "label1";
            label1.Size = new Size(114, 25);
            label1.TabIndex = 16;
            label1.Text = "SessionData";
            // 
            // TestForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(label1);
            Controls.Add(SessionTextBox);
            Controls.Add(TextTextBox);
            Controls.Add(MSGLable);
            Controls.Add(ChatsLable);
            Controls.Add(GroupErrorLable);
            Controls.Add(MSGErrorLable);
            Controls.Add(UserLable);
            Controls.Add(UserList);
            Controls.Add(button1);
            Controls.Add(TargetBox);
            Controls.Add(NameButton);
            Controls.Add(CreateButton);
            Controls.Add(UIDBox);
            Controls.Add(GetButton);
            Controls.Add(MessageTextBox);
            Controls.Add(SendButton);
            Name = "TestForm";
            Text = "TestForm";
            Load += TestForm_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button SendButton;
        private TextBox MessageTextBox;
        private Button GetButton;
        private TextBox UIDBox;
        private Button CreateButton;
        private TextBox NameButton;
        private TextBox TargetBox;
        private Button button1;
        private RichTextBox UserList;
        private Label UserLable;
        private Label MSGErrorLable;
        private Label GroupErrorLable;
        private Label ChatsLable;
        private Label MSGLable;
        private RichTextBox TextTextBox;
        private RichTextBox SessionTextBox;
        private Label label1;
    }
}