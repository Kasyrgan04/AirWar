using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour
{
    public GameObject aeropuertoPrefab;  // Prefab del aeropuerto
    public GameObject portaavionPrefab;  // Prefab del portaavión
    


    private List<GameObject> aeropuertosGenerados = new List<GameObject>();
    private List<GameObject> portaavionesGenerados = new List<GameObject>();
    

    Airport aeropuertoScript;
    Carrier portaavionScript;


    private Vector3[] posicionesAeropuertos = new Vector3[]
{
        new Vector3(-5.65f, 1.48f, 0),
        new Vector3(-3.68f,-0.83f, 0),
        new Vector3(0.64f,-1.76f,0),
        new Vector3(-0.69f,0.42f,0),
        new Vector3(6.12f,-1.82f,0),
        new Vector3(0.45f,2.05f,0),
        new Vector3(4.7f,1.12f,0),
        new Vector3(4.28f,3.28f,0),
        new Vector3(-2.59f,3.89f,0)
};

    private Vector3[] posicionesPortaaviones = new Vector3[]
    {
        new Vector3(-2.23f,0.41f,0),  // Portaavión 1
        new Vector3(-6.66f,-0.11f,0),  // Portaavión 2
        new Vector3(3.27f,-1.27f,0),  // Portaavión 3
        new Vector3(7.15f,0.82f,0),  // Portaavión 4
        new Vector3(-2.03f,2.36f,0),// Portaavión 5
    };

    public Graph grafo = new Graph();
    public Material lineaMaterial; // Material para las líneas



    void Start()
    {
        GenerarAeropuertos();
        GenerarPortaaviones();
        ConectarAeropuertosYPortaaviones();
        //grafo.ImprimirGrafo();
        DibujarConexiones();
        
    }

    void GenerarAeropuertos()
    {
        for (int i = 0; i < posicionesAeropuertos.Length; i++)
        {
            // Instanciar el aeropuerto en la posición especificada
            GameObject aeropuertoInstancia = Instantiate(aeropuertoPrefab, posicionesAeropuertos[i], Quaternion.identity);

            // Asignar el componente Aeropuerto y configurarlo
            aeropuertoScript = aeropuertoInstancia.GetComponent<Airport>();
            if (aeropuertoScript != null)
            {
                aeropuertoScript.nombre = "Aeropuerto_" + i;

                // Configurar capacidad del hangar y combustible de manera personalizada si es necesario
                aeropuertoScript.capacidadHangar = UnityEngine.Random.Range(3, 10); // Ejemplo: capacidad aleatoria
                aeropuertoScript.combustibleDisponible = UnityEngine.Random.Range(500, 2000); // Ejemplo: combustible aleatorio

                // Agregar el aeropuerto al grafo
                //grafo.AñadirNodo(aeropuertoScript.nombre);
            }
        }
    }
    void GenerarPortaaviones() 
    {
        for (int i = 0; i < posicionesPortaaviones.Length; i++)
        {
            string portaavionID = "Portaavion_" + i;
            GameObject portaavion = Instantiate(portaavionPrefab, posicionesPortaaviones[i], Quaternion.identity);
            portaavionScript = portaavion.GetComponent<Carrier>();
            if (portaavionScript != null)
            {
                portaavionScript.InicializarPortaaviones(portaavionID, UnityEngine.Random.Range(1000, 50000));
                portaavionesGenerados.Add(portaavion);
               // grafo.AñadirNodo(portaavionID);
            }
            
        }
    }

    
    // Método para conectar aeropuertos y portaaviones con rutas aleatorias
    void ConectarAeropuertosYPortaaviones()
    {
        // Conectar aeropuertos con otros aeropuertos aleatoriamente
        for (int i = 0; i < posicionesAeropuertos.Length; i++)
        {
            string aeropuertoID = "Aeropuerto_" + i;

            // Conectar cada aeropuerto con otros aeropuertos aleatorios (por ejemplo, 2 conexiones por aeropuerto)
            List<int> aeropuertosConectados = new List<int>(); // Lista para evitar conexiones repetidas
            int numConexiones = UnityEngine.Random.Range(1, 3); // Número aleatorio de conexiones entre aeropuertos

            for (int j = 0; j < numConexiones; j++)
            {
                int destino = UnityEngine.Random.Range(0, posicionesAeropuertos.Length);
                while (destino == i || aeropuertosConectados.Contains(destino)) // Asegurarse de que no se conecte a sí mismo o repetidamente
                {
                    destino = UnityEngine.Random.Range(0, posicionesAeropuertos.Length);
                }

                string aeropuertoDestinoID = "Aeropuerto_" + destino;
                float peso = CalcularPeso(posicionesAeropuertos[i], posicionesAeropuertos[destino], true);
               // grafo.AñadirConexion(aeropuertoID, aeropuertoDestinoID, peso);
                aeropuertosConectados.Add(destino);
            }

            // Conectar aeropuertos con portaaviones aleatoriamente
            for (int j = 0; j < posicionesPortaaviones.Length; j++)
            {
                string portaavionID = "Portaavion_" + j;
                if (UnityEngine.Random.Range(0f, 1f) < 0.5f) // Probabilidad de conexión aleatoria
                {
                    float peso = CalcularPeso(posicionesAeropuertos[i], posicionesPortaaviones[j], false);
                    //grafo.AñadirConexion(aeropuertoID, portaavionID, peso);
                }
            }
        }

        // Conectar portaaviones entre sí aleatoriamente
        for (int i = 0; i < posicionesPortaaviones.Length; i++)
        {
            string portaavionID = "Portaavion_" + i;

            // Conectar cada portaavión con otros portaaviones aleatoriamente (por ejemplo, 2 conexiones por portaavión)
            List<int> portaavionesConectados = new List<int>(); // Lista para evitar conexiones repetidas
            int numConexiones = UnityEngine.Random.Range(1, 3); // Número aleatorio de conexiones entre portaaviones

            for (int j = 0; j < numConexiones; j++)
            {
                int destino = UnityEngine.Random.Range(0, posicionesPortaaviones.Length);
                while (destino == i || portaavionesConectados.Contains(destino)) // Asegurarse de que no se conecte a sí mismo o repetidamente
                {
                    destino = UnityEngine.Random.Range(0, posicionesPortaaviones.Length);
                }

                string portaavionDestinoID = "Portaavion_" + destino;
                float peso = CalcularPeso(posicionesPortaaviones[i], posicionesPortaaviones[destino], false);
                //grafo.AñadirConexion(portaavionID, portaavionDestinoID, peso);
                portaavionesConectados.Add(destino);
            }
        }
    }


    // Método para calcular el peso de la ruta considerando la distancia y el tipo de ruta
    float CalcularPeso(Vector3 origen, Vector3 destino, bool esContinental)
    {
        // Calcular la distancia euclidiana
        float distancia = Vector3.Distance(origen, destino);

        // Si es continental, el peso es la distancia normal
        if (esContinental)
        {
            return distancia;

        }

        // Si es interoceánica (ruta entre un aeropuerto y un portaavión), se añade un costo adicional
        float costoExtra = 50f; // Costo adicional por ser una ruta interoceánica
        return distancia + costoExtra;
    }

    // Método para dibujar todas las conexiones
    void DibujarConexiones()
    {
        // Dibujar conexiones de los aeropuertos
        for (int i = 0; i < posicionesAeropuertos.Length; i++)
        {
            string aeropuertoID = "Aeropuerto_" + i;
            //List<Ruta> conexiones = grafo.ObtenerConexiones(aeropuertoID);

           /* foreach (Ruta conexion in conexiones)
            {
                int destinoID = int.Parse(conexion.NodoDestino.Split('_')[1]);
                Vector3 origen = posicionesAeropuertos[i];
                Vector3 destino = (conexion.NodoDestino.StartsWith("Aeropuerto_"))
                                  ? posicionesAeropuertos[destinoID]
                                  : posicionesPortaaviones[destinoID];

                CrearLinea(origen, destino);
            }*/
        }

        // Dibujar conexiones de los portaaviones
        for (int i = 0; i < posicionesPortaaviones.Length; i++)
        {
            string portaavionID = "Portaavion_" + i;
            /*List<Ruta> conexiones = grafo.ObtenerConexiones(portaavionID);

            foreach (Ruta conexion in conexiones)
            {
                int destinoID = int.Parse(conexion.NodoDestino.Split('_')[1]);
                Vector3 origen = posicionesPortaaviones[i];
                Vector3 destino = (conexion.NodoDestino.StartsWith("Aeropuerto_"))
                                  ? posicionesAeropuertos[destinoID]
                                  : posicionesPortaaviones[destinoID];

                CrearLinea(origen, destino);
            }*/
        }
    }

    // Método para crear una línea entre dos puntos
    void CrearLinea(Vector3 origen, Vector3 destino)
    {
        GameObject linea = new GameObject("Linea");
        LineRenderer lineRenderer = linea.AddComponent<LineRenderer>();
        lineRenderer.material = lineaMaterial; // Asignar el material para la línea
        lineRenderer.startWidth = 0.01f; // Ancho inicial de la línea
        lineRenderer.endWidth = 0.01f;   // Ancho final de la línea
        lineRenderer.positionCount = 2; // Solo necesitamos dos puntos
        lineRenderer.SetPosition(0, new Vector3(origen.x, origen.y, 0)); // Establecer el primer punto en 2D
        lineRenderer.SetPosition(1, new Vector3(destino.x, destino.y, 0)); // Establecer el segundo punto en 2D
        lineRenderer.useWorldSpace = false; // Usar espacio local en lugar de espacio global
    }


    

 
}

    



