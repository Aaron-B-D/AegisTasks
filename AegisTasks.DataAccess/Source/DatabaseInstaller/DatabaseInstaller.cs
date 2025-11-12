using AegisTasks.DataAccess.DataAccesses;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace AegisTasks.DataAccess
{
    public class DatabaseInstaller
    {
        private UsersDataAccess _UsersAccess = new UsersDataAccess();
        private UserParametersAccess _UserParametersAccess = new UserParametersAccess();
        private TemplatesAccess _TemplatesAccess = new TemplatesAccess();
        private ExecutionHistoryDataAccess _ExecutionHistoryAccess = new ExecutionHistoryDataAccess();

        public DatabaseInstaller()
        {
        }

        public void Install(SqlConnection conn)
        {
            _UsersAccess.CreateTable(conn);
            _UserParametersAccess.CreateTable(conn);
            _TemplatesAccess.CreateTable(conn);
            _ExecutionHistoryAccess.CreateTable(conn);
        }
    }
}
