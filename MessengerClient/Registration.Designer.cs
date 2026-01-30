namespace MessengerClient
{
    partial class Registration
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
            ConnectButton = new Button();
            NameBox = new TextBox();
            IPBox = new TextBox();
            SuspendLayout();
            // 
            // ConnectButton
            // 
            ConnectButton.Location = new Point(38, 143);
            ConnectButton.Name = "ConnectButton";
            ConnectButton.Size = new Size(165, 82);
            ConnectButton.TabIndex = 0;
            ConnectButton.Text = "Connect";
            ConnectButton.UseVisualStyleBackColor = true;
            ConnectButton.Click += ConnectButton_Click;
            // 
            // NameBox
            // 
            NameBox.Location = new Point(38, 23);
            NameBox.Name = "NameBox";
            NameBox.Size = new Size(165, 23);
            NameBox.TabIndex = 1;
            // 
            // IPBox
            // 
            IPBox.Location = new Point(38, 87);
            IPBox.Name = "IPBox";
            IPBox.Size = new Size(165, 23);
            IPBox.TabIndex = 2;
            // 
            // Registration
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(247, 256);
            Controls.Add(IPBox);
            Controls.Add(NameBox);
            Controls.Add(ConnectButton);
            Name = "Registration";
            Text = "Registration";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button ConnectButton;
        private TextBox NameBox;
        private TextBox IPBox;
    }
}
