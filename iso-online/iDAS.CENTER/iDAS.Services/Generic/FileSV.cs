using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iDAS.Base;
using iDAS.Core;
using iDAS.DA;
using iDAS.Items;
using System.IO;
using DocumentFormat.OpenXml.Packaging;
using System.Drawing.Imaging;
using System.Xml.Linq;
using OpenXmlPowerTools;
using System.Data;

namespace iDAS.Services
{
    public class FileSV : BaseService
    {
        private FileDA fileDA = new FileDA();
        /// <summary>
        /// Lấy danh sách 
        /// || Author: Thanhnv || CreateDate: 16/06/2015  
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public List<FileItem> GetAll(int page, int pageSize, out int totalCount)
        {
            var File = fileDA.GetQuery()
                        .Select(item => new FileItem()
                        {
                            ID = item.ID,
                            Name = item.Name,
                            Extension = item.Extension,
                            //Path = item.Path,
                            Size = item.Size,
                            Type = item.Type,
                            CreateAt = item.CreateAt,
                        })
                        .OrderByDescending(item => item.CreateAt)
                        .Page(page, pageSize, out totalCount)
                        .ToList();
            return File;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<FileItem> GetByIds(List<Guid> Ids)
        {
            var File = fileDA.GetQuery(i => Ids.Contains(i.ID))
                        .Select(item => new FileItem()
                        {
                            ID = item.ID,
                            Name = item.Name,
                            Extension = item.Extension,
                            Size = item.Size,
                            Type = item.Type,
                            CreateAt = item.CreateAt,
                            IsDelete = false
                        })
                        .ToList();
            return File;
        }
        /// <summary>
        /// Lấy  theo ID
        /// || Author: Thanhnv || CreateDate: 16/06/2015  
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public FileItem GetById(System.Guid Id)
        {
            try
            {
                var File = fileDA.GetQuery(i => i.ID == Id)
                            .Select(item => new FileItem()
                            {
                                ID = item.ID,
                                Name = item.Name,
                                Extension = item.Extension,
                                //Path = item.Path,
                                Size = item.Size,
                                Type = item.Type,
                                Data = item.Data,
                            })
                            .FirstOrDefault();
                return File;
            }
            catch
            {
                throw;
            }
            return null;
        }
        /// <summary>
        /// Cập nhật 
        /// || Author: Thanhnv || CreateDate: 16/06/2015  
        /// </summary>
        /// <param name="item"></param>
        /// <param name="userID"></param>
        public void Update(FileItem item, int userID)
        {
            var file = fileDA.GetById(item.ID);
            file.ID = item.ID;
            file.Name = item.Name;
            file.Extension = item.Extension;
            //file.Path = item.Path;
            file.Size = item.Size;
            file.Type = item.Type;
            file.Data = item.Data;
            file.UpdateAt = DateTime.Now;
            file.UpdateBy = userID;
            fileDA.Save();
        }
        /// <summary>
        /// Thêm mới 
        /// || Author: Thanhnv || CreateDate: 16/06/2015  
        /// </summary>
        /// <param name="item"></param>
        /// <param name="userID"></param>
        public Guid Insert(FileItem item, int userID)
        {
            var file = new Base.File()
            {
                ID = item.ID,
                Name = item.Name,
                Extension = item.Extension,
                //Path = item.Path,
                Size = item.Size,
                Type = item.Type,
                Data = item.Data,
                IsDeleted = false,
                CreateAt = DateTime.Now,
                CreateBy = userID
            };
            fileDA.Insert(file);
            fileDA.Save();
            return file.ID;
        }
        /// <summary>
        /// Xóa 
        /// || Author: Thanhnv || CreateDate: 16/06/2015  
        /// </summary>
        /// <param name="id"></param>
        public bool Delete(System.Guid id)
        {
            var dbo = fileDA.Repository;
            var File = dbo.Files.FirstOrDefault(i => i.ID == id);
            dbo.Files.Remove(File);
            dbo.SaveChanges();
            return true;
        }
        public Guid Insert(System.Web.HttpPostedFileBase file, int userID)
        {
            if (file != null)
            {
                var fileInsert = new Base.File()
                {
                    ID = Guid.NewGuid(),
                    Name = file.FileName,
                    Extension = string.IsNullOrWhiteSpace(file.FileName) ?
                        "" : file.FileName.Substring(file.FileName.IndexOf('.') + 1),
                    Size = file.ContentLength,
                    Type = file.ContentType,
                    Data = iDAS.Utilities.Convert.ToByteArray(file.InputStream),
                    IsDeleted = false,
                    CreateAt = DateTime.Now,
                    CreateBy = userID
                };
                fileDA.Insert(fileInsert);
                fileDA.Save();
                return fileInsert.ID;
            }
            return Guid.Empty;
        }
        public List<Guid> InsertRange(List<System.Web.HttpPostedFileBase> files, int userID)
        {
            var lsFileInsert = new List<Base.File>();
            foreach (var file in files)
            {
                if (file != null)
                    lsFileInsert.Add(new Base.File()
                    {
                        ID = Guid.NewGuid(),
                        Name = file.FileName,
                        Extension = string.IsNullOrWhiteSpace(file.FileName) ?
                            "" : file.FileName.Substring(file.FileName.IndexOf('.') + 1),
                        Size = file.ContentLength,
                        Type = file.ContentType,
                        Data = iDAS.Utilities.Convert.ToByteArray(file.InputStream),
                        IsDeleted = false,
                        CreateAt = DateTime.Now,
                        CreateBy = userID
                    });

            }
            if (lsFileInsert.Count > 0)
            {
                fileDA.InsertRange(lsFileInsert);
                fileDA.Save();
            }
            return lsFileInsert.Select(i => i.ID).ToList();
        }
        public void DeleteRange(List<Guid> Ids)
        {
            var fileDeletes = fileDA.GetQuery(i => Ids.Contains(i.ID)).ToList();
            fileDA.DeleteRange(fileDeletes);
            fileDA.Save();
        }
        public void Delete<TEntity>(int paramID) where TEntity : class
        {
            var dbo = fileDA.Repository;
            var paramName = typeof(TEntity).GetProperties()
                    .Where(i => i.Name.Contains("ID"))
                    .Where(i => !i.Name.Contains("FileID"))
                    .Where(i => i.Name.Length > 2)
                    .Select(i => i.Name).FirstOrDefault();
            var table = dbo.Set<TEntity>().ToList();
            var t = table.Where(i => i.GetType().GetProperty(paramName).GetValue(i).ToString().Equals(paramID.ToString()));
            var attachmentFileIDs = t.Select(i => i.GetType().GetProperty("FileID").GetValue(i)).ToList();
            dbo.Set<TEntity>().RemoveRange(t);
            if (attachmentFileIDs != null && attachmentFileIDs.Count > 0)
            {
                fileDA.DeleteRange(attachmentFileIDs);
            }
            fileDA.Save();
        }
        private void insert<TEntity>(List<Guid> fileIDs, int paramID) where TEntity : class
        {
            var dbo = fileDA.Repository;
            var obj = iDAS.Core.ReflectionHelper.CreateInstance<TEntity>();
            var paramName = typeof(TEntity).GetProperties()
                    .Where(i => i.Name.Contains("ID"))
                    .Where(i => !i.Name.Contains("FileID"))
                    .Where(i => i.Name.Length > 2)
                    .Select(i => i.Name).FirstOrDefault();
            foreach (var fileID in fileIDs)
            {
                dbo.Entry(obj).Property("FileID").CurrentValue = fileID;
                dbo.Entry(obj).Property(paramName).CurrentValue = paramID;
                dbo.Entry(obj).Property("CreateAt").CurrentValue = DateTime.Now;
                dbo.Entry(obj).Property("CreateBy").CurrentValue = User.ID;
                dbo.Set<TEntity>().Add(obj);
                fileDA.Save();
            }
        }
        private void delete<TEntity>(List<Guid> fileIDs, int paramID) where TEntity : class
        {
            var dbo = fileDA.Repository;
            var paramName = typeof(TEntity).GetProperties()
                    .Where(i => i.Name.Contains("ID"))
                    .Where(i => !i.Name.Contains("FileID"))
                    .Where(i => i.Name.Length > 2)
                    .Select(i => i.Name).FirstOrDefault();
            var table = dbo.Set<TEntity>().ToList();
            foreach (var fileID in fileIDs)
            {
                var t = table
                        .Where(i => i.GetType().GetProperty("FileID").GetValue(i).ToString().Contains(fileID.ToString()))
                        .Where(i => i.GetType().GetProperty(paramName).GetValue(i).ToString().Equals(paramID.ToString()))
                        .FirstOrDefault();
                dbo.Set<TEntity>().Remove(t);
            }
        }
        public List<Guid> Upload(AttachmentFileItem files)
        {
            List<Guid> data = new List<Guid>();
            //Insert File
            if (files.FileAttachments.First() != null)
            {
                foreach (var item in files.FileAttachments)
                {
                    var file = new Base.File();
                    file.ID = Guid.NewGuid();
                    file.Name = item.FileName;
                    file.Extension = item.FileName.Split('.').LastOrDefault();
                    file.Size = item.ContentLength;
                    file.Type = item.ContentType;
                    file.Data = iDAS.Utilities.Convert.ToByteArray(item.InputStream);
                    file.IsDeleted = false;
                    file.CreateAt = DateTime.Now;
                    file.CreateBy = User.ID;
                    fileDA.Insert(file);
                    data.Add(file.ID);
                }
            }
            //Remove File
            foreach (var item in files.FileRemoves)
            {
                var file = fileDA.GetById(item);
                fileDA.Delete(file);
            }

            fileDA.Save();
            return data;
        }
        public List<Guid> Upload<TEntity>(AttachmentFileItem files, int paramID) where TEntity : class
        {
            List<Guid> data = new List<Guid>();
            //Insert File
            if (files.FileAttachments.First() != null)
            {
                foreach (var item in files.FileAttachments)
                {
                    var file = new Base.File();
                    file.ID = Guid.NewGuid();
                    file.Name = item.FileName;
                    file.Extension = item.FileName.Split('.').LastOrDefault();
                    file.Size = item.ContentLength;
                    file.Type = item.ContentType;
                    file.Data = iDAS.Utilities.Convert.ToByteArray(item.InputStream);
                    file.IsDeleted = false;
                    file.CreateAt = DateTime.Now;
                    file.CreateBy = User.ID;
                    fileDA.Insert(file);
                    data.Add(file.ID);
                }
                insert<TEntity>(data, paramID);
            }
            //Remove File
            delete<TEntity>(files.FileRemoves, paramID);
            foreach (var item in files.FileRemoves)
            {
                var file = fileDA.GetById(item);
                fileDA.Delete(file);
            }

            fileDA.Save();
            return data;
        }
        //public byte[] GetAvatarFile(int employeeID)
        //{
        //    var bytedata = new HumanEmployeeSV().GetAvatarFile(employeeID);
        //    return bytedata;
        //}
        //public byte[] GetLogoFile(string code)
        //{
        //    var bytedata = new CenterCustomerSV().GetLogoFile(code);
        //    return bytedata;
        //}
        public string ReadFile(string fileID)
        {
            var file = fileDA.GetById(new Guid(fileID));
            var html = ReadFile(file.Data, file.Extension);
            return html;
        }
        public string ReadFile(byte[] fileData, string fileExtension)
        {
            string html = "<div></div>";
            switch (fileExtension)
            {
                case "pdf": html = ConvertPdfToHtml(fileData); break;
                case "xls": html = ConvertExcelToHtml(fileData); break;
                case "xlsx": html = ConvertExcelToHtml(fileData); break;
                case "docx": html = ConvertDocxToHtml(fileData); break;
                case "doc": html = ConvertDocToHtml(fileData); break;
                case "png": html = ConvertImageToHtml(fileData, fileExtension); break;
                case "gif": html = ConvertImageToHtml(fileData, fileExtension); break;
                case "bmp": html = ConvertImageToHtml(fileData, fileExtension); break;
                case "jpg": html = ConvertImageToHtml(fileData, fileExtension); break;
                case "jpeg": html = ConvertImageToHtml(fileData, fileExtension); break;
                case "tiff": html = ConvertImageToHtml(fileData, fileExtension); break;
                case "wmf": html = ConvertImageToHtml(fileData, fileExtension); break;
                case "x-wmf": html = ConvertImageToHtml(fileData, fileExtension); break;
            }
            return html;
        }
        public string ConvertPdfToHtml(byte[] fileData)
        {
            var data = Convert.ToBase64String(fileData);
            var htmlString = string.Format("<iframe src='data:application/pdf;base64,{0}' width=100% height=100%/>", data);
            //var htmlString = string.Format("src='data:application/pdf;base64,{0}'", data);

            return htmlString;
        }
        public string ConvertImageToHtml(byte[] fileData, string extension)
        {
            var data = Convert.ToBase64String(fileData);
            var htmlString = string.Format("<img src='data:image/" + extension + ";base64,{0}'/>", data);
            return htmlString;
        }
        public string ConvertDocxToHtml(byte[] fileData)
        {
            if (fileData == null || fileData.Count() == 0) return string.Empty;
            using (MemoryStream memoryStream = new MemoryStream(fileData))
            {
                using (WordprocessingDocument wDoc = WordprocessingDocument.Open(memoryStream, true))
                {
                    var pageTitle = (string)wDoc.CoreFilePropertiesPart.GetXDocument().Descendants(DC.title).FirstOrDefault();
                    if (pageTitle == null)
                        pageTitle = "";

                    HtmlConverterSettings settings = new HtmlConverterSettings()
                    {
                        PageTitle = pageTitle,
                        FabricateCssClasses = true,
                        CssClassPrefix = "pt-",
                        RestrictToSupportedLanguages = false,
                        RestrictToSupportedNumberingFormats = false,

                        ImageHandler = imageInfo =>
                        {
                            string extension = imageInfo.ContentType.Split('/')[1].ToLower();
                            ImageFormat imageFormat = null;
                            if (extension == "png")
                            {
                                imageFormat = ImageFormat.Png;
                            }
                            else if (extension == "gif")
                                imageFormat = ImageFormat.Gif;
                            else if (extension == "bmp")
                                imageFormat = ImageFormat.Bmp;
                            else if (extension == "jpeg")
                                imageFormat = ImageFormat.Jpeg;
                            else if (extension == "tiff")
                            {
                                imageFormat = ImageFormat.Tiff;
                            }
                            else if (extension == "x-wmf")
                            {
                                extension = "wmf";
                                imageFormat = ImageFormat.Wmf;
                            }

                            if (imageFormat == null)
                                return null;

                            string imageSrc = "";
                            try
                            {
                                MemoryStream imgMemory = new MemoryStream();
                                imageInfo.Bitmap.Save(imgMemory, imageFormat);
                                var data = Convert.ToBase64String(imgMemory.GetBuffer());
                                imageSrc = string.Format("data:image/" + extension + ";base64,{0}", data);
                            }
                            catch (System.Runtime.InteropServices.ExternalException)
                            {
                                return null;
                            }
                            XElement img = new XElement(Xhtml.img,
                                new XAttribute(NoNamespace.src, imageSrc),
                                imageInfo.ImgStyleAttribute,
                                imageInfo.AltText != null ?
                                    new XAttribute(NoNamespace.alt, imageInfo.AltText) : null);
                            return img;
                        }
                    };
                    XElement html = HtmlConverter.ConvertToHtml(wDoc, settings);
                    var htmlString = html.ToString();
                    if (string.IsNullOrEmpty(htmlString))
                    {
                        htmlString = "<div></div>";
                    }
                    return htmlString;
                }
            }
        }
        public string ConvertDocToHtml(byte[] fileData)
        {
            if (fileData == null || fileData.Count() == 0) return string.Empty;
            var html = "";
            Spire.Doc.Document doc;
            using (MemoryStream memoryStream = new MemoryStream(fileData))
            {
                doc = new Spire.Doc.Document(memoryStream);
            }
            doc.HtmlExportOptions.ImageEmbedded = true;
            doc.HtmlExportOptions.CssStyleSheetType = Spire.Doc.CssStyleSheetType.Internal;
            doc.HtmlExportOptions.EPubExportFont = false;
            doc.HtmlExportOptions.HasHeadersFooters = true;
            doc.RemoveEncryption();
            using (MemoryStream memoryStream = new MemoryStream())
            {
                doc.SaveToFile(memoryStream, Spire.Doc.FileFormat.Html);
                html = UnicodeEncoding.UTF8.GetString(memoryStream.GetBuffer());
            }
            html = html.Replace("Evaluation Warning : The document was created with Spire.Doc for .NET.", "");
            html = html.Replace(";font-family:'Wingdings';", ";font-family:'inherit';");
            html = html.Replace(";font-family:'Symbol';", ";font-family:'inherit';");
            return html;
        }
        public string ConvertExcelToHtml(byte[] fileData)
        {
            var html = "";
            Aspose.Cells.Workbook book;
            using (MemoryStream memoryStream = new MemoryStream(fileData))
            {
                book = new Aspose.Cells.Workbook(memoryStream);
            }
            Aspose.Cells.HtmlSaveOptions save = new Aspose.Cells.HtmlSaveOptions(Aspose.Cells.SaveFormat.Html);
            save.ExportImagesAsBase64 = true;
            save.ExportActiveWorksheetOnly = true;
            save.ValidateMergedAreas = true;
            var pages = book.Worksheets.Count;
            for (int i = 0; i < 1; i++)
            {
                book.Worksheets.ActiveSheetIndex = i;
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    book.Save(memoryStream, save);
                    html = html + UnicodeEncoding.UTF8.GetString(memoryStream.GetBuffer());
                }
            }
            return html;
        }
    }
}
