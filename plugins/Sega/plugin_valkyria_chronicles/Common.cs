﻿using System;
using System.IO;
using Komponent.IO;

namespace plugin_valkyria_chronicles
{
    public static class Common
    {
        /// <summary>
        /// The size in bytes of the Packet Header.
        /// </summary>
        public const int PacketHeaderSize = 0x10;

        /// <summary>
        /// The size in bytes of the Extended Packet Header.
        /// </summary>
        public const int PacketHeaderXSize = 0x20;
    }

    /// <summary>
    /// PacketHeader is the standard header in most VC file formats.
    /// </summary>
    public sealed class PacketHeader
    {
        [FixedLength(4)]
        public string Magic;

        /// <summary>
        /// The size of this packet after this header and just before the EOFC including nested packets.
        /// </summary>
        public int PacketSize;
        public int HeaderSize;
        public int Flags;
    }
    
    /// <summary>
    /// PacketHeaderX is an extended header in many VC file formats.
    /// </summary>
    public sealed class PacketHeaderX
    {
        [FixedLength(4)]
        public string Magic;
        
        /// <summary>
        /// The size of this packet after this header and just before the EOFC including nested packets.
        /// </summary>
        public int PacketSize;
        public int HeaderSize;
        public int Flags;
        public int Depth;

        /// <summary>
        /// The size of the data in the packet after this header and just before any nested packets.
        /// </summary>
        public int DataSize;
        public int Unk2;
        public int Unk3;

        /// <summary>
        /// Create a new EOFC footer with default values.
        /// </summary>
        /// <returns>A new EOFC footer.</returns>
        public static PacketHeaderX NewEOFC() => new PacketHeaderX
        {
            Magic = "EOFC",
            HeaderSize = Common.PacketHeaderXSize,
            Flags = 0x10000000
        };
    }

    public static class Extensions
    {
        /// <summary>
        /// Read bytes that are ROTn obfuscated.
        /// </summary>
        /// <param name="br">A BinaryReader.</param>
        /// <param name="count">The number of bytes to read. This value must be 0 or a non-negative number or an exception will occur.</param>
        /// <param name="rot">The number that each byte needs to be rotated by.</param>
        /// <returns></returns>
        public static byte[] ReadROTnBytes(this BinaryReader br, int count, int rot = 1)
        {
            var bytes = br.ReadBytes(count);
            for (var i = 0; i < bytes.Length; i++)
                if (bytes[i] >= rot) bytes[i] -= (byte)rot;
            return bytes;
        }

        /// <summary>
        /// Read an Int32 that is ROTn obfuscated.
        /// </summary>
        /// <param name="br">A BinaryReader.</param>
        /// <param name="rot">The number that each byte needs to be rotated by.</param>
        /// <returns></returns>
        public static int ReadROTnInt32(this BinaryReader br, int rot = 1)
        {
            return BitConverter.ToInt32(br.ReadROTnBytes(4, rot), 0);
        }
    }
}
