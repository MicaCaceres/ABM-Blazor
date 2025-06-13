using Microsoft.Data.SqlClient;
using ABM.DTOs;

namespace ABM.DALs.ModelsDALs
{
    public class PersonaModelDALs : IBaseDAL<Persona, int, SqlTransaction>
    {
        private readonly SqlConnection _sqlConnection;
        private readonly ILogger<PersonaModelDALs> _logger;
        public PersonaModelDALs(SqlConnection sqlConnection, ILogger<PersonaModelDALs> logger)
        {
            _sqlConnection = sqlConnection;
            _logger = logger;
        }

        public async Task<List<Persona>> GetAll(ITransaction<SqlTransaction>? transaccion = null)
        {
            var lista = new List<Persona>();

            string sqlQuery = @"
                             SELECT p.* 
                             FROM Personas p";

            var conexion = await GetOpenedConnectionAsync(transaccion);

            using var query = new SqlCommand(sqlQuery, conexion, transaccion?.GetInternalTransaction());

            using var reader = await query.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var objeto = ReadAsObjeto(reader);
                lista.Add(objeto);
            }
            return lista;
        }

        public async Task<Persona?> GetByKey(int id, ITransaction<SqlTransaction>? transaccion = null)
        {
            Persona objeto = null;

            string sqlQuery = @"
                              SELECT TOP 1 p.* 
                              FROM Personas p
                              WHERE p.Id = @Id";

            var conexion = await GetOpenedConnectionAsync(transaccion);

            using var query = new SqlCommand(sqlQuery, conexion, transaccion?.GetInternalTransaction());
            query.Parameters.AddWithValue("@Id", id);

            using var reader = await query.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                objeto = ReadAsObjeto(reader);
            }
            return objeto;
        }

        public async Task<bool> Insert(Persona nuevo, ITransaction<SqlTransaction>? transaccion = null)
        {
            string sqlQuery = @"
                              INSERT INTO Personas (Dni, Nombre, Fecha_Nacimiento)
                              OUTPUT INSERTED.ID 
                              VALUES (@Dni, @Nombre, @Fecha_Nacimiento)";

            var conexion = await GetOpenedConnectionAsync(transaccion);

            using var query = new SqlCommand(sqlQuery, conexion, transaccion?.GetInternalTransaction());
            query.Parameters.AddWithValue("@Dni", nuevo.DNI);
            query.Parameters.AddWithValue("@Nombre", nuevo.Nombre);
            query.Parameters.AddWithValue("@Fecha_Nacimiento", nuevo.FechaNacimiento);

            var respuesta = await query.ExecuteScalarAsync();
            nuevo.Id = Convert.ToInt32(respuesta);
            return nuevo.Id > 0;
        }

        public async Task<bool> Update(Persona actualizar, ITransaction<SqlTransaction>? transaccion = null)
        {
            string sqlQuery = @"
                              UPDATE Personas 
                              SET Dni = @Dni, Nombre = @Nombre, Fecha_Nacimiento = @Fecha_Nacimiento 
                              WHERE Id = @Id"
            ;

            var conexion = await GetOpenedConnectionAsync(transaccion);

            using var query = new SqlCommand(sqlQuery, conexion, transaccion?.GetInternalTransaction());
            query.Parameters.AddWithValue("@Dni", actualizar.DNI);
            query.Parameters.AddWithValue("@Nombre", actualizar.Nombre);
            query.Parameters.AddWithValue("@Fecha_Nacimiento", actualizar.FechaNacimiento);
            query.Parameters.AddWithValue("@Id", actualizar.Id);

            int cantidad = await query.ExecuteNonQueryAsync();
            return cantidad > 0;
        }

        public async Task<bool> Delete(int id, ITransaction<SqlTransaction>? transaccion = null)
        {
            string sqlQuery = @"
                             DELETE FROM Personas
                             WHERE Id = @Id";

            var conexion = await GetOpenedConnectionAsync(transaccion);

            using var query = new SqlCommand(sqlQuery, conexion, transaccion?.GetInternalTransaction());
            query.Parameters.AddWithValue("@Id", id);

            int eliminados = await query.ExecuteNonQueryAsync();
            return eliminados > 0;
        }

        private async Task<SqlConnection> GetOpenedConnectionAsync(ITransaction<SqlTransaction>? transaccion)
        {
            var conexion = transaccion?.GetInternalTransaction()?.Connection ?? _sqlConnection;

            if (conexion.State == System.Data.ConnectionState.Closed || conexion.State == System.Data.ConnectionState.Broken)
            {

                if (conexion.State == System.Data.ConnectionState.Broken)
                {
                    conexion.Close();
                }

                await conexion.OpenAsync();
            }

            return conexion;
        }

        private Persona ReadAsObjeto(SqlDataReader reader)
        {
            int id = reader["Id"] != DBNull.Value ? Convert.ToInt32(reader["Id"]) : 0;
            int dni = reader["DNI"] != DBNull.Value ? Convert.ToInt32(reader["DNI"]) : 0;
            string nombre = reader["Nombre"] != DBNull.Value ? Convert.ToString(reader["Nombre"]) : "";
            DateTime? nacimiento = reader["Fecha_Nacimiento"] != DBNull.Value
                ? Convert.ToDateTime(reader["Fecha_Nacimiento"])
                : (DateTime?)null;

            return new Persona
            {
                Id = id,
                DNI = dni,
                Nombre = nombre,
                FechaNacimiento = nacimiento
            };
        }
    }
}
