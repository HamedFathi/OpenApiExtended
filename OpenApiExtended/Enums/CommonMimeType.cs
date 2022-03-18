using System.ComponentModel;

namespace OpenApiExtended.Enums
{
    public enum CommonMimeType
    {
        [Description("application/json")]
        ApplicationJson,
        [Description("text/json")]
        TextJson,
        [Description("application/*+json")]
        ApplicationAsteriskJson,
        [Description("application/xml")]
        ApplicationXml,
        [Description("text/xml")]
        TextXml,
        [Description("application/*+xml")]
        ApplicationAsteriskXml
    }
}
