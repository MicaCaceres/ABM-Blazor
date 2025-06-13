using ABM.Common;
using ABM.DALs;
using ABM.DALs.ModelsDALs;
using ABM.DTOs;
using Microsoft.Data.SqlClient;

namespace ABM.Services
{
    public class PersonaService
    {
        readonly private PersonaModelDALs _personasDao;

        private readonly IConfiguration _configuracion;
        private readonly ITransaction<SqlTransaction> _transaction;

        public PersonaService(PersonaModelDALs personasDao, IConfiguration configuracion, ITransaction<SqlTransaction> transaction)
        {
            _personasDao = personasDao;
            _configuracion = configuracion;
            _transaction = transaction;
        }

        async public Task<List<Persona?>?> GetAll()
        {
            return await _personasDao.GetAll();
        }

        async public Task<Persona?> GetById(int id)
        {
            return await _personasDao.GetByKey(id);
        }

        async public Task CrearNuevo(Persona objeto)
        {
            await _personasDao.Insert(objeto);
        }

        async public Task Actualizar(Persona objeto)
        {
            await _personasDao.Update(objeto);
        }

        async public Task Eliminar(int id)
        {
            try
            {
                await _transaction.BeginTransaction();

                var objeto = await _personasDao.GetByKey(id, _transaction);
                if (objeto != null)
                {
                    await _personasDao.Delete(id, _transaction);
                }

                await _transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await _transaction.RollbackAsync();
                throw ex;
            }
        }

        [Transaction(Propagation = "Required", RollbackFor = "Exception")]
        async public Task Eliminar2(int id)
        {
            try
            {
                var objeto = await _personasDao.GetByKey(id, _transaction);
                if (objeto != null)
                {
                    await _personasDao.Delete(id, _transaction);
                }

                await _transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await _transaction.RollbackAsync();
                throw ex;
            }
        }
    }
}