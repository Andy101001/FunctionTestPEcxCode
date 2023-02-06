using Newtonsoft.Json;

namespace ABMVantage_Outbound_API.EntityModels
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class BookingReservation
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("bookingId")]
        public int BookingId { get; set; }

        [JsonProperty("reference")]
        public string? Reference { get; set; }

        [JsonProperty("originalReference")]
        public string? OriginalReference { get; set; }

        [JsonProperty("barrierReference")]
        public string? BarrierReference { get; set; }

        [JsonProperty("type")]
        public string? Type { get; set; }

        [JsonProperty("version")]
        public int? Version { get; set; }

        [JsonProperty("affiliate")]
        public string? Affiliate { get; set; }

        [JsonProperty("affiliateId")]
        public int? AffiliateId { get; set; }

        [JsonProperty("site")]
        public string? Site { get; set; }

        [JsonProperty("siteId")]
        public int? SiteId { get; set; }

        [JsonProperty("primary")]
        public bool? Primary { get; set; }

        [JsonProperty("createdDateTime")]
        public DateTime? CreatedDateTime { get; set; }

        [JsonProperty("bookingDateTime")]
        public DateTime? BookingDateTime { get; set; }

        [JsonProperty("entryDateTime")]
        public DateTime? EntryDateTime { get; set; }

        [JsonProperty("exitDateTime")]
        public DateTime? ExitDateTime { get; set; }

        [JsonProperty("title")]
        public string? Title { get; set; }

        [JsonProperty("firstName")]
        public string? FirstName { get; set; }

        [JsonProperty("lastName")]
        public string? LastName { get; set; }

        [JsonProperty("emailAddress")]
        public string? EmailAddress { get; set; }

        [JsonProperty("address1")]
        public string? Address1 { get; set; }

        [JsonProperty("address2")]
        public string? Address2 { get; set; }

        [JsonProperty("city")]
        public string? City { get; set; }

        [JsonProperty("postcode")]
        public string? Postcode { get; set; }

        [JsonProperty("phoneNumber")]
        public string? PhoneNumber { get; set; }

        [JsonProperty("emailOptIn")]
        public bool? EmailOptIn { get; set; }

        [JsonProperty("inboundFlightNumber")]
        public string? InboundFlightNumber { get; set; }

        [JsonProperty("outboundFlightNumber")]
        public string? OutboundFlightNumber { get; set; }

        [JsonProperty("carLicensePlate")]
        public string? CarLicensePlate { get; set; }

        [JsonProperty("carMake")]
        public string? CarMake { get; set; }

        [JsonProperty("carModel")]
        public string? CarModel { get; set; }

        [JsonProperty("carColour")]
        public string? CarColour { get; set; }

        [JsonProperty("carState")]
        public string? CarState { get; set; }

        [JsonProperty("carCountry")]
        public string? CarCountry { get; set; }

        [JsonProperty("product")]
        public string? Product { get; set; }

        [JsonProperty("productId")]
        public int? ProductId { get; set; }

        [JsonProperty("carPark")]
        public string? CarPark { get; set; }

        [JsonProperty("carParkId")]
        public int? CarParkId { get; set; }

        [JsonProperty("tickets")]
        public int? Tickets { get; set; }

        [JsonProperty("ticketUnitPrice")]
        public double? TicketUnitPrice { get; set; }

        [JsonProperty("adults")]
        public int? Adults { get; set; }

        [JsonProperty("adultUnitPrice")]
        public double? AdultUnitPrice { get; set; }

        [JsonProperty("children")]
        public int? Children { get; set; }

        [JsonProperty("childrenUnitPrice")]
        public double? ChildrenUnitPrice { get; set; }

        [JsonProperty("infants")]
        public int? Infants { get; set; }

        [JsonProperty("infantUnitPrice")]
        public double? InfantUnitPrice { get; set; }

        [JsonProperty("total")]
        public double? Total { get; set; }

        [JsonProperty("subTotal")]
        public double? SubTotal { get; set; }

        [JsonProperty("cancellationWaiver")]
        public double? CancellationWaiver { get; set; }

        [JsonProperty("bookingFee")]
        public double? BookingFee { get; set; }

        [JsonProperty("transactionId")]
        public string? TransactionId { get; set; }

        [JsonProperty("cardCharge")]
        public double? CardCharge { get; set; }

        [JsonProperty("paymentMethod")]
        public string oPaymentMethod { get; set; }

        [JsonProperty("cardType")]
        public string? CardType { get; set; }

        [JsonProperty("obscuredCardNumber")]
        public string? ObscuredCardNumber { get; set; }

        [JsonProperty("cardExpiryDate")]
        public string? CardExpiryDate { get; set; }

        [JsonProperty("amendedDateTime")]
        public string? AmendedDateTime { get; set; }

        [JsonProperty("cancellationDateTime")]
        public string? CancellationDateTime { get; set; }

        [JsonProperty("vatRate")]
        public double? VatRate { get; set; }

        [JsonProperty("vatAmount")]
        public double? VatAmount { get; set; }

        [JsonProperty("stateTaxRate")]
        public double? StateTaxRate { get; set; }

        [JsonProperty("stateTaxAmount")]
        public double? StateTaxAmount { get; set; }

        [JsonProperty("cityTaxRate")]
        public double? CityTaxRate { get; set; }

        [JsonProperty("taxAmount")]
        public double? TaxAmount { get; set; }

        [JsonProperty("promotionId")]
        public int? PromotionId { get; set; }

        [JsonProperty("promotionName")]
        public string? PromotionName { get; set; }

        [JsonProperty("promoCode")]
        public string? PromoCode { get; set; }

        [JsonProperty("contactId")]
        public int? ContactId { get; set; }

        [JsonProperty("hasAccount")]
        public bool? HasAccount { get; set; }

        [JsonProperty("accountCreated")]
        public DateTime ?AccountCreated { get; set; }

        [JsonProperty("contactCreated")]
        public DateTime? ContactCreated { get; set; }

        [JsonProperty("bookingLoyaltyPoints")]
        public double? BookingLoyaltyPoints { get; set; }

        [JsonProperty("currentLoyaltyPoints")]
        public double? CurrentLoyaltyPoints { get; set; }

        [JsonProperty("loyaltyTier")]
        public string? LoyaltyTier { get; set; }

        [JsonProperty("status")]
        public string? Status { get; set; }

        [JsonProperty("partnerCompany")]
        public string? PartnerCompany { get; set; }

        [JsonProperty("partnerCode")]
        public string? PartnerCode { get; set; }

        [JsonProperty("paymentOwner")]
        public string? PaymentOwner { get; set; }

        [JsonProperty("company")]
        public string? Company { get; set; }

        [JsonProperty("language")]
        public string? Language { get; set; }

        [JsonProperty("partnerBookingReference")]
        public string? PartnerBookingReference { get; set; }

        [JsonProperty("invoiceNumber")]
        public string? InvoiceNumber { get; set; }

        [JsonProperty("dropOff")]
        public string? DropOff { get; set; }

        [JsonProperty("tariffId")]
        public int? TariffId { get; set; }

        [JsonProperty("tariffName")]
        public string? TariffName { get; set; }

        [JsonProperty("country")]
        public string? Country { get; set; }

        [JsonProperty("amountRefunded")]
        public double? AmountRefunded { get; set; }

        [JsonProperty("actualEntryDateTime")]
        public string? ActualEntryDateTime { get; set; }

        [JsonProperty("actualExitDateTime")]
        public string? ActualExitDateTime { get; set; }

        [JsonProperty("cancellationFee")]
        public double? CancellationFee { get; set; }

        [JsonProperty("state")]
        public string? State { get; set; }

        [JsonProperty("parkingStatus")]
        public string? ParkingStatus { get; set; }

        [JsonProperty("agentName")]
        public string? AgentName { get; set; }

        [JsonProperty("commission")]
        public double? Commission { get; set; }

        [JsonProperty("unmodifiedPrice")]
        public double? UnmodifiedPrice { get; set; }

        [JsonProperty("yieldUplift")]
        public double? YieldUplift { get; set; }

        [JsonProperty("utmSource")]
        public string? UtmSource { get; set; }

        [JsonProperty("utmMedium")]
        public string? UtmMedium { get; set; }

        [JsonProperty("utmCampaign")]
        public string? UtmCampaign { get; set; }

        [JsonProperty("utmTerm")]
        public string? UtmTerm { get; set; }

        [JsonProperty("utmContent")]
        public string? UtmContent { get; set; }

        [JsonProperty("internalLink")]
        public string? InternalLink { get; set; }

        [JsonProperty("misc")]
        public string? Misc { get; set; }

        [JsonProperty("encryptedBookingId")]
        public string? EncryptedBookingId { get; set; }

        [JsonProperty("airline")]
        public string? Airline { get; set; }

        [JsonProperty("reasonForTravel")]
        public string? ReasonForTravel { get; set; }

        [JsonProperty("destination")]
        public string? Destination { get; set; }

        [JsonProperty("promoDiscount")]
        public double? PromoDiscount { get; set; }

        [JsonProperty("invoiceGuid")]
        public string? InvoiceGuid { get; set; }

        [JsonProperty("creditSubTotal")]
        public double? CreditSubTotal { get; set; }

        [JsonProperty("creditCityTaxAmount")]
        public double? CreditCityTaxAmount { get; set; }

        [JsonProperty("creditStateTaxAmount")]
        public double? CreditStateTaxAmount { get; set; }

        [JsonProperty("creditVatAmount")]
        public double? CreditVatAmount { get; set; }

        [JsonProperty("isoCode")]
        public string? IsoCode { get; set; }

        [JsonProperty("upgradeName")]
        public string? UpgradeName { get; set; }

        [JsonProperty("upgradeAmount")]
        public double? UpgradeAmount { get; set; }

        [JsonProperty("upgradeFromProductName")]
        public string? UpgradeFromProductName { get; set; }

        [JsonProperty("companyReceipt")]
        public string? CompanyReceipt { get; set; }

        [JsonProperty("address1Receipt")]
        public string? Address1Receipt { get; set; }

        [JsonProperty("address2Receipt")]
        public string? Address2Receipt { get; set; }

        [JsonProperty("postcodeReceipt")]
        public string? PostcodeReceipt { get; set; }

        [JsonProperty("townReceipt")]
        public string? TownReceipt { get; set; }

        [JsonProperty("countyReceipt")]
        public string? CountyReceipt { get; set; }

        [JsonProperty("countryReceipt")]
        public string? CountryReceipt { get; set; }

        [JsonProperty("countryCodeReceipt")]
        public string? CountryCodeReceipt { get; set; }

        [JsonProperty("taxIdentificationNumber")]
        public string? TaxIdentificationNumber { get; set; }

        [JsonProperty("paymentDifference")]
        public double? PaymentDifference { get; set; }
    }

    public class Booking
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("bookings")]
        public List<BookingReservation>? BookingReservations { get; set; }
    }
}