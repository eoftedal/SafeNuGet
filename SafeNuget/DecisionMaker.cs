using SafeNuGet.NuGet;
using SafeNuGet.Unsafe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SafeNuGet
{
    public class DecisionMaker
    {
        public List<KeyValuePair<NuGetPackage, UnsafePackage>> Evaluate(NuGetPackages packages, UnsafePackages unsafePackages) 
        {
            var result = new List<KeyValuePair<NuGetPackage, UnsafePackage>>();
            foreach(var package in packages) 
            {
                foreach(var unsafePackage in unsafePackages.Where(u => u.Is(package))) 
                {
                    result.Add(new KeyValuePair<NuGetPackage,UnsafePackage>(package, unsafePackage));
                }
            }
            return result;
        }
    }
}
