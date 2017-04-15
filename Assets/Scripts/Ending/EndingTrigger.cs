using UnityEngine;
using System.Collections;

public class EndingTrigger : MonoBehaviour
{
    [SerializeField]
    private EndingController endingController;

    [Tooltip("Is this the death trigger?")]
    [SerializeField]
    private bool deathTrigger;
    
    /// <summary>
    /// Call the ending controller's ending script when something enters this trigger.
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerEnter (Collider other)
    {
        endingController.EndingTrigger(other, deathTrigger);
    }
}
