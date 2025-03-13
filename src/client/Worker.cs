using infrastructure.Service;

namespace client
{
    public class Worker(IPastorService pastorService) : BackgroundService
    {
        

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Console.WriteLine("Hello I am your personal pastor , please start by asking question");
            while (!stoppingToken.IsCancellationRequested)
            {
                string query = Console.ReadLine();
                Console.WriteLine(await pastorService.GetReponse(query));
                Console.WriteLine("\n **************************************");
            }
        }
    }
}
