using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using FCore.Class;
using FCore.Constant;

namespace FUIControls.FormControl
{
    [ToolboxData("<fr:CaptchaControl runat=\"server\" ID=\"CaptchaControl1\" />")]
    public sealed class CaptchaControl : AbsractBasicControl
    {
        private const int DefaultWidth = 150;
        private const int DefaultHeight = 40;
        private const int DefaultLength = 6;

        private TextBox _txtCaptcha;
        private Image _imgCaptcha;
        private HiddenField _hiddenSessionId;

        private string SessionId { get; set; }

        #region Constructors

        public CaptchaControl()
            : this(null, null)
        {
        }

        public CaptchaControl(string controlId, string options)
        {
            if (!string.IsNullOrEmpty(controlId))
                ID = controlId;
            if (!string.IsNullOrEmpty(options))
                SetOptions(options);
        }

        #endregion

        protected override void EnsureControls()
        {
            if (string.IsNullOrEmpty(SessionId))
                SessionId = Guid.NewGuid().ToString();
            if (_txtCaptcha == null)
            {
                _txtCaptcha = new TextBox();
                _txtCaptcha.ID = string.Format("txtCaptcha{0}", ID);
            }
            if (_imgCaptcha == null)
            {
                _imgCaptcha = new Image();
                _imgCaptcha.ImageUrl = string.Format("/captcha.ashx?sessionid={0}&width={1}&height={2}&length={3}", SessionId, DefaultWidth, DefaultHeight, DefaultLength);
            }

            if (_hiddenSessionId == null)
            {
                _hiddenSessionId = new HiddenField();
                _hiddenSessionId.ID = string.Format("_hiddenSessionId{0}", ID);
            }
            _hiddenSessionId.Value = SessionId;
        }

        protected override void CreateChildControls()
        {
            string captchaLabel = GetResource("captchaLabel");
            if (string.IsNullOrEmpty(captchaLabel) || captchaLabel == "captchaLabel")
                captchaLabel = "What code is in image?";

            WebControl div = new WebControl(HtmlTextWriterTag.Div);
            div.CssClass = "control-group";

            Label label = new Label();
            label.CssClass = "control-label";
            label.Text = captchaLabel;

            div.Controls.Add(label);

            WebControl controlDiv = new WebControl(HtmlTextWriterTag.Div);
            controlDiv.CssClass = "controls";
            controlDiv.Controls.Add(_imgCaptcha);
            controlDiv.Controls.Add(new WebControl(HtmlTextWriterTag.Br));
            controlDiv.Controls.Add(_txtCaptcha);
            controlDiv.Controls.Add(_hiddenSessionId);
            div.Controls.Add(controlDiv);
            Controls.Add(div);
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            EnsureControls();

            if (Page.IsPostBack)
            {
                _txtCaptcha.Text = GetControlValue(_txtCaptcha.ID);
                SessionId = GetControlValue(_hiddenSessionId.ID);
                _hiddenSessionId.Value = GetControlValue(_hiddenSessionId.ID);
                _imgCaptcha.ImageUrl = string.Format("/captcha.ashx?sessionid={0}&width={1}&height={2}&length={3}", SessionId, DefaultWidth, DefaultHeight, DefaultLength);
            }
        }

        public override bool Validate()
        {
            if (_hiddenSessionId.Value == null)
                return false;

            if (HttpContext.Current.Session[_hiddenSessionId.Value] == null || !_txtCaptcha.Text.ToLower().Equals(HttpContext.Current.Session[_hiddenSessionId.Value].ToString().ToLower()))
            {
                ErrorInfo error = new ErrorInfo();
                error.Source = FieldName;
                string captchaMessage = GetResource(SiteConstants.CaptchaErrorMessage);
                if (string.IsNullOrEmpty(captchaMessage))
                    captchaMessage = "Incorrect captcha!";
                error.Message = captchaMessage;
                RegisterError(error);
                return false;
            }
            return true;
        }
    }
}
