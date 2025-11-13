using AegisTasks.BLL.Aegis;
using AegisTasks.BLL.DataAccess;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AegisTasks.UnitTests.BLL
{
    [TestClass]
    public class AegisManagerBLL_Tests
    {

        [TestMethod]
        public void RecoverTaskPlans()
        {
            Assert.IsTrue(AegisManagerBLL.GetAvailableTaskPlans().Count > 0, "No se han recuperado TaskPlans. Debería haber al menos 1");
        }

    }
}
