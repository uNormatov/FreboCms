using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FCore.Class;
using FCore.Class.Poll;
using FCore.Helper;
using FDataProvider;
using FUIControls.Page;
using FUIControls.Settings;

namespace FWeb.Administrator.Poll
{
    public partial class pollaction : FAdminEditPage
    {
        private PollProvider _pollProvider;

        protected override void Init()
        {
            base.Init();

            if (_pollProvider == null)
                _pollProvider = new PollProvider();


            if (IsEdit)
            {
                Title = "Edit Poll | " + CoreSettings.CurrentSite.Name;
                ltlTitle.Text = "Edit Poll";
            }
            else
            {
                Title = "New Poll | " + CoreSettings.CurrentSite.Name;
                ltlTitle.Text = "New Poll";
            }
        }

        protected override void Load()
        {
            CancelUrl = "/administrator/poll";
            RedrictUrl = CancelUrl;
        }

        protected override void FillFields()
        {
            if (IsEdit)
            {
                PollInfo pollInfo = _pollProvider.SelectById(ValidationHelper.GetInteger(Id, 0), ErrorList);
                if (pollInfo != null)
                {
                    txtQuestion.Text = pollInfo.Question;
                    rbtBlockTypes.SelectedValue = pollInfo.BlockMode.ToString();
                    chbxIsActive.Checked = pollInfo.IsActive;
                }
            }
        }

        protected override bool Update()
        {
            PollInfo pollInfo = _pollProvider.SelectById(ValidationHelper.GetInteger(Id, 0), ErrorList);
            if (pollInfo != null)
            {
                pollInfo.Question = txtQuestion.Text;
                pollInfo.BlockMode = ValidationHelper.GetInteger(rbtBlockTypes.SelectedValue, 1);
                pollInfo.IsActive = chbxIsActive.Checked;
                _pollProvider.UpdatePoll(pollInfo, ErrorList);
            }
            return CheckErrors();
        }

        protected override bool Insert()
        {
            PollInfo pollInfo = new PollInfo();
            pollInfo.Question = txtQuestion.Text;
            pollInfo.BlockMode = ValidationHelper.GetInteger(rbtBlockTypes.SelectedValue, 1);
            pollInfo.IsActive = chbxIsActive.Checked;
            _pollProvider.CreatePoll(pollInfo, ErrorList);
            return CheckErrors();
        }

        protected override void ValidateForm()
        {
            if (string.IsNullOrEmpty(txtQuestion.Text))
            {
                ErrorList.Add(new ErrorInfo()
                                  {
                                      Name = "Poll details",
                                      Message = "Question is required"
                                  });
            }
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
            throw new NotImplementedException();
        }
    }
}