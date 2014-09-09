using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using FCore.Class;
using FCore.Class.Poll;
using FCore.Collection;
using FCore.Enum;
using FCore.Helper;

namespace FDataProvider
{
    public class PollProvider : IDisposable
    {
        private readonly DataConnection _connection;

        public PollProvider()
        {
            _connection = new DataConnection();
        }

        public PollInfo SelectById(int id, ErrorInfoList errors)
        {
            object[,] param = new object[1, 3];
            param[0, 0] = "@Id";
            param[0, 1] = id;

            ErrorInfo error = new ErrorInfo();
            DataTable dataTable = _connection.ExecuteDataTableQuery("freb_Poll_SelectById", param, QueryType.StoredProcedure, error);
            if (error.Ok && dataTable != null && dataTable.Rows.Count > 0)
                return new PollInfo(dataTable.Rows[0]);

            if (!error.Ok)
                errors.Add(error);
            return null;
        }

        public List<PollInfo> SelectPollsPagingSorting(int pageSize, int pageIndex, string sortBy, string sortOrder, ErrorInfoList errors)
        {
            var param = new object[4, 3];
            param[0, 0] = "@PageSize";
            param[0, 1] = pageSize;
            param[1, 0] = "@PageIndex";
            param[1, 1] = pageIndex;
            param[2, 0] = "@OrderBy";
            param[2, 1] = sortBy;
            param[3, 0] = "@SortOrder";
            param[3, 1] = sortOrder;
            var error = new ErrorInfo();
            DataTable dataTable = _connection.ExecuteDataTableQuery("freb_Poll_SelectByPagingSorting", param, QueryType.StoredProcedure, error);

            if (error.Ok && dataTable != null && dataTable.Rows.Count > 0)
            {
                var result = new List<PollInfo>();
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    result.Add(new PollInfo(dataTable.Rows[i]));
                }
                return result;
            }
            if (!error.Ok)
                errors.Add(error);
            return null;
        }

        public int SelectTotalCount(ErrorInfoList errors)
        {
            var error = new ErrorInfo();
            object result = _connection.ExecuteScalar("freb_Poll_TotalCount", null, QueryType.StoredProcedure, error);
            if (error.Ok)
            {
                return ValidationHelper.GetInteger(result, 0);
            }
            errors.Add(error);
            return 0;
        }

        public object CreatePoll(PollInfo info, ErrorInfoList errors)
        {
            object[,] param = new object[3, 3];
            param[0, 0] = "@Question";
            param[0, 1] = info.Question;
            param[1, 0] = "@BlockMode";
            param[1, 1] = info.BlockMode;
            param[2, 0] = "@IsActive";
            param[2, 1] = info.IsActive;
            ErrorInfo error = new ErrorInfo();
            object result = _connection.ExecuteScalar("freb_Poll_Insert", param, QueryType.StoredProcedure, error);
            if (!errors.HasError())
                return result;
            return 0;
        }

        public bool UpdatePoll(PollInfo info, ErrorInfoList errors)
        {
            object[,] param = new object[4, 3];
            param[0, 0] = "@Id";
            param[0, 1] = info.Id;
            param[1, 0] = "@Question";
            param[1, 1] = info.Question;
            param[2, 0] = "@BlockMode";
            param[2, 1] = info.BlockMode;
            param[3, 0] = "@IsActive";
            param[3, 1] = info.IsActive;
            ErrorInfo error = new ErrorInfo();
            _connection.ExecuteNonQuery("freb_Poll_Update", param, QueryType.StoredProcedure, error);
            if (!errors.HasError())
                return true;
            return false;
        }

        public bool DeletePoll(int id, ErrorInfoList errors)
        {
            object[,] param = new object[1, 3];
            param[0, 0] = "@Id";
            param[0, 1] = id;
            ErrorInfo error = new ErrorInfo();
            _connection.ExecuteNonQuery("freb_Poll_Delete", param, QueryType.StoredProcedure, error);
            if (!errors.HasError())
                return true;
            return false;
        }


