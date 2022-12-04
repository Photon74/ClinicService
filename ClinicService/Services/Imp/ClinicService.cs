using ClinicService.Data;
using ClinicServiceNamespace;
using Grpc.Core;
using static ClinicServiceNamespace.ClinicService;

namespace ClinicService.Services.Imp
{
    public class ClinicService : ClinicServiceBase
    {
        private readonly ClinicServiceDbContext _dbContext;

        public ClinicService(ClinicServiceDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public override Task<CreateClientResponse> CreateClient(CreateClientRequest request, ServerCallContext context)
        {
            try
            {
                var client = new Client
                {
                    Document = request.Document,
                    FirstName = request.Firstname,
                    Surname = request.Surname,
                    Patronymic = request.Patronymic,
                };
                _dbContext.Clients.Add(client);
                _dbContext.SaveChanges();

                var response = new CreateClientResponse
                {
                    ClientId = client.Id,
                    ErrCode = 0,
                    ErrMessage = ""
                };

                return Task.FromResult(response);
            }
            catch (Exception e)
            {
                var response = new CreateClientResponse
                {
                    ErrCode = 1001,
                    ErrMessage = $"Error! {e.Message}"
                };

                return Task.FromResult(response);
            }
        }

        public override Task<GetClientsResponse> GetClients(GetClientsRequest request, ServerCallContext context)
        {
            try
            {
                var clients = _dbContext.Clients.Select(client =>
                    new ClientResponse
                    {
                        ClientId = client.Id,
                        Document = client.Document,
                        Firstname = client.FirstName,
                        Surname = client.Surname,
                        Patronymic = client.Patronymic,
                    }).ToList();

                var response = new GetClientsResponse()
                {
                    ErrCode = 0,
                    ErrMessage = ""
                };
                response.Clients.AddRange(clients);

                return Task.FromResult(response);
            }
            catch (Exception e)
            {
                var response = new GetClientsResponse
                {
                    ErrCode = 1002,
                    ErrMessage = $"Error! {e.Message}"
                };

                return Task.FromResult(response);
            }
        }
    }
}
