using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyberTime01
{
    // Camada View especializada para a entidade Máquina.
    // Ela estende a classe genérica BaseView para gerenciar toda a interação gráfica e captura de dados dos terminais na console.
    internal class MaquinaView : BaseView<MaquinaModel>
    {
        // Construtores que repassam as configurações de cores de texto e fundo para a classe base
        public MaquinaView(ConsoleColor cf, ConsoleColor ct) : base(cf, ct) { }
        public MaquinaView() : base() { }

        // Desenha a estrutura de contorno do formulário na console e plota os labels dos campos linha por linha
        public override void ShowForm(int column, int row, int width, int height, List<string> fields)
        {
            this.MontarMoldura(column, row, column + width, row + height);
            int r = row + 1;

            // Centraliza o título superior do formulário dentro da moldura criada
            this.Centralizar(column, column + width, r, "Cadastro de Maquinas");
            r++;

            // Varre a lista de campos injetados pelo controlador para imprimir os rótulos verticalmente
            for (int i = 0; i < fields.Count; i++)
            {
                Console.SetCursorPosition(column + 1, r);
                Console.Write(fields[i]);
                r++;
            }
        }

        // Renderiza na tela os dados internos de uma máquina recuperada do arquivo
        public override void ShowData(int column, int row, MaquinaModel objeto, List<string> fields)
        {
            // Calcula o recuo horizontal alinhando os valores com base no tamanho do primeiro label de campo
            int col = column + 1 + fields[0].Length;
            int r = row + 3;

            // Linha 3: Imprime a descrição de hardware/especificação técnica salva da máquina
            Console.SetCursorPosition(col, r);
            Console.Write(objeto.CombinacaoEspecificacao);

            // Linha 4: Avalia o estado booleano para imprimir de forma limpa e legível se está "Ocupada" ou "Livre"
            r++;
            Console.SetCursorPosition(col, r);
            Console.Write(objeto.Ocupada ? "Ocupada" : "Livre");
        }

        // Centraliza a lógica de captura e validação de prompts de digitação do teclado para a entidade Máquina
        public override void EnterData(int column, int row, string which, MaquinaModel model, List<string> fields, int width, int height)
        {
            int col = column + 1 + fields[0].Length;

            if (which == "PK") // Fluxo operacional de leitura de Chave Primária (Número da Máquina)
            {
                int r = row + 2;
                Console.SetCursorPosition(col, r);
                try
                {
                    // Tenta converter o input textual diretamente para o identificador numérico correspondente
                    model.Numero = Convert.ToInt32(Console.ReadLine());
                }
                catch
                {
                    // Se o operador digitar letras ou um valor inválido, define uma flag negativa para evitar crash por parse falho
                    model.Numero = -1;
                }
            }
            else // Fluxo operacional de leitura de dados complementares ("DT")
            {
                int r = row + 3;

                // Limpa a área interna interna do formulário para evitar rastros textuais de pesquisas passadas
                this.LimparArea(col, r, column + width - 2, r + height - 5);

                // Captura a descrição e especificação técnica geral do terminal
                Console.SetCursorPosition(col, r);
                model.CombinacaoEspecificacao = Console.ReadLine();

                // Avança uma linha para processar e atualizar o status de disponibilidade
                r++;
                Console.SetCursorPosition(col, r);
                try
                {
                    string resp = Console.ReadLine().ToUpper();
                    // Aceita tanto a letra "S" quanto a palavra explícita "OCUPADA" para ligar a flag lógica como verdadeira
                    model.Ocupada = (resp == "S" || resp == "OCUPADA");
                }
                catch
                {
                    // Tratamento padrão contra inputs nulos ou inesperados, assumindo estado padrão como falso (Livre)
                    model.Ocupada = false;
                }
            }
        }
    }
}