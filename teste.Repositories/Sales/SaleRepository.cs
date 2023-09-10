using System.Collections.Generic;
using teste.ApiCore31.Infrastructure.DataBase;
using teste.Models;
using Snapper.Core.DataBase;
using System.Data;
using System.Text;
using System;
using Oracle.ManagedDataAccess.Client;
using System.Threading.Tasks;

namespace teste.Repositories.Sales
{
    public class SaleRepository : ISaleRepository
    {
        private readonly ISnapperDataBase _snapperDataBase = new SnapperDataBase();

        /// <summary>
        /// Retrieve record from database table.
        /// </summary>
        /// <param name="id">Id it the target field to retrieve single record</param>
        /// <returns>Sales entity</returns>
        public async Task<Sale> GetSale(int id)
        {
            var oracle = _snapperDataBase.CreateOracleConnection();
            oracle.CreateCommand(
                "SELECT * FROM sales WHERE Id = (p)Id",
                new List<Parameter>
                {
                    new Parameter
                    {
                        Name = "Id",
                        Value = id,
                        ParameterDirection = ParameterDirection.Input,
                        ParameterDbType = ParameterDbType.Numeric
                    }
                },
                CommandType.Text
            );
            var tb = await oracle.Command.ExecuteTableAsync();
            return tb.Rows.Count > 0 ? new Sale(tb, 0) : null;
        }

        /// <summary>
        /// Count record from database table to check if exists
        /// </summary>
        /// <param name="id">Id it the target field to retrieve single recordt</param>
        /// <returns>True or False for Exists or not exists</returns>
        public async Task<bool> SaleExists(int id)
        {
            var oracle = _snapperDataBase.CreateOracleConnection();
            oracle.CreateCommand(
                "SELECT count(id) existe FROM sales WHERE Id = (p)Id",
                new List<Parameter>
                {
                    new Parameter
                    {
                        Name = "Id",
                        Value = id,
                        ParameterDirection = ParameterDirection.Input,
                        ParameterDbType = ParameterDbType.Numeric
                    }
                },
                CommandType.Text
            );
            var tb = await oracle.Command.ExecuteTableAsync();
            return Convert.ToInt32(tb.Rows[0]["existe"]) > 0;
        }

