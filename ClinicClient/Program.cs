using ClinicServiceNamespace;
using Grpc.Net.Client;
using static ClinicServiceNamespace.ClinicService;

namespace ClinicClient
{
    internal class Program
    {
        static void Main(string[] args)
        {
            AppContext.SetSwitch(
        "System.Net.Http.SocketsHttpHadler.Http2UnencryptedSupport", true);

            using var channal = GrpcChannel.ForAddress("http://localhost:5001");
            var clinicServiceClient = new ClinicServiceClient(channal);

            var createClientResponse = clinicServiceClient.CreateClient(new CreateClientRequest
            {
                Document = "DOC69 244",
                Firstname = "Александр",
                Patronymic = "Валериевич",
                Surname = "Колодинский"
            });

            if (createClientResponse.ErrCode == 0)
            {
                Console.WriteLine($"Client {createClientResponse.ClientId} created succesfully.");
            }
            else
            {
                Console.WriteLine($"Create client error.\nError Code: {createClientResponse.ErrCode}\nError message: {createClientResponse.ErrMessage}");
            }

            var getClientResponse = clinicServiceClient.GetClients(new GetClientsRequest());

            if (getClientResponse.ErrCode == 0)
            {
                Console.WriteLine($"Clients list\n==========");

                foreach(var client in getClientResponse.Clients)
                {
                    Console.WriteLine(client.ToString());
                }
            }
            else
            {
                Console.WriteLine($"Inner error {getClientResponse.ErrCode}\nError message: {getClientResponse.ErrMessage}");
            }

            Console.ReadLine();
        }
    }
}