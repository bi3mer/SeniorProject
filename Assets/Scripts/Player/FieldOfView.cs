using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FieldOfView : MonoBehaviour
{
    // radius of the field of view
    public float ViewRadius;

    // angle the player will be able to see
    [Range(0, 360)]
    public float ViewAngle;

    // the closest interactable as well as the distance
    private Collider closestInteractable;
    private float closestDistance;

    // the previous closest collider
    private Collider prevInteractable;

    [SerializeField]
    private Animator playerAnimator;

    // all the layer that has all the interactables
    [SerializeField]
    private LayerMask interactablesMask;

    // the layer that has all the obstacles (ex.) walls, non-interactable objects, etc.)
    [SerializeField]
    [Tooltip("Any object that blocks player's view to interactable object.")]
    private LayerMask obstacleMask;

    private InteractableObject interactable;

    void Start()
    {
        // set the closest distance as nothing
        closestDistance = 0;
    }

    void Update()
    {
        FindVisibleInteractables();
    }


    /// <summary>
    /// Finds all the interactable objects within the player's field of view
    /// </summary>
    void FindVisibleInteractables()
    {
        // find the interactable objects within a sphere around the character
        Collider[] interactablesInRadius = Physics.OverlapSphere(playerAnimator.transform.position, ViewRadius, interactablesMask);

        // check all the items within the radius 
        for (int i = 0; i < interactablesInRadius.Length; ++i)
        {
            // get the direction to the interactable
            Transform target = interactablesInRadius[i].transform;
            Vector3 targetDir = (target.position - playerAnimator.transform.position).normalized;

            // check if angle between item is within view angle
            if (Vector3.Angle(playerAnimator.transform.forward, targetDir) < ViewAngle / 2)
            {
                float targetDist = Vector3.Distance(playerAnimator.transform.position, target.position);

                // check that the interactable object is not behind a non-interactable object
                if (!Physics.Raycast(playerAnimator.transform.position, targetDir, targetDist, obstacleMask))
                {
                    CheckClosestInteractable(interactablesInRadius[i], targetDist);
                }
            }
        }

        // show item if closest item and stop showing previous item
        if (closestInteractable != null && closestInteractable != prevInteractable && closestInteractable.CompareTag("Interactable"))
        {

            // only stop showing if there was a previous collider
            if (prevInteractable != null)
            {
                interactable.Show = false;
            }

            interactable = closestInteractable.GetComponent<InteractableObject>();
            interactable.Show = true;
        }

        closestDistance = 0;
        closestInteractable = null;
    }

    /// <summary>
    /// Check if an interactable is the closest interactable in view.
    /// </summary>
    /// <param name="interactable"></param>
    /// <param name="distance"></param>
    public void CheckClosestInteractable(Collider target, float targetDist)
    {
        // set first found interactable object as the closest item
        if (closestDistance == 0)
        {
            // set previous interactable
            prevInteractable = closestInteractable;

            closestInteractable = target;
            closestDistance = targetDist;
        }

        // if an interactable object is closer than previous closest object, set it as the closest
        else if (targetDist < closestDistance)
        {
            // set previous interactable
            prevInteractable = closestInteractable;

            closestInteractable = target;
            closestDistance = targetDist;
        }
    }

    /// <summary>
    /// Gets the direction of the angle. Used for editor mode to see how big the field of view will be.
    /// </summary>
    /// <param name="angleInDegrees"></param>
    /// <returns></returns>
    public Vector3 DirFromAngle(float angleInDegrees)
    {
        // shifts the angle to the front of the character.
        angleInDegrees += transform.eulerAngles.y;
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}