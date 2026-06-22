using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyberTime01
{
    // Classe de Modelo para a entidade Máquina.
    // Ela serve especificamente para estruturar os dados de cada terminal/estação em memória.
    internal class MaquinaModel
    {
        // Atributos privados para garantir o isolamento e encapsulamento dos dados brutos
        private int _numero;
        private string _especificacao;
        private bool _ocupada;

        // Propriedade pública para leitura e definição do identificador único numérico da máquina
        public int Numero
        {
            get { return _numero; }
            set { _numero = value; }
        }

        // Propriedade pública que encapsula a string com a descrição técnica do hardware do terminal
        public string CombinacaoEspecificacao
        {
            get { return _especificacao; }
            set { _especificacao = value; }
        }

        // Propriedade pública booleana para verificar ou alterar instantaneamente o estado de uso do terminal
        public bool Ocupada
        {
            get { return _ocupada; }
            set { _ocupada = value; }
        }

        // Construtor parametrizado completo, ideal para quando os dados já vêm prontos das camadas de persistência ou controle
        public MaquinaModel(int numero, string especificacao, bool ocupada)
        {
            this._numero = numero;
            this._especificacao = especificacao;
            this._ocupada = ocupada;
        }

        // Construtor padrão sem parâmetros, obrigatório para atender à restrição 'new()' definida no nosso BaseCRUD genérico
        public MaquinaModel()
        {
            this._numero = 0;
            this._especificacao = "";
            this._ocupada = false;
        }
    }
}