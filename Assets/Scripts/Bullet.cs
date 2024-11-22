using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float velocidad;
    public Vector3 direccion;

    void Update()
    {
        // Mueve la bala en la dirección especificada con la velocidad correspondiente.
        transform.position += direccion * velocidad * Time.deltaTime;
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
