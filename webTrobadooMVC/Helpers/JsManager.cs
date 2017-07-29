using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.IO;

namespace trobadoo.com.web.Helpers
{
    public class JsManager
    {
        private List<sJs> lJs ;
    private DateTime lastModifiedDate;
    private string siteBasePath;
    private string cdnBasePath;
    private string sharedPath;
    private string cdnDomain;
    private bool compress;
    private int cacheMinutes = 15;
    //Para el calculo del hashcode para la cache
    private string hashCodeCache;
    private string strHashCode;

    private class sJs
    {
        public string path;
        public bool compress;
        public bool external;
        public bool addCdn; //Añadir url cdn si no comprime
        public DateTime lastModifiedDate;
        public string basePath;
        public bool included; //Para saber si ya se imprimido este Js en la página
        public int zIndex; //Para ordenar los elementos de la lista
        public IEVersion ieVersion;
        }

    public JsManager(string cdnDomain){
       init(cdnDomain, ConfigurationManager.AppSettings["pathAplicacion"]);
    }

    public JsManager(string cdnDomain, string physicalPath){
        init(cdnDomain,physicalPath);
    }

        private void init(string cdnDomain, string physicalPath){
            sharedPath = ConfigurationManager.AppSettings["pathContenidosShared"];
        cdnDomain = cdnDomain;
        siteBasePath = physicalPath + "/";
        cdnBasePath = ConfigurationManager.AppSettings["pathCDN"] + "/";
        lJs = new List<sJs>();
        if( ! bool.TryParse(ConfigurationManager.AppSettings["compressJS"], compress) ){
            compress = false;
        }
        if( siteBasePath == "/" ){
            compress = false;
        }
        }

    public void addJS(string rutaJs, bool habilitarCompresion = false){
        addJS(rutaJs, false, habilitarCompresion);
    }

    public void addJS(string rutaJs, bool addCdn, bool habilitarCompresion){
        int indice = getMaxIndice();
        addJS(rutaJs, addCdn, habilitarCompresion, indice)
    }

    public void addJS(string rutaJs, bool addCdn, bool habilitarCompresion, IEVersion ieVersion){
        int indice = getMaxIndice();
        addJS(rutaJs, addCdn, habilitarCompresion, indice, ieVersion);
    }

    public void addJS(string rutaJs, bool addCdn, bool habilitarCompresion, int indice){
        addJS(rutaJs, addCdn, habilitarCompresion, indice, IEVersion.None);
    }

    public void addJS(string rutaJs, bool addCdn, bool habilitarCompresion, int indice, IEVersion ieVersion){
        var repetido  = false;
        foreach (var oJS in lJs){
            if(normalizarRuta(rutaJs).Contains(oJS.path) ){
                repetido = true;
            }
        }
        if(!repetido ){
            var Js = new sJs();
            if( rutaJs.Contains("http://") || rutaJs.Contains("https://")){
                Js.path = rutaJs;
                Js.compress = false;
                Js.external = true;
                Js.addCdn = addCdn;
                Js.included = false;
                Js.zIndex = indice;
                Js.ieVersion = IEVersion;
                lJs.Add(Js);
            } else {
                Js.path = normalizarRuta(rutaJs);
                Js.compress = compress? habilitarCompresion: false;
                Js.external = false;
                Js.addCdn = addCdn;
                Js.included = false;
                Js.zIndex = indice;
                Js.ieVersion = ieVersion;
                if( Js.path.EndsWith(".js") && !lJs.Contains(Js) ){
                    lJs.Add(Js);
                    strHashCode += Js.path.Replace("/", "").Replace(".js", "")
                }
            }
        }
    }

    public string getIncludeJS(){
        return getIncludeJs(false);
    }

