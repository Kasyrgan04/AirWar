using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PriorityQueue<T>
{
    private List<KeyValuePair<T, float>> elementos = new List<KeyValuePair<T, float>>();

    public int Count => elementos.Count;

    public void Enqueue(T item, float prioridad)
    {
        elementos.Add(new KeyValuePair<T, float>(item, prioridad));
        elementos.Sort((a, b) => a.Value.CompareTo(b.Value)); // Ordenar por prioridad
    }

    public T Dequeue()
    {
        T valor = elementos[0].Key;
        elementos.RemoveAt(0);
        return valor;
    }
}
