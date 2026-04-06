using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

using BarberRezende.Web.Models.Agendamentos;
using BarberRezende.Web.Models.Barbeiros;
using BarberRezende.Web.Models.Clientes;
using BarberRezende.Web.Models.Common;
using BarberRezende.Web.Models.Servicos;

namespace BarberRezende.Web.Services
{
    public class ApiClient
    {
        private readonly HttpClient _http;
        private readonly TokenSessionService _tokenSession;

        public ApiClient(HttpClient http, TokenSessionService tokenSession)
        {
            _http = http;
            _tokenSession = tokenSession;

            // 🔥 GARANTE QUE TEM BASE ADDRESS
            if (_http.BaseAddress == null)
                throw new InvalidOperationException("HttpClient sem BaseAddress. Verifique Api:BaseUrl no appsettings.");
        }

        // =========================================================
        // TOKEN
        // =========================================================

        private void AddAuthorizationHeader()
        {
            var token = _tokenSession.GetToken();

            if (!string.IsNullOrWhiteSpace(token)) {
                _http.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
            }
            else {
                _http.DefaultRequestHeaders.Authorization = null;
            }
        }

        // =========================================================
        // CLIENTES
        // =========================================================

        public async Task<ApiResult<List<ClienteVm>>> GetClientesAsync()
            => await GetAsync<List<ClienteVm>>("/api/clientes");

        public async Task<ApiResult<ClienteVm>> GetClienteByIdAsync(int id)
            => await GetAsync<ClienteVm>($"/api/clientes/{id}");

        public async Task<ApiResult<object>> CreateClienteAsync(ClienteCreateVm vm)
            => await PostAsync("/api/clientes", vm);

        public async Task<ApiResult<object>> UpdateClienteAsync(int id, ClienteUpdateVm vm)
            => await PutAsync($"/api/clientes/{id}", vm);

        public async Task<ApiResult<object>> DeleteClienteAsync(int id)
            => await DeleteAsync($"/api/clientes/{id}");

        // =========================================================
        // BARBEIROS
        // =========================================================

        public async Task<ApiResult<List<BarbeiroVm>>> GetBarbeirosAsync()
            => await GetAsync<List<BarbeiroVm>>("/api/barbeiros");

        public async Task<ApiResult<BarbeiroVm>> GetBarbeiroByIdAsync(int id)
            => await GetAsync<BarbeiroVm>($"/api/barbeiros/{id}");

        public async Task<ApiResult<object>> CreateBarbeiroAsync(BarbeiroVm vm)
            => await PostAsync("/api/barbeiros", vm);

        public async Task<ApiResult<object>> UpdateBarbeiroAsync(int id, BarbeiroVm vm)
            => await PutAsync($"/api/barbeiros/{id}", vm);

        public async Task<ApiResult<object>> DeleteBarbeiroAsync(int id)
            => await DeleteAsync($"/api/barbeiros/{id}");


        // =========================================================
        // SERVIÇOS
        // =========================================================

        public async Task<ApiResult<List<ServicoVm>>> GetServicosAsync()
            => await GetAsync<List<ServicoVm>>("/api/servicos");

        public async Task<ApiResult<ServicoVm>> GetServicoByIdAsync(int id)
            => await GetAsync<ServicoVm>($"/api/servicos/{id}");

        public async Task<ApiResult<object>> CreateServicoAsync(ServicoCreateVm vm)
            => await PostAsync("/api/servicos", vm);

        public async Task<ApiResult<object>> UpdateServicoAsync(int id, ServicoUpdateVm vm)
            => await PutAsync($"/api/servicos/{id}", vm);

        public async Task<ApiResult<object>> DeleteServicoAsync(int id)
            => await DeleteAsync($"/api/servicos/{id}");

        // =========================================================
        // AGENDAMENTOS
        // =========================================================

        public async Task<ApiResult<List<AgendamentoListItemVm>>> GetAgendamentosAsync()
            => await GetAsync<List<AgendamentoListItemVm>>("/api/agendamentos");

        public async Task<ApiResult<object>> CreateAgendamentoAsync(AgendamentoCreateVm form)
            => await PostAsync("/api/agendamentos", form);

        public async Task<ApiResult<AgendamentoUpdateVm>> GetAgendamentoByIdAsync(int id)
            => await GetAsync<AgendamentoUpdateVm>($"/api/agendamentos/{id}");

        public async Task<ApiResult<object>> UpdateAgendamentoAsync(int id, AgendamentoUpdateVm form)
            => await PutAsync($"/api/agendamentos/{id}", form);

        public async Task<ApiResult<object>> DeleteAgendamentoAsync(int id)
            => await DeleteAsync($"/api/agendamentos/{id}");

        // =========================================================
        // OPTIONS
        // =========================================================

        public async Task<ApiResult<List<SimpleOptionVm>>> GetClientesOptionsAsync()
        {
            var res = await GetClientesAsync();
            if (!res.Success)
                return ApiResult<List<SimpleOptionVm>>.Fail(res.FriendlyMessage!, res.StatusCode);

            var opts = (res.Data ?? new List<ClienteVm>())
                .OrderBy(c => c.Nome)
                .Select(c => new SimpleOptionVm { Id = c.Id, Text = c.Nome ?? $"Cliente #{c.Id}" })
                .ToList();

            return ApiResult<List<SimpleOptionVm>>.Ok(opts, 200);
        }

