using System.Collections.Generic;
using System.Threading.Tasks;
using CRUDAPI.Models;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CRUDAPI.Controllers {
    [ApiController]
    [Route ("api/[controller]")]
    public class PessoasController : ControllerBase {
        private readonly Contexto _contexto;

        public PessoasController (Contexto contexto) {
            _contexto = contexto;
        }
        #region "GetAllPersons"
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Pessoa>>> PegarTodosAsync()
        {
            try
            {
                var pessoas = await _contexto.Pessoas.ToListAsync();
                return Ok(pessoas);
            }
            catch (Exception ex)
            {
                // Tratamento de erro genérico
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro ao buscar as pessoas. Detalhes: " + ex.Message);
            }
        }
        #endregion
        #region "GetPersonId"
        [HttpGet("{pessoaId}")]
        public async Task<ActionResult<Pessoa>> PegarPessoaPeloIdAsync(int pessoaId)
        {
            try
            {
                var pessoa = await _contexto.Pessoas.FindAsync(pessoaId);

                if (pessoa == null)
                    return NotFound();

                return Ok(pessoa);
            }
            catch (Exception ex)
            {
                // Tratamento de erro genérico
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro ao buscar a pessoa. Detalhes: " + ex.Message);
            }
        }
        #endregion
        #region "CreatePerson"
        [HttpPost]
        public async Task<ActionResult<Pessoa>> SalvarPessoaAsync(Pessoa pessoa)
        {
            try
            {
                if (pessoa == null)
                    return BadRequest("Dados inválidos. Nenhuma pessoa fornecida.");

                await _contexto.Pessoas.AddAsync(pessoa);
                await _contexto.SaveChangesAsync();

                return Ok(pessoa);
            }
            catch (Exception ex)
            {
                // Tratamento de erro genérico
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro durante a criação da pessoa. Detalhes: " + ex.Message);
            }
        }
        #endregion
        #region "UpdatePersonId"
        [HttpPut]
        public async Task<ActionResult> AtualizarPessoaAsync(Pessoa pessoa)
        {
            try
            {
                _contexto.Pessoas.Update(pessoa);
                await _contexto.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                // Tratamento de erro genérico
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro ao atualizar a pessoa. Detalhes: " + ex.Message);
            }
        }
        #endregion
        #region "DeletePersonId"

        [HttpDelete("{pessoaId}")]
        public async Task<ActionResult> ExcluirPessoaAsync(int pessoaId)
        {
            try
            {
                var pessoa = await _contexto.Pessoas.FindAsync(pessoaId);
                if (pessoa == null)
                    return NotFound();

                _contexto.Remove(pessoa);
                await _contexto.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                // Tratamento de erro genérico
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro ao excluir a pessoa. Detalhes: " + ex.Message);
            }
        }
        #endregion

        
    }
    [ApiController]
    [Route("api/cep")]
    public class CepController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public CepController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet("{cep}")]
        public async Task<IActionResult> Get(string cep)
        {
            try
            {
                // Cria uma instância do HttpClient utilizando a factory
                var httpClient = _httpClientFactory.CreateClient();

                // Faz a requisição GET para a API ViaCEP
                var response = await httpClient.GetAsync($"https://viacep.com.br/ws/{cep}/json/");

                // Verifica se a requisição foi bem-sucedida
                if (response.IsSuccessStatusCode)
                {
                    // Lê o conteúdo da resposta
                    var content = await response.Content.ReadAsStringAsync();

                    // Aqui você pode utilizar uma biblioteca JSON para desserializar o conteúdo e obter os dados desejados
                    // Neste exemplo, vamos apenas retornar o conteúdo da resposta como uma string
                    return Ok(content);
                }
                else
                {
                    // A requisição não foi bem-sucedida, retorna um erro
                    return StatusCode((int)response.StatusCode);
                }
            }
            catch (HttpRequestException ex)
            {
                // Ocorreu um erro na requisição, retorna um erro
                return BadRequest(ex.Message);
            }
        }
    }
}