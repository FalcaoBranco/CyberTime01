using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyberTime01
{
    internal class MaquinaModel
    {
        private int _numero;
        private string _especificacao;
        private bool _ocupada;

        public int Numero
        {
            get { return _numero; }
            set { _numero = value; }
        }

        public string CombinacaoEspecificacao
        {
            get { return _especificacao; }
            set { _especificacao = value; }
        }

        public bool Ocupada
        {
            get { return _ocupada; }
            set { _ocupada = value; }
        }

        public MaquinaModel(int numero, string especificacao, bool ocupada)
        {
            this._numero = numero;
            this._especificacao = especificacao;
            this._ocupada = ocupada;
        }

        public MaquinaModel()
        {
            this._numero = 0;
            this._especificacao = "";
            this._ocupada = false;
        }
    }
}