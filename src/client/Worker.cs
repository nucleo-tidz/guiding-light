using infrastructure.Service;

namespace client
{
    public class Worker(IExpertService pastorService) : BackgroundService
    {
        

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Console.WriteLine("Eneter your user name");
            string username = Console.ReadLine();

            Console.WriteLine("Choose a session name");
            string sessionname = Console.ReadLine();

            Console.WriteLine("Hello I am your personal pastor , please start by asking question");
            while (!stoppingToken.IsCancellationRequested)
            {
                string query = Console.ReadLine();
                Console.WriteLine(await pastorService.GetReponse(query, username,sessionname));
                Console.WriteLine("\n **************************************");
            }
        }
    }
}
