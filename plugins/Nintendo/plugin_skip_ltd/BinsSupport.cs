using Komponent.IO.Attributes;

namespace plugin_skip_ltd
{
    class BinsHeader
    {
        [FixedLength(4)]
        public string magic = "BINS";
        public byte version = 1;

        public int fileCount;

        [FixedLength(8)]
        public string headerString;
    }

    class BinsFileEntry
    {
        int padding;
        public int fileOffset;
        public int fileSize;
        public int fileNameOffset;
    }
}
