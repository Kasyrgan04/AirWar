using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Carrier : MonoBehaviour
{
    public string ID;          // ID del portaaviones
    public float CombustibleTotal;  // Capacidad máxima de combustible en el portaaviones

    // Método para inicializar un portaaviones
    public void InicializarPortaaviones(string id, float combustible)
    {
        ID = id;
        CombustibleTotal = combustible;

    }

}
