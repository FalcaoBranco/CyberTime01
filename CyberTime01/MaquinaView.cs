using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyberTime01
{
    internal class MaquinaView : BaseView<MaquinaModel>
    {
        public MaquinaView(ConsoleColor cf, ConsoleColor ct) : base(cf, ct) { }
        public MaquinaView() : base() { }

        public override void ShowForm(int column, int row, int width, int height, List<string> fields)
        {
            this.MontarMoldura(column, row, column + width, row + height);

            int r = row + 1;

            this.Centralizar(column, column + width, r, "Cadastro de Maquinas");

            r++;

            for (int i = 0; i < fields.Count; i++)
            {
                Console.SetCursorPosition(column + 1, r);
                Console.Write(fields[i]);
                r++;
            }
        }

        public override void ShowData(int column, int row, MaquinaModel objeto, List<string> fields)
        {
            int col = column + 1 + fields[0].Length;
            int r = row + 3;

            Console.SetCursorPosition(col, r);
            Console.Write(objeto.CombinacaoEspecificacao);

            r++;
            Console.SetCursorPosition(col, r);
            Console.Write(objeto.Ocupada ? "Ocupada" : "Livre");
        }

        public override void EnterData(int column, int row, string which, MaquinaModel model, List<string> fields, int width, int height)
        {
            int col = column + 1 + fields[0].Length;

            if (which == "PK")
            {
                int r = row + 2;
                Console.SetCursorPosition(col, r);
                try
                {
                    model.Numero = Convert.ToInt32(Console.ReadLine());
                }
                catch
                {
                    model.Numero = -1;
                }
            }
            else
            {
                int r = row + 3;
                this.LimparArea(col, r, column + width - 2, r + height - 5);

                Console.SetCursorPosition(col, r);
                model.CombinacaoEspecificacao = Console.ReadLine();

                r++;
                Console.SetCursorPosition(col, r);
                try
                {
                    string resp = Console.ReadLine().ToUpper();
                    model.Ocupada = (resp == "S" || resp == "OCUPADA");
                }
                catch
                {
                    model.Ocupada = false;
                }
            }
        }
    }
}