using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using word = Microsoft.Office.Interop.Word;

namespace iDAS.Core
{
    //public enum FileTemplateType
    //{
    //    Table,
    //    Column,
    //    Field,
    //}
    public class FieldTemp
    {
        public bool IsCheck { get; set; }
        public bool IsImage { get; set; }
        public int MapIndex { get; set; }
        public string MapTemp { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }

    }

    public class TableTemp : FieldTemp
    {
        public TableTemp()
        {
            Columns = new List<FieldTemp>();
            Rows = new List<List<FieldTemp>>();
        }
        public List<FieldTemp> Columns { get; set; }
        public List<List<FieldTemp>> Rows { get; set; }
    }
    public class FileTemplate
    {
        public FileTemplate()
        {
            Tables = new List<TableTemp>();
            Fields = new List<FieldTemp>();
        }
        public List<TableTemp> Tables { get; set; }
        public List<FieldTemp> Fields { get; set; }
    }

    public class Template
    {
        public void Export(FileTemplate temp, string pathOpen, string pathSave)
        {
            word.Application app = new word.Application();
            app.Visible = false;
            try
            {
                word.Document doc = app.Documents.Open(pathOpen);
                if (temp.Fields == null) temp.Fields = new List<FieldTemp>();
                if (temp.Tables == null) temp.Tables = new List<TableTemp>();

                //case field
                setFields(app, temp);

                //case table
                setTables(app, temp);

                var fileFormat = word.WdSaveFormat.wdFormatDocumentDefault;

                var extend = pathSave.Split('.').LastOrDefault();
                switch (extend.ToLower())
                {
                    case "doc": fileFormat = word.WdSaveFormat.wdFormatDocument97; break;
                    case "pdf": fileFormat = word.WdSaveFormat.wdFormatPDF; break;
                    case "docx": fileFormat = word.WdSaveFormat.wdFormatDocument; break;
                    //case "docx": fileFormat = word.WdSaveFormat.wdFormatDocumentDefault; break;
                    case "xml": fileFormat = word.WdSaveFormat.wdFormatXML; break;
                }
                doc.SaveAs2(pathSave, FileFormat: fileFormat);
            }
            catch (Exception ex) { throw ex; }
            finally { app.Quit(SaveChanges: false); }
        }


        /// <summary>
        /// Get report template from file design for report
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public FileTemplate GetTemplate(string path)
        {
            Application app = new word.Application();
            app.Visible = false;
            try
            {
                Document doc = app.Documents.Open(path);
                FileTemplate temp = new FileTemplate();

                //case table
                temp.Tables.AddRange(getTables(app));

                //case field
                temp.Fields.AddRange(getFields(app));

                return temp;
            }
            catch (Exception ex) { throw ex; }
            finally { app.Quit(SaveChanges: false); }
        }

        //private List<TableTemp> getTables(word.Application app)
        //{
        //    List<TableTemp> result = new List<TableTemp>();
        //    var doc = app.ActiveDocument;
        //    var tNames = Regex.Matches(doc.Content.Text, @"\{(.*?)\}").Cast<Match>().Select(i => i.Value);
        //    foreach (var tName in tNames)
        //    {
        //        for (var i = 1; i < doc.Tables.Count; i++)
        //        {
        //            var tText = doc.Tables[i].Range.Text;
        //            if (tText.Contains(tName))
        //            {
        //                TableTemp tab = new TableTemp()
        //                {
        //                    MapIndex = i,
        //                    MapTemp = tName,
        //                    Name = tName.Replace("{", string.Empty).Replace("}", string.Empty).Replace("!", string.Empty).Trim(),
        //                };
        //                word.Find find = app.Selection.Find;
        //                find.Format = false;
        //                find.Text = tName;
        //                find.Execute();
        //                int rowIndex = app.Selection.Range.Cells[1].RowIndex;
        //                foreach (word.Cell c in doc.Tables[i].Rows[rowIndex + 1].Cells)
        //                {
        //                    FieldTemp col = new FieldTemp()
        //                    {
        //                        MapIndex = c.ColumnIndex,
        //                        MapTemp = c.Range.Text.Trim('\a', '\r', 'f', '\n', '\t'),
        //                        Name = c.Range.Text.Trim('\a', '\r', 'f', '\n', '\t'),
        //                    };
        //                    tab.Columns.Add(col);
        //                }

        //                app.Selection.Collapse();
        //                if (tab.Columns.Count > 0) result.Add(tab);
        //            }
        //        }
        //    }

        //    return result;
        //}
        //private List<FieldTemp> getFields(word.Application app)
        //{
        //    List<FieldTemp> result = new List<FieldTemp>();
        //    var doc = app.ActiveDocument;
        //    var index = 0;
        //    foreach (Match field in Regex.Matches(doc.Content.Text, @"\<(.*?)\>|\[(.*?)\]"))
        //    {
        //        FieldTemp fieldRp = new FieldTemp()
        //        {
        //            MapIndex = index,
        //            MapTemp = field.Groups[0].Value,
        //            Name = (field.Groups[1].Value + field.Groups[2].Value)
        //                    .Replace("[", string.Empty)
        //                    .Replace("]", string.Empty)
        //                    .Replace(":", string.Empty)
        //                    .Trim(),
        //        };
        //        index++;
        //        result.Add(fieldRp);
        //    }
        //    return result;
        //}

