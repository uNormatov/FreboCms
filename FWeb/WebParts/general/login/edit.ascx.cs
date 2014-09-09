using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FCore.Class;
using FCore.Collection;
using FCore.Helper;
using FDataProvider;
using FUIControls.PortalControl;

namespace FWeb.WebParts.general.login
{
    public partial class edit : FWebPartEdit
    {
        private const string Form = "Form";
        private const string RedrictUrl = "RedirictUrl";
        private const string ErrorMessage = "ErrorMessage";
        public edit()
            : base("", null)
        {

        }

        public edit(string properties, ErrorInfoList errorInfoList)
            : base(properties, errorInfoList)
        {

        }

        protected override void GetValues()
        {
            string form = GetControlValue(txtForm.ID);
            SetValue(Form, form);

            string errorMessage = GetControlValue(txtErrorMessage.ID);
            SetValue(ErrorMessage, errorMessage);

            string redrictUrl = GetControlValue(txtReturnUrl.ID);
            SetValue(RedrictUrl, redrictUrl);
        }

        protected override void EnsureControlsValue()
        {
            string form = ValidationHelper.GetString(GetValue(Form), string.Empty);
            if (!string.IsNullOrEmpty(form))
                txtForm.Text = form;

            string errorMessage = ValidationHelper.GetString(GetValue(ErrorMessage), string.Empty);
            if (!string.IsNullOrEmpty(errorMessage))
                txtErrorMessage.Text = errorMessage;

            string redrictUrl = ValidationHelper.GetString(GetValue(RedrictUrl), string.Empty);
            if (!string.IsNullOrEmpty(redrictUrl))
                txtReturnUrl.Text = redrictUrl;
        }
    }
}