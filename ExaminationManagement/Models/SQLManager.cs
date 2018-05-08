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

        /// <summary>
        /// 连接数据库
        /// </summary>
        public SQLManager()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ExamDb"].ConnectionString;
            this._connection = new SqlConnection(connectionString);
            //this._dataSet = new DataSet("ExamDb");
        }
        #region  select
        /// <summary>
        /// 登录验证
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <returns>验证成功返回用户类型，失败返回NotFound</returns>
        public DataBaseModels.RoleType CheckUser(string username, string password)
        {
            using (SqlCommand command = _connection.CreateCommand())
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
                    return DataBaseModels.RoleType.NotFound;
                switch (int.Parse(reslut.ToString()))
                {
                    case 0:
                        return DataBaseModels.RoleType.Admin;
                    case 1:
                        return DataBaseModels.RoleType.Teacher;
                    case 2:
                        return DataBaseModels.RoleType.Student;
                    default:
                        return DataBaseModels.RoleType.NotFound;
                }
            }
        }
        public IEnumerable<DataBaseModels.Major> GetMajors()
        {
            using(SqlCommand command = _connection.CreateCommand())
            {
                command.CommandText = "select * from tb_major";
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    DataBaseModels.Major major = new DataBaseModels.Major
                    {
                        Major_id = reader.GetInt32(0),
                        MajorName = reader.GetString(1)
                    };
                    yield return major;
                }
                reader.Close();
            }
        }

        public IEnumerable<DataBaseModels.Course> GetCourses()
        {
            SqlDataAdapter adapter = new SqlDataAdapter("", _connection);
            DataTable table = new DataTable();
            adapter.Fill(table);

            foreach (var item in table.Rows)
            {
                DataBaseModels.Course course = new DataBaseModels.Course
                {

                };
                yield return course;
            }
        }
        //未验证
        public DataBaseModels.StuInfo GetStuInfo(string studentId)
        {
            DataBaseModels.StuInfo info = null;
            using (SqlCommand command = _connection.CreateCommand())
            {
                command.CommandText = "select * from tb_stuinfo where stu_id=@studentId";
                if (_connection.State == ConnectionState.Closed)
                    _connection.Open();
                SqlParameter parameter = new SqlParameter("@studentId", studentId);
                command.Parameters.Add(parameter);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    info = new DataBaseModels.StuInfo
                    {
                        Stu_id = reader[0].ToString(),
                        Name = reader[1].ToString(),
                        Birth = reader[2].ToString(),
                        Photo = reader[3].ToString(),
                        Tel = reader[4].ToString(),
                        Email = reader[5].ToString(),
                        Major_id = reader.GetInt32(6),
                        Enroll_year = reader.GetInt32(7),
                        Credit_got = reader.GetDouble(8),
                        Credit_need = reader.GetDouble(9)
                    };
                }
                reader.Close();
                _connection.Close();
                return info;
            }
        }
        #endregion

        #region
        //
        public void AddTeacher(DataBaseModels.TeachInfo info)
        {
            SqlDataAdapter adapter = new SqlDataAdapter("", _connection);
        }
        /// <summary>
        /// 添加课程
        /// </summary>
        /// <param name="courses">课程:课程名称和学分</param>
        /// <returns></returns>
        public bool AddCourse(WebModels.Course[] courses)
        {
            SqlDataAdapter adapter = new SqlDataAdapter("select * from tb_course", _connection);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            DataTable table = new DataTable();
            adapter.Fill(table);

            foreach (var item in courses)
            {
                DataRow row = table.NewRow();
                row[1] = item.CourseName;
                row[3] = item.Credit;
                table.Rows.Add(row);
            }

            return adapter.Update(table) > 0 ? true : false;
        }
        #endregion




        #region others
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
        #endregion
    }
}