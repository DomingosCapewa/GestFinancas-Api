namespace Usuarios.DB;
using System.Collections.Generic;
using System.Linq;

public class Usuario
{
    public string nomeUsuario { get; set; }
    public string emailUsuario { get; set; }
    public string senha { get; set; }

    
    public Usuario(string nomeUsuario, string emailUsuario, string senha)
    {
        this.nomeUsuario = nomeUsuario ?? throw new ArgumentNullException(nameof(nomeUsuario));
        this.emailUsuario = emailUsuario ?? throw new ArgumentNullException(nameof(emailUsuario));
        this.senha = senha ?? throw new ArgumentNullException(nameof(senha));
    }
}

public class UsuarioDB
{
    private static List<Usuario> _usuarios = new List<Usuario>()
    {
        new Usuario("admin", "admin@gmail.com", "admin123"),
        new Usuario("teste", "teste@gmail.com", "teste123")
    };

    public static List<Usuario> GetUsuarios()
    {
        return _usuarios;
    }

    public static Usuario? GetUsuario(string EmailUsuario, string Senha)
    {
        return _usuarios.SingleOrDefault(usuario => usuario.emailUsuario == EmailUsuario && usuario.senha == Senha);
    }

    public static void CriarUsuario(string NomeUsuario, string EmailUsuario, string Senha)
    {
        _usuarios.Add(new Usuario(NomeUsuario, EmailUsuario, Senha));
    }

    public static void AtualizarUsuario(string NomeUsuario, string EmailUsuario, string Senha)
    {
        var usuario = _usuarios.SingleOrDefault(usuario => usuario.emailUsuario == EmailUsuario);
        if (usuario != null)
        {
            usuario.nomeUsuario = NomeUsuario;
            usuario.emailUsuario = EmailUsuario;
            usuario.senha = Senha;
        }
    }

    public static void DeletarUsuario(string EmailUsuario)
    {
        var usuario = _usuarios.SingleOrDefault(usuario => usuario.emailUsuario == EmailUsuario);
        if (usuario != null)
        {
            _usuarios.Remove(usuario);
        }
    }
}
