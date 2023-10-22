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
using System.Runtime.CompilerServices;
using System.Text;
using static System.Collections.Specialized.BitVector32;

namespace PEOpener.Infrastuture
{
    public static class HexFile
    {
        public static byte[] HexBytes { get; set; }
        public static string FileName { get; set; }

        private static PEFile peFile;

        public static long maxFileSize = 1024L * 1024L * 1024L * 2L;

        public static bool IsFileOpen { get =>  peFile != null; }

        public static event EventHandler OnFileLoadComplete = delegate { };
        public static event EventHandler OnFileLoadStart = delegate { };
        public static event EventHandler OnFileHexLoadComplete = delegate { };

        public static void LoadFile(string fileName, IBrowserFile file)
        {
            Task loadFile = new( async () =>
            {
                OnFileLoadStart?.Invoke(null, EventArgs.Empty);
                using (var reader = new StreamReader(file.OpenReadStream(HexFile.maxFileSize)))
                {
                    using (var memstream = new MemoryStream())
                    {
                        try
                        {
                            await reader.BaseStream.CopyToAsync(memstream);
                            var bytes = memstream.ToArray();
                            HexBytes = bytes;
                            FileName = fileName;
                            peFile = PEFile.FromBytes(bytes);
                            OnFileLoadComplete?.Invoke(null, EventArgs.Empty);
                        }
                        catch (Exception ex)
                        {
                            memstream.Dispose();
                        }
                    }
                }
                
            });
            loadFile.Start();
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

        public static List<TableItem> ImportsTable {get; private set;}
        public static List<TableItem> getImports()
        {
            ImportsTable = new List<TableItem>();
            var peImage = PEImage.FromFile(peFile);
            foreach (var module in peImage.Imports)
            {
                List<string> vals = new List<string>();
                foreach (var member in module.Symbols)
                {
                    if (member.IsImportByName) vals.Add(member.Name);
                    else vals.Add(member.Ordinal.ToString());
                }
                ImportsTable.Add(new TableItem { Key = module.Name, Value = string.Join(",", vals.ToArray()) });
            }
            return ImportsTable;
        }

        public static List<TableItem> ExportsTable { get; private set; }
        public static List<TableItem> getExports()
        {
            ExportsTable = new List<TableItem>();
            var peImage = PEImage.FromFile(peFile);
            foreach (var symbol in peImage.Exports.Entries)
            {
                string key = $"Ordinal: {symbol.Ordinal}";
                if (symbol.IsByName)
                    key += $" Name: {symbol.Name}";
                ExportsTable.Add(new TableItem { Key = key, Value = $"Address: {symbol.Address.Rva:X8}" });
            }
            return ExportsTable;
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
