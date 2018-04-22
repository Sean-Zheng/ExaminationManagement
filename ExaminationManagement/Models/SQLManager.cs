using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ExaminationManagement.Models
{
    public class SQLManager
    {
        private SqlConnection _connection;
        private DataSet _dataSet;

        public SQLManager()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ExamDb"].ConnectionString;
            this._connection = new SqlConnection(connectionString);
            this._dataSet = new DataSet("ExamDb");
        }

        public SQLManager(string dataSetName)
        {
            string connectionString = ConfigurationManager.ConnectionStrings[dataSetName].ConnectionString;
            this._connection = new SqlConnection(connectionString);
            this._dataSet = new DataSet(dataSetName);
        }

        public string checkuser()
        {
            SqlCommand command = this._connection.CreateCommand();
            command.CommandText = "select role_name from tb_roles";
            if (this._connection.State == ConnectionState.Closed)
                this._connection.Open();
            string role = command.ExecuteScalar() as string;
            this._connection.Close();
            return role;
        }
    }
}