        public List<PollChoiceInfo> SelectPollChoiceByPollId(int pollId, ErrorInfoList errors)
        {
            object[,] param = new object[1, 3];
            param[0, 0] = "@PollId";
            param[0, 1] = pollId;

            ErrorInfo error = new ErrorInfo();
            DataTable dataTable = _connection.ExecuteDataTableQuery("freb_PollChoice_SelectByPollId", param, QueryType.StoredProcedure, error);
            if (error.Ok && dataTable != null && dataTable.Rows.Count > 0)
            {
                List<PollChoiceInfo> resultList = new List<PollChoiceInfo>();
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    resultList.Add(new PollChoiceInfo(dataTable.Rows[i]));
                }
            }
            if (!error.Ok)
                errors.Add(error);
            return null;
        }

        public object CreatePollChoice(PollChoiceInfo info, ErrorInfoList errors)
        {
            object[,] param = new object[3, 3];
            param[0, 0] = "@PollId";
            param[0, 1] = info.PollId;
            param[1, 0] = "@Choice";
            param[1, 1] = info.Choice;
            param[2, 0] = "@VoteCount";
            param[2, 1] = info.VoteCount;
            ErrorInfo error = new ErrorInfo();
            object result = _connection.ExecuteScalar("freb_PollChoice_Insert", param, QueryType.StoredProcedure, error);
            if (!errors.HasError())
                return result;
            return 0;
        }

        public bool UpdatePollChoice(PollChoiceInfo info, ErrorInfoList errors)
        {
            object[,] param = new object[4, 3];
            param[0, 0] = "@Id";
            param[0, 1] = info.Id;
            param[1, 0] = "@PollId";
            param[1, 1] = info.PollId;
            param[2, 0] = "@Choice";
            param[2, 1] = info.Choice;
            param[3, 0] = "@VoteCount";
            param[3, 1] = info.VoteCount;
            ErrorInfo error = new ErrorInfo();
            _connection.ExecuteNonQuery("freb_PollChoice_Update", param, QueryType.StoredProcedure, error);
            if (!errors.HasError())
                return true;
            return false;
        }

        public bool DeletePollChoice(int id, ErrorInfoList errors)
        {
            object[,] param = new object[1, 3];
            param[0, 0] = "@Id";
            param[0, 1] = id;
            ErrorInfo error = new ErrorInfo();
            _connection.ExecuteNonQuery("freb_PollChoice_Delete", param, QueryType.StoredProcedure, error);
            if (!errors.HasError())
                return true;
            return false;
        }


        public PollIpAddressInfo SelectPollIpAddressByIpAddress(string ipAddress, ErrorInfoList errors)
        {
            object[,] param = new object[1, 3];
            param[0, 0] = "@IpAddress";
            param[0, 1] = ipAddress;

            ErrorInfo error = new ErrorInfo();
            DataTable dataTable = _connection.ExecuteDataTableQuery("freb_PollIpAddress_SelectByIpAddress", param, QueryType.StoredProcedure, error);
            if (error.Ok && dataTable != null && dataTable.Rows.Count > 0)
                return new PollIpAddressInfo(dataTable.Rows[0]);

            if (!error.Ok)
                errors.Add(error);
            return null;
        }

        public object CreatePollIpAddress(PollIpAddressInfo info, ErrorInfoList errors)
        {
            object[,] param = new object[3, 3];
            param[0, 0] = "@PollId";
            param[0, 1] = info.PollId;
            param[1, 0] = "@IpAddress";
            param[1, 1] = info.IpAddress;
            param[2, 0] = "@ChoiceId";
            param[2, 1] = info.ChoiceId;
            ErrorInfo error = new ErrorInfo();
            object result = _connection.ExecuteScalar("freb_PollIpAddress_Insert", param, QueryType.StoredProcedure, error);
            if (!errors.HasError())
                return result;
            return 0;
        }

        public bool UpdatePollIpAddress(PollIpAddressInfo info, ErrorInfoList errors)
        {
            object[,] param = new object[4, 3];
            param[0, 0] = "@Id";
            param[0, 1] = info.Id;
            param[1, 0] = "@PollId";
            param[1, 1] = info.PollId;
            param[2, 0] = "@IpAddress";
            param[2, 1] = info.IpAddress;
            param[3, 0] = "@ChoiceId";
            param[3, 1] = info.ChoiceId;
            ErrorInfo error = new ErrorInfo();
            _connection.ExecuteNonQuery("freb_PollIpAddress_Update", param, QueryType.StoredProcedure, error);
            if (!errors.HasError())
                return true;
            return false;
        }

        public bool DeletePollIpAddress(int id, ErrorInfoList errors)
        {
            object[,] param = new object[1, 3];
            param[0, 0] = "@Id";
            param[0, 1] = id;
            ErrorInfo error = new ErrorInfo();
            _connection.ExecuteNonQuery("freb_PollIpAddress_Delete", param, QueryType.StoredProcedure, error);
            if (!errors.HasError())
                return true;
            return false;
        }


        private bool _dispoce;
        private void Dispose(bool dispose)
        {
            if (dispose)
                _connection.Dispose();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

    }
}
