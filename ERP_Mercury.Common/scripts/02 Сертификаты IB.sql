
 ALTER TABLE T_PARTSCERTIFICATE ADD CERTIFICATE_NUMFORWAYBILL D_VARCHAR_256;
 ALTER TABLE T_PARTSCERTIFICATE ADD CERTIFICATE_ISACTIVE D_ALLOK;
 ALTER TABLE T_PARTSCERTIFICATE ADD CERTIFICATE_DESCRIPTION D_VARCHAR_256;
 COMMIT WORK;

 UPDATE  T_PARTSCERTIFICATE SET CERTIFICATE_ISACTIVE = 1, CERTIFICATE_NUMFORWAYBILL = CERTIFICATE_NUM;
 COMMIT WORK;

 SET TERM ^ ;

ALTER procedure SP_EDIT_CERTIFICATELIST_FOR_PARTS (
    CERTIFICATETYPE_ID integer,
    PARTS_ID integer,
    CERTIFICATE_NUM varchar(128),
    CERTIFICATE_WHOGIVE varchar(128),
    CERTIFICATE_BEGINDATE date,
    CERTIFICATE_ENDDATE date,
    CERTIFICATE_NUMFORWAYBILL varchar(256),
    CERTIFICATE_DESCRIPTION  varchar(256),
    CERTIFICATE_ISACTIVE int
    )
returns (
    ERROR_NUMBER integer,
    ERROR_TEXT varchar(480))
as
declare variable EXISTS_PARTS_ID integer;
declare variable EXISTS_CERTIFICATETYPE_ID integer;
declare variable PARTSCERTIFICATE_ID integer;
begin


 ERROR_NUMBER = -1;
 ERROR_TEXT = '';

 /*проверка на наличие записи с заданным кодом*/
 select parts_id from t_parts where  parts_id = :parts_id
 into :exists_parts_id;
 if( ( :exists_parts_id is null ) or ( :exists_parts_id = 0 ) ) then
  begin
   ERROR_NUMBER = 1;
   ERROR_TEXT = cast( ( 'В БД не найден товар с заданным кодом: ' ||  cast( :parts_id as varchar(8) ) ) as varchar(480));
   suspend;
   exit;
  end

 select certificatetype_id from t_certificatetype where  certificatetype_id = :certificatetype_id
 into :exists_certificatetype_id;
 if( ( :exists_certificatetype_id is null ) or ( :exists_certificatetype_id = 0 ) ) then
  begin
   ERROR_NUMBER = 1;
   ERROR_TEXT = cast( ( 'В БД не найден тип документа о качестве товара с заданным кодом: ' ||  cast( :CERTIFICATETYPE_ID as varchar(8) ) ) as varchar(480));
   suspend;
   exit;
  end

  PARTSCERTIFICATE_ID = NULL;

  select max( partscertificate_id ) from t_partscertificate
    where parts_id = :parts_id
      and CERTIFICATETYPE_ID = :CERTIFICATETYPE_ID
      and CERTIFICATE_NUM = :CERTIFICATE_NUM
      and CERTIFICATE_WHOGIVE = :CERTIFICATE_WHOGIVE
      and CERTIFICATE_NUMFORWAYBILL = :CERTIFICATE_NUMFORWAYBILL
  into :PARTSCERTIFICATE_ID;

  if( ( :PARTSCERTIFICATE_ID is null ) OR ( :PARTSCERTIFICATE_ID = 0 ) ) then
   begin
    insert into t_partscertificate( CERTIFICATETYPE_ID, PARTS_ID, CERTIFICATE_NUM, CERTIFICATE_WHOGIVE,
     CERTIFICATE_BEGINDATE, CERTIFICATE_ENDDATE, CERTIFICATE_NUMFORWAYBILL, CERTIFICATE_ISACTIVE, CERTIFICATE_DESCRIPTION )
    values( :CERTIFICATETYPE_ID, :PARTS_ID, :CERTIFICATE_NUM, :CERTIFICATE_WHOGIVE,
     :CERTIFICATE_BEGINDATE, :CERTIFICATE_ENDDATE, :CERTIFICATE_NUMFORWAYBILL, :CERTIFICATE_ISACTIVE, :CERTIFICATE_DESCRIPTION );
   end
  else
   begin
    update t_partscertificate set CERTIFICATE_NUM = :CERTIFICATE_NUM, CERTIFICATE_WHOGIVE = :CERTIFICATE_WHOGIVE,
     CERTIFICATE_BEGINDATE = :CERTIFICATE_BEGINDATE, CERTIFICATE_ENDDATE = :CERTIFICATE_ENDDATE,
     CERTIFICATE_NUMFORWAYBILL = :CERTIFICATE_NUMFORWAYBILL, CERTIFICATE_ISACTIVE = :CERTIFICATE_ISACTIVE,
     CERTIFICATE_DESCRIPTION = :CERTIFICATE_DESCRIPTION
    where PARTSCERTIFICATE_ID = :PARTSCERTIFICATE_ID;
   end

 ERROR_NUMBER = 0;
 ERROR_TEXT = cast( 'Успешное завершение операции.' as varchar(480));

 suspend;

 WHEN ANY DO
  BEGIN
   ERROR_NUMBER = -1;
   ERROR_TEXT = ERROR_TEXT || 'Не удалось изменить информацию о качестве товара. Неизвестная ошибка, т.к не удается вернуть SQLCODE.';

   suspend;
  END

end^

SET TERM ; ^


