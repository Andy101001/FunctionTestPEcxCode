namespace ABMVantage_Outbound_API.Configuration
{
    /// <summary>
    /// Settings to connect to Cosmos DB.
    /// </summary>
    public class SecuritySettings
    {
        /// <summary>
        /// Gets or sets the endpoint.
        /// </summary>
        public string? KeyVaultName { get; set; }

        /// <summary>
        /// Gets or sets the access key.
        /// </summary>
        public string? AzureADApplicationId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether startup should check for migrations.
        /// </summary>
        public string? AzureADCertThumbprint { get; set; }

        /// <summary>
        /// Gets or sets the id of the document to check for migration.
        /// </summary>
        public string? AzureADDirectoryId { get; set; }
        /// <summary>
        /// Gets or sets the id of the document to check for migration.
        /// </summary>
        public string? KeyVaultUri { get; set; }

    }
}