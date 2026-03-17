namespace MessengerClient
{
    public partial class WelcomeForm : Form
    {
        public WelcomeForm()
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
