namespace ABMVantage_Outbound_API.Services
{
    using ABMVantage_Outbound_API.EntityModels;
    using Microsoft.Extensions.Logging;
    using System.Threading.Tasks;
    
    /// <summary>
    /// Parcs Ticket Transactions Service
    /// </summary>
    public class ParcsTicketTransactionsService : IParcsTicketTransactionsService
    {
        private readonly ILogger<ParcsTicketTransactionsService> _logger;
        private readonly IDataAccessService _dataAccessService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ParcsTicketTransactionsService"/> class.
        /// </summary>
        /// <param name="loggerFactory">logging</param>
        /// <param name="dataAccessService">data access</param>
        public ParcsTicketTransactionsService(ILoggerFactory loggerFactory, IDataAccessService dataAccessService)
        {
            ArgumentNullException.ThrowIfNull(dataAccessService);
            ArgumentNullException.ThrowIfNull(loggerFactory);
            
            _logger = loggerFactory.CreateLogger<ParcsTicketTransactionsService>();
            _dataAccessService = dataAccessService;
            
            _logger.LogInformation("Constructing Parcs Ticket Transactions Service");
        }

        /// <summary>
        /// Get all the parcs ticket transactions and apply biz rules
        /// </summary>
        /// <returns>ActiveClosedEvChargingSession</returns>
        public async Task<List<ParcsTicketsTransactions>> GetParcsTicketTransactionsAsync()
        {
            _logger.LogInformation("Getting Parcs Ticket Transactions from the DataService");

            var parcsTicketsTransactions = await _dataAccessService.GetParcsTicketTransactionsAsync().ConfigureAwait(false);

            return parcsTicketsTransactions;
        }
    }
}