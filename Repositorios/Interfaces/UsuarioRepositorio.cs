using Microsoft.EntityFrameworkCore;
using SistemaTarefas.Data;
using SistemaTarefas.Models;

namespace SistemaTarefas.Repositorios.Interfaces
{
    public class UsuarioRepositorio : IUsuarioRepositorio
    {
        private readonly SistemaTarefasDbContext _dbcontext;

        public UsuarioRepositorio(SistemaTarefasDbContext sistemaTarefasDbContext)
        {
            _dbcontext = sistemaTarefasDbContext;
        }

        public async Task<UsuarioModel> BuscarPorId(int id)
        {
            // Busca o primeiro usuário cujo ID corresponde ao parâmetro. 
            // Retorna null caso não encontre nenhum registro.
            return await _dbcontext.Usuarios.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<UsuarioModel>> BuscarTodosUsuarios()
        {
            // Retorna todos os registros da tabela de usuários como uma lista.
            return await _dbcontext.Usuarios.ToListAsync();
        }

        public async Task<UsuarioModel> Adicionar(UsuarioModel usuario)
        {
            // Adiciona um novo registro à memória. As alterações não são persistidas até SaveChangesAsync.
            await _dbcontext.Usuarios.AddAsync(usuario);

            // Salva efetivamente as alterações no banco de dados.
            await _dbcontext.SaveChangesAsync();

            return usuario;
        }

        public async Task<UsuarioModel> Atualizar(UsuarioModel usuario, int id)
        {
            UsuarioModel usuarioPorId = await BuscarPorId(id);
            if (usuarioPorId == null)
            {
                // Mensagem personalizada para informar que o usuário não foi encontrado. 
                // Note que utilizamos o ID na mensagem para facilitar o rastreamento do erro.
                throw new Exception($"Usuario para o ID {id} não encontrado.");
            }

            // Atualiza apenas os campos modificáveis. 
            // As alterações só serão refletidas no banco após SaveChangesAsync.
            usuarioPorId.Nome = usuario.Nome;
            usuarioPorId.Email = usuario.Email;

            _dbcontext.Usuarios.Update(usuarioPorId); // Marca o registro como modificado.
            await _dbcontext.SaveChangesAsync(); // Persiste as alterações no banco de dados.
            return usuarioPorId;
        }

        public async Task<bool> Apagar(int id)
        {
            UsuarioModel usuarioPoriD = await BuscarPorId(id);
            if (usuarioPoriD == null)
            {
                // Assim como no método Atualizar, a mensagem deixa claro que o ID foi usado na busca.
                throw new Exception($"Usuario com ID {id} não encontrado em nossa base de dados.");
            }

            // Remove o registro da memória. Ainda não foi apagado do banco até SaveChangesAsync.
            _dbcontext.Usuarios.Remove(usuarioPoriD);

            // Salva efetivamente as alterações no banco, garantindo a exclusão do registro.
            await _dbcontext.SaveChangesAsync();
            return true; // Indica que a exclusão foi concluída com sucesso.
        }
    }
}
