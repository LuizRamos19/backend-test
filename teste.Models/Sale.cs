using System;
using System.Data;
using System.Text.Json.Serialization;

namespace teste.Models
{
    /// <summary>
    /// Sale class
    /// </summary>
    public class Sale : DataBaseEntity
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public Sale(){}

        /// <summary>
        /// Build constructor for data base dada
        /// </summary>
        /// <param name="table">Table where data came from</param>
        /// <param name="indexRow">Index row for each item</param>
        /// <exception cref="Exception">Could result some exceptions due required property if data is null</exception>
        public Sale(DataTable table, int indexRow)
        {
            #region-- Populate Object Properties --
            if (ColumnExists(table, "id"))
            {
                if (table.Rows[indexRow]["id"] == DBNull.Value)
                {
                    throw new Exception("Id can't be null!");
                }
                Id = Convert.ToInt64(table.Rows[indexRow]["id"]);
            }

            if (ColumnExists(table, "account_id"))
            {
                if (table.Rows[indexRow]["account_id"] == DBNull.Value)
                {
                    throw new Exception("Account Id can't be null!");
                }
                AccountID = table.Rows[indexRow]["account_id"].ToString();
            }

            if (ColumnExists(table, "transaction_id"))
            {
                if (table.Rows[indexRow]["transaction_id"] == DBNull.Value)
                {
                    throw new Exception("Transaction Id can't be null!");
                }
                TransactionID = table.Rows[indexRow]["transaction_id"].ToString();
            }
            		
            if (ColumnExists(table, "transaction_amount_usd"))
            {
                if (table.Rows[indexRow]["transaction_amount_usd"] == DBNull.Value)
                {
                    throw new Exception("Transaction amount in USD can't be null!");
                }
                TransactionAmount = Convert.ToDouble(table.Rows[indexRow]["transaction_amount_usd"]);
            }

            if (ColumnExists(table, "transaction_currency_code"))
            {
                if (table.Rows[indexRow]["transaction_currency_code"] == DBNull.Value)
                {
                    throw new Exception("Transaction currency code can't be null!");
                }
                TransactionCurrencyCode = table.Rows[indexRow]["transaction_currency_code"].ToString();
            }

            if (ColumnExists(table, "local_hour"))
            {
                if (table.Rows[indexRow]["local_hour"] == DBNull.Value)
                {
                    throw new Exception("Local hour can't be null!");
                }
                LocalHour = Convert.ToInt64(table.Rows[indexRow]["local_hour"]);
            }

            if (ColumnExists(table, "transaction_scenario"))
            {
                if (table.Rows[indexRow]["transaction_scenario"] == DBNull.Value)
                {
                    throw new Exception("Transaction scenario can't be null!");
                }
                TransactionScenario = Convert.ToChar(table.Rows[indexRow]["transaction_scenario"]);
            }

            if (ColumnExists(table, "transaction_type"))
            {
                if (table.Rows[indexRow]["transaction_type"] == DBNull.Value)
                {
                    throw new Exception("Transaction type can't be null!");
                }
                TransactionType = Convert.ToChar(table.Rows[indexRow]["transaction_type"]);
            }

            if (ColumnExists(table, "transaction_ip_address"))
            {
                if (table.Rows[indexRow]["transaction_ip_address"] == DBNull.Value)
                {
                    throw new Exception("Transaction IP address can't be null!");
                }
                TransactionIPaddress = Convert.ToDecimal(table.Rows[indexRow]["transaction_ip_address"]);
            }

            if (ColumnExists(table, "ip_state"))
            {
                if (table.Rows[indexRow]["ip_state"] == DBNull.Value)
                {
                    throw new Exception("Transaction IP state can't be null!");
                }
                IpState = table.Rows[indexRow]["ip_state"].ToString();
            }

            if (ColumnExists(table, "ip_postal_code"))
            {
                if (table.Rows[indexRow]["ip_postal_code"] == DBNull.Value)
                {
                    throw new Exception("Transaction IP postal code can't be null!");
                }
                IpPostalCode = table.Rows[indexRow]["ip_postal_code"].ToString();
            }

            if (ColumnExists(table, "ip_country"))
            {
                if (table.Rows[indexRow]["ip_country"] == DBNull.Value)
                {
                    throw new Exception("Transaction IP country can't be null!");
                }
                IpCountry = table.Rows[indexRow]["ip_country"].ToString();
            }

            if (ColumnExists(table, "is_proxy_ip"))
            {
                if (table.Rows[indexRow]["is_proxy_ip"] == DBNull.Value)
                {
                    throw new Exception("If is proxy IP can't be null!");
                }
                IsProxyIP = Convert.ToInt32(table.Rows[indexRow]["is_proxy_ip"]);
            }

            if (ColumnExists(table, "browser_language"))
            {
                if (table.Rows[indexRow]["browser_language"] == DBNull.Value)
                {
                    throw new Exception("Browser language can't be null!");
                }
                BrowserLanguage = table.Rows[indexRow]["browser_language"].ToString();
            }

            if (ColumnExists(table, "payment_instrument_type"))
            {
                if (table.Rows[indexRow]["payment_instrument_type"] == DBNull.Value)
                {
                    throw new Exception("Payment instrument type can't be null!");
                }
                PaymentInstrumentType = table.Rows[indexRow]["payment_instrument_type"].ToString();
            }

            if (ColumnExists(table, "card_type"))
            {
                CardType = table.Rows[indexRow]["card_type"].ToString();
            }

            if (ColumnExists(table, "payment_billing_postal_code"))
            {
                PaymentBillingPostalCode = table.Rows[indexRow]["payment_billing_postal_code"].ToString();
            }

            if (ColumnExists(table, "payment_billing_state"))
            {
                PaymentBillingState = table.Rows[indexRow]["payment_billing_state"].ToString();
            }

            if (ColumnExists(table, "payment_billing_country_code"))
            {
                PaymentBillingCountryCode = table.Rows[indexRow]["payment_billing_country_code"].ToString();
            }

            if (ColumnExists(table, "shipping_postal_code"))
            {
                ShippingPostalCode = table.Rows[indexRow]["shipping_postal_code"].ToString();
            }

            if (ColumnExists(table, "shipping_state"))
            {
                ShippingState = table.Rows[indexRow]["shipping_state"].ToString();
            }

            if (ColumnExists(table, "shipping_country"))
            {
                ShippingCountry = table.Rows[indexRow]["shipping_country"].ToString();
            }

            if (ColumnExists(table, "cvv_verify_result"))
            {
                CvvVerifyResult = table.Rows[indexRow]["cvv_verify_result"].ToString();
            }

            if (ColumnExists(table, "digital_item_count"))
            {
                if (table.Rows[indexRow]["digital_item_count"] == DBNull.Value)
                {
                    throw new Exception("Digital item count can't be null!");
                }
                DigitalItemCount = Convert.ToInt64(table.Rows[indexRow]["digital_item_count"]);
            }

            if (ColumnExists(table, "physical_item_count"))
            {
                if (table.Rows[indexRow]["physical_item_count"] == DBNull.Value)
                {
                    throw new Exception("Physical item count can't be null!");
                }
                PhysicalItemCount = Convert.ToInt64(table.Rows[indexRow]["physical_item_count"]);
            }

            if (ColumnExists(table, "transaction_date_time"))
            {
                if (table.Rows[indexRow]["transaction_date_time"] == DBNull.Value)
                {
                    throw new Exception("Transaction date time can't be null!");
                }
                TransactionDateTime = Convert.ToDateTime(table.Rows[indexRow]["transaction_date_time"]);
            }
            #endregion
        }

