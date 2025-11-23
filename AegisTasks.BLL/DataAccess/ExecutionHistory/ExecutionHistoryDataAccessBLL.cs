using AegisTasks.Core.DTO;
using AegisTasks.DataAccess.DataAccesses;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;

namespace AegisTasks.BLL.DataAccess
{
    public static class ExecutionHistoryDataAccessBLL
    {
        private static readonly ExecutionHistoryDataAccess _DataAccess = new ExecutionHistoryDataAccess();

        static ExecutionHistoryDataAccessBLL() { }

        public static int RegisterExecution(string workflowId, string workflowName, string username)
        {
            if (string.IsNullOrWhiteSpace(workflowId))
                throw new ArgumentNullException(nameof(workflowId));
            if (string.IsNullOrWhiteSpace(workflowName))
                throw new ArgumentNullException(nameof(workflowName));
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentNullException(nameof(username));

            username = username.ToLowerInvariant();

            using (SqlConnection conn = DataAccessBLL.CreateConnection())
            {
                conn.Open();
                return _DataAccess.RegisterExecution(conn, workflowId, workflowName, username);
            }
        }



        public static void UpdateExecution(int executionId, bool success)
        {
            using (SqlConnection conn = DataAccessBLL.CreateConnection())
            {
                conn.Open();
                _DataAccess.UpdateExecution(conn, executionId, success);
            }
        }

        public static List<ExecutionHistoryLineDTO> GetExecutionsByUser(string username, bool includeInactive = false)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentNullException(nameof(username));

            username = username.ToLowerInvariant();

            using (SqlConnection conn = DataAccessBLL.CreateConnection())
            {
                conn.Open();
                return _DataAccess.GetExecutionsByUser(conn, username, includeInactive);
            }
        }
    }
}
