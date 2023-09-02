using System;
using teste.ApiCore31.Helpers;
using teste.ApiCore31.Models;

namespace teste.ApiCore31.Mockupping
{
    /// <summary>
    /// Class for mockupping something needed for application use
    /// </summary>
    public static class Mock
    {
        /// <summary>
        /// The Sale entity mockup
        /// </summary>
        public static Sale MockSaleEntity => new Sale
        {
            AccountID = "Y9572700698484288",
            TransactionID = "3SWVJA0R-MZ11-RM1T-E0BJ-XACU1NOQQRNC",
            TransactionAmount = Convert.ToDouble("697.31"),
            TransactionCurrencyCode = Convert.ToString("BRL"),
            LocalHour = Convert.ToInt64("14"),
            TransactionScenario = 'A',
            TransactionType = 'P',
            TransactionIPaddress = Convert.ToDecimal("91.119"),
            IpState = Convert.ToString("wien"),
            IpPostalCode = Convert.ToString("1000"),
            IpCountry = Convert.ToString("at"),
            IsProxyIP = "FALSE".ToLower() == "true" ? 1 : 0,
            BrowserLanguage = "de-AT",
            PaymentInstrumentType = "CREDITCARD",
            CardType = "VISA",
            PaymentBillingPostalCode = "1220",
            PaymentBillingState = "AT",
            PaymentBillingCountryCode = "1220",
            ShippingPostalCode = "",
            ShippingState = "AT",
            ShippingCountry = "M",
            CvvVerifyResult = "0",
            DigitalItemCount = Convert.ToInt64("25"),
            PhysicalItemCount = Convert.ToInt64("25"),
            TransactionDateTime = TreatmentHelper.TreatDateFromText("10/18/2007 12:52")
        };
    }
}