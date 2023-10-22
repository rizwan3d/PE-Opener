using PEOpener.Infrastuture;

namespace PEOpener.Components
{
    public partial class ShowData
    {
        private List<TableItem>? Items { get; set; }

        public static ShowData _instance;

        public ShowData()
        {
            if (_instance is null)
                _instance = this;
        }

        public void UpdateData(List<TableItem> data)
        {
            Items = data;
            StateHasChanged();
        }
    }
}
