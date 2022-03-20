using Microsoft.OpenApi.Models;

namespace OpenApiExtended
{
    public class OpenApiMembersInfo
    {
        public OpenApiMembersType Type { get; set; }
        public OpenApiMembersType ParentType { get; set; }
        public bool HasReference { get; set; }
        public bool HasMissingReference { get; set; }
        public bool HasProperties { get; set; }
        public bool HasItems { get; set; }
        public bool Required { get; set; }
        public string[] Parents { get; set; }
        public string Name { get; set; }
        public OpenApiSchema Value { get; set; }
    }
}

