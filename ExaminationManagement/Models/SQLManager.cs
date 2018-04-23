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

        /// <summary>
        /// 连接数据库
        /// </summary>
        public SQLManager()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ExamDb"].ConnectionString;
            this._connection = new SqlConnection(connectionString);
            this._dataSet = new DataSet("ExamDb");
        }

        public RoleType CheckUser(string username,string password)
        {
            using(SqlCommand command = _connection.CreateCommand())
            {
                command.CommandText = "select role_id from tb_users where id=@username and passwd=@password";
                if (_connection.State == ConnectionState.Closed)
                    _connection.Open();
                SqlParameter[] parameters = { new SqlParameter("@username", username), new SqlParameter("@password", password) };
                command.Parameters.AddRange(parameters);
                object reslut = command.ExecuteScalar();
                _connection.Close();
                if (reslut == null)
                    return RoleType.NotFound;
                switch (int.Parse(reslut.ToString()))
                {
                    case 0:
                        return RoleType.Admin;
                    case 1:
                        return RoleType.Teacher;
                    case 3:
                        return RoleType.Student;
                    default:
                        return RoleType.NotFound;
                }
            }
            
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