using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Middlewares
{
    //Posso definir uma classe que será o nosso middleware
    //Posso Definir um padrão que terá a estrutura da classe a seguir
    public class Middleware
    {
        private Func<IDictionary<string, object>, Task> _next;

        public Middleware(Func<IDictionary<string, object>, Task> next)
        {
            _next = next;
        }

        public async Task Invoke(IDictionary<string, object> dict)
        {
            //Como já foi dito um middlewere é um dicionário que carrega todas
            //as informações de uma requisição em um dicionario de dados
            //Este dicioário tem um padrão: owin + <<variável da requisição>>
            using (var sw = new StreamWriter((Stream)dict["owin.ResponseBody"]))
            {
                //Posso retornar algum dado (Task) para o usuário
                await sw.WriteAsync("Testando..");
            }

            //Para que as informações sejam repassadas para outro o middleware subsequente
            //Tenho que usar o invoke => Invocará o próximo middleware!!
            _next.Invoke(dict);
        }
    }
}
