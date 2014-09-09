using System;
using System.Data;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FCore.Helper;
using FUIControls.Context;

namespace FUIControls.PortalControl
{
    public class FAbstractTransformation : AbstractControl, IDataItemContainer
    {
        #region Methods

        public object DataItem
        {
            get
            {
                return ((IDataItemContainer)Parent).DataItem;
            }
        }

        public int DataItemIndex
        {
            get { return ((IDataItemContainer)Parent).DataItemIndex; }
        }

        public int DisplayIndex
        {
            get { return ((IDataItemContainer)Parent).DisplayIndex; }
        }

        public DataRowView DataRowView
        {
            get
            {
                object item = DataItem;
                if ((item != null) && (item is DataRowView))
                {
                    return (DataRowView)item;
                }
                return null;
            }
        }

        public new virtual object Eval(string columnName)
        {
            DataRowView drv = DataRowView;
            if (drv != null)
            {
                object o = DBNull.Value;
                if (drv.DataView.Table.Columns.Contains(columnName))
                {
                    o = drv.Row[columnName];
                }
                return o;
            }
            try
            {
                return base.Eval(columnName);
            }
            catch
            {
                return DBNull.Value;
            }
        }

        public virtual object Eval(string columnName, bool decode)
        {
            try
            {
                object value = base.Eval(columnName);
                if (value != null && decode)
                {
                    value = ValidationHelper.GetString(value, "").ToHtmlDecode();
                }

                return value;
            }
            catch
            {
                return null;
            }
        }

        public virtual object Eval(string columnName, bool decode, int count)
        {
            try
            {
                object value = base.Eval(columnName);

                if (decode)
                {
                    value = Regex.Replace(ValidationHelper.GetString(value, ""), "<.*?>", string.Empty);
                }
                if (count > 0)
                {
                    string val = value.ToString();
                    if (val.Length > count)
                    {
                        val = val.Substring(0, count) + "...";
                    }
                    value = val;
                }

                return value;
            }
            catch
            {
                return null;
            }
        }

        public void GetValueObject(string columnName, ref object returnObj)
        {
            DataRowView drv = DataRowView;
            if (drv != null)
            {
                if (drv.DataView.Table.Columns.Contains(columnName))
                {
                    returnObj = drv.Row[columnName];
                }

            }
        }

        public string GedDate(string column, string format)
        {
            object value = base.Eval(column);
            if (value != null)
            {
                DateTime dateTime = ValidationHelper.GetDateTime(value, DateTime.MaxValue);
                if (dateTime.CompareTo(DateTime.MaxValue) < 0)
                    return dateTime.ToString(format);
            }
            return string.Empty;
        }

        public object IfIsNotNull(string columnName, object defaultValue)
        {
            object o = Eval(columnName);
            if (o != DBNull.Value && o != null && !string.IsNullOrEmpty(ValidationHelper.GetString(o, string.Empty)))
                return o;
            return defaultValue;
        }

        public bool IsNotEmpty(string columnName)
        {
            object o = Eval(columnName);
            return o != DBNull.Value && o != null && string.IsNullOrEmpty(ValidationHelper.GetString(o, string.Empty));
        }

        public string HtmlDecode(string value)
        {
           return Server.HtmlDecode(value);
        }

        #endregion
    }
}
