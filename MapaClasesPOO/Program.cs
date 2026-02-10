using MapaClasesPOO.Clases;

class Program
{
    static void Main(string[] args)
    {
        Maestro maestro = new Maestro
        {
            Nombre = "Carlos Martinez",
            Cedula = "001-0000000-0",
            Salario = 50000,
            Asignatura = "Programación",
            HorasClases = 25
        };

        maestro.MostrarInformacion();

        Console.WriteLine($"Asignatura: {maestro.Asignatura}");
        Console.WriteLine($"Horas de clases: {maestro.HorasClases}");

        Console.ReadLine();
    }
}
