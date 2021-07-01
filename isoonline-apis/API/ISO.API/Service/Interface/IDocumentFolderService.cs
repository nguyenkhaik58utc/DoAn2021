using ISO.API.Models.DocumentFolder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISO.API.Service.Interface
{
    public interface IDocumentFolderService
    {
        /// <summary>
        /// Lấy chi tiết theo id
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        DocumentFolderDTO GetByID(int ID);

        /// <summary>
        /// Lấy chi tiết theo Name
        /// </summary>
        /// <param name="Name"></param>
        /// <returns></returns>
        DocumentFolderDTO GetByName(string Name,bool DocumentOut, int ParentID);

        /// <summary>
        /// Lấy toàn bộ danh sách
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        List<DocumentFolderDTO> GetAll(int DocumentOut);

        /// <summary>
        /// Thêm mới 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        int Insert(DocumentFolderDTO entity);

        /// <summary>
        /// cập nhật 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        int Update(DocumentFolderDTO entity);

        /// <summary>
        /// Xóa
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        int Delete(int ID);
    }
}
