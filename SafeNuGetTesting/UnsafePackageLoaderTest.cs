using Microsoft.VisualStudio.TestTools.UnitTesting;
using SafeNuGet.Unsafe;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SafeNuGetTesting
{
    [TestClass]
    public class UnsafePackageLoaderTest
    {
        [TestMethod]
        public void TestLoad()
        {
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(Properties.TestResources.unsafepackages));
            var loader = new PackageListLoader();
            var packages = loader.LoadPackages(stream);
            Assert.AreEqual(1, packages.Count);
            Assert.IsTrue(packages.Exists(p => p.Id == "AntiXss" && p.Before == "4.2.1"));
        }
    }
}
