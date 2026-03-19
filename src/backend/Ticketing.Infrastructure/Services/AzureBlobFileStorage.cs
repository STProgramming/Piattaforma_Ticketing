using Azure.Storage.Blobs;

namespace Ticketing.Infrastructure.Services;

public sealed class AzureBlobFileStorage
{
    private readonly BlobServiceClient _blobServiceClient;

    public AzureBlobFileStorage(BlobServiceClient blobServiceClient)
    {
        _blobServiceClient = blobServiceClient;
    }

    public BlobServiceClient Client => _blobServiceClient;
}
