using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float tiempoDestruccion = 2f;
    void Start()
    {
        Destroy(gameObject, tiempoDestruccion);
    }
}
