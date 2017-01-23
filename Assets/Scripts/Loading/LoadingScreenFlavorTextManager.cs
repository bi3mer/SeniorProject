using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LoadingScreenFlavorTextManager
{
    private List<LoadingScreenFlavorText> texts;

    /// <summary>
    /// Creates a new flvor text manager and loads flavor text in.
    /// </summary>
    /// <param name="filename">Location of file with flavor text.</param>
    public LoadingScreenFlavorTextManager (string filename)
    {
        LoadingScreenFlavorTextYAMLParser parser = new LoadingScreenFlavorTextYAMLParser(filename);
        texts = parser.Load();
    }

    /// <summary>
    /// Returns the a random flavor text.
    /// </summary>
    /// <returns>A random flavor text.</returns>
    public LoadingScreenFlavorText GetRandom ()
    {
        return texts[Random.Range(0, texts.Count)];
    }
}