    public string getIncludeJs(bool getOnlyJsSources) {
        lJs.Sort(x, y=> x.zIndex.CompareTo(y.zIndex));
        var sInclude = new StringBuilder();
        if( lJs.Count > 0 ){
            lJs = getFechasModificacion();
            //Se comprueba que tenga js cargados
            var lJs2 = new List<sJs>();
            foreach (var js in lJs){
                if(!js.included ){
                    if(js.compress && !js.external ){
                        js = CompressJs(js);
                    }
                    sInclude.AppendLine(strIncludeJs(js, getOnlyJsSources));
                    js.included = true;
                    lJs2.Add(js);
                }
            }
            lJs = lJs2;
            return sInclude.ToString();
        } else {
            return string.Empty;
        }
    }

    private string strIncludeJs(sJs Js, bool getOnlyJsSources) {
        var src  = string.Empty;
        var script = string.Empty;

        if( Js.compress ){
            src = string.Format("{0}/contenidosShared{1}?{2}",cdnDomain,Js.path,Js.lastModifiedDate.ToString("ddMMyyyyHHmm"));
            if( ! getOnlyJsSources ){
                script =string.Format("<script type=\"text/javascript\" language=\"javascript\" src=\"{0}\"></script>",src);
            }
        } else {
            if( Js.external ){
            src = Js.path;
            if( ! getOnlyJsSources ){
                script = string.Format("<script type=\"text/javascript\" language=\"javascript\" src=\"{0}\"></script>",Js.path);
            }
        } else {
            if( !string.IsNullOrEmpty(Js.basePath) && Js.basePath == siteBasePath ){
                src = string.Format("/{0}?" ,Js.path  ,Js.lastModifiedDate.ToString("ddMMyyyyHHmm");
            } else {
                src = string.Format("{0}/{1}?{2}",Js.addCdn ? cdnDomain: string.Empty , Js.path ,Js.lastModifiedDate.ToString("ddMMyyyyHHmm"));
            }

            if( ! getOnlyJsSources ){
                script = string.Format("<script type=\"text/javascript\" language=\"javascript\" src=\"{0}\"></script>",src);
            }
        }

        if( getOnlyJsSources ){
            return src;
        } else {
            switch(Js.ieVersion){
                case IEVersion.IE7:
                    return string.Format(string.Format("<!--[if( lt IE 7]>{0}<![endif(]-->", script))
                case IEVersion.IE8:
                    return string.Format(string.Format("<!--[if( lt IE 8]>{0}<![endif(]-->", script))
                case IEVersion.IE9:
                    return string.Format(string.Format("<!--[if( lt IE 9]>{0}<![endif(]-->", script))
                case IEVersion.IE10:
                    return string.Format(string.Format("<!--[if( lt IE 10]>{0}<![endif(]-->", script))
                default:
                    return script;
            }
        }
    }
        return string.Empty;
    }

    private sJs CompressJs(sJs Js){
    //    StreamWriter objStreamWriter = null;
    //    string tmpRutaFichero;
    //    string jsComprimido;
    //    string jsString ;

    //    tmpRutaFichero = Js.path;
    //    Js.path = string.Format("/Js/{0}" ,Js.path.Replace("\\", "/");

    //    if( ! File.Exists(sharedPath + Js.path) || FileSystem.FileDateTime(sharedPath + Js.path) < Js.lastModifiedDate ){
    //        //Garantizamos exclusión mutua con concurrencia
    //        /*static*/ var objSincro = new Object
    //        //SyncLock objSincro
    //            try{
    //                //Segunda comprobación tras exclusión mutua
    //                if( ! File.Exists(sharedPath + Js.path) || FileSystem.FileDateTime(sharedPath + Js.path) < Js.lastModifiedDate ){
    //                    jsString = File.ReadAllText(siteBasePath + tmpRutaFichero, System.Text.Encoding.GetEncoding("iso-8859-1")).ToString()
    //                    if(!string.IsNullOrEmpty(jsString)){
    //                        var oCulture = new System.Globalization.CultureInfo("es-ES");
    //                        jsComprimido = JavaScriptCompressor.Compress(jsString, false, false, false, false, 0, System.Text.Encoding.GetEncoding("utf-8"), oCulture);
    //                        if(!string.IsNullOrEmpty(jsComprimido)){
    //                            if( ! Directory.Exists(sharedPath + Js.path.Remove(Js.path.LastIndexOf("/"))) ){
    //                                Directory.CreateDirectory(sharedPath + Js.path.Remove(Js.path.LastIndexOf("/")));
    //                            }
    //                            objStreamWriter = new StreamWriter(File.Create(sharedPath + Js.path), System.Text.Encoding.GetEncoding(28605));
    //                            objStreamWriter.Write(jsComprimido);
    //                            objStreamWriter.Close();
    //                        } else {
    //                            Js.path = tmpRutaFichero
    //                            Js.compress = false
    //                        }
    //                    } else {
    //                        Js.path = tmpRutaFichero
    //                        Js.compress = false
    //                    }
    //                }
    //                } catch (Exception ex){
    //                //Si no ha ido correctamente borramos el fichero si existe y no lo comprimimos
    //                if( File.Exists(sharedPath + Js.path) ){
    //                    if(objStreamWriter != null ){
    //                        objStreamWriter.Close();
    //                        File.Delete(sharedPath + Js.path);
    //                    }
    //                }
    //                Js.compress = false;
    //                Js.path = tmpRutaFichero;
    //                }
    //        //End SyncLock
    //    }
        return Js;
    }

    private string normalizarRuta(string rutaCSS) {
        string rutaNormalizada;
        rutaNormalizada = rutaCSS.Replace("\\", "\/");
        if( rutaNormalizada.StartsWith("/") ){
            rutaNormalizada = rutaNormalizada.Substring(1);
        }
        if( rutaNormalizada.Contains("?") ){
            rutaNormalizada.Remove(rutaNormalizada.LastIndexOf("?"));
        }
        return rutaNormalizada;
    }

    //private List<sJs> getFechasModificacion() {
    //    var hashCodeCache = Math.Abs(strHashCode.GetHashCode);
    //    var tmplJs = new List<sJs>();
    //    if( ! System.Web.HttpContext.Current I== null && AppSettings["modo") <> "desarrollo" && ! Current.Cache.Item(hashCodeCache) == null ){
    //        tmplJs = Current.Cache.Item(hashCodeCache);
    //    } else {
    //        For Each Js As sJS In lJs
    //            if( Not Js.external ){
    //                if( File.Exists(cdnBasePath & Js.path) ){
    //                    Js.fModif(icacion = FileSystem.FileDateTime(cdnBasePath & Js.path)
    //                    Js.basePath = cdnBasePath
    //                    tmplJs.Add(Js)
    //                } else {if( File.Exists(siteBasePath & Js.path) ){
    //                    Js.fModif(icacion = FileSystem.FileDateTime(siteBasePath & Js.path)
    //                    Js.basePath = siteBasePath
    //                    tmplJs.Add(Js)
    //                } else {if( Not compress ){
    //                    Js.fModif(icacion = Date.Now
    //                    tmplJs.Add(Js)
    //                }
    //            } else {
    //                tmplJs.Add(Js)
    //            }
    //        }
    //        if( Not System.Web.HttpContext.Current Is Nothing ){
    //            Current.Cache.Add(hashCodeCache, tmplJs, Nothing, Date.Now.AddMinutes(cacheMinutes), Web.Caching.Cache.NoSlidingExpiration, Web.Caching.CacheItemPriority.Normal, Nothing)
    //        }
    //    }
    //    return tmplJs
    //}

    private int getMaxIndice(){
        var maxIndice = -1;
        if( lJs != null  && lJs.Count > 0 ){
            foreach(var js in lJs){
                if( js.zIndex > maxIndice ){
                    maxIndice = js.zIndex;
                }
            }
        } else {
            return 100;
        }
        return maxIndice + 1;
    }
    }
}