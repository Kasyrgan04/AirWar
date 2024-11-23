using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Avion : MonoBehaviour
{
    public Node posicionActual; // Nodo actual del avión
    public Graph grafo; // Grafo del juego
    public float combustibleActual;
    public Font fuentePersonalizada; 
    public float combustibleMax = 100f;
    public float consumoPorDistancia = 0.1f;
    public float consumoPorPeso = 0.05f;
    public string ID;

    private LineRenderer lineRenderer; // Para la estela
    private GameObject pesoTexto; // Para mostrar el peso de la ruta

    public void IniciarViaje()
    {
        Node destino = SeleccionarDestinoAleatorio(grafo);
        StartCoroutine(GestionarViaje(destino)); // Inicia la gestión del viaje
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
            float pesoRuta = ObtenerPesoRuta(nodo);
            yield return MoverHacia(nodo); // Mover hacia cada nodo con peso
        }

        yield return new WaitForSeconds(Random.Range(5, 15)); // Tiempo de espera en destino
        RecargarCombustible();
        IniciarViaje(); // Retoma una nueva ruta
    }

    private IEnumerator MoverHacia(Node nodo)
    {
        // Crear y configurar el LineRenderer si aún no existe
        if (lineRenderer == null)
        {
            lineRenderer = gameObject.AddComponent<LineRenderer>();
            lineRenderer.positionCount = 2; // Dos puntos: inicio y fin
            lineRenderer.startWidth = 0.02f; // Grosor más delgado
            lineRenderer.endWidth = 0.02f;
            lineRenderer.material = new Material(Shader.Find("Sprites/Default")); // Material predeterminado
            lineRenderer.startColor = Color.blue;
            lineRenderer.endColor = Color.blue;
        }

        // Establecer la posición inicial de la estela
        lineRenderer.SetPosition(0, new Vector3(posicionActual.Posicion.x, posicionActual.Posicion.y, 0));

        // Obtener el peso de la ruta
        float pesoRuta = ObtenerPesoRuta(nodo);

        // Crear texto para mostrar el peso de la ruta (en caso de que no exista ya)
        if (pesoTexto == null)
        {
            pesoTexto = new GameObject("PesoTexto");
            TextMesh textMesh = pesoTexto.AddComponent<TextMesh>();
            textMesh.fontSize = 8;
            textMesh.color = Color.white;
            textMesh.alignment = TextAlignment.Center;
            textMesh.anchor = TextAnchor.MiddleCenter;
        }

        // Establecer la posición del texto
        pesoTexto.transform.position = (Vector3)transform.position + new Vector3(0, 1, 0); // Ajuste vertical para que no se sobreponga

        // Mostrar el peso en el texto
        pesoTexto.GetComponent<TextMesh>().text = $" {pesoRuta:F2}";

        // Mover el avión hacia el nodo destino
        while (Vector2.Distance(transform.position, nodo.Posicion) > 0.1f)
        {
            // Estela: actualizar los puntos de inicio y fin
            lineRenderer.SetPosition(1, new Vector3(transform.position.x, transform.position.y, 0));

            // Movimiento del avión
            transform.position = Vector2.MoveTowards(transform.position, nodo.Posicion, Time.deltaTime * 5f);
            yield return null;
        }

        // Actualizar la posición actual del avión
        posicionActual = nodo;

        // Reducir el combustible
        ReducirCombustible();

        // Destruir la estela al llegar al destino
        Destroy(lineRenderer);
        lineRenderer = null;

        // Destruir el texto del peso cuando llegue al destino
        Destroy(pesoTexto);
        pesoTexto = null;
    }


    private void RecargarCombustible()
    {
        combustibleActual = Mathf.Min(combustibleActual + Random.Range(10, 30), combustibleMax);
    }

    private void ReducirCombustible()
    {
        float distancia = Vector2.Distance(transform.position, posicionActual.Posicion);
        float pesoRuta = ObtenerPesoRuta(posicionActual);

        float consumo = distancia * consumoPorDistancia + pesoRuta * consumoPorPeso;
        combustibleActual -= consumo;
        Debug.Log($"Avión {ID} consumió {consumo:F2} de combustible en la ruta {posicionActual.Nombre} -> {posicionActual.Nombre}. Combustible restante: {combustibleActual:F2}");
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
        Dictionary<Node, float> distancias = new Dictionary<Node, float>();
        Dictionary<Node, Node> previos = new Dictionary<Node, Node>();
        HashSet<Node> visitados = new HashSet<Node>();
        PriorityQueue<Node> colaPrioridad = new PriorityQueue<Node>();

        foreach (var nodo in grafo.Nodos)
        {
            distancias[nodo] = float.MaxValue;
            previos[nodo] = null;
        }
        distancias[inicio] = 0;
        colaPrioridad.Enqueue(inicio, 0);

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

        List<Node> ruta = new List<Node>();
        Node temp = destino;
        while (temp != null)
        {
            ruta.Insert(0, temp);
            temp = previos[temp];
        }

        return ruta;
    }
    void ImprimirRutaConPesos(List<Node> ruta)
    {
        if (ruta != null && ruta.Count > 1)
        {
            Debug.Log("Ruta más corta con pesos:");
            for (int i = 0; i < ruta.Count - 1; i++)
            {
                Node nodoActual = ruta[i];
                Node nodoSiguiente = ruta[i + 1];

                // Buscar la arista entre los dos nodos consecutivos
                Edge arista = nodoActual.Adyacentes.Find(e => e.Destino == nodoSiguiente);

                if (arista != null)
                {
                    Debug.Log($"{nodoActual.Nombre} -> {nodoSiguiente.Nombre} (Peso: {arista.Peso})");
                }
            }
        }
        else
        {
            Debug.Log("No se encontró una ruta.");
        }
    }
    public void ImprimirGrafo()
    {
        foreach (var nodo in grafo.Nodos)
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
