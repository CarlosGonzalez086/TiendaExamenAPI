using Microsoft.Data.SqlClient;
using System.Data;
using TiendaExamenAPI.DbData.Connection;
using TiendaExamenAPI.DbData.DtoModels.Cliente;
using TiendaExamenAPI.DbData.DtoModels.Tienda;

namespace TiendaExamenAPI.DbData.Repository.Tienda
{
    public class TiendaRepositorio
    {
        DbData.Connection.Connection conexion = new();

        public async Task<bool> insertado(dtoTienda dtoTienda)
        {
            try
            {
                string sql = @"
                                INSERT INTO tiendas 
                                    (sucursal, 
                                     direccion, 
                                     fecha)
                                VALUES (
                                    @sucursal,
                                    @direccion,   
                                    @fecha);";

                SqlCommand cmd = new SqlCommand(sql, conexion.ConnectionSql());
                cmd.Parameters.Add("@sucursal", SqlDbType.VarChar).Value = dtoTienda.sucursal;
                cmd.Parameters.Add("@direccion", SqlDbType.VarChar).Value = dtoTienda.direccion;
                cmd.Parameters.Add("@fecha", SqlDbType.DateTime).Value = DateTime.UtcNow;
                return conexion.InsertaSql("", cmd);
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public dtoTienda obternerPorId(long ID)
        {
            string sql = $@"SELECT 
                            id,
                            sucursal,
                            direccion
                        FROM tiendas 
                       WHERE id = {ID}";
            var dt = conexion.Consultasql(sql);

            if (dt.Rows.Count == 0)
                return null;

            var row = dt.Rows[0];
            return new dtoTienda
            {
                id = long.Parse(row["id"].ToString()),
                sucursal = row["sucursal"].ToString(),
                direccion = row["direccion"].ToString()
            };
        }
        public bool actualizacion(dtoTienda dto, long ID)
        {
            string sql = @$"UPDATE tiendas SET
                            sucursal =  @sucursal,
                            direccion = @direccion,
                            fecha_actualizacion = @fecha_actualizacion
                            WHERE id = @id";

            SqlCommand cmd = new SqlCommand(sql, conexion.ConnectionSql());
            cmd.Parameters.Add("@sucursal", SqlDbType.VarChar).Value = dto.sucursal;
            cmd.Parameters.Add("@direccion", SqlDbType.VarChar).Value = dto.direccion;
            cmd.Parameters.Add("@fecha_actualizacion", SqlDbType.DateTime).Value = DateTime.UtcNow;
            cmd.Parameters.Add("@id", SqlDbType.BigInt).Value = ID;

            return conexion.InsertaSql("", cmd);
        }
        public bool eliminacion(long id)
        {
            string sql = @"UPDATE tiendas
                        SET eliminado = 1,
                        fecha_eliminado = @fecha_eliminado
                        WHERE id = @id ";

            SqlCommand cmd = new SqlCommand(sql, conexion.ConnectionSql());
            cmd.Parameters.Add("@fecha_eliminado", SqlDbType.DateTime).Value = DateTime.UtcNow;
            cmd.Parameters.Add("@id", SqlDbType.BigInt).Value = id;

            return conexion.InsertaSql("", cmd);
        }
    }
}
