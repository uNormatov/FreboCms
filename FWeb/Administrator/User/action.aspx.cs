using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using FCore.Class;
using FCore.Collection;
using FCore.Enum;
using FCore.Helper;
using FDataProvider;
using FUIControls.FormControl;
using FUIControls.Page;
using FUIControls.Settings;

namespace FWeb.Administrator.User
{
    public partial class action : FAdminEditPage
    {
        private UserProfileProvider _userProfileProvider;
        private RoleProfileProvider _roleProfileProvider;
        private ContentTypeProvider _contentTypeProvider;
        private GeneralConnection _generalConnection;

        private int ContentTypeId
        {
            get
            {
                object value = ViewState["__content_type_id"];
                if (value == null)
                    return 0;
                return ValidationHelper.GetInteger(value, 0);
            }
            set { ViewState["__content_type_id"] = value; }
        }

        private int ContentId
        {
            get
            {
                object value = ViewState["__content_id"];
                if (value == null)
                    return 0;
                return ValidationHelper.GetInteger(value, 0);
            }
            set { ViewState["__content_id"] = value; }
        }

        protected override void Init()
        {
            base.Init();

            if (_userProfileProvider == null)
                _userProfileProvider = new UserProfileProvider();
            if (_roleProfileProvider == null)
                _roleProfileProvider = new RoleProfileProvider();
            if (_generalConnection == null)
                _generalConnection = new GeneralConnection();
            if (_contentTypeProvider == null)
                _contentTypeProvider = new ContentTypeProvider();

            if (IsEdit)
            {
                Title = "Edit User | " + CoreSettings.CurrentSite.Name;
                ltlTitle.Text = "Edit";
            }
            else
            {
                Title = "New User | " + CoreSettings.CurrentSite.Name;
                ltlTitle.Text = "New";
            }

            if (IsEdit)
            {
                UserProfileInfo userProfileInfo = _userProfileProvider.SelectByUserId(Id, ErrorList);
                string[] selectedRoles = Roles.GetRolesForUser(Id);
                if (selectedRoles.Length > 0)
                {
                    RoleProfileInfo roleProfileInfo = _roleProfileProvider.SelectByRoleId(selectedRoles[0], ErrorList);
                    if (userProfileInfo != null && roleProfileInfo != null)
                    {
                        ContentTypeId = roleProfileInfo.ContentTypeId;
                        ContentId = userProfileInfo.ContentId;
                        pnlContentType.Visible = true;
                        ShowContentType();
                    }
                }
            }
        }

        protected override void Load()
        {
            CancelUrl = "/administrator/user/default.aspx";
            RedrictUrl = CancelUrl;
        }

        protected override void FillFields()
        {
            List<string> rols = new List<string>();
            rols.Add("Select");
            rols.AddRange(Roles.GetAllRoles());
            drlRoles.DataSource = rols;
            drlRoles.DataBind();
            drlRoles.SelectedIndex = 0;

            if (IsEdit)
            {
                MembershipUser user = Membership.GetUser(Id);
                txtUserName.Text = user.UserName;
                txtEmail.Text = user.Email;
                chbxIsActive.Checked = user.IsApproved;
                string[] selectedRoles = Roles.GetRolesForUser(user.UserName);
                drlRoles.SelectedValue = selectedRoles[0];
            }
        }

        protected override bool Update()
        {
            MembershipUser user = Membership.GetUser(Id);
            if (user != null)
            {
                user.Email = txtEmail.Text;
                user.IsApproved = chbxIsActive.Checked;
                Membership.UpdateUser(user);

                string[] selectedRoles = Roles.GetRolesForUser(user.UserName);
                if (selectedRoles != null && selectedRoles.Length > 0)
                    Roles.RemoveUserFromRoles(user.UserName, selectedRoles);
                Roles.AddUserToRole(user.UserName, drlRoles.SelectedValue);

                if (pnlContentType.Visible)
                {
                    ShowContentType();
                    mainForm.SaveData();
                    _userProfileProvider.DeleteUserProfile(user.UserName, ErrorList);
                    UserProfileInfo userProfileInfo = new UserProfileInfo();
                    userProfileInfo.ContentId = mainForm.ContentId;
                    userProfileInfo.ContentTypeId = mainForm.ContentTypeId;
                    userProfileInfo.UserId = user.UserName;
                    _userProfileProvider.Create(userProfileInfo, ErrorList);
                }
            }
            return CheckErrors();
        }

