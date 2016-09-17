using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using DataBrowser.Core;

namespace DataBrowser
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private DataBrowseService dataBrowseService;

        private async void btnConnect_Click(object sender, EventArgs e)
        {
            var connectionForm = new ConnectionForm();
            if (connectionForm.ShowDialog() == DialogResult.OK)
            {
                var connectionData = connectionForm.GetConnectionData();
                dataBrowseService = new DataBrowseService(connectionData);
                Text = $"SEDC Data Browser - connecting to {connectionData.ServerName}";
                try
                {
                    await dataBrowseService.Connect();
                    Text = $"SEDC Data Browser - connected to {connectionData.ServerName}";
                }
                catch (SqlException)
                {
                    Text = $"SEDC Data Browser - connecting to {connectionData.ServerName} failed";
                    throw;
                }
                cbxDatabases.DataSource = (await dataBrowseService.GetDatabaseNames()).ToList();
            }
        }

        private void cbxDatabases_SelectedIndexChanged(object sender, EventArgs e)
        {
            var databaseName = (string)cbxDatabases.SelectedItem;
            var task = dataBrowseService.GetTableNames(databaseName);
            task.Wait();
            lbxTables.DataSource = task.Result.ToList();
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
                dataBrowseService.Dispose();
            }
            base.Dispose(disposing);
        }

        private async void lbxTables_SelectedIndexChanged(object sender, EventArgs e)
        {
            var databaseName = (string)cbxDatabases.SelectedItem;
            var tableName = (string)lbxTables.SelectedItem;

            var columnNames = (await dataBrowseService.GetColumnNames(databaseName, tableName)).ToList();

            dgvData.Columns.Clear();
            foreach (var cname in columnNames)
            {
                dgvData.Columns.Add(cname, cname);
            }

            var rowData = await dataBrowseService.GetData(databaseName, tableName);
            foreach (var item in rowData)
            {
                dgvData.Rows.Add(item.ToArray());
            }
        }
    }
}
