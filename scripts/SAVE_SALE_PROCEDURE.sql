CREATE OR REPLACE PROCEDURE SAVE_SALE
(
  P_AccountID                 sales.account_id%TYPE,
  P_TransactionID             sales.transaction_id%TYPE,
  P_TransactionAmountUsd      sales.transaction_amount_usd%type,
  P_TransactionCurrencyCode   sales.transaction_currency_code%type,
  P_LocalHour                 sales.local_hour%type,
  P_TransactionScenario       sales.transaction_scenario%type,
  P_TransactionType           sales.transaction_type%type,
  P_TransactionIPaddress      sales.transaction_ip_address%type,
  P_IpState                   sales.ip_state%type,
  P_IpPostalCode              sales.ip_postal_code%type,
  P_IpCountry                 sales.ip_country%type,
  P_IsProxyIP                 sales.is_proxy_ip%type,
  P_BrowserLanguage           sales.browser_language%type,
  P_PaymentInstrumentType     sales.payment_instrument_type%type,
  P_CardType                  sales.card_type%type,
  P_PaymentBillingPostalCode  sales.payment_billing_postal_code%type,
  P_PaymentBillingState       sales.payment_billing_state%type,
  P_PaymentBillingCountryCode sales.payment_billing_country_code%type,
  P_ShippingPostalCode        sales.shipping_postal_code%type,
  P_ShippingState             sales.shipping_state%type,
  P_ShippingCountry           sales.shipping_country%type,
  P_CvvVerifyResult           sales.cvv_verify_result%type,
  P_DigitalItemCount          sales.digital_item_count%type,
  P_PhysicalItemCount         sales.physical_item_count%type,
  P_TransactionDateTime       sales.transaction_date_time%type,
  O_Result                    OUT VARCHAR2
) 
IS 
  CURSOR cr_NextVal IS
    SELECT sales_seq.nextval next_id from dual;
  c_NexVal cr_NextVal%ROWTYPE;
BEGIN
  OPEN cr_NextVal;
  FETCH cr_NextVal INTO c_NexVal;
  CLOSE cr_NextVal;
  
  INSERT INTO SALES
  (
    ID,
    ACCOUNT_ID,                   TRANSACTION_ID,           TRANSACTION_AMOUNT_USD,
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
    c_NexVal.Next_Id, 
    P_AccountID,                  P_TransactionID,          P_TransactionAmountUsd,
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
  O_Result := 'OK'||'|'||c_NexVal.Next_Id;
EXCEPTION
  WHEN OTHERS THEN
    O_Result := SQLERRM; 
END SAVE_SALE;
