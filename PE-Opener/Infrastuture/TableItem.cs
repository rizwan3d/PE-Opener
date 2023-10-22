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
}
