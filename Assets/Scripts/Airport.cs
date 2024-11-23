using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Airport : MonoBehaviour
{
    public int capacidadMaxima = 10; // Capacidad máxima del hangar
    private int avionesActuales = 0;

    public bool PuedeGenerarAvion()
    {
        return avionesActuales < capacidadMaxima;
    }

    public void RegistrarAvion()
    {
        if (avionesActuales < capacidadMaxima)
        {
            avionesActuales++;
        }
    }

    public void SalirAvion()
    {
        Debug.Log("Aviones anteriores: " + avionesActuales);

        if (avionesActuales > 0)
        {
            avionesActuales--;
            Debug.Log("Aviones actuales: " + avionesActuales);
        }
    }
}
