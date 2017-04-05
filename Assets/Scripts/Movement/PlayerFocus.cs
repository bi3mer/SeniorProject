using UnityEngine;
using System.Collections;
using RootMotion.FinalIK;
using DG.Tweening;

/// <summary>
/// Controls the player's head IK setup to have them look at the closest interactable if they can.
/// </summary>

[RequireComponent (typeof(PlayerController))]
public class PlayerFocus : MonoBehaviour
{
    [SerializeField]
    private PlayerController playerController;
    [SerializeField]
    private BipedIK bipedIk;

    [SerializeField]
    [Tooltip("How fast to tween the head to focus on the object and back to front.")]
    private float focusSpeed;

    [SerializeField]
    [Tooltip("The max distance to focus on an interactable.")]
    private float maxViewDistance;

    /// <summary>
    /// Get components if they were not set up in inspector.
    /// </summary>
    void Start ()
    {
        if(playerController == null)
        {
            playerController = GetComponent<PlayerController>();
        }
        if(bipedIk == null)
        {
            bipedIk = GetComponentInChildren<BipedIK>();
        }
	}

    /// <summary>
    /// Look at the object if it meets our requirements.
    /// </summary>
    void Update ()
    {
	    if (playerController.Interactable == null)
        {
            DOTween.To(() => bipedIk.solvers.lookAt.IKPositionWeight, x => bipedIk.solvers.lookAt.IKPositionWeight = x, 0f, focusSpeed);
        }
        // If the closest interactable is within view distance.
        else if (Vector3.Distance(playerController.Interactable.transform.position, transform.position) < maxViewDistance)
        {
            bipedIk.solvers.lookAt.target.DOMove(playerController.Interactable.transform.position, focusSpeed);
            DOTween.To(() => bipedIk.solvers.lookAt.IKPositionWeight, x => bipedIk.solvers.lookAt.IKPositionWeight = x, 1f, focusSpeed);
        }
    }

}
