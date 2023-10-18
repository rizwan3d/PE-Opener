using AsmResolver.PE.File;
using AsmResolver.PE.File.Headers;
using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using System.Reflection.PortableExecutable;

namespace PEOpener.Infrastuture
{
    public class TableItem
    {
		public TableItem()
		{
		}

		public TableItem(string key, string value)
		{
			Key = key;
			Value = value;
		}

		public string Key { get; set; }
        public string Value { get; set; }
	}
    public static class HexFile
    {
        public static byte[] HexBytes { get; set; }
        public static string FileName { get; set; }

        private static PEFile peFile;

        public static bool IsFileOpen { get =>  peFile != null; }

        public static void LoadFile(string fileName, byte[] hexBytes)
        {
            HexBytes = hexBytes;
            FileName = fileName;
            peFile = PEFile.FromBytes(hexBytes);
        }

        public static List<TableItem> FileHeader()
        {
            FileHeader header = peFile.FileHeader;
			return new()
            {
                new TableItem("Machine", header.Machine.ToString()),
                new TableItem("NumberOfSections:", header.NumberOfSections.ToString()),
                new TableItem("TimeDateStamp:", $"0x{header.TimeDateStamp:X8}"),
                new TableItem("PointerToSymbolTable:", $"0x{header.PointerToSymbolTable:X8}"),
                new TableItem("NumberOfSymbols:", header.NumberOfSymbols.ToString()),
                new TableItem("SizeOfOptionalHeader:", $"0x{header.SizeOfOptionalHeader:X4}"),
                new TableItem("Characteristics:", header.Characteristics.ToString()),
            };
        }
    }
}
