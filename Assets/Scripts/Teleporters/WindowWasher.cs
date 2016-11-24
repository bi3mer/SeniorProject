using UnityEngine;
using System.Collections;

/// <summary>
/// Goes on the window washer, handles the ropes on the washer, moving it up with the water, and teleporting the player.
/// </summary>
public class WindowWasher : MonoBehaviour
{
    [SerializeField]
    private TeleportPlayer teleportLocations;

    private bool IsUp;

}
