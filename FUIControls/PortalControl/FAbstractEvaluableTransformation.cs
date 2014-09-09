using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.UI;
using FCore.Helper;

namespace FUIControls.PortalControl
{
    public class FAbstractEvaluableTransformation : AbstractControl
    {
        public DataRow DataItem { get; set; }

        public int DataIndex { get; set; }

        public string GetStringValue(string columnName)
        {
            if (DataItem != null && DataItem.Table.Columns.Contains(columnName))
            {
                return ValidationHelper.GetString(DataItem[columnName], string.Empty);
            }
            return string.Empty;
        }

        public string GetDateFormat(string columnName, string format)
        {
            if (DataItem != null && DataItem.Table.Columns.Contains(columnName))
            {
                return ValidationHelper.GetDateTime(DataItem[columnName], DateTime.Today).ToString(format);
            }
            return string.Empty;
        }

        public bool IsNotEmpty(string columnName)
        {
            if (DataItem != null && DataItem.Table.Columns.Contains(columnName))
            {
                return !string.IsNullOrEmpty(ValidationHelper.GetString(DataItem[columnName], string.Empty));
            }
            return false;
        }
    }
}
