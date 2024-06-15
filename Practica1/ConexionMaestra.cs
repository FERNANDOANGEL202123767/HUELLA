using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Windows.Forms;

namespace Practica1
{
    internal class ConexionMaestra
    {
        public static OdbcConnection conexion;
        public static OdbcCommand consulta;
        public static OdbcDataReader leer;

        public static void conectar()
        {
            string strConexion = "DRIVER={MySQL ODBC 5.1 Driver};" +
                "SERVER=localhost;" +
                "DATABASE=huella;" +
                "USER=root;" +
                "PASSWORD=2002;" +
                "PORT=3306;";

            conexion = new OdbcConnection(strConexion);
            conexion.Open();

            consulta = new OdbcCommand();
            consulta.Connection = conexion;

            Console.Write("CONEXIÓN EXITOSA");
        }

        public static void ejecutarConsulta(string sql)
        {
            consulta.CommandType = CommandType.Text;
            consulta.CommandText = sql;
            leer = consulta.ExecuteReader();
        }

        public static void Grid(DataGridView tabla, string sql)
        {
            ejecutarConsulta(sql);
            leer.Close();

            DataSet cuadricula = new DataSet();
            OdbcDataAdapter adaptador = new OdbcDataAdapter(consulta);
            adaptador.Fill(cuadricula, "Datos");

            tabla.DataSource = cuadricula;
            tabla.DataMember = "Datos";
        }

        public static void Combo(ComboBox lista, string sql)
        {
            ejecutarConsulta(sql);
            lista.Items.Clear();

            while (leer.Read())
            {
                lista.Items.Add(leer[0]);
            }

            leer.Close();
        }

        public static DataTable obtenerDatos(string sql)
        {
            DataTable dt = new DataTable();

            using (OdbcDataAdapter adaptador = new OdbcDataAdapter(sql, conexion))
            {
                adaptador.Fill(dt);
            }

            return dt;
        }
    }
}
