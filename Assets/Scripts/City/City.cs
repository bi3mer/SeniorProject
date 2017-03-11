using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class City
{
    private Vector3 cityCenter;

    /// <summary>
    /// Creates a city.
    /// </summary>
    /// <param name="districts">The list of districts conatined in the city.</param>
    /// <param name="boundingBox">The bounding box that defines the size of the city.</param>
    /// <param name="cityCenter">The location of the city center.</param>
    /// <param name="tallestBuiling">The instance of the tallest building.</param>
    public City (District[] districts, Bounds boundingBox, Vector3 cityCenter, Building tallestBuiling)
    {
        Districts = districts;
        BoundingBox = boundingBox;
        Center = cityCenter;
        TallestBuilding = tallestBuiling;
    }

    /// <summary>
    /// The list of districts contained in the city.
    /// </summary>
    public District[] Districts
    {
        get;
        private set;
    }

    /// <summary>
    /// The bounding box that defines the size of the city.
    /// </summary>
    public Bounds BoundingBox
    {
        get;
        private set;
    }

    /// <summary>
    /// The position of the center of the city.
    /// </summary>
    public Vector3 Center
    {
        get;
        private set;
    }

    /// <summary>
    /// The tallest building located at the city center.
    /// </summary>
    public Building TallestBuilding
    {
        get;
        private set;
    }

    /// <summary>
    /// Class for creating an iterator to traverse the tree-strcuture of builings in the City.
    /// </summary>
    public class BuildingEnumerator : IEnumerator<Building>
    {
        private List<Building> buildings;
        private City city;
        private int current;

        /// <summary>
        /// Creates a builing enumeratir.
        /// </summary>
        /// <param name="city">City to traverse.</param>
        internal BuildingEnumerator(City city)
        {
            this.city = city;
            Reset();
        }

        /// <summary>
        /// Gets current building in iteration.
        /// </summary>
        public Building Current
        {
            get
            {
                return buildings[current];
            }
        }

        /// <summary>
        /// Gets current building in iteration.
        /// </summary>
        object IEnumerator.Current
        {
            get
            {
                return Current;
            }
        }

        /// <summary>
        /// Moves to the next building in iteration.
        /// </summary>
        /// <returns>True if index is in range.</returns>
        public bool MoveNext ()
        {
            return (++current < buildings.Count);
        }

        /// <summary>
        /// Disposes of the iterator.
        /// </summary>
        public void Dispose() { }

        /// <summary>
        /// Traverses the tree and adds all buildings to the iterator.
        /// </summary>
        public void Reset()
        {
            buildings = new List<Building>();
            current = 0;

            // Add all building in the city
            for (int i = 0; i < city.Districts.Length; ++i)
            {
                District district = city.Districts[i];

                for (int j = 0; j < district.Blocks.Count; ++j)
                {
                    Block block = district.Blocks[j];

                    for (int k = 0; k < block.Buildings.Count; ++k)
                    {
                        buildings.Add(block.Buildings[k]);
                    }
                }
            }
        }
    }

    /// <summary>
    /// Get an iterator that traverses tree-structure of buildings in the city.
    /// </summary>
    /// <returns>Building iterator.</returns>
    public IEnumerator<Building> GetEnumerator()
    {
        return new BuildingEnumerator(this);
    }
}
