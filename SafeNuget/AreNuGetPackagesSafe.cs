using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using System.IO;
using SafeNuGet.NuGet;
using SafeNuGet.Unsafe;


namespace SafeNuGet
{
    public class AreNuGetPackagesSafe : Task
    {
        public string ProjectPath { get; set; }

        public override bool Execute()
        {
            var nugetFile = Path.Combine(ProjectPath, "packages.config");
            BuildEngine.LogMessageEvent(new BuildMessageEventArgs("Checking " + nugetFile + " ...", "", "SafeNuGet", MessageImportance.High));
            if (File.Exists(nugetFile))
            {
                var packages = new NuGetPackageLoader().LoadPackages(nugetFile);
                var unsafePackages = new PackageListLoader().GetUnsafePackages();
                var failures = new DecisionMaker().Evaluate(packages, unsafePackages);
                if (failures.Count() == 0) {
                    BuildEngine.LogMessageEvent(new BuildMessageEventArgs("No vulnerable packages found", "", "SafeNuGet", MessageImportance.High));
                } else {
                    foreach(var k in failures) {
                        var s = k.Key.Id + " " + k.Key.Version;
                        BuildEngine.LogWarningEvent(new BuildWarningEventArgs("SECURITY ERROR", s, nugetFile, 0, 0, 0, 0, "Library is vulnerable: " + s, "", "SafeNuGet"));
                    }
                    return false;
                }

            } else {
                BuildEngine.LogMessageEvent(new BuildMessageEventArgs("No packages.config found", "", "SafeNuGet", MessageImportance.High));
            }
            return true;
        }

    }
}
