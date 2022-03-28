using System.Collections.Generic;
using System.Diagnostics;

namespace OpenApiExtended.Models
{
    public class TypeScriptInfo
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal string PathKey { get; set; }
        public string Name { get; set; }
        public IList<TypeScriptMemberInfo> Members { get; set; }

        public TypeScriptInfo()
        {
            Members = new List<TypeScriptMemberInfo>();
        }
    }
}
