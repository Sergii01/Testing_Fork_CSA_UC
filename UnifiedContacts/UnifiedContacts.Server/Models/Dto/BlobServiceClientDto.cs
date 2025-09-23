using Azure.Storage.Blobs;

namespace UnifiedContacts.Models.Dto
{
    public class BlobServiceDto
    {
        public BlobServiceClient? Client { get; set; }
    }
}
