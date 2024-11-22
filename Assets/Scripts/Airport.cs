using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Airport : MonoBehaviour
{
    public string nombre;
    public int capacidadHangar = 5; // Capacidad de aviones que puede soportar
    public int combustibleDisponible = 1000; // Cantidad de combustible disponible
    public List<Plane> aviones;
    public GameObject prefabAvion;
    
    public float intervaloCreacion = 10f;

    private void Start()
    {
        aviones = new List<Plane>();
        StartCoroutine(CrearAviones());

    }

    private IEnumerator CrearAviones()
    {
        while (true)
        {
            yield return new WaitForSeconds(intervaloCreacion);

            if (aviones.Count < capacidadHangar) 
            {
                CrearAvion();
            }
        }
    }

    private void CrearAvion()
    {
        GameObject nuevoAvion = Instantiate(prefabAvion,transform.position,Quaternion.identity);
        Plane avionScript = nuevoAvion.GetComponent<Plane>();
        if (avionScript != null) 
        { 
            avionScript.ID = System.Guid.NewGuid().ToString();
            
            aviones.Add(avionScript);
            

            Debug.Log($"Avión creado en {nombre}. ID: {avionScript.ID}");
        }
    }

    public void RemoverAvion(Plane avion)
    {
        if (aviones.Contains(avion)) 
        {
            aviones.Remove(avion);
        }
    }


}
