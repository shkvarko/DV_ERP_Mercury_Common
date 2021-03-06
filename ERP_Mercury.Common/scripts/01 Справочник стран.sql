USE [ERP_Mercury]
GO

ALTER TABLE [dbo].[T_Country] ADD Country_Id D_ID
GO

UPDATE [dbo].[T_Country] SET Country_Id = 1  WHERE [Country_Guid] = '484C3304-1625-4527-95AF-0330CB7349B8'
UPDATE [dbo].[T_Country] SET Country_Id = 2  WHERE [Country_Guid] = 'D952D5BA-6474-4ABB-8C31-D42984ABC8A4'
UPDATE [dbo].[T_Country] SET Country_Id = 3  WHERE [Country_Guid] = 'A376B1D8-07DD-45FF-94F6-B8FB82296877'
UPDATE [dbo].[T_Country] SET Country_Id = 4  WHERE [Country_Guid] = 'F1ABDFB4-5791-428C-9A59-D15E68F538E1'
UPDATE [dbo].[T_Country] SET Country_Id = 5  WHERE [Country_Guid] = '5C36EEF7-585A-429F-BA57-2E72E32EC7B9'
UPDATE [dbo].[T_Country] SET Country_Id = 6  WHERE [Country_Guid] = 'AF8833EF-35C0-457E-BB18-7A61A0B6F332'
UPDATE [dbo].[T_Country] SET Country_Id = 7  WHERE [Country_Guid] = '172AB7AD-0896-4E60-BB56-B9BB2B4D9F81'
UPDATE [dbo].[T_Country] SET Country_Id = 8  WHERE [Country_Guid] = '50465BCA-34FD-445D-8DF9-1F073325BCB1'
UPDATE [dbo].[T_Country] SET Country_Id = 9  WHERE [Country_Guid] = '0431ABFF-6F3A-46EC-8A18-DA5ECB834451'
UPDATE [dbo].[T_Country] SET Country_Id = 10  WHERE [Country_Guid] = 'A32E6E99-3A9B-4A80-B213-A464C5C5C0C1'
UPDATE [dbo].[T_Country] SET Country_Id = 11  WHERE [Country_Guid] = '31EFFCB3-BE43-41BC-9EA9-FE1D4884C823'
UPDATE [dbo].[T_Country] SET Country_Id = 12  WHERE [Country_Guid] = '82140163-9674-4D4F-B825-5876BD906BE2'
UPDATE [dbo].[T_Country] SET Country_Id = 13  WHERE [Country_Guid] = 'F01342A0-FD54-40B2-B54D-D508C5B871F4'
UPDATE [dbo].[T_Country] SET Country_Id = 14  WHERE [Country_Guid] = '775F35B8-3F10-470D-B3DC-5F3CE12F9928'
UPDATE [dbo].[T_Country] SET Country_Id = 15  WHERE [Country_Guid] = 'AE47DC5A-900F-409E-83D2-B3BCA00C15D9'
UPDATE [dbo].[T_Country] SET Country_Id = 19  WHERE [Country_Guid] = 'AA322390-5D59-42F7-914A-3880C9A9F306'
UPDATE [dbo].[T_Country] SET Country_Id = 17  WHERE [Country_Guid] = '232E005A-1A05-407E-9896-7ECF8689A6DD'
GO

SET ANSI_PADDING ON
GO

