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

        public override bool Execute()
        {
            Console.WriteLine("FACK");
            var path = new FileInfo(this.BuildEngine.ProjectFileOfTaskNode).Directory;
            var nugetFile = Path.Combine(path.ToString(), "packages.config");
            if (File.Exists(nugetFile))
            {
                var packages = new NuGetPackageLoader().LoadPackages(nugetFile);
                var unsafePackages = new PackageListLoader().GetUnsafePackages();
                var failures = new DecisionMaker().Evaluate(packages, unsafePackages);
                if (failures.Count() == 0) {
                    BuildEngine.LogMessageEvent(new BuildMessageEventArgs("No vulnerable packages found", "", "SafeNuGet", MessageImportance.Normal));
                } else {
                    foreach(var k in failures) {
                        var s = k.Key.Id + " " + k.Key.Version;
                        BuildEngine.LogWarningEvent(new BuildWarningEventArgs("SECURITY ERROR", s, nugetFile, 0, 0, 0, 0, "Library is vulnerable: " + s, "", "SafeNuGet"));
                    }
                    return false;
                }

            } else {
                BuildEngine.LogMessageEvent(new BuildMessageEventArgs("No packages.config found", "", "SafeNuGet", MessageImportance.Normal));
            }
            return true;
        }

    }
}
