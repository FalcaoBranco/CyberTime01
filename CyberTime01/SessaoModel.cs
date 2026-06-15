using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyberTime01
{
    internal class SessaoModel
    {
        private string _idSessao;
        private int _numeroMaquina;
        private string _cpfCliente;
        private DateTime _inicio;
        private DateTime? _fim;
        private decimal _valorCobrado;

        public string IdSessao
        {
            get { return _idSessao; }
            set { _idSessao = value; }
        }

        public int NumeroMaquina
        {
            get { return _numeroMaquina; }
            set { _numeroMaquina = value; }
        }

        public string CpfCliente
        {
            get { return _cpfCliente; }
            set { _cpfCliente = value; }
        }

        public DateTime Inicio
        {
            get { return _inicio; }
            set { _inicio = value; }
        }

        public DateTime? Fim
        {
            get { return _fim; }
            set { _fim = value; }
        }

        public decimal ValorCobrado
        {
            get { return _valorCobrado; }
            set { _valorCobrado = value; }
        }

        public SessaoModel(string idSessao, int numeroMaquina, string cpfCliente, DateTime inicio)
        {
            this._idSessao = idSessao;
            this._numeroMaquina = numeroMaquina;
            this._cpfCliente = cpfCliente;
            this._inicio = inicio;
            this._fim = null;
            this._valorCobrado = 0;
        }

        public SessaoModel()
        {
            this._idSessao = "";
            this._numeroMaquina = 0;
            this._cpfCliente = "";
            this._inicio = DateTime.Now;
            this._fim = null;
            this._valorCobrado = 0;
        }
    }
}