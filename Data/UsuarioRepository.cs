using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace GestFinancas.Data
{
  public class UsuarioRepository
  {
    private readonly string _connectionString;

    public UsuarioRepository(string connectionString)
    {
      _connectionString = connectionString;
    }

    // Método assíncrono para obter todos os usuários
    public async Task<List<Usuario>> GetAllUsuariosAsync()
    {
      var usuario = new List<Usuario>();

      using (var connection = new SqlConnection(_connectionString))
      {
        await connection.OpenAsync();

        // Aqui você está criando o comando SQL para selecionar todos os usuários
        var command = new SqlCommand("SELECT Id, Nome, Email FROM Usuario", connection);

        using (var reader = await command.ExecuteReaderAsync())
        {
          while (await reader.ReadAsync())
          {
            usuario.Add(new Usuario
            {
              Id = reader.GetInt32(reader.GetOrdinal("Id")),
              Nome = reader.GetString(reader.GetOrdinal("Nome")),
              Email = reader.GetString(reader.GetOrdinal("Email"))
            });
          }
        }
      }

      return usuario;
    }

    // Método assíncrono para obter um usuário por ID
    public async Task<Usuario?> GetUsuarioByIdAsync(int id)
    {
      Usuario? usuario = null;

      using (var connection = new SqlConnection(_connectionString))
      {
        await connection.OpenAsync();

        var command = new SqlCommand("SELECT Id, Nome, Email FROM Usuario WHERE Id = @Id", connection);
        command.Parameters.Add("@Id", SqlDbType.Int).Value = id;

        using (var reader = await command.ExecuteReaderAsync())
        {
          if (await reader.ReadAsync())
          {
            usuario = new Usuario
            {
              Id = reader.GetInt32(reader.GetOrdinal("Id")),
              Nome = reader.GetString(reader.GetOrdinal("Nome")),
              Email = reader.GetString(reader.GetOrdinal("Email")),

            };
          }
        }
      }

      return usuario;
    }

    public async Task<Usuario?> GetUsuarioByEmailSenhaAsync(string email, string senha)
    {
      Usuario? usuario = null;

      using (var connection = new SqlConnection(_connectionString))
      {
        await connection.OpenAsync();

        var command = new SqlCommand("SELECT Id, Nome, Email FROM Usuario WHERE Email = @Email AND Senha = @Senha", connection);
        command.Parameters.Add("@Email", SqlDbType.VarChar).Value = email;
        command.Parameters.Add("@Senha", SqlDbType.VarChar).Value = senha;

        using (var reader = await command.ExecuteReaderAsync())
        {
          if (await reader.ReadAsync())
          {
            usuario = new Usuario
            {
              Id = reader.GetInt32(reader.GetOrdinal("Id")),
              Nome = reader.GetString(reader.GetOrdinal("Nome")),
              Email = reader.GetString(reader.GetOrdinal("Email")),
              Senha = reader.GetString(reader.GetOrdinal("Senha"))

            };
          }
        }
      }

      return usuario;
    }

    // Método assíncrono para adicionar um usuário
    public async Task<int> AddUsuarioAsync(Usuario usuario)
    {
      using (var connection = new SqlConnection(_connectionString))
      {
        await connection.OpenAsync();

        // Comando SQL para inserir um novo usuário
        var command = new SqlCommand("INSERT INTO Usuario (Nome, Email, Senha) OUTPUT INSERTED.Id VALUES (@Nome, @Email, @Senha)", connection);
        command.Parameters.AddWithValue("@Nome", usuario.Nome);
        command.Parameters.AddWithValue("@Email", usuario.Email);
        command.Parameters.AddWithValue("@Senha", usuario.Senha);

        // Retorna o ID do usuário inserido
        return (int)await command.ExecuteScalarAsync();
      }
    }

    // Método assíncrono para atualizar um usuário
    public async Task<int> UpdateUsuarioAsync(Usuario usuario)
    {
      using (var connection = new SqlConnection(_connectionString))
      {
        await connection.OpenAsync();

        var command = new SqlCommand("UPDATE Usuario SET Nome = @Nome, Email = @Email WHERE Id = @Id", connection);
        command.Parameters.AddWithValue("@Id", usuario.Id);
        command.Parameters.AddWithValue("@Nome", usuario.Nome);
        command.Parameters.AddWithValue("@Email", usuario.Email);

        // Retorna o número de linhas afetadas
        return await command.ExecuteNonQueryAsync();
      }

    }

    //Metodo assíncrono para deletar um usuário
    public async Task<int> DeleteUsuarioAsync(int id)
    {
      using (var connection = new SqlConnection(_connectionString))
      {
        await connection.OpenAsync();

        var command = new SqlCommand("DELETE FROM Usuario WHERE Id = @Id", connection);
        command.Parameters.AddWithValue("@Id", id);

        // Retorna o número de linhas afetadas
        return await command.ExecuteNonQueryAsync();
      }
    }
  }
}