        #region-- Object Properties --
        /// <summary>
        /// Id
        /// </summary>
        [JsonPropertyName("id")]
        public Int64 Id { get; set;}

        /// <summary>
        /// Accound Id
        /// </summary>
        [JsonPropertyName("account_id")] 
        public string AccountID { get; set;}

        /// <summary>
        /// Id of Transaction
        /// </summary>
        [JsonPropertyName("transaction_id")]
        public string TransactionID { get; set;}

        /// <summary>
        /// Amount of the transation in US dollar
        /// </summary>
        [JsonPropertyName("transaction_amount_usd")]
        public double TransactionAmount { get; set;}

        /// <summary>
        /// The currency code of transaction
        /// </summary>
        [JsonPropertyName("transaction_currency_code")]
        public string TransactionCurrencyCode { get; set;}

        /// <summary>
        /// Place local hour of the transaction
        /// </summary>
        [JsonPropertyName("local_hour")]
        public Int64 LocalHour { get; set;}

        /// <summary>
        /// Transaction scenario
        /// </summary>
        [JsonPropertyName("transaction_scenario")]
        public char TransactionScenario { get; set;}

        /// <summary>
        /// Type of the transaction
        /// </summary>
        [JsonPropertyName("transaction_type")]
        public char TransactionType { get; set;}

