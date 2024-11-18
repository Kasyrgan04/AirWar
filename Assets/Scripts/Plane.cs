using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plane : MonoBehaviour
{
    public string ID { get; private set; }
    public float combustible = 100f; // Cantidad de combustible actual del avi�n
    public float consumoCombustiblePorKm = 10f; // Consumo de combustible por kil�metro
    private string ubicacionActual;
    private Grafo grafo;
    private Airport aeropuertoActual;
    private List<string> rutaOptima = new List<string>();
    private int indiceRuta = 0;

    public Plane(string id, Grafo grafo, Airport aeropuertoInicial)
    {
        ID = id;
        this.grafo = grafo;
        aeropuertoActual = aeropuertoInicial;
        ubicacionActual = aeropuertoInicial.nombre;
    }

    // Seleccionar un destino y calcular la mejor ruta usando el grafo
    public void SeleccionarDestino(string destino)
    {
        rutaOptima = grafo.CalcularRutaOptima(ubicacionActual, destino);
        indiceRuta = 0;
        Debug.Log("Ruta calculada para avi�n " + ID + ": " + string.Join(" -> ", rutaOptima));
    }

    private void Update()
    {
        if (rutaOptima != null && indiceRuta < rutaOptima.Count)
        {
            VolarHaciaSiguienteNodo();
        }
    }

    // M�todo que mueve el avi�n hacia el siguiente nodo en la ruta
    private void VolarHaciaSiguienteNodo()
    {
        string siguienteNodoNombre = rutaOptima[indiceRuta];
        Vector3 posicionSiguienteNodo = ObtenerPosicionNodo(siguienteNodoNombre);
        float distancia = Vector3.Distance(transform.position, posicionSiguienteNodo);

        // Verificar si hay suficiente combustible
        if (distancia * consumoCombustiblePorKm > combustible)
        {
            Debug.LogWarning("Avi�n " + ID + " se ha quedado sin combustible y se ha ca�do.");
            Destroy(gameObject);
            return;
        }

        // Mover el avi�n y consumir combustible
        transform.position = Vector3.MoveTowards(transform.position, posicionSiguienteNodo, distancia * Time.deltaTime);
        combustible -= distancia * consumoCombustiblePorKm;

        if (Vector3.Distance(transform.position, posicionSiguienteNodo) < 0.1f)
        {
            // Si lleg� al nodo
            ubicacionActual = siguienteNodoNombre;
            indiceRuta++;

            // Si lleg� al destino final
            if (indiceRuta >= rutaOptima.Count)
            {
                Debug.Log("Avi�n " + ID + " ha llegado a su destino en " + ubicacionActual);
                RealizarProcedimientosDeAterrizaje();
            }
        }
    }

    // Realizar los procedimientos de aterrizaje
    private void RealizarProcedimientosDeAterrizaje()
    {
        // Espera y recarga de combustible en el aeropuerto
        if (aeropuertoActual != null)
        {
            int combustibleRecibido = aeropuertoActual.ProveerCombustible(Random.Range(50, 100));
            combustible += combustibleRecibido;

            StartCoroutine(EsperarAntesDeDespegar());
        }
    }

    private IEnumerator EsperarAntesDeDespegar()
    {
        yield return new WaitForSeconds(Random.Range(3, 10));
        SeleccionarDestino("OtroDestinoAleatorio"); // Ejemplo, selecciona otro destino
    }

    private Vector3 ObtenerPosicionNodo(string nodoNombre)
    {
        GameObject nodo = GameObject.Find(nodoNombre);
        return nodo != null ? nodo.transform.position : Vector3.zero;
    }
}
