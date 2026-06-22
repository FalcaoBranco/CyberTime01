using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyberTime01
{
    // Classe de Modelo para a entidade Sessao.
    // Ela serve especificamente para estruturar e reter em memória os dados das sessões de uso dos computadores.
    internal class SessaoModel
    {
        // Atributos privados para garantir o isolamento e encapsulamento dos dados operacionais da sessão
        private string _idSessao;
        private int _numeroMaquina;
        private string _cpfCliente;
        private DateTime _inicio;
        private DateTime? _fim; // Tipo anulável (nullable) que indica se a sessão ainda está ativa (null) ou já foi encerrada
        private decimal _valorCobrado;

        // Propriedade pública para leitura e definição do identificador único da sessão de uso
        public string IdSessao
        {
            get { return _idSessao; }
            set { _idSessao = value; }
        }

        // Propriedade pública para vincular a sessão ao número do terminal físico utilizado
        public int NumeroMaquina
        {
            get { return _numeroMaquina; }
            set { _numeroMaquina = value; }
        }

        // Propriedade pública para vincular a sessão ao CPF do cliente correspondente
        public string CpfCliente
        {
            get { return _cpfCliente; }
            set { _cpfCliente = value; }
        }

        // Propriedade pública para leitura e gravação da data e hora exata de início do acesso
        public DateTime Inicio
        {
            get { return _inicio; }
            set { _inicio = value; }
        }

        // Propriedade pública que controla a data e hora de encerramento da sessão
        public DateTime? Fim
        {
            get { return _fim; }
            set { _fim = value; }
        }

        // Propriedade pública para leitura e definição do valor financeiro total cobrado pelo período de uso
        public decimal ValorCobrado
        {
            get { return _valorCobrado; }
            set { _valorCobrado = value; }
        }

        // Construtor parametrizado completo, ideal para realizar a abertura e inicialização imediata de uma nova sessão ativa
        public SessaoModel(string idSessao, int numeroMaquina, string cpfCliente, DateTime inicio)
        {
            this._idSessao = idSessao;
            this._numeroMaquina = numeroMaquina;
            this._cpfCliente = cpfCliente;
            this._inicio = inicio;
            this._fim = null; // Toda sessão nova inicia com o tempo de encerramento em aberto
            this._valorCobrado = 0; // O valor financeiro permanece zerado até a execução do cálculo de encerramento
        }

        // Construtor padrão sem parâmetros, obrigatório para atender à restrição 'new()' do nosso motor BaseCRUD genérico
        public SessaoModel()
        {
            this._idSessao = "";
            this._numeroMaquina = 0;
            this._cpfCliente = "";
            this._inicio = DateTime.MinValue;
            this._fim = null;
            this._valorCobrado = 0;
        }
    }
}