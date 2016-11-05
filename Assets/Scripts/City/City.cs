using UnityEngine;
using System.Collections;

public class City
{
    public City (District[] districts)
    {
        Districts = districts;
    }

    /// <summary>
    /// The list of districts contained in the city.
    /// </summary>
    public District[] Districts
    {
        get;
        private set;
    }
}
