using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iDAS.DA;
using iDAS.Items;
using iDAS.Core;
using iDAS.Base;

namespace iDAS.Services
{
    public class BusinessChatSV
    {
        BusinessChatDA chatDA = new BusinessChatDA();

        public List<BusinessChatItem> GetChats(int employeeID, int employeeChatID) 
        {
            var dbo = chatDA.Repository;
            var chats = chatDA.GetQuery()
                        .Where(i => dbo.HumanUsers.FirstOrDefault(o => o.ID == i.CreateBy).HumanEmployeeID == employeeID || i.HumanEmployeeID == employeeID)
                        .Where(i => dbo.HumanUsers.FirstOrDefault(o => o.ID == i.CreateBy).HumanEmployeeID == employeeChatID || i.HumanEmployeeID == employeeChatID)
                        .Select(i => new BusinessChatItem()
                        {
                            ID = i.ID,
                            Sender = new BusinessChatEmployeeItem()
                            {
                                ID = dbo.HumanUsers.FirstOrDefault(o => o.ID == i.CreateBy).HumanEmployee.ID,
                                Name = dbo.HumanUsers.FirstOrDefault(o => o.ID == i.CreateBy).HumanEmployee.Name,
                                Department = dbo.HumanUsers.FirstOrDefault(o => o.ID == i.CreateBy).HumanEmployee.HumanOrganizations.FirstOrDefault().HumanRole.HumanDepartment.Name,
                                Role = dbo.HumanUsers.FirstOrDefault(o => o.ID == i.CreateBy).HumanEmployee.HumanOrganizations.FirstOrDefault().HumanRole.Name,
                                Display = dbo.HumanUsers.FirstOrDefault(o => o.ID == i.CreateBy).HumanEmployee.ID == employeeID ? "right" : "left",
                                Content = i.Content,
                                CreateAt = i.CreateAt,
                            },
                        })
                        .OrderBy(i => i.Sender.CreateAt)
                        .ToList();
            return chats;
        }

        public List<BusinessChatEmployeeItem> GetEmployeeOnline(List<int> employeeIDs, int employeeID, string status) 
        {
            List<BusinessChatEmployeeItem> data = new List<BusinessChatEmployeeItem>();
            foreach (var employeeChatID in employeeIDs) {
                if (employeeChatID != employeeID)
                {
                    data.Add(GetEmployee(employeeID, employeeChatID, status));
                }
            }
            return data;
        }

        public List<BusinessChatEmployeeItem> GetEmployeeOffline(List<int> employeeIDs, int employeeID, string status)
        {
            var dbo = chatDA.Repository;
            var chats = chatDA.GetQuery()
                        .Where(i => i.HumanEmployeeID == employeeID);
            var employees = chats
                            .Select(i => dbo.HumanUsers.FirstOrDefault(o => o.ID == i.CreateBy).HumanEmployee)
                            .Distinct();
            var data = employees
                        .Where(i => !employeeIDs.Contains(i.ID) && i!=null)
                        .Select(i => new BusinessChatEmployeeItem()
                        {
                            ID = i.ID,
                            Name = i.Name,
                            Role = i.HumanOrganizations.FirstOrDefault().HumanRole.Name,
                            Department = i.HumanOrganizations.FirstOrDefault().HumanRole.HumanDepartment.Name,
                            NumberNotRead = chats.Where(o => dbo.HumanUsers.FirstOrDefault(t => t.ID == o.CreateBy).HumanEmployeeID == i.ID && !o.IsRead).Count(),
                            Status = status,
                            CreateAt = i.CreateAt
                        })                     
                        .ToList();
            return data;
        }

        public BusinessChatEmployeeItem GetEmployee(int employeeID, int employeeChatID, string status="")
        {
            var dbo = chatDA.Repository;
            var chats = chatDA.GetQuery()
                        .Where(i => dbo.HumanUsers.Where(o => o.ID == i.CreateBy).FirstOrDefault().HumanEmployeeID == employeeChatID)
                        .Where(i => i.HumanEmployeeID == employeeID)
                        .Where(i => !i.IsRead);
            var employee = dbo.HumanEmployees
                            .Where(i => i.ID == employeeChatID)
                            .Select(i => new BusinessChatEmployeeItem()
                            {
                                ID = i.ID,
                                Name = i.Name,
                                Role = i.HumanOrganizations.FirstOrDefault().HumanRole.Name,
                                Department = i.HumanOrganizations.FirstOrDefault().HumanRole.HumanDepartment.Name,
                                NumberNotRead = chats.Count(),
                                Status = status,
                            })
                            .FirstOrDefault();
            return employee;
        }

        public void Insert(string content, List<int> employeeIDs, int userID)
        {
            foreach (var id in employeeIDs)
            {
                var chat = new Chat()
                {
                    Content = content,
                    IsRead = false,
                    CreateAt = DateTime.Now,
                    CreateBy = userID,
                    HumanEmployeeID = id
                };
                chatDA.Insert(chat);
            }
            chatDA.Save();
        }

        public void Update(int employeeID, int employeeChatID) {
            var dbo = chatDA.Repository;
            var chats = chatDA.GetQuery()
                        .Where(i => dbo.HumanUsers.Where(o => o.ID == i.CreateBy).FirstOrDefault().HumanEmployeeID == employeeChatID)
                        .Where(i => i.HumanEmployeeID == employeeID)
                        .Where(i => !i.IsRead)
                        .ToList();
            foreach (var chat in chats) {
                chat.IsRead = true;
                chat.ReadTime = DateTime.Now;
            }
            chatDA.Save();
        }
    }
}
