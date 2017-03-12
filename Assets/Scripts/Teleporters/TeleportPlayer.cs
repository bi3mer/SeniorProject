using UnityEngine;
using System.Collections;

/// <summary>
/// A class that moves the player when they press a key. Used by fire escapes, certain doors, and the window washers.
/// 
/// TODO: Make the class work with fire escapes.
/// </summary>
public class TeleportPlayer : MonoBehaviour
{
    [System.Serializable]
    public struct TeleportLocation
    {
        [SerializeField]
        private Transform location;
        /// <summary>
        /// A location the player can teleport to.
        /// </summary>
        public Transform Location
        {
            get
            {
                return location;
            }
        }
        // Is this location currently accessable?
        public bool CanTeleportTo;
    }

    // The Locations
    [SerializeField]
    private TeleportLocation LocationA;
    [SerializeField]
    private TeleportLocation LocationB;

    /// <summary>
    /// Teleports the player to the A position.
    /// </summary>
    public void TeleportToA()
    {
        if(LocationA.CanTeleportTo)
        { 
        Game.Instance.PlayerInstance.Controller.gameObject.transform.position = LocationA.Location.position;
        Game.Instance.PlayerInstance.Controller.gameObject.transform.rotation = LocationA.Location.rotation;
        }
    }

    /// <summary>
    /// Teleports the player to the B position.
    /// </summary>
    public void TeleportToB()
    {
        if (LocationB.CanTeleportTo)
        {
            Game.Instance.PlayerInstance.Controller.gameObject.transform.position = LocationB.Location.position;
            Game.Instance.PlayerInstance.Controller.gameObject.transform.rotation = LocationB.Location.rotation;
        }
    }
}
