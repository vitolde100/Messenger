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
            NameBox = new TextBox();
            TargetBox = new TextBox();
            AddButton = new Button();
            UserList = new RichTextBox();
            UserLable = new Label();
            MSGErrorLable = new Label();
            GroupErrorLable = new Label();
            ChatsLable = new Label();
            MSGLable = new Label();
            TextTextBox = new RichTextBox();
            SessionTextBox = new RichTextBox();
            label1 = new Label();
            LoginBox = new TextBox();
            UserListError = new Label();
            ChatsBox = new RichTextBox();
            GroupIDBox = new TextBox();
            SuspendLayout();
            // 
            // SendButton
            // 
            SendButton.Location = new Point(153, 35);
            SendButton.Name = "SendButton";
            SendButton.Size = new Size(75, 52);
            SendButton.TabIndex = 0;
            SendButton.Text = "Send";
            SendButton.UseVisualStyleBackColor = true;
            SendButton.Click += SendButton_Click;
            // 
            // MessageTextBox
            // 
            MessageTextBox.Location = new Point(8, 64);
            MessageTextBox.Name = "MessageTextBox";
            MessageTextBox.PlaceholderText = "Message";
            MessageTextBox.Size = new Size(139, 23);
            MessageTextBox.TabIndex = 1;
            // 
            // GetButton
            // 
            GetButton.Location = new Point(379, 45);
            GetButton.Name = "GetButton";
            GetButton.Size = new Size(75, 42);
            GetButton.TabIndex = 2;
            GetButton.Text = "Get";
            GetButton.UseVisualStyleBackColor = true;
            GetButton.Click += GetButton_Click;
            // 
            // UIDBox
            // 
            UIDBox.Location = new Point(460, 296);
            UIDBox.Name = "UIDBox";
            UIDBox.PlaceholderText = "UserID";
            UIDBox.Size = new Size(185, 23);
            UIDBox.TabIndex = 3;
            // 
            // CreateButton
            // 
            CreateButton.Location = new Point(653, 35);
            CreateButton.Name = "CreateButton";
            CreateButton.Size = new Size(68, 52);
            CreateButton.TabIndex = 4;
            CreateButton.Text = "Create";
            CreateButton.UseVisualStyleBackColor = true;
            CreateButton.Click += CreateButton_Click;
            // 
            // NameBox
            // 
            NameBox.Location = new Point(460, 54);
            NameBox.Name = "NameBox";
            NameBox.PlaceholderText = "GroupName";
            NameBox.Size = new Size(185, 23);
            NameBox.TabIndex = 5;
            // 
            // TargetBox
            // 
            TargetBox.Location = new Point(8, 35);
            TargetBox.Name = "TargetBox";
            TargetBox.PlaceholderText = "Target";
            TargetBox.Size = new Size(139, 23);
            TargetBox.TabIndex = 6;
            // 
            // AddButton
            // 
            AddButton.Location = new Point(653, 267);
            AddButton.Name = "AddButton";
            AddButton.Size = new Size(69, 52);
            AddButton.TabIndex = 7;
            AddButton.Text = "Add";
            AddButton.UseVisualStyleBackColor = true;
            AddButton.Click += AddButton_Click;
            // 
            // UserList
            // 
            UserList.Location = new Point(234, 117);
            UserList.Name = "UserList";
            UserList.Size = new Size(220, 135);
            UserList.TabIndex = 8;
            UserList.Text = "";
            // 
            // UserLable
            // 
            UserLable.AutoSize = true;
            UserLable.Font = new Font("Segoe UI", 14F);
            UserLable.Location = new Point(376, 17);
            UserLable.Name = "UserLable";
            UserLable.Size = new Size(78, 25);
            UserLable.TabIndex = 9;
            UserLable.Text = "UserList";
            // 
            // MSGErrorLable
            // 
            MSGErrorLable.AutoSize = true;
            MSGErrorLable.ForeColor = Color.Red;
            MSGErrorLable.Location = new Point(8, 93);
            MSGErrorLable.Name = "MSGErrorLable";
            MSGErrorLable.Size = new Size(32, 15);
            MSGErrorLable.TabIndex = 10;
            MSGErrorLable.Text = "Error";
            // 
            // GroupErrorLable
            // 
            GroupErrorLable.AutoSize = true;
            GroupErrorLable.ForeColor = Color.Red;
            GroupErrorLable.Location = new Point(460, 93);
            GroupErrorLable.Name = "GroupErrorLable";
            GroupErrorLable.Size = new Size(32, 15);
            GroupErrorLable.TabIndex = 11;
            GroupErrorLable.Text = "Error";
            // 
            // ChatsLable
            // 
            ChatsLable.AutoSize = true;
            ChatsLable.Font = new Font("Segoe UI", 14F);
            ChatsLable.Location = new Point(518, 17);
            ChatsLable.Name = "ChatsLable";
            ChatsLable.Size = new Size(127, 25);
            ChatsLable.TabIndex = 12;
            ChatsLable.Text = "Groups/Chats";
            // 
            // MSGLable
            // 
            MSGLable.AutoSize = true;
            MSGLable.Font = new Font("Segoe UI", 14F);
            MSGLable.Location = new Point(134, 5);
            MSGLable.Name = "MSGLable";
            MSGLable.Size = new Size(94, 25);
            MSGLable.TabIndex = 13;
            MSGLable.Text = "Messages";
            // 
            // TextTextBox
            // 
            TextTextBox.Location = new Point(8, 117);
            TextTextBox.Name = "TextTextBox";
            TextTextBox.Size = new Size(220, 135);
            TextTextBox.TabIndex = 14;
            TextTextBox.Text = "";
            // 
            // SessionTextBox
            // 
            SessionTextBox.Location = new Point(8, 283);
            SessionTextBox.Name = "SessionTextBox";
            SessionTextBox.Size = new Size(278, 155);
            SessionTextBox.TabIndex = 15;
            SessionTextBox.Text = "";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 14F);
            label1.Location = new Point(8, 255);
            label1.Name = "label1";
            label1.Size = new Size(114, 25);
            label1.TabIndex = 16;
            label1.Text = "SessionData";
            // 
            // LoginBox
            // 
            LoginBox.Location = new Point(234, 54);
            LoginBox.Name = "LoginBox";
            LoginBox.PlaceholderText = "Nickname";
            LoginBox.Size = new Size(139, 23);
            LoginBox.TabIndex = 17;
            // 
            // UserListError
            // 
            UserListError.AutoSize = true;
            UserListError.ForeColor = Color.Red;
            UserListError.Location = new Point(234, 93);
            UserListError.Name = "UserListError";
            UserListError.Size = new Size(32, 15);
            UserListError.TabIndex = 18;
            UserListError.Text = "Error";
            // 
            // ChatsBox
            // 
            ChatsBox.Location = new Point(460, 117);
            ChatsBox.Name = "ChatsBox";
            ChatsBox.Size = new Size(220, 135);
            ChatsBox.TabIndex = 19;
            ChatsBox.Text = "";
            // 
            // GroupIDBox
            // 
            GroupIDBox.Location = new Point(460, 267);
            GroupIDBox.Name = "GroupIDBox";
            GroupIDBox.PlaceholderText = "GroupID";
            GroupIDBox.Size = new Size(185, 23);
            GroupIDBox.TabIndex = 20;
            // 
            // TestForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(GroupIDBox);
            Controls.Add(ChatsBox);
            Controls.Add(UserListError);
            Controls.Add(LoginBox);
            Controls.Add(label1);
            Controls.Add(SessionTextBox);
            Controls.Add(TextTextBox);
            Controls.Add(MSGLable);
            Controls.Add(ChatsLable);
            Controls.Add(GroupErrorLable);
            Controls.Add(MSGErrorLable);
            Controls.Add(UserLable);
            Controls.Add(UserList);
            Controls.Add(AddButton);
            Controls.Add(TargetBox);
            Controls.Add(NameBox);
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
        private TextBox NameBox;
        private TextBox TargetBox;
        private Button AddButton;
        private RichTextBox UserList;
        private Label UserLable;
        private Label MSGErrorLable;
        private Label GroupErrorLable;
        private Label ChatsLable;
        private Label MSGLable;
        private RichTextBox TextTextBox;
        private RichTextBox SessionTextBox;
        private Label label1;
        private TextBox LoginBox;
        private Label UserListError;
        private RichTextBox ChatsBox;
        private TextBox GroupIDBox;
    }
}