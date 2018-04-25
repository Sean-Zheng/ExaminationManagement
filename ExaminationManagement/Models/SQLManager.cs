using ExaminationManagement.Models.DataBaseModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace ExaminationManagement.Models
{
    /// <summary>
    /// 数据库操作
    /// </summary>
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
        /// <summary>
        /// 登录验证
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <returns>验证成功返回用户类型，失败返回NotFound</returns>
        public RoleType CheckUser(string username,string password)
        {
            using(SqlCommand command = _connection.CreateCommand())
            {
                password = Encryption(password);
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
        public void AddUser(string password)
        {

        }
        /// <summary>
        /// 加密字符串
        /// </summary>
        /// <param name="text">明文</param>
        /// <returns>密文</returns>
        private string Encryption(string text)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] encryptdata = md5.ComputeHash(Encoding.Default.GetBytes(text));
            return Convert.ToBase64String(encryptdata);
        }
    }
}