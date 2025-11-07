using Microsoft.Data.SqlClient;
using System.Data;
using TiendaExamenAPI.DbData.Connection;

namespace TiendaExamenAPI.DbData.Repository.ArticuloCliente
{
    public class ArticuloClienteRepositorio
    {
        DbData.Connection.Connection conexion = new();

        public (bool success, long insertedId) insertado(decimal total)
        {
            try
            {
                string sql = @"
                                INSERT INTO cliente_articulo 
                                    (
                                     total, 
                                     fecha)
                  OUTPUT INSERTED.ID
                                VALUES (                                    
                                    @total,
                                    @fecha);";
                SqlCommand cmd = new SqlCommand(sql, conexion.ConnectionSql());                
                cmd.Parameters.AddWithValue("@total", total);
                cmd.Parameters.AddWithValue("@fecha", DateTime.UtcNow);
                long insertedId = Convert.ToInt64(cmd.ExecuteScalar());
                return (true, insertedId);
            }
            catch (Exception)
            {
                return (false, 0);
            }
        }
        public async Task<bool> insertadoDetalle(long id_cliente_articulo, int articulo_id, long cliente_id,int cantidad)
        {
            try
            {
                string sql = @"
                                INSERT INTO rel_cliente_articulo 
                                    (
                                     id_cliente_articulo, 
                                     cantidad, 
                                     fecha,
                                     articulo_id,
                                     cliente_id)
                                VALUES (                                    
                                    @id_cliente_articulo,
                                    @cantidad,
                                    @fecha,
                                    @articulo_id,
                                    @cliente_id);";
                SqlCommand cmd = new SqlCommand(sql, conexion.ConnectionSql());
                cmd.Parameters.AddWithValue("@id_cliente_articulo", id_cliente_articulo);
                cmd.Parameters.AddWithValue("@cantidad", cantidad);
                cmd.Parameters.AddWithValue("@articulo_id", articulo_id);
                cmd.Parameters.AddWithValue("@cliente_id", cliente_id);
                cmd.Parameters.AddWithValue("@fecha", DateTime.UtcNow);
                return conexion.InsertaSql("", cmd);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public DataTable obtenerCompraCliente(long id)
        {

            string sql = $@" SELECT ca.id,
                                ca.total,
                                ca.fecha,
	                             ( SELECT  
                                  a.descripcion as nombre,					 
                                  a.precio,
                                  rca.cantidad
								  FROM rel_cliente_articulo rca
								  INNER JOIN articulos a ON rca.articulo_id = a.id
								  WHERE rca.id_cliente_articulo = ca.id  
								  FOR JSON PATH
							  ) AS articulos
		                        FROM cliente_articulo ca (NOLOCK)
		                        WHERE ca.id= " +id;
            return conexion.Consultasql(sql);
        }
        public (DataTable dtInfo, int filas) lista(int iTake, int iSkip, long idCliente)
        {
            string sqlData = @$"
                                SELECT t.total, t.fecha, t.TotalRegistros
                                FROM (
                                          SELECT top 1
                                        ca.total,
                                        ca.fecha,
                                        COUNT(*) OVER() AS TotalRegistros,
                                        ROW_NUMBER() OVER (ORDER BY ca.fecha DESC) AS rn
                                    FROM cliente_articulo ca WITH (NOLOCK)
                                    JOIN rel_cliente_articulo rca WITH (NOLOCK)
                                        ON rca.id_cliente_articulo = ca.id
                                    WHERE rca.cliente_id = {idCliente}
                                ) AS t
                                WHERE t.rn > {iSkip}
                                  AND t.rn <= ({iSkip} + {iTake})
                                ORDER BY t.rn;
                                ";

            using (SqlCommand cmd = new SqlCommand(sqlData, conexion.ConnectionSql()))
            {
                cmd.Parameters.Add("@cliente", System.Data.SqlDbType.BigInt).Value = idCliente;
                cmd.Parameters.Add("@skip", System.Data.SqlDbType.Int).Value = iSkip;
                cmd.Parameters.Add("@take", System.Data.SqlDbType.Int).Value = iTake;

                DataTable dtInfo = conexion.Consultasql("", cmd);
                int totalRows = dtInfo.Rows.Count > 0 && dtInfo.Columns.Contains("TotalRegistros")
                    ? Convert.ToInt32(dtInfo.Rows[0]["TotalRegistros"])
                    : 0;

                return (dtInfo, totalRows);
            }
        }
    }
}
