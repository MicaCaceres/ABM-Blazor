using ABM.DTOs;
namespace ABM.Services
{
    public interface ICuentasService
    {
        public Task<List<Usuario>> GetAll();

        public Task<Usuario> GetByNombre(string nombre);

        public Task CrearNuevo(Usuario objeto);

        public Task<List<UsuarioRol>> GetRolesByUsuario(string nombreUsuario);

        public Task Actualizar(Usuario objeto);

        public Task Eliminar(string nombre);
    }
}
