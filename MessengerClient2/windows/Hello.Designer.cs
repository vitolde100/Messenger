namespace MessengerClient2.windows
{
    partial class Hello
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Hello));
            sel1 = new TabControl();
            Server = new TabPage();
            serverConnect = new Button();
            pictureBox1 = new PictureBox();
            label2 = new Label();
            portBox = new TextBox();
            ipBox = new TextBox();
            label1 = new Label();
            Account = new TabPage();
            sErrLable = new Label();
            sel1.SuspendLayout();
            Server.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // sel1
            // 
            sel1.Controls.Add(Server);
            sel1.Controls.Add(Account);
            sel1.Location = new Point(12, 12);
            sel1.Name = "sel1";
            sel1.SelectedIndex = 0;
            sel1.Size = new Size(352, 272);
            sel1.TabIndex = 0;
            sel1.Selecting += sel1_Selecting;
            // 
            // Server
            // 
            Server.Controls.Add(sErrLable);
            Server.Controls.Add(serverConnect);
            Server.Controls.Add(pictureBox1);
            Server.Controls.Add(label2);
            Server.Controls.Add(portBox);
            Server.Controls.Add(ipBox);
            Server.Controls.Add(label1);
            Server.Location = new Point(4, 24);
            Server.Name = "Server";
            Server.Padding = new Padding(3);
            Server.Size = new Size(344, 244);
            Server.TabIndex = 0;
            Server.Text = "Server";
            Server.UseVisualStyleBackColor = true;
            // 
            // serverConnect
            // 
            serverConnect.Location = new Point(200, 215);
            serverConnect.Name = "serverConnect";
            serverConnect.Size = new Size(138, 23);
            serverConnect.TabIndex = 5;
            serverConnect.Text = "Connect";
            serverConnect.UseVisualStyleBackColor = true;
            serverConnect.Click += serverConnect_Click;
            // 
            // pictureBox1
            // 
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(18, 62);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(100, 65);
            pictureBox1.TabIndex = 4;
            pictureBox1.TabStop = false;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(15, 142);
            label2.Name = "label2";
            label2.Size = new Size(219, 15);
            label2.TabIndex = 3;
            label2.Text = "Пожалуйста введите ip и порт сервера";
            // 
            // portBox
            // 
            portBox.Location = new Point(238, 160);
            portBox.MaxLength = 8;
            portBox.Name = "portBox";
            portBox.PlaceholderText = "Port";
            portBox.Size = new Size(100, 23);
            portBox.TabIndex = 2;
            // 
            // ipBox
            // 
            ipBox.Location = new Point(15, 160);
            ipBox.Name = "ipBox";
            ipBox.PlaceholderText = "IP";
            ipBox.Size = new Size(217, 23);
            ipBox.TabIndex = 1;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI Semibold", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 204);
            label1.Location = new Point(15, 22);
            label1.Name = "label1";
            label1.Size = new Size(206, 30);
            label1.TabIndex = 0;
            label1.Text = "Добро пожаловать";
            // 
            // Account
            // 
            Account.Location = new Point(4, 24);
            Account.Name = "Account";
            Account.Padding = new Padding(3);
            Account.Size = new Size(344, 244);
            Account.TabIndex = 1;
            Account.Text = "Account";
            Account.UseVisualStyleBackColor = true;
            // 
            // sErrLable
            // 
            sErrLable.AutoSize = true;
            sErrLable.ForeColor = Color.Red;
            sErrLable.Location = new Point(14, 186);
            sErrLable.Name = "sErrLable";
            sErrLable.Size = new Size(218, 15);
            sErrLable.TabIndex = 6;
            sErrLable.Text = "Невозможно подключиться к серверу";
            sErrLable.Visible = false;
            // 
            // Hello
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(376, 289);
            Controls.Add(sel1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            Name = "Hello";
            SizeGripStyle = SizeGripStyle.Hide;
            Text = "Hello";
            Load += Hello_Load;
            sel1.ResumeLayout(false);
            Server.ResumeLayout(false);
            Server.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private TabControl sel1;
        private TabPage Server;
        private TabPage Account;
        private Label label2;
        private TextBox portBox;
        private TextBox ipBox;
        private Label label1;
        private Button serverConnect;
        private PictureBox pictureBox1;
        private Label sErrLable;
    }
}