using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Importa il namespace per i componenti UI

public class DebugToggle : MonoBehaviour
{
    public Toggle toggle; // Riferimento al toggle nell'interfaccia utente
    public GameObject objectToToggle; // Riferimento all'oggetto da attivare/disattivare

    void Start()
    {
        // Assicurati che il toggle e l'oggetto siano impostati
        if (toggle == null || objectToToggle == null)
        {
            Debug.LogError("Toggle or object to toggle not set.");
            return;
        }

        // Aggiungi un listener al toggle che chiama la funzione ToggleObjectActive
        toggle.onValueChanged.AddListener(ToggleObjectActive);

        // Imposta lo stato iniziale dell'oggetto in base al valore iniziale del toggle
        objectToToggle.SetActive(toggle.isOn);
    }

    // Funzione chiamata ogni volta che il valore del toggle cambia
    private void ToggleObjectActive(bool isOn)
    {
        if (objectToToggle != null)
        {
            objectToToggle.SetActive(isOn); // Attiva o disattiva l'oggetto
        }
    }
}
