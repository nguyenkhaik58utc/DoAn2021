using Ext.Net;
using iDAS.Items;
using iDAS.Services;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iDAS.Web.Areas.Systems
{
    public class CommonFunction
    {

        public static List<ServiceItem> GetAllSystem()
        {
            ServiceSV cateSV = new ServiceSV();           
            var lstCate = cateSV.GetAll();
            return lstCate;        
        }

        //

        public static List<object> GetDataFilter(StoreRequestParameters parameters, IEnumerable<object> lstObj)
        {
            FilterConditions fc = parameters.GridFilters;
            var data = lstObj.ToList();
            //-- start filtering ------------------------------------------------------------
            if (fc != null)
            {
                foreach (FilterCondition condition in fc.Conditions)
                {
                    Comparison comparison = condition.Comparison;
                    string field = condition.Field;
                    FilterType type = condition.Type;

                    object value;
                    switch (condition.Type)
                    {
                        case FilterType.Boolean:
                            value = condition.Value<bool>();
                            break;
                        case FilterType.Date:
                            value = condition.Value<DateTime>();
                            break;
                        case FilterType.List:
                            value = condition.List;
                            break;
                        case FilterType.Numeric:
                            if (data.Count > 0 && data[0].GetType().GetProperty(field).PropertyType == typeof(int))
                            {
                                value = condition.Value<int>();
                            }
                            else
                            {
                                value = condition.Value<double>();
                            }

                            break;
                        case FilterType.String:
                            value = condition.Value<string>();
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    data.RemoveAll(
                        item =>
                        {
                            object oValue = item.GetType().GetProperty(field).GetValue(item, null);
                            IComparable cItem = oValue as IComparable;
                            if (cItem != null)
                            {
                                switch (comparison)
                                {
                                    case Comparison.Eq:

                                        switch (type)
                                        {
                                            case FilterType.List:
                                                return !(value as List<string>).Contains(oValue.ToString());
                                            case FilterType.String:
                                                return !oValue.ToString().StartsWith(value.ToString());
                                            default:
                                                return !cItem.Equals(value);
                                        }

                                    case Comparison.Gt:
                                        return cItem.CompareTo(value) < 1;
                                    case Comparison.Lt:
                                        return cItem.CompareTo(value) > -1;
                                    default:
                                        throw new ArgumentOutOfRangeException();
                                }                                
                            }
                            return !cItem.Equals(value);
                        }
                    );
                }
            }
            //-- end filtering ------------------------------------------------------------


            //-- start sorting ------------------------------------------------------------
            if (parameters.Sort.Length > 0)
            {
                DataSorter sorter = parameters.Sort[0];
                data.Sort(delegate(object x, object y)
                {
                    object a;
                    object b;

                    int direction = sorter.Direction == Ext.Net.SortDirection.DESC ? -1 : 1;

                    a = x.GetType().GetProperty(sorter.Property).GetValue(x, null);
                    b = y.GetType().GetProperty(sorter.Property).GetValue(y, null);
                    return CaseInsensitiveComparer.Default.Compare(a, b) * direction;
                });
            }
            //-- end sorting ------------------------------------------------------------


            //-- start paging ------------------------------------------------------------
            int limit = parameters.Limit;

            if ((parameters.Start + parameters.Limit) > data.Count)
            {
                limit = data.Count - parameters.Start;
            }

            List<object> rangeData = (parameters.Start < 0 || limit < 0) ? data : data.GetRange(parameters.Start, limit);
            //-- end paging ------------------------------------------------------------
            return rangeData;
        }
    }
}