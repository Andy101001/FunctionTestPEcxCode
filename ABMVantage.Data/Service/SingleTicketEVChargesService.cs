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
        private readonly Container _RawEvActiveSessionsContainer;
        private readonly Container _RawEvClosedSessionsContainer;

        public SingleTicketEVChargesService(ILoggerFactory loggerFactory)
        {
            ArgumentNullException.ThrowIfNull(loggerFactory);
            _logger = loggerFactory.CreateLogger<SingleTicketEVChargesService>();
            _client = new CosmosClient("https://abm-vtg-cosmos01-dev.documents.azure.com:443/", "6CYkip2ZaFNGEsWy1JXWFe4LuV1fAJOOVDHeooIjmOWnxizz9BbWAkah3MEnjb8014upO3D91wuuACDb4rR0xg==");
            _RawEvActiveSessionsContainer = _client.GetDatabase("VantageDB").GetContainer("RawEvActiveSessions");
            _RawEvClosedSessionsContainer = _client.GetDatabase("VantageDB").GetContainer("RawEvClosedSessions");
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

                // Always first Check for Closed Session Event,
                // If the Response is  NULL then Get the Latest Active Session
                response = await CalculateEVCharges(_RawEvClosedSessionsContainer, cosmosQuery);
                if (response == null)
                    response = await CalculateEVCharges(_RawEvActiveSessionsContainer, cosmosQuery);
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
            return response;
        }

        private async Task<SingleTicketEVChargesResponse> CalculateEVCharges(Container container, string cosmosQuery)
        {
            SingleTicketEVChargesResponse cResponse = null;
            var results = new List<EVSession>();
            try
            {
                var feedIterator = container.GetItemQueryIterator<EVSession>(new QueryDefinition(cosmosQuery));
                while (feedIterator.HasMoreResults)
                {
                    var qresponse = await feedIterator.ReadNextAsync();
                    results.AddRange(qresponse.ToList());
                }
                if (results.Count > 0)
                {
                    EVSession firstItem = results[0]; //We fetched only TOP 1 record from DB
                    cResponse = new SingleTicketEVChargesResponse()
                    {
                        Id = firstItem.id,
                        SessionId = firstItem.ChargeSessionId,
                        SessionStartTimeInUtc = firstItem.ChargeStartDateTimeUTC,
                        SessionEndTimeInUtc = firstItem.ChargeEndDateTimeUTC,
                        EvChargingStationID = firstItem.ChargerId,
                        TotalKwhUsed = firstItem.KWHoursDelivered,
                        TicketId = firstItem.TicketId,
                        Lpn = firstItem.Lpn
                    };
                }
            }catch (Exception ex)
            {
                throw ex;
            }
            return cResponse;

        }
    }
}
