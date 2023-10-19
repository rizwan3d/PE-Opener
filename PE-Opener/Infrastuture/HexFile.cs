using AsmResolver;
using AsmResolver.PE;
using AsmResolver.PE.File;
using AsmResolver.PE.File.Headers;
using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using System.Collections.Generic;
using System.Reflection.PortableExecutable;
using System.Text;

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
            OptionalHeader header2 = peFile.OptionalHeader;
            return new()
            {
                new TableItem("Magic", header2.Magic.ToString()),
                new TableItem("Machine", header.Machine.ToString()),
                new TableItem("NumberOfSections:", header.NumberOfSections.ToString()),
                new TableItem("TimeDateStamp:", $"0x{header.TimeDateStamp:X8}"),
                new TableItem("PointerToSymbolTable:", $"0x{header.PointerToSymbolTable:X8}"),
                new TableItem("NumberOfSymbols:", header.NumberOfSymbols.ToString()),
                new TableItem("SizeOfOptionalHeader:", $"0x{header.SizeOfOptionalHeader:X4}"),
                new TableItem("Characteristics:", header.Characteristics.ToString()),
            };
        }

        public static List<TableItem> OptionalHeader()
        {
            OptionalHeader header = peFile.OptionalHeader;
            return new()
            {
                new TableItem("Offset", header.Offset.ToString()),
                new TableItem("AddressOfEntryPoint", header.AddressOfEntryPoint.ToString()),
                new TableItem("BaseOfCode", header.BaseOfCode.ToString()),
                new TableItem("BaseOfData", header.BaseOfData.ToString()),
                new TableItem("CanUpdateOffsets", header.CanUpdateOffsets.ToString()),
                new TableItem("CheckSum", header.CheckSum.ToString()),
                new TableItem("DllCharacteristics", header.DllCharacteristics.ToString()),
                new TableItem("FileAlignment", header.FileAlignment.ToString()),
                new TableItem("HashCode", header.GetHashCode().ToString()),
                new TableItem("PhysicalSize", header.GetPhysicalSize().ToString()),
                new TableItem("VirtualSize", header.GetVirtualSize().ToString()),
                new TableItem("ImageBase", header.ImageBase.ToString()),
                new TableItem("LoaderFlags", header.LoaderFlags.ToString()),
                new TableItem("MajorImageVersion", header.MajorImageVersion.ToString()),
                new TableItem("MinorImageVersion", header.MinorImageVersion.ToString()),
                new TableItem("MajorLinkerVersion", header.MajorLinkerVersion.ToString()),
                new TableItem("MinorLinkerVersion", header.MinorLinkerVersion.ToString()),
                new TableItem("MajorOperatingSystemVersion", header.MajorOperatingSystemVersion.ToString()),
                new TableItem("MajorSubsystemVersion", header.MajorSubsystemVersion.ToString()),
                new TableItem("MinorSubsystemVersion", header.MinorSubsystemVersion.ToString()),
                new TableItem("Rva", header.Rva.ToString()),
                new TableItem("NumberOfRvaAndSizes", header.NumberOfRvaAndSizes.ToString()),
                new TableItem("SectionAlignment", header.SectionAlignment.ToString()),
                new TableItem("SizeOfCode", header.SizeOfCode.ToString()),
                new TableItem("SizeOfHeaders", header.SizeOfHeaders.ToString()),
                new TableItem("SizeOfHeapCommit", header.SizeOfHeapCommit.ToString()),
                new TableItem("SizeOfHeapReserve", header.SizeOfHeapReserve.ToString()),
                new TableItem("SizeOfImage", header.SizeOfImage.ToString()),
                new TableItem("SizeOfInitializedData", header.SizeOfInitializedData.ToString()),
                new TableItem("SizeOfStackCommit", header.SizeOfStackCommit.ToString()),
                new TableItem("SizeOfStackReserve", header.SizeOfStackReserve.ToString()),
                new TableItem("SizeOfUninitializedData", header.SizeOfUninitializedData.ToString()),
                new TableItem("SubSystem", header.SubSystem.ToString()),
                new TableItem("Win32VersionValue", header.Win32VersionValue.ToString()),
            };
        }

        static Dictionary<string, PESection>  pESections = new Dictionary<string, PESection>();
        public static List<string> getSections()
        {
            List<string> SectionsName = new List<string>();
            foreach (var section in peFile.Sections)
            {
                pESections.Add(section.Name, section);
                SectionsName.Add(section.Name);
            }
            return SectionsName;
        }

        public static List<TableItem> GetSectionsByName(string Name)
        {
            PESection section = pESections[Name];
            return new()
            {
                new TableItem("Name", section.Name),
                new TableItem("Rva", section.Rva.ToString()),
                new TableItem("Offset", section.Offset.ToString()),
                new TableItem("IsMemoryShared", section.IsMemoryShared.ToString()),
                new TableItem("IsReadable", section.IsReadable.ToString()),
                new TableItem("Characteristics", section.Characteristics.ToString()),
                new TableItem("PhysicalSize", section.GetPhysicalSize().ToString()),
                new TableItem("VirtualSize", section.GetVirtualSize().ToString()),
                new TableItem("IsContentCode", section.IsContentCode.ToString()),
                new TableItem("IsContentInitializedData", section.IsContentInitializedData.ToString()),
                new TableItem("IsContentUninitializedData", section.IsContentUninitializedData.ToString()),
                new TableItem("IsMemoryDiscardable", section.IsMemoryDiscardable.ToString()),
                new TableItem("IsMemoryExecute", section.IsMemoryExecute.ToString()),
                new TableItem("IsMemoryNotCached", section.IsMemoryNotCached.ToString()),
                new TableItem("IsMemoryNotPaged", section.IsMemoryNotPaged.ToString()),
                new TableItem("IsMemoryRead", section.IsMemoryRead.ToString()),
                new TableItem("IsMemoryShared", section.IsMemoryShared.ToString()),
                new TableItem("IsMemoryWrite", section.IsMemoryWrite.ToString()),
                new TableItem("IsReadable", section.IsReadable.ToString()),
                new TableItem("Bytes", Encoding.ASCII.GetString(section.ToArray(), 0, section.ToArray().Length)),
            };
        }
    }
}
