using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapaClasesPOO.Clases
{
    public class MiembroDeLaComunidad
    {
        public string Nombre { get; set; }
        public string Cedula { get; set; }

        public virtual void MostrarInformacion()
        {
            Console.WriteLine($"Nombre: {Nombre}");
            Console.WriteLine($"Cédula: {Cedula}");
        }
    }
}

