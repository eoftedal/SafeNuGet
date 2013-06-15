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
        public string CacheTimeInMinutes { get; set; }

        public override bool Execute()
        {
            var nugetFile = Path.Combine(ProjectPath, "packages.config");
            int cacheTime = 0;
            if (!String.IsNullOrEmpty(CacheTimeInMinutes) && !int.TryParse(CacheTimeInMinutes, out cacheTime))
            {
                BuildEngine.LogErrorEvent(new BuildErrorEventArgs("Configuration error", "CacheTimeInMinutes", BuildEngine.ProjectFileOfTaskNode, 0, 0, 0, 0, "Invalid value for CacheTimeInMinutes: " + CacheTimeInMinutes, "", "SafeNuGet"));
                return false;
            }

            BuildEngine.LogMessageEvent(new BuildMessageEventArgs("Checking " + nugetFile + " ...", "", "SafeNuGet", MessageImportance.High));
            if (File.Exists(nugetFile))
            {
                var packages = new NuGetPackageLoader().LoadPackages(nugetFile);
                UnsafePackages unsafePackages;
                if (cacheTime > 0)
                {
                    bool cacheHit = false;
                    var cacheFolder = Path.Combine(new FileInfo(BuildEngine.ProjectFileOfTaskNode).Directory.FullName, "cache");
                    unsafePackages = new PackageListLoader().GetCachedUnsafePackages(cacheFolder, cacheTime, out cacheHit);
                    if (cacheHit)
                    {
                        BuildEngine.LogMessageEvent(new BuildMessageEventArgs("Using cached list of unsafe packages", "", "SafeNuGet", MessageImportance.High));
                    }
                }
                else
                {
                    unsafePackages = new PackageListLoader().GetUnsafePackages();
                }
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