        /// <summary>
        /// Device IP address where transaction happens 
        /// </summary>
        [JsonPropertyName("transaction_ip_address")]
        public decimal TransactionIPaddress { get; set;}

        /// <summary>
        /// The IP federation unity
        /// </summary>
        [JsonPropertyName("ip_state")]
        public string IpState { get; set;}

        /// <summary>
        /// The IP postal code for local where transaction happens
        /// </summary>
        [JsonPropertyName("ip_postal_code")]
        public string IpPostalCode { get; set;}

        /// <summary>
        /// The country for registered IP where transaction happens
        /// </summary>
        [JsonPropertyName("ip_country")]
        public string IpCountry { get; set;}

        /// <summary>
        /// Check if transaction happened under a proxy IP
        /// </summary>
        [JsonPropertyName("is_proxy_ip")]
        public int IsProxyIP { get; set;}

        /// <summary>
        /// The language of used browser
        /// </summary>
        [JsonPropertyName("browser_language")]
        public string BrowserLanguage { get; set;}

        /// <summary>
        /// The payment instrument type
        /// </summary>
        [JsonPropertyName("payment_instrument_type")]
        public string PaymentInstrumentType { get; set;}

        /// <summary>
        /// The type of the credit/debit card
        /// </summary>
        [JsonPropertyName("card_type")]
        #nullable enable
        public string? CardType { get; set;}

        /// <summary>
        /// The postal code for the billing address
        /// </summary>
        [JsonPropertyName("payment_billing_postal_code")]
        public string? PaymentBillingPostalCode { get; set;}

        /// <summary>
        /// The federation unity for the payment billing
        /// </summary>
        [JsonPropertyName("payment_billing_state")]
        public string? PaymentBillingState { get; set;}

        /// <summary>
        /// The ISO code for the country where the billing address is
        /// </summary>
        [JsonPropertyName("payment_billing_country_code")]
        public string? PaymentBillingCountryCode { get; set;}

        /// <summary>
        /// The shipping postal code
        /// </summary>
        [JsonPropertyName("shipping_postal_code")]
        public string? ShippingPostalCode { get; set;}

        /// <summary>
        /// The shipping federation unity
        /// </summary>
        [JsonPropertyName("shipping_state")]
        public string? ShippingState { get; set;}

        /// <summary>
        /// The shipping country
        /// </summary>
        [JsonPropertyName("shipping_country")]
        public string? ShippingCountry { get; set;}

        /// <summary>
        /// The Card Verification Value result
        /// </summary>
        [JsonPropertyName("cvv_verify_result")]
        public string? CvvVerifyResult { get; set;}

        /// <summary>
        /// Amount of items in a digital measurement
        /// </summary>
        [JsonPropertyName("digital_item_count")]
        public Int64 DigitalItemCount { get; set;}

        /// <summary>
        /// Amount of items in a empirical meaning
        /// </summary>
        [JsonPropertyName("physical_item_count")]
        public Int64 PhysicalItemCount { get; set;}

        /// <summary>
        /// The date when transaction happened
        /// </summary>
        [JsonPropertyName("transaction_date_time")]
        public DateTime TransactionDateTime { get; set;}

        #endregion
    }
}