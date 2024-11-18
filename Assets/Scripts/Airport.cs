using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Airport : MonoBehaviour
{
    public string nombre;
    public int capacidadHangar = 5; // Capacidad de aviones que puede soportar
    public int combustibleDisponible = 1000; // Cantidad de combustible disponible
    public List<Plane> aviones = new List<Plane>();

    // Crear un avión nuevo si hay espacio en el hangar
    public void CrearAvion(Grafo grafo)
    {
        if (aviones.Count < capacidadHangar)
        {
            Plane nuevoAvion = new Plane(System.Guid.NewGuid().ToString(), grafo, this);
            aviones.Add(nuevoAvion);
            Debug.Log("Avión creado con ID: " + nuevoAvion.ID);
        }
        else
        {
            Debug.LogWarning("Capacidad de hangar alcanzada. No se pueden crear más aviones.");
        }
    }

    // Racionar combustible para un avión
    public int ProveerCombustible(int cantidadSolicitada)
    {
        int combustibleProporcionado = Mathf.Min(cantidadSolicitada, combustibleDisponible);
        combustibleDisponible -= combustibleProporcionado;
        Debug.Log(nombre + " ha proporcionado " + combustibleProporcionado + " unidades de combustible. Restante: " + combustibleDisponible);
        return combustibleProporcionado;
    }
}
