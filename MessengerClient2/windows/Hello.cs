using MessengerClient2.src.clientDB;
using MessengerClient2.src.web.lowLevel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MessengerClient2.src.web.lowLevel;

namespace MessengerClient2.windows
{
    public partial class Hello : Form
    {
        public bool serverDone = false;
        public Hello()
        {
            InitializeComponent();

        }

        private void Hello_Load(object sender, EventArgs e)
        {

        }

        private void sel1_Selecting(object sender, TabControlCancelEventArgs e)
        {
            if (e.TabPage == Account && !serverDone)
            {
                e.Cancel = true; 
            }
            if (e.TabPage == Server && serverDone)
            {
                e.Cancel = true;
            }
        }

        private void serverConnect_Click(object sender, EventArgs e)
        {
            try
            {
                ClientDBHandler.data.serverIp = ipBox.Text;
                ClientDBHandler.data.serverPort = portBox.Text;
                ClientConnectionHandler.ConnectToServer(ClientDBHandler.data.serverIp, int.Parse(ClientDBHandler.data.serverPort), ServF, ServS);
            }
            catch (Exception ex) { }
        }

        public void ServS() { serverDone = true; sErrLable.Hide(); sel1.TabPages[1].Focus(); }

        public void ServF() { sErrLable.Show(); }
    }
}
