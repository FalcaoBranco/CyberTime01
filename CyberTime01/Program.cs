using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyberTime01
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ConsoleColor fundo = ConsoleColor.Black;
            ConsoleColor texto = ConsoleColor.White;

            Tela minhaTela = new Tela(fundo, texto);

            ClienteView cliView = new ClienteView(fundo, texto);
            ClienteController cliController = new ClienteController(25, 4, cliView, 50, 12);

            MaquinaView maqView = new MaquinaView(fundo, texto);
            MaquinaController maqController = new MaquinaController(25, 4, maqView, 50, 12);

            SessaoView sesView = new SessaoView(fundo, texto);
            SessaoController sesController = new SessaoController(25, 4, sesView, 50, 15);

            string opcao = "";
            List<string> oMenu = new List<string>();
            oMenu.Add("1 - Clientes   ");
            oMenu.Add("2 - Maquinas   ");
            oMenu.Add("3 - Sessoes    ");
            oMenu.Add("0 - Sair       ");

            while (true)
            {
                minhaTela.PrepararTela("CyberTime - Gerenciador de Lan House", 0, 0, 79, 24);
                opcao = minhaTela.MostrarMenu(2, 2, oMenu);

                if (opcao == "0")
                {
                    Console.Clear();
                    Console.WriteLine("Sistema encerrado.");
                    break;
                }
                if (opcao == "1")
                {
                    cliController.CRUD();
                }
                if (opcao == "2")
                {
                    maqController.CRUD();
                }
                if (opcao == "3")
                {
                    sesController.CRUD();
                }
            }
        }
    }
}