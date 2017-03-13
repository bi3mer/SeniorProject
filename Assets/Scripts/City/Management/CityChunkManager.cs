using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Used to manage loading and unloading city building chunks according to player location in world.
/// </summary>
public class CityChunkManager : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The size across the chunks.")]
    private float chunkSize;

    [SerializeField]
    [Tooltip("The number of chunks away from the current to load.")]
    private int chunksAwayToLoad;

    private Tuple<int, int> currentChunk;

    /// <summary>
    /// Initialize configuration.
    /// </summary>
    void Start ()
    {
        ChunkSize = chunkSize;
        ChunksAwayToLoad = chunksAwayToLoad;
        FreezeLoading = false;
    }

    /// <summary>
    /// Divides city building into chunks.
    /// </summary>
    /// <param name="city">Initialized city.</param>
    public void Init (City city)
    {
        // Grid the city (extents are 1/2 the size of the city)
        ChunksAcross = 2 * Mathf.FloorToInt(city.BoundingBox.extents.x / ChunkSize) + 1;
        ChunksDown   = 2 * Mathf.FloorToInt(city.BoundingBox.extents.z / ChunkSize) + 1;
        Chunks = new CityChunk[ChunksAcross + 1, ChunksDown + 1];

        // Set up for building iterator
        IEnumerator<Building> buildings = Game.Instance.CityInstance.GetEnumerator();

        // Populate the grid data
        for (int i = 0; i <= ChunksAcross; ++i)
        {
            for (int j = 0; j <= ChunksDown; ++j)
            {
                // Chunk coverage area
                float x = i * ChunkSize + city.BoundingBox.center.x - city.BoundingBox.extents.x;
                float z = j * ChunkSize + city.BoundingBox.center.z - city.BoundingBox.extents.z;

                Bounds bounds = new Bounds(
                    new Vector3(x, city.BoundingBox.center.y, z), 
                    new Vector3(ChunkSize, city.BoundingBox.size.y, ChunkSize));

                CityChunk chunk = new CityChunk(i, j, bounds);

                buildings.Reset();
                do
                {
                    if (chunk.BoundingBox.Contains(buildings.Current.Position))
                    {
                        chunk.Buildings.Add(buildings.Current);
                    }
                }
                while (buildings.MoveNext());

                Chunks[i, j] = chunk;
            }
        }
    }

    /// <summary>
    /// Update current chunk based on player location.
    /// </summary>
    void Update ()
    {
        if (Game.Player.IsInWorld && Chunks != null)
        {
            CurrentChunk = worldLocationToGrid(Game.Player.WorldTransform.position);
        }
    }

    /// <summary>
    /// Size of chunks across.
    /// </summary>
    public float ChunkSize
    {
        get;
        private set;
    }

    /// <summary>
    /// Number of chunks away to load.
    /// </summary>
    public int ChunksAwayToLoad
    {
        get;
        private set;
    }

    /// <summary>
    /// Number of chunks to cover x-axis extent of city.
    /// </summary>
    public int ChunksAcross
    {
        get;
        private set;
    }

    /// <summary>
    /// Number of chunks to cover y-axis of city.
    /// </summary>
    public int ChunksDown
    {
        get;
        private set;
    }

    /// <summary>
    /// The grid of city chunks.
    /// </summary>
    public CityChunk[,] Chunks
    {
        get;
        private set;
    }

    /// <summary>
    /// If true, freezes city loading and unloading.
    /// </summary>
    public bool FreezeLoading
    {
        get;
        set;
    }

    /// <summary>
    /// Coordinate location of the chunk the player is currently in.
    /// </summary>
    public Tuple<int, int> CurrentChunk
    {
        get
        {
            return currentChunk;
        }
        set
        {
            if (value != currentChunk)
            {
                currentChunk = value;
                updateChunks();
            }
        }
    }

    /// <summary>
    /// Get grid coordinate location from location in world coordinates.
    /// </summary>
    /// <param name="location">Location in world coordinates to check.</param>
    /// <returns>Grid coordinate location that conatins the point.</returns>
    private Tuple<int, int> worldLocationToGrid(Vector3 location)
    {
        for (int i = 0; i <= ChunksAcross; ++i)
        {
            for (int j = 0; j <= ChunksDown; ++j)
            {
                if (Chunks[i, j].BoundingBox.Contains(location))
                {
                    return Chunks[i, j].Location;
                }
            }
        }

        // Not on grid, return closest chunk
        float min = Mathf.Infinity;
        Tuple<int, int> chunk = null;
        for (int i = 0; i <= ChunksAcross; ++i)
        {
            for (int j = 0; j <= ChunksDown; ++j)
            {
                float dist = Chunks[i, j].BoundingBox.SqrDistance(location);
                if (dist < min)
                {
                    min = dist;
                    chunk = Chunks[i, j].Location;
                }
            }
        }

        return chunk;
    }

    /// <summary>
    /// Loads needed unloaded chunks and unload uneeded loaded chunks.
    /// </summary>
    private void updateChunks ()
    {
        if (FreezeLoading)
        {
            return;
        }

        for (int i = 0; i <= ChunksAcross; ++i)
        {
            for (int j = 0; j <= ChunksDown; ++j)
            {
                if (Mathf.Abs(CurrentChunk.X - i) <= chunksAwayToLoad && Mathf.Abs(CurrentChunk.Y - j) <= chunksAwayToLoad)
                {
                    loadChunk(Chunks[i, j]);
                }
                else
                {
                    unloadChunk(Chunks[i, j]);
                }
            }
        }
    }

    /// <summary>
    /// Make sure the chunk is not already loaded, stop any previous unloading, and start loading
    /// </summary>
    private void loadChunk (CityChunk chunk)
    {
        if (chunk.IsLoaded)
        {
            return;
        }

        if (chunk.CurrentCoroutine != null)
        {
            StopCoroutine(chunk.CurrentCoroutine);
        }

        chunk.CurrentCoroutine = chunk.Load();
        StartCoroutine(chunk.CurrentCoroutine);
    }

    /// <summary>
    /// Make sure the chunk is not already unloaded, stop any previous loading, and start unloading
    /// </summary>
    private void unloadChunk (CityChunk chunk)
    {
        if (!chunk.IsLoaded)
        {
            return;
        }

        if (chunk.CurrentCoroutine != null)
        {
            StopCoroutine(chunk.CurrentCoroutine);
        }

        chunk.CurrentCoroutine = chunk.Unload();
        StartCoroutine(chunk.CurrentCoroutine);
    }
}
