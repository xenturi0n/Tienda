using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Common;

namespace Tienda.Common.Interfaces
{
    public interface IDB
    {
        string Error { get;  }

        void Conectar();

        bool Comando(string command);

        object Consulta(string consulta);

        object Scalar(string consulta);
    }
}
