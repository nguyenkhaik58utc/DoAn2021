using ISO.API.Models.CertificateType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISO.API.Service.Interface
{
        /// <summary>
        /// Service danh mục loại chứng chỉ
        /// </summary>
        public interface ICertificateTypeService
        {
        /// <summary>
        /// Lấy chi tiết theo id
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        CertificateTypeDTO GetByID(int ID);

            /// <summary>
            /// Lấy toàn bộ danh sách
            /// </summary>
            /// <returns></returns>
            List<CertificateTypeDTO> GetAll();

            /// <summary>
            /// Thêm mới  
            /// </summary>
            /// <param name="entity"></param>
            /// <returns></returns>
            int Insert(CertificateTypeDTO entity);

            /// <summary>
            /// cập nhật 
            /// </summary>
            /// <param name="entity"></param>
            /// <returns></returns>
            int Update(CertificateTypeDTO entity);

            /// <summary>
            /// Xóa
            /// </summary>
            /// <param name="ID"></param>
            /// <returns></returns>
            int Delete(int ID);

        }
    }
