using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FXSplashManager : MonoBehaviour
{
    [SerializeField]
    private FXSplash splashFX;

    [SerializeField]
    private int splashPoolSize;
    public int SplashPoolSize
    {
        get
        {
            return splashPoolSize;
        }
        set
        {
            splashPoolSize = value;
            FillPool();
        }
    }

    private List<FXSplash> splashPool = new List<FXSplash>();

    private int listIndex;

    /// <summary>
    /// Populate the array.
    /// </summary>
    void Start()
    {
        FillPool();

        // We set the parent to null so that the splashes don't move with the player. The rest of the weather effects need to.
        transform.SetParent(null);
    }

    /// <summary>
    /// Fill the pool with splashes. (That souns like a fun summer day to me)
    /// </summary>
    public void FillPool()
    {
        if (splashPool.Count < SplashPoolSize)
        {
            int newSplashes = splashPoolSize - splashPool.Count;
            FXSplash newSplash;
            for (int i = 0; i < newSplashes; ++i)
            {
                newSplash = (FXSplash)Instantiate(splashFX, Vector3.zero, Quaternion.Euler(0f, 0f, 0f), transform);
                splashPool.Insert(listIndex, newSplash);
            }
        }
        else if (splashPool.Count > SplashPoolSize)
        {
            int destroySplashes = splashPool.Count - splashPoolSize;
            for (int i = 0; i < destroySplashes; ++i)
            {
                if (listIndex != 0 && splashPool.Count > 0)
                {
                    DestroyImmediate(splashPool[listIndex - 1].gameObject);
                    splashPool.RemoveAt(listIndex - 1);
                }
                else
                {
                    DestroyImmediate(splashPool[0].gameObject);
                    splashPool.RemoveAt(0);
                }
            }
        }

        if (listIndex > splashPool.Count)
        {
            listIndex = splashPool.Count;
        }
    }

    /// <summary>
    /// Place and play a water splash effect.
    /// </summary>
    /// <param name="position">The position of the new splash</param>
    /// <param name="scale">The scale of the new splash</param>
    /// <param name="allowAddingToPool">Can a new splash be added to the pool if there are no available splashes?</param>
    public void CreateSplash(Vector3 position, Vector3 scale, float speed = 1f, bool allowAddingToPool = false)
    {
        makeSplash(position, scale, speed, allowAddingToPool);
    }

    /// <summary>
    /// Place and play a water splash effect. Uniform Scale.
    /// </summary>
    /// <param name="position">The position of the new splash</param>
    /// <param name="scale">The scale of the new splash</param>
    /// <param name="allowAddingToPool">Can a new splash be added to the pool if there are no available splashes?</param>
    public void CreateSplash(Vector3 position, float scale, float speed = 1f, bool allowAddingToPool = false)
    {
        makeSplash(position, new Vector3(scale, scale, scale), speed, allowAddingToPool);
    }

    /// <summary>
    /// Place and play a water splash effect. Scale defaults to 1.
    /// </summary>
    /// <param name="position">The position of the new splash</param>
    /// <param name="scale">The scale of the new splash</param>
    /// <param name="allowAddingToPool">Can a new splash be added to the pool if there are no available splashes?</param>
    public void CreateSplash(Vector3 position, float speed = 1f, bool allowAddingToPool = false)
    {
        makeSplash(position, Vector3.one, speed, allowAddingToPool);
    }

    /// <summary>
    /// Plays a water splash effect. Called by the public methods.
    /// </summary>
    /// <param name="position">The position of the new splash</param>
    /// <param name="scale">The scale of the new splash</param>
    /// <param name="allowAddingToPool">Can a new splash be added to the pool if there are no available splashes?</param>
    private void makeSplash(Vector3 position, Vector3 scale, float speed, bool allowAddingToPool)
    {
        if (allowAddingToPool && (splashPool.Count == 0 || splashPool[listIndex].gameObject.activeSelf ))
        {
            ++SplashPoolSize;
        }

        // Find a splash to use in the pool... There's a few things we know that makes this easier.
        // If a splash in disabled, we can use it.
        // Each splash is enabled for a fixed amount of time.
        // If we can call splashes in order, by keeping a bookmark, if the next splash in the chain is enabled, no splash in the list will be disabled.
        if (SplashPoolSize > 0 && !splashPool[listIndex].gameObject.activeSelf)
        {
            splashPool[listIndex].transform.position = position;
            splashPool[listIndex].transform.localScale = scale;
            splashPool[listIndex].setAnimationSpeed(speed);
            // I want to rotate the splashes on their y axis randomly so they look a bit different.
            splashPool[listIndex].transform.eulerAngles = new Vector3(0f, Random.Range(0f, 360f), 0f);
            splashPool[listIndex].gameObject.SetActive(true);

            ++listIndex;
            if (listIndex > SplashPoolSize - 1)
            {
                listIndex = 0;
            }
        }
    }
}
