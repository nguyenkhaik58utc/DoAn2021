using ISO.API.Models.HumanProfileCuriculmViate;
using Dapper;
using ISO.API.DataAccess;
using ISO.API.Models.City;
using ISO.API.Service.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ISO.API.Models.ProfileHumanPermission;
using Newtonsoft.Json;

namespace ISO.API.Service.Implement
{
    public class HumanProfileCuriculmViateService : IHumanProfileCuriculmViateService
    {
        public List<HumanProfileCuriculmViateDTO> GetAll()
        {
            ExecuteQuery exec = new ExecuteQuery();

            List<HumanProfileCuriculmViateDTO> lst = exec.GetByField<HumanProfileCuriculmViateDTO>("sp_V3HumanProfileCuriculmViate_GetAll", null, commandType: CommandType.StoredProcedure);
            return lst;
        }

        public HumanProfileCuriculmViateDTO GetByHumanEmployeeID(int humanEmployeeID)
        {
            ExecuteQuery exec = new ExecuteQuery();
            object param = new { HumanEmployeeID = humanEmployeeID };

            List<HumanProfileCuriculmViateDTO> lst = exec.GetByField<HumanProfileCuriculmViateDTO>("sp_V3HumanProfileCuriculmViate_GetByHumanEmployeeID", param, commandType: CommandType.StoredProcedure);
            return lst[0];
        }

        public List<HumanCommonInformationDTO> GetCommonIformationByListEmployeeID(string lstEmployeeID)
        {
            ExecuteQuery exec = new ExecuteQuery();
            var dynamicParameters = new DynamicParameters();
            DataTable dt = new DataTable();
            dt.Columns.Add("ID", typeof(Int32));
            List<int> lstID = JsonConvert.DeserializeObject<List<int>>(lstEmployeeID);
            foreach (var item in lstID)
            {
                dt.Rows.Add(item);
            }
            dynamicParameters.Add("@lstID", dt.AsTableValuedParameter("[dbo].[EmployeeIDList]"));
            List<HumanCommonInformationDTO> lst = exec.GetByField<HumanCommonInformationDTO>("sp_V3GetCommonInformationByListEmployeeID", dynamicParameters, commandType: CommandType.StoredProcedure);
            return lst;
        }

        public List<HumanPoliticInformationDTO> GetPoliticIformationByListEmployeeID(string lstEmployeeID)
        {
            ExecuteQuery exec = new ExecuteQuery();
            var dynamicParameters = new DynamicParameters();
            DataTable dt = new DataTable();
            dt.Columns.Add("ID", typeof(Int32));
            List<int> lstID = JsonConvert.DeserializeObject<List<int>>(lstEmployeeID);
            foreach (var item in lstID)
            {
                dt.Rows.Add(item);
            }
            dynamicParameters.Add("@lstID", dt.AsTableValuedParameter("[dbo].[EmployeeIDList]"));
            List<HumanPoliticInformationDTO> lst = exec.GetByField<HumanPoliticInformationDTO>("sp_V3GetPoliticInformationByListEmployeeID", dynamicParameters, commandType: CommandType.StoredProcedure);
            return lst;
        }

        public int Save(HumanProfileCuriculmViateDTO entity)
        {
            ExecuteQuery exec = new ExecuteQuery();
            var param = new DynamicParameters();
            param.Add("ID", entity.ID);
            param.Add("HumanEmployeeID", entity.HumanEmployeeID);
            param.Add("Aliases", entity.Aliases);

            param.Add("NationalityID", entity.NationalityID);
            param.Add("EthnicID", entity.EthnicID);
            param.Add("ReligionID", entity.ReligionID);
            param.Add("CityIDOfBirth", entity.CityIDOfBirth);
            param.Add("DistrictIDOfBirth", entity.DistrictIDOfBirth);
            param.Add("CommuneIDOfBirth", entity.CommuneIDOfBirth);
            param.Add("OfficePhone", entity.OfficePhone);

            param.Add("HomePhone", entity.HomePhone);
            param.Add("NumberOfIdentityCard", entity.NumberOfIdentityCard);
            param.Add("DateIssueOfIdentityCard", entity.DateIssueOfIdentityCard);
            param.Add("PlaceIssueOfIdentityCard", entity.PlaceIssueOfIdentityCard);
            param.Add("DateOnGroup", entity.DateOnGroup);
            param.Add("YouthPositionID", entity.YouthPositionID);
            param.Add("PlaceOfLoadedGroup", entity.PlaceOfLoadedGroup);

            param.Add("DateJoinRevolution", entity.DateJoinRevolution);
            param.Add("DateAtParty", entity.DateAtParty);
            param.Add("DateOfJoinParty", entity.DateOfJoinParty);
            param.Add("PlaceOfLoadedParty", entity.PlaceOfLoadedParty);
            param.Add("PartyPosititonID", entity.PartyPosititonID);


            param.Add("NumberOfPartyCard", entity.NumberOfPartyCard);
            param.Add("DateOnMilitary", entity.DateOnMilitary);
            param.Add("MilitaryPosition", entity.MilitaryPosition);
            param.Add("MilitaryPositionLevelID", entity.MilitaryPositionLevelID);
            param.Add("Likes", entity.Likes);

            param.Add("Forte", entity.Forte);
            param.Add("Defect", entity.Defect);
            param.Add("TaxCode", entity.TaxCode);
            param.Add("NumberOfBankAccounts", entity.NumberOfBankAccounts);
            param.Add("Bank", entity.Bank);

            param.Add("NumberOfPassport", entity.NumberOfPassport);
            param.Add("PlaceOfPassport", entity.PlaceOfPassport);
            param.Add("DateOfIssuePassport", entity.DateOfIssuePassport);
            param.Add("PassportExpirationDate", entity.PassportExpirationDate);
            param.Add("CityIDOfHomeTown", entity.CityIDOfHomeTown);
            param.Add("DistrictIDOfHomeTown", entity.DistrictIDOfHomeTown);
            param.Add("CommunelIDOfHomeTown", entity.CommunelIDOfHomeTown);
            param.Add("residence", entity.residence);
            param.Add("currentAddress", entity.currentAddress);
            param.Add("PoliticalTheoryID", entity.PoliticalTheoryID);
            param.Add("GovermentManagementID", entity.GovermentManagementID);

            param.Add("CreatedBy", entity.CreatedBy);
            //param.Add("CreatedAt", entity.CreatedAt);
            param.Add("UpdatedBy", entity.UpdatedBy);
            //param.Add("UpdatedAt", entity.UpdatedAt);
            param.Add("ReturnID", dbType: DbType.Int32, direction: ParameterDirection.Output);

            var result = exec.ExcuteReturnValue("sp_V3HumanProfileCuriculmViate_InsUpd", param, "ReturnID", commandType: CommandType.StoredProcedure);
            return result;
        }

        public HumanProfileCuriculmViateReportDTO GetFullByHumanEmployeeID(int humanEmployeeID)
        {
            ExecuteQuery exec = new ExecuteQuery();
            object param = new { HumanEmployeeID = humanEmployeeID };
            List<HumanProfileCuriculmViateReportDTO> lst = exec.GetByField<HumanProfileCuriculmViateReportDTO>("sp_V3HumanProfileCuriculmViate_GetFullByHumanEmployeeID", param, commandType: CommandType.StoredProcedure);
            return lst[0];
        }
    }
}
