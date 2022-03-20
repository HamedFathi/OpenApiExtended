using Microsoft.OpenApi.Models;
using System.Text.RegularExpressions;

namespace OpenApiExtended
{
    public class OpenApiMembersInfo
    {
        public string Type { get; set; }
        public string Format { get; set; }
        public string[] Path { get; set; }
        public string ParentType { get; set; }
        public bool HasReference { get; set; }
        public bool HasEmptyReference { get; set; }
        public bool HasProperties { get; set; }
        public bool IsPrimitive { get; set; }
        public bool IsInRoot
        {
            get { return Parents.Length == 0; }
        }
        public bool IsArrayItem
        {
            get
            {
                var arrayItemRegex = new Regex(@"(.+)\.\.\.\[(.+)\]");
                return arrayItemRegex.IsMatch(Name);
            }
        }
        public bool HasItems { get; set; }
        public bool Required { get; set; }
        public string[] Parents { get; set; }
        public string Name { get; set; }
        public OpenApiSchema Value { get; set; }
    }
}

