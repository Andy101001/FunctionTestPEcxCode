using ABMVantage.Data.EntityModels.SQL;
using ABMVantage.Data.Interfaces;
using ABMVantage.Data.Models;
using ABMVantage_Outbound_API.LPRImageUpload;
using HttpMultipartParser;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace ABMVantage_Outbound_API.Functions
{
    public class VehicleLPNImageWebhook
    {
        private readonly ILogger _logger;
        private readonly ILPNImageCaptureMetadataService _mService;
        private readonly IImageBlobClient _imageBlobClient;

        public VehicleLPNImageWebhook(ILoggerFactory loggerFactory, ILPNImageCaptureMetadataService mService, IImageBlobClient imageBlobCliet)
        {
            _logger = loggerFactory.CreateLogger<VehicleLPNImageWebhook>();
            _mService = mService;
            _imageBlobClient = imageBlobCliet;
        }

        [Function("ABM Vantage - Vehicle LPN Image Capture")]
        [OpenApiOperation(operationId: "VehicleLPNImageWebhook", tags: new[] { "ABM Vantage" }, Summary = "Capture the Image Blob and store in DB and Blob Storage", Description = "Capture the Image Blob and store in DB and Blob Storage")]
        [OpenApiRequestBody(contentType: "json", bodyType: typeof(FilterParam), Description = "Parameters")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(string), Summary = "Capture the Image Blob and store in DB and Blob Storage", Description = "Capture the Image Blob and store in DB and Blob Storage")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid Request", Description = "Invalid Request")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.MethodNotAllowed, Summary = "Validation exception", Description = "Validation exception")]

        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "vehiclelpnImageWebhook")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            var content = await new StreamReader(req.Body).ReadToEndAsync();

            try
            {
                WebhookInput? request = JsonConvert.DeserializeObject<WebhookInput>(content);
                string deCodedString = ExtensionMethods.DecodeBase64(request!.body!);
                using (var stream = GenerateStreamFromString(deCodedString))
                {
                    var parser = await MultipartFormDataParser.ParseAsync(stream, Encoding.UTF8).ConfigureAwait(false);
                    // Loop through all the files
                    foreach (var file in parser.Files)
                    {
                        if (file.Name == "Events" && file.FileName.Contains(".json"))
                        {
                            string sData = GenerateStringFromStream(file.Data);
                            EventInputPayload? payload = JsonConvert.DeserializeObject<EventInputPayload>(sData);
                            var dbInput = new LPNImageCaptureMetaData()
                            {
                                EventName = payload!.EventName,
                                UID = payload!.UID,
                                AreaType = payload.AreaType,
                                Mode = payload.Mode,
                                EnterUTC = payload.EnterUTC,
                                ExitUTC = payload!.ExitUTC,
                                TimeStampUTC = payload!.TimeStampUTC,
                                LPRTimeStampUTC = payload!.LPR!.LPRTimeStampUTC,
                                LPR = payload!.LPR!.LPR,
                                LPRCode = payload!.LPR!.LPRCode,
                                LPN = payload!.LPR!.LPN,
                                Occupied = payload!.LPR!.Occupied,
                                CreatedDate = DateTime.Now,
                                CreatedBy = "VehicleLPNImageWebhook-AzureFunction"
                            };
                            _mService.Insert(dbInput);
                        }
                        else if(file.Name == "image" && file.FileName.Contains(".png"))
                        {
                            string sfileName = file.FileName.Replace(".png", ".jpeg");
                            await _imageBlobClient.UploadBlobAsync(sfileName, file.Data);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(VehicleLPNImageWebhook)} Missing or invalid request object {ex.Message}");
                return new BadRequestObjectResult("Missing or invalid request object.");

            }
            return new OkObjectResult("Success");
        }

        public static Stream GenerateStreamFromString(string s)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream, Encoding.Default);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        public static string GenerateStringFromStream(Stream stream)
        {
            stream.Position = 0;
            using (StreamReader reader = new StreamReader(stream, Encoding.Default))
            {
                return reader.ReadToEnd();
            }
        }
    }

    public static class ExtensionMethods
    {
        public static string DecodeBase64(this string value)
        {
            var valueBytes = Convert.FromBase64String(value);
            return Encoding.Default.GetString(valueBytes);
        }
    }

    public class WebhookInput
    {  
        public string? body { get; set; }
    }

    public class EventInputPayload
    {
        public string? EventName { get; set; }
        public string? UID { get; set; }
        public int AreaType { get; set; }
        public int Mode { get; set; }
        public DateTime? EnterUTC { get; set; }
        public DateTime? ExitUTC { get; set; }
        public DateTime? TimeStampUTC { get; set; }
        public LicensePlateRead? LPR { get; set; }
    }

    public class LicensePlateRead
    {
        public DateTime? LPRTimeStampUTC { get; set; }
        public string? LPR { get; set; }
        public int LPRCode { get; set; }
        public string? LPN { get; set; }
        public bool Occupied { get; set; }
    }

}
