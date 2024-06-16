using System.Collections;
using UnityEngine;

public class Ground2017Calculator : MonoBehaviour
{
    public GameObject targetObject; // Oggetto che definisce la posizione finale
    public GameObject rotationSource; // Oggetto da cui prendere la direzione
    public GameObject bookModel; // Oggetto aggiuntivo da controllare
    public float delaySeconds = 5f; // Tempo di attesa prima dello spostamento
    public Vector3 Offset; // Offset per la posizione
    public GameObject debugCubeReset;

    private bool isMoving = false; // Traccia se la coroutine è in esecuzione

    void Start()
    {
        StartCoroutine(MoveAfterDelay());
    }

    public void RecenterButton()
    {
        // Avvia la coroutine solo se non è già in esecuzione
        if (!isMoving)
        {
            StartCoroutine(MoveAfterDelay());
        }
    }

    IEnumerator MoveAfterDelay()
    {
        isMoving = true; // Segna l'inizio dell'esecuzione della coroutine
        debugCubeReset.SetActive(true);

        // Aspetta per il numero di secondi definito in delaySeconds
        yield return new WaitForSeconds(delaySeconds);

        if (targetObject != null && rotationSource != null)
        {
            // Gestisce l'attivazione di bookModel solo se è disattivato
            if (!bookModel.activeSelf)
            {
                bookModel.SetActive(true);
            }

            // Sposta questo oggetto nella posizione dell'oggetto target con un offset
            transform.position = targetObject.transform.position + Offset;

            // Ottiene la direzione corretta guardando verso rotationSource
            Vector3 directionToSource = rotationSource.transform.position - transform.position;
            Quaternion rotationToSource = Quaternion.LookRotation(directionToSource);

            // Applica una rotazione inversa di 180 gradi sull'asse Y
            transform.rotation = Quaternion.Euler(0, rotationToSource.eulerAngles.y + 180, 0);
        }

        debugCubeReset.SetActive(false);
        isMoving = false; // Segna la fine dell'esecuzione della coroutine
    }
}
