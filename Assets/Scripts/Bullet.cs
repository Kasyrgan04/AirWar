using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float velocidad;
    public Vector3 direccion;
    public float duracion = 2f;

    void Update()
    {
        // Mueve la bala en la dirección especificada con la velocidad correspondiente.
        transform.position += direccion * velocidad * Time.deltaTime;

        duracion -= Time.deltaTime;

        if (duracion <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Plane"))
        {
            // Destruye la bala al colisionar con un enemigo.
            Destroy(gameObject);
        }
    }
}
