using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battery : MonoBehaviour
{
    public float velocidad = 5f;
    public float limiteIzquierdo = -10f;
    public float limiteDerecho = 10f;

    void Update()
    {
        float movimiento = velocidad * Time.deltaTime;
        transform.position += Vector3.right * movimiento;

        if (transform.position.x > limiteDerecho || transform.position.x < limiteIzquierdo)
        {
            velocidad *= -1;
        }
    }
}
