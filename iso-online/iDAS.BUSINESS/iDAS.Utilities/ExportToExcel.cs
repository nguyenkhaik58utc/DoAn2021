using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Data;
using System.Reflection;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml;
using System.IO;
using System.Net;
using System.Runtime.Serialization;

//using DocumentFormat.OpenXml.Wordprocessing;

namespace iDAS.Utilities
{

    public class ExportToExcel
    {
        
        /// <summary>
        /// Chèn data vào  file excel có sẵn
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="xlsxFilePath"></param>
        /// <param name="dict"></param>
        /// <returns></returns>
        public static bool CreateExcelDocument<T>(List<T> list, string xlsxFilePath, Dictionary<string, string> dict, string filename, System.Web.HttpResponseBase Response, bool hasHeader = false,bool hasLoadFile=true)
        {
            DataSet ds = new DataSet();
            if (dict.Count > 0)
                AppendRefRow(xlsxFilePath, ListToDataTable(list, dict), hasHeader, filename, Response, dict,hasLoadFile);
            else
                AppendRefRow(xlsxFilePath, ListToDataTable(list), hasHeader, filename, Response, dict,hasLoadFile);
            //AppendRefRow(xlsxFilePath,ds.Tables[0]);
            return true;
            //return CreateExcelDocument(ds, xlsxFilePath);
        }
        #region HELPER_FUNCTIONS
        public static DataTable ListToDataTable<T>(List<T> list)
        {
            DataTable dt = new DataTable();

            foreach (PropertyInfo info in typeof(T).GetProperties())
            {
                dt.Columns.Add(new DataColumn(info.Name, GetNullableType(info.PropertyType)));
            }
            foreach (T t in list)
            {
                DataRow row = dt.NewRow();
                foreach (PropertyInfo info in typeof(T).GetProperties())
                {
                    if (!IsNullableType(info.PropertyType))
                        row[info.Name] = info.GetValue(t, null);
                    else
                        row[info.Name] = (info.GetValue(t, null) ?? DBNull.Value);
                }
                dt.Rows.Add(row);
            }
            return dt;
        }
        public static DataTable ListToDataTable<T>(List<T> list, Dictionary<string, string> dict)
        {
            DataTable dt = new DataTable();
            Dictionary<string, Type> _dc = new Dictionary<string, Type>();

            foreach (PropertyInfo info in typeof(T).GetProperties())
            {
                if (dict.ContainsKey(info.Name))
                    _dc.Add(info.Name, GetNullableType(info.PropertyType));
            }
            foreach (KeyValuePair<string, string> pair in dict)
            {
                dt.Columns.Add(new DataColumn(pair.Key));
                dt.Columns[pair.Key].DataType = _dc[pair.Key];
            }
            //Add Header
            //DataRow drHeader = dt.NewRow();
            //foreach (KeyValuePair<string, string> pair in dict)
            //{
            //    drHeader[pair.Key] = pair.Value;
            //}
            //dt.Rows.Add(drHeader);
            foreach (T t in list)
            {
                DataRow row = dt.NewRow();
                //Add content
                foreach (PropertyInfo info in typeof(T).GetProperties())
                {
                    if (dict.Keys.Contains(info.Name))
                    {
                        if (!IsNullableType(info.PropertyType))
                            row[info.Name] = info.GetValue(t, null).ToString();
                        else
                            row[info.Name] = (info.GetValue(t, null) ?? DBNull.Value);
                    }
                }
                dt.Rows.Add(row);
            }
            return dt;
        }
        public static string splitStyle(string str)
        {
            string sub;
            if (str.Contains("<span") && str.Contains("\">"))
            {
                int i1 = str.LastIndexOf("\">");
                int i2 = str.LastIndexOf("</");
                int i3 = str.Length;
                 sub= str.Substring(i1+2,i2-i1-2);
                 str = sub;
            }

            return str;
        }
        public static DataTable ListObjectToDataTable(List<Dictionary<string,string>> list, Dictionary<string, string> dict)
        {
            DataTable dt = new DataTable();
            if(dict.Count>0)
            {
                foreach (KeyValuePair<string, string> pair in dict)
                {
                    dt.Columns.Add(new DataColumn(pair.Key));
                }
                foreach (Dictionary<string,string> t in list)
                {
                    DataRow row = dt.NewRow();
                    foreach(KeyValuePair<string,string> pair in t)
                    {
                        if(dict.ContainsKey(pair.Key))
                        {
                            row[pair.Key] = pair.Value;
                        }
                    }                    
                    dt.Rows.Add(row);
                }
                
            }
            
            return dt;
        }
        private static Type GetNullableType(Type t)
        {
            Type returnType = t;
            if (t.IsGenericType && t.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                returnType = Nullable.GetUnderlyingType(t);
            }
            return returnType;
        }
        private static bool IsNullableType(Type type)
        {
            return (type == typeof(string) ||
                    type.IsArray ||
                    (type.IsGenericType &&
                     type.GetGenericTypeDefinition().Equals(typeof(Nullable<>))));
        }

