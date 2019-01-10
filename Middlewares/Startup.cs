using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using System.Security.Claims;

// ==> ::Pacotes de instalação::

//Microsoft.Owin => Para rodar o pipeline do Owin

// => Para ficar escutando um hosting
//Microsoft.Owin.Host.HttpListener
//Microsoft.Owin.Hosting => Determinar

namespace Middlewares
{
    // Definindo uma variavel para reutilizá-la durante o pipeline do Owin
    using AppFunc = Func<IDictionary<string, object>, Task>;
    
    //Por padrão todo middleware deve ter uma classe Startup e um método Configuration
    public class Startup
    {
        // O que é um middleware?
        // Simples: Func<Dictionary<object, string>, task>
        public void Configuration(IAppBuilder builder)
        {
            //Como criar meu próprio middleware?

            //Posso criar um middleware apartir de uma classe
            builder.Use<Middleware>();

            //Posso definir usando a seguinte estrutura
            builder.Use(new Func<AppFunc, AppFunc>(Middleware2));
        }

        public AppFunc Middleware2(AppFunc next)
        {
            //Posso criar meu middleware de autenticação?
            //Sim!! Fazendo a seguinte estrutura
            AppFunc appFunc = async (IDictionary<string, object> dic) =>
            {
                //O namespace microsoft.owin tem a seguinte classe:
                //Classe muito ultilizada  no Asp.Net identity para carregar alguns dados
                //da requisição
                //Logo vamos usá-la para carregar um dic (dados da requisição)
                IOwinContext context = new OwinContext(dic);

                //Vamos definir alguma claims do usuario
                var identities = new List<Claim>()
                {
                    new Claim("nome", "Fábio Fonseca")
                };
                //Definir uma identidade para ele
                var identity = new ClaimsIdentity(identities, "Meu Tipo de Autenticao");

                //Colocala no principal da requisição (ver projeto sobre identity e principal)
                context.Authentication.User = new ClaimsPrincipal(identity);

                //Pronto estamos logado
                //Criamos um middleware de autenticação
                await context.Response.WriteAsync("Tentando 2");

                //Invocando o próximo middleware de autenticação
                await next.Invoke(dic);
            };

            return appFunc;
        }

        public AppFunc MiddleWare3(AppFunc next)
        {
            //Todo middleware é um dicionario que retorna um func 
            AppFunc appFunc = async (IDictionary<string, object> dic) =>
            {
                //podemos colocar em uma classe helper (OwinContext)
                var contexto = new OwinContext(dic);

                //Com esta classe fica mais fácil manipular as requisição(Melhor que usar
                //a classe Middleware criada anteriormente)
                await contexto.Response.WriteAsync("teste");

                //Repassando para os outros middlewares
                await next.Invoke(dic);
            };

            return appFunc;
        }
    }
}
