using UnityEngine;
using echo17.EndlessBook;
using TMPro; // Importa il namespace per TextMeshPro

public class HandSwipeGestureDetector : MonoBehaviour
{
    [Header("Debugger")]
    public GameObject BoxCheckTrigger;
    public GameObject RightCube;
    public GameObject LeftCube;
    public TextMeshPro textRot; // Riferimento al Text Mesh Pro nella scena

    [Header("Book Settings")]
    public EndlessBook book;

    [Header("Hand Models")]
    public GameObject leftHand;
    public GameObject rightHand;

    [Header("Swipe Settings")]
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
        BoxCheckTrigger.SetActive(false);/*
        if (book.CurrentState == EndlessBook.StateEnum.ClosedFront)
        {
            book.SetState(EndlessBook.StateEnum.OpenMiddle);
        }
        else
        {
            book.SetState(EndlessBook.StateEnum.ClosedFront);
        }*/
    }

    void Update()
    {
        if (isLeftHandInArea || isRightHandInArea)
        {
            BoxCheckTrigger.SetActive(true);
            UpdateTextMesh();
        }
        else
        {
            BoxCheckTrigger.SetActive(false);
        }

        if (isLeftHandInArea)
        {
            CheckSwipe(leftHand, ref leftHandStartPosition, ref leftSwipeStartTime, true);
        }
        if (isRightHandInArea)
        {
            CheckSwipe(rightHand, ref rightHandStartPosition, ref rightSwipeStartTime, false);
        }
    }

    private void UpdateTextMesh()
    {
        if (leftHand.activeInHierarchy && rightHand.activeInHierarchy)
        {
            float zRotationLeft = leftHand.transform.eulerAngles.z;
            float zRotationRight = rightHand.transform.eulerAngles.z;
                // Imposta il testo del TextMeshPro con le rotazioni
                textRot.text = $"Left Z: {zRotationLeft:F2}°, Right Z: {zRotationRight:F2}°";
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
                    float zRotation = hand.transform.eulerAngles.z;

                    if (direction.magnitude > minimumSwipeDistance)
                    {
                        if (isLeftHand && direction.x > swipeThreshold && zRotation >= 60f && zRotation <= 120f)
                        {
                            if (!book.IsFirstPageGroup)
                            {
                                Debug.Log("Left hand swipe right detected");
                                RightCube.SetActive(false);
                                LeftCube.SetActive(true);
                                book.TurnToPage(book.CurrentLeftPageNumber - 2, EndlessBook.PageTurnTimeTypeEnum.TimePerPage, 1f);
                            }
                        }
                        else if (!isLeftHand && direction.x < -swipeThreshold && zRotation >= 240f && zRotation <= 300f)
                        {
                            if (!book.IsLastPageGroup)
                            {
                                Debug.Log("Right hand swipe left detected");
                                RightCube.SetActive(true);
                                LeftCube.SetActive(false);
                                book.TurnToPage(book.CurrentLeftPageNumber + 2, EndlessBook.PageTurnTimeTypeEnum.TimePerPage, 1f);
                            }
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
