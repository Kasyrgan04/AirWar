using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shot : MonoBehaviour
{
    public GameObject balaPrefab;
    public Transform puntoDisparo;
    private float tiempoCarga = 0f;

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            tiempoCarga += Time.deltaTime;
        }

        if (Input.GetMouseButtonUp(0))
        {
            Disparar(tiempoCarga);
            tiempoCarga = 0f;
        }
    }

    void Disparar(float tiempoCarga)
    {
        GameObject bala = Instantiate(balaPrefab, puntoDisparo.position, Quaternion.identity);
        float velocidadBala = Mathf.Clamp(tiempoCarga * 10f, 5f, 50f);
        bala.GetComponent<Rigidbody2D>().velocity = Vector2.up * velocidadBala;
    }
}
