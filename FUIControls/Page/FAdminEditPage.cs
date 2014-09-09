using FCore.Helper;

namespace FUIControls.Page
{
    public abstract class FAdminEditPage : FAdminPage
    {
        protected string Id
        {
            get { return Request.QueryString["id"]; }
        }

        protected bool IsEdit
        {
            get { return !string.IsNullOrEmpty(Id); }
        }

        protected bool IsByName
        {
            get { return ValidationHelper.GetString(Request.QueryString["byname"], "false") == "true"; }
        }

        protected string RedrictUrl { get; set; }

        protected string CancelUrl { get; set; }

        protected override void ParsePost()
        {
            string action = Request.Form["form-action"];
            bool isSaved = false;
            if (!string.IsNullOrEmpty(action))
            {
                if (action.Equals("save") || action.Equals("publish") || action.Equals("unpublish"))
                {
                    ValidateForm();
                    if (CheckErrors())
                    {
                        if (!string.IsNullOrEmpty(Id))
                        {
                            if (action.Equals("save"))
                                isSaved = Update();
                            else if (action.Equals("publish"))
                                isSaved = Publish();
                            else if (action.Equals("unpublish"))
                                isSaved = Unpublish();
                        }
                        else
                        {
                            if (action.Equals("save"))
                                isSaved = Insert();
                            else if (action.Equals("publish"))
                                isSaved = Publish();
                            else if (action.Equals("unpublish"))
                                isSaved = Unpublish();
                        }
                        if (isSaved)
                            if (!RedrictUrl.Contains("?"))
                                Response.Redirect(RedrictUrl + "?status=ok");
                            else
                                Response.Redirect(RedrictUrl + "&status=ok");
                    }
                }
                else if (action.Equals("cancel"))
                {
                    Response.Redirect(CancelUrl);
                }
            }
            else
            {
                if (!IsPostBack)
                    FillFields();
            }
        }

        protected abstract void FillFields();

        protected abstract bool Update();

        protected virtual bool Publish()
        {
            return true;
        }

        protected virtual bool Unpublish()
        {
            return true;
        }

        protected abstract bool Insert();

        protected virtual void ValidateForm()
        {
        }
    }
}
