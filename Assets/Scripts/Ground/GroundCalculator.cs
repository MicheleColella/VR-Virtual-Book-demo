using System.Collections;
using UnityEngine;

public class GroundCalculator : MonoBehaviour
{
    public GameObject targetObject; // Oggetto che definisce la posizione finale
    public GameObject rotationSource; // Oggetto da cui copiare la rotazione sull'asse Y
    public float delaySeconds = 5f; // Tempo di attesa prima dello spostamento
    public Vector3 Offset; // Offset per la posizione

    private bool hasMoved = false;

    void Start()
    {
        StartCoroutine(MoveAfterDelay());
    }

    IEnumerator MoveAfterDelay()
    {
        // Aspetta per il numero di secondi definito in delaySeconds
        yield return new WaitForSeconds(delaySeconds);

        if (!hasMoved && targetObject != null && rotationSource != null)
        {
            // Sposta questo oggetto nella posizione dell'oggetto target con un offset
            transform.position = targetObject.transform.position + Offset;

            // Copia solo la rotazione sull'asse Y dall'oggetto rotationSource
            float yRotation = rotationSource.transform.eulerAngles.y;
            transform.rotation = Quaternion.Euler(transform.eulerAngles.x, yRotation, transform.eulerAngles.z);

            hasMoved = true; // Impedisce ulteriori spostamenti
        }
    }
}
