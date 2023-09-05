CREATE OR REPLACE PROCEDURE GET_SALE
(
  P_AccountID                 sales.account_id%type,
  O_Result                    OUT SYS_REFCURSOR
)
IS
BEGIN
    OPEN O_Result FOR SELECT * FROM sales s WHERE s.account_id = P_AccountID;
END GET_SALE;
