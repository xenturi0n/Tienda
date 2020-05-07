using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Tienda.Common.Interfaces;
using System.Data.SqlClient;
using System.Data.Common;

namespace Tienda.DAL.SQLServer
{
    class DBSQLServer : IDB
    {
        private SqlConnection _conexion;
        public DBSQLServer()
        {
            string _server = "INFORMATICA\\SQLEXPRESS";
            string _database = "Tienda";
            string _uid = "Tienda";
            string _password = "16123356";

            _conexion = new SqlConnection($"Server={_server};Database={_database};User Id={_uid};Password={_password}");

            Conectar();

        }

        public string Error { get; private set; }

        public void Conectar()
        {
            try
            {
                _conexion.Open();
                Error = "";

            }
            catch (Exception ex)
            {
                Error = ex.Message;
                throw;
            }
        }        

        public bool Comando(string command)
        {
            try
            {
                SqlCommand cmd = new SqlCommand(command, _conexion);
                cmd.ExecuteNonQuery();
                Error = "";
                return true;
            }
            catch (Exception ex)
            {
                Error = ex.Message;
                return false;
                throw;
            }
        }        

        public object Consulta(string consulta)
        {
            try
            {
                SqlCommand cmd = new SqlCommand(consulta, _conexion);
                SqlDataReader dr = cmd.ExecuteReader();
                Error = "";
                return dr;
            }
            catch (Exception ex)
            {
                Error = ex.Message;
                return null;
            }
        }

        public object Scalar(string consulta)
        {
            try
            {
                SqlCommand cmd = new SqlCommand(consulta, _conexion);
                object result = cmd.ExecuteScalar();
                return result;
            }catch (Exception ex)
            {
                Error = ex.Message;
                return null;
            }
        }

        ~DBSQLServer()
        {
            if(_conexion.State == System.Data.ConnectionState.Open)
            {
                _conexion.Close();
            }
        }
    }
}
