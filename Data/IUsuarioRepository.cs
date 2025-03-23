using GestFinancas.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GestFinancas.Data
{
  public interface IUsuarioRepository
  {
    Task<List<Usuario>> GetAllUsuariosAsync();
    Task<Usuario?> GetUsuarioByIdAsync(int id);
    Task<Usuario?> GetUsuarioByEmailSenhaAsync(string email, string senha);
    Task<Usuario?> ResetSenhaUsuario( string email, string novaSenha);
    Task<int> AddUsuarioAsync(Usuario usuario);
    Task<int> UpdateUsuarioAsync(Usuario usuario);
    Task<int> DeleteUsuarioAsync(int id);
  }
}
