using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iDAS.Core;
using iDAS.Services;
using iDAS.Items;
using Ext.Net;
using Ext.Net.MVC;
using iDAS.Web;
using System.IO;
using iDAS.Utilities;

namespace iDAS.Web.Areas.Customer.Controllers
{
    [SystemAuthorize(CheckAuthorize =false)]
    public class CustomerSurveyResultController : BaseController
    {
        private CustomerSurveyResultSV surveyResult = new CustomerSurveyResultSV();
        public ActionResult SurveyForm(int id = 0)
        {
            var data = new CustomerSurveyQuestionItem();
            data.CustomerID = id;
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Survey", Model = data };
        }
        public ActionResult LoadSurveyResult(string node, int groupID = 0, int customerID = 0)
        {
            NodeCollection nodes = new NodeCollection();
            int parentId = node == "root" ? 0 : System.Convert.ToInt32(node);
            var groups = surveyResult.GetByCustomer(parentId, customerID);
            var count = groups.Count();
            foreach (var group in groups)
            {
                nodes.Add(createNote(group, count));
            }
            return this.Content(nodes.ToJson());
        }
        private Node createNote(TreeCusotmerSurveyItem group, int count)
        {
            Node node = new Node();
            node.NodeID = group.ID.ToString();
            node.Text = group.Name.ToUpper();
            node.Leaf = group.Leaf;
            if (group.IsCategory)
            {
                node.Icon = Icon.Folder;
            }
            else
            {
                node.Icon = Icon.TagBlue;
            }
            node.CustomAttributes.Add(new ConfigItem { Name = "ID", Value = group.ID.ToString(), Mode = ParameterMode.Value });
            node.CustomAttributes.Add(new ConfigItem { Name = "ParentID", Value = group.ParentID.ToString(), Mode = ParameterMode.Value });
            node.CustomAttributes.Add(new ConfigItem { Name = "Name", Value = group.Name.ToString(), Mode = ParameterMode.Value });
            node.CustomAttributes.Add(new ConfigItem { Name = "IsCategory", Value = JSON.Serialize(group.IsCategory), Mode = ParameterMode.Value });
            node.CustomAttributes.Add(new ConfigItem { Name = "IsRoot", Value = JSON.Serialize(false), Mode = ParameterMode.Value });
            node.CustomAttributes.Add(new ConfigItem { Name = "IsSelect", Value = JSON.Serialize(group.IsSelect), Mode = ParameterMode.Value });
            node.CustomAttributes.Add(new ConfigItem { Name = "ResultID", Value = group.ResultID.ToString(), Mode = ParameterMode.Value });
            return node;
        }
        public ActionResult UpdateSurveyResult(string data, int CustomerID = 0)
        {
            var QuestionItem = Ext.Net.JSON.Deserialize<CustomerSurveyQuestionItem>(data);
            var ResultItem = new CustomerSurveyResultItem
            {
                ID = QuestionItem.ResultID,
                CustomerID = CustomerID,
                QuestionID = QuestionItem.ID,
                IsSelect = QuestionItem.IsSelect
            };
            try
            {
                if (ResultItem.ID == null & ResultItem.IsSelect == true)
                {
                    surveyResult.Insert(ResultItem, User.ID);

                }
                if (ResultItem.ID != null && ResultItem.IsSelect == false)
                {
                    surveyResult.Delete((int)ResultItem.ID);
                }
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.UpdateError, error: true);
            }
            X.GetCmp<Store>("StoreTreeSurveyResult").Reload();
            return this.Direct();
        }
    }
}
