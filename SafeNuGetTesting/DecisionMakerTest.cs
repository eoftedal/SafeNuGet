using Microsoft.VisualStudio.TestTools.UnitTesting;
using SafeNuGet;
using SafeNuGet.NuGet;
using SafeNuGet.Unsafe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SafeNuGetTesting
{
    [TestClass]
    public class DecisionMakerTest
    {
        [TestMethod]
        public void TestAtVersion() 
        {
            var nuget = new NuGetPackages() { new NuGetPackage(){Id="Test", Version="1.0.0"} };
            var unsafePacks = new UnsafePackages() { new UnsafePackage(){Id="Test", Version="1.0.0"}};
            var result = new DecisionMaker().Evaluate(nuget, unsafePacks);
            Assert.AreEqual(1, result.Count());
        }
        [TestMethod]
        public void NotAtVersion()
        {
            var nuget = new NuGetPackages() { new NuGetPackage() { Id = "Test", Version = "1.0.0" } };
            var unsafePacks = new UnsafePackages() { new UnsafePackage() { Id = "Test", Version = "1.0.1" } };
            var result = new DecisionMaker().Evaluate(nuget, unsafePacks);
            Assert.AreEqual(0, result.Count());
        }
        [TestMethod]
        public void BeforeVersion()
        {
            var nuget = new NuGetPackages() { new NuGetPackage() { Id = "Test", Version = "1.0.0-alpha1" } };
            var unsafePacks = new UnsafePackages() { new UnsafePackage() { Id = "Test", Before = "1.0.1" } };
            var result = new DecisionMaker().Evaluate(nuget, unsafePacks);
            Assert.AreEqual(1, result.Count());
        }
        [TestMethod]
        public void NotBeforeVersion()
        {
            var nuget = new NuGetPackages() { new NuGetPackage() { Id = "Test", Version = "1.0.0" } };
            var unsafePacks = new UnsafePackages() { new UnsafePackage() { Id = "Test", Before = "1.0.0" } };
            var result = new DecisionMaker().Evaluate(nuget, unsafePacks);
            Assert.AreEqual(0, result.Count());
        }
        [TestMethod]
        public void BeforeOrAtVersion()
        {
            var nuget = new NuGetPackages() { new NuGetPackage() { Id = "Test", Version = "1.0.0" }, new NuGetPackage(){Id="Test2", Version="1.0.0" } };
            var unsafePacks = new UnsafePackages() { new UnsafePackage() { Id = "Test", BeforeOrAt = "1.0.0" }, new UnsafePackage() { Id = "Test2", BeforeOrAt = "1.0.1" } };
            var result = new DecisionMaker().Evaluate(nuget, unsafePacks);
            Assert.AreEqual(2, result.Count());
        }
        [TestMethod]
        public void NotBeforeOrAtVersion()
        {
            var nuget = new NuGetPackages() { new NuGetPackage() { Id = "Test", Version = "1.0.1" } };
            var unsafePacks = new UnsafePackages() { new UnsafePackage() { Id = "Test", BeforeOrAt = "1.0.0" } };
            var result = new DecisionMaker().Evaluate(nuget, unsafePacks);
            Assert.AreEqual(0, result.Count());
        }

        [TestMethod]
        public void AfterVersion()
        {
            var nuget = new NuGetPackages() { new NuGetPackage() { Id = "Test", Version = "1.0.1" } };
            var unsafePacks = new UnsafePackages() { new UnsafePackage() { Id = "Test", After = "1.0.0" } };
            var result = new DecisionMaker().Evaluate(nuget, unsafePacks);
            Assert.AreEqual(1, result.Count());
        }
        [TestMethod]
        public void NotAfterVersion()
        {
            var nuget = new NuGetPackages() { new NuGetPackage() { Id = "Test", Version = "1.0.0" } };
            var unsafePacks = new UnsafePackages() { new UnsafePackage() { Id = "Test", After = "1.0.0" } };
            var result = new DecisionMaker().Evaluate(nuget, unsafePacks);
            Assert.AreEqual(0, result.Count());
        }
        [TestMethod]
        public void AfterOrAtVersion()
        {
            var nuget = new NuGetPackages() { new NuGetPackage() { Id = "Test", Version = "1.0.0" }, new NuGetPackage() { Id = "Test2", Version = "1.0.0" } };
            var unsafePacks = new UnsafePackages() { new UnsafePackage() { Id = "Test", AfterOrAt = "1.0.0" }, new UnsafePackage() { Id = "Test2", AfterOrAt = "0.0.9" } };
            var result = new DecisionMaker().Evaluate(nuget, unsafePacks);
            Assert.AreEqual(2, result.Count());
        }
        [TestMethod]
        public void NotAfterOrAtVersion()
        {
            var nuget = new NuGetPackages() { new NuGetPackage() { Id = "Test", Version = "1.0.0" } };
            var unsafePacks = new UnsafePackages() { new UnsafePackage() { Id = "Test", AfterOrAt = "1.0.1" } };
            var result = new DecisionMaker().Evaluate(nuget, unsafePacks);
            Assert.AreEqual(0, result.Count());
        }

        [TestMethod]
        public void BeforeAndAfterVersion()
        {
            var nuget = new NuGetPackages() { new NuGetPackage() { Id = "Test", Version = "1.0.0" } };
            var unsafePacks = new UnsafePackages() { new UnsafePackage() { Id = "Test", Before = "1.0.1", After = "0.0.9" } };
            var result = new DecisionMaker().Evaluate(nuget, unsafePacks);
            Assert.AreEqual(1, result.Count());
        }
        [TestMethod]
        public void NotBeforeAndAfterVersion()
        {
            var nuget = new NuGetPackages() { new NuGetPackage() { Id = "Test", Version = "1.0.1" }, new NuGetPackage() { Id = "Test2", Version = "0.0.9" } };
            var unsafePacks = new UnsafePackages() { new UnsafePackage() { Id = "Test", Before = "1.0.1", After = "0.0.9" }, new UnsafePackage() { Id = "Test2", Before = "1.0.1", After = "0.0.9" } };
            var result = new DecisionMaker().Evaluate(nuget, unsafePacks);
            Assert.AreEqual(0, result.Count());
        }

    }
}
