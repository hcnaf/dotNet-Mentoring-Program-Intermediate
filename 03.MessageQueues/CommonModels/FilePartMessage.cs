namespace CommonModels
{
    [Serializable]
    public class FilePartMessage
    {
        public long Id { get; set; }
        public long PartsAmount { get; set; }
        public string FileName { get; set; }
        public byte[] Data { get; set; }

    }
}