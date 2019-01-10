using Microsoft.Owin.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Middlewares
{
    public class Program
    {
        static void Main(string[] args)
        {
            //Posso definir mapear url e uma porta para ficar escutando todas
            //as requisições vinda desta.
            using (WebApp.Start<Startup>("http://localhost:1234"))
            {
                Console.ReadKey();
            }
        }
    }
}
