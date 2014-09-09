using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FCore.Class;
using FCore.Helper;
using FDataProvider;
using FUIControls.Page;

namespace FWeb.Administrator.Pages
{
    public partial class TransformationEditor : FAdminEditPage
    {
        private ContentTypeProvider _contentTypeProvider;
        private TransformationProvider _transformationProvider;

        protected override void Init()
        {
            base.Init();

            if (_contentTypeProvider == null)
                _contentTypeProvider = new ContentTypeProvider();
            if (_transformationProvider == null)
                _transformationProvider = new TransformationProvider();
        }

        protected override void PrintErrors()
        {

        }

        protected override void PrintSuccess()
        {

        }

        protected override void FillFields()
        {
            if (IsEdit)
            {
                TransformationInfo transformationInfo = _transformationProvider.SelectByName(Id, ErrorList);
                txtName.Text = transformationInfo.Name;
                txtQuery.Text = transformationInfo.Text.ToHtmlDecode();
            }
        }

        protected override bool Update()
        {
            TransformationInfo transformationInfo = _transformationProvider.SelectByName(Id, ErrorList);
            transformationInfo.Text = txtQuery.Text.ToHtmlEncode();
            _transformationProvider.Update(transformationInfo, ErrorList);
            return CheckErrors();
        }

        protected override bool Insert()
        {
            throw new NotImplementedException();
        }

        protected void btnSave_OnClick(object sender, EventArgs e)
        {
            Update();
            ClientScript.RegisterStartupScript(Page.GetType(), "closepopup", "<script>closePopup()</script>");
        }
    }
}