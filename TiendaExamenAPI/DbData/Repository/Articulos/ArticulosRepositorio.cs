using Microsoft.Data.SqlClient;
using System.Data;
using TiendaExamenAPI.DbData.Connection;
using TiendaExamenAPI.DbData.DtoModels.articulos;
using TiendaExamenAPI.DbData.DtoModels.Cliente;
using TiendaExamenAPI.DbData.DtoModels.Tienda;

namespace TiendaExamenAPI.DbData.Repository.Articulos
{
    public class ArticulosRepositorio
    {
        DbData.Connection.Connection conexion = new();

        public dtoArticulos obternerPorId(long ID)
        {
            string sql = $@"SELECT 
                            id,
                            codigo,
                            descripcion,
                            precio,
                            imagen,
                            stock
                        FROM articulos 
                       WHERE id = {ID}";
            var dt = conexion.Consultasql(sql);

            if (dt.Rows.Count == 0)
                return null;

            var row = dt.Rows[0];
            return new dtoArticulos
            {
                id = long.Parse(row["id"].ToString()),
                codigo = row["codigo"].ToString(),
                descripcion = row["descripcion"].ToString(),
                precio = Convert.ToDecimal(row["precio"].ToString()),
                imagen = row["imagen"].ToString(),
                stock = Convert.ToInt32(row["stock"].ToString()),
            };
        }
        public async Task<bool> insertado(dtoArticulos dtoArticulo)
        {
            try
            {
                string sql = @"
                                INSERT INTO articulos 
                                    (codigo, 
                                     descripcion, 
                                     imagen, 
                                     fecha, 
                                     precio, 
                                     stock)
                                VALUES (
                                    @codigo,
                                    @descripcion,
                                    @imagen,
                                    @fecha,
                                    @precio,
                                    @stock);";

                SqlCommand cmd = new SqlCommand(sql, conexion.ConnectionSql());
                cmd.Parameters.Add("@codigo", SqlDbType.VarChar).Value = dtoArticulo.codigo;
                cmd.Parameters.Add("@descripcion", SqlDbType.VarChar).Value = dtoArticulo.descripcion;
                cmd.Parameters.Add("@imagen", SqlDbType.VarChar).Value = dtoArticulo.imagen;
                cmd.Parameters.Add("@stock", SqlDbType.Int).Value = dtoArticulo.stock;
                cmd.Parameters.Add("@precio", SqlDbType.Decimal).Value = dtoArticulo.precio;
                cmd.Parameters.Add("@fecha", SqlDbType.DateTime).Value = DateTime.UtcNow;
                return conexion.InsertaSql("", cmd);
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool actualizacion(dtoArticulos dtoArticulo, long ID)
        {
            string sql = @$"UPDATE articulos SET
                            codigo =  @codigo,
                            descripcion = @descripcion,
                            imagen = @imagen,
                            stock = @stock,
                            precio = @precio,
                            fecha_actualizacion = @fecha_actualizacion
                            WHERE id = @id";

            SqlCommand cmd = new SqlCommand(sql, conexion.ConnectionSql());
            cmd.Parameters.Add("@codigo", SqlDbType.UniqueIdentifier).Value = dtoArticulo.codigo;
            cmd.Parameters.Add("@descripcion", SqlDbType.VarChar).Value = dtoArticulo.descripcion;
            cmd.Parameters.Add("@imagen", SqlDbType.VarChar).Value = dtoArticulo.imagen;
            cmd.Parameters.Add("@stock", SqlDbType.Int).Value = dtoArticulo.stock;
            cmd.Parameters.Add("@precio", SqlDbType.Decimal).Value = dtoArticulo.precio;
            cmd.Parameters.Add("@fecha_actualizacion", SqlDbType.DateTime).Value = DateTime.UtcNow;
            cmd.Parameters.Add("@id", SqlDbType.BigInt).Value = ID;

            return conexion.InsertaSql("", cmd);
        }
        public bool eliminacion(long id)
        {
            string sql = @"UPDATE articulos
                        SET eliminado = 1,
                        fecha_eliminado = @fecha_eliminado
                        WHERE id = @id ";

            SqlCommand cmd = new SqlCommand(sql, conexion.ConnectionSql());
            cmd.Parameters.Add("@fecha_eliminado", SqlDbType.DateTime).Value = DateTime.UtcNow;
            cmd.Parameters.Add("@id", SqlDbType.BigInt).Value = id;

            return conexion.InsertaSql("", cmd);
        }
        public (DataTable dtInfo, int filas) lista(int iTake, int iSkip)
        {
            string sqlData = $@"
                           SELECT 
                            a.id,
                            a.codigo,
                            a.descripcion,
                            a.imagen,
                            a.stock,
                            a.precio,
                            COUNT(*) OVER() as TotalRegistros   
	                        FROM 
                            articulos a (NOLOCK)
	                        WHERE 
                            a.eliminado = 0
                            ORDER BY fecha DESC
                            OFFSET {iSkip} ROWS
                            FETCH NEXT {iTake} ROWS ONLY ";

            SqlCommand cmd = new SqlCommand(sqlData, conexion.ConnectionSql());
            DataTable dtInfo = conexion.Consultasql("", cmd);
            int totalRows = dtInfo.Rows.Count > 0 ? Convert.ToInt32(dtInfo.Rows[0]["TotalRegistros"]) : 0;
            return (dtInfo, totalRows);
        }
    }
}
