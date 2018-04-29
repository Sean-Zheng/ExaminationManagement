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
    public class ExcalManager
    {
        private IWorkbook _workbook;

        public ExcalManager(string path)
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
        //成绩
        public IEnumerable<Achievement> GetAchievement()
        {
            ISheet sheet = _workbook.GetSheetAt(0);
            if (sheet == null || sheet.LastRowNum == 0)
                return null;
            IRow cells = sheet.GetRow(0);
            if (cells.LastCellNum != 6)
                return null;
            for (int i = 0; i < cells.LastCellNum; i++)
            {

            }
            return null;
        }
        
        //public IEnumerable<StuInfo> GetStuInfo()
        //{
        //    ISheet sheet = _workbook.GetSheetAt(0);
        //    if (sheet == null || sheet.LastRowNum == 0)
        //        return null;
        //    for (int i = 0; i < sheet.LastRowNum; i++)
        //    {
        //        IRow cells = sheet.GetRow(i);
        //        for (int j = 0; j < cells.LastCellNum; j++)
        //        {
        //            ICell cell = cells.GetCell(j);
        //        }
        //    }
        //}

        #region
        //public DataTable GetTable(ISheet sheet)
        //{
        //    if (sheet == null || sheet.LastRowNum == 0)
        //        return null;
        //    DataTable table = new DataTable(sheet.SheetName);
        //    IRow row = null;
        //    if (this.m_existTitle)
        //    {
        //        row = sheet.GetRow(0);
        //        for (int i = 0; i < row.LastCellNum; i++)
        //        {
        //            ICell cell = row.GetCell(i);
        //            DataColumn column = new DataColumn(cell.ToString().Trim());
        //            table.Columns.Add(column);
        //        }
        //        for (int i = 1; i < sheet.LastRowNum; i++)
        //        {
        //            row = sheet.GetRow(i);
        //            DataRow dr = table.NewRow();
        //            for (int j = 0; j < row.LastCellNum; j++)
        //            {
        //                ICell cell = row.GetCell(j);
        //                dr[j] = cell.ToString();
        //            }
        //            table.Rows.Add(dr);
        //        }
        //    }
        //    else
        //    {
        //        row = sheet.GetRow(0);
        //        DataColumn[] columns = new DataColumn[row.LastCellNum];
        //        table.Columns.AddRange(columns);
        //        for (int i = 0; i < sheet.LastRowNum; i++)
        //        {
        //            row = sheet.GetRow(i);
        //            DataRow dr = table.NewRow();
        //            for (int j = 0; j < row.LastCellNum; j++)
        //            {
        //                ICell cell = row.GetCell(j);
        //                dr[j] = cell.ToString();
        //            }
        //            table.Rows.Add(dr);
        //        }
        //    }
        //    return table;
        //}
        #endregion
    }
}