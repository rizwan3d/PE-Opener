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

    public class LoadingStatusEventArgs : EventArgs
    {
        public string Status { get; private set; }

        public LoadingStatusEventArgs(string Status)
        {
            this.Status = Status;
        }
    }
}