        private List<TableTemp> getTables(Application app)
        {
            List<TableTemp> list = new List<TableTemp>();
            var doc = app.ActiveDocument;
            foreach (string str in from i in Regex.Matches(doc.Content.Text, @"\{(.*?)\}").Cast<Match>() select i.Value)
            {
                for (int j = 1; j <= doc.Tables.Count; j++)
                {
                    if (doc.Tables[j].Range.Text.Contains(str))
                    {
                        TableTemp item = new TableTemp
                        {
                            MapIndex = j,
                            MapTemp = str,
                            Name = str.Replace("{", string.Empty).Replace("}", string.Empty).Replace("!", string.Empty).Trim()
                        };
                        Find find = app.Selection.Find;
                        find.Format = false;
                        find.Text = str;
                        find.Execute();
                        int rowIndex = app.Selection.Range.Cells[1].RowIndex;
                        foreach (Cell cell in doc.Tables[j].Rows[rowIndex + 1].Cells)
                        {
                            FieldTemp temp2 = new FieldTemp
                            {
                                MapIndex = cell.ColumnIndex,
                                MapTemp = cell.Range.Text.Trim(new char[] { '\a', '\r', 'f', '\n', '\t' }),
                                Name = cell.Range.Text.Trim(new char[] { '\a', '\r', 'f', '\n', '\t' })
                            };
                            item.Columns.Add(temp2);
                        }
                        app.Selection.Collapse();
                        if (item.Columns.Count > 0)
                        {
                            list.Add(item);
                        }
                    }
                }
            }
            return list;
        }
        private List<FieldTemp> getFields(Application app)
        {
            List<FieldTemp> list = new List<FieldTemp>();
            Document activeDocument = app.ActiveDocument;
            int num = 0;
            foreach (Match match in Regex.Matches(activeDocument.Content.Text, @"\<(.*?)\>|\[(.*?)\]"))
            {
                string str = match.Groups[0].Value;
                FieldTemp item = new FieldTemp
                {
                    MapIndex = num,
                    MapTemp = str,
                    Name = (match.Groups[1].Value + match.Groups[2].Value).Replace("[]", string.Empty).Replace("img", string.Empty).Replace("&", string.Empty).Replace(":", string.Empty).Trim(),
                    IsCheck = str.Contains("[]"),
                    IsImage = str.Contains("img")
                };
                num++;
                list.Add(item);
            }
            return list;
        }

        //private void setTables(word.Application app, FileTemplate temp)
        //{
        //    var doc = app.ActiveDocument;
        //    foreach (var tab in temp.Tables)
        //    {
        //        if (tab.MapIndex == 0) continue;
        //        word.Find find = app.Selection.Find;
        //        find.Format = false;
        //        find.Text = tab.MapTemp;
        //        find.Replacement.Text = tab.Name;
        //        find.Execute(Replace: word.WdReplace.wdReplaceOne);
        //        int rowIndex = app.Selection.Range.Cells[1].RowIndex;
        //        int row = rowIndex + 2;
        //        var table = doc.Tables[tab.MapIndex];

        //        //case column
        //        foreach (var r in tab.Rows)
        //        {
        //            table.AllowAutoFit = false;
        //            table.Rows.Add(table.Rows[row]);

        //            foreach (var c in tab.Columns)
        //            {
        //                try
        //                {
        //                    if (c.MapIndex == 0) continue;
        //                    word.Cell cell = table.Cell(row, c.MapIndex);
        //                    var value = r.Where(i => i.MapIndex == c.MapIndex).FirstOrDefault().Value;
        //                    if (!string.IsNullOrEmpty(value) && value.Length > 1)
        //                    {
        //                        var result = false;
        //                        var check = bool.TryParse(value, out result);
        //                        if (check)
        //                        {
        //                            value = result ? "☑" : "☐";
        //                        }
        //                    }
        //                    cell.Range.Text = value ?? string.Empty;
        //                }
        //                catch { continue; }
        //            }
        //            row++;
        //        }

