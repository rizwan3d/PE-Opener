namespace PEOpener.Infrastuture
{
    public class SideBarOnSectionChangeEventArgs : EventArgs
    {
        public byte[] Bytes { get; private set; }

        public SideBarOnSectionChangeEventArgs(byte[] bytes)
        {
            Bytes = bytes;
        }
    }
}
