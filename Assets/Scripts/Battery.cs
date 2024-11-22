using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battery : MonoBehaviour
{
    public float velocidad = 5f;
    public float limiteIzquierdo = -10f;
    public float limiteDerecho = 10f;
    public GameObject bulletPrefab;  // Prefabricado de la bala
    public float tiempoMaximoPresionado = 3f;  // El tiempo m�ximo que se puede presionar el clic

    private float tiempoPresionado = 0f;

    void Update()
    {
        // Movimiento de la bater�a
        float movimiento = velocidad * Time.deltaTime;
        transform.position += Vector3.right * movimiento;

        // Cambio de direcci�n al alcanzar los l�mites
        if (transform.position.x > limiteDerecho || transform.position.x < limiteIzquierdo)
        {
            velocidad *= -1;
        }

        // Disparo de bala
        if (Input.GetMouseButton(0))  // Si el bot�n izquierdo del rat�n est� presionado
        {
            tiempoPresionado += Time.deltaTime;  // Aumentar el tiempo de presi�n

            if (tiempoPresionado > tiempoMaximoPresionado)
            {
                tiempoPresionado = tiempoMaximoPresionado;  // Limitar el tiempo m�ximo
            }
        }

        if (Input.GetMouseButtonUp(0))  // Cuando se suelta el clic
        {
            DispararBala();
            tiempoPresionado = 0f;  // Resetear el tiempo de presi�n
        }
    }

    void DispararBala()
    {
        // Instanciar la bala
        GameObject bala = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

        // Obtener el script de la bala y asignar su velocidad
        Bullet bulletScript = bala.GetComponent<Bullet>();

        // La velocidad de la bala ser� proporcional al tiempo que se mantuvo presionado el clic
        bulletScript.velocidad = Mathf.Lerp(10f, 30f, tiempoPresionado / tiempoMaximoPresionado);  // Puedes ajustar estos valores

        // La bala se mueve hacia la derecha, puedes cambiar la direcci�n si es necesario
        bulletScript.direccion = Vector3.up;
    }
}
