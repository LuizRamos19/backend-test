using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using teste.Models;

namespace teste.Helpers
{
    /// <summary>
    /// Helper class for files
    /// </summary>
    public static class FileHelper
    {
        /// <summary>
        /// To find root path in the solution where read or write a file
        /// </summary>
        /// <param name="fileName">The file name. Shoul have te extension file! as: "txt", "json", whatever...</param>
        /// <returns>Full path to write or read a file</returns>
        private static string BuildFileRootPath(string fileName) =>
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory[..AppDomain.CurrentDomain.BaseDirectory.IndexOf(AppDomain.CurrentDomain.FriendlyName)], fileName);
        
        /// <summary>
        /// Create a list of Sale from text file rows
        /// </summary>
        /// <returns>List<Sale></returns>
        public static async Task<List<Sale>> SalesListFromFile()
        {
            var list = new List<Sale>();
            var jumpedHeader = false;
            string path = BuildFileRootPath("Sales.txt");
            using (StreamReader reader = new StreamReader(path))
            {
                while (true)
                {
                    string line = await reader.ReadLineAsync();
                    if (line == null) break;
                    if (!jumpedHeader)
                    {
                        jumpedHeader = true;
                        continue;
                    }
                    var fields = line.Split(',');
                    list.Add(
                        new Sale
                        {
                            AccountID = Convert.ToString(fields[0]),
                            TransactionID = Convert.ToString(fields[1]),
                            TransactionAmount = Convert.ToDouble(fields[2]),
                            TransactionCurrencyCode = Convert.ToString(fields[3]),
                            LocalHour = Convert.ToInt64(fields[4]),
                            TransactionScenario = Convert.ToChar(fields[5]),
                            TransactionType = Convert.ToChar(fields[6]),
                            TransactionIPaddress = Convert.ToDecimal(fields[7]),
                            IpState = Convert.ToString(fields[8]),
                            IpPostalCode = Convert.ToString(fields[9]),
                            IpCountry = Convert.ToString(fields[10]),
                            IsProxyIP = fields[11].ToLower() == "true" ? 1 : 0,
                            BrowserLanguage = Convert.ToString(fields[12]),
                            PaymentInstrumentType = Convert.ToString(fields[13]),
                            CardType = Convert.ToString(fields[14]),
                            PaymentBillingPostalCode = Convert.ToString(fields[15]),
                            PaymentBillingState = Convert.ToString(fields[16]),
                            PaymentBillingCountryCode = Convert.ToString(fields[17]),
                            ShippingPostalCode = Convert.ToString(fields[18]),
                            ShippingState = Convert.ToString(fields[19]),
                            ShippingCountry = Convert.ToString(fields[20]),
                            CvvVerifyResult = Convert.ToString(fields[21]),
                            DigitalItemCount = Convert.ToInt64(fields[22]),
                            PhysicalItemCount = Convert.ToInt64(fields[23]),
                            TransactionDateTime = TreatmentHelper.TreatDateFromText(fields[24])
                        }
                    );
                }
            }
            return list;
        }

        /// <summary>
        /// Create a json file from sales read from Sales.txt
        /// </summary>
        public static async Task SalesListToJsonFile()
        {
            string path = BuildFileRootPath("Sales.json");
            if (File.Exists(path)) File.Delete(path);
            List<Sale> sales = await SalesListFromFile();

            using (StreamWriter file = File.CreateText(path))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, sales);
            }
        }
    }
}