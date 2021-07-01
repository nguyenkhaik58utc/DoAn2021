using ISO.API.Models.ContractType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace ISO.API.Service.Interface
{
    /// <summary>
    /// Service danh mục loại hợp đồng lao động
    /// </summary>
    public interface IContractTypeService
    {
        /// <summary>
        /// Lấy chi tiết theo id
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        ContractTypeDTO GetByID(int ID);

        /// <summary>
        /// Lấy toàn bộ danh sách
        /// </summary>
        /// <returns></returns>
        List<ContractTypeDTO> GetAll();

        /// <summary>
        /// Thêm mới  
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        int Insert(ContractTypeDTO entity);

        /// <summary>
        /// cập nhật 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        int Update(ContractTypeDTO entity);

        /// <summary>
        /// Xóa
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        int Delete(int ID);
    }
}
