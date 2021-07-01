using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Data;
using System.Reflection;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
namespace iDAS.Utilities
{
    public class ExportWord
    {
        public static List<string> readListField(string path)
        {
            List<string> lstfield = new List<string>();
            using (WordprocessingDocument document = WordprocessingDocument.Open(path, false))
            {
                var maintPart = document.MainDocumentPart;
                string docText = maintPart.Document.Body.InnerText;
                foreach (var pr in maintPart.Document.Body.Elements<Paragraph>())
                {
                    Regex reg = new Regex("<tag>(.*)</tag>");
                    Match match = reg.Match(pr.InnerText);
                    //if (!lstfield.Contains(match.Value))
                    //    lstfield.Add(match.Value);
                    if (!lstfield.Contains(match.Groups[1].ToString()))
                        lstfield.Add(match.Groups[1].ToString());
                }
            }
            return lstfield;
        }

        public static void createNewFile(string path, Dictionary<string, string> dictValue)
        {
            try
            {
                using (WordprocessingDocument wordDoc = WordprocessingDocument.Create(path, WordprocessingDocumentType.Document))
                {
                    MainDocumentPart mainPart = wordDoc.AddMainDocumentPart();
                    mainPart.Document = new Document();
                    Body body = mainPart.Document.AppendChild(new Body());
                    StyleDefinitionsPart stylePart = mainPart.StyleDefinitionsPart;
                    if (stylePart == null)
                        stylePart = AddStylePartToPackage(wordDoc);
                    AddNewStyle(stylePart, "Title", "Title");
                    Paragraph pTitle = CreateParagraph("This is title", "Title");
                    body.Append(pTitle);
                    mainPart.Document.Save();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void editFile(string path)
        {

        }
        public static Paragraph CreateParagraph(string _content, string _style)
        {
            Paragraph p = new Paragraph();
            Text contentText = new Text(_content);
            Run run = new Run();
            run.Append(contentText);
            run.Append(new RunProperties(new RunFonts() { Ascii = "Arial" }));
            ParagraphProperties pPr = new ParagraphProperties();
            pPr.ParagraphStyleId = new ParagraphStyleId() { Val = _style };
            p.Append(pPr);
            p.Append(run);
            return p;
        }

        public static void AddNewStyle(StyleDefinitionsPart styleDefPart, string styleID, string styleName)
        {
            Styles styles = styleDefPart.Styles;
            Style style = new Style()
            {
                Type = StyleValues.Paragraph,
                StyleId = styleID,
                CustomStyle = true
            };
            StyleName styleName1 = new StyleName() { Val = styleName };
            BasedOn baseOn1 = new BasedOn { Val = "Normal" };
            NextParagraphStyle nextPragrapstyle1 = new NextParagraphStyle() { Val = "Normal" };

            style.Append(styleName1);
            style.Append(baseOn1);
            style.Append(nextPragrapstyle1);
            StyleRunProperties styleRunPropertype1 = new StyleRunProperties();
            Bold bold1 = new Bold();
            Color color1 = new Color() { ThemeColor = ThemeColorValues.Accent2 };
            RunFonts font1 = new RunFonts() { Ascii = "Times New Roman" };
            Italic italic1 = new Italic();
            FontSize fontSize1 = new FontSize() { Val = "24" };

            //styleRunPropertype1.Append(bold1);
            //styleRunPropertype1.Append(color1);
            styleRunPropertype1.Append(font1);
            styleRunPropertype1.Append(fontSize1);
            //styleRunPropertype1.Append(italic1);

            styles.Append(styleRunPropertype1);
            styles.Append(style);
        }
        public static void ApplyStyleToParagraph(WordprocessingDocument doc, string styleID, string styleName, Paragraph p)
        {
            if (p.Elements<ParagraphProperties>().Count() == 0)
                p.PrependChild<ParagraphProperties>(new ParagraphProperties());
            ParagraphProperties pPr = p.Elements<ParagraphProperties>().First();
            StyleDefinitionsPart part = doc.MainDocumentPart.StyleDefinitionsPart;
            if (part == null)
            {
                part = AddStylePartToPackage(doc);
                AddNewStyle(part, styleID, styleName);
            }
            else
            {
                if (hasStyleID(doc, styleID) != true)
                {
                    string styleIDfrName = GetStyleIDbyName(doc, styleName);
                    if (styleName == null)
                        AddNewStyle(part, styleID, styleName);
                    else
                        styleID = styleIDfrName;
                }
            }
            pPr.ParagraphStyleId = new ParagraphStyleId() { Val = styleID };
        }
        public static StyleDefinitionsPart AddStylePartToPackage(WordprocessingDocument doc)
        {
            StyleDefinitionsPart part;
            part = doc.MainDocumentPart.AddNewPart<StyleDefinitionsPart>();
            Styles root = new Styles();
            root.Save(part);
            return part;
        }
        private static string GetStyleIDbyName(WordprocessingDocument doc, string styleName)
        {
            StyleDefinitionsPart stylePart = doc.MainDocumentPart.StyleDefinitionsPart;
            string styleID = stylePart.Styles.Descendants<StyleName>().Where(s => s.Val.Value.Equals(styleName) && (((Style)s.Parent).Type == StyleValues.Paragraph)).Select(n => ((Style)n.Parent).StyleId).FirstOrDefault();
            return styleID;
        }
        private static bool hasStyleID(WordprocessingDocument doc, string styleID)
        {
            Styles s = doc.MainDocumentPart.StyleDefinitionsPart.Styles;
            if (s.Elements<Style>().Count() == 0)
                return false;
            Style style = s.Elements<Style>().Where(st => st.StyleId == styleID && st.Type == StyleValues.Paragraph).FirstOrDefault();
            if (style == null)
                return false;
            return true;
        }

        public static Dictionary<int, string> GetListBookmark(string path)
        {
            Dictionary<int, string> dict = new Dictionary<int, string>();
            using (WordprocessingDocument document = WordprocessingDocument.Open(path, true))
            {
                var maintPart = document.MainDocumentPart;
                int i = 0;
                foreach (BookmarkStart bmarkStart in maintPart.RootElement.Descendants<BookmarkStart>())
                {
                    i++;
                    Run bookmaxText = bmarkStart.NextSibling<Run>();
                    dict.Add(i, bmarkStart.Name);
                }
            }
            return dict;
        }
        public static void dowloadFile(string path, System.Web.HttpResponseBase Response, string filename = "export", bool isDelete = true)
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
                Response.AddHeader("content-disposition", "attachment; filename=" + filename + ".docx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.BinaryWrite(data1);
                Response.Flush();
                Response.End();
                Response.Close();
                if (isDelete)
                    file.Delete();
            }
        }
        public static Dictionary<string, string> ItemToDictionary<T>(T obj)
        {
            try
            {
                Dictionary<string, string> dict = new Dictionary<string, string>();
                foreach (PropertyInfo info in typeof(T).GetProperties())
                {
                    if (info.GetValue(obj, null) != null)
                    {
                        dict.Add(info.Name, info.GetValue(obj, null).ToString());
                    }

                }
                return dict;
            }
            catch (Exception ex)
            {
                return null;
            }

        }
    }
}
