 ALTER TABLE t_country_prod add COUNTRY_ISOCODE  VARCHAR(10) COLLATE PXW_CYRL;
 COMMIT WORK;

 UPDATE t_country_prod SET COUNTRY_ISOCODE = '';
 COMMIT WORK;

 /*------   ------*/

SET TERM ~~~ ;
 create procedure SP_ADD_COUNTRY_PROD_FROMSQL (
    COUNTRY_ID       INTEGER,
    COUNTRY_NAME     VARCHAR(52),
    COUNTRY_CODE     VARCHAR(32),
    COUNTRY_ISOCODE  VARCHAR(10)
    )
returns (
    ERROR_NUMBER integer,
    ERROR_TEXT varchar(480))
as
declare variable EXISTS_COUNTRY_ID integer;
declare variable ALIEN_COUNTRY_ID integer;
BEGIN
 ERROR_NUMBER = -1;
 ERROR_TEXT = '';

 /*проверка на наличие записи с заданным кодом*/
 select COUNTRY_ID from t_country_prod where  COUNTRY_ID = :COUNTRY_ID
 into :EXISTS_COUNTRY_ID;
 if( ( :EXISTS_COUNTRY_ID is not null ) or ( :EXISTS_COUNTRY_ID <> 0 ) ) then
  begin
   ERROR_NUMBER = 1;
   ERROR_TEXT = cast( ( 'В БД присутствует Страна с заданным идентификатором: ' ||  cast( :COUNTRY_ID as varchar(8) ) ) as varchar(480));
   suspend;
   exit;
  end
 /*проверка на дублирование по символьному коду*/
 if( exists ( select COUNTRY_ID from t_country_prod where COUNTRY_CODE = :COUNTRY_CODE  ) ) then
  begin
   select max( COUNTRY_ID ) from t_country_prod where COUNTRY_CODE = :COUNTRY_CODE into :ALIEN_COUNTRY_ID;
   ERROR_NUMBER = 2;
   ERROR_TEXT = cast( ( 'В БД присутствует Страна с заданным кодом: ' || :COUNTRY_CODE || '. Идентификатор страны: ' || cast( :ALIEN_COUNTRY_ID as varchar(8) )) as varchar(480));
   suspend;
   exit;
  end
 /*проверка на дублирование по числовому коду*/
 if( exists ( select COUNTRY_ID from t_country_prod where COUNTRY_ISOCODE = :COUNTRY_ISOCODE  ) ) then
  begin
   select max( COUNTRY_ID ) from t_country_prod where COUNTRY_ISOCODE = :COUNTRY_ISOCODE into :ALIEN_COUNTRY_ID;
   ERROR_NUMBER = 2;
   ERROR_TEXT = cast( ( 'В БД присутствует Страна с заданным кодом: ' || :COUNTRY_ISOCODE || '. Идентификатор страны: ' || cast( :ALIEN_COUNTRY_ID as varchar(8) )) as varchar(480));
   suspend;
   exit;
  end

 INSERT INTO t_country_prod( COUNTRY_ID, COUNTRY_NAME, COUNTRY_CODE, COUNTRY_ISOCODE )
 VALUES( :COUNTRY_ID, :COUNTRY_NAME, :COUNTRY_CODE, :COUNTRY_ISOCODE );

 ERROR_NUMBER = 0;
 ERROR_TEXT = cast( ( 'Успешное завершение операции. Идентификатор страны: ' || cast( :COUNTRY_ID as varchar(8) ) ) as varchar(480));

 suspend;

 WHEN ANY DO
  BEGIN
   ERROR_NUMBER = -1;
   ERROR_TEXT = cast( ( :ERROR_TEXT || ' Не удалось добавить описание страны. Неизвестная ошибка, т.к не удается вернуть SQLCODE.' ) as varchar(480));

   suspend;
  END
END
 ~~~
SET TERM ; ~~~
commit work;


/*------   ------*/

SET TERM ~~~ ;
create procedure SP_EDIT_COUNTRY_PROD_FROMSQL (
    COUNTRY_ID       INTEGER,
    COUNTRY_NAME     VARCHAR(52),
    COUNTRY_CODE     VARCHAR(32),
    COUNTRY_ISOCODE  VARCHAR(10)
    )
returns (
    ERROR_NUMBER integer,
    ERROR_TEXT varchar(480))
