using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using SistemaTarefas.Data;
using SistemaTarefas.Models;
using System.Security.Cryptography.Xml;

namespace SistemaTarefas.Repositorios.Interfaces
{
    public class TarefaRepositorio : ITarefaRepositorio
    {

        private readonly SistemaTarefasDbContext _dbContext;
        public TarefaRepositorio(SistemaTarefasDbContext sistemaTarefaRepositoriodbContext)
        {
            _dbContext = sistemaTarefaRepositoriodbContext;
        }


        public async Task<TarefaModel> Adicionar(TarefaModel tarefa)
        {
            await  _dbContext.Tarefas.AddAsync(tarefa);
            await _dbContext.SaveChangesAsync();
            return tarefa;
        }

        public async Task<TarefaModel> Atualizar(TarefaModel tarefa, int id)
        {
            TarefaModel tarefaPorId = await BuscarPorId(id);

            tarefaPorId.Nome = tarefa.Nome;
            tarefaPorId.Descricao = tarefa.Descricao;
            tarefaPorId.Status = tarefa.Status;
            tarefaPorId.UsuarioId = tarefa.UsuarioId;

            _dbContext.Tarefas.Update(tarefaPorId);
            await _dbContext.SaveChangesAsync();

            return tarefaPorId;
        }

        public async Task<TarefaModel> BuscarPorId(int id)
        {
           var tarefaPorId = await _dbContext.Tarefas
                //Vai trazer os dados do usuário caso esteja vinculado a uma tarefa.
                .Include(x => x.Usuario)
                //Traz o primeiro registro encontrado com o parametro dado
                .FirstOrDefaultAsync(x => x.Id == id);
            if (tarefaPorId == null)
            {
                throw new Exception($"Tarefa com o ID {id} não foi encontrada.");
            }
            return (tarefaPorId);
        }

        public async Task<List<TarefaModel>> BuscarTodasTarefas()
        {
            return await _dbContext.Tarefas
                .Include(x => x.Usuario)
                .ToListAsync();
        }

        public async Task<bool> Apagar(int id)
        {
            TarefaModel tarefaId = await BuscarPorId(id);

            if (tarefaId == null)
            {
                throw new Exception($"usuario para o id {id} não encontrado");
            }

            _dbContext.Tarefas.Remove(tarefaId);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}
