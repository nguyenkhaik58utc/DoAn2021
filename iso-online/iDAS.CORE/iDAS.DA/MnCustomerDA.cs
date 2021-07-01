using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iDAS.Base;

namespace iDAS.DAL.MnCustomer
{
    public class ProductDA : SystemBaseDA<MnCustomerProduct> {}
    public class CustomerDA : SystemBaseDA<MnCustomerCustomer> { }
    public class CampaignDA : SystemBaseDA<MnCustomerCampaign> { }
    public class ObjectDA : SystemBaseDA<MnCustomerObject> { }
    public class TaskDA : SystemBaseDA<MnCustomerTask> { }
    public class DemandDA : SystemBaseDA<MnCustomerDemand> { }
    public class ContactDA : SystemBaseDA<MnCustomerContact> { }
    public class TransactionDA : SystemBaseDA<MnCustomerTransaction> { }
    public class OrderDA : SystemBaseDA<MnCustomerOrder> { }
    public class ContractDA : SystemBaseDA<MnCustomerContract> { }
    public class CareDA : SystemBaseDA<MnCustomerCare> { }
    public class AssessDA : SystemBaseDA<MnCustomerAssess> { }
    public class CategoryDA : SystemBaseDA<MnCustomerCategory> { }
    public class MethodDA : SystemBaseDA<MnCustomerMethod> { }
    public class SurveyDA : SystemBaseDA<MnCustomerSurvey> { }
    public class CountryDA : SystemBaseDA<InitCountry> { }
    public class CityDA : SystemBaseDA<InitCity> { }
    public class DistrictDA : SystemBaseDA<InitDistrict> { }
    public class CurrencyDA : SystemBaseDA<InitCurrency> { }
}
