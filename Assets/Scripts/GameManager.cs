using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject aeropuertoPrefab;
    public GameObject portaavionPrefab;
    public GameObject avionPrefab;
    public GameObject pantallaFinal;

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
    public List<Avion> avionesDestruidos = new List<Avion>();

    public float tiempoJuego = 90f;
    public float tiempoEntreAviones = 10f;
    private bool juegoTerminado = false;

    public TMPro.TextMeshProUGUI tiempoText;
    public TMPro.TextMeshProUGUI avionesDestruidosText;
    public TMPro.TextMeshProUGUI resultadosText;
    public TMP_Dropdown Criterio;
    private string criterioSeleccionado = "id";

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

    private void Update()
    {
        if (juegoTerminado)
        {
            return;
        }

        tiempoJuego -= Time.deltaTime;
        tiempoEntreAviones -= Time.deltaTime;

        if (tiempoEntreAviones <= 0)
        {
            InstanciarAviones(5);
            tiempoEntreAviones = 10f;
        }

        if (tiempoJuego <= 0) 
        {
            tiempoJuego = 0;
            TerminarJuego();
        }

        ActualizarTimer();
    }

    private void ActualizarTimer()
    {
        int minutos = Mathf.FloorToInt(tiempoJuego / 60);
        int segundos = Mathf.FloorToInt(tiempoJuego % 60);

        tiempoText.text = $"{minutos:D2}:{segundos:D2}";
    }

    private void TerminarJuego()
    {
        pantallaFinal.SetActive(true);

        avionesDestruidos = MergeSort(avionesDestruidos);

       
    }

    public void CambiarCriterio(int index)
    {
        switch (index)
        {
            case 0:
                criterioSeleccionado = "id";
                break;
            case 1:
                criterioSeleccionado = "rol";
                break;
            case 2:
                criterioSeleccionado = "horas";
                break;
        }

        MostrarResultado();
    }

    public void MostrarResultado()
    {
        string resultados = $"Aviones destruidos: {avionesDestruidos.Count}\n ";

        resultados += "ID Avión\tPilot\tCopilot\tMaintenance\tSpace Awarness\n";
        resultados += "------------------------------------------------------------\n";

        foreach (var avion in avionesDestruidos)
        {
            resultados += $"{avion.ID}\t";

            var modulosOrdenados = OrdenarModulosPorCriterio(avion.modulos, criterioSeleccionado);
            foreach(var modulo in modulosOrdenados)
            {
                resultados += $"{modulo.ID}\n Horas de vuelo: {modulo.HorasDeVuelo}\t";
                
            }
            resultados += "\n";
            resultados += "------------------------------------------------------------\n";
        }

        resultadosText.text = resultados;
    }

    // Ordenar los módulos de IA por el criterio seleccionado
    private List<AIModule> OrdenarModulosPorCriterio(List<AIModule> modulos, string criterio)
    {
        List<AIModule> modulosOrdenados = new List<AIModule>(modulos);
        int n = modulosOrdenados.Count;

        for (int i = 0; i < n - 1; i++)
        {
            int minIndex = i;
            for (int j = i + 1; j < n; j++)
            {
                bool debeIntercambiar = false;

                switch (criterio.ToLower())
                {
                    case "id":
                        debeIntercambiar = string.Compare(modulosOrdenados[j].ID, modulosOrdenados[minIndex].ID) < 0;
                        break;
                    case "rol":
                        debeIntercambiar = string.Compare(modulosOrdenados[j].Rol, modulosOrdenados[minIndex].Rol) < 0;
                        break;
                    case "horas":
                        debeIntercambiar = modulosOrdenados[j].HorasDeVuelo < modulosOrdenados[minIndex].HorasDeVuelo;
                        break;
                }

                if (debeIntercambiar)
                {
                    minIndex = j;
                }
            }

            // Intercambiar elementos
            if (minIndex != i)
            {
                var temp = modulosOrdenados[i];
                modulosOrdenados[i] = modulosOrdenados[minIndex];
                modulosOrdenados[minIndex] = temp;
            }
        }

        return modulosOrdenados;
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
            GameObject portaavionObj = Instantiate(portaavionPrefab, portaavion.Posicion, Quaternion.identity);
            Carrier carrierScript = portaavionObj.GetComponent<Carrier>();
            carrierScript.CombustibleTotal = 100000f;
            carrierScript.ID = portaavion.Nombre;

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

    public void AvionDestruido(Avion avion)
    {
        avionesDestruidos.Add(avion);
        avionesDestruidosText.text = $"Aviones destruidos: {avionesDestruidos.Count}";
        Debug.Log($"Avión {avion.ID} destruido. Total de aviones destruidos: {avionesDestruidos.Count}");
    }


    private List<Avion> MergeSort(List<Avion> lista)
    {
        if (lista.Count <= 1)
        {
            return lista;
        }

        int mitad = lista.Count / 2;
        List<Avion> izquierda = MergeSort(lista.GetRange(0, mitad));
        List<Avion> derecha = MergeSort(lista.GetRange(mitad, lista.Count - mitad));

        return Merge(izquierda, derecha);
    }

    private List<Avion> Merge(List<Avion> izquierda, List<Avion> derecha)
    {
        List<Avion> resultado = new List<Avion>();
        int indiceIzquierda = 0;
        int indiceDerecha = 0;

        while (indiceIzquierda < izquierda.Count && indiceDerecha < derecha.Count)
        {
            if (string.Compare(izquierda[indiceIzquierda].ID, derecha[indiceDerecha].ID) <= 0)
            {
                resultado.Add(izquierda[indiceIzquierda]);
                indiceIzquierda++;
            }
            else
            {
                resultado.Add(derecha[indiceDerecha]);
                indiceDerecha++;
            }
        }

        while (indiceIzquierda < izquierda.Count)
        {
            resultado.Add(izquierda[indiceIzquierda]);
            indiceIzquierda++;
        }

        while (indiceDerecha < derecha.Count)
        {
            resultado.Add(derecha[indiceDerecha]);
            indiceDerecha++;
        }

        return resultado;
    }

}

