using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyberTime01
{
    // Classe abstrata genérica para a camada View do padrão estrutural do sistema.
    // Ela herda de Tela para reaproveitar os métodos de desenho de moldura e cores,
    // e amarra o tipo genérico (T) para que as telas conheçam o Model correspondente.
    internal abstract class BaseView<T> : Tela
    {
        // Construtor que repassa as cores personalizadas de fundo e texto para a classe base Tela
        public BaseView(ConsoleColor cf, ConsoleColor ct) : base(cf, ct) { }

        // Construtor alternativo vazio que invoca as configurações padrões da classe base
        public BaseView() : base() { }

        // Método abstrato obrigatório para desenhar a estrutura visual vazia do formulário na console
        public abstract void ShowForm(int column, int row, int width, int height, List<string> fields);

        // Método abstrato obrigatório para renderizar os dados de um registro existente nos campos da tela
        public abstract void ShowData(int column, int row, T objeto, List<string> fields);

        // Método abstrato obrigatório para gerenciar a captura de inputs do teclado.
        // O parâmetro 'which' serve para filtrar se estamos lendo apenas a chave primária ("PK") ou os dados gerais ("DT").
        public abstract void EnterData(int column, int row, string which, T model, List<string> fields, int width, int height);
    }
}