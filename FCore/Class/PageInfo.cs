
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using FCore.Helper;

namespace FCore.Class
{
    public class PageInfo : ClassInfo
    {
        public string SeoTemplate { get; set; }
        public string Title { get; set; }
        public string BreadCrumbTitle { get; set; }
        public int ParentId { get; set; }
        public int PageLayoutId { get; set; }
        public string PageLayoutName { get; set; }
        public int SiteLayoutId { get; set; }
        public string SiteLayoutName { get; set; }
        public bool IsRequiresAuthentication { get; set; }
        public string RedirectNoAuthenticated { get; set; }
        public string RedirectNoPermission { get; set; }

        public string MetaTitle { get; set; }

        public string MetadataDescription { get; set; }
        public string MetadataKeywords { get; set; }
        public string MetaQueryName { get; set; }
        public string MetaQueryParameters { get; set; }
        public string ContentRights { get; set; }
        public string MetaImage { get; set; }

        public DateTime MetaUpdateDate { get; set; }
        public string SeoUrl { get; set; }

        public string FullUrl { get; set; }

        public string QueryString { get; set; }
        public string Domain { get; set; }
        public bool IsStatic { get; set; }
        public List<BlockInfo> PageBlocks { get; set; }
        public List<BlockInfo> SiteBlocks { get; set; }
        public List<BlockInfo> PageLayoutBlocks { get; set; }
        public List<string> AvailableRoles { get; set; }


        public PageInfo()
        {
            IsStatic = false;
        }

        public PageInfo(DataRow dataRow)
        {
            if (dataRow.Table.Columns.IndexOf("Id") > -1)
                Id = ValidationHelper.GetInteger(dataRow["Id"], 0);
            if (dataRow.Table.Columns.IndexOf("Name") > -1)
                Name = ValidationHelper.GetString(dataRow["Name"], "");
            if (dataRow.Table.Columns.IndexOf("Title") > -1)
                Title = ValidationHelper.GetString(dataRow["Title"], "");
            if (dataRow.Table.Columns.IndexOf("BreadCrumbTitle") > -1)
                BreadCrumbTitle = ValidationHelper.GetString(dataRow["BreadCrumbTitle"], "");
            if (dataRow.Table.Columns.IndexOf("Description") > -1)
                Description = ValidationHelper.GetString(dataRow["Description"], "");
            if (dataRow.Table.Columns.IndexOf("SeoTemplate") > -1)
                SeoTemplate = ValidationHelper.GetString(dataRow["SeoTemplate"], "");
            if (dataRow.Table.Columns.IndexOf("ParentId") > -1)
                ParentId = ValidationHelper.GetInteger(dataRow["ParentId"], 0);
            if (dataRow.Table.Columns.IndexOf("PageLayoutId") > -1)
                PageLayoutId = ValidationHelper.GetInteger(dataRow["PageLayoutId"], 0);
            if (dataRow.Table.Columns.IndexOf("SiteLayoutId") > -1)
                SiteLayoutId = ValidationHelper.GetInteger(dataRow["SiteLayoutId"], 0);
            if (dataRow.Table.Columns.IndexOf("PageLayoutName") > -1)
                PageLayoutName = ValidationHelper.GetString(dataRow["PageLayoutName"], "");
            if (dataRow.Table.Columns.IndexOf("SiteLayoutName") > -1)
                SiteLayoutName = ValidationHelper.GetString(dataRow["SiteLayoutName"], "");

            if (dataRow.Table.Columns.IndexOf("IsRequiresAuthentication") > -1)
                IsRequiresAuthentication = ValidationHelper.GetBoolean(dataRow["IsRequiresAuthentication"], false);
            if (dataRow.Table.Columns.IndexOf("RedirectNoAuthenticated") > -1)
                RedirectNoAuthenticated = ValidationHelper.GetString(dataRow["RedirectNoAuthenticated"], "");
            if (dataRow.Table.Columns.IndexOf("RedirectNoAuthenticated") > -1)
                RedirectNoPermission = ValidationHelper.GetString(dataRow["RedirectNoPermission"], "");
            if (dataRow.Table.Columns.IndexOf("MetadataDescription") > -1)
                MetadataDescription = ValidationHelper.GetString(dataRow["MetadataDescription"], "");
            if (dataRow.Table.Columns.IndexOf("MetadataKeywords") > -1)
                MetadataKeywords = ValidationHelper.GetString(dataRow["MetadataKeywords"], "");
            if (dataRow.Table.Columns.IndexOf("MetaQueryName") > -1)
                MetaQueryName = ValidationHelper.GetString(dataRow["MetaQueryName"], "");
            if (dataRow.Table.Columns.IndexOf("MetaQueryParameters") > -1)
                MetaQueryParameters = ValidationHelper.GetString(dataRow["MetaQueryParameters"], "");
            if (dataRow.Table.Columns.IndexOf("MetadataKeywords") > -1)
                MetadataKeywords = ValidationHelper.GetString(dataRow["MetadataKeywords"], "");
            if (dataRow.Table.Columns.IndexOf("ContentRights") > -1)
                ContentRights = ValidationHelper.GetString(dataRow["ContentRights"], "");
            if (dataRow.Table.Columns.IndexOf("IsDeleted") > -1)
                IsDeleted = ValidationHelper.GetBoolean(dataRow["IsDeleted"], false);
            if (dataRow.Table.Columns.IndexOf("IsStatic") > -1)
                IsStatic = ValidationHelper.GetBoolean(dataRow["IsStatic"], false);
            if (dataRow.Table.Columns.IndexOf("CreatedBy") > -1)
                CreatedBy = ValidationHelper.GetString(dataRow["CreatedBy"], "");
            if (dataRow.Table.Columns.IndexOf("CreatedDate") > -1)
                CreatedDate = ValidationHelper.GetDateTime(dataRow["CreatedBy"], DateTime.Today);
            if (dataRow.Table.Columns.IndexOf("ModifiedBy") > -1)
                ModifiedBy = ValidationHelper.GetString(dataRow["CreatedBy"], "");
            if (dataRow.Table.Columns.IndexOf("ModifiedDate") > -1)
                CreatedDate = ValidationHelper.GetDateTime(dataRow["ModifiedDate"], DateTime.Today);
        }

    }
}