        //        app.Selection.Collapse();
        //        table.Rows[row].Delete();
        //        if (tab.MapTemp.Contains("!")) table.Rows[rowIndex].Delete();
        //    }
        //}
        //private void setFields(word.Application app, FileTemplate temp)
        //{
        //    foreach (var fieldInfo in temp.Fields)
        //    {
        //        if (fieldInfo.MapTemp == null) continue;
        //        var value = fieldInfo.Value;
        //        var text = fieldInfo.MapTemp;
        //        if (text.Contains("<") && text.Contains(">"))
        //        {
        //            var str = text.Replace("<", string.Empty).Replace(">", string.Empty).Trim();
        //            if (str.Contains("[]"))
        //            {
        //                var result = false;
        //                bool.TryParse(value, out result);
        //                if (result)
        //                    value = str.Replace("[]", "☑");
        //                else
        //                    value = str.Replace("[]", "☐");
        //            }
        //            else
        //            {
        //                value = str + " " + value;
        //            }
        //        }
        //        word.Find find = app.Selection.Find;
        //        find.Format = false;
        //        find.Text = text;
        //        find.Replacement.Text = value ?? string.Empty;
        //        find.Execute(Replace: word.WdReplace.wdReplaceAll);
        //    }
        //}

        private void setFields(Application app, FileTemplate temp)
        {
            Find find = app.Selection.Find;
            foreach (FieldTemp item in temp.Fields)
            {
                if (item.MapTemp == null)
                {
                    continue;
                }
                string str = item.Value;
                string mapTemp = item.MapTemp;
                if (mapTemp.Contains("<") && mapTemp.Contains(">"))
                {
                    string input = mapTemp.Replace("<", string.Empty).Replace(">", string.Empty).Trim();
                    if (input.Contains("[]"))
                    {
                        bool result = false;
                        bool.TryParse(str, out result);
                        if (result)
                        {
                            input = input.Replace("[]", "☑");
                        }
                        else
                        {
                            input = input.Replace("[]", "☐");
                        }
                        foreach (Match match in Regex.Matches(input, @"\&(.*?)\&"))
                        {
                            input = input.Replace(match.Groups[0].Value, string.Empty);
                        }
                        str = input;
                    }
                    else
                    {
                        str = input + " " + str;
                    }
                }
                find.Format = false;
                find.Wrap = word.WdFindWrap.wdFindContinue;
                find.Text = mapTemp;
                find.Execute();
                if (find.Found)
                {
                    foreach (Match match in Regex.Matches(mapTemp, @"\((.*?)\)"))
                    {
                        if (match.Groups[0].Value.Contains("img"))
                        {
                            string[] source = match.Groups[1].Value.Replace("img", string.Empty).Trim().Split(new char[] { 'x' });
                            float num = 28.34646f;
                            if (File.Exists(str))
                            {
                                InlineShape shape = app.Selection.InlineShapes.AddPicture(str);
                                if (source.Count<string>() == 2)
                                {
                                    shape.Width = float.Parse(source[0]) * num;
                                    shape.Height = float.Parse(source[1]) * num;
                                }
                            }
                            str = string.Empty;
                            break;
                        }
                    }
                    app.Selection.Text = str ?? string.Empty;
                    app.Selection.Collapse();
                }
            }
        }
        private void setTables(Application app, FileTemplate temp)
        {
            Document doc = app.ActiveDocument;
            Find find = app.Selection.Find;
            foreach (TableTemp temp2 in temp.Tables)
            {
                if (temp2.MapIndex != 0)
                {
                    find.Format = false;
                    find.Text = temp2.MapTemp;
                    find.Replacement.Text = temp2.Name;
                    object wdReplaceOne = WdReplace.wdReplaceOne;
                    find.Execute();
                    int rowIndex = app.Selection.Range.Cells[1].RowIndex;
                    int row = rowIndex + 2;
                    Table table = doc.Tables[temp2.MapIndex];
                    foreach (List<FieldTemp> list in temp2.Rows)
                    {
                        table.AllowAutoFit = false;
                        object beforeRow = table.Rows[row];
                        table.Rows.Add(ref beforeRow);
                        using (List<FieldTemp>.Enumerator enumerator3 = temp2.Columns.GetEnumerator())
                        {
                            while (enumerator3.MoveNext())
                            {
                                Func<FieldTemp, bool> predicate = null;
                                FieldTemp c = enumerator3.Current;
                                try
                                {
                                    if (c.MapIndex != 0)
                                    {
                                        Cell cell = table.Cell(row, c.MapIndex);
                                        if (predicate == null)
                                        {
                                            predicate = i => i.MapIndex == c.MapIndex;
                                        }
                                        string str = list.Where<FieldTemp>(predicate).FirstOrDefault<FieldTemp>().Value;
                                        if (!string.IsNullOrEmpty(str) && (str.Length > 1))
                                        {
                                            bool result = false;
                                            if (bool.TryParse(str, out result))
                                            {
                                                str = result ? "☑" : "☐";
                                            }
                                        }
                                        cell.Range.Text = str ?? string.Empty;
                                    }
                                    continue;
                                }
                                catch
                                {
                                    continue;
                                }
                            }
                        }
                        row++;
                    }
                    app.Selection.Collapse();
                    table.Rows[row].Delete();
                    if (temp2.MapTemp.Contains("!"))
                    {
                        table.Rows[rowIndex].Delete();
                    }
                }
            }
        }






    }
}
