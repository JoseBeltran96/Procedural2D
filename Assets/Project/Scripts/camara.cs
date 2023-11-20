using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camara : MonoBehaviour
{
    public Transform player; // Referencia al jugador que seguirá la cámara
    public float smoothSpeed = 0.125f; // Velocidad a la que la cámara se mueve para seguir al jugador
    public Vector3 offset; // Distancia entre la cámara y el jugador

    void FixedUpdate()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        Vector3 desiredPosition = player.position + offset; // Calcula la posición a la que la cámara debería moverse
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed); // Suaviza el movimiento de la cámara
        transform.position = smoothedPosition; // Actualiza la posición de la cámara
    }
}
