namespace MessengerClient
{
    partial class Enter
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            LoginBox = new TextBox();
            IPBox = new TextBox();
            ConnectButton = new Button();
            NameLable = new Label();
            label1 = new Label();
            ErrorLable = new Label();
            PasswordBox = new TextBox();
            label2 = new Label();
            SuspendLayout();
            // 
            // LoginBox
            // 
            LoginBox.BorderStyle = BorderStyle.FixedSingle;
            LoginBox.Location = new Point(23, 28);
            LoginBox.Name = "LoginBox";
            LoginBox.Size = new Size(165, 23);
            LoginBox.TabIndex = 1;
            // 
            // IPBox
            // 
            IPBox.Location = new Point(23, 123);
            IPBox.Name = "IPBox";
            IPBox.Size = new Size(165, 23);
            IPBox.TabIndex = 2;
            // 
            // ConnectButton
            // 
            ConnectButton.Font = new Font("Segoe UI", 14F);
            ConnectButton.Location = new Point(23, 190);
            ConnectButton.Name = "ConnectButton";
            ConnectButton.Size = new Size(165, 40);
            ConnectButton.TabIndex = 0;
            ConnectButton.Text = "Connect";
            ConnectButton.UseVisualStyleBackColor = true;
            ConnectButton.Click += ConnectButton_Click;
            // 
            // NameLable
            // 
            NameLable.AutoSize = true;
            NameLable.Location = new Point(23, 12);
            NameLable.Name = "NameLable";
            NameLable.Size = new Size(37, 15);
            NameLable.TabIndex = 3;
            NameLable.Text = "Login";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(23, 105);
            label1.Name = "label1";
            label1.Size = new Size(48, 15);
            label1.TabIndex = 4;
            label1.Text = "IP : Port";
            // 
            // ErrorLable
            // 
            ErrorLable.AutoSize = true;
            ErrorLable.ForeColor = Color.Red;
            ErrorLable.Location = new Point(23, 172);
            ErrorLable.Name = "ErrorLable";
            ErrorLable.Size = new Size(26, 15);
            ErrorLable.TabIndex = 5;
            ErrorLable.Text = "test";
            ErrorLable.TextAlign = ContentAlignment.MiddleCenter;
            ErrorLable.Visible = false;
            // 
            // PasswordBox
            // 
            PasswordBox.Location = new Point(23, 76);
            PasswordBox.Name = "PasswordBox";
            PasswordBox.Size = new Size(165, 23);
            PasswordBox.TabIndex = 6;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(23, 58);
            label2.Name = "label2";
            label2.Size = new Size(57, 15);
            label2.TabIndex = 7;
            label2.Text = "Password";
            // 
            // Registration
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(215, 256);
            Controls.Add(label2);
            Controls.Add(PasswordBox);
            Controls.Add(ErrorLable);
            Controls.Add(label1);
            Controls.Add(NameLable);
            Controls.Add(IPBox);
            Controls.Add(LoginBox);
            Controls.Add(ConnectButton);
            Name = "Registration";
            Text = "Registration";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox LoginBox;
        private TextBox IPBox;
        private Button ConnectButton;
        private Label NameLable;
        private Label label1;
        private Label ErrorLable;
        private TextBox PasswordBox;
        private Label label2;
    }
}
