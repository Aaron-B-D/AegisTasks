using AegisTasks.BLL.DataAccess;
using AegisTasks.DataAccess;
using AegisTasks.DataAccess.ConnectionFactory;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AegisTasks.UnitTests.BLL
{
    [TestClass]
    public class DataAccessBLL_Tests
    {

        [TestMethod]
        public void CanConnect()
        {
            Assert.IsTrue(DataAccessBLL.CanConnect(), $"No se ha podido comunicar con la base de datos {DatabaseInstaller.DATABASE_NAME}");
        }

    }
}
