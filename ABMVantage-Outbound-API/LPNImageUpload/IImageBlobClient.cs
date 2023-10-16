namespace ABMVantage_Outbound_API.LPRImageUpload
{
    public interface IImageBlobClient
    {
        /// <summary>
        /// Async Image Upload from Stream
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="stream"></param>
        /// <returns></returns>
        Task UploadBlobAsync(string filename, Stream stream);
    }
}
