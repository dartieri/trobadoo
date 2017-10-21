using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace trobadoo.com.web.Helpers
{
    public class CssManager
    {
     private string pageName;
    private List< sCSS> lCss;
    private DateTime lastModifiedDate;
    private string cdnBasePath;
    private string cdnDomain;
    private string sharedPath;
    private comprimir As Boolean
    private minutosCache As Integer = 15

    'Para el calculo del hashcode para la cache
    private strHashCode As String
    private hashCodeCache As String

    private class sCSS{
        public string path;
        public DateTime lastModifiedDate;
        //public IEversion ieVersion;
        }

    public Sub new(ByVal nombrePagina As String, ByVal aplicacion As String, ByVal dominioCDN As String)
        Me.nombrePagina = nombrePagina
        Me.aplicacion = aplicacion
        Me.lCSS = new List< sCSS>();
        Me.rutaShared = AppSettings["pathContenidosShared"]
        Me.rutabaseCDN = AppSettings["pathCDN"]
        Me.dominioCDN = dominioCDN
        Me.strHashCode = ""
        If Not Boolean.TryParse(AppSettings["comprimirCSS"], Me.comprimir) Then
            Me.comprimir = False
        }
        If Me.rutabaseCDN = "" Then
            Me.comprimir = False
        }
    }

    'Carga CSS en la lista con la ruta normalizada y eliminado texto no necesario.
    'También se concatena el nombre para poder calcular el hashcode para la cache.
    public Sub addCss(ByVal rutaCSS As String)
        Me.addCss(rutaCSS, IEVersion.None)
    }

    public Sub addCss(ByVal rutaCSS As String, ByVal ieVersion As IEVersion)
        Dim css As sCSS
        Try
            css.ruta = normalizarRuta(rutaCSS)
            css.fModificacion = Date.MinValue
            css.ieVersion = ieVersion

            If css.ruta.EndsWith(".css") AndAlso Not lCSS.Contains(css) Then
                lCSS.Add(css)
                'String para obtener un hashcode para la cache
                strHashCode &= css.ruta.Replace("/", "").Replace(".css", "")
            }
        } catch(Exception ex) {
        }
    }

    ''' <summary>
    ''' Recupera el código HTML con los includes de los CSS
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    public Function getIncludeCSS() As String
        Return getIncludeCSS(False)
    }

    ''' <summary>
    ''' Recupera los CSS a incluir, en codigo HTML o separados por #
    ''' </summary>
    ''' <param name="getOnlyCssSources"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    public Function getIncludeCSS(ByVal getOnlyCssSources As Boolean) As String
        Dim cssFinal As new System.Text.StringBuilder
        Dim cssOut As new System.Text.StringBuilder
        Dim rutaCSSComprimido As String = "", nomCSSComprimido As String = ""
        Dim objStreamWriter As StreamWriter = Nothing
        Dim errorCompresion As Boolean = False
        Try
            'Obtenemos los CSS a comprimir
            lCSS = getFechasModificacion()

            'Verificamos si hay CSS
            If lCSS.Count > 0 Then

                'Comprimimos
                If Me.comprimir Then
                    rutaCSSComprimido = "/css/" & Me.nombrePagina & "/" & Me.aplicacion & "/"
                    rutaCSSComprimido.Replace("//", "/")
                    nomCSSComprimido = getNomCSS()

                    'Si no existe, generamos el archivo
                    If Not File.Exists(rutaShared & rutaCSSComprimido & nomCSSComprimido) Then
                        Static Dim objSincro As new Object
                        SyncLock objSincro
                            Try
                                If Not File.Exists(rutaShared & rutaCSSComprimido & nomCSSComprimido) Then

                                    'Comprimimos
                                    For Each css As sCSS In lCSS
                                        Dim textoCSSAComprimir As String = File.ReadAllText(rutabaseCDN & "/" & css.ruta, System.Text.Encoding.GetEncoding(28605))
                                        If Not String.IsNullOrEmpty(textoCSSAComprimir) Then
                                            Dim textoCSSComprimido As String = CssCompressor.Compress(textoCSSAComprimir)
                                            If Not String.IsNullOrEmpty(textoCSSComprimido) Then
                                                cssFinal.AppendLine(textoCSSComprimido)
                                            Else
                                                errorCompresion = True
                                                Exit For
                                            }
                                        }
                                    }

                                    'Guardamos si todo ok
                                    If Not errorCompresion Then
                                        Dim cssComprimido = cssFinal.ToString

                                        'Creamos directorio
                                        If Not Directory.Exists(rutaShared & rutaCSSComprimido) Then
                                            Directory.CreateDirectory(rutaShared & rutaCSSComprimido)
                                        }

                                        objStreamWriter = new StreamWriter(File.Create(rutaShared & rutaCSSComprimido & nomCSSComprimido), System.Text.Encoding.GetEncoding(28605))
                                        objStreamWriter.Write(cssComprimido)
                                        objStreamWriter.Close()
                                    }
                                }

                            } catch(Exception ex) {
                                'Borramos archivo corrupto y marcamos error
                                If File.Exists(rutaShared & rutaCSSComprimido & nomCSSComprimido) Then
                                    File.Delete(rutaShared & rutaCSSComprimido & nomCSSComprimido)
                                }
                                errorCompresion = True
                            }
                        End SyncLock
                    }
                }

                If (Not Me.comprimir) OrElse (errorCompresion) Then
                    'Devolvemos CSS sin comprimir
                    For Each css As sCSS In lCSS
                        Dim fecha As Date = css.fModificacion
                        Dim src As String = dominioCDN & "/" & css.ruta.Replace("\", "/") & "?" & fecha.ToString("ddMMyyyyHHmm")
                        If getOnlyCssSources Then
                            cssOut.AppendLine(src)
                        Else
                            Dim cssLink As String = String.Format("<link href=""{0}"" rel=""stylesheet"" type=""text/css"" />", src)
                            Select Case css.ieVersion
                                Case IEVersion.IE7
                                    cssOut.AppendLine(String.Format("<!--[if lt IE 7]>{0}<![endif]-->", cssLink))
                                Case IEVersion.IE8
                                    cssOut.AppendLine(String.Format("<!--[if lt IE 8]>{0}<![endif]-->", cssLink))
                                Case IEVersion.IE9
                                    cssOut.AppendLine(String.Format("<!--[if lt IE 9]>{0}<![endif]-->", cssLink))
                                Case IEVersion.IE10
                                    cssOut.AppendLine(String.Format("<!--[if lt IE 10]>{0}<![endif]-->", cssLink))
                                Case Else
                                    cssOut.AppendLine(cssLink)
                            End Select
                        }
                    }
                Else
                    'Devolvemos un CSS comprimido
                    If getOnlyCssSources Then
                        cssOut.AppendLine(dominioCDN & "/contenidosShared" & rutaCSSComprimido.Replace("\", "/") & nomCSSComprimido)
                    Else
                        cssOut.AppendLine("<link href=""" & dominioCDN & "/contenidosShared" & rutaCSSComprimido.Replace("\", "/") & nomCSSComprimido & """ rel=""stylesheet"" type=""text/css"" />")
                    }
                }
            }

        Finally
            If Not objStreamWriter Is Nothing Then
                objStreamWriter.Close()
            }
        }

        Return cssOut.ToString
    }

    'Obtiene el nombre del CSS comprimido
    private Function getNomCSS() As String
        Dim concatRutas As new System.Text.StringBuilder
        Dim concatNombres As String
        For Each css As sCSS In lCSS
            concatRutas.Append(css.ruta.Substring((css.ruta.LastIndexOf("/") + 1)))
        }
        concatNombres = concatRutas.ToString.Replace(".css", "")
        Return Math.Abs(concatNombres.GetHashCode()) & "_" & fUltimaModificacion.ToString("ddMMyyyyHHmm") & ".css"
    }

    'normaliza la ruta del CSS
    private Function normalizarRuta(ByVal rutaCSS As String) As String
        Dim rutaNormalizada As String
        rutaNormalizada = rutaCSS.Replace("\", "/")
        If rutaNormalizada.StartsWith("/") Then
            rutaNormalizada = rutaNormalizada.Substring(1)
        }
        If rutaNormalizada.Contains("?") Then
            rutaNormalizada = rutaNormalizada.Remove(rutaNormalizada.LastIndexOf("?"))
        }
        Return rutaNormalizada
    }

    'Obtiene las fechas de modificacion
    private Function getFechasModificacion() As List< sCSS)
        Dim tmplCSS As new List< sCSS)
        hashCodeCache = Math.Abs(strHashCode.GetHashCode) & "_" & Me.aplicacion & "_" & Me.nombrePagina

        If Not System.Web.HttpContext.Current Is Nothing _
            AndAlso AppSettings["modo") <> "desarrollo" _
            AndAlso Not Current.Cache.Item(hashCodeCache) Is Nothing Then

            'Obtiene de cache la lista de CSS con fechas de modificacion.
            tmplCSS = Current.Cache.Item(hashCodeCache)
            For Each css As sCSS In tmplCSS
                If css.fModificacion > Me.fUltimaModificacion Then
                    Me.fUltimaModificacion = css.fModificacion
                }
            }

        Else
            For Each css As sCSS In lCSS
                Dim rutaOriginal As String = rutabaseCDN & "/" & css.ruta
                If File.Exists(rutaOriginal) Then
                    css.fModificacion = FileSystem.FileDateTime(rutaOriginal)
                    If css.fModificacion > Me.fUltimaModificacion Then
                        Me.fUltimaModificacion = css.fModificacion
                    }
                    tmplCSS.Add(css)
                ElseIf Not Me.comprimir Then
                    css.fModificacion = Date.Now
                    tmplCSS.Add(css)
                }
            }

            If Not System.Web.HttpContext.Current Is Nothing _
                AndAlso AppSettings["modo") <> "desarrollo" _
                AndAlso Current.Cache.Item(hashCodeCache) Is Nothing Then

                Current.Cache.Add(hashCodeCache, tmplCSS, Nothing, Date.Now.AddMinutes(minutosCache), Web.Caching.Cache.NoSlidingExpiration, Web.Caching.CacheItemPriority.Normal, Nothing)
            }
        }

        Return tmplCSS
    }
End Class

    }
}