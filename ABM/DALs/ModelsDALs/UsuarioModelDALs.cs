﻿using Microsoft.Data.SqlClient;
using ABM.DTOs;

namespace ABM.DALs.ModelsDALs
{
    public class UsuarioModelDALs : IBaseDAL<Usuario, string, SqlTransaction>
    {
        private readonly SqlConnection _sqlConnection;

        public UsuarioModelDALs(SqlConnection sqlConnection)
        {
            _sqlConnection = sqlConnection;
        }

        async public Task<List<Usuario>> GetAll(ITransaction<SqlTransaction>? transaccion = null)
        {
            var lista = new List<Usuario>();

            string sqlQuery =@"
                             SELECT u.* 
                             FROM Usuarios u";

            var conexion = await GetOpenedConnectionAsync(transaccion);

            using var query = new SqlCommand(sqlQuery, conexion, transaccion?.GetInternalTransaction());

            using var reader = await query.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                var objeto = ReadAsObjecto(reader);
                lista.Add(objeto);
            }
            return lista;
        }

        async public Task<Usuario?> GetByKey(string nombre, ITransaction<SqlTransaction>? transaccion = null)
        {
            Usuario objeto = null;

            string sqlQuery =@"
                             SELECT TOP 1 u.* 
                             FROM Usuarios u
                             WHERE UPPER(TRIM(u.Nombre)) LIKE UPPER(TRIM(@Nombre))";

            var conexion = await GetOpenedConnectionAsync(transaccion);

            using var query = new SqlCommand(sqlQuery, conexion, transaccion?.GetInternalTransaction());
            query.Parameters.AddWithValue("@Nombre", nombre);

            using var reader = await query.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                objeto = ReadAsObjecto(reader);
            }
            return objeto;
        }

        async public Task<bool> Insert(Usuario nuevo, ITransaction<SqlTransaction>? transaccion = null)
        {
            string sqlQuery =@"
                             INSERT Usuarios(Nombre, Clave)
                             VALUES (@Nombre, @Clave)";

            var conexion = await GetOpenedConnectionAsync(transaccion);

            using var query = new SqlCommand(sqlQuery, conexion, transaccion?.GetInternalTransaction());
            query.Parameters.AddWithValue("@Nombre", nuevo.Nombre);
            query.Parameters.AddWithValue("@Clave", nuevo.Clave);

            int cantInsertados = Convert.ToInt32(await query.ExecuteNonQueryAsync());
            return cantInsertados > 0;
        }

        async public Task<bool> Update(Usuario actualizar, ITransaction<SqlTransaction>? transaccion = null)
        {
            string sqlQuery =@"
                             UPDATE Usuarios SET Clave=@Clave
                             WHERE UPPER(TRIM(Nombre)) LIKE UPPER(@Nombre_Usuario)";

            var conexion = await GetOpenedConnectionAsync(transaccion);

            using var query = new SqlCommand(sqlQuery, conexion);
            query.Parameters.AddWithValue("@Clave", actualizar.Clave);
            query.Parameters.AddWithValue("@Nombre_Usuario", actualizar.Nombre);

            int cantidad = await query.ExecuteNonQueryAsync();

            return cantidad > 0;
        }

        async public Task<bool> Delete(string nombre, ITransaction<SqlTransaction>? transaccion = null)
        {
            string sqlQuery =@"
                             DELETE FROM Usuarios
                             WHERE UPPER(TRIM(Nombre)) LIKE UPPER(@Nombre)";

            var conexion = await GetOpenedConnectionAsync(transaccion);

            using var query = new SqlCommand(sqlQuery, conexion, transaccion?.GetInternalTransaction());
            query.Parameters.AddWithValue("@Nombre", nombre);

            int eliminados = await query.ExecuteNonQueryAsync();

            return eliminados > 0;
        }

        private async Task<SqlConnection> GetOpenedConnectionAsync(ITransaction<SqlTransaction>? transaccion)
        {
            var conexion = transaccion?.GetInternalTransaction()?.Connection ?? _sqlConnection;
            if (conexion.State == System.Data.ConnectionState.Closed)
            {
                await conexion.OpenAsync();
            }
            return conexion;
        }

        protected Usuario ReadAsObjecto(SqlDataReader reader, ITransaction<SqlTransaction>? transaccion = null)
        {
            string nombre = reader["Nombre"] != DBNull.Value ? Convert.ToString(reader["Nombre"]) : "";
            string clave = reader["Clave"] != DBNull.Value ? Convert.ToString(reader["Clave"]) : "";

            var objeto = new Usuario { Nombre = nombre, Clave = clave };

            return objeto;
        }
    }
}
