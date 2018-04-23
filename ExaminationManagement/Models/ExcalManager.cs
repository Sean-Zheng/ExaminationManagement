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
    public class ExcalManager
    {
        private IWorkbook _workbook;
        private bool _existTitle;

        //public int SheetCount
        //{
        //    get => this._workbook.NumberOfSheets;
        //}

        public ExcalManager(string path, bool existTitle)
        {
            this._existTitle = existTitle;
            string extension = Path.GetExtension(path);
            if (extension == ".xlsx")//Excel2007
                this._workbook = new XSSFWorkbook(File.Open(path, FileMode.OpenOrCreate, FileAccess.ReadWrite));
            else if (extension == ".xls")//Excel2003
                this._workbook = new HSSFWorkbook(File.Open(path, FileMode.OpenOrCreate, FileAccess.ReadWrite));
        }
        //public ISheet GetSheet()
        //{
        //    return this.m_workbook.GetSheetAt(0);
        //}
        //public ISheet GetSheet(int index)
        //{
        //    return this.m_workbook.GetSheetAt(index);
        //}
        //public ISheet GetSheet(string name)
        //{
        //    return this.m_workbook.GetSheet(name);
        //}
        public IEnumerable<string> GetSheets()
        {
            for (int i = 0; i < this._workbook.NumberOfSheets; i++)
                yield return this._workbook.GetSheetName(i);
        }
        public DataTable GetAchievement()
        {
            return null;
        }
        public DataTable GetInformation()
        {
            return null;
        }
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
    }
}