        public static bool CreateExcelDocument(DataTable dt, string xlsxFilePath, Dictionary<string, string> dict)
        {
            DataSet ds = new DataSet();
            ds.Tables.Add(dt);
            bool result = CreateExcelDocument(ds, xlsxFilePath, dict);
            ds.Tables.Remove(dt);
            return result;
        }
        #endregion
        #region Main function
        public static bool CreateExcelDocument_obj(List<Dictionary<string, string>> list, string xlsxFilePath, Dictionary<string, string> dict, string filename, System.Web.HttpResponseBase Response, bool hasHeader = false, bool hasLoadFile = true)
        {
            DataSet ds = new DataSet();
            AppendRefRow(xlsxFilePath, ListObjectToDataTable(list, dict), hasHeader, filename, Response, dict, hasLoadFile);
            return true;
        }
        public static bool CreateExcelDocument_obj(List<Dictionary<string, string>> list, string filename, System.Web.HttpResponseBase Response, Dictionary<string, string> dict, string _title = "")
        {
            DataSet ds = new DataSet();
            if (dict.Count > 0)
                ds.Tables.Add(ListObjectToDataTable(list, dict));
            CreateExcelDocumentAsStream(ds, filename, Response, dict, _title);
            return true;
            //return CreateExcelDocument(ds, xlsxFilePath);
        }
        public static bool CreateExcelDocumentAsStream(DataSet ds, string filename, System.Web.HttpResponseBase Response, Dictionary<string, string> dict, string _title = "")
        {
            try
            {
                System.IO.MemoryStream stream = new System.IO.MemoryStream();
                using (SpreadsheetDocument document = SpreadsheetDocument.Create(stream, SpreadsheetDocumentType.Workbook, true))
                {
                    WriteExcelFile(ds, document, dict, _title);
                }
                stream.Flush();
                stream.Position = 0;
                byte[] data1 = new byte[stream.Length];
                stream.Read(data1, 0, data1.Length);
                stream.Close();
                Response.ClearContent();
                Response.Clear();
                Response.Buffer = true;
                Response.BufferOutput = false;
                Response.Charset = "";
                Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);
                Response.AddHeader("content-disposition", "attachment; filename=" + filename + ".xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.BinaryWrite(data1);
                Response.Flush();
                Response.End();
                Response.Close();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public static string CreateExcelDocumentAsFile(DataSet ds, string _path, Dictionary<string, string> dict, string _title = "")
        {
            try
            {
                using (SpreadsheetDocument document = SpreadsheetDocument.Create(_path, SpreadsheetDocumentType.Workbook, true))
                {
                    WriteExcelFile(ds, document, dict, _title);
                }
                return "success";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        #endregion

        /// <summary>
        /// Create an Excel file, and write it out to a MemoryStream (rather than directly to a file)
        /// </summary>
        /// <param name="dt">DataTable containing the data to be written to the Excel.</param>
        /// <param name="filename">The filename (without a path) to call the new Excel file.</param>
        /// <param name="Response">HttpResponse of the current page.</param>
        /// <returns>True if it was created succesfully, otherwise false.</returns>
        public static bool CreateExcelDocument(DataTable dt, string filename, System.Web.HttpResponseBase Response, Dictionary<string, string> dict)
        {
            try
            {
                DataSet ds = new DataSet();
                ds.Tables.Add(dt);
                CreateExcelDocumentAsStream(ds, filename, Response, dict);
                ds.Tables.Remove(dt);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        /// <summary>
        /// Xuất excel theo danh sách các tên cột truyền vào
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="filename"></param>
        /// <param name="Response"></param>
        /// <param name="dict"></param>
        /// <returns></returns>
        public static bool CreateExcelDocument<T>(List<T> list, string filename, System.Web.HttpResponseBase Response, Dictionary<string, string> dict, string _title = "")
        {
            try
            {
                DataSet ds = new DataSet();
                if (dict.Count > 0)
                    ds.Tables.Add(ListToDataTable(list, dict));
                else
                    ds.Tables.Add(ListToDataTable(list));
                CreateExcelDocumentAsStream(ds, filename, Response, dict, _title);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        
        /// <summary>
        /// Create an Excel file, and write it out to a MemoryStream (rather than directly to a file)
        /// </summary>
        /// <param name="ds">DataSet containing the data to be written to the Excel.</param>
        /// <param name="filename">The filename (without a path) to call the new Excel file.</param>
        /// <param name="Response">HttpResponse of the current page.</param>
        /// <returns>Either a MemoryStream, or NULL if something goes wrong.</returns>
       
        //#endif      //  End of "INCLUDE_WEB_FUNCTIONS" section

        /// <summary>
        /// Create an Excel file, and write it to a file.
        /// </summary>
        /// <param name="ds">DataSet containing the data to be written to the Excel.</param>
        /// <param name="excelFilename">Name of file to be written.</param>
        /// <returns>True if successful, false if something went wrong.</returns>
        public static bool CreateExcelDocument(DataSet ds, string excelFilename, Dictionary<string, string> dict)
        {
            try
            {
                using (SpreadsheetDocument document = SpreadsheetDocument.Create(excelFilename, SpreadsheetDocumentType.Workbook))
                {
                    WriteExcelFile(ds, document, dict);
                }
                //Trace.WriteLine("Successfully created: " + excelFilename);
                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }

        private static void WriteExcelFile(DataSet ds, SpreadsheetDocument spreadsheet, Dictionary<string, string> dict, string title = "")
        {
            //  Create the Excel file contents.  This function is used when creating an Excel file either writing 
            //  to a file, or writing to a MemoryStream.
            spreadsheet.AddWorkbookPart();
            spreadsheet.WorkbookPart.Workbook = new DocumentFormat.OpenXml.Spreadsheet.Workbook();

            //  My thanks to James Miera for the following line of code (which prevents crashes in Excel 2010)
            spreadsheet.WorkbookPart.Workbook.Append(new BookViews(new WorkbookView()));

            //  If we don't add a "WorkbookStylesPart", OLEDB will refuse to connect to this .xlsx file !
            WorkbookStylesPart workbookStylesPart = spreadsheet.WorkbookPart.AddNewPart<WorkbookStylesPart>("rIdStyles");
            Stylesheet stylesheet = GennerateStyle();
            workbookStylesPart.Stylesheet = stylesheet;
            //  Loop through each of the DataTables in our DataSet, and create a new Excel Worksheet for each.
            uint worksheetNumber = 1;
            foreach (DataTable dt in ds.Tables)
            {
                //  For each worksheet you want to create
                string workSheetID = "rId" + worksheetNumber.ToString();
                string worksheetName = dt.TableName;

                WorksheetPart newWorksheetPart = spreadsheet.WorkbookPart.AddNewPart<WorksheetPart>();
                newWorksheetPart.Worksheet = new DocumentFormat.OpenXml.Spreadsheet.Worksheet();
                //AutoFit columns
                Columns columns = AutoSize(dt, dict);
                newWorksheetPart.Worksheet.Append(columns);
                // create sheet data
                newWorksheetPart.Worksheet.AppendChild(new DocumentFormat.OpenXml.Spreadsheet.SheetData());


                // save worksheet
                WriteDataTableToExcelWorksheet(dt, newWorksheetPart, dict, title);
                newWorksheetPart.Worksheet.Save();

                // create the worksheet to workbook relation
                if (worksheetNumber == 1)
                    spreadsheet.WorkbookPart.Workbook.AppendChild(new DocumentFormat.OpenXml.Spreadsheet.Sheets());

                spreadsheet.WorkbookPart.Workbook.GetFirstChild<DocumentFormat.OpenXml.Spreadsheet.Sheets>().AppendChild(new DocumentFormat.OpenXml.Spreadsheet.Sheet()
                {
                    Id = spreadsheet.WorkbookPart.GetIdOfPart(newWorksheetPart),
                    SheetId = (uint)worksheetNumber,
                    Name = dt.TableName
                });

                worksheetNumber++;
            }

            spreadsheet.WorkbookPart.Workbook.Save();
        }
        private static Columns AutoSize(DataTable dt, Dictionary<string, string> dict)
        {
            var maxColWidth = GetMaxCharWidth(dt, dict);
            Columns cols = new Columns();
            double maxWidth = 7;//this is the width of Font
            foreach (var item in maxColWidth)
            {
                double width = Math.Truncate((item.Value * maxWidth + 5) / maxWidth * 256) / 256;
                double pixels = Math.Truncate(((256 * width + Math.Truncate(128 / maxWidth)) / 256) * maxWidth);
                double charwidth = Math.Truncate((pixels - 5) / maxWidth * 100 + 0.5) / 100;
                Column col = new Column() { BestFit = true, Min = (UInt32)(item.Key + 1), Max = (UInt32)(item.Key + 1), CustomWidth = true, Width = (DoubleValue)width };
                cols.Append(col);
            }
            return cols;
        }
        private static Dictionary<int, int> GetMaxCharWidth(DataTable dt, Dictionary<string, string> dict)
        {
            Dictionary<int, int> maxColWidth = new Dictionary<int, int>();
            int temp = 0;
            int max = 100;//Fix max =100 ký tự
            foreach (DataColumn col in dt.Columns)
            {
                maxColWidth.Add(temp, dict[col.ColumnName].Length + 3);
                temp++;
            }
            foreach (DataRow dr in dt.Rows)
            {
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    int cellTextLenghth = dr[i].ToString().Length + 3;
                    if (cellTextLenghth >= max)
                        maxColWidth[i] = max;
                    else
                    {
                        if (cellTextLenghth > maxColWidth[i])
                            maxColWidth[i] = cellTextLenghth;
                    }
                }
            }
            return maxColWidth;
        }
        private static void WriteDataTableToExcelWorksheet(DataTable dt, WorksheetPart worksheetPart, Dictionary<string, string> dict, string title = "")
        {
            var worksheet = worksheetPart.Worksheet;
            var sheetData = worksheet.GetFirstChild<SheetData>();

            string cellValue = "";
            int numberOfColumns = dt.Columns.Count;
            bool[] IsNumericColumn = new bool[numberOfColumns];
            string[] excelColumnNames = new string[numberOfColumns];
            for (int n = 0; n < numberOfColumns; n++)
                excelColumnNames[n] = GetExcelColumnName(n);

            uint rowIndex = 2;
            //Create titile
            var titleRow = new Row { RowIndex = rowIndex };  // add a row at the top of spreadsheet
            sheetData.Append(titleRow);
            AppendTextCell("B" + rowIndex, title, titleRow, (uint)Common.Style_Cell.Title);
            rowIndex += 2;
            //  Create the Header row in our Excel Worksheet
            var headerRow = new Row { RowIndex = rowIndex };  // add a row at the top of spreadsheet
            sheetData.Append(headerRow);
            for (int colInx = 0; colInx < numberOfColumns; colInx++)
            {
                DataColumn col = dt.Columns[colInx];
                AppendTextCell(excelColumnNames[colInx] + rowIndex, dict[col.ColumnName], headerRow, (uint)Common.Style_Cell.Header);
                IsNumericColumn[colInx] = (col.DataType.FullName == "System.Decimal") || (col.DataType.FullName == "System.Int32");
            }

            double cellNumericValue = 0;
            foreach (DataRow dr in dt.Rows)
            {
                // ...create a new row, and append a set of this row's data to it.
                ++rowIndex;
                var newExcelRow = new Row { RowIndex = rowIndex };  // add a row at the top of spreadsheet
                sheetData.Append(newExcelRow);
                for (int colInx = 0; colInx < numberOfColumns; colInx++)
                {
                    cellValue = dr.ItemArray[colInx].ToString();
                    if (dr[colInx].GetType().Name == "DateTime" && dr[colInx].ToString() != "")
                        cellValue = ((DateTime)dr[colInx]).ToString("dd/MM/yyyy");
                    // Create cell with data
                    if (IsNumericColumn[colInx])
                    {
                        //  For numeric cells, make sure our input data IS a number, then write it out to the Excel file.
                        //  If this numeric value is NULL, then don't write anything to the Excel file.
                        cellNumericValue = 0;
                        if (double.TryParse(cellValue, out cellNumericValue))
                        {
                            cellValue = cellNumericValue.ToString();
                            AppendNumericCell(excelColumnNames[colInx] + rowIndex.ToString(), cellValue, newExcelRow);
                        }
                    }
                    else
                    {
                        //  For text cells, just write the input data straight out to the Excel file.
                        AppendTextCell(excelColumnNames[colInx] + rowIndex.ToString(), cellValue, newExcelRow);
                    }
                }
            }
        }

        private static void AppendTextCell(string cellReference, string cellStringValue, Row excelRow, uint _styleIndex = (uint)Common.Style_Cell.Default)
        {
            //  Add a new Excel Cell to our Row 
            Cell cell = new Cell() { CellReference = cellReference, DataType = CellValues.String, StyleIndex = _styleIndex };
            CellValue cellValue = new CellValue();
            cellValue.Text = splitStyle(cellStringValue);
            cell.Append(cellValue);
            excelRow.Append(cell);

        }

        private static void AppendNumericCell(string cellReference, string cellStringValue, Row excelRow, uint _styleIndex = (uint)Common.Style_Cell.Default)
        {
            //  Add a new Excel Cell to our Row 
            Cell cell = new Cell() { CellReference = cellReference, StyleIndex = _styleIndex };
            CellValue cellValue = new CellValue();

            cellValue.Text = cellStringValue;
            cell.Append(cellValue);
            excelRow.Append(cell);
        }
        private static void AppendTextCellNoStyle(string cellReference, string cellStringValue, Row excelRow)
        {
            //  Add a new Excel Cell to our Row 
            Cell cell = new Cell() { CellReference = cellReference, DataType = CellValues.String };
            CellValue cellValue = new CellValue();
            cellValue.Text =splitStyle(cellStringValue);
            cell.Append(cellValue);
            excelRow.Append(cell);

        }

        private static void AppendNumericCellNoStyle(string cellReference, string cellStringValue, Row excelRow)
        {
            //  Add a new Excel Cell to our Row 
            Cell cell = new Cell() { CellReference = cellReference };
            CellValue cellValue = new CellValue();

            cellValue.Text = cellStringValue;
            cell.Append(cellValue);
            excelRow.Append(cell);
        }

        private static string GetExcelColumnName(int columnIndex)
        {
            //  Convert a zero-based column index into an Excel column reference  (A, B, C.. Y, Y, AA, AB, AC... AY, AZ, B1, B2..)
            //
            //  eg  GetExcelColumnName(0) should return "A"
            //      GetExcelColumnName(1) should return "B"
            //      GetExcelColumnName(25) should return "Z"
            //      GetExcelColumnName(26) should return "AA"
            //      GetExcelColumnName(27) should return "AB"
            //      ..etc..
            //
            if (columnIndex < 26)
                return ((char)('A' + columnIndex)).ToString();

            char firstChar = (char)('A' + (columnIndex / 26) - 1);
            char secondChar = (char)('A' + (columnIndex % 26));

            return string.Format("{0}{1}", firstChar, secondChar);
        }

        public static void AppendRefRow(string path, DataTable dt, bool hasHeader, string filename, System.Web.HttpResponseBase Response, Dictionary<string, string> dict,bool isLoadfile=true)
        {
            var file = new FileInfo(path);

            if (file.Exists)
            {
                using (var sprDocument = SpreadsheetDocument.Open(path, true,new OpenSettings(){AutoSave = true}))
                {
                    var workSheetPart = sprDocument.WorkbookPart.WorksheetParts.FirstOrDefault();                    
                    SheetData sheetData = workSheetPart.Worksheet.GetFirstChild<SheetData>();
                    WorkbookStylesPart workbookStylesPart = sprDocument.WorkbookPart.WorkbookStylesPart;                    
                    #region Taoj border
                    Stylesheet stylesheet = workbookStylesPart.Stylesheet;
                    //Border border = new Border(
                    //    new LeftBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin },
                    //    new RightBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin },
                    //    new TopBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin },
                    //    new BottomBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin },
                    //    new DiagonalBorder()
                    //    );                    
                    //stylesheet.Borders.Append(border);                                   
                    UInt32 borderID = stylesheet.Borders.Count-1;                    
                    UInt32 fontID = stylesheet.Fonts.Count-1;
                    //var fonts = stylesheet.Fonts.ToList();
                    //if (fonts.Count > 0) stylesheet.Fonts.Append(fonts[fonts.Count - 1]);
                    CellFormat cellFormat = new CellFormat();
                    if ( borderID>=0) cellFormat.BorderId = borderID;
                    if (fontID >= 0) cellFormat.FontId = fontID;
                    stylesheet.CellFormats.Append(cellFormat);
                    var styleIndex = stylesheet.CellFormats.Count++;
                    workbookStylesPart.Stylesheet.Save();
                    #endregion
                    Row lastRow = sheetData.Elements<Row>().LastOrDefault();
                    #region lastRow !=null
                    if (lastRow != null)
                    {
                        string cellValue = "";
                        int numberOfColumns = dt.Columns.Count;
                        bool[] IsNumericColumn = new bool[numberOfColumns];
                        string[] excelColumnNames = new string[numberOfColumns];
                        for (int n = 0; n < numberOfColumns; n++)
                            excelColumnNames[n] = GetExcelColumnName(n);
                        uint rowIndex = lastRow.RowIndex;
                        if (hasHeader)
                        {
                            double cellNumericValue = 0;
                            foreach (DataRow dr in dt.Rows)
                            {
                                var newExcelRow = new Row { RowIndex = rowIndex };  // add a row at the top of spreadsheet
                                sheetData.Append(newExcelRow);
                                for (int colInx = 0; colInx < numberOfColumns; colInx++)
                                {
                                    cellValue = dr.ItemArray[colInx].ToString();
                                    if (dr[colInx].GetType().Name == "DateTime" && dr[colInx].ToString() != "")
                                        cellValue = ((DateTime)dr[colInx]).ToString("dd/MM/yyyy");
                                    if (IsNumericColumn[colInx])
                                    {
                                        cellNumericValue = 0;
                                        if (double.TryParse(cellValue, out cellNumericValue))
                                        {
                                            cellValue = cellNumericValue.ToString();
                                            //AppendNumericCellNoStyle(excelColumnNames[colInx] + rowIndex.ToString(), cellValue, newExcelRow);
                                            AppendNumericCell(excelColumnNames[colInx] + rowIndex.ToString(), cellValue, newExcelRow,styleIndex);
                                        }
                                    }
                                    else
                                    {
                                        //AppendTextCellNoStyle(excelColumnNames[colInx] + rowIndex.ToString(), cellValue, newExcelRow);
                                        AppendTextCell(excelColumnNames[colInx] + rowIndex.ToString(), cellValue, newExcelRow, styleIndex);
                                    }
                                }
                                rowIndex++;
                            }
                        }
                        else
                        {
                            var headerRow = new Row { RowIndex = rowIndex };
                            sheetData.Append(headerRow);
                            for (int colInx = 0; colInx < numberOfColumns; colInx++)
                            {
                                DataColumn col = dt.Columns[colInx];
                                if (dict.ContainsKey(col.ColumnName))
                                    //AppendTextCellNoStyle(excelColumnNames[colInx] + rowIndex, dict[col.ColumnName], headerRow);
                                    AppendTextCell(excelColumnNames[colInx] + rowIndex, dict[col.ColumnName], headerRow, styleIndex);
                                else
                                    //AppendTextCellNoStyle(excelColumnNames[colInx] + rowIndex, col.ColumnName, headerRow);
                                    AppendTextCell(excelColumnNames[colInx] + rowIndex, col.ColumnName, headerRow, styleIndex);
                                IsNumericColumn[colInx] = (col.DataType.FullName == "System.Decimal") || (col.DataType.FullName == "System.Int32");
                            }
                            double cellNumericValue = 0;
                            foreach (DataRow dr in dt.Rows)
                            {
                                ++rowIndex;
                                var newExcelRow = new Row { RowIndex = rowIndex };  // add a row at the top of spreadsheet
                                sheetData.Append(newExcelRow);
                                for (int colInx = 0; colInx < numberOfColumns; colInx++)
                                {
                                    cellValue = dr.ItemArray[colInx].ToString();
                                    if (dr[colInx].GetType().Name == "DateTime" && dr[colInx].ToString() != "")
                                        cellValue = ((DateTime)dr[colInx]).ToString("dd/MM/yyyy");
                                    if (IsNumericColumn[colInx])
                                    {
                                        cellNumericValue = 0;
                                        if (double.TryParse(cellValue, out cellNumericValue))
                                        {
                                            cellValue = cellNumericValue.ToString();
                                            //AppendNumericCellNoStyle(excelColumnNames[colInx] + rowIndex.ToString(), cellValue, newExcelRow);
                                            AppendNumericCell(excelColumnNames[colInx] + rowIndex.ToString(), cellValue, newExcelRow, styleIndex);
                                        }
                                    }
                                    else
                                    {
                                        //AppendTextCellNoStyle(excelColumnNames[colInx] + rowIndex.ToString(), cellValue, newExcelRow);
                                        AppendTextCell(excelColumnNames[colInx] + rowIndex.ToString(), cellValue, newExcelRow, styleIndex);
                                    }
                                }
                            }
                        }

                    }
                    #endregion
                    #region lastRow =null
                    else
                    {
                        string cellValue = "";
                        int numberOfColumns = dt.Columns.Count;
                        bool[] IsNumericColumn = new bool[numberOfColumns];
                        string[] excelColumnNames = new string[numberOfColumns];
                        for (int n = 0; n < numberOfColumns; n++)
                            excelColumnNames[n] = GetExcelColumnName(n);
                        uint rowIndex = 1;
                        var headerRow = new Row { RowIndex = rowIndex };  // add a row at the top of spreadsheet
                        sheetData.Append(headerRow);
                        for (int colInx = 0; colInx < numberOfColumns; colInx++)
                        {
                            DataColumn col = dt.Columns[colInx];
                            if (dict.ContainsKey(col.ColumnName))
                                AppendTextCellNoStyle(excelColumnNames[colInx] + "1", dict[col.ColumnName], headerRow);
                            else
                                AppendTextCellNoStyle(excelColumnNames[colInx] + "1", col.ColumnName, headerRow);
                            IsNumericColumn[colInx] = (col.DataType.FullName == "System.Decimal") || (col.DataType.FullName == "System.Int32");
                        }
                        double cellNumericValue = 0;
                        foreach (DataRow dr in dt.Rows)
                        {
                            // ...create a new row, and append a set of this row's data to it.
                            ++rowIndex;
                            var newExcelRow = new Row { RowIndex = rowIndex };  // add a row at the top of spreadsheet
                            sheetData.Append(newExcelRow);

                            for (int colInx = 0; colInx < numberOfColumns; colInx++)
                            {
                                cellValue = dr.ItemArray[colInx].ToString();
                                if (dr[colInx].GetType().Name == "DateTime" && dr[colInx].ToString() != "")
                                    cellValue = ((DateTime)dr[colInx]).ToString("dd/MM/yyyy");
                                if (IsNumericColumn[colInx])
                                {
                                    cellNumericValue = 0;
                                    if (double.TryParse(cellValue, out cellNumericValue))
                                    {
                                        cellValue = cellNumericValue.ToString();
                                        //AppendNumericCellNoStyle(excelColumnNames[colInx] + rowIndex.ToString(), cellValue, newExcelRow);
                                        AppendNumericCell(excelColumnNames[colInx] + rowIndex.ToString(), cellValue, newExcelRow, styleIndex);
                                    }
                                }
                                else
                                {
                                    //AppendTextCellNoStyle(excelColumnNames[colInx] + rowIndex.ToString(), cellValue, newExcelRow);
                                    AppendTextCell(excelColumnNames[colInx] + rowIndex.ToString(), cellValue, newExcelRow, styleIndex);
                                }
                            }
                        }
                    }
                    #endregion
                }
                if(isLoadfile)
                {
                    System.IO.MemoryStream stream = new System.IO.MemoryStream();
                    FileStream fs = file.OpenRead();
                    fs.CopyTo(stream);
                    fs.Close();
                    stream.Flush();
                    stream.Position = 0;
                    byte[] data1 = new byte[stream.Length];
                    stream.Read(data1, 0, data1.Length);
                    stream.Close();
                    Response.ClearContent();
                    Response.Clear();
                    Response.Buffer = true;
                    Response.BufferOutput = false;
                    Response.Charset = "";
                    Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);
                    Response.AddHeader("content-disposition", "attachment; filename=" + filename + ".xlsx");
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.BinaryWrite(data1);
                    Response.Flush();
                    Response.End();
                    Response.Close();
                }
                
            }

        }

        public static Stylesheet GennerateStyle()
        {
            return new Stylesheet(
                new Fonts(
                    new Font(
                        new FontSize() { Val = 11 },
                        new Color() { Rgb = new HexBinaryValue() { Value = "003366" } },
                        new FontName() { Val = "Calibri" }),
                    new Font(
                        new Bold(),
                        new FontSize() { Val = 11 },
                        new Color() { Rgb = new HexBinaryValue() { Value = "003366" } },
                        new FontName() { Val = "Calibri" }),
                    new Font(
                        new Bold(),
                        new FontSize() { Val = 13 },
                        new Color() { Rgb = new HexBinaryValue() { Value = "003366" } },
                        new FontName() { Val = "Calibri" }),
                    new Font(
                        new FontSize() { Val = 16 },
                        new Color() { Rgb = new HexBinaryValue() { Value = "003366" } },
                        new FontName() { Val = "Times New Roman" })
                ),
                new Fills(
                    new Fill(
                            new PatternFill() { PatternType = PatternValues.None }),
                    new Fill(
                            new PatternFill() { PatternType = PatternValues.Gray125 }),
                    new Fill(
                            new PatternFill(
                                 new ForegroundColor() { Rgb = new HexBinaryValue() { Value = "FFFFFF00" } }) { PatternType = PatternValues.Solid })
                    ),
                new Borders(
                    new Border(
                        new LeftBorder(),
                        new RightBorder(),
                        new TopBorder(),
                        new BottomBorder(),
                        new DiagonalBorder()),
                    new Border(
                        new LeftBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin },
                        new RightBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin },
                        new TopBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin },
                        new BottomBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin },
                        new DiagonalBorder()
                        )
                    ),
                new CellFormats(
                    new CellFormat() { FontId = 0, FillId = 0, BorderId = 1, ApplyBorder = true },
                    new CellFormat(new Alignment() { Horizontal = HorizontalAlignmentValues.Center }) { FontId = 1, FillId = 0, BorderId = 1, ApplyFont = true },
                    new CellFormat(new Alignment() { Horizontal = HorizontalAlignmentValues.Center }) { FontId = 2, FillId = 0, BorderId = 0, ApplyFont = true },
                    new CellFormat() { FontId = 0, FillId = 1, BorderId = 1, ApplyFill = true },
                    new CellFormat(new Alignment() { WrapText = true }) { FontId = 0, FillId = 0, BorderId = 1, ApplyAlignment = true },
                    new CellFormat() { FontId = 0, FillId = 1, BorderId = 1, ApplyFill = true }
                    )
                );
        }
        
        public static void MergeArrCell(string[] arrColumnName, Worksheet workSheet)
        {
            MergeCells merCells;
            if (workSheet.Elements<MergeCells>().Count() > 0)
                merCells = workSheet.Elements<MergeCells>().First();
            else
            {
                merCells = new MergeCells();
                if (workSheet.Elements<CustomSheetView>().Count() > 0)
                    workSheet.InsertAfter(merCells, workSheet.Elements<CustomSheetView>().First());
                else
                    workSheet.InsertAfter(merCells, workSheet.Elements<SheetData>().First());
            }
            MergeCell merCell = new MergeCell() { Reference = new StringValue(arrColumnName[0] + ":" + arrColumnName[1]) };

            merCells.Append(merCell);
            workSheet.Save();
        }

        public static void dowloadFile(string path, System.Web.HttpResponseBase Response,string filename = "export", bool isDelete = true)
        {            
            var file = new FileInfo(path);
            if (file.Exists)
            {
                System.IO.MemoryStream stream = new System.IO.MemoryStream();
                FileStream fs = file.OpenRead();
                fs.CopyTo(stream);
                fs.Close();
                stream.Flush();
                stream.Position = 0;
                byte[] data1 = new byte[stream.Length];
                stream.Read(data1, 0, data1.Length);
                stream.Close();
                Response.ClearContent();
                Response.Clear();
                Response.Buffer = true;
                Response.BufferOutput = false;
                Response.Charset = "";
                Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);
                Response.AddHeader("content-disposition", "attachment; filename=" + filename + ".xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.BinaryWrite(data1);
                Response.Flush();
                Response.End();
                Response.Close();
                if (isDelete)
                    file.Delete();
            }
        }
        
    }
}