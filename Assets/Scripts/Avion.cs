using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Avion : MonoBehaviour
{
    public Node posicionActual;
    public Graph grafo;

    public void IniciarViaje()
    {
        Node destino = SeleccionarDestinoAleatorio(grafo);
        StartCoroutine(GestionarViaje(destino));
    }

    private Node SeleccionarDestinoAleatorio(Graph grafo)
    {
        // Selecciona un nodo aleatorio del grafo.
        return grafo.Nodos[Random.Range(0, grafo.Nodos.Count)];
    }

    private IEnumerator GestionarViaje(Node destino)
    {
        List<Node> ruta = CalcularRutaMasCorta(grafo, posicionActual, destino);

        foreach (var nodo in ruta)
        {
            yield return MoverHacia(nodo);
        }

        yield return new WaitForSeconds(Random.Range(5, 15)); // Tiempo de espera en destino
        RecargarCombustible();
        IniciarViaje(); // Retoma una nueva ruta
    }

    private IEnumerator MoverHacia(Node nodo)
    {
        while (Vector2.Distance(transform.position, nodo.Posicion) > 0.1f)
        {
            transform.position = Vector2.MoveTowards(transform.position, nodo.Posicion, Time.deltaTime * 5f);
            yield return null;
        }

        posicionActual = nodo;
    }

    private void RecargarCombustible()
    {
        // L�gica para recargar combustible.
    }

    private List<Node> CalcularRutaMasCorta(Graph grafo, Node inicio, Node destino)
    {
        // Dijkstra
        Dictionary<Node, float> distancias = new Dictionary<Node, float>();
        Dictionary<Node, Node> previos = new Dictionary<Node, Node>();
        HashSet<Node> visitados = new HashSet<Node>();
        PriorityQueue<Node> colaPrioridad = new PriorityQueue<Node>();

        // Inicializaci�n
        foreach (var nodo in grafo.Nodos)
        {
            distancias[nodo] = float.MaxValue;
            previos[nodo] = null;
        }
        distancias[inicio] = 0;
        colaPrioridad.Enqueue(inicio, 0);

        // Bucle principal
        while (colaPrioridad.Count > 0)
        {
            Node actual = colaPrioridad.Dequeue();
            if (actual == destino) break;
            if (visitados.Contains(actual)) continue;

            visitados.Add(actual);

            foreach (var arista in actual.Adyacentes)
            {
                Node vecino = arista.Destino;
                float nuevoCosto = distancias[actual] + arista.Peso;

                if (nuevoCosto < distancias[vecino])
                {
                    distancias[vecino] = nuevoCosto;
                    previos[vecino] = actual;
                    colaPrioridad.Enqueue(vecino, nuevoCosto);
                }
            }
        }

        // Reconstrucci�n del camino
        List<Node> ruta = new List<Node>();
        Node temp = destino;
        while (temp != null)
        {
            ruta.Insert(0, temp);
            temp = previos[temp];
        }

        return ruta;
    }
}