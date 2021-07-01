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
    public class TemplateExportSV
    {
        TemplateExportDA templExportDA = new TemplateExportDA();
        public List<TemplateExportItem> GetAll(int page, int pageSize, out int totalCount)
        {
            var dbo = templExportDA.Repository;
            var lst = templExportDA.GetQuery().Select(i => new TemplateExportItem()
                {
                    ID = i.ID,
                    Name = i.Name,
                    Font = i.Font,
                    FontSize = i.FontSize,                    
                    Type = i.Type,
                    FileID =i.AttachmentFileID,
                    Default = i.Default,
                    FunctionCode = dbo.BusinessFunctions.FirstOrDefault(x => x.Code == i.FunctionCode).Name,
                    ModuleCode = dbo.BusinessModules.FirstOrDefault(xx => xx.Code == dbo.BusinessFunctions.FirstOrDefault(x => x.Code == i.FunctionCode).ModuleCode).Name,
                    FileName = i.AttachmentFileID.HasValue ? i.AttachmentFile.Name : "",
                    
                }).OrderByDescending(item => item.ID)
                        .Page(page, pageSize, out totalCount).ToList();
            return lst;
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

        public int Insert(TemplateExportItem data, int userID)
        {
            var obj = new TemplateExport();
            obj.Name = data.Name;
            obj.Type = data.Type;
            obj.Font = data.Font;
            obj.FunctionCode = data.FunctionCode;
            obj.Default = data.Default;
            obj.CreateAt = DateTime.Now;
            obj.CreateBy = userID;
            if (data.File != null)
            {
                var imgId = new FileSV().Insert(data.File, userID);
                obj.AttachmentFileID = imgId;
            }
            templExportDA.Insert(obj);
            templExportDA.Save();
            return obj.ID;
        }
        public bool Update(TemplateExportItem data, int userID)
        {
            try
            {
                var obj = templExportDA.GetById(data.ID);
                if (obj != null)
                {
                    if (data.File != null)
                    {
                        if (obj.AttachmentFileID == null)
                        {
                            data.File.ID = Guid.NewGuid();
                            new FileSV().Insert(data.File, userID);
                            obj.AttachmentFileID = data.File.ID;
                        }
                        else
                        {
                            data.File.ID = (Guid)obj.AttachmentFileID;
                            FileDA FileDA = new FileDA();
                            var file = FileDA.GetById(data.File.ID);
                            file.ID = data.File.ID;
                            file.Name = data.File.Name;
                            file.Extension = data.File.Extension;
                            //file.Path = item.Path;
                            file.Size = data.File.Size;
                            file.Type = data.File.Type;
                            file.Data = data.File.Data;
                            file.UpdateAt = DateTime.Now;
                            file.UpdateBy = userID;
                            FileDA.Save();
                            obj.AttachmentFileID = file.ID;
                        }
                    }
                    obj.Name = data.Name;
                    obj.Font = data.Font;
                    obj.Type = data.Type;
                    obj.FontSize = data.FontSize;
                    obj.FunctionCode = data.FunctionCode;
                    obj.Default = data.Default;
                    obj.UpdateAt = DateTime.Now;
                    obj.UpdateBy = userID;
                    templExportDA.Save();
                    return true;
                }
                else
                    return false;
            }catch
            {
                return false;
            }
        }
        public bool Delete(int ID)
        {
            var dbo = templExportDA.Repository;
            var obj = dbo.TemplateExports.FirstOrDefault(i => i.ID == ID);
            dbo.TemplateExports.Remove(obj);
            if (obj.AttachmentFileID.HasValue) dbo.AttachmentFiles.Remove(obj.AttachmentFile);
            dbo.SaveChanges();
            return true;
        }

        public TemplateExportItem GetByID(int ID)
        {
            var dbo = templExportDA.Repository;
            var obj = templExportDA.GetById(ID);
            if (obj != null)
            {
                return new TemplateExportItem()
                {
                    ID = obj.ID,
                    Name = obj.Name,
                    FontSize = obj.FontSize,
                    Font = obj.Font,
                    Type =obj.Type,
                    Default = obj.Default,
                    FunctionCode = obj.FunctionCode,
                    ModuleCode = dbo.BusinessFunctions.FirstOrDefault(x=>x.Code==obj.FunctionCode).ModuleCode,
                    FileID = obj.AttachmentFileID,
                    File = obj.AttachmentFileID.HasValue ? new FileItem()
                    {
                        ID = obj.AttachmentFile.ID,
                        Name = obj.AttachmentFile.Name,
                        Data = obj.AttachmentFile.Data,
                    } : null,
                    FileName = obj.AttachmentFileID.HasValue? obj.AttachmentFile.Name:"" 
                };
                
            }
            else
                return null;
        }

        public TemplateExportItem GetFileByFunctionCode(string functionCode,int type=1)
        {
            var obj = templExportDA.GetQuery(x => x.FunctionCode == functionCode && x.Type == type).FirstOrDefault();

            if(obj!=null&& obj.AttachmentFileID.HasValue)
            {
                return new TemplateExportItem()
                {
                    ID = obj.ID,
                    Name = obj.Name,
                    FontSize = obj.FontSize,
                    Font = obj.Font,
                    Type = obj.Type,
                    Default = obj.Default,
                    FunctionCode = obj.FunctionCode,
                    FileID = obj.AttachmentFileID,
                    File = obj.AttachmentFileID.HasValue ? new FileItem()
                    {
                        ID = obj.AttachmentFile.ID,
                        Name = obj.AttachmentFile.Name,
                        Data = obj.AttachmentFile.Data,
                    } : null,
                    FileName = obj.AttachmentFileID.HasValue ? obj.AttachmentFile.Name : ""
                };
            }
            else
                return null;
        }
    }
}
