using System.Linq;
using Microsoft.OpenApi.Models;

namespace OpenApiExtended
{
    public class OpenApiMemberInfo
    {
        public string Type { get; set; }
        public string Format { get; set; }
        public string[] Path { get; set; }
        public string PathKey => Constants.RootIndicator + "." + Path.Aggregate((a, b) => a + "." + b);
        public string ParentKey => Parents.Length == 0 ? Constants.RootIndicator : Constants.RootIndicator + "." + Parents.Aggregate((a, b) => a + "." + b);
        public string ParentType { get; set; }
        public bool HasReference { get; set; }
        public bool HasEmptyReference { get; set; }
        public bool HasProperties { get; set; }
        public bool IsPrimitive { get; set; }
        public bool IsInRoot => Parents.Length == 0;
        public bool IsArrayItem => Constants.ArrayItemRegex.IsMatch(Name);
        public bool HasItems { get; set; }
        public bool Required { get; set; }
        public string[] Parents { get; set; }
        public string Name { get; set; }
        public OpenApiSchema Value { get; set; }
    }
}

