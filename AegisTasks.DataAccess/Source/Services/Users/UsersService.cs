using AegisTasks.DataAccess.ConnectionFactory;
using AegisTasks.DataAccess.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace AegisTasks.DataAccess.Services
{
    public class UsersService: AegisServiceSqlServerBase
    {
        public UsersService(DBConnectionFactorySqlServer factory) : base(factory)
        {}
    }
}
