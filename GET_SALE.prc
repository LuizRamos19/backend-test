CREATE OR REPLACE PROCEDURE GET_SALE
(
  P_AccountID                 VARCHAR2,
  O_Result                    OUT SYS_REFCURSOR
)
IS
BEGIN
    OPEN O_Result FOR SELECT * FROM sales s WHERE s.Account_Id = P_AccountID;
END GET_SALE;
/
