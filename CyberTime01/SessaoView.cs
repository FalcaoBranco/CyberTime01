using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyberTime01
{
    internal class SessaoView : BaseView<SessaoModel>
    {
        public SessaoView(ConsoleColor cf, ConsoleColor ct) : base(cf, ct) { }
        public SessaoView() : base() { }

        public override void ShowForm(int column, int row, int width, int height, List<string> fields)
        {
            this.MontarMoldura(column, row, column + width, row + height);
            int r = row + 1;
            this.Centralizar(column, column + width, r, "Gerenciamento de Sessoes");
            r++;
            for (int i = 0; i < fields.Count; i++)
            {
                Console.SetCursorPosition(column + 1, r);
                Console.Write(fields[i]);
                r++;
            }
        }

        public override void ShowData(int column, int row, SessaoModel objeto, List<string> fields)
        {
            int col = column + 1 + fields[0].Length;
            int r = row + 3;

            Console.SetCursorPosition(col, r);
            Console.Write(objeto.NumeroMaquina);

            r++;
            Console.SetCursorPosition(col, r);
            Console.Write(objeto.CpfCliente);

            r++;
            Console.SetCursorPosition(col, r);
            Console.Write(objeto.Inicio.ToString("G"));

            r++;
            Console.SetCursorPosition(col, r);
            Console.Write(objeto.Fim.HasValue ? objeto.Fim.Value.ToString("G") : "Ativa");

            r++;
            Console.SetCursorPosition(col, r);
            Console.Write(objeto.ValorCobrado.ToString("F2"));
        }

        public override void EnterData(int column, int row, string which, SessaoModel model, List<string> fields, int width, int height)
        {
            int col = column + 1 + fields[0].Length;

            if (which == "PK")
            {
                int r = row + 2;
                Console.SetCursorPosition(col, r);
                model.IdSessao = Console.ReadLine();
            }
            else
            {
                int r = row + 3;
                this.LimparArea(col, r, column + width - 2, r + height - 5);

                Console.SetCursorPosition(col, r);
                try { model.NumeroMaquina = Convert.ToInt32(Console.ReadLine()); } catch { model.NumeroMaquina = 0; }

                r++;
                Console.SetCursorPosition(col, r);
                model.CpfCliente = Console.ReadLine();

                model.Inicio = DateTime.Now;
                model.Fim = null;
                model.ValorCobrado = 0;
            }
        }
    }
}