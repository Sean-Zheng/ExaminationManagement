using ExaminationManagement.Models.ExcelModels;
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
        }

        #region 登录
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
        #endregion

        #region  专业
        /// <summary>
        /// 获取专业列表
        /// </summary>
        /// <returns></returns>
        public IEnumerable<SelectOptions> GetMajors()
        {
            using (SqlCommand command = _connection.CreateCommand())
            {
                command.CommandText = "select * from tb_major";
                if (_connection.State == ConnectionState.Closed)
                    _connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    SelectOptions options = new SelectOptions
                    {
                        value = reader.GetInt32(0).ToString(),
                        text = reader.GetString(1)
                    };
                    yield return options;
                }
                reader.Close();
                _connection.Close();
            }
        }
        public IEnumerable<DataBaseModels.Major> GetMajorList()
        {
            using (SqlCommand command = _connection.CreateCommand())
            {
                command.CommandText = "select * from tb_major";
                if (_connection.State == ConnectionState.Closed)
                    _connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    DataBaseModels.Major major = new DataBaseModels.Major
                    {
                        MajorId = reader.GetInt32(0),
                        MajorName = reader.GetString(1),
                        Credit = reader.GetDouble(2)
                    };
                    yield return major;
                }
                reader.Close();
                _connection.Close();
            }
        }
        /// <summary>
        /// 添加专业
        /// </summary>
        /// <param name="majors"></param>
        /// <returns></returns>
        public bool AddMajors(WebModels.Major[] majors)
        {
            SqlDataAdapter adapter = new SqlDataAdapter("select * from tb_major", _connection);
            new SqlCommandBuilder(adapter);
            DataTable table = new DataTable();
            adapter.Fill(table);

            foreach (var item in majors)
            {
                DataRow row = table.NewRow();
                row[1] = item.MajorName;
                row[2] = item.Credit;
                table.Rows.Add(row);
            }
            return adapter.Update(table) > 0 ? true : false;
        }
        /// <summary>
        /// 修改专业
        /// </summary>
        /// <param name="majorId"></param>
        /// <param name="majorName"></param>
        /// <returns></returns>
        public bool UpdateMajors(int majorId, WebModels.Major major)
        {
            using (SqlCommand command = _connection.CreateCommand())
            {
                command.CommandText = "update tb_major set name=@majorName,credit_need=@credit where major_id=@majorId";
                if (_connection.State == ConnectionState.Closed)
                    _connection.Open();
                command.Parameters.AddWithValue("@majorName", major.MajorName);
                command.Parameters.AddWithValue("@credit", major.Credit);
                command.Parameters.AddWithValue("@majorId", majorId);
                int changeNumber = command.ExecuteNonQuery();
                _connection.Close();
                if (changeNumber > 0)
                    return true;
                return false;
            }
        }
        /// <summary>
        /// 删除专业
        /// </summary>
        /// <param name="majorId"></param>
        /// <returns></returns>
        public bool DeleteMajors(int majorId)
        {
            using (SqlCommand command = _connection.CreateCommand())
            {
                command.CommandText = "delete from tb_major where major_id=@Id";
                if (_connection.State == ConnectionState.Closed)
                    _connection.Open();
                SqlParameter parameter = new SqlParameter("@Id", majorId);
                command.Parameters.Add(parameter);
                try
                {
                    int changeNumber = command.ExecuteNonQuery();
                    if (changeNumber > 0)
                        return true;
                    return false;
                }
                catch (Exception)
                {
                    return false;
                }
                finally
                {
                    _connection.Close();
                }
            }
        }
        public bool CheckMajorExist(string[] majorName)
        {
            SqlDataAdapter adapter = new SqlDataAdapter("select * from tb_major", _connection);
            DataTable table = new DataTable();
            adapter.Fill(table);

            foreach (var item in majorName)
            {
                DataRow[] rows = table.Select($"name='{item}'");
                if (rows.Length == 0)
                    return false;
            }
            return true;
        }
        #endregion

        #region 课程
        /// <summary>
        /// 获取课程信息
        /// </summary>
        /// <returns></returns>
        public IEnumerable<DataBaseModels.Course> GetCourses()
        {
            SqlDataAdapter adapter = new SqlDataAdapter("select * from tb_course", _connection);
            DataTable table = new DataTable();
            adapter.Fill(table);

            foreach (DataRow item in table.Rows)
            {
                DataBaseModels.Course course = new DataBaseModels.Course
                {
                    CourseId = item.Field<int>(0),
                    CourseName = item.Field<string>(1),
                    Grade = item.Field<int?>(2),
                    Credit = item.Field<double>(3),
                    Teacher = item.Field<string>(4)
                };
                yield return course;
            }
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
        /// <summary>
        /// 删除课程
        /// </summary>
        /// <param name="courseId"></param>
        /// <returns></returns>
        public bool DeleteCourse(int courseId)
        {
            using (SqlCommand command = _connection.CreateCommand())
            {
                command.CommandText = "delete from tb_course where course_id=@Id";
                if (_connection.State == ConnectionState.Closed)
                    _connection.Open();
                SqlParameter parameter = new SqlParameter("@Id", courseId);
                command.Parameters.Add(parameter);
                int changeNumber = command.ExecuteNonQuery();
                _connection.Close();
                if (changeNumber > 0)
                    return true;
                return false;
            }
        }
        /// <summary>
        /// 更新课程信息
        /// </summary>
        /// <param name="course"></param>
        /// <returns></returns>
        public bool UpdateCourse(DataBaseModels.Course course)
        {
            SqlDataAdapter adapter = new SqlDataAdapter("select * from tb_course", _connection);
            DataTable table = new DataTable();
            adapter.Fill(table);
            new SqlCommandBuilder(adapter);

            DataRow[] rows = table.Select($"course_id={course.CourseId}");
            if (rows.Length == 0)
                return false;
            DataRow row = rows[0];
            row.SetField(1, course.CourseName);
            row.SetField(2, course.Grade);
            row.SetField(3, course.Credit);
            row.SetField(4, course.Teacher);

            return adapter.Update(table) > 0 ? true : false;
        }
        /// <summary>
        /// 根据教工号获取课程列表
        /// </summary>
        /// <param name="teaId"></param>
        /// <returns></returns>
        public IEnumerable<SelectOptions> CourseList(string teaId)
        {
            using (SqlCommand command = _connection.CreateCommand())
            {
                command.CommandText = "select course_id,name from tb_course where tea_id=@ID";
                command.Parameters.AddWithValue("@ID", teaId);
                if (_connection.State == ConnectionState.Closed)
                    _connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    SelectOptions options = new SelectOptions
                    {
                        value = reader.GetInt32(0).ToString(),
                        text = reader.GetString(1)
                    };
                    yield return options;
                }
            }
        }
        #endregion

        #region 教师
        public string SelectTeacherName(string Id)
        {
            using (SqlCommand command = _connection.CreateCommand())
            {
                command.CommandText = "select name from tb_teachinfo where tea_id=@ID";
                if (_connection.State == ConnectionState.Closed)
                    _connection.Open();
                command.Parameters.AddWithValue("@ID", Id);
                string name = command.ExecuteScalar().ToString();
                _connection.Close();
                return name;
            }
        }
        /// <summary>
        /// 获取教师映射表
        /// </summary>
        /// <returns></returns>
        public IEnumerable<SelectOptions> GetTeachers()
        {
            using (SqlCommand command = _connection.CreateCommand())
            {
                command.CommandText = "select distinct tea_id,name from tb_teachinfo";
                if (_connection.State == ConnectionState.Closed)
                    _connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    SelectOptions options = new SelectOptions
                    {
                        value = reader.GetString(0),
                        text = reader.GetString(1)
                    };
                    yield return options;
                }
                reader.Close();
                _connection.Close();
            }
        }
        /// <summary>
        /// 获取教师信息列表
        /// </summary>
        /// <returns></returns>
        public IEnumerable<DataBaseModels.TeachInfo> GetTeachInfoList()
        {
            SqlDataAdapter adapter = new SqlDataAdapter("select * from tb_teachinfo", _connection);
            DataTable table = new DataTable();
            adapter.Fill(table);

            foreach (DataRow item in table.Rows)
            {
                DataBaseModels.TeachInfo teachInfo = new DataBaseModels.TeachInfo
                {
                    Tea_id = item.Field<string>(0),
                    Name = item.Field<string>(1),
                    Major_id = item.Field<int>(6)
                };
                yield return teachInfo;
            }
        }
        /// <summary>
        /// 添加教师
        /// </summary>
        /// <param name="teachers"></param>
        /// <returns></returns>
        public bool AddTeachers(WebModels.Teacher[] teachers)
        {
            SqlDataAdapter teacher_adapter = new SqlDataAdapter("select * from tb_teachinfo", _connection);
            SqlDataAdapter user_adapter = new SqlDataAdapter("select * from tb_users", _connection);
            SqlCommandBuilder teacher_builder = new SqlCommandBuilder(teacher_adapter);
            SqlCommandBuilder user_builder = new SqlCommandBuilder(user_adapter);
            DataTable tb_teachers = new DataTable();
            DataTable tb_users = new DataTable();
            teacher_adapter.Fill(tb_teachers);
            user_adapter.Fill(tb_users);

            foreach (var item in teachers)
            {
                DataRow tea = tb_teachers.NewRow();
                DataRow user = tb_users.NewRow();
                tea[0] = item.Tea_id;
                tea[1] = item.TeaName;
                tea[6] = item.MajorId;

                user[0] = item.Tea_id;
                user[1] = Encryption(item.Passwd);
                user[2] = 1;

                tb_teachers.Rows.Add(tea);
                tb_users.Rows.Add(user);
            }

            int t = teacher_adapter.Update(tb_teachers);
            int u = user_adapter.Update(tb_users);
            if (t > 0 && u > 0)
                return true;
            return false;
        }
        /// <summary>
        /// 删除教师
        /// </summary>
        /// <param name="teaId"></param>
        /// <returns></returns>
        public bool DeleteTacher(string teaId)
        {
            bool success = this.DeleteUser(teaId);
            if (!success)
                return false;
            using (SqlCommand command = _connection.CreateCommand())
            {
                command.CommandText = "delete from tb_teachinfo where tea_id=@Id";
                if (_connection.State == ConnectionState.Closed)
                    _connection.Open();
                SqlParameter parameter = new SqlParameter("@Id", teaId);
                command.Parameters.Add(parameter);
                int changeNumber = command.ExecuteNonQuery();
                _connection.Close();
                if (success && changeNumber > 0)
                    return true;
                return false;
            }
        }
        /// <summary>
        /// 更新教师信息
        /// </summary>
        /// <param name="oldTeachId"></param>
        /// <param name="teacher"></param>
        /// <returns></returns>
        public bool UpdateTeacher(string oldTeachId, WebModels.Teacher teacher)
        {
            bool success = this.UpdateUser(oldTeachId, teacher.Tea_id);
            using (SqlCommand command = _connection.CreateCommand())
            {
                command.CommandText = "update tb_teachinfo set" +
                    " tea_id=@newId,name=@teaName,major_id=@majorId " +
                    "where tea_id=@oldTeachId";
                if (_connection.State == ConnectionState.Closed)
                    _connection.Open();
                command.Parameters.AddWithValue("@newId", teacher.Tea_id);
                command.Parameters.AddWithValue("@teaName", teacher.TeaName);
                command.Parameters.AddWithValue("@majorId", teacher.MajorId);
                command.Parameters.AddWithValue("@oldTeachId", oldTeachId);
                int changeNumber = command.ExecuteNonQuery();
                _connection.Close();
                if (success && changeNumber > 0)
                    return true;
                return false;
            }
        }
        #endregion

        #region 学生
        /// <summary>
        /// 获取学生姓名
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public string SelectStudentName(string Id)
        {
            using (SqlCommand command = _connection.CreateCommand())
            {
                command.CommandText = "select name from tb_stuinfo where stu_id=@ID";
                if (_connection.State == ConnectionState.Closed)
                    _connection.Open();
                command.Parameters.AddWithValue("@ID", Id);
                string name = command.ExecuteScalar().ToString();
                _connection.Close();
                return name;
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
        /// <summary>
        /// 获取所有学生
        /// </summary>
        /// <returns></returns>
        public IEnumerable<DataBaseModels.StuInfo> GetStudents()
        {
            SqlDataAdapter adapter = new SqlDataAdapter("select stu_id,name,major_id,enroll_year,class_number,gender from tb_stuinfo", _connection);
            DataTable table = new DataTable();
            adapter.Fill(table);

            foreach (DataRow item in table.Rows)
            {
                DataBaseModels.StuInfo info = new DataBaseModels.StuInfo
                {
                    Stu_id = item.Field<string>(0),
                    Name = item.Field<string>(1),
                    Major_id = item.Field<int>(2),
                    Enroll_year = item.Field<int>(3),
                    ClassNumer = item.Field<int>(4)
                };
                Gender gender = (Gender)item.Field<int>(5);
                switch (gender)
                {
                    case Gender.Unknow:
                        break;
                    case Gender.Male:
                        info.Sex = Gender.Male;
                        break;
                    case Gender.Female:
                        info.Sex = Gender.Female;
                        break;
                    default:
                        break;
                }
                yield return info;
            }
        }
        /// <summary>
        /// 获取专业学生
        /// </summary>
        /// <param name="majorId"></param>
        /// <returns></returns>
        public IEnumerable<DataBaseModels.StuInfo> GetStudents(int majorId)
        {
            string selectCommandText = $"select stu_id,name,major_id,enroll_year,class_number,gender from tb_stuinfo where major_id={majorId}";
            SqlDataAdapter adapter = new SqlDataAdapter(selectCommandText, _connection);
            DataTable table = new DataTable();
            adapter.Fill(table);

            foreach (DataRow item in table.Rows)
            {
                DataBaseModels.StuInfo info = new DataBaseModels.StuInfo
                {
                    Stu_id = item.Field<string>(0),
                    Name = item.Field<string>(1),
                    Major_id = item.Field<int>(2),
                    Enroll_year = item.Field<int>(3),
                    ClassNumer = item.Field<int>(4)
                };
                Gender gender = (Gender)item.Field<int>(5);
                switch (gender)
                {
                    case Gender.Unknow:
                        break;
                    case Gender.Male:
                        info.Sex = Gender.Male;
                        break;
                    case Gender.Female:
                        info.Sex = Gender.Female;
                        break;
                    default:
                        break;
                }
                yield return info;
            }
        }

        public IEnumerable<DataBaseModels.StuInfo> GetStudents(string teacherId)
        {
            using (SqlCommand command = _connection.CreateCommand())
            {
                command.CommandText =
                    @"select tb_stuinfo.stu_id,tb_stuinfo.name,tb_stuinfo.major_id,tb_stuinfo.enroll_year,tb_stuinfo.class_number,tb_stuinfo.gender from tb_stuinfo 
                        left join tb_stu_course on tb_stuinfo.stu_id = tb_stu_course.stu_id
                        left join tb_course on tb_stu_course.course_id = tb_course.course_id
                        where tea_id = @ID";
                command.Parameters.AddWithValue("@ID", teacherId);
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable table = new DataTable();
                adapter.Fill(table);

                foreach (DataRow item in table.Rows)
                {
                    DataBaseModels.StuInfo info = new DataBaseModels.StuInfo
                    {
                        Stu_id = item.Field<string>(0),
                        Name = item.Field<string>(1),
                        Major_id = item.Field<int>(2),
                        Enroll_year = item.Field<int>(3),
                        ClassNumer = item.Field<int>(4)
                    };
                    Gender gender = (Gender)item.Field<int>(5);
                    switch (gender)
                    {
                        case Gender.Unknow:
                            break;
                        case Gender.Male:
                            info.Sex = Gender.Male;
                            break;
                        case Gender.Female:
                            info.Sex = Gender.Female;
                            break;
                        default:
                            break;
                    }
                    yield return info;
                }
            }
        }


        /// <summary>
        /// 更新学生信息
        /// </summary>
        /// <param name="oldStuId"></param>
        /// <param name="student"></param>
        /// <returns></returns>
        public bool UpdateStudent(string oldStuId, WebModels.Student student)
        {
            bool success = this.UpdateUser(oldStuId, student.StuId);
            using (SqlCommand command = _connection.CreateCommand())
            {
                command.CommandText = "update tb_stuinfo set" +
                    " stu_id=@newId,name=@stuName,major_id=@majorId,enroll_year=@enter,class_number=@class,gender=@sex " +
                    "where stu_id=@oldStuId";
                if (_connection.State == ConnectionState.Closed)
                    _connection.Open();
                command.Parameters.AddWithValue("@newId", student.StuId);
                command.Parameters.AddWithValue("@stuName", student.Name);
                command.Parameters.AddWithValue("@majorId", student.MajorId);
                command.Parameters.AddWithValue("@enter", student.EnrollYear);
                command.Parameters.AddWithValue("@class", student.ClassNumber);
                command.Parameters.AddWithValue("@sex", (int)student.Gender);
                command.Parameters.AddWithValue("@oldStuId", oldStuId);
                int changeNumber = command.ExecuteNonQuery();
                _connection.Close();
                if (success && changeNumber > 0)
                    return true;
                return false;
            }
        }
        public bool DeleteStudent(string stuId)
        {
            bool success = this.DeleteUser(stuId);
            if (!success)
                return false;
            using (SqlCommand command = _connection.CreateCommand())
            {
                command.CommandText = "delete from tb_stuinfo where stu_id==@Id";
                if (_connection.State == ConnectionState.Closed)
                    _connection.Open();
                SqlParameter parameter = new SqlParameter("@Id", stuId);
                command.Parameters.Add(parameter);
                int changeNumber = command.ExecuteNonQuery();
                _connection.Close();
                if (success && changeNumber > 0)
                    return true;
                return false;
            }
        }
        public void DeleteStudents(string[] stuIds)
        {
            SqlDataAdapter sda_user = new SqlDataAdapter("select * from tb_users", _connection);
            SqlDataAdapter sda_stu = new SqlDataAdapter("select * from tb_stuinfo", _connection);

            DataTable tb_user = new DataTable();
            DataTable tb_stu = new DataTable();

            sda_user.Fill(tb_user);
            sda_stu.Fill(tb_stu);

            foreach (var item in stuIds)
            {
                DataRow[] user_row = tb_user.Select($"id='{item}'");
                DataRow[] stu_row = tb_stu.Select($"stu_id='{item}'");

                user_row[0].Delete();
                stu_row[0].Delete();
            }

            new SqlCommandBuilder(sda_user);
            new SqlCommandBuilder(sda_stu);

            sda_user.Update(tb_user);
            sda_stu.Update(tb_stu);
        }
        /// <summary>
        /// 添加学生
        /// </summary>
        /// <param name="informations"></param>
        /// <returns></returns>
        public bool AddStudents(IEnumerable<Information> informations)
        {
            SqlDataAdapter stu_adapter = new SqlDataAdapter("select stu_id,name,enroll_year,gender,major_id,class_number,credit_got from tb_stuinfo", _connection);
            SqlDataAdapter major_adapter = new SqlDataAdapter("select * from tb_major", _connection);
            SqlDataAdapter user_adapter = new SqlDataAdapter("select * from tb_users", _connection);

            DataTable tb_stu = new DataTable();
            DataTable tb_major = new DataTable();
            DataTable tb_user = new DataTable();
            stu_adapter.Fill(tb_stu);
            major_adapter.Fill(tb_major);
            user_adapter.Fill(tb_user);
            string pwd = Encryption("123456");

            foreach (var item in informations)
            {
                DataRow row = tb_stu.NewRow();
                row[0] = item.StudentId;
                row[1] = item.Name;
                row[2] = item.EnrollmentYear;
                row[3] = (int)item.Sex;
                DataRow[] rows = tb_major.Select($"name='{item.Major}'");
                if (rows.Length == 0)
                    continue;
                row[4] = rows[0][0];
                row[5] = item.ClassNumber;
                row[6] = 0;

                DataRow urow = tb_user.NewRow();
                urow[0] = item.StudentId;
                urow[1] = pwd;
                urow[2] = 2;

                tb_stu.Rows.Add(row);
                tb_user.Rows.Add(urow);
            }

            new SqlCommandBuilder(stu_adapter);
            new SqlCommandBuilder(user_adapter);
            int num_stu = stu_adapter.Update(tb_stu);
            int num_user = user_adapter.Update(tb_user);
            if (num_stu > 0 && num_user > 0)
                return true;
            return false;
        }
        /// <summary>
        /// 添加单个学生
        /// </summary>
        /// <param name="student"></param>
        public void AddStudent(WebModels.Student student)
        {
            SqlDataAdapter sda_stu = new SqlDataAdapter("select stu_id,name,enroll_year,gender,major_id,class_number,credit_got from tb_stuinfo", _connection);
            SqlDataAdapter sda_user = new SqlDataAdapter("select * from tb_users", _connection);

            DataTable tb_stu = new DataTable();
            DataTable tb_user = new DataTable();

            sda_stu.Fill(tb_stu);
            sda_user.Fill(tb_user);

            DataRow row_stu = tb_stu.NewRow();
            DataRow row_user = tb_user.NewRow();

            row_stu[0] = student.StuId;
            row_stu[1] = student.Name;
            row_stu[2] = student.EnrollYear;
            row_stu[3] = (int)student.Gender;
            row_stu[4] = student.MajorId;
            row_stu[5] = student.ClassNumber;
            row_stu[6] = 0;

            row_user[0] = student.StuId;
            row_user[1] = Encryption("123456");
            row_user[2] = 2;

            tb_stu.Rows.Add(row_stu);
            tb_user.Rows.Add(row_user);

            new SqlCommandBuilder(sda_stu);
            new SqlCommandBuilder(sda_user);

            sda_stu.Update(tb_stu);
            sda_user.Update(tb_user);
        }

        public IEnumerable<object> GetYearList()
        {
            using (SqlCommand command = _connection.CreateCommand())
            {
                command.CommandText = "select distinct enroll_year from tb_stuinfo";
                if (_connection.State == ConnectionState.Closed)
                    _connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    int year = reader.GetInt32(0);
                    yield return new { text = year, value = year.ToString() };
                }
                reader.Close();
                _connection.Close();
            }
        }
        #endregion

        #region  学生成绩

        public IEnumerable<DataBaseModels.GradeForStudent> GetStudentGrades(string stu_id,int term)
        {
            using (SqlCommand command = _connection.CreateCommand())
            {
                command.CommandText = @"select tb_stu_course.stu_course_id,tb_stu_course.course_id,
                            tb_stu_course.term,tb_stu_course.total_remark,
                            tb_stu_course.status_id,tb_teachinfo.name from tb_stu_course
                            left join tb_course on tb_stu_course.course_id=tb_course.course_id 
                            left join tb_teachinfo on tb_course.tea_id=tb_teachinfo.tea_id where stu_id=@ID";
                command.Parameters.AddWithValue("@ID", stu_id);
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable table = new DataTable();
                adapter.Fill(table);
                DataRow[] rows;
                if (term == 0)
                    rows = table.Select();
                else
                    rows = table.Select($"term={term}");
                foreach (var item in rows)
                {
                    DataBaseModels.GradeForStudent grade = new DataBaseModels.GradeForStudent
                    {
                        Id = item.Field<int>(0),
                        CourseId = item.Field<int>(1),
                        Term = item.Field<int>(2),
                        TotalRemark = item.Field<double?>(3),
                        Status = item.Field<int>(4),
                        TeacherName = item.Field<string>(5)
                    };
                    yield return grade;
                }
            }
        }


        #endregion

        #region 用户

        public bool DeleteUser(string Id)
        {
            using (SqlCommand command = _connection.CreateCommand())
            {
                command.CommandText = "delete from tb_users where id=@Id";
                if (_connection.State == ConnectionState.Closed)
                    _connection.Open();
                SqlParameter parameter = new SqlParameter("@Id", Id);
                command.Parameters.Add(parameter);
                int changeNumber = command.ExecuteNonQuery();
                _connection.Close();
                if (changeNumber > 0)
                    return true;
                return false;
            }
        }
        public bool UpdateUser(string oldId, string newId)
        {
            using (SqlCommand command = _connection.CreateCommand())
            {
                command.CommandText = "update tb_users set id=@newId where id=@oldId";
                if (_connection.State == ConnectionState.Closed)
                    _connection.Open();
                command.Parameters.AddWithValue("@newId", newId);
                command.Parameters.AddWithValue("@oldId", oldId);
                int changeNumber = command.ExecuteNonQuery();
                _connection.Close();
                if (changeNumber > 0)
                    return true;
                return false;
            }
        }
        #endregion

        #region  成绩

        public IEnumerable<DataBaseModels.Grade> GetGrades(string teaId)
        {
            using (SqlCommand command = _connection.CreateCommand())
            {
                command.CommandText = @"select tb_stu_course.stu_course_id,tb_stu_course.course_id,tb_stu_course.stu_id,tb_stuinfo.name,tb_stu_course.term,
                        tb_stu_course.daily_work,tb_stu_course.mid_exam,tb_stu_course.final_exam,tb_stu_course.total_remark,tb_stu_course.status_id
                        from tb_stu_course left join tb_stuinfo on tb_stu_course.stu_id=tb_stuinfo.stu_id 
                        where course_id in (select tb_course.course_id from tb_course where tea_id=@ID)";
                command.Parameters.AddWithValue("@ID", teaId);

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable table = new DataTable();
                adapter.Fill(table);


                foreach (DataRow item in table.Rows)
                {
                    DataBaseModels.Grade grade = new DataBaseModels.Grade
                    {
                        Id = item.Field<int>(0),
                        CourseId = item.Field<int>(1),
                        StudentId = item.Field<string>(2),
                        Name = item.Field<string>(3),
                        Term = item.Field<int>(4),
                        DailyWork = item.Field<double?>(5),
                        MidExam = item.Field<double?>(6),
                        FinalExam = item.Field<double?>(7),
                        TotalRemark = item.Field<double?>(8),
                        Status = item.Field<int>(9)
                    };
                    yield return grade;
                }
            }
        }

        public IEnumerable<DataBaseModels.Grade> GetGrades(string teaId,int courseId)
        {
            using (SqlCommand command = _connection.CreateCommand())
            {
                command.CommandText = @"select tb_stu_course.stu_course_id,tb_stu_course.course_id,tb_stu_course.stu_id,tb_stuinfo.name,tb_stu_course.term,
                        tb_stu_course.daily_work,tb_stu_course.mid_exam,tb_stu_course.final_exam,tb_stu_course.total_remark,tb_stu_course.status_id
                        from tb_stu_course left join tb_stuinfo on tb_stu_course.stu_id=tb_stuinfo.stu_id 
                        where course_id in (select tb_course.course_id from tb_course where tea_id=@ID)";
                command.Parameters.AddWithValue("@ID", teaId);

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable table = new DataTable();
                adapter.Fill(table);

                DataRow[] rows = table.Select($"course_id={courseId}");

                foreach (DataRow item in rows)
                {
                    DataBaseModels.Grade grade = new DataBaseModels.Grade
                    {
                        Id = item.Field<int>(0),
                        CourseId = item.Field<int>(1),
                        StudentId = item.Field<string>(2),
                        Name = item.Field<string>(3),
                        Term = item.Field<int>(4),
                        DailyWork = item.Field<double?>(5),
                        MidExam = item.Field<double?>(6),
                        FinalExam = item.Field<double?>(7),
                        TotalRemark = item.Field<double?>(8),
                        Status = item.Field<int>(9)
                    };
                    yield return grade;
                }
            }
        }

        public void UpdateGrade(WebModels.Grade grade)
        {
            SqlDataAdapter adapter = new SqlDataAdapter("select stu_course_id,daily_work,mid_exam,final_exam,total_remark from tb_stu_course", _connection);
            DataTable table = new DataTable();
            adapter.Fill(table);

            new SqlCommandBuilder(adapter);

            DataRow row = table.Select($"stu_course_id={grade.Id}")[0];

            if (grade.DailyWork != null)
                row[1] = grade.DailyWork;
            if (grade.MidExam != null)
                row[2] = grade.MidExam;
            if (grade.FinalExam != null)
                row[3] = grade.FinalExam;
            if (grade.TotalRemark != null)
                row[4] = grade.TotalRemark;

            adapter.Update(table);
        }

        public bool AddGrades(IEnumerable<Achievement> achievements,int courseID)
        {
            using (SqlCommand command=_connection.CreateCommand())
            {
                command.CommandText = "select stu_id,daily_work,mid_exam,final_exam,total_remark,stu_course_id from tb_stu_course where course_id=@courseID";
                command.Parameters.AddWithValue("@courseID", courseID);
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable table = new DataTable();
                adapter.Fill(table);

                foreach (var item in achievements)
                {
                    DataRow[] rows = table.Select($"stu_id='{item.StudentId}'");
                    if (rows.Length == 0)
                        continue;
                    DataRow row = rows[0];
                    row[1] = item.RegularGrade;
                    row[2] = item.MidtermGrade;
                    row[3] = item.FinalExamGrade;
                }
                new SqlCommandBuilder(adapter);
                return adapter.Update(table) > 0 ? true : false;
            }
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