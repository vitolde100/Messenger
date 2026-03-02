namespace MessengerClient
{
    partial class Test_Form
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
            button1 = new Button();
            vScrollBar1 = new VScrollBar();
            TestLable = new Label();
            textBox1 = new TextBox();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Location = new Point(877, 9);
            button1.Margin = new Padding(3, 2, 3, 2);
            button1.Name = "button1";
            button1.Size = new Size(86, 68);
            button1.TabIndex = 0;
            button1.Text = "Test";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // vScrollBar1
            // 
            vScrollBar1.Location = new Point(1016, 9);
            vScrollBar1.Name = "vScrollBar1";
            vScrollBar1.Size = new Size(17, 320);
            vScrollBar1.TabIndex = 1;
            vScrollBar1.Scroll += vScrollBar1_Scroll;
            // 
            // TestLable
            // 
            TestLable.AutoSize = true;
            TestLable.Location = new Point(994, 9);
            TestLable.Name = "TestLable";
            TestLable.Size = new Size(55, 15);
            TestLable.TabIndex = 2;
            TestLable.Text = "TestLable";
            // 
            // textBox1
            // 
            textBox1.Location = new Point(802, 82);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(161, 23);
            textBox1.TabIndex = 3;
            // 
            // Test_Form
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.AppWorkspace;
            ClientSize = new Size(1051, 623);
            Controls.Add(textBox1);
            Controls.Add(TestLable);
            Controls.Add(vScrollBar1);
            Controls.Add(button1);
            Margin = new Padding(3, 2, 3, 2);
            Name = "Test_Form";
            Text = "Test_Form";
            Load += Test_Form_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button button1;
        private VScrollBar vScrollBar1;
        private Label TestLable;
        private TextBox textBox1;
    }
}