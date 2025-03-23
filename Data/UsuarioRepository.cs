using GestFinancas.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace GestFinancas.Data
{
  public class UsuarioRepository : IUsuarioRepository
  {
    private readonly string _connectionString;

    public UsuarioRepository(IConfiguration configuration)
    {
      _connectionString = configuration.GetConnectionString("DefaultConnection");
    }

    public async Task<List<Usuario>> GetAllUsuariosAsync()
    {
      var usuarios = new List<Usuario>();

      using (var connection = new SqlConnection(_connectionString))
      {
        await connection.OpenAsync();
        var command = new SqlCommand("SELECT Id, Nome, Email FROM Usuario", connection);

        using (var reader = await command.ExecuteReaderAsync())
        {
          while (await reader.ReadAsync())
          {
            usuarios.Add(new Usuario
            {
              Id = reader.GetInt32("Id"),
              Nome = reader.GetString("Nome"),
              Email = reader.GetString("Email")
            });
          }
        }
      }

      return usuarios;
    }

    public async Task<Usuario?> GetUsuarioByIdAsync(int id)
    {
      using (var connection = new SqlConnection(_connectionString))
      {
        await connection.OpenAsync();
        var command = new SqlCommand("SELECT Id, Nome, Email FROM Usuario WHERE Id = @Id", connection);
        command.Parameters.AddWithValue("@Id", id);

        using (var reader = await command.ExecuteReaderAsync())
        {
          if (await reader.ReadAsync())
          {
            return new Usuario
            {
              Id = reader.GetInt32("Id"),
              Nome = reader.GetString("Nome"),
              Email = reader.GetString("Email")
            };
          }
        }
      }

      return null;
    }

    public async Task<Usuario?> GetUsuarioByEmailSenhaAsync(string email, string senha)
    {
      using (var connection = new SqlConnection(_connectionString))
      {
        await connection.OpenAsync();
        var command = new SqlCommand("SELECT Id, Nome, Email FROM Usuario WHERE Email = @Email AND Senha = @Senha", connection);
        command.Parameters.AddWithValue("@Email", email);
        command.Parameters.AddWithValue("@Senha", senha);

        using (var reader = await command.ExecuteReaderAsync())
        {
          if (await reader.ReadAsync())
          {
            return new Usuario
            {
              Id = reader.GetInt32("Id"),
              Nome = reader.GetString("Nome"),
              Email = reader.GetString("Email")
            };
          }
        }
      }

      return null;
    }

    public async Task<Usuario?> ResetSenhaUsuario(string email, string novaSenha)
    {
      using (var connection = new SqlConnection(_connectionString))
      {
        await connection.OpenAsync();
        var updateCommand = new SqlCommand("UPDATE Usuario SET Senha = @NovaSenha, DataAtualizacao = @DataAtualizacao WHERE Email = @Email", connection);

        updateCommand.Parameters.AddWithValue("@Email", email);
        updateCommand.Parameters.AddWithValue("@NovaSenha", novaSenha);
        updateCommand.Parameters.AddWithValue("@DataAtualizacao", DateTime.Now);

        var rows = await updateCommand.ExecuteNonQueryAsync();

        if (rows == 0)
        {
          return null;
        }

        return await GetUsuarioByEmailSenhaAsync(email, novaSenha);
      }
    }

    public async Task<int> AddUsuarioAsync(Usuario usuario)
    {
      using (var connection = new SqlConnection(_connectionString))
      {
        await connection.OpenAsync();
        var command = new SqlCommand("INSERT INTO Usuario (Nome, Email, Senha, DataCadastro) OUTPUT INSERTED.Id VALUES (@Nome, @Email, @Senha, @DataCadastro)", connection);
        command.Parameters.AddWithValue("@Nome", usuario.Nome);
        command.Parameters.AddWithValue("@Email", usuario.Email);
        command.Parameters.AddWithValue("@Senha", usuario.Senha);
        command.Parameters.AddWithValue("@DataCadastro", DateTime.Now);

        return (int)await command.ExecuteScalarAsync();
      }
    }

    public async Task<int> UpdateUsuarioAsync(Usuario usuario)
    {
      using (var connection = new SqlConnection(_connectionString))
      {
        await connection.OpenAsync();
        var command = new SqlCommand("UPDATE Usuario SET Nome = @Nome, Email = @Email WHERE Id = @Id", connection);
        command.Parameters.AddWithValue("@Id", usuario.Id);
        command.Parameters.AddWithValue("@Nome", usuario.Nome);
        command.Parameters.AddWithValue("@Email", usuario.Email);

        return await command.ExecuteNonQueryAsync();
      }
    }

    public async Task<int> DeleteUsuarioAsync(int id)
    {
      using (var connection = new SqlConnection(_connectionString))
      {
        await connection.OpenAsync();
        var command = new SqlCommand("DELETE FROM Usuario WHERE Id = @Id", connection);
        command.Parameters.AddWithValue("@Id", id);

        return await command.ExecuteNonQueryAsync();
      }
    }
  }
}
