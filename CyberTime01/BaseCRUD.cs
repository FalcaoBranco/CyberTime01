using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyberTime01
{
    internal abstract class BaseCRUD<T, M, V> where V : BaseView<M>, new() where M : new()
    {
        protected int column, row, width, height, position;
        protected List<string> fields;
        protected M model;
        protected List<M> itemDatabase;
        protected V view;

        public BaseCRUD(int col, int row, V viewInstance)
        {
            this.column = col;
            this.row = row;
            this.view = viewInstance;
            this.model = new M();
            this.itemDatabase = new List<M>();
            this.fields = new List<string>();
        }

        protected abstract string GetKey(M obj);
        protected abstract void MapToDatabase(M destination, M source);
        protected abstract void LoadFields();

        protected bool FindItem()
        {
            bool found = false;
            for (int i = 0; i < this.itemDatabase.Count; i++)
            {
                if (GetKey(this.itemDatabase[i]) == GetKey(this.model))
                {
                    found = true;
                    this.position = i;
                    break;
                }
            }
            return found;
        }

        public virtual void CRUD()
        {
            bool found;
            string resp;
            int colini = this.column + 1;
            int colfin = this.column + this.width - 1;
            int linha = this.row + this.height - 1;

            this.view.ShowForm(this.column, this.row, this.width, this.height, this.fields);
            this.view.EnterData(this.column, this.row, "PK", this.model, this.fields, this.width, this.height);
            found = this.FindItem();

            if (found)
            {
                this.view.ShowData(this.column, this.row, this.itemDatabase[this.position], this.fields);
                resp = this.view.Perguntar("Deseja alterar/excluir/voltar (A/E/V): ", linha, colini, colfin);

                if (resp == "a")
                {
                    this.view.EnterData(this.column, this.row, "DT", this.model, this.fields, this.width, this.height);
                    resp = this.view.Perguntar("Confirma alteração (S/N): ", linha, colini, colfin);
                    if (resp == "s")
                    {
                        MapToDatabase(this.itemDatabase[this.position], this.model);
                    }
                }
                if (resp == "e")
                {
                    resp = this.view.Perguntar("Confirma exclusão (S/N): ", linha, colini, colfin);
                    if (resp == "s")
                    {
                        this.itemDatabase.RemoveAt(this.position);
                    }
                }
            }
            else
            {
                resp = this.view.Perguntar("Código não encontrado. Deseja cadastrar (S/N): ", linha, colini, colfin);
                if (resp == "s")
                {
                    this.view.EnterData(this.column, this.row, "DT", this.model, this.fields, this.width, this.height);
                    resp = this.view.Perguntar("Confirma cadastro (S/N): ", linha, colini, colfin);
                    if (resp == "s")
                    {
                        M copy = new M();
                        MapToDatabase(copy, this.model);
                        this.itemDatabase.Add(copy);
                    }
                }
            }
        }

        public abstract void SalvarArquivo();
        public abstract void CarregarArquivo();
    }
}