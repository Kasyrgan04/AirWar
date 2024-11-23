using UnityEngine;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using Unity.VisualScripting;

public class GameManager : MonoBehaviour
{
    public GameObject aeropuertoPrefab;
    public GameObject portaavionPrefab;
    public GameObject avionPrefab;

    public List<Vector2> posicionesAeropuertos = new List<Vector2>
{
        new Vector2(-5.65f, 1.48f),
        new Vector2(-3.68f,-0.83f),
        new Vector2(0.64f,-1.76f),
        new Vector2(-0.69f,0.42f),
        new Vector2(6.12f,-1.82f),
        new Vector2(0.45f,2.05f),
        new Vector2(4.7f,1.12f),
        new Vector2(4.28f,3.28f),
        new Vector2(-2.59f,3.89f)
};
    public List<Vector2> posicionesPortaaviones = new List<Vector2>
    {
        new Vector2(-2.23f,0.41f),  // Portaavión 1
        new Vector2(-6.66f,-0.11f),  // Portaavión 2
        new Vector2(3.27f,-1.27f),  // Portaavión 3
        new Vector2(7.15f, 0.82f),  // Portaavión 4
        new Vector2(-2.03f,2.36f),// Portaavión 5
    };

    private Graph grafo;

    void Start()
    {
        grafo = new Graph();

        Debug.Log("Instanciando nodos...");
        InstanciarNodos();

        Debug.Log($"Total de nodos en el grafo: {grafo.Nodos.Count}");

        CrearRutas();

        Debug.Log("Instanciando aviones...");
        InstanciarAviones(5);
        //grafo.ImprimirGrafo();
    }


    void InstanciarNodos()
    {
        // Aeropuertos
        for (int i = 0; i < posicionesAeropuertos.Count; i++)
        {
            Node aeropuerto = new Node($"Aeropuerto_{i + 1}", posicionesAeropuertos[i]);
            grafo.AgregarNodo(aeropuerto);
            GameObject aeropuertoObj = Instantiate(aeropuertoPrefab, aeropuerto.Posicion, Quaternion.identity);
            Airport airportScript = aeropuertoObj.GetComponent<Airport>();
            airportScript.capacidadMaxima = Random.Range(5, 15);
            Debug.Log($"Aeropuerto {aeropuerto.Nombre} con capacidad de {airportScript.capacidadMaxima} aviones.");
        }

        // Portaaviones
        for (int i = 0; i < posicionesPortaaviones.Count; i++)
        {
            Node portaavion = new Node($"Portaavion_{i + 1}", posicionesPortaaviones[i]);
            grafo.AgregarNodo(portaavion);
            Instantiate(portaavionPrefab, portaavion.Posicion, Quaternion.identity);
        }

        if (grafo.Nodos.Count == 0)
        {
            Debug.LogError("No se agregaron nodos al grafo.");
        }
    }


    void CrearRutas()
    {
        // Ejemplo de cómo crear rutas con pesos entre nodos
        for (int i = 0; i < grafo.Nodos.Count; i++)
        {
            for (int j = i + 1; j < grafo.Nodos.Count; j++)
            {
                Node nodo1 = grafo.Nodos[i];
                Node nodo2 = grafo.Nodos[j];

                // Peso basado en la distancia y si es ruta interoceánica
                float distancia = Vector2.Distance(nodo1.Posicion, nodo2.Posicion);
                float peso = distancia;

                if (nodo1.Nombre.Contains("Portaavion") || nodo2.Nombre.Contains("Portaavion"))
                {
                    peso += 10f; // Penalización por incluir portaaviones
                }

                grafo.AgregarArista(nodo1, nodo2, peso);
                grafo.AgregarArista(nodo2, nodo1, peso); // Grafo bidireccional
            }
        }
    }

    void InstanciarAviones(int cantidad)
    {
        if (grafo.Nodos.Count == 0)
        {
            Debug.LogError("No hay nodos disponibles en el grafo para instanciar aviones.");
            return;
        }

        for (int i = 0; i < cantidad; i++)
        {
            Node nodoInicial = grafo.Nodos[Random.Range(0, grafo.Nodos.Count)];

            GameObject avionObj = Instantiate(avionPrefab, nodoInicial.Posicion, Quaternion.identity);
            Avion avion = avionObj.GetComponent<Avion>();
            avion.posicionActual = nodoInicial;
            avion.grafo = grafo;
            avion.ID = System.Guid.NewGuid().ToString();
            avion.combustibleActual = UnityEngine.Random.Range(0, avion.combustibleMax);

            avion.IniciarViaje();
        }
    }

}

