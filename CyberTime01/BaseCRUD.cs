using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace CyberTime01
{
    // Classe abstrata genérica que serve como base para todos os meus controladores do sistema.
    // Ela implementa o padrão genérico para amarrar os tipos de Model (M) e View (V) de forma fortemente tipada.
    internal abstract class BaseCRUD<T, M, V> where V : BaseView<M>, new() where M : new()
    {
        // Variáveis de controle de posicionamento espacial na tela console e coordenadas geométricas
        protected int column, row, width, height, position;

        // Lista contendo os labels/rótulos dos campos dinâmicos da entidade ativa
        protected List<string> fields;

        // Instância de modelo genérico temporário para buffers de leitura e manipulação
        protected M model;

        // Banco de dados em memória que armazena a coleção principal da respectiva entidade
        protected List<M> itemDatabase;

        // Instância fortemente tipada da View responsável por gerenciar a interface dessa tela
        protected V view;

        // Construtor base padrão que injeta o posicionamento e a respectiva instância visual
        public BaseCRUD(int col, int row, V viewInstance)
        {
            this.column = col;
            this.row = row;
            this.view = viewInstance;
            this.model = new M();
            this.itemDatabase = new List<M>();
            this.fields = new List<string>();
        }

        // CORREÇÃO UNIFICADA: Método público exposto para que controladores externos acessem a lista em memória sem quebras de tipo
        public virtual List<M> GetDatabase()
        {
            return this.itemDatabase;
        }

        // Métodos abstratos obrigatórios que herdeiros devem sobrescrever para retornar chaves unívocas, 
        // mapear transferências entre buffers e databases, e instanciar labels de campos textuais.
        protected abstract string GetKey(M obj);
        protected abstract void MapToDatabase(M destination, M source);
        protected abstract void LoadFields();

        // Rotina interna que efua busca sequencial na lista com base na assinatura de chave exclusiva
        protected bool FindItem()
        {
            bool found = false;
            for (int i = 0; i < this.itemDatabase.Count; i++)
            {
                // Verifica equivalência da chave identificadora (Ex: CPF ou Número da Máquina)
                if (GetKey(this.itemDatabase[i]) == GetKey(this.model))
                {
                    found = true;
                    this.position = i; // Armazena a linha exata onde o objeto foi localizado na coleção
                    break;
                }
            }
            return found;
        }

        // Método mestre contendo o fluxo básico do motor de telas estruturadas (Create, Read, Update, Delete)
        public virtual void CRUD()
        {
            bool found;
            string resp;
            int colini = this.column + 1;
            int colfin = this.column + this.width - 1;
            int linha = this.row + this.height - 1;

            // Renderiza a moldura de console padrão e solicita de imediato o campo de identificação (Chave Primária)
            this.view.ShowForm(this.column, this.row, this.width, this.height, this.fields);
            this.view.EnterData(this.column, this.row, "PK", this.model, this.fields, this.width, this.height);

            // Executa varredura em memória para determinar comportamento do menu sequencial
            found = this.FindItem();

            if (found) // Cenário lógico: Entidade existente (Atualização ou Remoção)
            {
                // Mostra na tela os dados já cadastrados do item encontrado
                this.view.ShowData(this.column, this.row, this.itemDatabase[this.position], this.fields);
                resp = this.view.Perguntar("Deseja alterar/excluir/voltar (A/E/V): ", linha, colini, colfin);

                if (resp == "a") // Fluxo operacional de alteração de registro
                {
                    this.view.EnterData(this.column, this.row, "DT", this.model, this.fields, this.width, this.height);
                    resp = this.view.Perguntar("Confirma alteração (S/N): ", linha, colini, colfin);
                    if (resp == "s")
                    {
                        // Efetua transferência de dados entre buffers aplicando regras de mapeamento específicas
                        MapToDatabase(this.itemDatabase[this.position], this.model);
                    }
                }
                if (resp == "e") // Fluxo operacional de deleção de registro
                {
                    resp = this.view.Perguntar("Confirma exclusão (S/N): ", linha, colini, colfin);
                    if (resp == "s")
                    {
                        // Remove fisicamente da coleção em memória através do índice posicional mapeado
                        this.itemDatabase.RemoveAt(this.position);
                    }
                }
            }
            else // Cenário lógico: Entidade inédita (Fluxo de Cadastro de novos itens)
            {
                resp = this.view.Perguntar("Código não encontrado. Deseja cadastrar (S/N): ", linha, colini, colfin);
                if (resp == "s")
                {
                    // Solicita os dados complementares na console
                    this.view.EnterData(this.column, this.row, "DT", this.model, this.fields, this.width, this.height);
                    resp = this.view.Perguntar("Confirma cadastro (S/N): ", linha, colini, colfin);
                    if (resp == "s")
                    {
                        M copy = new M();
                        // Realiza cópia defensiva para evitar persistência de referências cruzadas em memória
                        MapToDatabase(copy, this.model);
                        this.itemDatabase.Add(copy);
                    }
                }
            }
        }

        // Métodos abstratos para obrigar a persistência física em disco em suas respectivas sub-classes
        public abstract void SalvarArquivo();
        public abstract void CarregarArquivo();
    }
}