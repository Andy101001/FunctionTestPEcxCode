using ABMVantage.Data.EntityModels;
using ABMVantage.Data.Interfaces;
using ABMVantage.Data.Models;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;

namespace ABMVantage.Data.Service
{
    public  class SingleTicketEVChargesService : ISingleTicketEVChargesService
    {
        private readonly ILogger<SingleTicketEVChargesService> _logger;
        private readonly CosmosClient _client;
        private readonly Container _SingleTicketEVContainer;

        public SingleTicketEVChargesService(ILoggerFactory loggerFactory)
        {
            ArgumentNullException.ThrowIfNull(loggerFactory);
            _logger = loggerFactory.CreateLogger<SingleTicketEVChargesService>();
            _client = new CosmosClient("https://abm-vtg-cosmos01-dev.documents.azure.com:443/", "6CYkip2ZaFNGEsWy1JXWFe4LuV1fAJOOVDHeooIjmOWnxizz9BbWAkah3MEnjb8014upO3D91wuuACDb4rR0xg==");
           _SingleTicketEVContainer = _client.GetDatabase("VantageDB_UI").GetContainer("SingleTicketEv");
        }

       /// <summary>
       /// GetEVCharges
       /// </summary>
       /// <param name="request"></param>
       /// <returns></returns>
        public async Task<SingleTicketEVChargesResponse> GetEVCharges(SingleTicketEVChargesRequest request)
        {
            //TicketID:TKT_DXNK1ZHNR,LPN:CDG3498
            SingleTicketEVChargesResponse? response = new();
            try
            {
                string cosmosQuery = $"SELECT TOP 1 * FROM c WHERE";
                if (!string.IsNullOrEmpty(request.Lpn))
                    cosmosQuery += $" c.Lpn = '{request.Lpn}'";
                else if (!string.IsNullOrEmpty(request.TicketId))
                    cosmosQuery += $" c.TicketId = '{request.TicketId}'";
                cosmosQuery += $" ORDER BY c.TimeStampUTC DESC";

                response = await CalculateEVCharges(_SingleTicketEVContainer, cosmosQuery);
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
            return response;
        }

        private async Task<SingleTicketEVChargesResponse> CalculateEVCharges(Container container, string cosmosQuery)
        {
            SingleTicketEVChargesResponse cResponse = new();
            var results = new List<SingleTicketEV>();
            try
            {
                var feedIterator = container.GetItemQueryIterator<SingleTicketEV>(new QueryDefinition(cosmosQuery));
                while (feedIterator.HasMoreResults)
                {
                    var qresponse = await feedIterator.ReadNextAsync();
                    results.AddRange(qresponse.ToList());
                }
                if (results.Count > 0)
                {
                    SingleTicketEV firstItem = results[0]; //We fetched only TOP 1 record from DB
                    EvChargeSession? activeSession = firstItem.EvChargeSessions.FirstOrDefault( x => x.ChargeStartDateTimeUTC.HasValue && x.ChargeEndDateTimeUTC.HasValue);
                    cResponse = new SingleTicketEVChargesResponse()
                    {
                        SingleTicketEvId = firstItem.SingleTicketEvId,
                        ChargeSessionId = firstItem.ChargeSessionId,
                        ClientId = firstItem.ClientId,
                        TicketId = firstItem.TicketId,
                        Lpn = firstItem.Lpn,
                        RawTicketId = firstItem.RawTicketId,
                        ParkingSpaceId = firstItem.ParkingSpaceId,
                        SessionStartTimeInUtc = activeSession!.ChargeStartDateTimeUTC,
                        SessionEndTimeInUtc = activeSession!.ChargeStartDateTimeUTC,
                        EvChargingFee = activeSession!.Fee,
                        TotalKWHoursDelivered = activeSession.KWHoursDelivered
                    };
                }
            }catch (Exception)
            {
                throw;
            }
            return cResponse;
        }
    }
}
