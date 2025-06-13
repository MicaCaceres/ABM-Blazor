using ABM.DALs.ModelsDALs;
using ABM.DTOs;


namespace ABM.Services
{
  public class RolService
  {   
    readonly private RolModelDALs _rolesDao;

    public RolService(RolModelDALs rolesDao)
    {
        _rolesDao = rolesDao;
    }

    async public Task<List<Rol>> GetAll()
    {
        return await _rolesDao.GetAll();
    }
  }
}
