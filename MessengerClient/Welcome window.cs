namespace MessengerClient
{
    public partial class Welcome_window : Form
    {
        public Welcome_window()
        {
            InitializeComponent();
        }

        private void SingUp_Click(object sender, EventArgs e)
        {
            Form Registration = new Registration();
            this.Hide();
            Registration.ShowDialog();
            if (Program.isConnected) 
                this.Close();
            else this.Show();
        }
    }
}
