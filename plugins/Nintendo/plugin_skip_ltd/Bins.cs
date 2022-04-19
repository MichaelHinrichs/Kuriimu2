using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Komponent.IO;
using Komponent.IO.Streams;
using Kontract.Extensions;
using Kontract.Models.Archive;

namespace plugin_skip_ltd
{
    public class BINS
    {
        private static int _headerSize = Tools.MeasureType(typeof(BinsHeader));

        private string _headerString;

        public IList<IArchiveFileInfo> Load(Stream input)
        {
            using var br = new BinaryReaderX(input, true);

            // Read header
            var header = br.ReadType<BinsHeader>();
            _headerString = header.headerString;

            // Read file entries
            br.ReadBytes(4);
            var fileEntries = br.ReadMultiple<BinsFileEntry>(header.fileCount);

            // Add files
            var result = new List<IArchiveFileInfo>();

            for (var fileEntry = 0; fileEntry < fileEntries.Count; fileEntry++)
            {
                var fileStream = new SubStream(input, fileEntries[fileEntry].fileOffset, fileEntries[fileEntry].fileSize);

                br.BaseStream.Position = fileEntries[fileEntry].fileNameOffset;
                var fileName = br.ReadCStringASCII();
                fileName = fileName.Replace("..\\", "dd\\").Replace(".\\", "d\\");

                result.Add(new ArchiveFileInfo(fileStream, fileName));
            }
            return result;
        }
    }
}
