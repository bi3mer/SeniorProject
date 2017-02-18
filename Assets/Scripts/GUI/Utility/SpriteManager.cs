using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpriteManager
{
	private Dictionary<string, Sprite> spriteSheet;

	/// <summary>
	/// Initializes a new instance of the <see cref="SpriteManager"/> class.
	/// </summary>
	/// <param name="atlasPath">Atlas path.</param>
	public SpriteManager(string atlasPath)
	{
		spriteSheet = new Dictionary<string, Sprite>();

		Sprite[] sprites = Resources.LoadAll<Sprite>(atlasPath);

		for(int i = 0; i < sprites.Length; ++i)
		{
			spriteSheet.Add(sprites[i].name, sprites[i]);
		}
	}

	/// <summary>
	/// Gets the sprite from the sprite sheet by name.
	/// </summary>
	/// <returns>The sprite.</returns>
	/// <param name="name">Name.</param>
	public Sprite GetSprite(string name)
	{
		if(spriteSheet.ContainsKey(name))
		{
			return spriteSheet[name];
		}

		return null;
	}
}
