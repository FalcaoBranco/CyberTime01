using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyberTime01
{
    internal class ClienteModel
    {
        private string _cpf;
        private string _nome;
        private decimal _saldoPrePago;

        public string Cpf
        {
            get { return _cpf; }
            set { _cpf = value; }
        }

        public string Nome
        {
            get { return _nome; }
            set { _nome = value; }
        }

        public decimal SaldoPrePago
        {
            get { return _saldoPrePago; }
            set { _saldoPrePago = value; }
        }

        public ClienteModel(string cpf, string nome, decimal saldoPrePago)
        {
            this._cpf = cpf;
            this._nome = nome;
            this._saldoPrePago = saldoPrePago;
        }

        public ClienteModel()
        {
            this._cpf = "";
            this._nome = "";
            this._saldoPrePago = 0;
        }
    }
}