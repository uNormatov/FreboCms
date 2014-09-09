using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FCore.Class;
using FDataProvider;
using FUIControls.Page;

namespace FWeb.Administrator.Pages
{
    public partial class QueryEditor : FAdminEditPage
    {
        private ContentTypeProvider _contentTypeProvider;
        private QueryProvider _queryProvider;

        protected override void Init()
        {
            base.Init();
            if (_contentTypeProvider == null)
                _contentTypeProvider = new ContentTypeProvider();
            if (_queryProvider == null)
                _queryProvider = new QueryProvider();
        }


        protected override void PrintErrors()
        {
            throw new NotImplementedException();
        }

        protected override void PrintSuccess()
        {

        }

        protected override void FillFields()
        {
            if (IsEdit)
            {
                QueryInfo queryInfo = _queryProvider.SelectByName(Id, ErrorList);
                txtName.Text = queryInfo.Name;
                txtQuery.Text = queryInfo.Text;
            }
        }

        protected override bool Update()
        {
            QueryInfo queryInfo = _queryProvider.SelectByName(Id, ErrorList);
            queryInfo.Text = txtQuery.Text;
            _queryProvider.Update(queryInfo, ErrorList);
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