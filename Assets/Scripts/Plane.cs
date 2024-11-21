using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plane : MonoBehaviour
{
    public string ID;
    public float velocidad = 5f;
    public float combustibleActual;
    public float combustibleMax = 100f;

    private bool Movimiento = false;

    private void Start()
    {
        combustibleActual = Random.Range(combustibleMax/2, combustibleActual);
        
    }

}
