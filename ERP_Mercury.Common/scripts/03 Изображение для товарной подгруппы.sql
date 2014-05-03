USE [ERP_Mercury]
GO

ALTER TABLE [dbo].[T_Partsubtype] ADD [Partsubtype_ImageFileFullName]	D_NAMELONG NULL;

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- Возвращает изображение товарной подгруппы
--
-- Входные параметры:
--
--		@Partsubtype_Guid - УИ товарной подгруппы
-- 
-- Выходные параметры:
--
--		@ERROR_NUM - номер ошибки
--		@ERROR_MES - текст ошибки
--
-- Результат:
--    0 - успешное завершение
--    <>0 - ошибка запроса информации из базы данных

CREATE PROCEDURE [dbo].[usp_GetPartsubtypeImage] 
	@Partsubtype_Guid D_GUID,
	
  @ERROR_NUM int output,
  @ERROR_MES nvarchar(4000) output
AS

BEGIN

  SET @ERROR_NUM = 0;
  SET @ERROR_MES = '';

  BEGIN TRY

    SELECT [Partsubtype_Image], [Partsubtype_ImageFileFullName] FROM [dbo].[T_Partsubtype]
    WHERE [Partsubtype_Guid] = @Partsubtype_Guid

	END TRY
	BEGIN CATCH
		SET @ERROR_NUM = ERROR_NUMBER();
		SET @ERROR_MES = ERROR_MESSAGE();

		RETURN @ERROR_NUM;
	END CATCH;

	IF( @ERROR_NUM = 0 )
		SET @ERROR_MES = 'Удачное завершение операции.'

  RETURN @ERROR_NUM;
END

GO
GRANT EXECUTE ON [dbo].[usp_GetPartsubtypeImage] TO [public]
GO

