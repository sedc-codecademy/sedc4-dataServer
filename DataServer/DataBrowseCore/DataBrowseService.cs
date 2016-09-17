using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBrowser.Core
{
    public class DataBrowseService : IDisposable
    {
        private ConnectionData connectionData;
        private SqlConnection connection;

        public bool IsConnected { get; private set; }

        public DataBrowseService(ConnectionData data)
        {
            connectionData = data;
            IsConnected = false;
        }

        public async Task Connect()
        {
            var cnnString = connectionData.GetConnectionString();
            connection = new SqlConnection(cnnString);
            await connection.OpenAsync();
            IsConnected = true;
        }

        public async Task Disconnect()
        {
            connection.Close();
            IsConnected = false;
        }

        public async Task<bool> TestConnection()
        {
            if (IsConnected)
                return true;

            var cnnString = connectionData.GetConnectionString();
            using (SqlConnection connection = new SqlConnection(cnnString))
            {
                try
                {
                    await connection.OpenAsync();
                    return true;
                }
                catch (SqlException)
                {
                    return false;
                }
            }
        }

        public async Task<IEnumerable<string>> GetDatabaseNames()
        {
            DataTable databases = connection.GetSchema("Databases");
            return databases.Rows
                .Cast<DataRow>()
                .Select(dr => dr.Field<string>("database_name"));
        }

        public async Task<IEnumerable<string>> GetTableNames(string databaseName)
        {
            connection.ChangeDatabase(databaseName);
            var dataTables = connection.GetSchema("Tables");
            return dataTables.Rows
                .Cast<DataRow>()
                .Select(dr => dr.Field<string>("TABLE_NAME"));
        }

        public async Task<IEnumerable<string>> GetColumnNames(string databaseName, string tableName)
        {
            connection.ChangeDatabase(databaseName);
            var dataTables = connection.GetSchema("Columns", new string[] { null, null, tableName, null });
            return dataTables.Rows
                .Cast<DataRow>()
                .Select(dr => dr.Field<string>("COLUMN_NAME"));
        }

        public async Task<IEnumerable<IEnumerable<object>>> GetData(string databaseName, string tableName)
        {
            connection.ChangeDatabase(databaseName);
            var result = new List<IEnumerable<object>>();
            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = connection;
                command.CommandText = $"Select * from {databaseName}.dbo.{tableName}";

                using (var dataReader = await command.ExecuteReaderAsync())
                {
                    while (await dataReader.ReadAsync())
                    {
                        object[] values = new object[dataReader.FieldCount];
                        dataReader.GetValues(values);
                        result.Add(values);
                    }
                }
            }
            return result;
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    connection.Dispose();

                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~DataBrowseService() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion


        //private async void lbxTables_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    var databaseName = (string)cbxDatabases.SelectedItem;
        //    var tableName = (string)lbxTables.SelectedItem;

        //    var dataColumns = dataBrowseService.connection.GetSchema("Columns", new string[] { null, null, tableName, null });
        //    var columnNames = new List<string>();
        //    foreach (DataRow table in dataColumns.Rows)
        //    {
        //        columnNames.Add(table.Field<string>("COLUMN_NAME"));
        //    }

        //    dgvData.Columns.Clear();
        //    foreach (var cname in columnNames)
        //    {
        //        dgvData.Columns.Add(cname, cname);
        //    }

        //    using (SqlCommand command = new SqlCommand())
        //    {
        //        command.Connection = connection;
        //        command.CommandText = $"Select * from {tableName}";

        //        using (var dataReader = await command.ExecuteReaderAsync())
        //        {
        //            while (dataReader.Read())
        //            {
        //                object[] values = new object[dataReader.FieldCount];
        //                dataReader.GetValues(values);
        //                dgvData.Rows.Add(values);
        //            }
        //        }
        //    }
        //}

    }
}
