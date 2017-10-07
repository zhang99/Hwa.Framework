using NPOI.HPSF;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;

namespace Hwa.Framework.Excel
{
    /// <summary>
    /// NPOIExcel操作相关
    /// </summary>
    public class NPOIExcel
    {
        #region 读取Excel到DataTable

        /// <summary>
        /// 读取Excel文件的内容
        /// </summary>
        /// <param name="path"></param>
        /// <param name="sheetName">工作表名称</param>
        /// <returns></returns>
        public DataTable GetDataTable(string path, string sheetName = null)
        {
            using (FileStream file = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                return GetDataTable(file, sheetName);
            }
        }

        /// <summary>
        /// 从Excel文件流读取内容
        /// </summary>
        /// <param name="file"></param>
        /// <param name="sheetName"></param>
        /// <returns></returns>
        public DataTable GetDataTable(Stream file, string contentType, string sheetName = null)
        {
            //载入工作簿
            IWorkbook workBook = null;
            if (contentType == "application/vnd.ms-excel")
            {
                workBook = new HSSFWorkbook(file);
            }
            else if (contentType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            {
                workBook = new XSSFWorkbook(file);
            }
            else
            {
                try
                {
                    workBook = new HSSFWorkbook(file);
                }
                catch
                {
                    try
                    {
                        workBook = new XSSFWorkbook(file);
                    }
                    catch
                    {
                        throw new Exception("文件格式不被支持!");
                    }
                }
            }

            //获取工作表(sheetName为空则默认获取第一个工作表)
            var sheet = string.IsNullOrEmpty(sheetName) ? workBook.GetSheetAt(0) : workBook.GetSheet(sheetName);
            //生成DataTable
            if (sheet != null)
                return GetDataTable(sheet);
            else
                throw new Exception(string.Format("工作表{0}不存在!", sheetName ?? ""));

        }

        /// <summary>
        /// 读取工作表数据
        /// </summary>
        /// <param name="sheet"></param>
        /// <returns></returns>
        private DataTable GetDataTable(ISheet sheet)
        {
            IEnumerator rows = sheet.GetRowEnumerator();

            DataTable dt = new DataTable(sheet.SheetName);

            //默认第一个非空行为列头
            bool isColumnTitle = true;
            //标题行索引
            int titleRowIndex = 0;
            //默认列头后的第一个数据行，作为DataTable列类型的依据
            IRow firstDataRow = null;

            while (rows.MoveNext())
            {
                IRow row = null;
                if (rows.Current is XSSFRow)//*.xlsx
                {
                    row = (XSSFRow)rows.Current;
                }
                else//*.xls
                {
                    row = (HSSFRow)rows.Current;
                }

                //是否空行(如果是合并过的单元格，默认为标题行)
                if (IsEmptyRow(row) || row.Cells.Count <= 1 || row.Cells[0].IsMergedCell)
                {
                    if (isColumnTitle)
                    {
                        titleRowIndex++;
                    }
                    continue;
                }
                else
                {
                    if (isColumnTitle)
                    {
                        firstDataRow = sheet.GetRow(titleRowIndex + 1);//默认列头后的第一个数据行，作为DataTable列类型的依据
                    }
                }

                DataRow dr = dt.NewRow();

                for (int i = 0; i < row.LastCellNum; i++)
                {
                    var cell = row.GetCell(i);

                    if (isColumnTitle)
                    {
                        var firstDataRowCell = firstDataRow.GetCell(i);
                        if (firstDataRowCell != null || cell != null)
                        {
                            dt.Columns.Add(cell.StringCellValue.Trim());
                        }
                        else
                        {
                            dt.Columns.Add(string.Format("未知列{0}", i + 1));
                        }
                    }
                    else
                    {
                        if (i > dt.Columns.Count - 1) break;
                        dr[i] = GetCellValue(cell, dt.Columns[i].DataType);
                    }

                }
                if (!isColumnTitle && !IsEmptyRow(dr, dt.Columns.Count))
                {
                    dt.Rows.Add(dr);
                }
                isColumnTitle = false;
            }

            return dt;
        }

        /// <summary>
        /// 获取单元格值
        /// </summary>
        /// <param name="cell"></param>
        /// <param name="colType"></param>
        /// <returns></returns>
        private object GetCellValue(ICell cell, Type colType)
        {
            if (cell == null || cell.ToString().ToUpper().Equals("NULL") || cell.CellType == NPOI.SS.UserModel.CellType.Blank)
                return DBNull.Value;

            object val = null;
            switch (cell.CellType)
            {
                case NPOI.SS.UserModel.CellType.Boolean:
                    val = cell.BooleanCellValue;
                    break;
                case NPOI.SS.UserModel.CellType.Numeric:
                    var cellValueStr = cell.ToString().Trim();
                    if (cellValueStr.IndexOf('-') >= 0 || cellValueStr.IndexOf('/') >= 0)
                    {
                        DateTime d = DateTime.MinValue;
                        DateTime.TryParse(cellValueStr, out d);
                        if (!d.Equals(DateTime.MinValue)) val = cellValueStr;
                    }
                    if (val == null)
                    {
                        decimal vNum = 0;
                        decimal.TryParse(cellValueStr, out vNum);
                        val = vNum;
                    }
                    break;
                case NPOI.SS.UserModel.CellType.String:
                    val = cell.StringCellValue;
                    break;
                case NPOI.SS.UserModel.CellType.Error:
                    val = cell.ErrorCellValue;
                    break;
                case NPOI.SS.UserModel.CellType.Formula:
                default:
                    val = "=" + cell.CellFormula;
                    break;
            }

            return val;
        }

        /// <summary>
        /// 检查是否空数据行
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        private bool IsEmptyRow(DataRow dr, int colCount)
        {
            bool isEmptyRow = true;
            for (int i = 0; i < colCount; i++)
            {
                if (dr[i] != null && !dr[i].Equals(DBNull.Value))
                {
                    isEmptyRow = false;
                    break;
                }
            }
            return isEmptyRow;
        }

        /// <summary>
        /// 检查是否空的Excel行
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        private bool IsEmptyRow(IRow row)
        {
            bool isEmptyRow = true;
            for (int i = 0; i < row.LastCellNum; i++)
            {
                if (row.GetCell(i) != null)
                {
                    isEmptyRow = false;
                    break;
                }
            }

            return isEmptyRow;
        }
        #endregion

        #region 生成DataTable到Excel

        /// <summary>
        /// 生成Excel数据到路径
        /// </summary>
        /// <param name="data"></param>
        /// <param name="path"></param>
        public void GenerateExcel(DataTable data, string path)
        {
            GenerateExcel(data, path, false);
        }

        /// <summary>
        /// 生成Excel数据到路径
        /// </summary>
        /// <param name="data"></param>
        /// <param name="path"></param>
        /// <param name="autoSizeColumn">是否自适应宽度，默认为false</param>
        public void GenerateExcel(DataTable data, string path, bool autoSizeColumn)
        {
            var workBook = GenerateExcelData(data, autoSizeColumn);
            //保存至路径
            using (FileStream fs = File.OpenWrite(path)) //打开一个xls文件，如果没有则自行创建，如果存在则在创建时不要打开该文件！
            {
                workBook.Write(fs);   //向打开的这个xls文件中写入mySheet表并保存。
            }
        }

        /// <summary>
        /// 生成Excel数据到字节流
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public byte[] GenerateExcel(DataTable data)
        {
            return GenerateExcel(data, false);
        }

        /// <summary>
        /// 生成Excel数据到字节流
        /// </summary>
        /// <param name="data"></param>
        /// <param name="autoSizeColumn">是否自适应宽度，默认为false</param>
        /// <returns></returns>
        public byte[] GenerateExcel(DataTable data, bool autoSizeColumn)
        {
            var workBook = GenerateExcelData(data, autoSizeColumn);
            using (MemoryStream ms = new MemoryStream())
            {
                workBook.Write(ms);
                return ms.GetBuffer();
            }
        }

        /// <summary>
        /// 生成DataTable到Excel(TableName作为标题)
        /// </summary>
        /// <param name="data"></param>
        /// <param name="path"></param>
        private IWorkbook GenerateExcelData(DataTable data, bool autoSizeColumn)
        {
            if (data.Columns.Count == 0) throw new Exception("未指定任何可生成Excel的数据列！");

            //创建工作簿
            var workBook = new HSSFWorkbook();
            //生成文件基本信息
            GenerateSummaryInformation(workBook);
            //创建工作表
            var sheet = workBook.CreateSheet("Sheet1");
            //创建标题行
            int currentRowIndex = 0;
            if (!string.IsNullOrEmpty(data.TableName))
            {
                IRow row = sheet.CreateRow(currentRowIndex);
                currentRowIndex++;
                //合并单元格
                var rangAddress = new CellRangeAddress(0, 0, 0, data.Columns.Count - 1);
                sheet.AddMergedRegion(rangAddress);
                //标题样式
                var titleStyle = workBook.CreateCellStyle();
                titleStyle.Alignment = HorizontalAlignment.Center;
                var f = workBook.CreateFont();
                f.Boldweight = (short)FontBoldWeight.Bold;
                f.FontHeight = 300;
                titleStyle.SetFont(f);
                var cell = row.CreateCell(0);
                cell.SetCellValue(data.TableName);
                cell.CellStyle = titleStyle;
            }
            //创建列头
            if (data != null && data.Columns.Count > 0)
            {
                //列头样式
                var columnNameStyle = workBook.CreateCellStyle();
                var f = workBook.CreateFont();
                f.Boldweight = (short)FontBoldWeight.Bold;
                columnNameStyle.SetFont(f);
                //列头的行
                IRow row = sheet.CreateRow(currentRowIndex);
                currentRowIndex++;
                //列名
                for (int i = 0; i < data.Columns.Count; i++)
                {
                    var cell = row.CreateCell(i);
                    cell.SetCellValue(data.Columns[i].ColumnName);
                    cell.CellStyle = columnNameStyle;
                }
            }
            //创建数据行
            if (data != null && data.Rows.Count > 0)
            {
                for (int rowIndex = currentRowIndex; rowIndex <= data.Rows.Count + currentRowIndex - 1; rowIndex++)
                {
                    IRow row = sheet.CreateRow(rowIndex);
                    for (int colIndex = 0; colIndex < data.Columns.Count; colIndex++)
                    {
                        var cell = row.CreateCell(colIndex);
                        var dataValue = data.Rows[rowIndex - currentRowIndex][colIndex];
                        var dataType = data.Columns[colIndex].DataType;
                        SetCellValue(workBook, cell, dataType, dataValue);
                    }
                }
            }
            if (autoSizeColumn)
            {
                //宽度自适应(太耗时，通过参数控制，默认不自适应宽度)
                for (int i = 0; i < data.Columns.Count; i++)
                {
                    sheet.AutoSizeColumn(i);
                }
            }

            return workBook;
        }

        /// <summary>
        /// 设置数据格式
        /// </summary>
        /// <param name="workbook"></param>
        /// <param name="cell"></param>
        /// <param name="dataType"></param>
        /// <param name="dataValue"></param>
        public void SetCellValue(IWorkbook workbook, ICell cell, Type dataType, object dataValue)
        {
            switch (dataType.Name)
            {
                case "Byte":
                case "Int16":
                case "Int32":
                case "Int64":
                case "Single":
                    int intVal = 0;
                    if (dataValue != null && !dataValue.Equals(System.DBNull.Value))
                    {
                        int.TryParse(dataValue.ToString(), out intVal);
                        cell.SetCellValue(intVal);
                    }
                    cell.CellStyle = GetIntCellStyle(workbook);
                    break;
                case "Decimal":
                case "Double":
                    double doubleVal = 0;
                    if (dataValue != null && !dataValue.Equals(System.DBNull.Value))
                    {
                        double.TryParse(dataValue.ToString(), out doubleVal);
                        cell.SetCellValue(doubleVal);
                    }
                    cell.CellStyle = GetDecimalCellStyle(workbook);
                    break;
                case "Date":
                case "DateTime":
                    DateTime dateTimeVal = DateTime.MinValue;
                    if (dataValue != null && !dataValue.Equals(System.DBNull.Value))
                    {
                        DateTime.TryParse(dataValue.ToString(), out dateTimeVal);
                        if (dateTimeVal != DateTime.MinValue)
                        {
                            cell.SetCellValue(dateTimeVal);
                        }
                    }
                    if (dataType.Name == "Date")
                        cell.CellStyle = GetDateCellStyle(workbook);
                    else
                        cell.CellStyle = GetDateTimeCellStyle(workbook);
                    break;
                default:
                    if (dataValue != null && !dataValue.Equals(System.DBNull.Value))
                    {
                        cell.SetCellValue(dataValue.ToString());
                    }
                    break;
            }
        }

        #region 单元格样式(一个工作簿、一个类型只生成一个样式实体)
        private ICellStyle _intCellStyle;
        /// <summary>
        /// Int型数据单元格样式
        /// </summary>
        /// <param name="workbook"></param>
        /// <returns></returns>
        private ICellStyle GetIntCellStyle(IWorkbook workbook)
        {
            if (_intCellStyle == null)
            {
                _intCellStyle = workbook.CreateCellStyle();
                IDataFormat excelDataFormat = workbook.CreateDataFormat();
                _intCellStyle.DataFormat = excelDataFormat.GetFormat("#,##0");
            }
            return _intCellStyle;
        }

        private ICellStyle _decimalCellStyle;
        /// <summary>
        /// Decimal型数据单元格样式
        /// </summary>
        /// <param name="workbook"></param>
        /// <returns></returns>
        private ICellStyle GetDecimalCellStyle(IWorkbook workbook)
        {
            if (_decimalCellStyle == null)
            {
                _decimalCellStyle = workbook.CreateCellStyle();
                IDataFormat excelDataFormat = workbook.CreateDataFormat();
                _decimalCellStyle.DataFormat = excelDataFormat.GetFormat("#,##0.00");
            }
            return _decimalCellStyle;
        }

        private ICellStyle _dateTimeCellStyle;
        /// <summary>
        /// DateTime型数据单元格样式
        /// </summary>
        /// <param name="workbook"></param>
        /// <returns></returns>
        private ICellStyle GetDateTimeCellStyle(IWorkbook workbook)
        {
            if (_dateTimeCellStyle == null)
            {
                _dateTimeCellStyle = workbook.CreateCellStyle();
                IDataFormat excelDataFormat = workbook.CreateDataFormat();
                _dateTimeCellStyle.DataFormat = excelDataFormat.GetFormat("yyyy-mm-dd hh:mm:ss");
            }
            return _dateTimeCellStyle;
        }

        private ICellStyle _dateCellStyle;
        /// <summary>
        /// Date型数据单元格样式
        /// </summary>
        /// <param name="workbook"></param>
        /// <returns></returns>
        private ICellStyle GetDateCellStyle(IWorkbook workbook)
        {
            if (_dateCellStyle == null)
            {
                _dateCellStyle = workbook.CreateCellStyle();
                IDataFormat excelDataFormat = workbook.CreateDataFormat();
                _dateCellStyle.DataFormat = excelDataFormat.GetFormat("yyyy-mm-dd");
            }
            return _dateCellStyle;
        }
        #endregion

        /// <summary>
        /// 创建文档的基本信息(右击文件属性可看到的)
        /// </summary>
        /// <param name="workBook"></param>
        private void GenerateSummaryInformation(HSSFWorkbook workBook)
        {
            DocumentSummaryInformation dsi = PropertySetFactory.CreateDocumentSummaryInformation();
            dsi.Company = "深圳万国思迅软件有限公司";

            SummaryInformation si = PropertySetFactory.CreateSummaryInformation();
            si.Subject = "";//主题
            si.Author = "tdSystem";//作者

            workBook.DocumentSummaryInformation = dsi;
            workBook.SummaryInformation = si;
        }

        #endregion
    }
}