/****** Object:  StoredProcedure [dbo].[usp_AddPartSubType]    Script Date: 19.11.2013 12:07:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Добавляет новую запись в таблицу dbo.T_Partsubtype
--
-- Входящие параметры:
-- @InPartsubtype_Id							- код в InterBase в стороннем "Контракте"
--	 @Partsubtype_Name						- наименование
--	 @Partsubtype_Description			- примечание
--	 @Partsubtype_IsActive				- признак активности
--	 @Partline_Guid								- уникальный идентификатор товарной линии
-- @Partsubtype_VendorTariff			- тариф поставщика
-- @Partsubtype_TransportTariff - затраты на транспорт, %
-- @Partsubtype_CustomsTariff		- таможенная пошлина, %
-- @Partsubtype_Margin						- наценка базовая, %
-- @Partsubtype_NDS							- ставка НДС, %
-- @Partsubtype_Discont					- наценка, компенсирующая постоянную скидку, %
-- @PartsubtypeState_Guid				- уи состояния подгруппы
-- @Partsubtype_MarkUpRequired		- требуемая наценка
--	 @Partsubtype_Image						- изображение подгруппы в двоичном виде
-- @Partsubtype_ImageFileFullName - наименование файла с изображением
-- @Partsubtype_ImageActionTypeId - тип действия над изображением (1 - заменить картинку; 2 - удалить картинку)
--
-- Выходные параметры:
--  @Partsubtype_Guid						- уникальный идентификатор записи
--  @Partsubtype_Id							- уникальный идентификатор записи в IB
--  @ERROR_NUM										- номер ошибки
--  @ERROR_MES										- текст ошибки
--
-- Результат:
--    0 - Успешное завершение
--    <>0 - ошибка

ALTER PROCEDURE [dbo].[usp_AddPartSubType] 
	@InPartsubtype_Id D_ID = NULL,
	@Partsubtype_Name D_NAME,
	@Partsubtype_Description D_DESCRIPTION = NULL,
	@Partsubtype_IsActive D_ISACTIVE,
	@Partline_Guid D_GUID,
	@Partsubtype_VendorTariff D_MONEY,
	@Partsubtype_TransportTariff D_MONEY,
	@Partsubtype_CustomsTariff D_MONEY,
	@Partsubtype_Margin D_MONEY,
	@Partsubtype_NDS D_MONEY,
	@Partsubtype_Discont D_MONEY,
	@PartsubtypeState_Guid D_GUID,
	@Partsubtype_MarkUpRequired	D_MONEY = 0,
	
	@Partsubtype_Image image = NULL,
	@Partsubtype_ImageFileFullName nvarchar(256) = NULL,
	@Partsubtype_ImageActionTypeId int = NULL,

	@Partsubtype_Id D_ID output,
  @Partsubtype_Guid D_GUID output,
  @ERROR_NUM int output,
  @ERROR_MES nvarchar(4000) output

AS

BEGIN

	BEGIN TRY

    SET @ERROR_NUM = 0;
    SET @ERROR_MES = NULL;
    DECLARE @tmpERROR_NUM int;
    DECLARE @tmpERROR_MES nvarchar(4000);
    SET @tmpERROR_NUM = 0;
    SET @tmpERROR_MES = '';

		-- 2012.08.17
		-- Проверим, зарегистрирован ли эта товарная подгруппа в системе, как поступившая из другой системы
		IF( ( @InPartsubtype_Id IS NOT NULL ) AND ( @InPartsubtype_Id <> 0 ) )		
			BEGIN
				DECLARE @ExistsPartsubtype_Guid D_GUID = NULL;
				SELECT @ExistsPartsubtype_Guid = Partsubtype_Guid FROM dbo.T_Partsubtype_AdvCode	WHERE Partsubtype_Id = @InPartsubtype_Id;
				
				IF( @ExistsPartsubtype_Guid IS NOT NULL )
					BEGIN
						-- товарная подгруппа была внесена в систему ранее
						IF NOT EXISTS ( SELECT Partsubtype_Guid FROM dbo.T_Partsubtype WHERE Partsubtype_Name = @Partsubtype_Name AND Partsubtype_Guid <> @ExistsPartsubtype_Guid )
							BEGIN
								DECLARE @CurrentPartsubtype_Name	D_NAME;
								SELECT @CurrentPartsubtype_Name = Partsubtype_Name FROM dbo.T_Partsubtype WHERE Partsubtype_Guid = @ExistsPartsubtype_Guid;

								IF( @CurrentPartsubtype_Name <> @Partsubtype_Name )
									BEGIN
										UPDATE dbo.T_Partsubtype SET Partsubtype_Name = @Partsubtype_Name
										WHERE Partsubtype_Guid = @ExistsPartsubtype_Guid;
										
										-- редактируем подгруппу в InterBase
										EXEC dbo.usp_EditPartsubtypeToIB @Partsubtype_Guid = @ExistsPartsubtype_Guid, @Partsubtype_Name = @Partsubtype_Name, 
											@Partline_Guid = @Partline_Guid, @ERROR_NUM = @ERROR_NUM output, @ERROR_MES = @ERROR_MES output;
									END
								
								IF( @ERROR_NUM = 0 )
									BEGIN
										SET @ERROR_NUM = @tmpERROR_NUM;
										SET @ERROR_MES = @tmpERROR_MES;

										SET @Partsubtype_Guid = @ExistsPartsubtype_Guid;
										SET @Partsubtype_Id = @InPartsubtype_Id;
									END
							END
							
					END
				
			END
		
		IF( @ERROR_NUM <> 0 ) RETURN @ERROR_NUM;

    -- Проверяем наличие записи с заданным именем
    IF EXISTS ( SELECT * FROM dbo.T_Partsubtype WHERE Partsubtype_Name = @Partsubtype_Name )
      BEGIN
				SELECT Top 1 @Partsubtype_Guid = Partsubtype_Guid, @Partsubtype_Id = Partsubtype_Id 
				FROM dbo.T_Partsubtype WHERE Partsubtype_Name = @Partsubtype_Name;

				IF( ( @InPartsubtype_Id IS NOT NULL ) AND ( @InPartsubtype_Id <> 0 ) )
					BEGIN
						IF NOT EXISTS ( SELECT Partsubtype_Guid FROM dbo.T_Partsubtype_AdvCode	WHERE Partsubtype_Id = @InPartsubtype_Id )
							INSERT INTO dbo.T_Partsubtype_AdvCode( Partsubtype_Guid, Partsubtype_Id )	VALUES( @Partsubtype_Guid, @InPartsubtype_Id);
					END

        SET @ERROR_NUM = 1;
        SET @ERROR_MES = 'В базе данных уже присутствует товарная подгруппа с указанным именем.' + Char(13) + 
          'Имя: ' + Char(9) + @Partsubtype_Name + Char(13) + 'Код подгруппы: ' + CAST( @Partsubtype_Id as nvarchar( 10 ) );
        RETURN @ERROR_NUM;
      END

    -- Проверяем наличие товарной линии с указанным идентификатором
    IF NOT EXISTS ( SELECT * FROM dbo.T_Partline WHERE Partline_Guid = @Partline_Guid )
      BEGIN
        SET @ERROR_NUM = 2;
        SET @ERROR_MES = 'В базе данных не найдена товарная линия с указанным идентификатором.' + Char(13) + 
          'УИ: ' + Char(9) + CAST( @Partline_Guid as nvarchar( 36 ) );
        RETURN @ERROR_NUM;
      END
      
    -- Проверяем наличие состояния подгруппы с указанным идентификатором
    IF NOT EXISTS ( SELECT PartsubtypeState_Guid FROM dbo.T_PartsubtypeState WHERE PartsubtypeState_Guid = @PartsubtypeState_Guid )
      BEGIN
        SET @ERROR_NUM = 4;
        SET @ERROR_MES = 'В базе данных не найдено состояние подгруппы с указанным идентификатором.' + Char(13) + 
          'УИ: ' + Char(9) + CAST( @PartsubtypeState_Guid as nvarchar( 36 ) );
        RETURN @ERROR_NUM;
      END

    DECLARE @NewID D_GUID;
    SET @NewID = NEWID ( );	
    EXEC @Partsubtype_Id = SP_GetGeneratorID @strTableName = 'T_Partsubtype';
    
    INSERT INTO dbo.T_Partsubtype( Partsubtype_Guid, Partsubtype_Id, Partsubtype_Name, Partsubtype_Description, Partsubtype_IsActive,
			 Partsubtype_VendorTariff, Partsubtype_TransportTariff, Partsubtype_CustomsTariff, Partsubtype_Margin, Partsubtype_NDS, Partsubtype_Discont, 
			 PartsubtypeState_Guid, Partsubtype_MarkUpRequired )
    VALUES( @NewID, @Partsubtype_Id, @Partsubtype_Name, @Partsubtype_Description, @Partsubtype_IsActive, 
			@Partsubtype_VendorTariff, @Partsubtype_TransportTariff, @Partsubtype_CustomsTariff, @Partsubtype_Margin, @Partsubtype_NDS, @Partsubtype_Discont, 
			@PartsubtypeState_Guid, @Partsubtype_MarkUpRequired );

    SET @Partsubtype_Guid = @NewID;
    
    IF( ( @InPartsubtype_Id IS NOT NULL ) AND ( @InPartsubtype_Id <> 0 ) )
			BEGIN
				IF NOT EXISTS ( SELECT Partsubtype_Guid FROM dbo.T_Partsubtype_AdvCode	WHERE Partsubtype_Id = @InPartsubtype_Id )
					INSERT INTO dbo.T_Partsubtype_AdvCode( Partsubtype_Guid, Partsubtype_Id )	VALUES( @Partsubtype_Guid, @InPartsubtype_Id);
			END

    IF NOT EXISTS ( SELECT PartsubtypePartline_Guid FROM dbo.T_PartsubtypePartline WHERE Partsubtype_Guid = @Partsubtype_Guid )
			INSERT INTO dbo.T_PartsubtypePartline( PartsubtypePartline_Guid, Partsubtype_Guid, Partline_Guid )
			VALUES( NEWID(), @Partsubtype_Guid, @Partline_Guid );
		ELSE
			UPDATE 	dbo.T_PartsubtypePartline SET Partline_Guid = @Partline_Guid WHERE Partsubtype_Guid = @Partsubtype_Guid;
    
		IF( @Partsubtype_ImageActionTypeId IS NOT NULL )
			BEGIN
				IF( ( @Partsubtype_ImageActionTypeId = 1 ) AND ( @Partsubtype_Image IS NOT NULL ) )
					BEGIN
						-- заменить картинку
						UPDATE dbo.T_Partsubtype SET [Partsubtype_Image] = @Partsubtype_Image, [Partsubtype_ImageFileFullName] = @Partsubtype_ImageFileFullName
						WHERE [Partsubtype_Guid] = @Partsubtype_Guid;
					END
				ELSE IF( @Partsubtype_ImageActionTypeId = 2 )
					BEGIN
						-- удалить картинку
						UPDATE dbo.T_Partsubtype SET [Partsubtype_Image] = NULL, [Partsubtype_ImageFileFullName] = NULL
						WHERE [Partsubtype_Guid] = @Partsubtype_Guid;
					END
			END

	END TRY
	BEGIN CATCH
    SET @ERROR_NUM = ERROR_NUMBER();
    SET @ERROR_MES = ERROR_MESSAGE();
		RETURN @ERROR_NUM;
	END CATCH;

	SET @ERROR_NUM = 0;
	SET @ERROR_MES = 'Успешное завершение операции.';
	RETURN @ERROR_NUM;
END

GO

/****** Object:  StoredProcedure [dbo].[usp_EditPartSubType]    Script Date: 19.11.2013 12:18:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Изменяет свойства записи в таблице dbo.T_Partsubtype
--
-- Входящие параметры:
--		@Partsubtype_Guid - уникальный идентификатор записи
--		@Partsubtype_Name - наименование
--		@Partsubtype_Description - примечание
--		@Partsubtype_IsActive - признак активности
--		@Partline_Guid - уникальный идентификатор товарной линии
--		@Partsubtype_VendorTariff- тариф поставщика
--		@Partsubtype_TransportTariff - величина (пройент) расходов на транспорт
--		@Partsubtype_CustomsTariff - размер (процент) таможенной пошлины
--		@Partsubtype_Margin - маржа (процент)
--		@Partsubtype_NDS - ставка НДС (процент)
--		@Partsubtype_Discont - дисконт (средняя сложившаяся скидка)
--		@Partsubtype_MarkUpRequired		- требуемая наценка
--		@Partsubtype_Image						- изображение подгруппы в двоичном виде
--		@Partsubtype_ImageFileFullName - наименование файла с изображением
--		@Partsubtype_ImageActionTypeId - тип действия над изображением (1 - заменить картинку; 2 - удалить картинку)
--
-- Выходные параметры:
--  @ERROR_NUM - номер ошибки
--  @ERROR_MES - текст ошибки
--
-- Результат:
--    0 - Успешное завершение
--    <>0 - ошибка

ALTER PROCEDURE [dbo].[usp_EditPartSubType] 
  @Partsubtype_Guid D_GUID,
	@Partsubtype_Name D_NAME,
	@Partsubtype_Description D_DESCRIPTION = NULL,
	@Partsubtype_IsActive D_ISACTIVE,
	@Partline_Guid D_GUID,
	@Partsubtype_VendorTariff D_MONEY,
	@Partsubtype_TransportTariff D_MONEY,
	@Partsubtype_CustomsTariff D_MONEY,
	@Partsubtype_Margin D_MONEY,
	@Partsubtype_NDS D_MONEY,
	@Partsubtype_Discont D_MONEY,
	@PartsubtypeState_Guid D_GUID,
	@Partsubtype_MarkUpRequired	D_MONEY = NULL,

	@Partsubtype_Image image = NULL,
	@Partsubtype_ImageFileFullName nvarchar(256) = NULL,
	@Partsubtype_ImageActionTypeId int = NULL,

  @ERROR_NUM int output,
  @ERROR_MES nvarchar(4000) output

AS

BEGIN

	BEGIN TRY

    SET @ERROR_NUM = 0;
    SET @ERROR_MES = '';

    -- Проверяем наличие записи с заданным идентификатором
    IF NOT EXISTS ( SELECT * FROM dbo.T_Partsubtype WHERE Partsubtype_Guid = @Partsubtype_Guid )
      BEGIN
        SET @ERROR_NUM = 1;
        SET @ERROR_MES = 'В базе данных не найдена товарная подгруппа с указанным идентификатором.' + Char(13) + 
          'УИ: ' + Char(9) + CAST( @Partsubtype_Guid as nvarchar( 36 ) );
        RETURN @ERROR_NUM;
      END

    -- Проверяем наличие записи с заданным именем
    IF EXISTS ( SELECT * FROM dbo.T_Partsubtype WHERE Partsubtype_Name = @Partsubtype_Name AND Partsubtype_Guid <> @Partsubtype_Guid )
      BEGIN
        SET @ERROR_NUM = 2;
        SET @ERROR_MES = 'В базе данных уже присутствует товарная подгруппа с указанным именем.' + Char(13) + 
          'Имя: ' + Char(9) + @Partsubtype_Name;
        RETURN @ERROR_NUM;
      END

    -- Проверяем наличие товарной линии с указанным идентификатором
    IF NOT EXISTS ( SELECT * FROM dbo.T_Partline WHERE Partline_Guid = @Partline_Guid )
      BEGIN
        SET @ERROR_NUM = 3;
        SET @ERROR_MES = 'В базе данных не найдена товарная линия с указанным идентификатором.' + Char(13) + 
          'УИ: ' + Char(9) + CAST( @Partline_Guid as nvarchar( 36 ) );
        RETURN @ERROR_NUM;
      END

    -- Проверяем наличие состояния подгруппы с указанным идентификатором
    IF NOT EXISTS ( SELECT PartsubtypeState_Guid FROM dbo.T_PartsubtypeState WHERE PartsubtypeState_Guid = @PartsubtypeState_Guid )
      BEGIN
        SET @ERROR_NUM = 4;
        SET @ERROR_MES = 'В базе данных не найдено состояние подгруппы с указанным идентификатором.' + Char(13) + 
          'УИ: ' + Char(9) + CAST( @PartsubtypeState_Guid as nvarchar( 36 ) );
        RETURN @ERROR_NUM;
      END

    UPDATE dbo.T_Partsubtype SET Partsubtype_Name = @Partsubtype_Name, Partsubtype_Description = @Partsubtype_Description, 
			Partsubtype_IsActive = @Partsubtype_IsActive, Partsubtype_VendorTariff = @Partsubtype_VendorTariff, 
			Partsubtype_TransportTariff = @Partsubtype_TransportTariff, Partsubtype_CustomsTariff = @Partsubtype_CustomsTariff, 
			Partsubtype_Margin = @Partsubtype_Margin, Partsubtype_NDS = @Partsubtype_NDS, Partsubtype_Discont = @Partsubtype_Discont, 
			PartsubtypeState_Guid = @PartsubtypeState_Guid
		WHERE Partsubtype_Guid = @Partsubtype_Guid;

		IF( @Partsubtype_MarkUpRequired IS NOT NULL )
			UPDATE dbo.T_Partsubtype SET Partsubtype_MarkUpRequired = @Partsubtype_MarkUpRequired
			WHERE Partsubtype_Guid = @Partsubtype_Guid;
    
    IF NOT EXISTS ( SELECT PartsubtypePartline_Guid FROM dbo.T_PartsubtypePartline WHERE Partsubtype_Guid = @Partsubtype_Guid )
			INSERT INTO dbo.T_PartsubtypePartline( PartsubtypePartline_Guid, Partsubtype_Guid, Partline_Guid )
			VALUES( NEWID(), @Partsubtype_Guid, @Partline_Guid );
		ELSE
			UPDATE 	dbo.T_PartsubtypePartline SET Partline_Guid = @Partline_Guid WHERE Partsubtype_Guid = @Partsubtype_Guid;
    
		IF( @Partsubtype_ImageActionTypeId IS NOT NULL )
			BEGIN
				IF( ( @Partsubtype_ImageActionTypeId = 1 ) AND ( @Partsubtype_Image IS NOT NULL ) )
					BEGIN
						-- заменить картинку
						UPDATE dbo.T_Partsubtype SET [Partsubtype_Image] = @Partsubtype_Image, [Partsubtype_ImageFileFullName] = @Partsubtype_ImageFileFullName
						WHERE [Partsubtype_Guid] = @Partsubtype_Guid;
					END
				ELSE IF( @Partsubtype_ImageActionTypeId = 2 )
					BEGIN
						-- удалить картинку
						UPDATE dbo.T_Partsubtype SET [Partsubtype_Image] = NULL, [Partsubtype_ImageFileFullName] = NULL
						WHERE [Partsubtype_Guid] = @Partsubtype_Guid;
					END
			END

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

/****** Object:  StoredProcedure [dbo].[usp_GetPartSubType]    Script Date: 19.11.2013 13:04:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- Возвращает список записей из ( dbo.T_Partsubtype )
--
-- Входящие параметры:
--
-- Выходные параметры:
--
-- Результат:
--    0 - успешное завершение
--    <>0 - ошибка запроса информации из базы данных

ALTER PROCEDURE [dbo].[usp_GetPartSubType] 
  @ERROR_NUM int output,
  @ERROR_MES nvarchar(4000) output
AS

BEGIN

  SET @ERROR_NUM = 0;
  SET @ERROR_MES = '';

		CREATE TABLE #PartSubTypeStock( Stock_Guid uniqueidentifier, PartSybType_Guid uniqueidentifier, StockQty int );

		INSERT INTO #PartSubTypeStock( Stock_Guid, PartSybType_Guid, StockQty )
		SELECT dbo.GetStockGuidForStockId( STOCK_ID ), dbo.GetPartsubtypeGuidForPartsubtypeId( PARTSUBTYPE_ID ), QUANTITY 
		FROM [DB02].[ERP_Report].dbo.StockRestPartSubTypeView;
		
		CREATE TABLE #PartsubtypeInWarehouseForShipping( PartSybType_Guid uniqueidentifier, StockQty int );
		
		INSERT INTO #PartsubtypeInWarehouseForShipping( PartSybType_Guid, StockQty )
		
		SELECT  #PartSubTypeStock.PartSybType_Guid, SUM( #PartSubTypeStock.StockQty )
		FROM #PartSubTypeStock, dbo.T_Stock, dbo.T_Warehouse as Warehouse
		WHERE #PartSubTypeStock.Stock_Guid = dbo.T_Stock.Stock_Guid
			AND dbo.T_Stock.Warehouse_Guid = Warehouse.Warehouse_Guid
			AND Warehouse.Warehouse_IsForShipping = 1
		GROUP BY #PartSubTypeStock.PartSybType_Guid;	
		
		DROP TABLE 	#PartSubTypeStock;

		WITH Partsubtype AS
		(
			SELECT Partsubtype_Guid, Partsubtype_Id, Partsubtype_Name, Partsubtype_Description, Partsubtype_Image, Partsubtype_IsActive, 
				Partsubtype_VendorTariff, Partsubtype_TransportTariff, Partsubtype_CustomsTariff, Partsubtype_Margin, Partsubtype_NDS, Partsubtype_Discont,
				dbo.GetPartLineGuidForPartSubType( Partsubtype_Guid ) as PartLine_Guid,
				dbo.GetPartOwnerGuidForPartSubType( Partsubtype_Guid ) as PartOwner_Guid,
				dbo.GetPartTypeGuidForPartSubType( Partsubtype_Guid ) as PartType_Guid,
				PartsubtypeState_Guid, 
				dbo.GetIsContainingPartSubTypePartsActualNotValid( Partsubtype_Guid ) as ContainingPartsActualNotValid,
				dbo.GetIsContainingPartSubTypePartsNotValid( Partsubtype_Guid ) as ContainingPartsNotValid, 
				dbo.GetIsContainingPartSubTypePartsActual( Partsubtype_Guid ) as ContainingPartsActual, 
				Partsubtype_PriceEXW, 
				#PartsubtypeInWarehouseForShipping.StockQty, Partsubtype_MarkUpRequired, 
				dbo.T_Partsubtype.Partsubtype_ImageFileFullName,
				ExistImage = 
				CASE 
					 WHEN Partsubtype_Image IS NULL THEN 0
					 ELSE 1
				END
			FROM dbo.T_Partsubtype LEFT OUTER JOIN #PartsubtypeInWarehouseForShipping ON dbo.T_Partsubtype.Partsubtype_Guid = #PartsubtypeInWarehouseForShipping.PartSybType_Guid
		)
    SELECT Partsubtype.Partsubtype_Guid, Partsubtype.Partsubtype_Id, Partsubtype.Partsubtype_Name, Partsubtype.Partsubtype_Description, 
			Partsubtype.Partsubtype_Image, Partsubtype.Partsubtype_IsActive, 
			Partsubtype.Partsubtype_VendorTariff, Partsubtype.Partsubtype_TransportTariff, Partsubtype.Partsubtype_CustomsTariff, Partsubtype.Partsubtype_Margin, Partsubtype.Partsubtype_NDS, Partsubtype.Partsubtype_Discont,
			Partsubtype.ContainingPartsActualNotValid,
			Partsubtype.ContainingPartsNotValid,
			ContainingPartsActual,
			dbo.GetPartSubtypePriceEXW( Partsubtype.Partsubtype_Guid ) as PartSubtypePriceEXW,
			Partsubtype.PartLine_Guid, Partline.Partline_Id, Partline.Partline_Name, Partline.Partline_Description, Partline.Partline_IsActive,
			Partsubtype.PartOwner_Guid, PartsOwner.Owner_Id, PartsOwner.Owner_Name,
			Partsubtype.PartType_Guid, Parttype.Parttype_Id, Parttype.Parttype_Name, 
			Partsubtype.PartsubtypeState_Guid, PartsubtypeState.PartsubtypeState_Name, PartsubtypeState.PartsubtypeState_Description, PartsubtypeState.PartsubtypeState_IsActive,
			Partsubtype.StockQty AS StockQtyInWarehouseForShipping, Partsubtype.Partsubtype_MarkUpRequired, 
			Partsubtype.Partsubtype_ImageFileFullName, Partsubtype.ExistImage
    FROM Partsubtype LEFT OUTER JOIN dbo.T_Partline as Partline ON Partsubtype.PartLine_Guid = Partline.Partline_Guid 
      LEFT OUTER JOIN dbo.T_Owner as PartsOwner ON Partsubtype.PartOwner_Guid = PartsOwner.Owner_Guid 
      LEFT OUTER JOIN dbo.T_Parttype as Parttype ON Partsubtype.PartType_Guid = Parttype.Parttype_Guid 
      LEFT OUTER JOIN dbo.T_PartsubtypeState as PartsubtypeState ON Partsubtype.PartsubtypeState_Guid = PartsubtypeState.PartsubtypeState_Guid
    --ORDER BY PartsOwner.Owner_Name, Parttype.Parttype_Name, Partsubtype.Partsubtype_Name;
    ORDER BY Partsubtype.Partsubtype_Name;

		DROP TABLE 	#PartsubtypeInWarehouseForShipping;
	--END TRY
	--BEGIN CATCH
	--	SET @ERROR_NUM = ERROR_NUMBER();
	--	SET @ERROR_MES = ERROR_MESSAGE();
	--	RETURN @ERROR_NUM;
	--END CATCH;

  IF( @ERROR_NUM = 0 )
		SET @ERROR_MES = 'Успешное завершение операции.';

  RETURN @ERROR_NUM;
END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Назначает товарной подгруппе изображение
--
-- Входящие параметры:
--		@Partsubtype_Guid								- уникальный идентификатор записи
--		@Partsubtype_Image							- изображение подгруппы в двоичном виде
--		@Partsubtype_ImageFileFullName	- наименование файла с изображением
--		@Partsubtype_ImageActionTypeId	- тип действия над изображением (1 - заменить картинку; 2 - удалить картинку)
--
-- Выходные параметры:
--  @ERROR_NUM - номер ошибки
--  @ERROR_MES - текст ошибки
--
-- Результат:
--    0 - Успешное завершение
--    <>0 - ошибка

CREATE PROCEDURE [dbo].[usp_SetPartSubTypeImage] 
  @Partsubtype_Guid D_GUID,

	@Partsubtype_Image image = NULL,
	@Partsubtype_ImageFileFullName nvarchar(256) = NULL,
	@Partsubtype_ImageActionTypeId int,

  @ERROR_NUM int output,
  @ERROR_MES nvarchar(4000) output

AS

BEGIN

	BEGIN TRY

    SET @ERROR_NUM = 0;
    SET @ERROR_MES = '';

    -- Проверяем наличие записи с заданным идентификатором
    IF NOT EXISTS ( SELECT * FROM dbo.T_Partsubtype WHERE Partsubtype_Guid = @Partsubtype_Guid )
      BEGIN
        SET @ERROR_NUM = 1;
        SET @ERROR_MES = 'В базе данных не найдена товарная подгруппа с указанным идентификатором.' + Char(13) + 
          'УИ: ' + Char(9) + CAST( @Partsubtype_Guid as nvarchar( 36 ) );
        RETURN @ERROR_NUM;
      END

		IF( @Partsubtype_ImageActionTypeId IS NOT NULL )
			BEGIN
				IF( ( @Partsubtype_ImageActionTypeId = 1 ) AND ( @Partsubtype_Image IS NOT NULL ) )
					BEGIN
						-- заменить картинку
						UPDATE dbo.T_Partsubtype SET [Partsubtype_Image] = @Partsubtype_Image, [Partsubtype_ImageFileFullName] = @Partsubtype_ImageFileFullName
						WHERE [Partsubtype_Guid] = @Partsubtype_Guid;
					END
				ELSE IF( @Partsubtype_ImageActionTypeId = 2 )
					BEGIN
						-- удалить картинку
						UPDATE dbo.T_Partsubtype SET [Partsubtype_Image] = NULL, [Partsubtype_ImageFileFullName] = NULL
						WHERE [Partsubtype_Guid] = @Partsubtype_Guid;
					END
			END

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
GRANT EXECUTE ON [dbo].[usp_SetPartSubTypeImage] TO [public]
GO

DECLARE	@return_value int

EXEC	@return_value = [dbo].[AddRights]
		@strName = N'Редактировать только изображения товарных подгрупп',
		@strDescription = N'Редактировать только изображения товарных подгрупп',
		@strRole = N'Редактировать только изображения товарных подгрупп'

SELECT	'Return Value' = @return_value

GO
