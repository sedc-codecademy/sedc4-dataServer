using DataBrowser.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DataBrowser
{
    public partial class ConnectionForm : Form
    {
        public ConnectionForm()
        {
            InitializeComponent();
        }

        private void ConnectionForm_Load(object sender, EventArgs e)
        {
            cbxAuthType.SelectedIndex = 0;
        }

        internal ConnectionData GetConnectionData()
        {
            var result = new ConnectionData
            {
                ServerName = txtServer.Text,
                AuthType = (cbxAuthType.SelectedIndex == 0) ? AuthenticationType.Windows : AuthenticationType.SqlServer,
                Username = txtUserName.Text,
                Password = txtPassword.Text
            };
            return result;
        }

        private async void btnTest_Click(object sender, EventArgs e)
        {
            var data = GetConnectionData();

            var dataBrowseService = new DataBrowseService(data);

            lblConnecting.Text = "connecting...";
            if (await dataBrowseService.TestConnection())
            {
                MessageBox.Show("Connection successful");
            } else
            {
                MessageBox.Show("Connection failed");
            }
            lblConnecting.Text = string.Empty;
            btnTest.Enabled = true;



        }
    }
}
