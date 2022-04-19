using System.Threading.Tasks;
using Komponent.IO;
using Kontract.Interfaces.FileSystem;
using Kontract.Interfaces.Managers;
using Kontract.Interfaces.Plugins.Identifier;
using Kontract.Interfaces.Plugins.State;
using Kontract.Models;
using Kontract.Models.Context;
using Kontract.Models.IO;

namespace plugin_skip_ltd
{
    public class BinsPlugin : IIdentifyFiles
    {
        public PluginType PluginType => PluginType.Archive;
        public string[] FileExtensions => new[] { "*.h3d", "*.lyt", "*.efb" };
        public PluginMetadata Metadata { get; }

        public BinsPlugin()
        {
            Metadata = new PluginMetadata("PAC", "NintenHero", "The main resource in Chibi Robo Zip Lash.");
        }

        public async Task<bool> IdentifyAsync(IFileSystem fileSystem, UPath filePath, IdentifyContext identifyContext)
        {
            var fileStream = await fileSystem.OpenFileAsync(filePath);

            using var br = new BinaryReaderX(fileStream);
            return br.ReadString(4) == "BINS";
        }

        public IPluginState CreatePluginState(IBaseFileManager pluginManager)
        {
            return new BinsState();
        }
    }
}
