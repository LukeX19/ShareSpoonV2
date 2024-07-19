namespace ShareSpoon.Infrastructure.Options
{
    public class BlobStorageSettings
    {
        public string ConnectionString { get; set; }
        public string ContainerName { get; set; }
        public string BlobURLFormat { get; set; }
    }
}
