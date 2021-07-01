﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iDAS.Web.Areas.Department.Models.TitleMenuRoleV3
{
    public class v3OrgMenuDTO
    {
        public int ID { get; set; }
        public string SystemCode { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int ParentID { get; set; }
        public string ParentCode { get; set; }
        public int ModuleID { get; set; }
        public string ModuleCode { get; set; }
        public string ModuleName { get; set; }
        public string ModuleIcon { get; set; }
        public bool IsActive { get; set; }
        public bool IsShow { get; set; }
        public bool IsDelete { get; set; }
        public bool IsGroup { get; set; }
        public int Position { get; set; }
        public string Url { get; set; }
        public string Icon { get; set; }
        public string Description { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}