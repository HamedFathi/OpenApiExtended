using System.Linq;
using Microsoft.OpenApi.Models;

namespace OpenApiExtended
{
    public class OpenApiMemberInfo
    {
        public string Type { get; set; }
        public string Format { get; set; }
        public string[] Path { get; set; }
        public string PathKey => Path == null || Path.Length == 0 ? null : (Constants.RootIndicator + "." + Path.Aggregate((a, b) => a + "." + b)).Replace("->", "");
        public string ParentKey => Parents == null ? null : Parents.Length == 0 ? Constants.RootIndicator : (Constants.RootIndicator + "." + Parents.Aggregate((a, b) => a + "." + b)).Replace("->", "");
        public string ParentType { get; set; }
        public bool HasReference { get; set; }
        public string ReferenceId { get; set; }
        public bool IsEmptyObject { get; set; }
        public bool IsObject { get; set; }
        public bool IsPrimitive { get; set; }
        public bool IsInRoot => Parents is { Length: 0 };
        public bool IsRoot { get; set; }
        public bool IsArrayItem => Name != null && Constants.ArrayItemRegex.IsMatch(Name);
        public bool IsArray { get; set; }
        public string[] Required { get; set; }
        public bool IsOptional { get; set; }
        public bool IsNullable { get; set; }
        public string[] Parents { get; set; }
        public string Name { get; set; }
        public OpenApiSchema Value { get; set; }
    }
}

