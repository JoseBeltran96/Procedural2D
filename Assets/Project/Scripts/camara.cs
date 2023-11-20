using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camara : MonoBehaviour
{
    public Transform player; // Referencia al jugador que seguir� la c�mara
    public float smoothSpeed = 0.125f; // Velocidad a la que la c�mara se mueve para seguir al jugador
    public Vector3 offset; // Distancia entre la c�mara y el jugador

    void FixedUpdate()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        Vector3 desiredPosition = player.position + offset; // Calcula la posici�n a la que la c�mara deber�a moverse
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed); // Suaviza el movimiento de la c�mara
        transform.position = smoothedPosition; // Actualiza la posici�n de la c�mara
    }
}
