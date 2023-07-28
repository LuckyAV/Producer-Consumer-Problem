// Controllers/HomeController.cs
using Microsoft.AspNetCore.Mvc;
using Producer_Consumer_Problem.Views.Shared;

namespace Producer_Consumer_Problem.Services
{
    public class HomeController : Controller
    {
        private readonly SharedBuffer _buffer;

        public HomeController()
        {
            _buffer = new SharedBuffer(); 
        }

        public IActionResult Index()
        {
            // Create producer and consumer instances
            var producer = new Producer(_buffer);
            var consumer = new Consumer(_buffer);

            // Start producer and consumer threads
            var producerThread = new Thread(producer.Produce);
            var consumerThread = new Thread(consumer.Consume);

            producerThread.Start();
            consumerThread.Start();

            producerThread.Join();
            consumerThread.Join();

            return Content("Producer-Consumer process completed.");
        }
    }
}
