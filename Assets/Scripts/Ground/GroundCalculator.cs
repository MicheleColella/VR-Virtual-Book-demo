using System.Collections;
using UnityEngine;

public class GroundCalculator : MonoBehaviour
{
    public GameObject targetObject; // Oggetto che definisce la posizione finale
    public float delaySeconds = 5f; // Tempo di attesa prima dello spostamento
    public Vector3 Offeset;

    private bool hasMoved = false;

    void Start()
    {
        StartCoroutine(MoveAfterDelay());
    }

    IEnumerator MoveAfterDelay()
    {
        // Aspetta per il numero di secondi definito in delaySeconds
        yield return new WaitForSeconds(delaySeconds);

        if (!hasMoved && targetObject != null)
        {
            // Sposta questo oggetto nella posizione dell'oggetto target
            transform.position = new Vector3(targetObject.transform.position.x + Offeset.x, targetObject.transform.position.y + Offeset.y, targetObject.transform.position.z + Offeset.z);
            hasMoved = true; // Impedisce ulteriori spostamenti
        }
    }
}
