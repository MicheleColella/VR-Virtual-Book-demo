using UnityEngine;

public class HandSwipeGestureDetector : MonoBehaviour
{
    public GameObject BoxCheckTrigger;
    public GameObject RightCube;
    public GameObject LeftCube;
    public GameObject leftHand;
    public GameObject rightHand;
    public Collider swipeArea;  // Collider che definisce l'area dello swipe
    public float swipeThreshold = 0.5f;
    public float minimumSwipeDistance = 0.2f;
    public float swipeTime = 0.3f; // Tempo entro il quale il movimento deve essere completato

    private Vector3 leftHandStartPosition;
    private Vector3 rightHandStartPosition;
    private float leftSwipeStartTime;
    private float rightSwipeStartTime;
    private bool isLeftHandInArea = false;
    private bool isRightHandInArea = false;

    private void Start()
    {
        RightCube.SetActive(false);
        LeftCube.SetActive(false);
        BoxCheckTrigger.SetActive(false);
    }

    void Update()
    {
        if (isLeftHandInArea)
        {
            CheckSwipe(leftHand, ref leftHandStartPosition, ref leftSwipeStartTime, true);
        }
        if (isRightHandInArea)
        {
            CheckSwipe(rightHand, ref rightHandStartPosition, ref rightSwipeStartTime, false);
        }

        if (isLeftHandInArea || isRightHandInArea)
        {
            BoxCheckTrigger.SetActive(true);
        }
        else
        {
            BoxCheckTrigger.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == leftHand)
        {
            isLeftHandInArea = true;
        }
        else if (other.gameObject == rightHand)
        {
            isRightHandInArea = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == leftHand)
        {
            isLeftHandInArea = false;
        }
        else if (other.gameObject == rightHand)
        {
            isRightHandInArea = false;
        }
    }

    private void CheckSwipe(GameObject hand, ref Vector3 startHandPosition, ref float swipeStartTime, bool isLeftHand)
    {
        if (hand.activeInHierarchy) // Assicurati che la mano sia attiva e tracciata
        {
            if (swipeStartTime == 0)
            {
                startHandPosition = hand.transform.position;
                swipeStartTime = Time.time;
            }
            else
            {
                if (Time.time - swipeStartTime < swipeTime)
                {
                    Vector3 direction = hand.transform.position - startHandPosition;
                    if (direction.magnitude > minimumSwipeDistance)
                    {
                        if (isLeftHand && direction.x > swipeThreshold)
                        {
                            Debug.Log("Left hand swipe right detected");
                            RightCube.SetActive(false);
                            LeftCube.SetActive(true);
                        }
                        else if (!isLeftHand && direction.x < -swipeThreshold)
                        {
                            Debug.Log("Right hand swipe left detected");
                            RightCube.SetActive(true);
                            LeftCube.SetActive(false);
                        }
                    }
                }
                else
                {
                    ResetSwipe(ref swipeStartTime);
                }
            }
        }
        else if (swipeStartTime != 0)
        {
            ResetSwipe(ref swipeStartTime);
        }
    }

    private void ResetSwipe(ref float swipeStartTime)
    {
        swipeStartTime = 0;
    }
}
