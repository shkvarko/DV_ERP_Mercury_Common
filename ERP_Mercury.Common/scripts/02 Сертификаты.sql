USE [ERP_Mercury]
GO

ALTER TABLE [dbo].[T_PartsCertificate] ADD [Certificate_NumForWaybill] D_NAMELONG NULL
ALTER TABLE [dbo].[T_PartsCertificate] ADD [Certificate_IsActive] D_YESNO NULL
ALTER TABLE [dbo].[T_PartsCertificate] ADD [Certificate_Description] D_DESCRIPTION NULL
ALTER TABLE [dbo].[T_PartsCertificate] ADD [Certificate_Image] D_IMAGE NULL
ALTER TABLE [dbo].[T_PartsCertificate] ADD [Certificate_ImageFileFullName] D_NAMELONG NULL
GO

UPDATE [dbo].[T_PartsCertificate] SET Certificate_IsActive = 1
GO

/****** Object:  View [dbo].[PartsCertificateView]    Script Date: 17.05.2013 10:51:24 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


ALTER VIEW [dbo].[PartsCertificateView]
AS

SELECT dbo.T_PartsCertificate.Certificate_Guid, dbo.T_PartsCertificate.CertificateType_Guid, dbo.T_PartsCertificate.Parts_Guid, dbo.T_PartsCertificate.Certificate_Num, 
  dbo.T_PartsCertificate.Certificate_WhoGive, dbo.T_PartsCertificate.Certificate_BeginDate, dbo.T_PartsCertificate.Certificate_EndDate, 
	dbo.T_PartsCertificate.Certificate_NumForWaybill, dbo.T_PartsCertificate.Certificate_IsActive, dbo.T_PartsCertificate.Certificate_Description,
  dbo.T_CertificateType.CertificateType_Name, dbo.T_CertificateType.CertificateType_Description, dbo.T_CertificateType.CertificateType_IsActive
FROM  dbo.T_PartsCertificate INNER JOIN
 dbo.T_CertificateType ON dbo.T_PartsCertificate.CertificateType_Guid = dbo.T_CertificateType.CertificateType_Guid


GO


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Возвращает список записей из ( PartsCertificateView )
--
-- Входящие параметры:
--	@Parts_Guid	- уникальный идентификатор товара
--
-- Выходные параметры:
--
-- Результат:
--    0 - успешное завершение
--    <>0 - ошибка запроса информации из базы данных

ALTER PROCEDURE [dbo].[usp_GetPartsCertificate] 
	@Parts_Guid dbo.D_GUID,
	
  @ERROR_NUM int output,
  @ERROR_MES nvarchar(4000) output
AS

BEGIN

  SET @ERROR_NUM = 0;
  SET @ERROR_MES = '';

  BEGIN TRY

		SELECT dbo.T_PartsCertificate.Certificate_Guid, dbo.T_PartsCertificate.CertificateType_Guid, dbo.T_PartsCertificate.Parts_Guid, 
			dbo.T_PartsCertificate.Certificate_Num, dbo.T_PartsCertificate.Certificate_WhoGive, dbo.T_PartsCertificate.Certificate_BeginDate, 
			dbo.T_PartsCertificate.Certificate_EndDate, dbo.T_PartsCertificate.Certificate_NumForWaybill, dbo.T_PartsCertificate.Certificate_IsActive, 
			dbo.T_PartsCertificate.Certificate_Description,	dbo.T_CertificateType.CertificateType_Name, dbo.T_CertificateType.CertificateType_Description, 
			dbo.T_CertificateType.CertificateType_IsActive, dbo.T_PartsCertificate.Certificate_ImageFileFullName,
			ExistImage = 
      CASE 
         WHEN Certificate_Image IS NULL THEN 0
         ELSE 1
      END
		FROM  dbo.T_PartsCertificate INNER JOIN
		 dbo.T_CertificateType ON dbo.T_PartsCertificate.CertificateType_Guid = dbo.T_CertificateType.CertificateType_Guid
		WHERE dbo.T_PartsCertificate.Parts_Guid = @Parts_Guid
		ORDER BY dbo.T_CertificateType.CertificateType_Name, dbo.T_PartsCertificate.Certificate_Num;

	END TRY
	BEGIN CATCH
		SET @ERROR_NUM = ERROR_NUMBER();
		SET @ERROR_MES = ERROR_MESSAGE();

		RETURN @ERROR_NUM;
	END CATCH;

  IF( @ERROR_NUM = 0 )
		SET @ERROR_MES = 'Успешное завершение операции.';

  RETURN @ERROR_NUM;
END
GO


ALTER TABLE [dbo].[T_PartsCertificate_Archive] ADD [Certificate_NumForWaybill] D_NAMELONG NULL
ALTER TABLE [dbo].[T_PartsCertificate_Archive] ADD [Certificate_IsActive] D_YESNO NULL
ALTER TABLE [dbo].[T_PartsCertificate_Archive] ADD [Certificate_Description] D_DESCRIPTION NULL
ALTER TABLE [dbo].[T_PartsCertificate_Archive] ADD [Certificate_ImageFileFullName] D_NAMELONG NULL
GO

/****** Object:  Trigger [dbo].[TG_PartsCertificateAfterDelete]    Script Date: 17.05.2013 11:27:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Триггер добавляет записи в случае их удаления в таблицу удаленных записей
-- =============================================
ALTER TRIGGER [dbo].[TG_PartsCertificateAfterDelete]
   ON [dbo].[T_PartsCertificate] 
   AFTER DELETE
AS 
BEGIN
	SET NOCOUNT ON;

	INSERT INTO dbo.T_PartsCertificate_Archive( Certificate_Guid, CertificateType_Guid, Parts_Guid, Certificate_Num, Certificate_WhoGive, 
		Certificate_BeginDate, Certificate_EndDate, Certificate_NumForWaybill, Certificate_IsActive, Certificate_Description, 
		Record_Updated, Record_UserUdpated, Action_TypeId )
	SELECT Certificate_Guid, CertificateType_Guid, Parts_Guid, Certificate_Num, Certificate_WhoGive, 
		Certificate_BeginDate, Certificate_EndDate, Certificate_NumForWaybill, Certificate_IsActive, Certificate_Description,
		sysutcdatetime(), ( Host_Name() + ': ' + SUSER_SNAME() ), 2
	FROM deleted;
END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Триггер обновляет время редактирования/вставки записи
-- =============================================
ALTER TRIGGER [dbo].[TG_PartsCertificateAfterUpdate]
   ON  [dbo].[T_PartsCertificate]
   AFTER INSERT, UPDATE
AS 
BEGIN
	SET NOCOUNT ON;
	
	INSERT INTO dbo.T_PartsCertificate_Archive( Certificate_Guid, CertificateType_Guid, Parts_Guid, Certificate_Num, Certificate_WhoGive, 
		Certificate_BeginDate, Certificate_EndDate, Certificate_NumForWaybill, Certificate_IsActive, Certificate_Description, 
		Record_Updated, Record_UserUdpated, Action_TypeId )
	SELECT Certificate_Guid, CertificateType_Guid, Parts_Guid, Certificate_Num, Certificate_WhoGive, 
		Certificate_BeginDate, Certificate_EndDate, Certificate_NumForWaybill, Certificate_IsActive, Certificate_Description,
		sysutcdatetime(), ( Host_Name() + ': ' + SUSER_SNAME() ), 0
	FROM inserted;

	UPDATE dbo.[T_PartsCertificate] SET Record_Updated = sysutcdatetime(), Record_UserUdpated = ( Host_Name() + ': ' + SUSER_SNAME() )
	WHERE Certificate_Guid IN ( SELECT Certificate_Guid FROM inserted );
END

GO


DROP PROCEDURE [dbo].[usp_AssignCertificateWithPart] 
GO

DROP TYPE [dbo].[udt_Certificate]
GO

/****** Object:  UserDefinedTableType [dbo].[udt_Certificate]    Script Date: 17.05.2013 11:54:07 ******/
CREATE TYPE [dbo].[udt_Certificate] AS TABLE(
	[Certificate_Guid] [dbo].[D_GUID] NULL,
	[CertificateType_Guid] [dbo].[D_GUID] NULL,
	[Certificate_Num] [dbo].[D_NAMELONG] NULL,
	[Certificate_WhoGive] [dbo].[D_NAMELONG] NULL,
	[Certificate_BeginDate] [dbo].[D_DATE] NULL,
	[Certificate_EndDate] [dbo].[D_DATE] NULL,
	[Certificate_NumForWaybill] [dbo].[D_NAMELONG] NULL,
	[Certificate_IsActive] [dbo].[D_YESNO] NULL,
	[Certificate_Description] [dbo].[D_DESCRIPTION] NULL,
	[Certificate_Image] [dbo].[D_IMAGE] NULL,
	[Certificate_ImageFileFullName] [dbo].[D_NAMELONG] NULL,
	[ActionType_Id] [dbo].[D_ID] NULL
)
GO

