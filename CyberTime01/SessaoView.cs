using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyberTime01
{
    // Camada View especializada para a entidade Sessão.
    // Ela estende a classe genérica BaseView para gerenciar toda a interface gráfica e captura de entradas das sessões na console.
    internal class SessaoView : BaseView<SessaoModel>
    {
        // Construtores que repassam as configurações de cores de texto e fundo para a classe base
        public SessaoView(ConsoleColor cf, ConsoleColor ct) : base(cf, ct) { }
        public SessaoView() : base() { }

        // Desenha a estrutura de contorno do formulário na console e plota os labels dos campos linha por linha
        public override void ShowForm(int column, int row, int width, int height, List<string> fields)
        {
            this.MontarMoldura(column, row, column + width, row + height);
            int r = row + 1;

            // Centraliza o título superior do formulário dentro da moldura criada
            this.Centralizar(column, column + width, r, "Gerenciamento de Sessoes");
            r++;

            // Varre a lista de campos injetados pelo controlador para imprimir os rótulos verticalmente
            for (int i = 0; i < fields.Count; i++)
            {
                Console.SetCursorPosition(column + 1, r);
                Console.Write(fields[i]);
                r++;
            }
        }

        // Renderiza na tela os dados internos de uma sessão recuperada do arquivo
        public override void ShowData(int column, int row, SessaoModel objeto, List<string> fields)
        {
            // Calcula o recuo horizontal alinhando os valores com base no tamanho do primeiro label de campo
            int col = column + 1 + fields[0].Length;
            int r = row + 3;

            // Linha 3: Número da Máquina vinculada
            Console.SetCursorPosition(col, r);
            Console.Write(objeto.NumeroMaquina);

            // Linha 4: CPF do Cliente vinculado
            r++;
            Console.SetCursorPosition(col, r);
            Console.Write(objeto.CpfCliente);

            // Linha 5: Horário de Início formatado de maneira completa
            r++;
            Console.SetCursorPosition(col, r);
            Console.Write(objeto.Inicio.ToString("G"));

            // Linha 6: Avalia se existe horário de término ou se a sessão continua ativa
            r++;
            Console.SetCursorPosition(col, r);
            Console.Write(objeto.Fim.HasValue ? objeto.Fim.Value.ToString("G") : "Ativa");

            // Linha 7: Valor financeiro cobrado formatado com duas casas decimais
            r++;
            Console.SetCursorPosition(col, r);
            Console.Write(objeto.ValorCobrado.ToString("F2"));
        }

        // Centraliza a lógica de captura e validação de prompts de digitação do teclado para a entidade Sessão
        public override void EnterData(int column, int row, string which, SessaoModel model, List<string> fields, int width, int height)
        {
            int col = column + 1 + fields[0].Length;

            if (which == "PK") // Fluxo operacional de leitura de Chave Primária (ID da Sessão)
            {
                int r = row + 2;
                Console.SetCursorPosition(col, r);
                model.IdSessao = Console.ReadLine();
            }
            else // Fluxo operacional de leitura de dados complementares ("DT")
            {
                // Verifica se a sessão já existe (está ativa em andamento) para disparar a rotina de encerramento
                if (model.Inicio != DateTime.MinValue && model.Fim == null)
                {
                    int r = row + 6;
                    Console.SetCursorPosition(column + 1, r);
                    Console.Write("Deseja encerrar esta sessao agora? (S/N): ");

                    string resposta = Console.ReadLine()?.ToUpper();
                    if (resposta == "S")
                    {
                        // Registra o horário exato atual para o gatilho de cálculo de tarifas do controlador
                        model.Fim = DateTime.Now;
                    }
                }
                else // Fluxo de abertura e preenchimento de uma nova sessão de uso
                {
                    int r = row + 3;

                    // Limpa a área interna interna do formulário para evitar rastros textuais de pesquisas passadas
                    this.LimparArea(col, r, column + width - 2, r + height - 5);

                    // Captura e valida o identificador numérico do terminal de rede
                    Console.SetCursorPosition(col, r);
                    try { model.NumeroMaquina = Convert.ToInt32(Console.ReadLine()); } catch { model.NumeroMaquina = 0; }

                    // Captura o CPF do cliente que iniciará o acesso
                    r++;
                    Console.SetCursorPosition(col, r);
                    model.CpfCliente = Console.ReadLine();

                    // Define os parâmetros iniciais padrão para o ciclo de vida de uma nova sessão ativa
                    model.Inicio = DateTime.Now;
                    model.Fim = null;
                    model.ValorCobrado = 0;
                }
            }
        }
    }
}