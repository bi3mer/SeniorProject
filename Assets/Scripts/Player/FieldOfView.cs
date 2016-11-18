using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FieldOfView : MonoBehaviour
{
    //radius of the field of view
    public float viewRadius;

    //angle the player will be able to see
    [Range(0, 360)]
    public float viewAngle;

    //the closest interactable as well as the distance
    private Collider closestInteractable;
    private float closestDistance;

    [SerializeField]
    private Animator playerAnimator;

    //all the layer that has all the interactables
    [SerializeField]
    private LayerMask interactablesMask;

    //the layer that has all the obstacles (ex.) walls, non-interactable objects, etc.)
    [SerializeField]
    private LayerMask obstacleMask;

    void Start()
    {
        //set the closest distance as nothing
        closestDistance = 0;
        StartCoroutine(FindInteractablesWithDelay(.2f));
    }

    /// <summary>
    /// Finds all visible interactable items every x seconds
    /// </summary>
    /// <param name="delay"></param>
    /// <returns></returns>
    IEnumerator FindInteractablesWithDelay(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            FindVisibleInteractables();
        }
    }

    /// <summary>
    /// Finds all the interactable objects within the player's field of view
    /// </summary>
    void FindVisibleInteractables()
    {
        //find the interactable objects within a sphere around the character
        Collider[] interactablesInRadius = Physics.OverlapSphere(playerAnimator.transform.position, viewRadius, interactablesMask);

        //check all the items within the radius 
        for (int i = 0; i < interactablesInRadius.Length; i++)
        {
            Transform interactable = interactablesInRadius[i].transform;
            Vector3 interactableDir = (interactable.position - playerAnimator.transform.position).normalized;
            if (Vector3.Angle(playerAnimator.transform.forward, interactableDir) < viewAngle / 2)
            {
                float interactableDist = Vector3.Distance(playerAnimator.transform.position, interactable.position);

                //check that the interactable object is not behind a non-interactable object
                if (!Physics.Raycast(playerAnimator.transform.position, interactableDir, interactableDist, obstacleMask))
                {
                    //set first found interactable object as the closest item
                    if (closestDistance == 0)
                    {
                        closestInteractable = interactablesInRadius[i];
                        closestDistance = interactableDist;
                    }

                    //if an interactable object is closer than previous closest object, set it as the closest
                    else if (interactableDist < closestDistance)
                    {
                        closestInteractable = interactablesInRadius[i];
                        interactableDist = closestDistance;
                    }
                }
            }

            // enter into the range of an interactable item
            if (closestInteractable.CompareTag("Interactable"))
            {
                //testing that the correct interactable is showing up
                Debug.Log(closestInteractable.name);
            }

            closestDistance = 0;
        }
    }

    /// <summary>
    /// Gets the direction of the angle.
    /// </summary>
    /// <param name="angleInDegrees"></param>
    /// <param name="angleIsGlobal"></param>
    /// <returns></returns>
    public Vector3 DirFromAngle(float angleInDegrees)
    {
        angleInDegrees += transform.eulerAngles.y;
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}