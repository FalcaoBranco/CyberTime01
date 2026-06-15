using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyberTime01
{
    internal abstract class BaseView<T> : Tela
    {
        public BaseView(ConsoleColor cf, ConsoleColor ct) : base(cf, ct) { }

        public BaseView() : base() { }

        public abstract void ShowForm(int column, int row, int width, int height, List<string> fields);

        public abstract void ShowData(int column, int row, T objeto, List<string> fields);

        public abstract void EnterData(int column, int row, string which, T model, List<string> fields, int width, int height);
    }
}