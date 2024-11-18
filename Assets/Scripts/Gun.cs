using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public float velocidad = 5f;
    public GameObject balaPrefab; // Prefab de la bala
    public Transform puntoDisparo; // Punto desde donde se disparan las balas
    public float velocidadBala = 10f; // Velocidad inicial de la bala

    void Update()
    {
        float movimientoHorizontal = Input.GetAxis("Horizontal");  
        transform.position += new Vector3(movimientoHorizontal * velocidad * Time.deltaTime, 0, 0);

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(1))
        {
            Disparar();
        }
    }

    void Disparar()
    {
        GameObject bullet = Instantiate(balaPrefab, puntoDisparo.position, Quaternion.identity);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            rb.velocity = transform.right * velocidadBala;
        }
    }
}
