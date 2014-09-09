using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using FCore.Class;
using FCore.Helper;
using FDataProvider;
using FUIControls.FormControl;
using FUIControls.Page;

namespace FWeb.Administrator.User
{
    public partial class _default : FAdminPage
    {
        private UserProfileProvider _userProfileProvider;
        private ContentTypeProvider _contentTypeProvider;
        private RoleProfileProvider _roleProfileProvider;
        protected override void Init()
        {
            base.Init();
            if (_userProfileProvider == null)
                _userProfileProvider = new UserProfileProvider();
            if (_contentTypeProvider == null)
                _contentTypeProvider = new ContentTypeProvider();
            if (_roleProfileProvider == null)
                _roleProfileProvider = new RoleProfileProvider();
        }

        protected override void FillGrid()
        {
            int totalCount = 0;
            MembershipUserCollection users = Membership.GetAllUsers(PageIndex - 1, PageSize, out totalCount);
            List<MembershipUser> list = new List<MembershipUser>();
            if (SortOrder == "ASC")
            {
                if (SortBy == "Name")
                {
                    list = users.Cast<MembershipUser>().OrderBy(x => x.UserName).ToList();
                }
                else if (SortBy == "IsOnline")
                {
                    list = users.Cast<MembershipUser>().OrderBy(x => x.IsOnline).ToList();
                }
                else if (SortBy == "LoginDate")
                {
                    list = users.Cast<MembershipUser>().OrderBy(x => x.LastLoginDate).ToList();
                }
                else if (SortBy == "Email")
                {
                    list = users.Cast<MembershipUser>().OrderBy(x => x.Email).ToList();
                }
                else
                {
                    list = users.Cast<MembershipUser>().OrderBy(x => x.IsApproved).ToList();
                }
            }
            else
            {
                if (SortBy == "Name")
                {
                    list = users.Cast<MembershipUser>().OrderByDescending(x => x.UserName).ToList();
                }
                else if (SortBy == "IsOnline")
                {
                    list = users.Cast<MembershipUser>().OrderByDescending(x => x.IsOnline).ToList();
                }
                else if (SortBy == "LoginDate")
                {
                    list = users.Cast<MembershipUser>().OrderByDescending(x => x.LastLoginDate).ToList();
                }
                else if (SortBy == "Email")
                {
                    list = users.Cast<MembershipUser>().OrderByDescending(x => x.Email).ToList();
                }
                else
                {
                    list = users.Cast<MembershipUser>().OrderByDescending(x => x.IsApproved).ToList();
                }
            }
            TotalCount = totalCount;
            rptList.DataSource = list;
            rptList.DataBind();
        }

        protected override void ParsePost()
        {
            string action = Request.Form["form-action"];
            if (!string.IsNullOrEmpty(action))
            {
                if (action.Equals("add"))
                {
                    Response.Redirect("/administrator/user/action.aspx?type=entry");
                }
                else if (action.Equals("edit"))
                {
                    string checkboxs = Request.Form["chbxRow"];
                    if (!string.IsNullOrEmpty(checkboxs))
                    {
                        string[] temps = checkboxs.Split(',');
                        Response.Redirect("/administrator/user/action.aspx?type=entry&id=" + temps[0]);
                    }
                }
                else
                {
                    string checkboxs = Request.Form["chbxRow"];
                    if (!string.IsNullOrEmpty(checkboxs))
                    {
                        string[] temps = checkboxs.Split(',');
                        DeleteRows(temps);
                    }
                }
            }
            base.ParsePost();
        }

        protected void drlDisplay_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList drl = sender as DropDownList;
            Response.Redirect("default.aspx?size=" + drl.SelectedValue);
            FillGrid();
        }

