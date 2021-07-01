using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iDAS.DA;
using iDAS.Base;
using iDAS.Core;

namespace iDAS.Services
{
    public class CustomerContractAttachmentFileSV
    {
        CustomerContractAttachmentFileDA CustomerContractAttachmentFileDA = new CustomerContractAttachmentFileDA();
        public void InsertRange(List<Guid> IDs, int contractId, int userID)
        {
            var lstCustomerContractAttachment = new List<CustomerContractAttachmentFile>();
            var datetimeNow = DateTime.Now;
            if (IDs.Count > 0)
            {
                foreach (var id in IDs)
                {
                    lstCustomerContractAttachment.Add(
                            new CustomerContractAttachmentFile()
                            {
                                AttachmentFileID = id,
                                CustomerContractID = contractId,
                                CreateAt = datetimeNow,
                                CreateBy = userID,
                            }
                        );
                }
                CustomerContractAttachmentFileDA.InsertRange(lstCustomerContractAttachment);
                CustomerContractAttachmentFileDA.Save();
            }
        }
        public void DeleteRange(List<Guid> IDs)
        {
            var itemDeltes =  CustomerContractAttachmentFileDA.GetQuery(i=>IDs.Contains(i.AttachmentFileID)).ToList();
            CustomerContractAttachmentFileDA.DeleteRange(itemDeltes);
            CustomerContractAttachmentFileDA.Save();
        }
    }
}
