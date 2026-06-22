using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyberTime01
{
    public class Tela
    {
        
        private ConsoleColor _corFundo;
        private ConsoleColor _corTexto;




        
        public Tela(ConsoleColor cf, ConsoleColor ct)
        {
            this._corFundo = cf;
            this._corTexto = ct;
        }

       
        public Tela() { }




        
        public void PrepararTela(string titulo, int ci, int li, int cf, int lf)
        {
            Console.BackgroundColor = this._corFundo;
            Console.ForegroundColor = this._corTexto;
            Console.Clear();
            this.MontarMoldura(ci, li, cf, lf);
            this.Centralizar(ci, cf, li + 1, titulo);
        }

        
        public void Centralizar(int ci, int cf, int linha, string texto)
        {
            int coluna = ci + ((cf - ci - texto.Length) / 2);
            Console.SetCursorPosition(coluna, linha);
            Console.Write(texto);
        }


       
        public string Perguntar(string pergunta, int linha, int ci, int cf)
        {
            string resp;
            this.LimparArea(ci, linha, cf, linha);
            Console.SetCursorPosition(ci, linha);
            Console.Write(pergunta);
            resp = Console.ReadLine();
            return resp.ToLower();
        }

        
        public void LimparArea(int ci, int li, int cf, int lf)
        {
            for (int x = ci; x <= cf; x++)
            {
                for (int y = li; y <= lf; y++)
                {
                    Console.SetCursorPosition(x, y);
                    Console.Write(" ");
                }
            }
        }

       
        public void MontarMoldura(int ci, int li, int cf, int lf)
        {
            int linha, coluna;

           
            this.LimparArea(ci, li, cf, lf);

            
            for (coluna = ci; coluna <= cf; coluna++)
            {
                Console.SetCursorPosition(coluna, li);
                Console.Write('═');

                Console.SetCursorPosition(coluna, lf);
                Console.Write('═');
            }

           
            for (linha = li; linha <= lf; linha++)
            {
                Console.SetCursorPosition(ci, linha);
                Console.Write("║");

                Console.SetCursorPosition(cf, linha);
                Console.Write("║"); 
            }

            
            Console.SetCursorPosition(ci, li);
            Console.Write('╔'); 

            
            Console.SetCursorPosition(cf, li);
            Console.Write('╗');

            
            Console.SetCursorPosition(ci, lf);
            Console.Write('╚'); 

            
            Console.SetCursorPosition(cf, lf);
            Console.Write('╝'); 
        }


        public string MostrarMenu(int colini, int linini, List<string> opcoes)
        {
            string op;
            int x;
            int colfin = colini + opcoes[0].Length + 1;
            int linfin = linini + opcoes.Count() + 2;

            this.MontarMoldura(colini, linini, colfin, linfin);
            for (x = 0; x < opcoes.Count(); x++)
            {
                Console.SetCursorPosition(colini + 1, linini + 1 + x);
                Console.Write(opcoes[x]);
            }
            Console.SetCursorPosition(colini + 1, linini + 1 + x);
            Console.Write("Opção : ");
            op = Console.ReadLine();

            return op;
        }




    }
}
