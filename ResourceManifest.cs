using Orchard.Environment.Extensions;
using Orchard.UI.Resources;

namespace Piedone.BBCode
{
    public class ResourceManifest : IResourceManifestProvider
    {
        public void BuildManifests(ResourceManifestBuilder builder)
        {
            var manifest = builder.Add();
            manifest.DefineStyle("BBCode").SetUrl("piedone-bbcode-styles.css");
        }
    }
}
