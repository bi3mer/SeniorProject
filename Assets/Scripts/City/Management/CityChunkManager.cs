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
    private int chunksAcross;
    private int chunksDown;

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
        // Grid the city, with the city center chunk being at chunk (0, 0)
        chunksAcross = Mathf.FloorToInt(city.BoundingBox.extents.x / ChunkSize);
        chunksDown   = Mathf.FloorToInt(city.BoundingBox.extents.y / ChunkSize);
        Chunks = new CityChunk[2 * chunksAcross, 2 * chunksDown];

        // Set up for building iterator
        Vector3 center = city.BoundingBox.center;
        IEnumerator<Building> buildings = Game.Instance.CityInstance.GetEnumerator();

        // Populate the grid data
        for (int i = -chunksAcross; i < chunksAcross; ++i)
        {
            for (int j = -chunksDown; j < chunksDown; ++j)
            {
                // Chunk coverage area
                Vector3 min = new Vector3(i * ChunkSize + center.x, city.BoundingBox.min.y, j * ChunkSize + center.z);
                Vector3 max = new Vector3((i + 1) * ChunkSize + center.x, city.BoundingBox.max.y, (j + 1) * ChunkSize + center.z);
                Bounds bounds = new Bounds(min, max);

                CityChunk chunk = new CityChunk(bounds);

                // Add buildings conatined in area
                buildings.Reset();
                while (buildings.MoveNext())
                {
                    if (bounds.Contains(buildings.Current.RootPosition))
                    {
                        chunk.Buildings.Add(buildings.Current);
                    }
                }

                Chunks[i, j] = chunk;
            }
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
                updateChunks();
                currentChunk = value;
            }
        }
    }

    /// <summary>
    /// Loads needed unloaded chunks and unload uneeded loaded chunks.
    /// </summary>
    private void updateChunks ()
    {
        if (FreezeLoading || Chunks == null)
        {
            return;
        }

        for (int i = -chunksAcross; i < chunksAcross; ++i)
        {
            for (int j = -chunksDown; j < chunksDown; ++j)
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

        StartCoroutine(chunk.Load());
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

        StartCoroutine(chunk.Unload());
    }
}
