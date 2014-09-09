using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FCore.Class;
using FCore.Collection;
using FCore.Enum;
using FCore.Helper;
using FDataProvider;
using FUIControls.PortalControl;
using FUIControls.Settings;

namespace FWeb.WebParts.general.form
{
    public partial class webpart : FWebPart
    {
        private const string ContentTypeId = "ContentTypeId";
        private const string FormId = "FormId";
        private const string QueryName = "QueryName";
        private const string QueryParameters = "QueryParameters";
        private const string ButtonText = "ButtonText";
        private const string CancelButtonText = "CancelButtonText";

        private FormProvider _formProvider;


        protected override void LoadWebPart()
        {
            if (_formProvider == null)
                _formProvider = new FormProvider();

            string contentType = GetProperty(ContentTypeId);
            string formId = GetProperty(FormId);
            string queryName = GetProperty(QueryName);
            string queryParams = GetProperty(QueryParameters);
            string buttonText = GetProperty(ButtonText);
            string cancelButtonText = GetProperty(CancelButtonText);

            FormInfo formInfo = _formProvider.Select(ValidationHelper.GetInteger(formId, 0), new ErrorInfoList());

            DataTable dt = null;
            using (GeneralConnection generalConnection = new GeneralConnection())
            {
                bool ok;
                object[,] pars = GetSelectParamaters(queryParams, out ok);
                if (ok)
                    dt = generalConnection.ExecuteDataTableQuery(queryName, pars, QueryType.SqlQuery, new ErrorInfoList());
            }

            frmMain.ContentTypeId = ValidationHelper.GetInteger(contentType, 0);
            frmMain.CustomForm = formInfo;
            frmMain.SaveButtonText = buttonText;
            frmMain.CancelButtonText = cancelButtonText;
            if (dt != null && dt.Rows.Count > 0)
            {
                frmMain.DataRow = dt.Rows[0];
                frmMain.FormMode = FormActionMode.Edit;
                frmMain.LoadData();
                frmMain.OnAfterUpdate += new FUIControls.FormControl.MainForm.OnAfterUpdateEventHandler(frmMain_OnAfterUpdate);
            }
            else
            {
                frmMain.FormMode = FormActionMode.Insert;
                frmMain.LoadData();
                string languge = "all";
                if (CoreSettings.CurrentSite.IsMultilanguage)
                    languge = GetCurrentLanguage();
                if (string.IsNullOrEmpty(languge))
                    languge = "all";
                frmMain.ContentTypeModel.SetValue("Language", languge);
                frmMain.OnAfterSave += new FUIControls.FormControl.MainForm.OnAfterSaveEventHandler(frmMain_OnAfterSave);
            }
        }

        void frmMain_OnAfterUpdate()
        {
            Response.Redirect(BuildUrl(GetProperty("ReturnUrl")));
        }

        private void frmMain_OnAfterSave()
        {
            Response.Redirect(BuildUrl(GetProperty("ReturnUrl")));
        }
    }

}