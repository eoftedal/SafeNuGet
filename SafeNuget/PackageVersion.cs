using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SafeNuGet
{
    public class PackageVersion 
    {
        private String[] _parts;
        private String _version;
        
        public PackageVersion(String version) 
        {
            _parts = version.Split('.', '-');
            _version = version;
        }

        public override int GetHashCode()
        {
            return _version.GetHashCode();
        }
        public override bool Equals(object obj)
        {
            var pv = obj as PackageVersion;
            if ((object)pv == null) return false;
            return this == pv;
        }

        public static bool operator <(PackageVersion p1, PackageVersion p2) 
        {
            return p1.CompareTo(p2) < 0;
        }
        public static bool operator >(PackageVersion p1, PackageVersion p2)
        {
            return p1.CompareTo(p2) > 0;
        }
        public static bool operator <=(PackageVersion p1, PackageVersion p2)
        {
            return p1.CompareTo(p2) <= 0;
        }
        public static bool operator >=(PackageVersion p1, PackageVersion p2)
        {
            return p1.CompareTo(p2) >= 0;
        }
        public static bool operator ==(PackageVersion p1, PackageVersion p2)
        {
            if ((object)p1 == null && (object)p2 == null) return true;
            if ((object)p1 == null) return false;
            return p1.CompareTo(p2) == 0;
        }
        public static bool operator !=(PackageVersion p1, PackageVersion p2)
        {
            return !(p1 == p2);
        }
        public int CompareTo(PackageVersion o)
        {
            var pv = o as PackageVersion;
            if (pv == null) return -1;

            var result = 0;
            if (pv._parts.Length > _parts.Length)
                return -pv.CompareTo(this);

            for (var i = 0; i < _parts.Length; i++)
            {
                if (pv._parts.Length < i + 1)
                {
                    result = Compare(_parts[i], "0");
                }
                else
                {
                    result = Compare(_parts[i], pv._parts[i]);
                }
                if (result != 0)
                    return result;
            }
            return result;
        }
        private int Compare(String part1, String part2)
        {
            int i1, i2;
            if (int.TryParse(part1, out i1) && int.TryParse(part2, out i2))
            {
                return i1.CompareTo(i2);
            }
            return part1.CompareTo(part2);
        }

    }
}