        protected void rptList_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Header)
            {
                Image nameImage = e.Item.FindControl("imgSortByName") as Image;
                Image lastLoginImage = e.Item.FindControl("imgLastLogin") as Image;
                Image isOnlineImage = e.Item.FindControl("imgIsOnline") as Image;
                Image isApprovedImage = e.Item.FindControl("imgIsApproved") as Image;
                Image emailImage = e.Item.FindControl("imgEmail") as Image;

                string image = "/content/css/images/menu/sort-asc.png";
                if (SortOrder == "DESC")
                    image = "/content/css/images/menu/sort-desc.png";

                if (SortBy == "Name")
                {
                    nameImage.ImageUrl = image;
                    lastLoginImage.ImageUrl = "";
                    isOnlineImage.ImageUrl = "";
                    isApprovedImage.ImageUrl = "";
                    emailImage.ImageUrl = "";
                }
                else if (SortBy == "LoginDate")
                {
                    nameImage.ImageUrl = "";
                    lastLoginImage.ImageUrl = image;
                    isOnlineImage.ImageUrl = "";
                    isApprovedImage.ImageUrl = "";
                    emailImage.ImageUrl = "";
                }
                else if (SortBy == "IsOnline")
                {
                    nameImage.ImageUrl = "";
                    lastLoginImage.ImageUrl = "";
                    isOnlineImage.ImageUrl = image;
                    isApprovedImage.ImageUrl = "";
                    emailImage.ImageUrl = "";
                }
                else if (SortBy == "IsApproved")
                {
                    nameImage.ImageUrl = "";
                    lastLoginImage.ImageUrl = "";
                    isOnlineImage.ImageUrl = "";
                    isApprovedImage.ImageUrl = image;
                    emailImage.ImageUrl = "";
                }
                else if (SortBy == "Email")
                {
                    nameImage.ImageUrl = "";
                    lastLoginImage.ImageUrl = "";
                    isOnlineImage.ImageUrl = "";
                    isApprovedImage.ImageUrl = "";
                    emailImage.ImageUrl = image;
                }

            }
            else if (e.Item.ItemType == ListItemType.Footer)
            {
                DropDownList drlDisplay = e.Item.FindControl("drlDisplay") as DropDownList;
                for (int i = 0; i < drlDisplay.Items.Count; i++)
                {
                    if (drlDisplay.Items[i].Value.Equals(PageSize.ToString()))
                    {
                        drlDisplay.SelectedIndex = i;
                        break;
                    }
                }
            }
        }

        protected void lnkSortByName_Click(object sender, EventArgs e)
        {
            if (SortBy == "Name")
            {
                if (SortOrder == "ASC")
                    SortOrder = "DESC";
                else
                    SortOrder = "ASC";
            }
            else
            {
                SortBy = "Name";
                SortOrder = "ASC";
            }

            FillGrid();
        }

        protected void lnkLastLoginDate_OnClick(object sender, EventArgs e)
        {
            if (SortBy == "LastLogin")
            {
                if (SortOrder == "ASC")
                    SortOrder = "DESC";
                else
                    SortOrder = "ASC";
            }
            else
            {
                SortBy = "LastLogin";
                SortOrder = "ASC";
            }

            FillGrid();
        }

        protected void lnkIsOnline_OnClick(object sender, EventArgs e)
        {
            if (SortBy == "IsOnline")
            {
                if (SortOrder == "ASC")
                    SortOrder = "DESC";
                else
                    SortOrder = "ASC";
            }
            else
            {
                SortBy = "IsOnline";
                SortOrder = "ASC";
            }

            FillGrid();
        }

        protected void lnkIsApproved_OnClick(object sender, EventArgs e)
        {
            if (SortBy == "IsApproved")
            {
                if (SortOrder == "ASC")
                    SortOrder = "DESC";
                else
                    SortOrder = "ASC";
            }
            else
            {
                SortBy = "IsApproved";
                SortOrder = "ASC";
            }

            FillGrid();
        }

        protected void lnkEmail_OnClick(object sender, EventArgs e)
        {
            if (SortBy == "Email")
            {
                if (SortOrder == "ASC")
                    SortOrder = "DESC";
                else
                    SortOrder = "ASC";
            }
            else
            {
                SortBy = "Email";
                SortOrder = "ASC";
            }

            FillGrid();
        }

        private void DeleteRows(string[] ids)
        {
            foreach (string item in ids)
            {
                UserProfileInfo userProfileInfo = _userProfileProvider.SelectByUserId(item, ErrorList);
                string[] selectedRoles = Roles.GetRolesForUser(item);
                if (selectedRoles.Length > 0)
                {
                    RoleProfileInfo roleProfileInfo = _roleProfileProvider.SelectByRoleId(selectedRoles[0], ErrorList);
                    if (userProfileInfo != null && roleProfileInfo != null)
                    {
                        var generalConnection = new GeneralConnection();
                        var model = new ContentTypeModel(roleProfileInfo.ContentTypeId, userProfileInfo.ContentId, generalConnection, _contentTypeProvider, ErrorList);
                        model.Delete();
                        _userProfileProvider.DeleteUserProfile(item, ErrorList);
                    }
                }

                if (!Membership.DeleteUser(item))
                {
                    PrintErrors();
                    break;
                }
            }
            CacheHelper.ClearUserProfileCache();
            FillGrid();
        }

        protected override void PrintErrors()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<dl id=\"system-message\">");
            builder.Append("<dt class=\"message\">Message</dt><dd class=\"error\">");
            builder.Append("<ul>");
            foreach (ErrorInfo error in ErrorList)
            {
                builder.AppendFormat("<li>{0} - {1}</li>", error.Name, error.Message);
            }
            builder.Append("</ul></dd></dl>");
            ltlMessage.Text = builder.ToString();
            ErrorList.Clear();
        }

        protected override void PrintSuccess()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<dl id=\"system-message\">");
            builder.Append("<dt class=\"message\">Message</dt><dd class=\"message message\">");
            builder.Append("<ul>");
            builder.Append("<li>User successfully created</li>");
            builder.Append("</ul></dd></dl>");
            ltlMessage.Text = builder.ToString();
        }


    }
}