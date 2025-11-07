using Microsoft.Data.SqlClient;
using System.Data;

namespace TiendaExamenAPI.DbData.Connection
{
    public class Connection
    {
        public static string host = "";
        public SqlConnection connGlobal = null;
        public SqlTransaction transGlobal = null;
        private string defaultDb = "TiendaExamen";

        public static bool isProduction()
        {
            try
            {
                var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
                if (environment == Environments.Production)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {

                throw;
            }

        }
        public static string getServer()
        {
            var ruta = "http://localhost:5173";
            if (isProduction())
            {
                ruta = Environment.GetEnvironmentVariable("URL_MANAGER"); ;
            }
            return ruta;
        }

        public SqlConnection ConnectionSql()
        {
            string server;
            string usuario;
            string pas;
            string db = defaultDb;

            if (isProduction())
            {
                server = Environment.GetEnvironmentVariable("SERVER_DB", EnvironmentVariableTarget.Process);
                db = Environment.GetEnvironmentVariable("DB_TiendaExame", EnvironmentVariableTarget.Process) ?? db;
                usuario = Environment.GetEnvironmentVariable("USER_DB", EnvironmentVariableTarget.Process);
                pas = Environment.GetEnvironmentVariable("PASSWORD", EnvironmentVariableTarget.Process);
            }
            else
            {
                server = "YUKIE-ALTAIR-68"; 
                usuario = ""; 
                pas = "";
            }

            string conSQL;
            if (string.IsNullOrWhiteSpace(usuario))
            {
                conSQL = $"Data Source={server};Initial Catalog={db};Integrated Security=True;TrustServerCertificate=True;";
            }
            else
            {
                conSQL = $"Data Source={server};Initial Catalog={db};Persist Security Info=False;User ID={usuario};Password={pas};TrustServerCertificate=True;";
            }

            var cnn = new SqlConnection(conSQL);
            try
            {
                cnn.Open();
                return cnn; 
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("No se pudo abrir la conexión a la base de datos. Revisar cadena de conexión y credenciales.", ex);
            }
        }


        public DataTable Consultasql(string query, SqlCommand comando = null)
        {

            DataTable dt = new DataTable("tabla");
            SqlDataAdapter adapter = new SqlDataAdapter();
            if (comando == null)
            {
                comando = new SqlCommand(query, ConnectionSql());
            }
            adapter.SelectCommand = comando;
            try
            {
                adapter.Fill(dt);
            }
            catch (Exception ex)
            {
                int A = 0;
                string a = ex.ToString();
                Console.WriteLine(a);
            }
            try
            {
                comando.Connection.Close();
            }
            catch (Exception)
            {
            }
            return dt;
        }

        public bool InsertaSql(string Query, SqlCommand comando = null)
        {
            try
            {
                if (comando == null)
                {
                    comando = new SqlCommand(Query, ConnectionSql());
                }
                int filasAfectadas = comando.ExecuteNonQuery();
                if (filasAfectadas == 0)
                {
                    return false;
                }
                comando.Connection.Close();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                if (comando != null && comando.Connection != null && comando.Connection.State == ConnectionState.Open)
                {
                    comando.Connection.Close();
                }
            }

            return false;
        }
        public int NumeroFilas(string sql)
        {
            sql = $@"
            SELECT COUNT(*) FILAS FROM
            ( 
                {sql} 
            ) FILAS
            ";
            var resp = Consultasql(sql);
            int filas = 0;
            try
            {
                filas = Convert.ToInt32(resp.Rows[0]["FILAS"].ToString());
            }
            catch (Exception)
            {
            }
            return filas;
        }


        public void beginTransaction()
        {

            if (connGlobal == null)
            {
                connGlobal = ConnectionSql();
            }
            if (connGlobal.State == ConnectionState.Closed)
            {
                connGlobal.Open();
            }
            transGlobal = connGlobal.BeginTransaction();
        }

        public bool endTransaction()
        {
            try
            {
                transGlobal.Commit();
                connGlobal.Close();
                return true;
            }
            catch (Exception)
            {
                transGlobal.Rollback();
            }
            finally
            {
                if (connGlobal.State == ConnectionState.Open)
                {
                    connGlobal.Close();
                }
            }
            return false;
        }

        public void rollbackTransaction()
        {
            try
            {
                if (transGlobal != null)
                {
                    transGlobal.Rollback();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al hacer rollback: " + ex.Message);
            }
            finally
            {
                if (connGlobal != null && connGlobal.State == ConnectionState.Open)
                    connGlobal.Close();
            }
        }


        public bool InsertaSqlTransaction(string Query, SqlCommand comando = null)
        {
            try
            {
                if (connGlobal.State == ConnectionState.Closed)
                {
                    connGlobal.Open();
                }
                if (comando == null)
                {
                    comando = new SqlCommand(Query, connGlobal, transGlobal);
                }
                comando.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                string a = ex.ToString();
                rollbackTransaction();

            }
            return false;
        }


        public string getQuery(SqlCommand cmd)
        {
            string queryRaw = cmd.CommandText;
            foreach (SqlParameter p in cmd.Parameters)
            {
                queryRaw = queryRaw.Replace(p.ParameterName, p.Value.ToString());
            }
            return queryRaw;
        }
    }
}
