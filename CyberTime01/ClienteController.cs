using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace CyberTime01
{
    // Controlador específico para gerenciar as regras de negócio e persistência de Clientes.
    internal class ClienteController : BaseCRUD<ClienteController, ClienteModel, ClienteView>
    {
        // Nome do arquivo de texto plano usado como base de dados física local
        private string arquivoDados = "clientes.txt";

        // Construtor principal da classe. Configura o posicionamento, as dimensões e carrega os registros do disco.
        public ClienteController(int col, int row, ClienteView viewInstance, int width, int height)
            : base(col, row, viewInstance)
        {
            this.width = width;
            this.height = height;
            this.LoadFields(); // Define a lista de rótulos textuais que serão exibidos nos formulários
            this.CarregarArquivo(); // Importa os dados persistidos em disco para a memória RAM

            // Regra de inicialização preventiva: Se a base iniciar completamente vazia, insere um cliente padrão para testes rápidos
            if (this.GetDatabase().Count == 0)
            {
                ClienteModel clientePadrao = new ClienteModel();
                clientePadrao.Cpf = "08915756509";
                clientePadrao.Nome = "Cliente Teste Padrao";
                clientePadrao.SaldoPrePago = 50.00m;

                this.GetDatabase().Add(clientePadrao); // Adiciona o cliente mockado na coleção
                this.SalvarArquivo(); // Persiste o novo estado de forma imediata no arquivo local
            }
        }

        // Método ocultador (new) para garantir o retorno da lista fortemente tipada (ClienteModel) sem ambiguidades com a classe base
        public new List<ClienteModel> GetDatabase()
        {
            return base.GetDatabase() as List<ClienteModel> ?? new List<ClienteModel>();
        }

        // Define o CPF como a chave primária de identificação unívoca para o mecanismo de buscas sequenciais
        protected override string GetKey(ClienteModel obj)
        {
            return obj.Cpf;
        }

        // Faz o mapeamento e cópia explícita de propriedades entre o buffer temporário da interface e o item definitivo da lista
        protected override void MapToDatabase(ClienteModel destination, ClienteModel source)
        {
            destination.Cpf = source.Cpf;
            destination.Nome = source.Nome;
            destination.SaldoPrePago = source.SaldoPrePago;
        }

        // Inicializa e insere a lista ordenada de strings correspondentes às perguntas do formulário
        protected override void LoadFields()
        {
            this.fields.Add("CPF do Cliente   : ");
            this.fields.Add("Nome do Cliente  : ");
            this.fields.Add("Saldo Pre-Pago   : ");
        }

        // Abre o arquivo local de texto e serializa linearmente toda a coleção em formato delimitado por ponto e vírgula
        public override void SalvarArquivo()
        {
            try
            {
                // Abre o StreamWriter com modo append desativado (false) para sobrescrever o arquivo antigo com dados atualizados
                using (StreamWriter sw = new StreamWriter(arquivoDados, false))
                {
                    var lista = this.GetDatabase();
                    for (int i = 0; i < lista.Count; i++)
                    {
                        sw.WriteLine($"{lista[i].Cpf};{lista[i].Nome};{lista[i].SaldoPrePago}");
                    }
                }
            }
            catch
            {
                // Proteção contra concorrência de ficheiros (I/O bloqueado pelo SO)
            }
        }

        // Executa a leitura física sequencial do arquivo e reconstrói as instâncias de objetos populando a coleção em memória
        public override void CarregarArquivo()
        {
            try
            {
                // Verifica a existência do arquivo para evitar exceções de arquivo não encontrado (FileNotFoundException)
                if (File.Exists(arquivoDados))
                {
                    var database = this.GetDatabase();
                    database.Clear(); // Limpa as instâncias antigas para prevenir duplicações induzidas de leitura
                    using (StreamReader sr = new StreamReader(arquivoDados))
                    {
                        string linha;
                        // Lê linha por linha de forma linear até atingir o fim do documento (null)
                        while ((linha = sr.ReadLine()) != null)
                        {
                            if (string.IsNullOrWhiteSpace(linha)) continue; // Salta linhas vazias residuais

                            string[] dados = linha.Split(';');
                            // Valida se o vetor resultante do split possui estritamente as 3 colunas estruturais
                            if (dados.Length == 3)
                            {
                                ClienteModel novo = new ClienteModel();
                                novo.Cpf = dados[0];
                                novo.Nome = dados[1];
                                novo.SaldoPrePago = Convert.ToDecimal(dados[2]); // Conversão direta para decimal para manter precisão financeira

                                database.Add(novo); // Registra a instância reconstruída na lista ativa
                            }
                        }
                    }
                }
            }
            catch
            {
                // Proteção contra erros de leitura física ou corrupção de estrutura de arquivos textuais
            }
        }

        // Intercepta e estende a chamada mestre de fluxos CRUD da tela para garantir a persistência imediata após inclusão/alteração/exclusão
        public override void CRUD()
        {
            base.CRUD(); // Dispara o motor nativo da tela base (BaseCRUD)
            this.SalvarArquivo(); // Executa o salvamento protetor e síncrono em disco
        }
    }
}