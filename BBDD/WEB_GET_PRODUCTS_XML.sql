USE [TROBADOO]
GO
/****** Object:  StoredProcedure [dbo].[WEB_GET_PRODUCTS_XML]    Script Date: 10/21/2017 09:17:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		David ARTIERI
-- Create date: 01/10/2017
-- Description:	Recupera informacion articulos para la web
-- Ejemplo: EXEC WEB_GET_PRODUCTS_XML
-- =============================================
ALTER PROCEDURE [dbo].[WEB_GET_PRODUCTS_XML]
	-- Add the parameters for the stored procedure here
	--@codProveedor as int = null
AS
BEGIN
	SET NOCOUNT ON;
	set dateformat dmy
	--Declaramos las variables

	BEGIN TRY
	
		select	a.fecalta as "@creationDate",
				case when nullif(ltrim(rtrim(a.param8)),'') is null then null
								else CONVERT(datetime,a.param8, 120) end as "@lastModificationDate",
				case when nullif(ltrim(rtrim(a.param9)),'') is null then null
								else CONVERT(datetime,a.param9, 120) end as "@formerModificationDate",
				c.Fecha as depositCreationDate,
			   RTRIM(LTRIM(a.codart)) as "Code",
			   a.descart as "Description",
			   cast(convert(int, RTRIM(LTRIM(a.UNIDADESSTOCK))) as numeric(10,0)) as "Stock",
			   cast(convert(float, RTRIM(LTRIM(f.codfam))) as numeric(10,2)) as "Family/Code",
			   f.descfam as "Family/Description",
			   cast(convert(float, RTRIM(LTRIM(a.param7))) as numeric(10,2)) as "Price/Initial",
			   cast(convert(float, RTRIM(LTRIM(a.prcventa))) as numeric(10,2)) as "Price/Sell",
			   a.texto as "Observations"
		FROM __CabeDepC c (NOLOCK)
		inner join __LineDepo l (NOLOCK) ON l.IdDepC = c.IdDepC
		inner join articulo a(NOLOCK) ON l.codArt = a.codArt
		inner join familias f (NOLOCK) on f.codfam = a.codfamest
		where a.unidadesstock>=1
		AND a.prccompra > 0
		and not exists (select 1 from LineFact WITH(NOLOCK) where LineFact.codart = a.codart)
		order by convert(float, RTRIM(LTRIM(f.codfam))), code
		FOR XML PATH ('Product'), ROOT('Products');
		
		/*SELECT 
   CustomerID as "@CustomerID",
   CompanyName,
   Address as "address/street",
   City as "address/city",
   Region as "address/region",
   PostalCode as "address/zip",
   Country as "address/country",
   ContactName as "contact/name",
   ContactTitle as "contact/title",
   Phone as "contact/phone", 
   Fax as "contact/fax"
FROM Customers
FOR XML PATH('Customer')*/
	END TRY
	BEGIN CATCH
			BEGIN
				PRINT 'Error: ' + ERROR_MESSAGE()
			END
	END CATCH
END