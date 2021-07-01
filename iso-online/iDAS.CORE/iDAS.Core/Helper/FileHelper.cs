using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using excel = Microsoft.Office.Interop.Excel;
using word = Microsoft.Office.Interop.Word;
namespace iDAS.Core
{
    public class FileHelper
    {
        public static void ConvertToPdf(string pathOpen, string pathSave)
        {
            var typeWord = new[] { ".dot", ".dotx", ".doc", ".docx", ".docm", ".dotm" };
            var typeExcel = new[] { ".xls", ".xlt", ".xla", ".xlsx", ".xltx", ".xlsm", ".xltm", ".xlam", ".xlsb" };

            var type = Path.GetExtension(pathOpen).ToLower();
            try
            {
                if (typeWord.Contains(type))
                {
                    WordToPdf(pathOpen, pathSave);
                }
                if (typeExcel.Contains(type))
                {
                    ExcelToPdf(pathOpen, pathSave);
                }
            }
            catch (Exception ex) { throw ex; }

        }
        public static void WordToPdf(string pathOpen, string pathSave)
        {
            word.Application app = new word.Application();
            app.Visible = false;
            try
            {
                word.Document doc = app.Documents.Open(pathOpen);
                doc.ExportAsFixedFormat(pathSave, word.WdExportFormat.wdExportFormatPDF);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                app.Quit(SaveChanges: false);
            }
        }
        public static void ExcelToPdf(string pathOpen, string pathSave)
        {
            excel.Application app = new excel.Application();
            app.Visible = false;
            try
            {
                excel.Workbook workbook = app.Workbooks.Open(pathOpen);
                foreach (excel.Worksheet worksheet in workbook.Worksheets)
                {
                    worksheet.PageSetup.Orientation = excel.XlPageOrientation.xlLandscape;
                    worksheet.PageSetup.Zoom = false;
                    worksheet.PageSetup.FitToPagesTall = false;
                    worksheet.PageSetup.FitToPagesWide = 1;
                }
                workbook.ExportAsFixedFormat(excel.XlFixedFormatType.xlTypePDF, pathSave);
                workbook.Close(false);
                Marshal.ReleaseComObject(workbook);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                app.Quit();
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }
    }
}
