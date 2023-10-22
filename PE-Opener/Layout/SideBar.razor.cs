using Havit.Blazor.Components.Web.Bootstrap;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using PEOpener.Components;
using PEOpener.Infrastuture;
using System.Text;

namespace PEOpener.Layout
{
    public partial class SideBar : ComponentBase
    {
        private List<PeFileTree> treeSystem = new List<PeFileTree>();

        public static EventHandler<SideBarOnSectionChangeEventArgs> OnSectionChange = delegate { };

        protected override void OnInitialized()
        {
            HexFile.OnFileLoadComplete += HexFile_OnFileLoadComplete;
        }

        private void HexFile_OnFileLoadComplete(object? sender, EventArgs e)
        {
            build();
        }

        public async void OnSelectedItemChanged(PeFileTree selectedItem)
        {
            if (selectedItem.Name == HexFile.FileName)
            {
                OnSectionChange?.Invoke(this, new SideBarOnSectionChangeEventArgs(HexFile.HexBytes));
                return;
            }
            else if (selectedItem.Name == "Herader")
            {
                ShowData._instance.UpdateData(HexFile.FileHeader());
            }
            else if (selectedItem.Name == "Optional Header")
            {
                ShowData._instance.UpdateData(HexFile.OptionalHeader());
            }
            else if (selectedItem.Name.StartsWith("."))
            {
                var data = HexFile.GetSectionsByName(selectedItem.Name);
                ShowData._instance.UpdateData(data);
                var bytesString = data.Find(x => x.Key == "Bytes");
                var bytes = Encoding.ASCII.GetBytes(bytesString.Value);
                OnSectionChange?.Invoke(this, new SideBarOnSectionChangeEventArgs(bytes));
            }
            else if (selectedItem.Name == "Imports")
            {
                ShowData._instance.UpdateData(HexFile.ImportsTable);
            }
            else if (selectedItem.Name == "Exports")
            {
                ShowData._instance.UpdateData(HexFile.ExportsTable);
            }
        }
        public void build()
        {
            treeSystem = new List<PeFileTree>();

            var top = new PeFileTree(HexFile.FileName, BootstrapIcon.Folder);
            treeSystem.Add(top);

            var Herader = new PeFileTree("Herader", BootstrapIcon.Envelope);
            var OptionalHeader = new PeFileTree("Optional Header", BootstrapIcon.Envelope);

            top.SubTree = new List<PeFileTree>
            {
                Herader,
                OptionalHeader
            };

            var sections = HexFile.getSections();
            if (sections.Count > 0)
            {
                var Section = new PeFileTree("Sections", BootstrapIcon.Envelope);
                Section.SubTree = new List<PeFileTree>();
                foreach (var s in sections)
                {
                    Section.SubTree.Add(new PeFileTree(s, BootstrapIcon.Envelope));
                }
                top.SubTree.Add(Section);
            }

            var imports = HexFile.getImports();
            if (imports.Count > 0)
            {                
                top.SubTree.Add(new PeFileTree("Imports",BootstrapIcon.Envelope));
            }

            var export = HexFile.getExports();
            if (export.Count > 0)
            {
                top.SubTree.Add(new PeFileTree("Exports", BootstrapIcon.Envelope));
            }

            OnSelectedItemChanged(new PeFileTree(HexFile.FileName, BootstrapIcon.Folder));
            StateHasChanged();
        }
    }
}
