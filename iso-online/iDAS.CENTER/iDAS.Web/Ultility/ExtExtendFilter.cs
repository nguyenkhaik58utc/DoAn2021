using Ext.Net;
using iDAS.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iDAS.Web.ExtExtend
{
    public class ExtExtendFilter
    {
        public static ModelFilter ConvertToFilter(StoreRequestParameters parameters)
        {
            ModelFilter filter = getFilterHeader();
            if (filter == null)
            {
                filter = new ModelFilter();
                FilterConditions storeFilter = parameters.GridFilters;

                if (storeFilter != null)
                {
                    foreach (FilterCondition condition in storeFilter.Conditions)
                    {
                        ConditionFilter item = new ConditionFilter();
                        item.Field = condition.Field;
                        Comparison comparison = condition.Comparison;
                        switch (comparison)
                        {
                            case Comparison.Eq: item.Comparison = "="; break;
                            case Comparison.Gt: item.Comparison = ">"; break;
                            case Comparison.Lt: item.Comparison = "<"; break;
                        }
                        switch (condition.Type)
                        {
                            case FilterType.Boolean:
                                item.Type = TypeFilter.Boolean;
                                item.Value = condition.Value<bool>();
                                break;
                            case FilterType.Date:
                                item.Type = TypeFilter.Date;
                                item.Value = condition.Value<DateTime>();
                                break;
                            case FilterType.List:
                                item.Type = TypeFilter.List;
                                item.Value = condition.List;
                                break;
                            case FilterType.Numeric:
                                var value = condition.Value<string>();
                                if (value.Contains(",") || value.Contains("."))
                                {
                                    item.Type = TypeFilter.Double;
                                    item.Value = condition.Value<double>();
                                }
                                else
                                {
                                    item.Type = TypeFilter.Int;
                                    item.Value = condition.Value<int>();
                                }
                                break;
                            case FilterType.String:
                                item.Type = TypeFilter.String;
                                item.Value = condition.Value<string>();
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                        filter.Conditions.Add(item);
                    }
                }
            }
            filter.SortName = "CreateAt";
            filter.SortDirection = "desc";
            if (parameters.Sort.Length > 0)
            {
                DataSorter sorter = parameters.Sort[0];
                filter.SortDirection = sorter.Direction == Ext.Net.SortDirection.DESC ? "desc" : "asc";
                filter.SortName = sorter.Property;
            }

            filter.PageIndex = parameters.Page;
            filter.PageSize = parameters.Limit;

            return filter;
        }

        private static ModelFilter getFilterHeader()
        {
            ModelFilter filter = new ModelFilter();
            var request = HttpContext.Current.Request["filterheader"];
            FilterHeaderConditions storeFilter = new FilterHeaderConditions(request);
            if (storeFilter.Conditions.Count <= 0) return null;
            foreach (FilterHeaderCondition condition in storeFilter.Conditions)
            {
                ConditionFilter item = new ConditionFilter();
                item.Field = condition.DataIndex;
                item.Comparison = condition.Operator;
                switch (condition.Operator)
                {
                    case "+": item.Comparison = "="; break;
                }
                switch (condition.Type)
                {
                    case FilterType.Boolean:
                        item.Type = TypeFilter.Boolean;
                        item.Value = condition.Value<bool>();
                        break;
                    case FilterType.Date:
                        item.Type = TypeFilter.Date;
                        item.Value = condition.Value<DateTime>();
                        break;
                    //case FilterType.List:
                    //    item.Type = TypeFilter.List;
                    //    item.Value = condition.List;
                    //    break;
                    case FilterType.Numeric:
                        var value = condition.Value<string>();
                        if (value.Contains(",") || value.Contains("."))
                        {
                            item.Type = TypeFilter.Double;
                            item.Value = condition.Value<double>();
                        }
                        else
                        {
                            item.Type = TypeFilter.Int;
                            item.Value = condition.Value<int>();
                        }
                        break;
                    case FilterType.String:
                        item.Type = TypeFilter.String;
                        item.Value = condition.Value<string>();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                filter.Conditions.Add(item);
            }
            return filter;
        }
    }
}