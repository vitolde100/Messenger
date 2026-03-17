namespace MessengerClient
{
    public partial class Test_Form : Form
    {
        //LayoutRenderer renderer = new LayoutRenderer(new Panel());
        public Test_Form()
        {
            InitializeComponent();
        }

        private void Test_Form_Load(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //m_chat.SetTarget();
        }

        private void vScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            TestLable.Text = vScrollBar1.Value.ToString();
            TestLable.Location = new Point(TestLable.Location.X, vScrollBar1.Value * 3);
        }
    }
}