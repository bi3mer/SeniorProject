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
        //Is this location currently accessable?
        public bool CanTeleportTo;
    }

    //The Location
    public TeleportLocation LocationA;
    public TeleportLocation LocationB;
}
