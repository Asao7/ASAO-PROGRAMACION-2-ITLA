using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapaClasesPOO.Clases // Fixed namespace to match folder structure
{
    public class Administrator : Docente // Renamed class and base class to fix spelling issues
    {
        public required string AreaAdministrativa { get; set; } // Added 'required' modifier and renamed property to fix spelling and CS8618
    }
}