        public async Task<ApiResult<List<SimpleOptionVm>>> GetBarbeirosOptionsAsync()
        {
            var res = await GetBarbeirosAsync();

            if (!res.Success)
                return ApiResult<List<SimpleOptionVm>>.Fail(res.FriendlyMessage!, res.StatusCode);

            var opts = (res.Data ?? new List<BarbeiroVm>())
                .OrderBy(b => b.Nome)
                .Select(b => new SimpleOptionVm { Id = b.Id, Text = b.Nome ?? $"Barbeiro #{b.Id}" })
                .ToList();

            return ApiResult<List<SimpleOptionVm>>.Ok(opts, 200);
        }

        public async Task<ApiResult<List<SimpleOptionVm>>> GetServicosOptionsAsync()
        {
            var res = await GetServicosAsync();

            if (!res.Success)
                return ApiResult<List<SimpleOptionVm>>.Fail(res.FriendlyMessage!, res.StatusCode);

            var opts = (res.Data ?? new List<ServicoVm>())
                .OrderBy(s => s.NomeServico)
                .Select(s => new SimpleOptionVm { Id = s.Id, Text = s.NomeServico ?? $"Serviço #{s.Id}" })
                .ToList();

            return ApiResult<List<SimpleOptionVm>>.Ok(opts, 200);
        }

        // =========================================================
        // HTTP BASE
        // =========================================================

        private async Task<ApiResult<T>> GetAsync<T>(string url)
        {
            try {
                AddAuthorizationHeader();

                var response = await _http.GetAsync(url);

                if (response.IsSuccessStatusCode) {
                    var data = await response.Content.ReadFromJsonAsync<T>();
                    return ApiResult<T>.Ok(data!, (int)response.StatusCode);
                }

                return await ApiResult<T>.FromErrorResponse(response);
            }
            catch (HttpRequestException ex) {
                return ApiResult<T>.Fail($"Falha ao conectar na API: {ex.Message}", 0);
            }
            catch (Exception ex) {
                return ApiResult<T>.Fail($"Erro interno no cliente HTTP: {ex.Message}", 0);
            }
        }

        private async Task<ApiResult<object>> PostAsync<TBody>(string url, TBody body)
        {
            try {
                AddAuthorizationHeader();

                var response = await _http.PostAsJsonAsync(url, body);

                if (response.IsSuccessStatusCode)
                    return ApiResult<object>.Ok(new object(), (int)response.StatusCode);

                return await ApiResult<object>.FromErrorResponse(response);
            }
            catch (Exception ex) {
                return ApiResult<object>.Fail($"Erro ao conectar: {ex.Message}", 0);
            }
        }

        private async Task<ApiResult<object>> PutAsync<TBody>(string url, TBody body)
        {
            try {
                AddAuthorizationHeader();

                var response = await _http.PutAsJsonAsync(url, body);

                if (response.IsSuccessStatusCode)
                    return ApiResult<object>.Ok(new object(), (int)response.StatusCode);

                return await ApiResult<object>.FromErrorResponse(response);
            }
            catch (Exception ex) {
                return ApiResult<object>.Fail($"Erro ao conectar: {ex.Message}", 0);
            }
        }

        private async Task<ApiResult<object>> DeleteAsync(string url)
        {
            try {
                AddAuthorizationHeader();

                var response = await _http.DeleteAsync(url);

                if (response.IsSuccessStatusCode)
                    return ApiResult<object>.Ok(new object(), (int)response.StatusCode);

                return await ApiResult<object>.FromErrorResponse(response);
            }
            catch (Exception ex) {
                return ApiResult<object>.Fail($"Erro ao conectar: {ex.Message}", 0);
            }
        }
    }

    // =========================================================
    // RESULT
    // =========================================================

    public class ApiResult<T>
    {
        public bool Success { get; private set; }
        public T? Data { get; private set; }
        public string? FriendlyMessage { get; private set; }
        public int StatusCode { get; private set; }

        private ApiResult() { }

        public static ApiResult<T> Ok(T data, int statusCode)
            => new ApiResult<T> { Success = true, Data = data, StatusCode = statusCode };

        public static ApiResult<T> Fail(string message, int statusCode)
            => new ApiResult<T> { Success = false, FriendlyMessage = message, StatusCode = statusCode };

        public static async Task<ApiResult<T>> FromErrorResponse(HttpResponseMessage response)
        {
            var status = (int)response.StatusCode;

            if (response.StatusCode == HttpStatusCode.NotFound)
                return Fail("Recurso não encontrado.", status);

            if (response.StatusCode == HttpStatusCode.BadRequest)
                return Fail("Dados inválidos.", status);

            if (response.StatusCode == HttpStatusCode.Conflict)
                return Fail("Conflito ao executar operação.", status);

            var body = await response.Content.ReadAsStringAsync();

            return Fail(!string.IsNullOrWhiteSpace(body) ? body : "Erro na API.", status);
        }
    }
}