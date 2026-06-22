using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyberTime01
{
    // Classe principal de inicialização que serve como ponto de entrada (Entry Point) do software.
    // Aqui eu configuro o ciclo de vida da aplicação, instancio o motor de telas e gerencio o menu mestre.
    internal class Program
    {
        static void Main(string[] args)
        {
            // Configuração da identidade visual padrão do sistema (Fundo preto com texto verde estilo terminal hacker/cyber)
            ConsoleColor fundo = ConsoleColor.Black;
            ConsoleColor texto = ConsoleColor.DarkGreen;

            // Inicializa o objeto de tela principal responsável pela área de trabalho geral do console
            Tela minhaTela = new Tela(fundo, texto);

            // Acoplamento da camada View e Controller de Clientes para gerenciar o cadastro e saldos de horas
            ClienteView cliView = new ClienteView(fundo, texto);
            ClienteController cliController = new ClienteController(25, 4, cliView, 50, 12);

            // Acoplamento da camada View e Controller de Máquinas para gerenciar o inventário das estações físicas
            MaquinaView maqView = new MaquinaView(fundo, texto);
            MaquinaController maqController = new MaquinaController(25, 4, maqView, 50, 12);

            // Acoplamento da camada View e Controller de Sessões de uso cronometradas
            SessaoView sesView = new SessaoView(fundo, texto);

            // Injeção de dependência manual: Passo os controladores de clientes e máquinas para o gerenciador de sessões.
            // Isso é vital para permitir o relacionamento e cruzamento de dados (Ex: validar saldo e ocupar máquina em tempo real).
            SessaoController sesController = new SessaoController(25, 4, sesView, 50, 15, cliController, maqController);

            // Montagem dinâmica das opções textuais do Menu Principal da Lan House
            string opcao = "";
            List<string> oMenu = new List<string>();
            oMenu.Add("1 - Clientes        ");
            oMenu.Add("2 - Maquinas        ");
            oMenu.Add("3 - Sessoes         ");
            oMenu.Add("4 - Relatorio Atual "); // Renderiza o faturamento e sessões do dia corrente
            oMenu.Add("5 - Relatorio Mensal"); // Renderiza a consolidação completa do mês atual
            oMenu.Add("0 - Sair            ");

            // Loop principal do sistema (Game Loop / Message Loop) para manter o programa ativo até a ação de saída
            while (true)
            {
                // Limpa o buffer visual anterior e desenha a moldura de contorno mestre em toda a extensão da console (80x25)
                minhaTela.PrepararTela("CyberTime - Gerenciador de Lan House", 0, 0, 79, 24);

                // Renderiza as opções e captura a escolha informada pelo operador
                opcao = minhaTela.MostrarMenu(2, 2, oMenu);

                if (opcao == "0") // Encerramento seguro da aplicação
                {
                    Console.Clear();
                    Console.WriteLine("Sistema encerrado.");
                    break;
                }
                if (opcao == "1") // Dispara o subprocesso e tela de CRUD de Clientes
                {
                    cliController.CRUD();
                }
                if (opcao == "2") // Dispara o subprocesso e tela de CRUD de Máquinas
                {
                    maqController.CRUD();
                }
                if (opcao == "3") // Dispara o subprocesso de controle e abertura/fechamento de Sessões
                {
                    sesController.CRUD();
                }
                if (opcao == "4") // Renderiza na tela o relatório de movimentações diárias
                {
                    sesController.MostrarRelatorioAtual(minhaTela);
                }
                if (opcao == "5") // Renderiza na tela o relatório analítico consolidado do mês
                {
                    sesController.MostrarRelatorioMensal(minhaTela);
                }
            }
        }
    }
}