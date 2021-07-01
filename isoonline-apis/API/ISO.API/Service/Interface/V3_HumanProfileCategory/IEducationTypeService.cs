using ISO.API.Models.EducationType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISO.API.Service.Interface
{
    /// <summary>
    /// Service danh mục Hình thức đào tạo
    /// </summary>
    public interface IEducationTypeService
    {
        /// <summary>
        /// Lấy chi tiết theo id
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        EducationTypeDTO GetByID(int ID);

        /// <summary>
        /// Lấy toàn bộ danh sách
        /// </summary>
        /// <returns></returns>
        List<EducationTypeDTO> GetAll();

        /// <summary>
        /// Thêm mới  
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        int Insert(EducationTypeDTO entity);

        /// <summary>
        /// cập nhật 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        int Update(EducationTypeDTO entity);

        /// <summary>
        /// Xóa
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        int Delete(int ID);

    }
}
