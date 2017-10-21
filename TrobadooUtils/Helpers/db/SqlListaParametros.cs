using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Text;

namespace trobadoo.com.utils.Helpers.db
{

    public class SqlParametersList
    {
        private List<SqlParameter> _listaParametros;

        public SqlParametersList()
        {
            _listaParametros = new List<SqlParameter>();
        }

        public List<SqlParameter> ListaParametros
        {
            get { return _listaParametros; }
        }

        public void @add(string name, string value, SqlDbType tipo)
        {
            if (value == null)
            {
                @add(name, tipo);
            }
            else
            {
                SqlParameter param = new SqlParameter();
                var _with1 = param;
                _with1.SqlDbType = tipo;
                _with1.Value = value;
                _with1.ParameterName = name;
                this._listaParametros.Add(param);
            }
        }

        //Metodo para pasar valores NULL

        public void @add(string name, SqlDbType tipo)
        {
            SqlParameter param = new SqlParameter();
            var _with2 = param;
            _with2.SqlDbType = tipo;
            _with2.Value = System.DBNull.Value;
            _with2.ParameterName = name;
            this._listaParametros.Add(param);
        }

        //Metodo para pasar valores NULL

        public void @add(string name, object value)
        {
            SqlParameter param = new SqlParameter();
            var _with3 = param;
            _with3.SqlDbType = SqlDbType.Variant;
            _with3.Value = (value == null ? System.DBNull.Value : value);
            _with3.ParameterName = name;
            this._listaParametros.Add(param);
        }


        public void @add(string name, DataTable value, SqlDbType tipo)
        {
            SqlParameter param = new SqlParameter();
            var _with4 = param;
            _with4.SqlDbType = tipo;
            _with4.Value = value;
            _with4.ParameterName = name;
            this._listaParametros.Add(param);
        }

    //public override string toString()
    //{
    //    StringBuilder strOut = new StringBuilder();
    //    if (_listaParametros != null) {
    //        foreach (SqlParameter param in _listaParametros) {
    //            strOut.Append("[Name] ");
    //            strOut.Append(param.ParameterName.ToString());
    //            strOut.Append(" - ");
    //            strOut.Append("[Value] ");
    //            if (param.Value == null) {
    //                strOut.Append("NULL");
    //            } else {
    //                strOut.Append(param.Value.ToString());
    //            }
    //            strOut.Append(" - ");
    //            strOut.Append("[type] ");
    //            strOut.Append(param.SqlDbType.ToString());
    //        }
    //    }
    //    return strOut.ToString();
    //}

    }

}