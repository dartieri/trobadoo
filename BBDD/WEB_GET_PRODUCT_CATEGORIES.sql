USE [TROBADOO]
GO
/****** Object:  StoredProcedure [dbo].[WEB_GET_PRODUCT_CATEGORIES]    Script Date: 10/21/2017 09:17:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		David ARTIERI
-- Create date: 11/06/2017
-- Description:	Recupera informacion articulos para la web
-- Ejemplo: EXEC WEB_GET_PRODUCT_CATEGORIES
-- =============================================
ALTER PROCEDURE [dbo].[WEB_GET_PRODUCT_CATEGORIES]
	-- Add the parameters for the stored procedure here
	--@codProveedor as int = null
AS
BEGIN
	SET NOCOUNT ON;
	set dateformat dmy
	--Declaramos las variables

	BEGIN TRY
		select convert(float, RTRIM(LTRIM(f.codfam))) as code,
			   f.descfam as description,
			   COUNT(a.codart) as numProducts			   
		FROM __CabeDepC c (NOLOCK)
		inner join __LineDepo l (NOLOCK) ON l.IdDepC = c.IdDepC
		inner join articulo a(NOLOCK) ON l.codArt = a.codArt
		inner join familias f on f.codfam = a.codfamest
		where a.unidadesstock>=1
		AND a.prccompra > 0
		and not exists (select 1 from LineFact WITH(NOLOCK) where LineFact.codart = a.codart)
		group by convert(float, RTRIM(LTRIM(f.codfam))),f.descfam
		order by code;
	END TRY
	BEGIN CATCH
			BEGIN
				PRINT 'Error: ' + ERROR_MESSAGE()
			END
	END CATCH
END