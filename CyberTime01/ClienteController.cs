using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace CyberTime01
{
    internal class ClienteController : BaseCRUD<ClienteController, ClienteModel, ClienteView>
    {
        private string arquivoDados = "clientes.txt";

        public ClienteController(int col, int row, ClienteView viewInstance, int width, int height)
            : base(col, row, viewInstance)
        {
            this.width = width;
            this.height = height;
            this.LoadFields();
            this.CarregarArquivo();
        }

        protected override string GetKey(ClienteModel obj)
        {
            return obj.Cpf;
        }

        protected override void MapToDatabase(ClienteModel destination, ClienteModel source)
        {
            destination.Cpf = source.Cpf;
            destination.Nome = source.Nome;
            destination.SaldoPrePago = source.SaldoPrePago;
        }

        protected override void LoadFields()
        {
            this.fields.Add("CPF           : ");
            this.fields.Add("Nome          : ");
            this.fields.Add("Saldo Pre-Pago: ");
        }

        public override void SalvarArquivo()
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(arquivoDados, false))
                {
                    for (int i = 0; i < this.itemDatabase.Count; i++)
                    {
                        sw.WriteLine($"{this.itemDatabase[i].Cpf};{this.itemDatabase[i].Nome};{this.itemDatabase[i].SaldoPrePago}");
                    }
                }
            }
            catch
            {
                // Tratamento preventivo de falha em disco
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
                            if (dados.Length == 3)
                            {
                                ClienteModel novo = new ClienteModel();
                                novo.Cpf = dados[0];
                                novo.Nome = dados[1];
                                novo.SaldoPrePago = Convert.ToDecimal(dados[2]);
                                this.itemDatabase.Add(novo);
                            }
                        }
                    }
                }
            }
            catch
            {
                // Contingência para arquivo inacessível
            }
        }

        public override void CRUD()
        {
            base.CRUD();
            this.SalvarArquivo();
        }
    }
}