as
declare variable EXISTS_COUNTRY_ID integer;
declare variable ALIEN_COUNTRY_ID integer;
BEGIN
 ERROR_NUMBER = -1;
 ERROR_TEXT = '';

 /*проверка на наличие записи с заданным кодом*/
 select COUNTRY_ID from t_country_prod where  COUNTRY_ID = :COUNTRY_ID
 into :EXISTS_COUNTRY_ID;
 if( ( :EXISTS_COUNTRY_ID is null ) or ( :EXISTS_COUNTRY_ID = 0 ) ) then
  begin
   ERROR_NUMBER = 1;
   ERROR_TEXT = cast( ( 'В БД не найдена Страна с заданным идентификатором: ' ||  cast( :COUNTRY_ID as varchar(8) ) ) as varchar(480));
   suspend;
   exit;
  end

 /*проверка на дублирование по символьному коду*/
 if( exists ( select COUNTRY_ID from t_country_prod where COUNTRY_CODE = :COUNTRY_CODE AND COUNTRY_ID <> :COUNTRY_ID ) ) then
  begin
   select max( COUNTRY_ID ) from t_country_prod where COUNTRY_CODE = :COUNTRY_CODE into :ALIEN_COUNTRY_ID;
   ERROR_NUMBER = 2;
   ERROR_TEXT = cast( ( 'В БД присутствует Страна с заданным кодом: ' || :COUNTRY_CODE || '. Идентификатор страны: ' || cast( :ALIEN_COUNTRY_ID as varchar(8) )) as varchar(480));
   suspend;
   exit;
  end
 /*проверка на дублирование по числовому коду*/
 if( exists ( select COUNTRY_ID from t_country_prod where COUNTRY_ISOCODE = :COUNTRY_ISOCODE AND COUNTRY_ID <> :COUNTRY_ID ) ) then
  begin
   select max( COUNTRY_ID ) from t_country_prod where COUNTRY_ISOCODE = :COUNTRY_ISOCODE into :ALIEN_COUNTRY_ID;
   ERROR_NUMBER = 2;
   ERROR_TEXT = cast( ( 'В БД присутствует Страна с заданным кодом: ' || :COUNTRY_ISOCODE || '. Идентификатор страны: ' || cast( :ALIEN_COUNTRY_ID as varchar(8) )) as varchar(480));
   suspend;
   exit;
  end

 UPDATE t_country_prod SET
   COUNTRY_NAME = :COUNTRY_NAME, COUNTRY_CODE = :COUNTRY_CODE, COUNTRY_ISOCODE = :COUNTRY_ISOCODE
 WHERE  COUNTRY_ID = :COUNTRY_ID;

 ERROR_NUMBER = 0;
 ERROR_TEXT = cast( ( 'Успешное завершение операции. Идентификатор страны: ' || cast( :COUNTRY_ID as varchar(8) ) ) as varchar(480));

 suspend;

 WHEN ANY DO
  BEGIN
   ERROR_NUMBER = -1;
   ERROR_TEXT = cast( ( :ERROR_TEXT || ' Не удалось изменить описание страны. Неизвестная ошибка, т.к не удается вернуть SQLCODE.' ) as varchar(480));

   suspend;
  END
END
 ~~~
SET TERM ; ~~~
commit work;


/*------   ------*/

SET TERM ~~~ ;
create procedure SP_DELETE_COUNTRY_PROD_FROMSQL (
    COUNTRY_ID integer)
returns (
    ERROR_NUMBER integer,
    ERROR_TEXT varchar(480))
as
 declare variable EXISTS_COUNTRY_ID integer;
BEGIN
 ERROR_NUMBER = -1;
 ERROR_TEXT = '';

 /*проверка на наличие записи с заданным кодом*/
 select COUNTRY_ID from t_country_prod where  COUNTRY_ID = :COUNTRY_ID
 into :EXISTS_COUNTRY_ID;
 if( ( :EXISTS_COUNTRY_ID is null ) or ( :EXISTS_COUNTRY_ID = 0 ) ) then
  begin
   ERROR_NUMBER = 1;
   ERROR_TEXT = cast( ( 'В БД не найдена Страна с заданным идентификатором: ' ||  cast( :COUNTRY_ID as varchar(8) ) ) as varchar(480));
   suspend;
   exit;
  end

  delete from t_country_prod  where COUNTRY_ID = :COUNTRY_ID;

 ERROR_NUMBER = 0;
 ERROR_TEXT = cast( ( 'Успешное завершение операции. Идентификатор страны: ' || cast( :COUNTRY_ID as varchar(8) ) ) as varchar(480));

 suspend;


 WHEN ANY DO
  BEGIN
   ERROR_NUMBER = -1;
   ERROR_TEXT = 'Не удалось удалить описание страны. Неизвестная ошибка, т.к не удается вернуть SQLCODE.';

   suspend;
  END

END
 ~~~
SET TERM ; ~~~
commit work;


