namespace MessengerClient
{
    partial class Welcome_window
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
            Welcome = new Label();
            SingUp = new Label();
            SuspendLayout();
            // 
            // Welcome
            // 
            Welcome.Font = new Font("Segoe UI", 34F);
            Welcome.Location = new Point(-1, 9);
            Welcome.Name = "Welcome";
            Welcome.RightToLeft = RightToLeft.No;
            Welcome.Size = new Size(311, 64);
            Welcome.TabIndex = 0;
            Welcome.Text = "Welcome";
            Welcome.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // SingUp
            // 
            SingUp.AutoSize = true;
            SingUp.BackColor = SystemColors.Control;
            SingUp.Cursor = Cursors.Hand;
            SingUp.Font = new Font("Segoe UI", 14F);
            SingUp.Location = new Point(109, 159);
            SingUp.Name = "SingUp";
            SingUp.Size = new Size(78, 25);
            SingUp.TabIndex = 1;
            SingUp.Text = "Sing Up";
            SingUp.TextAlign = ContentAlignment.MiddleCenter;
            SingUp.Click += SingUp_Click;
            // 
            // Welcome_window
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Control;
            ClientSize = new Size(309, 332);
            Controls.Add(SingUp);
            Controls.Add(Welcome);
            FormBorderStyle = FormBorderStyle.None;
            Margin = new Padding(3, 2, 3, 2);
            Name = "Welcome_window";
            Text = "Welcome_window";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label Welcome;
        private Label SingUp;
    }
}