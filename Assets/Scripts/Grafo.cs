using System;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.Collections.Generic;
using System.Linq;

public class Ruta
{
    public string NodoDestino;
    public float Peso;

    public Ruta(string nodoDestino, float peso)
    {
        NodoDestino = nodoDestino;
        Peso = peso;
    }
}

public class Grafo
{
    private Dictionary<string, List<Ruta>> adjList;

    public Grafo()
    {
        adjList = new Dictionary<string, List<Ruta>>();
    }

    // Añadir un nodo al grafo
    public void AñadirNodo(string nodo)
    {
        if (!adjList.ContainsKey(nodo))
        {
            adjList[nodo] = new List<Ruta>();
        }
    }

    // Añadir una conexión con un peso
    public void AñadirConexion(string nodo1, string nodo2, float peso)
    {
        if (!adjList.ContainsKey(nodo1))
        {
            AñadirNodo(nodo1);
        }

        if (!adjList.ContainsKey(nodo2))
        {
            AñadirNodo(nodo2);
        }

        adjList[nodo1].Add(new Ruta(nodo2, peso));
        adjList[nodo2].Add(new Ruta(nodo1, peso)); // Conexiones bidireccionales
    }

    // Obtener las conexiones de un nodo
    public List<Ruta> ObtenerConexiones(string nodo)
    {
        if (adjList.ContainsKey(nodo))
        {
            return adjList[nodo];
        }
        return null;
    }

    // Método de Dijkstra para encontrar la ruta óptima
    public List<string> CalcularRutaOptima(string inicio, string destino)
    {
        var distancias = new Dictionary<string, float>();
        var previos = new Dictionary<string, string>();
        var nodosPendientes = new List<string>();

        foreach (var nodo in adjList.Keys)
        {
            distancias[nodo] = float.MaxValue;
            previos[nodo] = null;
            nodosPendientes.Add(nodo);
        }

        distancias[inicio] = 0;

        while (nodosPendientes.Count > 0)
        {
            // Nodo con menor distancia actual
            string nodoActual = nodosPendientes.OrderBy(n => distancias[n]).First();
            nodosPendientes.Remove(nodoActual);

            if (nodoActual == destino)
            {
                break;
            }

            foreach (var conexion in adjList[nodoActual])
            {
                float distanciaAlt = distancias[nodoActual] + conexion.Peso;
                if (distanciaAlt < distancias[conexion.NodoDestino])
                {
                    distancias[conexion.NodoDestino] = distanciaAlt;
                    previos[conexion.NodoDestino] = nodoActual;
                }
            }
        }

        // Reconstrucción de la ruta óptima
        var rutaOptima = new List<string>();
        string paso = destino;
        while (paso != null)
        {
            rutaOptima.Insert(0, paso);
            paso = previos[paso];
        }

        return rutaOptima;
    }

    // Imprimir el grafo
    public void ImprimirGrafo()
    {
        foreach (var nodo in adjList)
        {
            Console.WriteLine(nodo.Key + " está conectado con: ");
            foreach (var conexion in nodo.Value)
            {
                Console.WriteLine(" - " + conexion.NodoDestino + " con peso " + conexion.Peso);
            }
        }
    }
}



