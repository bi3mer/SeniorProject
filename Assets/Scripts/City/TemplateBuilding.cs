using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// A building based on a prefab.
/// </summary>
public class TemplateBuilding : Building
{
    /// <summary>
    /// Creates an instance of a <see cref="TemplateBuilding" /> class.
    /// </summary>
    /// <param name="template">The prefab gameobject.</param>
    /// <param name="parent">The gameobject the building should be parented to.</param>
    /// <param name="position">The position of the building.</param>
    public TemplateBuilding (Transform parent, Vector3 position, GameObject template)
    {
        Parent = parent;
        Position = position;
        Template = template;
        IsLoaded = false;
        Attachments = new List<GameObject>();
    }
    
    /// <summary>
    /// Prefab to instantiate.
    /// </summary>
    public GameObject Template
    {
        get;
        private set;
    }

    /// <summary>
    /// Loads the instance of the building into the scene.
    /// </summary>
    public override void Load()
    {
        if (IsLoaded)
        {
            return;
        }

        Instance = GameObject.Instantiate(Template) as GameObject;
        Instance.transform.position = Position;
        Instance.transform.parent = Parent;

        IsLoaded = true;
    }
}
