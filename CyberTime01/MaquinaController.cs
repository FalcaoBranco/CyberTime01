using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace CyberTime01
{
    // Controlador específico para gerenciar as regras e persistência das Máquinas (estações de trabalho).
    // Herda da nossa estrutura BaseCRUD genérica passando as classes corretas de controle, modelo e visão.
    internal class MaquinaController : BaseCRUD<MaquinaController, MaquinaModel, MaquinaView>
    {
        // Arquivo físico local onde a listagem de terminais fica salva
        private string arquivoDados = "maquinas.txt";

        // Construtor principal. Ajusta tamanho da tela, carrega os labels dos campos e trata a carga inicial.
        public MaquinaController(int col, int row, MaquinaView viewInstance, int width, int height)
            : base(col, row, viewInstance)
        {
            this.width = width;
            this.height = height;
            this.LoadFields(); // Define a estrutura de labels textuais da tela de máquinas

            // Se o arquivo não existir ou se a leitura retornar uma lista vazia, força a criação do registro padrão.
            if (!File.Exists(arquivoDados))
            {
                CriarDadosPadrao();
            }
            else
            {
                this.CarregarArquivo();
                if (this.GetDatabase().Count == 0)
                {
                    CriarDadosPadrao();
                }
            }
        }

        // Método exposto para permitir que o SessaoController acesse e faça o cruzamento de dados com a lista de terminais.
        // Utiliza o modificador 'new' para evitar conflitos com assinaturas genéricas da classe BaseCRUD pai.
        public new List<MaquinaModel> GetDatabase()
        {
            return base.GetDatabase() as List<MaquinaModel> ?? new List<MaquinaModel>();
        }

        // Define o Identificador Único Numérico convertido para string como a chave unívoca do motor CRUD
        protected override string GetKey(MaquinaModel obj)
        {
            return obj.Numero.ToString();
        }

        // Mapeia de forma segura os dados operacionais entre o objeto de origem e o destino do banco de dados
        protected override void MapToDatabase(MaquinaModel destination, MaquinaModel source)
        {
            destination.Numero = source.Numero;
            destination.CombinacaoEspecificacao = source.CombinacaoEspecificacao;
            destination.Ocupada = source.Ocupada;
        }

        // Configuração textual dos campos de cabeçalho que serão impressos nas janelas de formulários
        protected override void LoadFields()
        {
            this.fields.Add("Numero do Terminal : ");
            this.fields.Add("Especificacoes     : ");
            this.fields.Add("Status (Ocupada)   : ");
        }

        // Gera registros padrão de terminais para testes caso a aplicação inicie sem base física de dados
        private void CriarDadosPadrao()
        {
            var database = this.GetDatabase();
            for (int i = 1; i <= 5; i++)
            {
                MaquinaModel mq = new MaquinaModel();
                mq.Numero = i;
                mq.CombinacaoEspecificacao = $"PC {i} - Gamer Intel i7, 16GB, RTX 3060";
                mq.Ocupada = false;
                database.Add(mq);
            }
            this.SalvarArquivo(); // Sincroniza imediatamente o estado padrão com o disco rígido
        }

        // Executa a escrita e serialização sequencial da coleção de máquinas para o arquivo de texto plano
        public override void SalvarArquivo()
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(arquivoDados, false))
                {
                    var lista = this.GetDatabase();
                    for (int i = 0; i < lista.Count; i++)
                    {
                        sw.WriteLine($"{lista[i].Numero};{lista[i].CombinacaoEspecificacao};{lista[i].Ocupada}");
                    }
                }
            }
            catch
            {
                // Tratamento protetor contra travas em concorrências de gravação de arquivos de texto
            }
        }

        // Realiza o carregamento físico e faz a reconstrução orientada a objetos das máquinas registradas
        public override void CarregarArquivo()
        {
            try
            {
                if (File.Exists(arquivoDados))
                {
                    var database = this.GetDatabase();
                    database.Clear(); // Limpa instâncias em memória para prevenir duplicações induzidas de carga
                    using (StreamReader sr = new StreamReader(arquivoDados))
                    {
                        string linha;
                        while ((linha = sr.ReadLine()) != null)
                        {
                            if (string.IsNullOrWhiteSpace(linha)) continue;

                            string[] dados = linha.Split(';');
                            if (dados.Length == 3)
                            {
                                MaquinaModel novo = new MaquinaModel();
                                novo.Numero = Convert.ToInt32(dados[0]);
                                novo.CombinacaoEspecificacao = dados[1];
                                novo.Ocupada = Convert.ToBoolean(dados[2]);

                                database.Add(novo);
                            }
                        }
                    }
                }
            }
            catch
            {
                // Tratamento preventivo contra quebras ou falhas estruturais de leitura física
            }
        }

        // Extensão do método CRUD padrão que assegura a persistência e o fechamento estável dos arquivos texto em cada loop
        public override void CRUD()
        {
            base.CRUD();
            this.SalvarArquivo(); // Salva e fecha o arquivo texto garantindo a persistência do estado
        }
    }
}