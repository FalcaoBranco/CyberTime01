using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace CyberTime01
{
    internal class MaquinaController : BaseCRUD<MaquinaController, MaquinaModel, MaquinaView>
    {
        private string arquivoDados = "maquinas.txt";

        public MaquinaController(int col, int row, MaquinaView viewInstance, int width, int height)
            : base(col, row, viewInstance)
        {
            this.width = width;
            this.height = height;
            this.LoadFields();
            this.CarregarArquivo();
        }

        protected override string GetKey(MaquinaModel obj)
        {
            return obj.Numero.ToString();
        }

        protected override void MapToDatabase(MaquinaModel destination, MaquinaModel source)
        {
            destination.Numero = source.Numero;
            destination.CombinacaoEspecificacao = source.CombinacaoEspecificacao;
            destination.Ocupada = source.Ocupada;
        }

        protected override void LoadFields()
        {
            this.fields.Add("Numero da Maquina: ");
            this.fields.Add("Especificacao    : ");
            this.fields.Add("Ocupada (S/N)    : ");
        }

        public override void SalvarArquivo()
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(arquivoDados, false))
                {
                    for (int i = 0; i < this.itemDatabase.Count; i++)
                    {
                        sw.WriteLine($"{this.itemDatabase[i].Numero};{this.itemDatabase[i].CombinacaoEspecificacao};{this.itemDatabase[i].Ocupada}");
                    }
                }
            }
            catch
            {
                // Tratamento de erro de gravação
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
                                MaquinaModel novo = new MaquinaModel();
                                novo.Numero = Convert.ToInt32(dados[0]);
                                novo.CombinacaoEspecificacao = dados[1];
                                novo.Ocupada = Convert.ToBoolean(dados[2]);
                                this.itemDatabase.Add(novo);
                            }
                        }
                    }
                }
            }
            catch
            {
                // Contingência para erro de parse ou leitura
            }
        }

        public override void CRUD()
        {
            base.CRUD();
            this.SalvarArquivo();
        }
    }
}