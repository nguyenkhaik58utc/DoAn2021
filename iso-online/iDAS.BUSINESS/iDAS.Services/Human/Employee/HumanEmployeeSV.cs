using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iDAS.Base;
using iDAS.Core;
using System.Security.Principal;
using iDAS.DA;
using iDAS.Items;

namespace iDAS.Services
{
    public class HumanEmployeeSV
    {
        private EmployeeDA EmployeeDA = new EmployeeDA();
        public HumanEmployeeViewItem GetEmployeeView(int? id)
        {
            if (id == null) return new HumanEmployeeViewItem();
            var employee = EmployeeDA.GetQuery()
                        .Where(i => i.ID == id)
                        .Select(i => new HumanEmployeeViewItem()
                        {
                            ID = i.ID,
                            Name = i.Name,
                            Department = i.HumanOrganizations.FirstOrDefault().HumanRole.HumanDepartment != null ? i.HumanOrganizations.FirstOrDefault().HumanRole.HumanDepartment.Name : string.Empty,
                            Role = i.HumanOrganizations.FirstOrDefault().HumanRole != null ? i.HumanOrganizations.FirstOrDefault().HumanRole.Name : string.Empty,
                        }).FirstOrDefault();
            return employee;
        }
        public List<HumanEmployeeViewItem> GetEmployeesByIDs(int page, int pageSize, out int totalCount, int[] ids)
        {
            return EmployeeDA.GetQuery(i => ids.Contains(i.ID))
                             .Select(i => new HumanEmployeeViewItem()
                                        {
                                            ID = i.ID,
                                            Name = i.Name,
                                            Department = i.HumanOrganizations.FirstOrDefault().HumanRole.HumanDepartment != null ? i.HumanOrganizations.FirstOrDefault().HumanRole.HumanDepartment.Name : string.Empty,
                                            Role = i.HumanOrganizations.FirstOrDefault().HumanRole != null ? i.HumanOrganizations.FirstOrDefault().HumanRole.Name : string.Empty,
                                        })
                            .OrderByDescending(item => item.Name)
                            .Page(page, pageSize, out totalCount)
                            .ToList();
        }
        public HumanEmployeeViewItem GetEmployeeViewByUser(int? userId)
        {
            if (userId == null) return new HumanEmployeeViewItem();
            var employee = EmployeeDA.Repository.HumanUsers.Where(i => i.ID == userId)
                        .Select(i => i.HumanEmployee)
                        .Select(i => new HumanEmployeeViewItem()
                        {
                            ID = i.ID,
                            Name = i.Name,
                            Department = i.HumanOrganizations.FirstOrDefault().HumanRole.HumanDepartment != null ? i.HumanOrganizations.FirstOrDefault().HumanRole.HumanDepartment.Name : string.Empty,
                            Role = i.HumanOrganizations.FirstOrDefault().HumanRole != null ? i.HumanOrganizations.FirstOrDefault().HumanRole.Name : string.Empty,
                        }).FirstOrDefault();
            return employee;
        }
        /// <summary>
        /// Author: Thanhnv DateTime: 09/12/2014
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public List<HumanEmployeeItem> GetAll(int page, int pageSize, out int totalCount, string querySearch = "")
        {
            var query = EmployeeDA.GetQuery();
            if (!string.IsNullOrWhiteSpace(querySearch))
            {
                query = query.Where(i => i.Name.Contains(querySearch));
            }
            var users = query
                        .Select(item => new HumanEmployeeItem()
                        {
                            ID = item.ID,
                            FileID = item.AttachmentFileID,
                            FileName = item.AttachmentFile.Name,
                            Role = item.HumanOrganizations.FirstOrDefault(i => i.HumanEmployeeID == item.ID).HumanRole.Name,
                            DepartmentName = item.HumanOrganizations.FirstOrDefault(i => i.HumanEmployeeID == item.ID).HumanRole.HumanDepartment.Name,
                            Code = item.Code,
                            Name = item.Name,
                            Email = item.Email,
                            Phone = item.Phone,
                            Address = item.Address,
                            Birthday = item.Birthday,
                            Gender = item.Gender,
                            CreateAt = item.CreateAt,
                            CreateBy = item.CreateBy,
                            UpdateAt = item.UpdateAt,
                            UpdateBy = item.UpdateBy,
                            IsTrial = item.IsTrial,
                            lstHumanGrPos = item.HumanOrganizations.Where(i => i.HumanEmployeeID == item.ID
                                && i.HumanRole.IsActive && !i.HumanRole.IsDestroy && !i.HumanRole.IsDelete
                                ).Select(i => new HumanGroupPositionItem()
                            {
                                ID = i.ID,
                                Name = i.HumanRole.Name,
                                GroupName = i.HumanRole.HumanDepartment.Name
                            }).ToList()
                        })
                        .OrderByDescending(item => item.CreateAt)
                        .Page(page, pageSize, out totalCount)
                        .ToList();
            return users;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public List<HumanEmployeeItem> GetAllByName(int page, int pageSize, out int totalCount, string Name)
        {
            var employee = String.IsNullOrEmpty(Name) ? EmployeeDA.GetQuery() : EmployeeDA.GetQuery(i => i.Name.ToLower().Contains(Name.ToLower()));
            var users = employee.Select(item => new HumanEmployeeItem()
                        {
                            ID = item.ID,
                            //UserID = item.UserID,
                            //Image = item.Image,
                            Code = item.Code,
                            Name = item.Name,
                            Email = item.Email,
                            Phone = item.Phone,
                            Address = item.Address,
                            Birthday = item.Birthday,
                            Gender = item.Gender,
                            CreateAt = item.CreateAt,
                            CreateBy = item.CreateBy,
                            UpdateAt = item.UpdateAt,
                            UpdateBy = item.UpdateBy
                        })
                        .OrderByDescending(item => item.CreateAt)
                        .Page(page, pageSize, out totalCount)
                        .ToList();
            return users;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <param name="DepartmentId"></param>
        /// <returns></returns>
        public List<HumanEmployeeItem> GetAllByDepartmentId(int page, int pageSize, out int totalCount, int DepartmentId, string querySearch = "")
        {
             var dbo = EmployeeDA.Repository;
            var query =  dbo.HumanRoles.Where(i => i.HumanDepartmentID == DepartmentId)
                .SelectMany(i => i.HumanOrganizations)
                .Select(i => i.HumanEmployee).Distinct();
            if(!string.IsNullOrWhiteSpace(querySearch))
            {
                query = query.Where( i=>i.Name.Contains(querySearch));
            }

            var employee = query.Select(item => new HumanEmployeeItem()
                        {
                            ID = item.ID,
                            FileID = item.AttachmentFileID,
                            FileName = item.AttachmentFile.Name,
                            Role = item.HumanOrganizations.FirstOrDefault(i => i.HumanEmployeeID == item.ID).HumanRole.Name,
                            DepartmentName = item.HumanOrganizations.FirstOrDefault(i => i.HumanEmployeeID == item.ID).HumanRole.HumanDepartment.Name,
                            Code = item.Code,
                            Name = item.Name,
                            Email = item.Email,
                            Phone = item.Phone,
                            Address = item.Address,
                            Birthday = item.Birthday,
                            IsAudit = false,
                            IsCheck = false,
                            IsPerform = false,
                            IsApproval = false,
                            IsChange = false,
                            IsClosed = false,
                            lstHumanGrPos = dbo.HumanOrganizations.Where(i => i.HumanEmployeeID == item.ID).Select(i => new HumanGroupPositionItem()
                            {
                                ID = i.ID,
                                Name = i.HumanRole.Name,
                                GroupName = i.HumanRole.HumanDepartment.Name
                            }).ToList()
                        })
                        .OrderByDescending(item => item.Name)
                        .Page(page, pageSize, out totalCount)
                        .ToList();
            return employee;
        }

        public List<HumanEmployeeItem> GetEmployeeByFunctionCode(int page, int pageSize, out int totalCount, string moduleCode, string functionCode)
        {
            var dbo = EmployeeDA.Repository;
            var functionCodequery = "";
            if (functionCode.Length > moduleCode.Length) functionCodequery = functionCode.Substring(moduleCode.Length);
            var employee =
                dbo.HumanPermissions.Where(i => i.ModuleCode == moduleCode && i.FunctionCode == functionCodequery)
                .Select(i => i.HumanRole)
               .SelectMany(i => i.HumanOrganizations)
               .Select(i => i.HumanEmployee).Distinct()
               .Select(item => new HumanEmployeeItem()
               {
                   ID = item.ID,
                   FileID = item.AttachmentFileID,
                   FileName = item.AttachmentFile.Name,
                   Code = item.Code,
                   Name = item.Name,
                   Email = item.Email,
                   Phone = item.Phone,
                   Address = item.Address,
                   Birthday = item.Birthday,
                   IsAudit = false,
                   IsCheck = false,
                   IsPerform = false,
                   IsApproval = false,
                   IsChange = false,
                   IsClosed = false,
                   lstHumanGrPos = dbo.HumanOrganizations.Where(i => i.HumanEmployeeID == item.ID).Select(i => new HumanGroupPositionItem()
                   {
                       ID = i.ID,
                       Name = i.HumanRole.Name,
                       GroupName = i.HumanRole.HumanDepartment.Name
                   }).ToList()
               })
                       .OrderByDescending(item => item.Name)
                       .Page(page, pageSize, out totalCount)
                       .ToList();
            return employee;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public string GetAvatarById(int Id)
        {
            var employee = EmployeeDA.GetById(Id);
            string avatar = string.Empty;
            if (employee != null)
            {
                avatar = employee.Avatar;
            }
            return avatar;
        }
        /// <summary>
        /// Author: Thanhnv DateTime: 09/12/2014 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public HumanEmployeeItem GetById(int Id)
        {

            var dbo = EmployeeDA.Repository;
            //ProfileEmployeeDA ProfileEmployeeDa = new ProfileEmployeeDA();
            var employee = EmployeeDA.GetQuery(i => i.ID == Id)
                        .Select(item => new HumanEmployeeItem()
                        {
                            ID = item.ID,
                            // Image = item.Image,
                            FileID = item.AttachmentFileID,
                            ImageFile = item.AttachmentFile != null ? new FileItem()
                            {
                                ID = item.AttachmentFile.ID,
                                Name = item.AttachmentFile.Name,
                                Data = item.AttachmentFile.Data,
                            } : null,
                            Code = item.Code,
                            Name = item.Name,
                            Email = item.Email,
                            Phone = item.Phone,
                            Address = item.Address,
                            Birthday = item.Birthday,
                            Gender = item.Gender,
                            sex = item.Gender == true ? "Nam" : "Nữ",
                            WeddingStatus = item.WeddingStatus,
                            CreateAt = item.CreateAt,
                            CreateBy = item.CreateBy,
                            UpdateAt = item.UpdateAt,
                            UpdateBy = item.UpdateBy,
                            CreateName = item.CreateBy == null ? "" : dbo.HumanUsers.FirstOrDefault(u => u.ID == item.CreateBy).HumanEmployee.Name,
                            //,
                            //ID = item.HumanEmployee.ID,
                            //UserID = item.HumanEmployee.UserID,
                            //Avatar = item.HumanEmployee.Avatar,
                            //Code = item.HumanEmployee.Code,
                            //Name = item.HumanEmployee.Name,
                            //Email = item.HumanEmployee.Email,
                            //Phone = item.HumanEmployee.Phone,
                            //Address = item.HumanEmployee.Address,
                            //Birthday = item.HumanEmployee.Birthday,
                            //Gender = item.HumanEmployee.Gender,
                            //CreateAt = item.HumanEmployee.CreateAt,
                            //CreateBy = item.HumanEmployee.CreateBy,
                            //UpdateAt = item.HumanEmployee.UpdateAt,
                            //UpdateBy = item.HumanEmployee.UpdateBy,
                            //,
                            #region Profile Employee
                            //Aliases = item.HumanProfileCuriculmViate.Aliases,
                            //ArmyRank = item.HumanProfileCuriculmViate.ArmyRank,
                            //Bank = item.HumanProfileCuriculmViate.Bank,
                            //DateAtParty = item.HumanProfileCuriculmViate.DateAtParty,
                            //DateIssueOfIdentityCard = item.HumanProfileCuriculmViate.DateIssueOfIdentityCard,
                            //DateJoinRevolution = item.HumanProfileCuriculmViate.DateJoinRevolution,
                            //DateOfIssuePassport = item.HumanProfileCuriculmViate.DateOfIssuePassport,
                            //DateOfJoinParty = item.HumanProfileCuriculmViate.DateOfJoinParty,
                            //DateOnArmy = item.HumanProfileCuriculmViate.DateOnArmy,
                            //DateOnGroup = item.HumanProfileCuriculmViate.DateOnGroup,
                            //Defect = item.HumanProfileCuriculmViate.Defect,
                            //Forte = item.HumanProfileCuriculmViate.Forte,
                            //HomePhone = item.HumanProfileCuriculmViate.HomePhone,
                            //Likes = item.HumanProfileCuriculmViate.Likes,
                            //Nationality = item.HumanProfileCuriculmViate.Nationality,
                            //NumberOfBankAccounts = item.HumanProfileCuriculmViate.NumberOfBankAccounts,
                            //NumberOfPartyCard = item.HumanProfileCuriculmViate.NumberOfPartyCard,
                            //NumberOfPassport = item.HumanProfileCuriculmViate.NumberOfPassport,
                            //OfficePhone = item.HumanProfileCuriculmViate.OfficePhone,
                            //PassportExpirationDate = item.HumanProfileCuriculmViate.PassportExpirationDate,
                            //People = item.HumanProfileCuriculmViate.People,
                            //PlaceIssueOfIdentityCard = item.HumanProfileCuriculmViate.PlaceIssueOfIdentityCard,
                            //PlaceOfBirth = item.HumanProfileCuriculmViate.PlaceOfBirth,
                            //PlaceOfLoadedGroup = item.HumanProfileCuriculmViate.PlaceOfLoadedGroup,
                            //PlaceOfLoadedParty = item.HumanProfileCuriculmViate.PlaceOfLoadedParty,
                            //PlaceOfPassport = item.HumanProfileCuriculmViate.PlaceOfPassport,
                            //PositionArmy = item.HumanProfileCuriculmViate.PositionArmy,
                            //PositionGroup = item.HumanProfileCuriculmViate.PositionGroup,
                            //PosititonParty = item.HumanProfileCuriculmViate.PosititonParty,
                            //Religion = item.HumanProfileCuriculmViate.Religion,
                            //TaxCode = item.HumanProfileCuriculmViate.TaxCode,
                            //WeddingStatus = item.HumanProfileCuriculmViate.WeddingStatus
                            #endregion

                        }).FirstOrDefault()
                        ;
            return employee;
        }
        public byte[] GetAvatarFile(int employeeID)
        {
            var data = EmployeeDA.GetQuery(i => i.ID == employeeID)
                        .Where(i => i.AttachmentFileID != null)
                        .Select(i => i.AttachmentFile.Data).FirstOrDefault();
            if (data == null)
            {
                data = new CenterSystemSV().GetAvatarSystem();
            }
            return data;
        }
        public List<DocumentSecurityItem> GetAllCbo()
        {
            var users = EmployeeDA.GetQuery()
                        .Select(item => new DocumentSecurityItem()
                        {
                            ID = item.ID,
                            //UserID = item.UserID,
                            Name = item.Name
                        })
                        .OrderByDescending(item => item.Name)

                        .ToList();
            return users;
        }
        //HuongLL: get Name by ID
        public String GetNameByID(int? id)
        {
            if (id > 0)
            {

                var users = EmployeeDA.GetById(id);
                if (users != null)
                    return users.Name;
            }
            return "";
        }
        /// <summary>
        /// Author: Thanhnv DateTime: 09/12/2014
        /// </summary>
        /// <param name="item"></param>
        /// <param name="userID"></param>
        public void Update(HumanEmployeeItem item, int userID)
        {
            var employee = EmployeeDA.GetById(item.ID);
            if (item.ImageFile != null)
            {
                if (employee.AttachmentFileID == null)
                {
                    item.ImageFile.ID = Guid.NewGuid();
                    new FileSV().Insert(item.ImageFile, userID);
                    employee.AttachmentFileID = item.ImageFile.ID;
                }
                else
                {
                    item.ImageFile.ID = (Guid)employee.AttachmentFileID;
                    FileDA FileDA = new FileDA();
                    var file = FileDA.GetById(item.ImageFile.ID);
                    file.ID = item.ImageFile.ID;
                    file.Name = item.ImageFile.Name;
                    file.Extension = item.ImageFile.Extension;
                    //file.Path = item.Path;
                    file.Size = item.ImageFile.Size;
                    file.Type = item.ImageFile.Type;
                    file.Data = item.ImageFile.Data;
                    file.UpdateAt = DateTime.Now;
                    file.UpdateBy = userID;
                    FileDA.Save();
                    employee.AttachmentFileID = file.ID;
                }
            }
            employee.Code = item.Code;
            employee.Name = item.Name;
            employee.Email = item.Email;
            employee.Phone = item.Phone;
            employee.Address = item.Address;
            employee.Birthday = item.Birthday;
            employee.WeddingStatus = item.WeddingStatus;
            employee.Gender = item.Gender;
            employee.UpdateAt = DateTime.Now;
            employee.UpdateBy = userID;

            #region Profile employee
            //ProfileEmployeeDA ProfileEmployeeDa = new ProfileEmployeeDA();
            //var pr = ProfileEmployeeDa.GetById(item.ProfileEmployeeID);
            //var pr = user.HumanProfileCuriculmViate;
            //pr.Aliases = item.Aliases;
            //pr.ArmyRank = item.ArmyRank;
            //pr.Bank = item.Bank;
            //pr.DateAtParty = item.DateAtParty;
            //pr.DateIssueOfIdentityCard = item.DateIssueOfIdentityCard;
            //pr.DateJoinRevolution = item.DateJoinRevolution;
            //pr.DateOfIssuePassport = item.DateOfIssuePassport;
            //pr.DateOfJoinParty = item.DateOfJoinParty;
            //pr.DateOnArmy = item.DateOnArmy;
            //pr.DateOnGroup = item.DateOnGroup;
            //pr.Defect = item.Defect;
            //pr.Forte = item.Forte;
            //pr.HomePhone = item.HomePhone;
            //pr.Likes = item.Likes;
            //pr.Nationality = item.Nationality;
            //pr.NumberOfBankAccounts = item.NumberOfBankAccounts;
            //pr.NumberOfPartyCard = item.NumberOfPartyCard;
            //pr.NumberOfPassport = item.NumberOfPassport;
            //pr.OfficePhone = item.OfficePhone;
            //pr.PassportExpirationDate = item.PassportExpirationDate;
            //pr.People = item.People;
            //pr.PlaceIssueOfIdentityCard = item.PlaceIssueOfIdentityCard;
            //pr.PlaceOfBirth = item.PlaceOfBirth;
            //pr.PlaceOfLoadedGroup = item.PlaceOfLoadedGroup;
            //pr.PlaceOfLoadedParty = item.PlaceOfLoadedParty;
            //pr.PlaceOfPassport = item.PlaceOfPassport;
            //pr.PositionArmy = item.PositionArmy;
            //pr.PositionGroup = item.PositionGroup;
            //pr.PosititonParty = item.PosititonParty;
            //pr.Religion = item.Religion;
            //pr.TaxCode = item.TaxCode;
            //pr.WeddingStatus = item.WeddingStatus;
            //pr.CreateAt = item.CreateAt;
            //pr.CreateBy = item.CreateBy;
            //pr.UpdateAt = DateTime.Now;
            //pr.UpdateBy = userID;
            #endregion

            EmployeeDA.Save();
        }
        /// <summary>
        /// Author: Thanhnv DateTime: 09/12/2014
        /// Thêm mới người dùng và thiết lập chức danh
        /// </summary>
        /// <param name="item"></param>
        /// <param name="userID"></param>
        //public void InsertAndSetting(EmployeeItem item, int userID, String strListRoleId)
        //{
        //    var user = new HumanEmployee()
        //    {
        //        Name = item.Name,
        //        Email = item.Email,
        //        Phone = item.Phone,
        //        Address = item.Address,
        //        Birthday = item.Birthday,
        //        Gender = item.Gender,
        //        CreateAt = DateTime.Now,
        //        CreateBy = userID,
        //    };
        //    EmployeeDA.Insert(user);
        //    EmployeeDA.Save();
        //    var ids = strListRoleId.Substring(1).Split(',').Select(o => int.Parse(o)).Distinct();
        //    OrganizationDA organizationDA = new OrganizationDA();
        //    var organizations = new List<HumanOrganization>();
        //    foreach (var id in ids)
        //    {
        //        organizations.Add(
        //            new HumanOrganization()
        //            {
        //                EmployeeID = user.ID,
        //                RoleID = id,
        //                CreateAt = DateTime.Now,
        //                CreateBy = userID,
        //            }
        //            );
        //    }
        //    organizationDA.InsertRange(organizations);
        //    organizationDA.Save();
        //}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="userID"></param>
        public int Insert(HumanEmployeeItem item, int userID)
        {

            #region Profile Employee
            //var pr = new HumanProfileCuriculmViate()
            //{

            //    Aliases = item.Aliases,
            //    ArmyRank = item.ArmyRank,
            //    Bank = item.Bank,
            //    DateAtParty = item.DateAtParty,
            //    DateIssueOfIdentityCard = item.DateIssueOfIdentityCard,
            //    DateJoinRevolution = item.DateJoinRevolution,
            //    DateOfIssuePassport = item.DateOfIssuePassport,
            //    DateOfJoinParty = item.DateOfJoinParty,
            //    DateOnArmy = item.DateOnArmy,
            //    DateOnGroup = item.DateOnGroup,
            //    Defect = item.Defect,
            //    Forte = item.Forte,
            //    HomePhone = item.HomePhone,
            //    Likes = item.Likes,
            //    Nationality = item.Nationality,
            //    NumberOfBankAccounts = item.NumberOfBankAccounts,
            //    NumberOfPartyCard = item.NumberOfPartyCard,
            //    NumberOfPassport = item.NumberOfPassport,
            //    OfficePhone = item.OfficePhone,
            //    PassportExpirationDate = item.PassportExpirationDate,
            //    People = item.People,
            //    PlaceIssueOfIdentityCard = item.PlaceIssueOfIdentityCard,
            //    PlaceOfBirth = item.PlaceOfBirth,
            //    PlaceOfLoadedGroup = item.PlaceOfLoadedGroup,
            //    PlaceOfLoadedParty = item.PlaceOfLoadedParty,
            //    PlaceOfPassport = item.PlaceOfPassport,
            //    PositionArmy = item.PositionArmy,
            //    PositionGroup = item.PositionGroup,
            //    PosititonParty = item.PosititonParty,
            //    Religion = item.Religion,
            //    TaxCode = item.TaxCode,
            //    //WeddingStatus = item.WeddingStatus,
            //    UpdateAt = item.UpdateAt,
            //    UpdateBy = item.UpdateBy,
            //    CreateAt = DateTime.Now,
            //    CreateBy = userID,
            //};
            //var dbo = EmployeeDA.Repository;
            //dbo.HumanProfileCuriculmViate.Add(pr);
            #endregion
            var user = new HumanEmployee()
            {
                //ProfileEmployeeID = pr.ID,
                Name = item.Name,
                Email = item.Email,
                Code = item.Code,
                Phone = item.Phone,
                Address = item.Address,
                Birthday = item.Birthday,
                WeddingStatus = item.WeddingStatus,
                Gender = item.Gender,
                CreateAt = DateTime.Now,
                CreateBy = userID,
            };
            if (item.ImageFile != null)
            {
                var imgId = new FileSV().Insert(item.ImageFile, userID);
                user.AttachmentFileID = imgId;
            }
            EmployeeDA.Insert(user);
            EmployeeDA.Save();
            return user.ID;
        }
        ///// <summary>
        ///// Author: Thanhnv DateTime: 09/12/2014
        ///// </summary>
        ///// <param name="item"></param>
        ///// <param name="userID"></param>
        ///// <param name="id"></param>
        //public void Insert(EmployeeItem item, int userID, out int id)
        //{
        //    var user = new HumanEmployee()
        //    {
        //        Name = item.Name,
        //        Avatar = item.Avatar,
        //        Code = item.Code,
        //        Email = item.Email,
        //        Phone = item.Phone,
        //        Address = item.Address,
        //        Birthday = item.Birthday,
        //        Gender = item.Gender,
        //        CreateAt = DateTime.Now,
        //        CreateBy = userID,
        //    };
        //    EmployeeDA.Insert(user);
        //    EmployeeDA.Save();
        //    id = user.ID;
        //}
        /// <summary>
        /// Author: Thanhnv DateTime: 09/12/2014
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            var dbo = EmployeeDA.Repository;
            var prem = dbo.HumanProfileCuriculmViates.FirstOrDefault(i => i.HumanEmployeeID == id);
            if (prem != null)
                dbo.HumanProfileCuriculmViates.Remove(prem);
            EmployeeDA.Delete(id);

            EmployeeDA.Save();
        }
        public List<ComboboxItem> Combobox(int page, int pageSize, out int total, string name)
        {
            var dbo = EmployeeDA.Repository;
            var data = dbo.HumanEmployees
                        .Where(i => string.IsNullOrEmpty(name) || i.Name.Contains(name))
                        .Select(
                                item => new ComboboxItem()
                                {
                                    ID = item.ID,
                                    Name = item.Name,
                                }
                                )
                        .OrderBy(i => i.Name)
                        .Page(page, pageSize, out total)
                        .ToList();
            return data;
        }

        public void UpdateTrial(int empID,bool p)
        {
            var employee = EmployeeDA.GetById(empID);
            employee.IsTrial = p;
            EmployeeDA.Save();
            var profileDA = new HumanRecruitmentProfileDA();
            var pr = profileDA.GetQuery(x => x.EmployeeID == empID).First();
            if (pr != null)
                pr.IsEmployee = p;
            profileDA.Save();
        }
    }
}
