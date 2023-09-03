CREATE OR REPLACE PROCEDURE SAVE_SALE
(
  P_Id                        NUMBER,
  P_AccountID                 VARCHAR2,
  P_TransactionID             VARCHAR2,
  P_TransactionAmount         NUMBER,
  P_TransactionCurrencyCode   VARCHAR2,
  P_LocalHour                 NUMBER,
  P_TransactionScenario       CHAR,
  P_TransactionType           CHAR,
  P_TransactionIPaddress      NUMBER,
  P_IpState                   VARCHAR2,
  P_IpPostalCode              VARCHAR2,
  P_IpCountry                 VARCHAR2,
  P_IsProxyIP                 NUMBER,
  P_BrowserLanguage           VARCHAR2,
  P_PaymentInstrumentType     VARCHAR2,
  P_CardType                  VARCHAR2,
  P_PaymentBillingPostalCode  VARCHAR2,
  P_PaymentBillingState       VARCHAR2,
  P_PaymentBillingCountryCode VARCHAR2,
  P_ShippingPostalCode        VARCHAR2,
  P_ShippingState             VARCHAR2,
  P_ShippingCountry           VARCHAR2,
  P_CvvVerifyResult           VARCHAR2,
  P_DigitalItemCount          VARCHAR2,
  P_PhysicalItemCount         VARCHAR2,
  P_TransactionDateTime       VARCHAR2,
  O_Result                    OUT VARCHAR2
) 
IS
BEGIN
  INSERT INTO SALES
  (
    ID,
    ACCOUNT_ID,                	  TRANSACTION_ID,           TRANSACTION_AMOUNT_USD,
    TRANSACTION_CURRENCY_CODE,    LOCAL_HOUR,               TRANSACTION_SCENARIO,
    TRANSACTION_TYPE,             TRANSACTION_IP_ADDRESS,   IP_STATE,
    IP_POSTAL_CODE,               IP_COUNTRY,               IS_PROXY_IP,
    BROWSER_LANGUAGE,             PAYMENT_INSTRUMENT_TYPE,  CARD_TYPE,
    PAYMENT_BILLING_POSTAL_CODE,  PAYMENT_BILLING_STATE,    PAYMENT_BILLING_COUNTRY_CODE,
    SHIPPING_POSTAL_CODE,         SHIPPING_STATE,           SHIPPING_COUNTRY,
    CVV_VERIFY_RESULT,            DIGITAL_ITEM_COUNT,       PHYSICAL_ITEM_COUNT,
    TRANSACTION_DATE_TIME
  )
  VALUES
  (
    P_Id, 
    P_AccountID,            	  P_TransactionID,          P_TransactionAmount,
    P_TransactionCurrencyCode,    P_LocalHour,              P_TransactionScenario, 
    P_TransactionType,            P_TransactionIPaddress,   P_IpState,
    P_IpPostalCode,               P_IpCountry,              P_IsProxyIP,
    P_BrowserLanguage,            P_PaymentInstrumentType,  P_CardType,
    P_PaymentBillingPostalCode,   P_PaymentBillingState,    P_PaymentBillingCountryCode,
    P_ShippingPostalCode,         P_ShippingState,          P_ShippingCountry,
    P_CvvVerifyResult,            P_DigitalItemCount,       P_PhysicalItemCount,
    P_TransactionDateTime
  );
  COMMIT;
  O_Result := 'OK';
EXCEPTION
  WHEN OTHERS THEN
    O_Result := to_char(SQLERRM||' - '||SQLERRM); 
END SAVE_SALE;
/
