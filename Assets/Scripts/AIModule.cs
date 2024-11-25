using System;

public class AIModule
{
    public string ID { get; private set; }
    public string Rol { get; private set; }
    public int HorasDeVuelo { get; set; }

    public AIModule(string rol)
    {
        ID = GenerarID();
        Rol = rol;
        HorasDeVuelo = 0;
    }

    private string GenerarID()
    {
        Random random = new Random();
        char letra1 = (char)random.Next('A', 'Z' + 1);
        char letra2 = (char)random.Next('A', 'Z' + 1);
        char letra3 = (char)random.Next('A', 'Z' + 1);
        return $"{letra1}{letra2}{letra3}";
    }

    public override string ToString()
    {
        return $"{Rol}: {ID}, Horas de vuelo: {HorasDeVuelo}";
    }
}