        protected override bool Insert()
        {
            if (!txtPassword.Text.Equals(txtComfirmPassword.Text))
            {
                ErrorInfo error = new ErrorInfo();
                error.Name = "User Create Error!";
                error.Message = "Password and Confirm Password mismatch!";
                error.Ok = false;
                ErrorList.Add(error);
                return CheckErrors();
            }

            MembershipCreateStatus status;
            Membership.CreateUser(txtUserName.Text, txtPassword.Text, txtEmail.Text, null, null, chbxIsActive.Checked, out status);
            if (CheckErrors(status))
            {
                Roles.AddUserToRole(txtUserName.Text, drlRoles.SelectedValue);

                if (pnlContentType.Visible)
                {
                    _userProfileProvider.DeleteUserProfile(txtUserName.Text, ErrorList);
                    ShowContentType();
                    mainForm.SaveData();

                    UserProfileInfo userProfileInfo = new UserProfileInfo();
                    userProfileInfo.ContentId = mainForm.ContentId;
                    userProfileInfo.ContentTypeId = mainForm.ContentTypeId;
                    userProfileInfo.UserId = txtUserName.Text;
                    _userProfileProvider.Create(userProfileInfo, ErrorList);
                }
            }
            return CheckErrors();
        }

        private bool CheckErrors(MembershipCreateStatus status)
        {
            if (status == MembershipCreateStatus.Success)
                return true;

            if (status == MembershipCreateStatus.InvalidEmail)
            {
                ErrorInfo errorInfo = new ErrorInfo();
                errorInfo.Name = "User Create Error!";
                errorInfo.Message = "Invalid Email!";
                errorInfo.Ok = false;
                ErrorList.Add(errorInfo);
            }

            if (status == MembershipCreateStatus.DuplicateEmail)
            {
                ErrorInfo errorInfo = new ErrorInfo();
                errorInfo.Name = "User Create Error!";
                errorInfo.Message = "Duplicate Email!";
                errorInfo.Ok = false;
                ErrorList.Add(errorInfo);
            }

            if (status == MembershipCreateStatus.DuplicateUserName)
            {
                ErrorInfo errorInfo = new ErrorInfo();
                errorInfo.Name = "User Create Error!";
                errorInfo.Message = "Duplicate UserName!";
                errorInfo.Ok = false;
                ErrorList.Add(errorInfo);
            }
            if (status == MembershipCreateStatus.InvalidPassword)
            {
                ErrorInfo errorInfo = new ErrorInfo();
                errorInfo.Name = "User Create Error!";
                errorInfo.Message = "Invalid Password!";
                errorInfo.Ok = false;
                ErrorList.Add(errorInfo);
            }

            return false;
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
        }

        protected override void PrintSuccess()
        {
            throw new NotImplementedException();
        }

        protected void drlRoles_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            ltlMessage.Text = string.Empty;
            if (drlRoles.SelectedIndex != 0 && drlRoles.SelectedIndex != -1)
            {
                RoleProfileInfo roleProfileInfo = _roleProfileProvider.SelectByRoleId(drlRoles.SelectedValue, ErrorList);
                if (roleProfileInfo != null)
                {
                    pnlContentType.Visible = true;
                    ContentTypeId = ValidationHelper.GetInteger(roleProfileInfo.ContentTypeId, 0);
                }
                else
                {
                    pnlContentType.Visible = false;
                    ContentTypeId = 0;
                }
            }
            ShowContentType();
        }

        private void ShowContentType()
        {
            mainForm.ContentTypeId = ContentTypeId;
            mainForm.FormMode = IsEdit ? FormActionMode.Edit : FormActionMode.Insert;
            if (ContentTypeId != 0)
                mainForm.LoadData();
        }

        protected override void ValidateForm()
        {
            if (!IsEdit)
            {
                if (string.IsNullOrEmpty(txtUserName.Text))
                {
                    ErrorList.Add(new ErrorInfo()
                                      {
                                          Message = "UserName is required"
                                      });
                }
                if (string.IsNullOrEmpty(txtPassword.Text))
                {
                    ErrorList.Add(new ErrorInfo()
                    {
                        Message = "Password is required"
                    });
                }
                if (string.IsNullOrEmpty(txtComfirmPassword.Text))
                {
                    ErrorList.Add(new ErrorInfo()
                    {
                        Message = "Confirm Password is required"
                    });
                }
                if (txtComfirmPassword.Text != txtPassword.Text)
                {
                    ErrorList.Add(new ErrorInfo()
                    {
                        Message = "Confirm Password and Password is not match"
                    });
                }
            }
            else
            {
                if (string.IsNullOrEmpty(txtEmail.Text))
                {
                    ErrorList.Add(new ErrorInfo()
                    {
                        Message = "Email is required"
                    });
                }
            }
            ShowContentType();
            mainForm.Validate();
            ErrorList.AddRange(mainForm.ErrorInfoList);
        }
    }
}