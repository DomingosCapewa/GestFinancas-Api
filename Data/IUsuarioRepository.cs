using GestFinancas_Api.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GestFinancas.Data
{
    public interface IUsuarioRepository
    {
        Task<List<Usuario>> ObterTodosUsuariosAsync();
        Task<Usuario?> ObterUsuarioPorIdAsync(int id);
        Task<Usuario?> ObterUsuarioPorEmailSenhaAsync(string email, string senha);
        // Task<Usuario?> ResetarSenhaUsuario(string email, string novaSenha);
        Task<int> AddUsuarioAsync(Usuario usuario);
        Task<int> AtualizarUsuarioAsync(Usuario usuario);
        Task<Usuario?> RecuperarSenha(string email);
        Task<Usuario?> BuscarUsuarioPorEmail(string email); 
     
        // Task<int> DeleteUsuarioAsync(int id);
    }
}
