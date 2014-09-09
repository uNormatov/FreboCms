using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FUIControls.FormControl
{
    public interface IValidatable
    {
        #region Control for detection parameters

        bool IsValid
        { get; set; }

        bool IsRequired
        { get; set; }

        string RequiredErrorMessage
        { get; set; }

        string RegularExpression
        { get; set; }

        string RegularExpressionErrorMessage
        { get; set; }

        bool Validate();

        #endregion
    }
}
