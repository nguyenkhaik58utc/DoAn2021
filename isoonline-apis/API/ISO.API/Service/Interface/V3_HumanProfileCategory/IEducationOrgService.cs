using ISO.API.Models.EducationOrg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISO.API.Service.Interface
{
  /// <summary>
  /// Service danh mục nơi đào tạo
  /// </summary>
    public interface IEducationOrgService
    {
        /// <summary>
        /// Lấy chi tiết theo id
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        EducationOrgDTO GetByID(int ID);

        /// <summary>
        /// Lấy toàn bộ danh sách
        /// </summary>
        /// <returns></returns>
        List<EducationOrgDTO> GetAll();

        /// <summary>
        /// Thêm mới  
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        int Insert(EducationOrgDTO entity);

        /// <summary>
        /// cập nhật 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        int Update(EducationOrgDTO entity);

        /// <summary>
        /// Xóa
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        int Delete(int ID);

    }
}
