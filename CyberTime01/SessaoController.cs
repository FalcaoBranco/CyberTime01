using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace CyberTime01
{
    internal class SessaoController : BaseCRUD<SessaoController, SessaoModel, SessaoView>
    {
        private string arquivoDados = "sessoes.txt";

        public SessaoController(int col, int row, SessaoView viewInstance, int width, int height)
            : base(col, row, viewInstance)
        {
            this.width = width;
            this.height = height;
            this.LoadFields();
            this.CarregarArquivo();
        }

        protected override string GetKey(SessaoModel obj)
        {
            return obj.IdSessao;
        }

        protected override void MapToDatabase(SessaoModel destination, SessaoModel source)
        {
            destination.IdSessao = source.IdSessao;
            destination.NumeroMaquina = source.NumeroMaquina;
            destination.CpfCliente = source.CpfCliente;
            destination.Inicio = source.Inicio;
            destination.Fim = source.Fim;
            destination.ValorCobrado = source.ValorCobrado;
        }

        protected override void LoadFields()
        {
            this.fields.Add("ID da Sessao    : ");
            this.fields.Add("Numero Maquina   : ");
            this.fields.Add("CPF do Cliente   : ");
            this.fields.Add("Horario Inicio   : ");
            this.fields.Add("Horario Fim      : ");
            this.fields.Add("Valor Cobrado    : ");
        }

        public override void SalvarArquivo()
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(arquivoDados, false))
                {
                    for (int i = 0; i < this.itemDatabase.Count; i++)
                    {
                        string dataFimTexto = this.itemDatabase[i].Fim.HasValue ? this.itemDatabase[i].Fim.Value.ToString("O") : "NULL";
                        sw.WriteLine($"{this.itemDatabase[i].IdSessao};{this.itemDatabase[i].NumeroMaquina};{this.itemDatabase[i].CpfCliente};{this.itemDatabase[i].Inicio.ToString("O")};{dataFimTexto};{this.itemDatabase[i].ValorCobrado}");
                    }
                }
            }
            catch
            {
                // Tratamento de erro de escrita
            }
        }

        public override void CarregarArquivo()
        {
            try
            {
                if (File.Exists(arquivoDados))
                {
                    this.itemDatabase.Clear();
                    using (StreamReader sr = new StreamReader(arquivoDados))
                    {
                        string linha;
                        while ((linha = sr.ReadLine()) != null)
                        {
                            string[] dados = linha.Split(';');
                            if (dados.Length == 6)
                            {
                                SessaoModel novo = new SessaoModel();
                                novo.IdSessao = dados[0];
                                novo.NumeroMaquina = Convert.ToInt32(dados[1]);
                                novo.CpfCliente = dados[2];
                                novo.Inicio = Convert.ToDateTime(dados[3]);
                                novo.Fim = dados[4] == "NULL" ? (DateTime?)null : Convert.ToDateTime(dados[4]);
                                novo.ValorCobrado = Convert.ToDecimal(dados[5]);
                                this.itemDatabase.Add(novo);
                            }
                        }
                    }
                }
            }
            catch
            {
                // Tratamento de erro de leitura
            }
        }

        public override void CRUD()
        {
            base.CRUD();
            this.SalvarArquivo();
        }
    }
}