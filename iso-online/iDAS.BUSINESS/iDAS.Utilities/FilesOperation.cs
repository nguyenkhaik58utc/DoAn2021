using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml;
using DocumentFormat.OpenXml;

namespace iDAS.Utilities
{

    //Class xử lý file     
    //Create: 13/02/2015 
    public class FileOperations
    {
        public static void DeleteFiles(string fileNames, string filePath)
        {
            string[] arrName = fileNames.Split(',');
            DirectoryInfo d = new DirectoryInfo(filePath);
            foreach (var file in d.GetFiles())
            {
                foreach (string name in arrName)
                {
                    if (name == file.Name)
                    {
                        file.Delete();
                    }
                }
            }
        }

        public static bool  DeleteFile(string fileName, string filePath)
        {
            try
            {
                if (CheckFileExist( filePath))
                {

                    File.Delete(filePath);
                    return true;
                }
                return false;                
            }
            catch (Exception)
            {
                throw;               
            }
        }

        public static bool CheckFileNameExist(string fileName, string filePath)
        {
            bool vcheck = false;
            DirectoryInfo d = new DirectoryInfo(filePath);
            foreach (var file in d.GetFiles())
            {
                if (file.Name == fileName)
                {
                    vcheck = true;
                }
            }
            return vcheck;
        }

        public static bool CheckFileExist(string filePath)
        {
            bool chkFile = File.Exists(filePath);
            return chkFile;
        }

        public static void MoveFiles(string fileNames, string fileSourcePath, string fileDestinationPath)
        {
            string[] arrName = fileNames.Split(',');
            foreach (string name in arrName)
            {
                string sourceFile = fileSourcePath + name;
                string destinationFile = fileDestinationPath + name;
                System.IO.File.Move(sourceFile, destinationFile);
            }
        }

        public static void CheckAndCreateFolder(string pathName)
        {
            if (!Directory.Exists(pathName)) Directory.CreateDirectory(pathName);
        }
        public static XmlDocument GetEntityToXML<T>(object obj)
        {
            StringWriter sWriter = new StringWriter();
            XmlDocument xmlDoc = new XmlDocument();
            XmlTextWriter xmlWriter = new XmlTextWriter(sWriter);
            //DataContractSerializer ser = new DataContractSerializer();
            DataContractSerializer serializer = new DataContractSerializer(typeof(T));
            MemoryStream mStream = new MemoryStream();
            serializer.WriteObject(mStream, obj);
            string xmlResult = Encoding.UTF8.GetString(mStream.GetBuffer());
            xmlDoc.LoadXml(xmlResult);
            return xmlDoc;
        }

        
    }
}
