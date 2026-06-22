using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace CyberTime01
{
    // Controlador específico para gerenciar as regras de negócio de Sessões de uso na Lan House.
    // Herda da nossa estrutura BaseCRUD genérica passando as classes corretas de controle, modelo e visão.
    internal class SessaoController : BaseCRUD<SessaoController, SessaoModel, SessaoView>
    {
        // Nome do arquivo de texto local onde a listagem de sessões fica armazenada
        private string arquivoDados = "sessoes.txt";

        // Referências internas injetadas para permitir a integridade e o cruzamento de dados relacionais
        private ClienteController _clienteCtrl;
        private MaquinaController _maquinaCtrl;

        // Construtor principal. Inicializa referências externas, monta rótulos e gerencia a carga inicial em memória.
        public SessaoController(int col, int row, SessaoView viewInstance, int width, int height, ClienteController cliCtrl, MaquinaController maqCtrl)
            : base(col, row, viewInstance)
        {
            this.width = width;
            this.height = height;
            this._clienteCtrl = cliCtrl;
            this._maquinaCtrl = maqCtrl;
            this.LoadFields(); // Configura os rótulos textuais específicos para os formulários de sessões
            this.CarregarArquivo(); // Importa os registros existentes do disco rígido

            // Regra de negócio: Se o repositório iniciar zerado, injeta um registro inicial simulado para testes rápidos
            if (this.GetDatabase().Count == 0)
            {
                SessaoModel sessaoPadrao = new SessaoModel();
                sessaoPadrao.IdSessao = "1";
                sessaoPadrao.NumeroMaquina = 1;
                sessaoPadrao.CpfCliente = "08915756509";
                sessaoPadrao.Inicio = DateTime.Now.AddHours(-1);
                sessaoPadrao.Fim = null;
                sessaoPadrao.ValorCobrado = 0;

                this.GetDatabase().Add(sessaoPadrao);
                this.SalvarArquivo(); // Sincroniza imediatamente o estado inicial padrão com o disco
            }
        }

        // Define o Identificador Único Textual da sessão como a chave unívoca de busca do motor CRUD
        protected override string GetKey(SessaoModel obj)
        {
            return obj.IdSessao;
        }

        // Mapeia e transfere de forma segura os dados operacionais aplicando as regras financeiras e relacionais
        protected override void MapToDatabase(SessaoModel destination, SessaoModel source)
        {
            // Regra de Negócio: Se a sessão em andamento está sendo encerrada nesta operação (transição de nulo para data final)
            if (destination.Fim == null && source.Fim != null)
            {
                destination.Fim = source.Fim;

                // Calcula o intervalo de tempo transcorrido entre a abertura e o fechamento
                TimeSpan tempoDecorrido = destination.Fim.Value - destination.Inicio;
                double totalHoras = tempoDecorrido.TotalHours;

                // Arredonda a cobrança para cima para garantir frações de horas inteiras tarifadas
                int horasCobradas = (int)Math.Ceiling(totalHoras);

                // Garante a cobrança mínima de pelo menos 1 hora de uso no estabelecimento
                if (horasCobradas < 1) horasCobradas = 1;

                // Calcula o montante final aplicando o valor fixado por hora da Lan House
                destination.ValorCobrado = horasCobradas * 15.00m;

                // Cruzamento de dados relacional: Localiza e altera dinamicamente o status da máquina para Livre (false) via método seguro
                var maq = _maquinaCtrl.GetDatabase().FirstOrDefault(m => m.Numero == destination.NumeroMaquina);
                if (maq != null)
                {
                    maq.Ocupada = false;
                    _maquinaCtrl.SalvarArquivo(); // Persiste a liberação do terminal de rede
                }
            }
            else // Regra de Abertura: Fluxo operacional padrão de criação de nova sessão de uso
            {
                destination.IdSessao = source.IdSessao;
                destination.NumeroMaquina = source.NumeroMaquina;
                destination.CpfCliente = source.CpfCliente;
                destination.Inicio = source.Inicio;
                destination.Fim = source.Fim;
                destination.ValorCobrado = source.ValorCobrado;

                // Cruzamento de dados relacional: Localiza e altera o status do terminal associado para Ocupada (true) via método seguro
                var maq = _maquinaCtrl.GetDatabase().FirstOrDefault(m => m.Numero == source.NumeroMaquina);
                if (maq != null)
                {
                    maq.Ocupada = true;
                    _maquinaCtrl.SalvarArquivo(); // Persiste o bloqueio e ocupação do computador
                }
            }
        }

        // Configuração textual dos campos textuais de cabeçalho que serão impressos nas janelas de formulários
        protected override void LoadFields()
        {
            this.fields.Add("ID da Sessao    : ");
            this.fields.Add("Numero Maquina   : ");
            this.fields.Add("CPF do Cliente   : ");
            this.fields.Add("Horario Inicio   : ");
            this.fields.Add("Horario Fim      : ");
            this.fields.Add("Valor Cobrado    : ");
        }

        // Método público reintroduzido explicitamente usando 'new' para contornar restrições rígidas de herança do BaseCRUD
        public new List<SessaoModel> GetDatabase()
        {
            // Executa a conversão segura baseada na assinatura padrão da propriedade ou campo base do sistema
            return base.GetDatabase() as List<SessaoModel> ?? new List<SessaoModel>();
        }

        // Executa a serialização e gravação física das sessões no arquivo de texto local
        public override void SalvarArquivo()
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(arquivoDados, false))
                {
                    var lista = this.GetDatabase();
                    for (int i = 0; i < lista.Count; i++)
                    {
                        string dataFim = lista[i].Fim.HasValue ? lista[i].Fim.Value.ToString() : "null";
                        sw.WriteLine($"{lista[i].IdSessao};{lista[i].NumeroMaquina};{lista[i].CpfCliente};{lista[i].Inicio};{dataFim};{lista[i].ValorCobrado}");
                    }
                }
            }
            catch
            {
                // Proteção contra erros de arquivos concorrentes
            }
        }

        // Executa o carregamento em memória do arquivo físico txt mapeando os tipos primitivos de volta
        public override void CarregarArquivo()
        {
            try
            {
                if (File.Exists(arquivoDados))
                {
                    var database = this.GetDatabase();
                    database.Clear();
                    using (StreamReader sr = new StreamReader(arquivoDados))
                    {
                        string linha;
                        while ((linha = sr.ReadLine()) != null)
                        {
                            if (string.IsNullOrWhiteSpace(linha)) continue;

                            string[] dados = linha.Split(';');
                            if (dados.Length == 6)
                            {
                                SessaoModel novo = new SessaoModel();
                                novo.IdSessao = dados[0];
                                novo.NumeroMaquina = Convert.ToInt32(dados[1]);
                                novo.CpfCliente = dados[2];
                                novo.Inicio = Convert.ToDateTime(dados[3]);
                                novo.Fim = dados[4] == "null" ? (DateTime?)null : Convert.ToDateTime(dados[4]);
                                novo.ValorCobrado = Convert.ToDecimal(dados[5]);

                                database.Add(novo);
                            }
                        }
                    }
                }
            }
            catch
            {
                // Proteção preventiva contra erros de leitura física
            }
        }

        // Sobrescrita do ciclo CRUD principal para sincronizar os dados locais após chamadas do motor de telas
        public override void CRUD()
        {
            base.CRUD();
            this.SalvarArquivo();
        }

        // RELATÓRIO ATUAL: Varre e filtra estritamente as movimentações iniciadas na data corrente (Hoje)
        public void MostrarRelatorioAtual(Tela tela)
        {
            tela.PrepararTela("RELATÓRIO DIÁRIO (HOJE)", 0, 0, 79, 24);

            int linha = 4;
            Console.SetCursorPosition(2, linha);
            Console.Write("{0,-5} | {1,-15} | {2,-20} | {3,-10} | {4,-8}", "ID", "Cliente", "Máquina", "Status", "Total");
            Console.SetCursorPosition(2, ++linha);
            Console.Write(new string('-', 75));

            decimal faturamentoTotal = 0;
            DateTime hoje = DateTime.Today;

            // Filtra os registros buscando diretamente através do método seguro ajustado
            var sessoesHoje = this.GetDatabase().Where(s => s.Inicio.Date == hoje).ToList();

            foreach (var sessao in sessoesHoje)
            {
                linha++;
                if (linha > 20) break; // Trava de segurança para impedir estouro vertical de layout na console

                // Resgata as entidades vinculadas através de cruzamento de chaves usando métodos limpos de acesso
                var cliente = _clienteCtrl.GetDatabase().FirstOrDefault(c => c.Cpf == sessao.CpfCliente);
                var maquina = _maquinaCtrl.GetDatabase().FirstOrDefault(m => m.Numero == sessao.NumeroMaquina);

                // Formata strings longas limitando caracteres para manter o alinhamento da tabela visual intacto
                string nomeCliente = cliente != null ? (cliente.Nome.Length > 15 ? cliente.Nome.Substring(0, 12) + "..." : cliente.Nome) : "Não Encontrado";
                string descMaquina = maquina != null ? (maquina.CombinacaoEspecificacao.Length > 20 ? maquina.CombinacaoEspecificacao.Substring(0, 17) + "..." : maquina.CombinacaoEspecificacao) : "PC Removido";
                string status = sessao.Fim.HasValue ? "Finalizada" : "Ativa";

                Console.SetCursorPosition(2, linha);
                Console.Write("{0,-5} | {1,-15} | {2,-20} | {3,-10} | R${4,-6:F2}",
                    sessao.IdSessao, nomeCliente, descMaquina, status, sessao.ValorCobrado);

                faturamentoTotal += sessao.ValorCobrado;
            }

            // Exibe o rodapé consolidado com dados de faturamento bruto diário
            Console.SetCursorPosition(2, 22);
            Console.Write($"Faturamento do Dia: R$ {faturamentoTotal:F2} | Total de Sessoes: {sessoesHoje.Count}");
            Console.SetCursorPosition(2, 23);
            Console.Write("Pressione qualquer tecla para retornar ao menu...");
            Console.ReadKey();
        }

        // RELATÓRIO MENSAL: Varre e agrupa os registros financeiros do mês e ano correntes
        public void MostrarRelatorioMensal(Tela tela)
        {
            DateTime agora = DateTime.Now;
            tela.PrepararTela($"RELATÓRIO MENSAL ({agora.Month}/{agora.Year})", 0, 0, 79, 24);

            var sessoesMes = this.GetDatabase().Where(s => s.Inicio.Month == agora.Month && s.Inicio.Year == agora.Year).ToList();
            decimal totalMes = sessoesMes.Sum(s => s.ValorCobrado);

            Console.SetCursorPosition(2, 5);
            Console.Write($"Total de Sessões no Mês : {sessoesMes.Count}");
            Console.SetCursorPosition(2, 7);
            Console.Write($"Faturamento Total Bruto : R$ {totalMes:F2}");

            Console.SetCursorPosition(2, 23);
            Console.Write("Pressione qualquer tecla para retornar ao menu...");
            Console.ReadKey();
        }
    }
}