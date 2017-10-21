USE [TROBADOO]
GO
/****** Object:  StoredProcedure [dbo].[WEB_GET_PRODUCTS]    Script Date: 10/21/2017 09:17:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		David ARTIERI
-- Create date: 11/06/2017
-- Description:	Recupera informacion articulos para la web
-- Ejemplo: EXEC WEB_GET_PRODUCTS
-- =============================================
ALTER PROCEDURE [dbo].[WEB_GET_PRODUCTS]
	-- Add the parameters for the stored procedure here
	--@codProveedor as int = null
AS
BEGIN
	SET NOCOUNT ON;
	set dateformat dmy
	--Declaramos las variables
	/*DECLARE @antiguoPrecio decimal(10,2)
	DECLARE @precioInicial decimal(10,2)
	DECLARE @nuevoPrecio decimal(10,2)
	DECLARE @antiguoTotal decimal(10,2)
	DECLARE @fechaDepo datetime
	DECLARE @numDepo int*/

	BEGIN TRY
	
		select c.Fecha as depositCreationDate,
			   cast(convert(float, RTRIM(LTRIM(f.codfam))) as numeric(10,2)) as familyCode,
			   f.descfam as familyDescription,
			   a.codart as code,
			   a.descart as description,
			   a.fecalta as creationDate,
			   a.param8,
			   case when nullif(ltrim(rtrim(a.param8)),'') is null then null
								else CONVERT(datetime,a.param8, 120) end as lastModificationDate,
			   a.param9,
			   case when nullif(ltrim(rtrim(a.param9)),'') is null then null
								else CONVERT(datetime,a.param9, 120) end as formerModificationDate,
			   cast(CONVERT(float,a.PARAM7) as numeric(10,2)) as initialPrice,
			   cast(CONVERT(float,a.prcventa) as numeric(10,2)) as sellPrice,
			   a.unidadesstock as stock,
			   a.texto as observations
		FROM __CabeDepC c (NOLOCK)
		inner join __LineDepo l (NOLOCK) ON l.IdDepC = c.IdDepC
		inner join articulo a(NOLOCK) ON l.codArt = a.codArt
		inner join familias f (NOLOCK) on f.codfam = a.codfamest
		where a.unidadesstock>=1
		AND a.prccompra > 0
		and not exists (select 1 from LineFact WITH(NOLOCK) where LineFact.codart = a.codart)
		order by familyCode, code;
		
	END TRY
	BEGIN CATCH
			BEGIN
				ROLLBACK TRAN --RollBack in case of Error
				PRINT 'Error: ' + ERROR_MESSAGE()
			END
	END CATCH
END