using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Text;
using SafeNuGet.NuGet;

namespace SafeNuGetTesting
{
    [TestClass]
    public class NuGetPackageLoaderTest
    {
        [TestMethod]
        public void TestLoad()
        {
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(Properties.TestResources.packages));
            var loader = new NuGetPackageLoader();
            var packages = loader.LoadPackages(stream);
            Assert.AreEqual(2, packages.Count);
            Assert.IsTrue(packages.Exists(p => p.Id == "jQuery" && p.Version == "1.5.1"));
            Assert.IsTrue(packages.Exists(p => p.Id == "EntityFramework.SqlServerCompact" && p.Version == "4.1.8482.2"));
        }
    }
}
