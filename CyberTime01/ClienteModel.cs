using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyberTime01
{
    // Classe de modelo que define as propriedades estruturais da entidade Cliente.
    internal class ClienteModel
    {
        // Chave primária unívoca identificadora do cliente no sistema (CPF)
        public string Cpf { get; set; }

        // Nome completo do cliente cadastrado
        public string Nome { get; set; }

        // Tipo alterado para decimal para armazenar os créditos monetários com precisão
        public decimal SaldoPrePago { get; set; }

        // Construtor padrão para inicializar o objeto e prevenir erros de referência nula
        public ClienteModel()
        {
            this.Cpf = string.Empty;
            this.Nome = string.Empty;
            this.SaldoPrePago = 0.00m;
        }
    }
}