CREATE UNIQUE NONCLUSTERED INDEX [INDX_T_Country_Country_ISOCode] ON [dbo].[T_Country]
(
	[Country_ISOCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [INDEX]

GO

CREATE UNIQUE NONCLUSTERED INDEX [INDX_T_Country_Country_Code] ON [dbo].[T_Country]
(
	[Country_Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [INDEX]

GO

CREATE UNIQUE NONCLUSTERED INDEX [INDX_T_Country_Country_Id] ON [dbo].[T_Country]
(
	[Country_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [INDEX]

GO

/****** Object:  StoredProcedure [dbo].[sp_GetCountry]    Script Date: 11.01.2013 13:13:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- Возвращает список записей из ( dbo.T_Country )
--
-- Входящие параметры:
--
-- Выходные параметры:
--
-- Результат:
--    0 - успешное завершение
--    <>0 - ошибка запроса информации из базы данных

ALTER PROCEDURE [dbo].[sp_GetCountry] 
  @ERROR_NUM int output,
  @ERROR_MES nvarchar(4000) output
AS

BEGIN

  SET @ERROR_NUM = 0;
  SET @ERROR_MES = '';

  BEGIN TRY

    SELECT Country_Guid, Country_ISOCode, Country_Name, Country_Code, Country_Id
    FROM dbo.T_Country
    ORDER BY Country_Name;

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

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Добавляет в InterBase информацию о стране

-- Входные параметры
-- @Country_Guid - УИ страны
-- @IBLINKEDSERVERNAME - имя LINKEDSERVER

-- Выходные параметры
-- @ERROR_NUM - номер ошибки
-- @ERROR_MES - сообщение об ошибке

CREATE PROCEDURE [dbo].[usp_AddCountryToIB]
	@Country_Guid D_GUID,
  @IBLINKEDSERVERNAME dbo.D_NAME = NULL,

  @ERROR_NUM int output,
  @ERROR_MES nvarchar(4000) output
AS

BEGIN
  BEGIN TRY
    SET @ERROR_NUM = -1;
    SET @ERROR_MES = '';

    DECLARE @EventID D_ID;
    SET @EventID = NULL;
    DECLARE @ParentEventID D_ID;
    SET @ParentEventID = NULL;
    DECLARE @strMessage D_EVENTMSG;
    SET @strMessage = '';
    DECLARE @EventSrc D_NAME;
	  SET @EventSrc = 'Добавление страны в IB';

 	  IF( @IBLINKEDSERVERNAME IS NULL ) SELECT @IBLINKEDSERVERNAME = dbo.GetIBLinkedServerName();

    DECLARE @strIBSQLText nvarchar( 250 );
    DECLARE	@COUNTRY_ID       INTEGER;
    DECLARE	@COUNTRY_NAME     NVARCHAR(52);
    DECLARE	@COUNTRY_CODE     NVARCHAR(32);
    DECLARE	@COUNTRY_ISOCODE  NVARCHAR(10);
    
		-- Проверяем, есть ли страна с указанным идентификатором 
    IF NOT EXISTS ( SELECT * FROM dbo.T_Country WHERE Country_Guid = @Country_Guid )
      BEGIN
        SET @ERROR_NUM = 1;
        SET @ERROR_MES = '[usp_AddCountryToIB] В базе данных не найдена страна с указанным идентификатором.' + nChar(13) + nChar(10) + CONVERT( nvarchar(36), @Country_Guid );
        RETURN @ERROR_NUM;
      END
      
		SELECT @COUNTRY_ID = Country_Id, @COUNTRY_NAME = Country_Name, @COUNTRY_CODE = Country_Code, @COUNTRY_ISOCODE = Country_ISOCode 
		FROM dbo.T_Country
		WHERE Country_Guid = @Country_Guid;
		
		IF( @COUNTRY_ID IS NULL ) SET @COUNTRY_ID = 0;
		IF( @COUNTRY_NAME IS NULL ) SET @COUNTRY_NAME = '';
		IF( @COUNTRY_CODE IS NULL ) SET @COUNTRY_CODE = '';
		IF( @COUNTRY_ISOCODE IS NULL ) SET @COUNTRY_ISOCODE = '';
		
		IF( ( @COUNTRY_ID IS NULL ) OR ( @COUNTRY_ID = 0 ) )
      BEGIN
        SET @ERROR_NUM = 2;
        SET @ERROR_MES = '[usp_AddCountryToIB] Не найден идентификатор страны в базе InterBase.';
        RETURN @ERROR_NUM;
      END
		
    DECLARE @SQLString nvarchar( 2048 );
    DECLARE @ParmDefinition nvarchar( 500 );
    DECLARE @RETURNVALUE int;


    SET @ParmDefinition = N'@ErrorNum_Ib int output, @ErrorText_Ib varchar(480) output'; 

    -- Добавляем запись в таблицу T_SALE в InterBase
    SET @SQLString = 'SELECT @ErrorNum_Ib = ERROR_NUMBER, @ErrorText_Ib = ERROR_TEXT FROM OPENQUERY( ' + 
			@IBLINKEDSERVERNAME + ', ''SELECT ERROR_NUMBER, ERROR_TEXT FROM SP_ADD_COUNTRY_PROD_FROMSQL( ' + cast( @COUNTRY_ID as nvarchar( 20)) + ', ' +
					'''''' + cast( @COUNTRY_NAME as nvarchar( 52 )) + '''''' + ', ' +
					'''''' + cast( @COUNTRY_CODE as nvarchar( 48 )) + '''''' + ', ' + 
					'''''' + cast( @COUNTRY_ISOCODE as nvarchar( 48 )) + '''''' + ' )'' )'; 
					

    EXECUTE sp_executesql @SQLString, @ParmDefinition, @ErrorNum_Ib = @ERROR_NUM output, @ErrorText_Ib = @ERROR_MES output;
    
    PRINT @SQLString;

 	END TRY
	BEGIN CATCH
    SET @ERROR_NUM = ERROR_NUMBER();
    SET @ERROR_MES = @SQLString + ' ' + @ERROR_MES + ' ' + ERROR_MESSAGE();
    SET @strMessage = @ERROR_MES;
    EXEC dbo.spAddEventToLog @EVENT_SOURCE = @EventSrc, @EVENT_CATEGORY = 'None', 
      @EVENT_COMPUTER = ' ', @EVENT_TYPE = 'Error', @EVENT_IS_COMPOSITE = 0, 
      @EVENT_DESCRIPTION = @strMessage, @EVENT_PARENTID = @ParentEventID, @EVENT_ID = @EventID output;

		RETURN @ERROR_NUM;
	END CATCH;

	
	IF( @ERROR_NUM = 0 ) 
		SET @ERROR_MES = 'Успешное завершение операции.';
	
	RETURN @ERROR_NUM;

END

GO
GRANT EXECUTE ON [dbo].[usp_AddCountryToIB] TO [public]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Редактирует в InterBase информацию о стране

-- Входные параметры
-- @Country_Guid - УИ страны
-- @IBLINKEDSERVERNAME - имя LINKEDSERVER

-- Выходные параметры
-- @ERROR_NUM - номер ошибки
-- @ERROR_MES - сообщение об ошибке

CREATE PROCEDURE [dbo].[usp_EditCountryToIB]
	@Country_Guid D_GUID,
  @IBLINKEDSERVERNAME dbo.D_NAME = NULL,

  @ERROR_NUM int output,
  @ERROR_MES nvarchar(4000) output
AS

BEGIN
  BEGIN TRY
    SET @ERROR_NUM = -1;
    SET @ERROR_MES = '';

    DECLARE @EventID D_ID;
    SET @EventID = NULL;
    DECLARE @ParentEventID D_ID;
    SET @ParentEventID = NULL;
    DECLARE @strMessage D_EVENTMSG;
    SET @strMessage = '';
    DECLARE @EventSrc D_NAME;
	  SET @EventSrc = 'редактирование страны в IB';

 	  IF( @IBLINKEDSERVERNAME IS NULL ) SELECT @IBLINKEDSERVERNAME = dbo.GetIBLinkedServerName();

    DECLARE @strIBSQLText nvarchar( 250 );
    DECLARE	@COUNTRY_ID       INTEGER;
    DECLARE	@COUNTRY_NAME     NVARCHAR(52);
    DECLARE	@COUNTRY_CODE     NVARCHAR(32);
    DECLARE	@COUNTRY_ISOCODE  NVARCHAR(10);
    
		-- Проверяем, есть ли страна с указанным идентификатором 
    IF NOT EXISTS ( SELECT * FROM dbo.T_Country WHERE Country_Guid = @Country_Guid )
      BEGIN
        SET @ERROR_NUM = 1;
        SET @ERROR_MES = '[usp_EditCountryToIB] В базе данных не найдена страна с указанным идентификатором.' + nChar(13) + nChar(10) + CONVERT( nvarchar(36), @Country_Guid );
        RETURN @ERROR_NUM;
      END
      
		SELECT @COUNTRY_ID = Country_Id, @COUNTRY_NAME = Country_Name, @COUNTRY_CODE = Country_Code, @COUNTRY_ISOCODE = Country_ISOCode 
		FROM dbo.T_Country
		WHERE Country_Guid = @Country_Guid;
		
		IF( @COUNTRY_ID IS NULL ) SET @COUNTRY_ID = 0;
		IF( @COUNTRY_NAME IS NULL ) SET @COUNTRY_NAME = '';
		IF( @COUNTRY_CODE IS NULL ) SET @COUNTRY_CODE = '';
		IF( @COUNTRY_ISOCODE IS NULL ) SET @COUNTRY_ISOCODE = '';
		
		IF( ( @COUNTRY_ID IS NULL ) OR ( @COUNTRY_ID = 0 ) )
      BEGIN
        SET @ERROR_NUM = 2;
        SET @ERROR_MES = '[usp_EditCountryToIB] Не найден идентификатор страны в базе InterBase.';
        RETURN @ERROR_NUM;
      END
		
    DECLARE @SQLString nvarchar( 2048 );
    DECLARE @ParmDefinition nvarchar( 500 );
    DECLARE @RETURNVALUE int;


    SET @ParmDefinition = N'@ErrorNum_Ib int output, @ErrorText_Ib varchar(480) output'; 

    -- Добавляем запись в таблицу T_SALE в InterBase
    SET @SQLString = 'SELECT @ErrorNum_Ib = ERROR_NUMBER, @ErrorText_Ib = ERROR_TEXT FROM OPENQUERY( ' + 
			@IBLINKEDSERVERNAME + ', ''SELECT ERROR_NUMBER, ERROR_TEXT FROM SP_EDIT_COUNTRY_PROD_FROMSQL( ' + cast( @COUNTRY_ID as nvarchar( 20)) + ', ' +
					'''''' + cast( @COUNTRY_NAME as nvarchar( 52 )) + '''''' + ', ' +
					'''''' + cast( @COUNTRY_CODE as nvarchar( 48 )) + '''''' + ', ' + 
					'''''' + cast( @COUNTRY_ISOCODE as nvarchar( 48 )) + '''''' + ' )'' )'; 
					

    EXECUTE sp_executesql @SQLString, @ParmDefinition, @ErrorNum_Ib = @ERROR_NUM output, @ErrorText_Ib = @ERROR_MES output;
    
    PRINT @SQLString;

 	END TRY
	BEGIN CATCH
    SET @ERROR_NUM = ERROR_NUMBER();
    SET @ERROR_MES = @SQLString + ' ' + @ERROR_MES + ' ' + ERROR_MESSAGE();
    SET @strMessage = @ERROR_MES;
    EXEC dbo.spAddEventToLog @EVENT_SOURCE = @EventSrc, @EVENT_CATEGORY = 'None', 
      @EVENT_COMPUTER = ' ', @EVENT_TYPE = 'Error', @EVENT_IS_COMPOSITE = 0, 
      @EVENT_DESCRIPTION = @strMessage, @EVENT_PARENTID = @ParentEventID, @EVENT_ID = @EventID output;

		RETURN @ERROR_NUM;
	END CATCH;

	
	IF( @ERROR_NUM = 0 )
		SET @ERROR_MES = 'Успешное завершение операции.';

	RETURN @ERROR_NUM;

END

GO
GRANT EXECUTE ON [dbo].[usp_EditCountryToIB] TO [public]
GO


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Удаляет в InterBase информацию о стране

-- Входные параметры
-- @Country_Guid - УИ страны

-- Выходные параметры
-- @ERROR_NUM - номер ошибки
-- @ERROR_MES - сообщение об ошибке
-- @IBLINKEDSERVERNAME - имя LINKEDSERVER

CREATE PROCEDURE [dbo].[usp_DeleteCountryFromIB]
	@Country_Guid D_GUID,
  @IBLINKEDSERVERNAME dbo.D_NAME = NULL,

  @ERROR_NUM int output,
  @ERROR_MES nvarchar(4000) output
AS

BEGIN
  BEGIN TRY
    SET @ERROR_NUM = -1;
    SET @ERROR_MES = '';

    DECLARE @EventID D_ID;
    SET @EventID = NULL;
    DECLARE @ParentEventID D_ID;
    SET @ParentEventID = NULL;
    DECLARE @strMessage D_EVENTMSG;
    SET @strMessage = '';
    DECLARE @EventSrc D_NAME;
	  SET @EventSrc = 'Удаление страны в IB';

 	  IF( @IBLINKEDSERVERNAME IS NULL ) SELECT @IBLINKEDSERVERNAME = dbo.GetIBLinkedServerName();

    DECLARE	@COUNTRY_ID  int;
    
		-- Проверяем, есть ли страна с указанным идентификатором 
    IF NOT EXISTS ( SELECT * FROM dbo.T_Country WHERE Country_Guid = @Country_Guid )
      BEGIN
        SET @ERROR_NUM = 1;
        SET @ERROR_MES = '[usp_DeleteCountryFromIB] В базе данных не найдена страна с указанным идентификатором.' + nChar(13) + nChar(10) + CONVERT( nvarchar(36), @Country_Guid );
        RETURN @ERROR_NUM;
      END

      
		SELECT @COUNTRY_ID = Country_Id	FROM dbo.T_Country WHERE Country_Guid = @Country_Guid;
		
		IF( @COUNTRY_ID IS NULL ) SET @COUNTRY_ID = 0;
		
    DECLARE @SQLString nvarchar( 2048 );
    DECLARE @ParmDefinition nvarchar( 500 );
    DECLARE @RETURNVALUE int;


    SET @ParmDefinition = N'@ErrorNum_Ib int output, @ErrorText_Ib varchar(480) output'; 

    SET @SQLString = 'SELECT @ErrorNum_Ib = ERROR_NUMBER, @ErrorText_Ib = ERROR_TEXT FROM OPENQUERY( ' + 
			@IBLINKEDSERVERNAME + ', ''SELECT ERROR_NUMBER, ERROR_TEXT FROM SP_DELETE_COUNTRY_PROD_FROMSQL( ' + 
									 cast( @COUNTRY_ID as nvarchar(20)) + ' )'' )'; 
					

    EXECUTE sp_executesql @SQLString, @ParmDefinition, @ErrorNum_Ib = @ERROR_NUM output, @ErrorText_Ib = @ERROR_MES output;
    
    --PRINT @SQLString;

 	END TRY
	BEGIN CATCH
    SET @ERROR_NUM = ERROR_NUMBER();
    SET @ERROR_MES = '[usp_DeleteCountryFromIB] ' + @SQLString + ' ' + @ERROR_MES + ' ' + ERROR_MESSAGE();
    SET @strMessage = @ERROR_MES;
    EXEC dbo.spAddEventToLog @EVENT_SOURCE = @EventSrc, @EVENT_CATEGORY = 'None', 
      @EVENT_COMPUTER = ' ', @EVENT_TYPE = 'Error', @EVENT_IS_COMPOSITE = 0, 
      @EVENT_DESCRIPTION = @strMessage, @EVENT_PARENTID = @ParentEventID, @EVENT_ID = @EventID output;

		RETURN @ERROR_NUM;
	END CATCH;

	
	IF( @ERROR_NUM = 0 )SET @ERROR_MES = 'Успешное завершение операции.';
	RETURN @ERROR_NUM;

END

GO
GRANT EXECUTE ON [dbo].[usp_DeleteCountryFromIB] TO [public]
GO

/****** Object:  StoredProcedure [dbo].[sp_AddCountry]    Script Date: 11.01.2013 14:10:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Добавляет новую запись в таблицу dbo.T_Country
--
-- Входящие параметры:
--	@Country_Name - наименование страны
--	@Country_Code - код страны (символьный)
--	@Country_ISOCode - код страны (числовой)

--
-- Выходные параметры:
--  @Country_Id - уникальный идентификатор записи в InterBase
--  @Country_Guid - уникальный идентификатор записи
--  @ERROR_NUM - номер ошибки
--  @ERROR_MES - текст ошибки
--
-- Результат:
--    0 - Успешное завершение
--    <>0 - ошибка

ALTER PROCEDURE [dbo].[sp_AddCountry] 
	@Country_Name			D_NAME,
	@Country_Code			D_COUNTRYCODE,
	@Country_ISOCode	D_NAMESHORT,

	@Country_Id				D_ID output,
  @Country_Guid			D_GUID output,
  @ERROR_NUM				int output,
  @ERROR_MES				nvarchar(4000) output

AS

BEGIN

	BEGIN TRY

    SET @ERROR_NUM = 0;
    SET @ERROR_MES = NULL;
    SET @Country_Guid = NULL;
		SET @Country_Id = 0;

    -- Проверяем наличие записи с заданным именем
    IF EXISTS ( SELECT * FROM dbo.T_Country WHERE Country_Name = @Country_Name )
      BEGIN
        SET @ERROR_NUM = 1;
        SET @ERROR_MES = 'В базе данных уже присутствует название страны.' + Char(13) + 
          'Имя: ' + Char(9) + @Country_Name;
        RETURN @ERROR_NUM;
      END
    IF EXISTS ( SELECT * FROM dbo.T_Country WHERE Country_Code = @Country_Code )
      BEGIN
        SET @ERROR_NUM = 1;
        SET @ERROR_MES = 'В базе данных уже присутствует код страны.' + Char(13) + 
          'Код: ' + Char(9) + @Country_Code;
        RETURN @ERROR_NUM;
      END
    IF EXISTS ( SELECT * FROM dbo.T_Country WHERE Country_ISOCode = @Country_ISOCode )
      BEGIN
        SET @ERROR_NUM = 1;
        SET @ERROR_MES = 'В базе данных уже присутствует код страны.' + Char(13) + 
          'Код: ' + Char(9) + @Country_ISOCode;
        RETURN @ERROR_NUM;
      END

    DECLARE @NewID D_GUID;
    SET @NewID = NEWID ( );	
    EXEC @Country_Id = SP_GetGeneratorID @strTableName = 'T_Country';

		BEGIN TRANSACTION UpdateData;

    INSERT INTO dbo.T_Country( Country_Guid, Country_Name, Country_Code, Country_ISOCode, Country_Id )
    VALUES( @NewID, @Country_Name, @Country_Code, @Country_ISOCode, @Country_Id );
    
    SET @Country_Guid = @NewID;

    EXEC dbo.usp_AddCountryToIB @Country_Guid = @Country_Guid, @IBLINKEDSERVERNAME = NULL, 
     @ERROR_NUM = @ERROR_NUM output, @ERROR_MES = @ERROR_MES output;

		IF( @ERROR_NUM = 0 ) COMMIT TRANSACTION UpdateData
				ELSE ROLLBACK TRANSACTION UpdateData;
    

	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION UpdateData;
    SET @ERROR_NUM = ERROR_NUMBER();
    SET @ERROR_MES = ERROR_MESSAGE();
		RETURN @ERROR_NUM;
	END CATCH;

	IF( @ERROR_NUM = 0 )
		SET @ERROR_MES = 'Успешное завершение операции.';

	RETURN @ERROR_NUM;
END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Редактирует запись в таблице dbo.T_Country
--
-- Входящие параметры:
--  @Country_Guid - уникальный идентификатор записи
--	@Country_Name - наименование страны
--	@Country_Code - код страны (символьный)
--	@Country_ISOCode - код страны (числовой)

--
-- Выходные параметры:
--  @ERROR_NUM - номер ошибки
--  @ERROR_MES - текст ошибки
--
-- Результат:
--    0 - Успешное завершение
--    <>0 - ошибка

ALTER PROCEDURE [dbo].[sp_EditCountry] 
  @Country_Guid D_GUID,
	@Country_Name D_NAME,
	@Country_Code D_COUNTRYCODE,
	@Country_ISOCode	D_NAMESHORT,

  @ERROR_NUM int output,
  @ERROR_MES nvarchar(4000) output

AS

BEGIN

	BEGIN TRY

    SET @ERROR_NUM = 0;
    SET @ERROR_MES = '';

    -- Проверяем наличие записи с заданным именем
    IF EXISTS ( SELECT * FROM dbo.T_Country WHERE Country_Name = @Country_Name AND Country_Guid <> @Country_Guid )
      BEGIN
        SET @ERROR_NUM = 1;
        SET @ERROR_MES = 'В базе данных уже присутствует название страны.' + Char(13) + 
          'Имя: ' + Char(9) + @Country_Name;
        RETURN @ERROR_NUM;
      END

    IF EXISTS ( SELECT * FROM dbo.T_Country WHERE Country_Code = @Country_Code AND Country_Guid <> @Country_Guid )
      BEGIN
        SET @ERROR_NUM = 1;
        SET @ERROR_MES = 'В базе данных уже присутствует код страны.' + Char(13) + 
          'Код: ' + Char(9) + @Country_Code;
        RETURN @ERROR_NUM;
      END

    IF EXISTS ( SELECT * FROM dbo.T_Country WHERE Country_ISOCode = @Country_ISOCode AND Country_Guid <> @Country_Guid )
      BEGIN
        SET @ERROR_NUM = 1;
        SET @ERROR_MES = 'В базе данных уже присутствует код страны.' + Char(13) + 
          'Код: ' + Char(9) + @Country_ISOCode;
        RETURN @ERROR_NUM;
      END

		BEGIN TRANSACTION UpdateData;

    UPDATE dbo.T_Country SET Country_Name = @Country_Name, Country_Code = @Country_Code, Country_ISOCode = @Country_ISOCode
    WHERE Country_Guid = @Country_Guid;
    
    EXEC dbo.usp_EditCountryToIB @Country_Guid = @Country_Guid, @IBLINKEDSERVERNAME = NULL, 
     @ERROR_NUM = @ERROR_NUM output, @ERROR_MES = @ERROR_MES output;

		IF( @ERROR_NUM = 0 ) COMMIT TRANSACTION UpdateData
				ELSE ROLLBACK TRANSACTION UpdateData;
    
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION UpdateData;
    SET @ERROR_NUM = ERROR_NUMBER();
    SET @ERROR_MES = ERROR_MESSAGE();
		RETURN @ERROR_NUM;
	END CATCH;

	IF( @ERROR_NUM = 0 )
		SET @ERROR_MES = 'Успешное завершение операции.';

	RETURN @ERROR_NUM;
END

GO

/****** Object:  StoredProcedure [dbo].[sp_DeleteCountry]    Script Date: 11.01.2013 14:21:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Удаление элемента из таблицы dbo.T_Country
--
-- Входящие параметры:
--	@Country_Guid - уникальный идентификатор страны
--
-- Результат:
--    0 - успешное завершение
--    <>0 - ошибка

ALTER PROCEDURE [dbo].[sp_DeleteCountry] 
	@Country_Guid D_GUID,

  @ERROR_NUM int output,
  @ERROR_MES nvarchar(4000) output

AS

BEGIN

  SET @ERROR_NUM = 0;
  SET @ERROR_MES = NULL;

	BEGIN TRY

		BEGIN TRANSACTION UpdateData;
		
      EXEC dbo.usp_DeleteCountryFromIB @Country_Guid = @Country_Guid, @IBLINKEDSERVERNAME = NULL, 
	     @ERROR_NUM = @ERROR_NUM output, @ERROR_MES = @ERROR_MES output;

			IF( @ERROR_NUM = 0 ) 
				BEGIN
					DELETE FROM dbo.T_Country WHERE Country_Guid = @Country_Guid;
					
					COMMIT TRANSACTION UpdateData;
				END	
			ELSE ROLLBACK TRANSACTION UpdateData;

   DELETE FROM dbo.T_Country WHERE Country_Guid = @Country_Guid;
    
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

SET ANSI_NULLS ON

GO

	INSERT INTO [dbo].[TS_GENERATOR]( GENERATOR_ID, TABLE_NAME )
	VALUES(20, 'T_Country');
