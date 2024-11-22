using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Avion : MonoBehaviour
{
    public Node posicionActual;
    public Graph grafo;
    public float combustibleActual;
    public float combustibleMax = 100f;
    public float consumoPorDistancia = 0.1f;
    public float consumoPorPeso = 0.05f;
    public string ID;


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
        ReducirCombustible();
    }

    private void RecargarCombustible()
    {
        combustibleActual = Mathf.Min(combustibleActual + Random.Range(10, 30), combustibleMax);
    }

    private void ReducirCombustible()
    {
        float distancia = Vector2.Distance(transform.position, posicionActual.Posicion);
        float peroRuta = ObtenerPesoRuta(posicionActual);

        float consumo = distancia * consumoPorDistancia + peroRuta * consumoPorPeso;
        combustibleActual -= consumo;
        Debug.Log($"Avión {ID} consumió {consumo} de combustible en la ruta {posicionActual.Nombre} -> {posicionActual.Nombre}. Combustible restante {combustibleActual}");
    }

    private float ObtenerPesoRuta(Node nodo)
    {
        
        foreach (var arista in nodo.Adyacentes)
        {
            if (arista.Destino == posicionActual)
            {
                return arista.Peso;
                
            }
        }
        return 0f;
    }

    private List<Node> CalcularRutaMasCorta(Graph grafo, Node inicio, Node destino)
    {
        // Dijkstra
        Dictionary<Node, float> distancias = new Dictionary<Node, float>();
        Dictionary<Node, Node> previos = new Dictionary<Node, Node>();
        HashSet<Node> visitados = new HashSet<Node>();
        PriorityQueue<Node> colaPrioridad = new PriorityQueue<Node>();

        // Inicialización
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

        // Reconstrucción del camino
        List<Node> ruta = new List<Node>();
        Node temp = destino;
        while (temp != null)
        {
            ruta.Insert(0, temp);
            temp = previos[temp];
        }

        return ruta;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            Destruir();
        }
    } 
    
    private void Destruir()
    {
        Destroy(gameObject);
    }
}