GRANT CONTROL ON TYPE::[dbo].[udt_Certificate] TO [public]
GO
use [ERP_Mercury]
GO
GRANT REFERENCES ON TYPE::[dbo].[udt_Certificate] TO [public]
GO
use [ERP_Mercury]
GO
GRANT TAKE OWNERSHIP ON TYPE::[dbo].[udt_Certificate] TO [public]
GO
use [ERP_Mercury]
GO
GRANT VIEW DEFINITION ON TYPE::[dbo].[udt_Certificate] TO [public]
GO

/****** Object:  StoredProcedure [dbo].[usp_AssignCertificateWithPart]    Script Date: 17.05.2013 11:31:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Устанавливает привязку сертификатов к товару
--
-- Параметры:
-- [in] @Parts_Guid	- уникальный идентификатор товара
-- [in] @tCertificate	- таблица с сертификатами
--
-- [out]  @ERROR_NUM - номер ошибки
-- [out]  @ERROR_MES - текст ошибки
--
-- Результат:
--    0 - успешное завершение
--
-- Возвращает:
--

CREATE PROCEDURE [dbo].[usp_AssignCertificateWithPart] 
	@Parts_Guid dbo.D_GUID,
	@tCertificate dbo.udt_Certificate READONLY,
	@IBLINKEDSERVERNAME dbo.D_NAME = NULL,

  @ERROR_NUM int output,
  @ERROR_MES nvarchar(4000) output
AS

BEGIN

  SET @ERROR_NUM = 0;
  SET @ERROR_MES = '';

  BEGIN TRY

    IF( @IBLINKEDSERVERNAME IS NULL ) SELECT @IBLINKEDSERVERNAME = dbo.GetIBLinkedServerName();
    DECLARE @strIBSQLText nvarchar( 250 );
    DECLARE @SQLString nvarchar( 2048 );
    DECLARE @ParmDefinition nvarchar( 500 );
    SET @ParmDefinition = N'@ErrorNum_Ib int output, @ErrorText_Ib varchar(480) output'; 

    DECLARE @PARTS_ID D_ID;
    DECLARE @CERTIFICATETYPE_ID D_ID;
    DECLARE @CERTIFICATETYPE_GUID D_GUID;
    DECLARE @CERTIFICATE_NUM nvarchar(128);
    DECLARE @CERTIFICATE_WHOGIVE nvarchar(128);
    DECLARE @strCERTIFICATE_BEGINDATE nvarchar(10);
    DECLARE @strCERTIFICATE_ENDDATE nvarchar(10);
    DECLARE @CERTIFICATE_BEGINDATE D_DATE;
    DECLARE @CERTIFICATE_ENDDATE D_DATE;
    
		DECLARE @Certificate_Guid D_GUID;
		DECLARE @CERTIFICATE_NUMFORWAYBILL	D_NAMELONG;
    DECLARE @CERTIFICATE_ISACTIVE				D_YESNO;
    DECLARE @CERTIFICATE_DESCRIPTION		D_DESCRIPTION;
		DECLARE @CERTIFICATE_IMAGEFILEFULLNAME	D_NAMELONG;
		DECLARE @ActionType_Id							D_ID;

    SELECT @PARTS_ID = parts_id FROm T_Parts WHERE Parts_Guid = @Parts_Guid;

		-- В Interbase удаляется информация о сертификатах для товара с кодом @PARTS_ID
    SET @SQLString = 'SELECT @ErrorNum_Ib = ERROR_NUMBER, @ErrorText_Ib = ERROR_TEXT FROM OPENQUERY( ' + 
			@IBLINKEDSERVERNAME + ', ''SELECT ERROR_NUMBER, ERROR_TEXT FROM SP_DELETE_CERTIFICATE_FOR_PARTS( ' + 
									 cast( @PARTS_ID as nvarchar(20)) + ' )'' )'; 

    EXECUTE sp_executesql @SQLString, @ParmDefinition, @ErrorNum_Ib = @ERROR_NUM output, @ErrorText_Ib = @ERROR_MES output;
    
    IF( @ERROR_NUM <> 0 )
			RETURN @ERROR_NUM;

		-- Для каждой записи в списке сертификатов выполняется процедура сохранения в Interbase    
	  DECLARE crCertificateIB CURSOR
	  FOR SELECT Certificate_Guid, CERTIFICATETYPE_GUID, CAST( CERTIFICATE_NUM as nvarchar(128) ), CAST( CERTIFICATE_WHOGIVE as nvarchar(128) ), 
			CERTIFICATE_BEGINDATE, CERTIFICATE_ENDDATE, CERTIFICATE_NUMFORWAYBILL, CERTIFICATE_DESCRIPTION, CERTIFICATE_ISACTIVE, ActionType_Id
	  FROM @tCertificate;
	  OPEN crCertificateIB;
	  FETCH NEXT FROM crCertificateIB INTO @Certificate_Guid, @CERTIFICATETYPE_GUID, @CERTIFICATE_NUM, @CERTIFICATE_WHOGIVE, @CERTIFICATE_BEGINDATE, 
			@CERTIFICATE_ENDDATE, @CERTIFICATE_NUMFORWAYBILL, @CERTIFICATE_DESCRIPTION, @CERTIFICATE_ISACTIVE,  @ActionType_Id;
	  WHILE ( @@fetch_status = 0)
		  BEGIN
        SELECT @CERTIFICATETYPE_ID = CERTIFICATETYPE_ID FROM T_CertificateType WHERE CertificateType_Guid = @CERTIFICATETYPE_GUID;

				SET @strCERTIFICATE_BEGINDATE = CONVERT (varchar(10), @CERTIFICATE_BEGINDATE, 104 );

				SET @strCERTIFICATE_ENDDATE = CONVERT (varchar(10), @CERTIFICATE_ENDDATE, 104 );

				IF( @CERTIFICATE_WHOGIVE IS NULL ) SET @CERTIFICATE_WHOGIVE = '';
				IF( @CERTIFICATE_DESCRIPTION IS NULL ) SET @CERTIFICATE_DESCRIPTION = '';
				IF( @CERTIFICATE_NUMFORWAYBILL IS NULL ) SET @CERTIFICATE_NUMFORWAYBILL = '';
				IF( @CERTIFICATE_ISACTIVE IS NULL ) SET @CERTIFICATE_ISACTIVE = 0;
        
 						BEGIN TRY
							SET @SQLString = 'SELECT @ErrorNum_Ib = ERROR_NUMBER, @ErrorText_Ib = ERROR_TEXT FROM OPENQUERY( ' + 
								@IBLINKEDSERVERNAME + ', ''SELECT ERROR_NUMBER, ERROR_TEXT FROM SP_EDIT_CERTIFICATELIST_FOR_PARTS( ' + 
									 CAST( @CERTIFICATETYPE_ID as varchar(20)) + ', '  + 
									 CAST( @PARTS_ID as varchar(20)) + ', '  + 
									'''''' +	CAST( @CERTIFICATE_NUM as varchar(128)) +  ''''', ' +
									'''''' +	CAST( @CERTIFICATE_WHOGIVE as varchar(128)) +  ''''',  ' + 
									'''''' +	CAST( @strCERTIFICATE_BEGINDATE as varchar(20)) +  ''''',  ' + 
									'''''' +	CAST( @strCERTIFICATE_ENDDATE as varchar(20)) +  ''''', ' + 
									'''''' +	CAST( @CERTIFICATE_NUMFORWAYBILL as varchar(256)) +  ''''', ' +
									'''''' +	CAST( @CERTIFICATE_DESCRIPTION as varchar(256)) +  ''''', ' +
									 CAST( @CERTIFICATE_ISACTIVE as varchar(2)) + 	' )'' )'; 
										
							EXECUTE sp_executesql @SQLString, @ParmDefinition, @ErrorNum_Ib = @ERROR_NUM output, @ErrorText_Ib = @ERROR_MES output;
							IF( @ERROR_NUM <> 0 ) BREAK;
							
 						END TRY
						BEGIN CATCH
							SET @ERROR_NUM = ERROR_NUMBER();
							SET @ERROR_MES = ( @ERROR_MES + ' ' + ERROR_MESSAGE() );
							CLOSE crCertificateIB;
							DEALLOCATE crCertificateIB;

							RETURN @ERROR_NUM;
						END CATCH;

			  FETCH NEXT FROM crCertificateIB INTO @Certificate_Guid, @CERTIFICATETYPE_GUID, @CERTIFICATE_NUM, @CERTIFICATE_WHOGIVE, @CERTIFICATE_BEGINDATE, 
					@CERTIFICATE_ENDDATE, @CERTIFICATE_NUMFORWAYBILL, @CERTIFICATE_DESCRIPTION, @CERTIFICATE_ISACTIVE, @ActionType_Id;
		  END -- while
  	
	  CLOSE crCertificateIB;
	  DEALLOCATE crCertificateIB;
    
    IF( @ERROR_NUM = 0 )
			BEGIN
				BEGIN TRY
					BEGIN TRANSACTION UpdateData;
					
					DECLARE @NullGuid D_GUID;
					SELECT @NullGuid = ( SELECT dbo.GetNullGuid() );

					-- В таблице удаляются те сертификаты, коорых нет в списке 
					DELETE FROM dbo.T_PartsCertificate WHERE Parts_Guid = @Parts_Guid AND Certificate_Guid NOT IN ( SELECT Certificate_Guid FROM @tCertificate );

					CREATE TABLE #Certificate( Certificate_Guid uniqueidentifier,
						CertificateType_Guid uniqueidentifier,
						Certificate_Num nvarchar(256),
						Certificate_WhoGive nvarchar(256),
						Certificate_BeginDate date,
						Certificate_EndDate date,
						Certificate_NumForWaybill nvarchar(256),
						Certificate_IsActive bit,
						Certificate_Description nvarchar(512),
						Certificate_Image image,
						Certificate_ImageFileFullName nvarchar(256),
						ActionType_Id int
					);

					INSERT INTO #Certificate( Certificate_Guid,	CertificateType_Guid,	Certificate_Num,	Certificate_WhoGive,
						Certificate_BeginDate, Certificate_EndDate,	Certificate_NumForWaybill,	Certificate_IsActive,
						Certificate_Description,	Certificate_Image, Certificate_ImageFileFullName, ActionType_Id	)
					SELECT Certificate_Guid,	CertificateType_Guid,	Certificate_Num,	Certificate_WhoGive,
						Certificate_BeginDate, Certificate_EndDate,	Certificate_NumForWaybill,	Certificate_IsActive,
						Certificate_Description, Certificate_Image, Certificate_ImageFileFullName, ActionType_Id
					FROM @tCertificate;

					UPDATE #Certificate SET Certificate_Guid = NEWID() WHERE Certificate_Guid = @NullGuid;

					-- Для каждой записи в списке сертификатов выполняется процедура сохранения в SQL Server    
					DECLARE crCertificate CURSOR
					FOR SELECT Certificate_Guid, CERTIFICATETYPE_GUID, CERTIFICATE_NUM, CERTIFICATE_WHOGIVE, 
						CERTIFICATE_BEGINDATE, CERTIFICATE_ENDDATE, CERTIFICATE_NUMFORWAYBILL, CERTIFICATE_DESCRIPTION, 
						CERTIFICATE_ISACTIVE, Certificate_ImageFileFullName, ActionType_Id
					FROM #Certificate;
					OPEN crCertificate;
					FETCH NEXT FROM crCertificate INTO @Certificate_Guid, @CERTIFICATETYPE_GUID, @CERTIFICATE_NUM, @CERTIFICATE_WHOGIVE, 
						@CERTIFICATE_BEGINDATE, @CERTIFICATE_ENDDATE, @CERTIFICATE_NUMFORWAYBILL, @CERTIFICATE_DESCRIPTION, @CERTIFICATE_ISACTIVE, 
						@CERTIFICATE_IMAGEFILEFULLNAME, @ActionType_Id;
					WHILE ( @@fetch_status = 0)
						BEGIN
        
 							BEGIN TRY
								
								IF NOT EXISTS ( SELECT Certificate_Guid = [Certificate_Guid] FROM [dbo].[T_PartsCertificate] 
																WHERE [Certificate_Guid] = @Certificate_Guid AND Parts_Guid = @Parts_Guid)
										INSERT INTO [dbo].[T_PartsCertificate]( Certificate_Guid, CertificateType_Guid, Parts_Guid, Certificate_Num, Certificate_WhoGive, 
											Certificate_BeginDate, Certificate_EndDate, Certificate_NumForWaybill, Certificate_IsActive, Certificate_Description,  
											Certificate_ImageFileFullName, Record_Updated, Record_UserUdpated )
										VALUES( @Certificate_Guid, @CERTIFICATETYPE_GUID, @Parts_Guid,  @CERTIFICATE_NUM, @CERTIFICATE_WHOGIVE, 
											@CERTIFICATE_BEGINDATE, @CERTIFICATE_ENDDATE,	@CERTIFICATE_NUMFORWAYBILL, @CERTIFICATE_ISACTIVE, @CERTIFICATE_DESCRIPTION, 
											@CERTIFICATE_IMAGEFILEFULLNAME, sysutcdatetime(), ( Host_Name() + ': ' + SUSER_SNAME() ) );
								ELSE
									UPDATE [dbo].[T_PartsCertificate] SET [Certificate_Num] = @CERTIFICATE_NUM, [Certificate_WhoGive] = @CERTIFICATE_WHOGIVE, 
										[Certificate_BeginDate] = @CERTIFICATE_BEGINDATE, [Certificate_EndDate] = @CERTIFICATE_ENDDATE, 
										[Certificate_NumForWaybill] = @CERTIFICATE_NUMFORWAYBILL, [Certificate_Description] = @CERTIFICATE_DESCRIPTION,
										[Certificate_IsActive] = @CERTIFICATE_ISACTIVE, Certificate_ImageFileFullName = @CERTIFICATE_IMAGEFILEFULLNAME
									WHERE Certificate_Guid = @Certificate_Guid AND Parts_Guid = @Parts_Guid	;

								IF( @ActionType_Id = 1 )
									-- заменить картинку
									UPDATE [dbo].[T_PartsCertificate] SET Certificate_Image = ( SELECT Top 1 Certificate_Image FROM #Certificate WHERE Certificate_Guid = @Certificate_Guid )
									WHERE Certificate_Guid = @Certificate_Guid AND Parts_Guid = @Parts_Guid;
								ELSE IF( @ActionType_Id = 2 )
									-- удалить картинку
									UPDATE [dbo].[T_PartsCertificate] SET Certificate_Image = NULL
									WHERE Certificate_Guid = @Certificate_Guid AND Parts_Guid = @Parts_Guid;


 							END TRY
							BEGIN CATCH
								ROLLBACK TRANSACTION UpdateData;
								SET @ERROR_NUM = ERROR_NUMBER();
								SET @ERROR_MES = ( @ERROR_MES + ' ' + ERROR_MESSAGE() );
								CLOSE crCertificate;
								DEALLOCATE crCertificate;

								RETURN @ERROR_NUM;

							END CATCH;

							FETCH NEXT FROM crCertificate INTO @Certificate_Guid, @CERTIFICATETYPE_GUID, @CERTIFICATE_NUM, @CERTIFICATE_WHOGIVE, 
								@CERTIFICATE_BEGINDATE, @CERTIFICATE_ENDDATE, @CERTIFICATE_NUMFORWAYBILL, @CERTIFICATE_DESCRIPTION, 
								@CERTIFICATE_ISACTIVE, @CERTIFICATE_IMAGEFILEFULLNAME, @ActionType_Id;
						END -- while
  	
					CLOSE crCertificate;
					DEALLOCATE crCertificate;
					
					COMMIT TRANSACTION UpdateData;
 				END TRY
				BEGIN CATCH
					ROLLBACK TRANSACTION UpdateData;
					SET @ERROR_NUM = ERROR_NUMBER();
					SET @ERROR_MES = 'Ошибка редактирования списка сертификатов. ' + ERROR_MESSAGE();
					RETURN @ERROR_NUM;
				END CATCH;

			END

 	END TRY
	BEGIN CATCH
    SET @ERROR_NUM = ERROR_NUMBER();
    SET @ERROR_MES = ( 'Ошибка редактирования списка сертификатов. ' + ERROR_MESSAGE() );
		RETURN @ERROR_NUM;
	END CATCH;

	IF( @ERROR_NUM = 0 )
		SET @ERROR_MES = 'Список сертификатов успешно сохранён.'

	RETURN @ERROR_NUM;
END



GO
GRANT EXECUTE ON [dbo].[usp_AssignCertificateWithPart] TO [public]
GO


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- Возвращает изображение сертификата
--
-- Входные параметры:
--
--		@Certificate_Guid - УИ сертификата
-- 
-- Выходные параметры:
--
--		@ERROR_NUM - номер ошибки
--		@ERROR_MES - текст ошибки
--
-- Результат:
--    0 - успешное завершение
--    <>0 - ошибка запроса информации из базы данных

CREATE PROCEDURE [dbo].[usp_GetPartCertificateImage] 
	@Certificate_Guid D_GUID,
	
  @ERROR_NUM int output,
  @ERROR_MES nvarchar(4000) output
AS

BEGIN

  SET @ERROR_NUM = 0;
  SET @ERROR_MES = '';

  BEGIN TRY

    SELECT [Certificate_Image], [Certificate_ImageFileFullName] FROM [dbo].[T_PartsCertificate]
    WHERE [Certificate_Guid] = @Certificate_Guid

	END TRY
	BEGIN CATCH
		SET @ERROR_NUM = ERROR_NUMBER();
		SET @ERROR_MES = ERROR_MESSAGE();

		RETURN @ERROR_NUM;
	END CATCH;

	IF( @ERROR_NUM = 0 )
		SET @ERROR_MES = 'Список сертификатов успешно сохранён.'

  RETURN @ERROR_NUM;
END

GO
GRANT EXECUTE ON [dbo].[usp_GetPartCertificateImage] TO [public]
GO

