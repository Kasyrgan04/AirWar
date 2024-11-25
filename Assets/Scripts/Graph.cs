using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public string Nombre { get; private set; }
    public Vector2 Posicion { get; private set; }
    public List<Edge> Adyacentes { get; private set; }

    public Node(string nombre, Vector2 posicion)
    {
        Nombre = nombre;
        Posicion = posicion;
        Adyacentes = new List<Edge>();
    }
}

public class Edge
{
    public Node Destino { get; set; }
    public float Peso { get; set; }

    public Edge(Node destino, float peso)
    {
        Destino = destino;
        Peso = peso;
    }
}



public class Graph
{
    public List<Node> Nodos { get; private set; }

    public Graph()
    {
        Nodos = new List<Node>();
    }

    public void AgregarNodo(Node nodo)
    {
        if (!Nodos.Contains(nodo))
        {
            Nodos.Add(nodo);
        }
    }

    public void AgregarArista(Node origen, Node destino, float peso)
    {
        if (!Nodos.Contains(origen) || !Nodos.Contains(destino))
        {
            Debug.LogError("Ambos nodos deben existir en el grafo antes de agregar una arista.");
            return;
        }

        Edge arista = new Edge(destino, peso);
        origen.Adyacentes.Add(arista);
    }

    public void ImprimirGrafo()
    {
        foreach (var nodo in Nodos)
        {
            // Imprimir el nodo actual
            Debug.Log($"Nodo: {nodo.Nombre} en {nodo.Posicion}");

            // Imprimir sus aristas (adyacentes)
            foreach (var arista in nodo.Adyacentes)
            {
                Debug.Log($"  -> {arista.Destino.Nombre} (Peso: {arista.Peso})");
            }
        }
    }



}
