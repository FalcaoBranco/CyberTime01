using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyberTime01
{
    // Camada View especializada para a entidade Cliente.
    internal class ClienteView : BaseView<ClienteModel>
    {
        public ClienteView(ConsoleColor cf, ConsoleColor ct) : base(cf, ct) { }
        public ClienteView() : base() { }

        // Desenha a estrutura de contorno do formulário na console e plota os labels
        public override void ShowForm(int column, int row, int width, int height, List<string> fields)
        {
            this.MontarMoldura(column, row, column + width, row + height);
            int r = row + 1;

            this.Centralizar(column, column + width, r, "Cadastro de Clientes");
            r++;

            for (int i = 0; i < fields.Count; i++)
            {
                Console.SetCursorPosition(column + 1, r);
                Console.Write(fields[i]);
                r++;
            }
        }

        // Renderiza na tela os dados internos de um cliente recuperado do arquivo
        public override void ShowData(int column, int row, ClienteModel objeto, List<string> fields)
        {
            int col = column + 1 + fields[0].Length;
            int r = row + 3;

            // Linha 3: Renderiza o Nome Completo do cliente
            Console.SetCursorPosition(col, r);
            Console.Write(objeto.Nome);

            // Linha 4: Renderiza o Saldo Pré-Pago diretamente formatado como moeda monetária
            r++;
            Console.SetCursorPosition(col, r);
            Console.Write($"R$ {objeto.SaldoPrePago:F2}");
        }

        // Centraliza a lógica de captura e validação de prompts de digitação do teclado
        public override void EnterData(int column, int row, string which, ClienteModel model, List<string> fields, int width, int height)
        {
            int col = column + 1 + fields[0].Length;

            if (which == "PK") // Fluxo operacional de leitura de Chave Primária (CPF)
            {
                int r = row + 2;
                Console.SetCursorPosition(col, r);
                model.Cpf = Console.ReadLine();
            }
            else // Fluxo operacional de leitura de dados complementares ("DT")
            {
                int r = row + 3;

                this.LimparArea(col, r, column + width - 2, r + height - 5);

                // Captura o Nome Completo do cliente
                Console.SetCursorPosition(col, r);
                model.Nome = Console.ReadLine();

                // Avança uma linha para processar a captura do Saldo Pré-Pago com conversão direta
                r++;
                Console.SetCursorPosition(col, r);
                try
                {
                    model.SaldoPrePago = Convert.ToDecimal(Console.ReadLine());
                }
                catch
                {
                    model.SaldoPrePago = 0.00m; // Proteção contra digitação inválida
                }
            }
        }
    }
}