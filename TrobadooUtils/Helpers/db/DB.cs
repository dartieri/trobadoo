using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Data.SqlClient;
using System.Xml.Linq;
using System.Xml;

namespace trobadoo.com.utils.Helpers.db
{

    public class DB
    {

        #region "Singleton"

        private static DB _instance = new DB();
        public static DB SingleInstance
        {
            get { return _instance; }
        }

        private DB()
        {
        }

        #endregion

        #region "private functions"

        private SqlCommand GetCommand(string strConexion, string ProcedureName, SqlParametersList @params, CommandType tipo = CommandType.StoredProcedure)
        {

            try
            {
                SqlCommand mySqlCommand = new SqlCommand();
                mySqlCommand.Connection = new SqlConnection(strConexion);
                mySqlCommand.CommandType = tipo;
                mySqlCommand.CommandText = ProcedureName;
                mySqlCommand.CommandTimeout = mySqlCommand.Connection.ConnectionTimeout;

                if ((@params != null))
                {
                    foreach (SqlParameter sqlParm in @params.ListaParametros)
                    {
                        mySqlCommand.Parameters.Add(sqlParm);
                    }
                }
                return mySqlCommand;
            }
            catch (Exception ex)
            {
                string paramsAux = "";
                if (@params != null)
                {
                    //paramsAux = @params.toString();
                }
                throw new Exception( "GetCommand ERROR: No se ha podido crear el comando de ejecucion." + strConexion + ". " + ProcedureName + ". " + paramsAux + ". " + tipo.ToString() + ". " + ex.Message,ex);
            }
        }
        #endregion

        //Esta funcion devuelve la primera columna de la la primera fila. Las demás filas y columnas serán descartadas. 
        //Ejemplo para usar este método es un count(X) 
        public object execProcedureEscalar(string strConexion, string ProcedureName, SqlParametersList @params, CommandType tipo = CommandType.StoredProcedure)
        {

            SqlCommand mySqlCommand = GetCommand(strConexion, ProcedureName, @params, tipo);
            object ret = null;
            try
            {
                mySqlCommand.Connection.Open();
                ret = mySqlCommand.ExecuteScalar();
            }
            catch (Exception e)
            {
                throw this.throwException(e, strConexion, ProcedureName, @params, tipo);
            }
            finally
            {
                close(mySqlCommand);
                mySqlCommand.Parameters.Clear();
            }
            return ret;
        }

        public System.Xml.Linq.XDocument execProcedureXml(string strConexion, string ProcedureName, SqlParametersList @params, CommandType tipo = CommandType.StoredProcedure)
        {
            SqlCommand mySqlCommand = GetCommand(strConexion, ProcedureName, @params, tipo);
            XmlReader ret = default(XmlReader);
            XDocument xDoc = default(XDocument);


            try
            {
                mySqlCommand.Connection.Open();
                ret = mySqlCommand.ExecuteXmlReader();
                xDoc = XDocument.Load(ret);

            }
            catch (Exception e)
            {
                throw this.throwException(e, strConexion, ProcedureName, @params, tipo);
            }
            finally
            {
                close(mySqlCommand);
                mySqlCommand.Parameters.Clear();
            }
            return xDoc;
        }

        public DataTable execProcedureData(string strConexion, string ProcedureName, SqlParametersList @params, CommandType tipo = CommandType.StoredProcedure)
        {
            SqlCommand mySqlCommand = GetCommand(strConexion, ProcedureName, @params, tipo);
            DataSet myDataSet = new DataSet();
            DataTable myDataTable = null;


            try
            {
                mySqlCommand.Connection.Open();
                SqlDataAdapter mySqlDataAdapter = new SqlDataAdapter(mySqlCommand);
                mySqlDataAdapter.Fill(myDataSet);
                if (myDataSet.Tables.Count > 0)
                {
                    myDataTable = myDataSet.Tables[0];
                }
            }
            catch (Exception e)
            {
                throw this.throwException(e, strConexion, ProcedureName, @params, tipo);
            }
            finally
            {
                close(mySqlCommand);
                mySqlCommand.Parameters.Clear();
            }
            return myDataTable;
        }

        /// <summary>
        /// Return a dataSet 
        /// </summary>
        /// <param name="strConexion"></param>
        /// <param name="ProcedureName"></param>
        /// <param name="params"></param>
        /// <param name="tipo"></param>
        /// <returns></returns>
        /// <remarks>Devuelve el conjunto de tablas de todos los SELECTs que se hagan dentro del SP</remarks>
        public DataSet execProcedureDataSet(string strConexion, string ProcedureName, SqlParametersList @params, CommandType tipo = CommandType.StoredProcedure)
        {
            SqlCommand mySqlCommand = GetCommand(strConexion, ProcedureName, @params, tipo);
            //  Dim ret As SqlDataReader
            DataSet myDataSet = new DataSet();
            try
            {
                mySqlCommand.Connection.Open();
                SqlDataAdapter mySqlDataAdapter = new SqlDataAdapter(mySqlCommand);
                mySqlDataAdapter.Fill(myDataSet);

            }
            catch (Exception e)
            {
                throw this.throwException(e, strConexion, ProcedureName, @params, tipo);
            }
            finally
            {
                close(mySqlCommand);
                mySqlCommand.Parameters.Clear();
            }
            return myDataSet;
        }

        private void close(SqlCommand mySqlCommand)
        {
            try
            {
                if (mySqlCommand != null && mySqlCommand.Connection != null)
                {
                    mySqlCommand.Connection.Close();
                }
            }
            catch (Exception ex)
            {
            }
        }

        //'Función para llamar a updates inserts etc
        public int execNonQuery(string strConexion, string ProcedureName, SqlParametersList @params, CommandType tipo = CommandType.StoredProcedure)
        {
            SqlCommand mySqlCommand = GetCommand(strConexion, ProcedureName, @params, tipo);
            int ret = 0;
            try
            {
                mySqlCommand.Connection.Open();
                ret = mySqlCommand.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                throw this.throwException(e, strConexion, ProcedureName, @params, tipo);
            }
            finally
            {
                close(mySqlCommand);
                mySqlCommand.Parameters.Clear();
            }
            return ret;
        }

        private Exception throwException(Exception ex, string strConexion, string ProcedureName, SqlParametersList @params, CommandType tipo = CommandType.StoredProcedure)
        {
            try
            {
                System.Text.StringBuilder msg = new System.Text.StringBuilder();
                var _with1 = msg;
                string paramsAux = "";
                if (@params != null)
                {
                    //paramsAux = @params.toString();
                }
                _with1.Append(ProcedureName + " " + paramsAux + " . [CONEXION]: " + strConexion + " . " + ex.Message);
                return new Exception(msg.ToString(), ex);
            }
            catch (Exception exc)
            {
                throw new Exception(exc.Message, ex);
            }
        }
    }
}