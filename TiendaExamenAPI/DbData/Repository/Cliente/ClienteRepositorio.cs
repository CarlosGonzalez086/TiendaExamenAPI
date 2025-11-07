using Microsoft.Data.SqlClient;
using System.Data;
using TiendaExamenAPI.DbData.DtoModels.Cliente;

namespace TiendaExamenAPI.DbData.Repository.Cliente
{
    public class ClienteRepositorio
    {
        DbData.Connection.Connection conexion = new();
        public dtoCliente obternerPorId(long ID)
        {
            string sql = $@"SELECT 
                            id,
                            nombre,
                            apellidos,
                            direccion,
                            correo_electronico,
                            contrasena
                        FROM clientes 
                       WHERE id = {ID}";
            var dt = conexion.Consultasql(sql);

            if (dt.Rows.Count == 0)
                return null;

            var row = dt.Rows[0];
            return new dtoCliente
            {
                id = long.Parse(row["id"].ToString()),
                nombre = row["nombre"].ToString(),
                apellidos = row["apellidos"].ToString(),
                direccion = row["direccion"].ToString(),
                correo_electronico = row["correo_electronico"].ToString(),
                contrasena = row["contrasena"].ToString(),
            };
        }
        public DataTable obtenerCorreo(string email, long ID = 0)
        {
            string sql = @$"    
                 SELECT COUNT(*) correo		
                 FROM
                 clientes
                 WHERE correo_electronico = @correo_electronico     
                AND eliminado = 0
                AND id <> {ID}";

            SqlCommand cmd = new SqlCommand(sql, conexion.ConnectionSql());
            cmd.Parameters.Add("@correo_electronico", SqlDbType.VarChar).Value = email;

            return conexion.Consultasql("", cmd);
        }
        public DataTable obtenerInfoLogin(string correo_electronico, string contrasena)
        {
            string sql = @$"SELECT 
                              id,
                              nombre,
                              apellidos,
                              nombre +' '+ apellidos AS nombre_completo,
                              correo_electronico,
                              contrasena     
                              FROM clientes (NOLOCK)
                              WHERE  
                              contrasena = @contrasena 
                              AND correo_electronico = @correo_electronico
                              AND eliminado = 0
";
            SqlCommand cmd = new SqlCommand(sql, conexion.ConnectionSql());
            cmd.Parameters.Add("@contrasena", SqlDbType.VarChar).Value = contrasena;
            cmd.Parameters.Add("@correo_electronico", SqlDbType.VarChar).Value = correo_electronico;
            return conexion.Consultasql("", cmd);
        }
        public (bool success, long insertedId) insertado(dtoCliente dtoUser)
        {
            try
            {
                string sql = @"
                                INSERT INTO clientes 
                                    (nombre, 
                                     apellidos, 
                                     direccion, 
                                     fecha, 
                                     correo_electronico, 
                                     contrasena)
                                OUTPUT INSERTED.ID
                                VALUES (
                                    @nombre,
                                    @apellidos,
                                    @direccion,
                                    @fecha,
                                    @correo_electronico,
                                    @contrasena);";

                using (SqlCommand cmd = new SqlCommand(sql, conexion.ConnectionSql()))
                {
                    cmd.Parameters.Add("@nombre", SqlDbType.VarChar).Value = dtoUser.nombre;
                    cmd.Parameters.Add("@apellidos", SqlDbType.VarChar).Value = dtoUser.apellidos;
                    cmd.Parameters.Add("@direccion", SqlDbType.VarChar).Value = dtoUser.direccion;
                    cmd.Parameters.Add("@correo_electronico", SqlDbType.VarChar).Value = dtoUser.correo_electronico;
                    cmd.Parameters.Add("@contrasena", SqlDbType.VarChar).Value = dtoUser.contrasena;
                    cmd.Parameters.Add("@fecha", SqlDbType.VarChar).Value = DateTime.UtcNow;
                    long insertedId = Convert.ToInt64(cmd.ExecuteScalar());
                    return (true, insertedId);
                }
            }
            catch (Exception ex)
            {
                return (false, 0);
            }
        }
        public bool actualizacion(dtoCliente dtoUser, long ID)
        {
            string sql = @$"UPDATE clientes SET
                            nombre =  @nombre,
                            apellidos = @apellidos,
                            direccion = @direccion,
                            fecha_actualizacion = @fecha_actualizacion
                            WHERE id = @id";

            SqlCommand cmd = new SqlCommand(sql, conexion.ConnectionSql());
            cmd.Parameters.Add("@nombre", SqlDbType.VarChar).Value = dtoUser.nombre;
            cmd.Parameters.Add("@apellidos", SqlDbType.VarChar).Value = dtoUser.apellidos;
            cmd.Parameters.Add("@direccion", SqlDbType.VarChar).Value = dtoUser.direccion;
            cmd.Parameters.Add("@fecha_actualizacion", SqlDbType.DateTime).Value = DateTime.UtcNow;
            cmd.Parameters.Add("@id", SqlDbType.BigInt).Value = ID;

            return conexion.InsertaSql("", cmd);
        }
        public bool eliminacion(long id)
        {
            string sql = @"UPDATE clientes
                        SET eliminado = 1,
                        fecha_eliminado = @fecha_eliminado
                        WHERE id = @id ";

            SqlCommand cmd = new SqlCommand(sql, conexion.ConnectionSql());
            cmd.Parameters.Add("@fecha_eliminado", SqlDbType.DateTime).Value = DateTime.UtcNow;
            cmd.Parameters.Add("@id", SqlDbType.BigInt).Value = id;

            return conexion.InsertaSql("", cmd);
        }
        public bool actualizarToken(long id, string token)
        {
            string sql = @$"UPDATE clientes
                            SET token = @token
                            WHERE id = @id";
            SqlCommand cmd = new SqlCommand(sql, conexion.ConnectionSql());
            cmd.Parameters.Add("@token", SqlDbType.VarChar).Value = token;
            cmd.Parameters.Add("@id", SqlDbType.BigInt).Value = id;
            return conexion.InsertaSql("", cmd);
        }
    }
}
