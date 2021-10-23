using System.Threading.Tasks;

namespace WebScraper
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var program = new Startup();
            await program.Run();
        }
    }
}
