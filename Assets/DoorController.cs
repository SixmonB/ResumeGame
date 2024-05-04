using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    [SerializeField] private GameObject leftDoor;
    [SerializeField] private GameObject rightDoor;
    private float rotationDuration = 1.5f;
    void Start()
    {
        BossTrigger.current.onDoorwayTriggerEnter += OnDoorwayClose;
    }

    private void OnDoorwayClose()
    {
        StartCoroutine(CloseDoors());
    }

    IEnumerator CloseDoors()
    {
        Quaternion startRotationLeft = leftDoor.transform.rotation;
        Quaternion startRotationRight = rightDoor.transform.rotation;

        Quaternion targetRotation = Quaternion.identity; // This is the closed position

        float elapsedTime = 0.0f;
        while (elapsedTime < rotationDuration)
        {
            // Interpolate between the start and target rotations over time
            float t = elapsedTime / rotationDuration;
            leftDoor.transform.rotation = Quaternion.Slerp(startRotationLeft, targetRotation, t);
            rightDoor.transform.rotation = Quaternion.Slerp(startRotationRight, targetRotation, t);

            elapsedTime += Time.deltaTime;
            yield return null; // Wait for the next frame
        }

        // Ensure doors reach the exact closed position
        leftDoor.transform.rotation = targetRotation;
        rightDoor.transform.rotation = targetRotation;
    }
}
