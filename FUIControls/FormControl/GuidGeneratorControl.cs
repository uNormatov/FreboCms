using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using FCore.Enum;

namespace FUIControls.FormControl
{
    [ToolboxData("<fr:GuidGeneratorControl runat=\"server\" ID=\"GuidGeneratorControl1\" />")]
    public sealed class GuidGeneratorControl : AbsractBasicControl
    {
        #region Variables

        private HiddenField _hiddenGuid;

        #endregion

        #region Constructors

        public GuidGeneratorControl()
            : this(null)
        {
        }

        public GuidGeneratorControl(string controlId)
        {
            if (!string.IsNullOrEmpty(controlId))
                ID = controlId;
        }

        #endregion

        #region Properties

        #endregion

        #region Methods

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            if (Page.IsPostBack)
            {
                if (ViewMode == FormControlViewMode.Editor)
                {
                    EnsureControls();
                    _hiddenGuid.Value = GetControlValue(_hiddenGuid.ID);
                }
            }
        }

        public override void SetValue(string value)
        {
            EnsureControls();
            if (ViewMode == FormControlViewMode.Editor)
            {
                _hiddenGuid.Value = value;
            }
        }

        public override string GetValue()
        {
            if (ViewMode == FormControlViewMode.Editor)
            {
                return _hiddenGuid.Value;
            }
            return string.Empty;
        }

        protected override void EnsureControls()
        {
            if (ViewMode == FormControlViewMode.Editor)
            {
                if (_hiddenGuid == null)
                {
                    _hiddenGuid = new HiddenField { ID = string.Format("hidden_guid{0}", ID) };
                }

                if (string.IsNullOrEmpty(_hiddenGuid.Value))
                {
                    _hiddenGuid.Value = Guid.NewGuid().ToString();
                }
            }
        }

        protected override void CreateChildControls()
        {
            EnsureControls();
            if (ViewMode == FormControlViewMode.Editor)
            {
                Controls.Add(_hiddenGuid);
            }
        }

        #endregion
    }
}