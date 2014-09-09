using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using FCore.Collection;
using FDataProvider;

namespace FUIControls.UIControl
{
    [ToolboxData("<fr:FQueryDataSource  runat=\"server\" ID=\"FQueryDataSource\"></<fr:FQueryDataSource>")]
    public class FQueryDataSource : DataSourceControl
    {
        private QueryDataSourceView _dataSourceView;

        public string QueryName { get; set; }

        protected QueryDataSourceView View
        {
            get
            {
                if (_dataSourceView == null)
                {
                    _dataSourceView = GetView(null) as QueryDataSourceView;
                }
                return _dataSourceView;
            }
        }

        public object[,] SelectParameters
        {
            get
            {
                object o = ViewState["__query_data_source_select_parameters"];
                if (o == null)
                    return new object[0, 3];
                return (object[,])o;
            }
            set { ViewState["__query_data_source_select_parameters"] = value; }
        }

        protected override DataSourceView GetView(string viewName)
        {
            _dataSourceView = new QueryDataSourceView(this, viewName);
            _dataSourceView.QueryName = QueryName;
            _dataSourceView.SelectParameters = SelectParameters;
            return _dataSourceView;
        }

        protected override ICollection GetViewNames()
        {
            var list = new ArrayList();
            list.Add(QueryDataSourceView.DefaultViewName);
            return list;
        }
    }


    public class QueryDataSourceView : DataSourceView
    {
        #region Variables

        private FQueryDataSource _owner;
        private string _queryName = string.Empty;
        public static string DefaultViewName = "defaultViewName";
        protected object[,] _selectParameters;
        private readonly GeneralConnection _generalConnection;

        #endregion

        #region Properties

        public string QueryName
        {
            get
            {
                if (string.IsNullOrEmpty(_queryName))
                    return "";
                return _queryName;
            }
            set { _queryName = value; }
        }

        public object[,] SelectParameters
        {
            get
            {
                if (_selectParameters == null)
                    _selectParameters = new object[0, 3];
                return _selectParameters;
            }
            set { _selectParameters = value; }
        }

        #endregion

        #region Constructor

        public QueryDataSourceView(IDataSource owner, string name)
            : base(owner, name)
        {
            _owner = owner as FQueryDataSource;
            DefaultViewName = name;
            _generalConnection = new GeneralConnection();
        }

        #endregion

        #region Methods

        protected override IEnumerable ExecuteSelect(DataSourceSelectArguments arguments)
        {
            try
            {
                if (string.IsNullOrEmpty(QueryName))
                    return new List<string>();


                arguments.RaiseUnsupportedCapabilitiesError(this);

                DataTable datatable = _generalConnection.ExecuteDataTableQuery(QueryName, SelectParameters,
                                                                               new ErrorInfoList());
                if (datatable != null)
                {
                    var dataview = new DataView(datatable);
                    if (!string.IsNullOrEmpty(arguments.SortExpression))
                        dataview.Sort = arguments.SortExpression;

                    return dataview;
                }
                return null;
            }
            catch
            {
                return new List<string>();
            }
        }

        #endregion
    }
}
