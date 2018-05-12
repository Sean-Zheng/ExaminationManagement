using ExaminationManagement.Models.ExcelModels;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;

namespace ExaminationManagement.Models
{
    /// <summary>
    /// Excel操作
    /// </summary>
    public class ExcelManager
    {
        private IWorkbook _workbook;

        public ExcelManager(string path)
        {
            string extension = Path.GetExtension(path);
            if (extension == ".xlsx")//Excel2007
                this._workbook = new XSSFWorkbook(File.Open(path, FileMode.OpenOrCreate, FileAccess.ReadWrite));
            else if (extension == ".xls")//Excel2003
                this._workbook = new HSSFWorkbook(File.Open(path, FileMode.OpenOrCreate, FileAccess.ReadWrite));
        }
        /// <summary>
        /// 获取工作簿列表
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> GetSheets()
        {
            for (int i = 0; i < this._workbook.NumberOfSheets; i++)
                yield return this._workbook.GetSheetName(i);
        }
        /// <summary>
        /// 检查成绩模板是否合法
        /// </summary>
        /// <returns></returns>
        public bool CheackAchievementTemplates()
        {
            ISheet sheet = _workbook.GetSheetAt(0);
            if (sheet == null || sheet.LastRowNum == 0)
                return false;
            IRow cells = sheet.GetRow(0);
            if (cells.LastCellNum != 5)
                return false;
            //if (cells.GetCell(0).ToString() != "课程名")
            //    return false;
            if (cells.GetCell(0).ToString() != "学生姓名")
                return false;
            if (cells.GetCell(1).ToString() != "学号")
                return false;
            if (cells.GetCell(2).ToString() != "平时成绩")
                return false;
            if (cells.GetCell(3).ToString() != "期中成绩")
                return false;
            if (cells.GetCell(4).ToString() != "期末成绩")
                return false;
            return true;
        }
        /// <summary>
        /// 返回学生成绩
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Achievement> GetAchievement()
        {
            ISheet sheet = _workbook.GetSheetAt(0);
            double temp;
            for (int i = 1; i <= sheet.LastRowNum; i++)
            {
                IRow cells = sheet.GetRow(i);
                Achievement achievement = new Achievement
                {                    
                    Name = cells.GetCell(0).ToString() ?? "",
                    StudentId = cells.GetCell(1).ToString() ?? "",
                    RegularGrade = double.TryParse(cells.GetCell(2).ToString(), out temp) ? temp : 0,
                    MidtermGrade = double.TryParse(cells.GetCell(3).ToString(), out temp) ? temp : 0,
                    FinalExamGrade = double.TryParse(cells.GetCell(4).ToString(), out temp) ? temp : 0
                };
                yield return achievement;
            }
        }
        /// <summary>
        /// 检查信息模板是否合法
        /// </summary>
        /// <returns></returns>
        public bool CheackInformationTemplates()
        {
            ISheet sheet = _workbook.GetSheetAt(0);
            if (sheet == null || sheet.LastRowNum == 0)
                return false;
            IRow cells = sheet.GetRow(0);
            if (cells.LastCellNum != 6)
                return false;
            if (cells.GetCell(0).ToString() != "学号")
                return false;
            if (cells.GetCell(1).ToString() != "姓名")
                return false;
            if (cells.GetCell(2).ToString() != "入学年份")
                return false;
            if (cells.GetCell(3).ToString() != "性别")
                return false;
            if (cells.GetCell(4).ToString() != "专业")
                return false;
            if (cells.GetCell(5).ToString() != "班级")
                return false;
            return true;
        }
        /// <summary>
        /// 返回学生信息
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Information> GetInformation()
        {
            ISheet sheet = _workbook.GetSheetAt(0);
            int temp;
            for (int i = 1; i <= sheet.LastRowNum; i++)
            {
                IRow cells = sheet.GetRow(i);
                Information information = new Information
                {
                    StudentId = cells.GetCell(0).ToString() ?? "",
                    Name = cells.GetCell(1).ToString() ?? "",
                    EnrollmentYear = int.TryParse(cells.GetCell(2).ToString(), out temp) ? temp : 0,
                    Major = cells.GetCell(4).ToString() ?? "",
                    ClassNumber = int.TryParse(cells.GetCell(5).ToString(), out temp) ? temp : 0,
                };
                if (cells.GetCell(3).ToString() == "男")
                    information.Sex = Gender.Male;
                else if (cells.GetCell(3).ToString() == "女")
                    information.Sex = Gender.Female;
                else
                    information.Sex = Gender.Unknow;
                yield return information;
            }
        }
    }
}