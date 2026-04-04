using BlazorApp1.Models;

namespace BlazorApp1.Application.Interfaces;

public interface IUsuarioRepository
{
    Task<Usuario?> GetByUsernameAsync(string username);
    Task<IEnumerable<Usuario>> GetAllAsync();
    Task<int> GetCountAsync();
}
