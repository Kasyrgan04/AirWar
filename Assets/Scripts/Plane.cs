using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plane : MonoBehaviour
{
    public string ID;
    public float velocidad = 5f;
    public float combustibleActual;
    public float combustibleMax = 100f;
    //public Graph grafo;
    private bool Movimiento = false;
    //public List<Ruta> conexiones;


    private void Start()
    {
        combustibleActual = Random.Range(combustibleMax/2, combustibleActual);
       
    }

}