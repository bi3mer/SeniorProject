using UnityEngine;
using System.Collections;

/// <summary>
/// A class that moves the player when they press a key. Used by fire escapes, certain doors, and the window washers.
/// </summary>
public class TeleportPlayer : MonoBehaviour
{
    public struct TeleportLocation
    {
        /// <summary>
        /// A location the player can teleport to.
        /// </summary>
        private Transform location;
        public Transform Location
        {
            get
            {
                return location;
            }
        }
        //Is this location currently accessable?
        public bool CanTeleportTo;
    }

    //The Location
    public TeleportLocation LocationA;
    public TeleportLocation LocationB;
}