        public async Task<Sale> SaveSale(Sale sale)
        {
            var oracle = _snapperDataBase.CreateOracleConnection();

            #region-- Usar procedure mas DBever não deixa. PL-SQL Developer 100% mas no DB fica inválida --
            //oracle.CreateCommand(
            //    "SAVE_SALE",
            //    new List<Parameter>
            //    {
            //        new Parameter
            //        {
            //            Name = "P_AccountID",
            //            Value = sale.AccountID,
            //            ParameterDirection = ParameterDirection.Input,
            //            ParameterDbType = ParameterDbType.Varchar2
            //        },
            //        new Parameter
            //        {
            //            Name = "P_TransactionID",
            //            Value = sale.TransactionID,
            //            ParameterDirection = ParameterDirection.Input,
            //            ParameterDbType = ParameterDbType.Varchar2
            //        },
            //        new Parameter
            //        {
            //            Name = "P_TransactionAmountUsd",
            //            Value = sale.TransactionAmount,
            //            ParameterDirection = ParameterDirection.Input,
            //            ParameterDbType = ParameterDbType.Numeric
            //        },
            //        new Parameter
            //        {
            //            Name = "P_TransactionCurrencyCode",
            //            Value = sale.TransactionCurrencyCode,
            //            ParameterDirection = ParameterDirection.Input,
            //            ParameterDbType = ParameterDbType.Varchar2
            //        },
            //        new Parameter
            //        {
            //            Name = "P_LocalHour",
            //            Value = sale.LocalHour,
            //            ParameterDirection = ParameterDirection.Input,
            //            ParameterDbType = ParameterDbType.Numeric
            //        },
            //        new Parameter
            //        {
            //            Name = "P_TransactionScenario",
            //            Value = sale.TransactionScenario,
            //            ParameterDirection = ParameterDirection.Input,
            //            ParameterDbType = ParameterDbType.Char
            //        },
            //        new Parameter
            //        {
            //            Name = "P_TransactionType",
            //            Value = sale.TransactionType,
            //            ParameterDirection = ParameterDirection.Input,
            //            ParameterDbType = ParameterDbType.Char
            //        },
            //        new Parameter
            //        {
            //            Name = "P_TransactionIPaddress",
            //            Value = sale.TransactionIPaddress,
            //            ParameterDirection = ParameterDirection.Input,
            //            ParameterDbType = ParameterDbType.Numeric
            //        },
            //        new Parameter
            //        {
            //            Name = "P_IpState",
            //            Value = sale.IpState,
            //            ParameterDirection = ParameterDirection.Input,
            //            ParameterDbType = ParameterDbType.Varchar2
            //        },
            //        new Parameter
            //        {
            //            Name = "P_IpPostalCode",
            //            Value = sale.IpPostalCode,
            //            ParameterDirection = ParameterDirection.Input,
            //            ParameterDbType = ParameterDbType.Varchar2
            //        },
            //        new Parameter
            //        {
            //            Name = "P_IpCountry",
            //            Value = sale.IpCountry,
            //            ParameterDirection = ParameterDirection.Input,
            //            ParameterDbType = ParameterDbType.Varchar2
            //        },
            //        new Parameter
            //        {
            //            Name = "P_IsProxyIP",
            //            Value = sale.IsProxyIP,
            //            ParameterDirection = ParameterDirection.Input,
            //            ParameterDbType = ParameterDbType.Numeric
            //        },
            //        new Parameter
            //        {
            //            Name = "P_BrowserLanguage",
            //            Value = sale.BrowserLanguage,
            //            ParameterDirection = ParameterDirection.Input,
            //            ParameterDbType = ParameterDbType.Varchar2
            //        },
            //        new Parameter
            //        {
            //            Name = "P_PaymentInstrumentType",
            //            Value = sale.PaymentInstrumentType,
            //            ParameterDirection = ParameterDirection.Input,
            //            ParameterDbType = ParameterDbType.Varchar2
            //        },
            //        new Parameter
            //        {
            //            Name = "P_CardType",
            //            Value = sale.CardType,
            //            ParameterDirection = ParameterDirection.Input,
            //            ParameterDbType = ParameterDbType.Varchar2
            //        },
            //        new Parameter
            //        {
            //            Name = "P_PaymentBillingPostalCode",
            //            Value = sale.PaymentBillingPostalCode,
            //            ParameterDirection = ParameterDirection.Input,
            //            ParameterDbType = ParameterDbType.Varchar2
            //        },
            //        new Parameter
            //        {
            //            Name = "P_PaymentBillingState",
            //            Value = sale.PaymentBillingState,
            //            ParameterDirection = ParameterDirection.Input,
            //            ParameterDbType = ParameterDbType.Varchar2
            //        },
            //        new Parameter
            //        {
            //            Name = "P_PaymentBillingCountryCode",
            //            Value = sale.PaymentBillingCountryCode,
            //            ParameterDirection = ParameterDirection.Input,
            //            ParameterDbType = ParameterDbType.Varchar2
            //        },
            //        new Parameter
            //        {
            //            Name = "P_ShippingPostalCode",
            //            Value = sale.ShippingPostalCode,
            //            ParameterDirection = ParameterDirection.Input,
            //            ParameterDbType = ParameterDbType.Varchar2
            //        },
            //        new Parameter
            //        {
            //            Name = "P_ShippingState",
            //            Value = sale.ShippingState,
            //            ParameterDirection = ParameterDirection.Input,
            //            ParameterDbType = ParameterDbType.Varchar2
            //        },
            //        new Parameter
            //        {
            //            Name = "P_ShippingCountry",
            //            Value = sale.ShippingCountry,
            //            ParameterDirection = ParameterDirection.Input,
            //            ParameterDbType = ParameterDbType.Varchar2
            //        },
            //        new Parameter
            //        {
            //            Name = "P_CvvVerifyResult",
            //            Value = sale.CvvVerifyResult,
            //            ParameterDirection = ParameterDirection.Input,
            //            ParameterDbType = ParameterDbType.Varchar2
            //        },
            //        new Parameter
            //        {
            //            Name = "P_DigitalItemCount",
            //            Value = sale.DigitalItemCount,
            //            ParameterDirection = ParameterDirection.Input,
            //            ParameterDbType = ParameterDbType.Numeric
            //        },
            //        new Parameter
            //        {
            //            Name = "P_PhysicalItemCount",
            //            Value = sale.PhysicalItemCount,
            //            ParameterDirection = ParameterDirection.Input,
            //            ParameterDbType = ParameterDbType.Numeric
            //        },
            //        new Parameter
            //        {
            //            Name = "P_TransactionDateTime",
            //            Value = sale.TransactionDateTime,
            //            ParameterDirection = ParameterDirection.Input,
            //            ParameterDbType = ParameterDbType.Varchar2
            //        },
            //        new Parameter
            //        {
            //            Name = "O_Result",
            //            Value = null,
            //            ParameterDirection = ParameterDirection.Output,
            //            ParameterDbType = ParameterDbType.Varchar2
            //        }
            //    },
            //    CommandType.StoredProcedure
            //);
            //var term = oracle.Command.ExecuteReturn();
            //var xpto = term;
            #endregion

            //Ai vai do jeito que da mesmo..
            //Pegar sequence que na proc faz internamente.
            oracle.CreateCommand("SELECT sales_seq.nextval id FROM dual", null, CommandType.Text);
            var tb = await oracle.Command.ExecuteTableAsync();
            sale.Id = Convert.ToInt64(tb.Rows[0]["id"]);

            //Insert na tabela Sales
            var sql = new StringBuilder();
            sql.AppendLine("INSERT INTO SALES																			             ");
            sql.AppendLine("  (																							             ");
            sql.AppendLine("    ID,								 														             ");
            sql.AppendLine("    ACCOUNT_ID,                       TRANSACTION_ID,                  TRANSACTION_AMOUNT_USD,		     ");
            sql.AppendLine("    TRANSACTION_CURRENCY_CODE,        LOCAL_HOUR,                      TRANSACTION_SCENARIO,		     ");
            sql.AppendLine("    TRANSACTION_TYPE,                 TRANSACTION_IP_ADDRESS,          IP_STATE,					     ");
            sql.AppendLine("    IP_POSTAL_CODE,                   IP_COUNTRY,                      IS_PROXY_IP,				         ");
            sql.AppendLine("    BROWSER_LANGUAGE,                 PAYMENT_INSTRUMENT_TYPE,         CARD_TYPE,					     ");
            sql.AppendLine("    PAYMENT_BILLING_POSTAL_CODE,      PAYMENT_BILLING_STATE,           PAYMENT_BILLING_COUNTRY_CODE,     ");
            sql.AppendLine("    SHIPPING_POSTAL_CODE,             SHIPPING_STATE,                  SHIPPING_COUNTRY,				 ");
            sql.AppendLine("    CVV_VERIFY_RESULT,                DIGITAL_ITEM_COUNT,              PHYSICAL_ITEM_COUNT,			     ");
            sql.AppendLine("    TRANSACTION_DATE_TIME																	             ");
            sql.AppendLine("  )																							             ");
            sql.AppendLine("  VALUES																					             ");
            sql.AppendLine("  (																							             ");
            sql.AppendLine($"   {sale.Id}, 													                                         ");
            sql.AppendLine($"  '{sale.AccountID}',               '{sale.TransactionID}',           {sale.TransactionAmount},		 ");
            sql.AppendLine($"  '{sale.TransactionCurrencyCode}',  {sale.LocalHour},               '{sale.TransactionScenario}',      ");
            sql.AppendLine($"  '{sale.TransactionType}',          {sale.TransactionIPaddress},    '{sale.IpState}',				     ");
            sql.AppendLine($"  '{sale.IpPostalCode}',            '{sale.IpCountry}',               {sale.IsProxyIP},			     ");
            sql.AppendLine($"  '{sale.BrowserLanguage}',         '{sale.PaymentInstrumentType}',  '{sale.CardType}',			     ");
            sql.AppendLine($"  '{sale.PaymentBillingPostalCode}','{sale.PaymentBillingState}',    '{sale.PaymentBillingCountryCode}',");
            sql.AppendLine($"  '{sale.ShippingPostalCode}',      '{sale.ShippingState}',          '{sale.ShippingCountry}',	         ");
            sql.AppendLine($"  '{sale.CvvVerifyResult}',         '{sale.DigitalItemCount}',       '{sale.PhysicalItemCount}',	     ");
            sql.AppendLine($"  to_date('{sale.TransactionDateTime}','dd-mm-rr HH24:mi:ss')									         ");
            sql.AppendLine("  )																						                 ");

            oracle.CreateCommand(sql.ToString(), null, CommandType.Text);
            await oracle.Command.ExecuteAsync();
            return sale;
        }
    }
}
