using API.DTOs.Accounts;
using API.Utilites.Handlers;
using Client.Contracts;
using Newtonsoft.Json;
using System.Text;

namespace Client.Repositories;

public class AccountRepository : GeneralRepository<RegisterDto, string>, IAccountRepository
{
    private readonly HttpClient _httpClient;
    private readonly string _request;
    public AccountRepository(string request = "accounts/") : base(request)
    {
        _request = request;
        _httpClient = new HttpClient()
        {
            BaseAddress = new Uri("https://localhost:7239/api/")
        };
    }

    public async Task<ResponseHandler<AccountRepository>> Register(RegisterDto entity)
    {
        ResponseHandler<AccountRepository> entityVM = null;
        StringContent content = new StringContent(JsonConvert.SerializeObject(entity), Encoding.UTF8, "application/json");
        using (var response = _httpClient.PostAsync(_request + "register", content).Result)
        {
            string apiResponse = await response.Content.ReadAsStringAsync();
            entityVM = JsonConvert.DeserializeObject<ResponseHandler<AccountRepository>>(apiResponse);
        }
        return entityVM;
    }

    public async Task<ResponseHandler<string>> Login(LoginDto login)
    {
        ResponseHandler<string> responseHandler = null;
        StringContent content = new StringContent(JsonConvert.SerializeObject(login), Encoding.UTF8, "application/json");
        using (var response = await _httpClient.PostAsync(_request + "login", content))
        {
            string apiResponse = await response.Content.ReadAsStringAsync();
            responseHandler = JsonConvert.DeserializeObject<ResponseHandler<string>>(apiResponse);
        }
        return responseHandler;
    }
}
