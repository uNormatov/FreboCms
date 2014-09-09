using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.UI;
using FCore.Collection;
using FCore.Helper;

namespace FUIControls.Page
{
    public abstract class FAdminPage : FPage
    {
        protected string SortBy
        {
            get
            {
                if (ViewState["_orderBy"] == null)
                    return "Id";
                return ViewState["_orderBy"].ToString();
            }
            set
            {
                ViewState["_orderBy"] = value;
            }
        }

        protected virtual string SortOrder
        {
            get
            {
                if (ViewState["_sortOrder"] == null)
                    return "DESC";
                return ViewState["_sortOrder"].ToString();
            }
            set
            {
                ViewState["_sortOrder"] = value;
            }
        }

        protected int PageSize
        {
            get { return ValidationHelper.GetInteger(Request.QueryString["size"], 10); }
        }

        protected int PageIndex
        {
            get
            {
                return ValidationHelper.GetInteger(Request.QueryString["page"], 1);
            }

        }

        protected int TotalCount
        {
            get
            {
                if (ViewState["_totalCount"] == null)
                    return 1;
                return ValidationHelper.GetInteger(ViewState["_totalCount"], 1);
            }
            set
            {
                ViewState["_totalCount"] = value;
            }
        }

        protected string SearchKeyword { get { return Server.UrlDecode(ValidationHelper.GetString(Request.QueryString["keyword"], string.Empty)); } }

        protected void Page_Load(object sender, EventArgs e)
        {

            Load();

            if (!IsPostBack)
            {
                FillGrid();
                CheckErrors();
            }

            ParsePost();
        }

        protected virtual void ParsePost()
        {
            if (!string.IsNullOrEmpty(ValidationHelper.GetString(Request.QueryString["status"], string.Empty)))
                PrintSuccess();
        }

        protected new virtual void Load()
        {

        }

        protected virtual void FillGrid()
        {
        }

        protected bool CheckErrors()
        {
            if (ErrorList.HasError())
            {
                PrintErrors();
                return false;
            }
            return true;
        }

        protected abstract void PrintErrors();

        protected abstract void PrintSuccess();
    }
}
