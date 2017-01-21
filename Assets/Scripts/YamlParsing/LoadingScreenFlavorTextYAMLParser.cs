using System.Collections;
using System.Collections.Generic;
using System.IO;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.RepresentationModel;
using UnityEngine;

/// <summary>
// Takes list of flavor text from yaml file and loads them into a list of flavor texts.
/// </summary>
public class LoadingScreenFlavorTextYAMLParser
{
    private string fileName;

    /// <summary>
    /// Awake this instance.
    /// </summary>
    public LoadingScreenFlavorTextYAMLParser(string file)
    {
        fileName = file;
    }

    /// <summary>
    /// Loads the flavor texts from the yaml file into list
    /// </summary>
    /// <returns>The list of flavor texts.</returns>
    public List<LoadingScreenFlavorText> Load()
    {
        // read in the yaml file and deserialize the input
        string file = FileManager.GetDocument(fileName);
        StringReader input = new StringReader(file);
        Deserializer deserializer = new Deserializer(namingConvention: new CamelCaseNamingConvention());

        // create a list of flavor text based on the yaml info and return them
        List<LoadingScreenFlavorText> texts = deserializer.Deserialize<List<LoadingScreenFlavorText>>(input);

        return texts;
    }
}
