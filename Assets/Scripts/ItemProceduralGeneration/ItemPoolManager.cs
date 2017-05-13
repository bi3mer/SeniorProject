using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemPoolManager : MonoBehaviour 
{
	[Tooltip("How many objects will be initialized for the starting pool for common items")]
	[SerializeField]
	private int commonPoolAmount;

	[Tooltip("How many objects will be initialized for the starting pool for uncommon items")]
	[SerializeField]
	private int uncommonPoolAmount;

	[Tooltip("How many objects will be initialized for the starting pool for rare items")]
	[SerializeField]
	private int rarePoolAmount;

	[Tooltip("How many objects will be initialized for the starting pool for legendary items")]
	[SerializeField]
	private int legendaryPoolAmount;

	[Tooltip("How range of cells around the player that will show items. For example, if 5, then items will show up to 5 cells in all directions.")]
	[SerializeField]
	private int itemActivationDistnace;

	[Tooltip("GameObject that will be the parent of activated items.")]
	[SerializeField]
	private Transform activePool;

	[Tooltip("GameObject that will be the parent of inactive items.")]
	[SerializeField]
	private Transform inactivePool;

	/// <summary>
	/// The grid that contains information about what space is occupied in the city.
	/// </summary>
	protected ItemPoolInfo[,] grid;

	private Dictionary<string, List<GameObject>> itemPool;

	private Dictionary<string, int>  poolAmountByRarity;

	private float cityWidth;

	private float cityDepth;

	private Vector3 cityCenter;

	private float cellSize;

	private const float distanceUnitSize = 0.5f;

	private Tuple<int, int> currentLocation;

	private Tuple<int, int> previousLocation;

	private int gridsAwayToActivate;

	/// <summary>
	/// Sets up item pool manager.
	/// </summary>
	/// <param name="width">Width.</param>
	/// <param name="depth">Depth.</param>
	/// <param name="center">Center.</param>
	public void SetUpItemPoolManager (float width, float depth, Vector3 center) 
	{
		poolAmountByRarity = new Dictionary<string, int>();
		itemPool = new Dictionary<string, List<GameObject>>();

		poolAmountByRarity.Add(ItemRarity.Common, commonPoolAmount);
		poolAmountByRarity.Add(ItemRarity.Uncommon, uncommonPoolAmount);
		poolAmountByRarity.Add(ItemRarity.Rare, rarePoolAmount);
		poolAmountByRarity.Add(ItemRarity.Legendary, legendaryPoolAmount);

		cityWidth = width;
		cityDepth = depth;
		cityCenter = center;

		// the diagonal of the cell is the defaultMinDistanceAway
		// so the dimensions of the cellSize square should be divided by Mathf.Sqrt(2)
		// Since the diagonal of a square is Mathf.Sqrt(2) * outsideDimension
		// ex: If the dimensions of a square is 3, then the diagonal is Mathf.Sqrt(9 + 9) = 3 * Mathf.Sqrt(2)
		cellSize = distanceUnitSize/Mathf.Sqrt(2);
		grid = new ItemPoolInfo[Mathf.CeilToInt (cityWidth / cellSize), Mathf.CeilToInt (cityDepth / cellSize)]; 

		gridsAwayToActivate = Mathf.CeilToInt(itemActivationDistnace/cellSize);
		Game.Instance.ItemPoolInstance = this;
	}
	
	/// <summary>
	/// Update this instance.
	/// </summary>
	void Update () 
	{
		if(playerMoved())
		{
			updateActivatedObjects();
		}
	}

	/// <summary>
	/// Populates the starting item pool and activates the items in range. Does so one frame after start.
	/// </summary>
	/// <returns>The managing pool.</returns>
	public IEnumerator StartManagingPool()
	{
		populatePool();

		yield return new WaitForEndOfFrame();
		currentLocation = PointToGrid(new Vector2(Game.Instance.PlayerInstance.WorldTransform.position.x, 
												  Game.Instance.PlayerInstance.WorldTransform.position.z));

		activateInRange();
	}

	/// <summary>
	/// Get the coord access points to grid from a worldspace point. (0, 0) is at the lower left corner of the city.
	/// All values returned should be positive!
	/// </summary>
	/// <returns>The coord points used to access the ItemPoolInfo for that cell in the grid.</returns>
	/// <param name="location">Location of the point.</param>
	public Tuple<int, int> PointToGrid(Vector2 location)
	{
		int xLoc = (int)((location.x - cityCenter.x + cityWidth/2f) / cellSize);
		int yLoc = (int)((location.y - cityCenter.z + cityDepth/2f) / cellSize);

		if(xLoc >= 0 && yLoc >= 0 && xLoc < grid.GetLength(0) && yLoc < grid.GetLength(1))
		{
			return new Tuple<int, int> (xLoc, yLoc);
		}

		return null;
	}

	/// <summary>
	/// Populates the pool. Different rarities will have different pool sizes.
	/// </summary>
	private void populatePool()
	{
		Dictionary<string, List<string>> itemsByDistrict = Game.Instance.ItemFactoryInstance.LandItemsByDistrict;
		int amountToCreate = 0;
		BaseItem item;

		foreach(string key in itemsByDistrict.Keys)
		{
			for(int i = 0; i < itemsByDistrict.Count; ++i)
			{
				List<GameObject> pool = new List<GameObject>();
				item = Game.Instance.ItemFactoryInstance.GetBaseItem(itemsByDistrict[key][i]);
				amountToCreate = poolAmountByRarity[item.Rarity];

				if(!itemPool.ContainsKey(itemsByDistrict[key][i]))
				{
					itemPool.Add(itemsByDistrict[key][i], pool);
				}

				pool = itemPool[itemsByDistrict[key][i]];

				for(int j = pool.Count; j < amountToCreate; ++j)
				{
					pool.Add(Game.Instance.WorldItemFactoryInstance.CreatePickUpInteractableItem(item, 1));
				}
			}
		}
	}

	/// <summary>
	/// Whether or not the player was moved.
	/// </summary>
	/// <returns><c>true</c>, if player was moved, <c>false</c> otherwise.</returns>
	private bool playerMoved()
	{
		Tuple<int, int> playerLocation = PointToGrid(new Vector2(Game.Instance.PlayerInstance.WorldTransform.position.x, 
																 Game.Instance.PlayerInstance.WorldTransform.position.z));

		if(currentLocation != null && playerLocation != null)
		{
			if(playerLocation.X != currentLocation.X || playerLocation.Y != currentLocation.Y)
			{
				previousLocation = currentLocation;
				currentLocation = playerLocation;
				return true;
			}
		}

		return false;
	}

	/// <summary>
	/// Updates which objects should be activated and which should be deactivated.
	/// </summary>
	private void updateActivatedObjects()
	{
		deactivateOutOfRange();
		activateInRange();
	}

	/// <summary>
	/// Activates the items in range.
	/// </summary>
	private void activateInRange()
	{
		for(int i = currentLocation.X - gridsAwayToActivate; i < currentLocation.X + gridsAwayToActivate; ++i)
		{
			for(int j = currentLocation.Y - gridsAwayToActivate; j < currentLocation.Y + gridsAwayToActivate; ++j)
			{
				if((i > 0 && i < grid.GetLength(0)) && (j > 0 && j < grid.GetLength(1)))
				{
					if(grid[i, j] != null && !grid[i, j].Activated)
					{
						activateCell(ref grid[i, j]);
						grid[i, j].Activated = true;
					}
				}
			}
		}
	}

	/// <summary>
	/// Deactivates the items out of range.
	/// </summary>
	private void deactivateOutOfRange()
	{
		int xDiff = currentLocation.X - previousLocation.X;
		int yDiff = currentLocation.Y - previousLocation.Y;

		int xEdgeToCheck = gridsAwayToActivate;
		int yEdgeToCheck = gridsAwayToActivate;

		if(xDiff > 0)
		{
			xEdgeToCheck = -gridsAwayToActivate;
		}

		if(yDiff > 0)
		{
			yEdgeToCheck = -gridsAwayToActivate;
		}
			
		ItemPoolInfo xGridCellInfo = null;
		ItemPoolInfo yGridCellInfo = null;

		int xGridEdgeToRemove = currentLocation.X + xEdgeToCheck - xDiff;
		int yGridEdgeToRemove = currentLocation.Y + yEdgeToCheck - yDiff;

		if(xGridEdgeToRemove < 0 || xGridEdgeToRemove >= grid.GetLength(0) || yGridEdgeToRemove < 0 || yGridEdgeToRemove >= grid.GetLength(1))
		{
			return;
		}

		if(xDiff != 0 || yDiff != 0)
		{
			for(int i = 0; i < grid.GetLength(0); ++i)
			{
				 if(xDiff != 0)
				 {
				 	xGridCellInfo = grid[xGridEdgeToRemove, i];
				 }

				 if(yDiff != 0)
				 {
				 	yGridCellInfo = grid[i, yGridEdgeToRemove];
				 }

				if(xGridCellInfo != null && xGridCellInfo.Activated)
				{
					deactivateCell(ref xGridCellInfo);
				}

				if(yGridCellInfo != null && yGridCellInfo.Activated)
				{
					deactivateCell(ref yGridCellInfo);
				}
			}
		}
	}

	/// <summary>
	/// Activates the cell so that all items within the cell are in the world.
	/// </summary>
	/// <param name="gridCell">Grid cell.</param>
	private  void activateCell(ref ItemPoolInfo gridCell)
	{

		for(int i = 0; i < gridCell.ItemNames.Count; ++i)
		{
			gridCell.Items.Add (getItemFromPool (gridCell.ItemNames [i]));
			gridCell.Items [i].transform.position = gridCell.Locations [i];
			gridCell.Items [i].transform.SetParent (activePool);
		}
	}

	/// <summary>
	/// Deactivates the cell so that all items within the cell are not in the world.
	/// </summary>
	/// <param name="gridCell">Grid cell.</param>
	private void deactivateCell(ref ItemPoolInfo gridCell)
	{
		if(gridCell != null && gridCell.Activated)
		{
			for(int j  = 0; j < gridCell.Items.Count; ++j)
			{
				gridCell.Items[j].SetActive(false);
				gridCell.Items[j].transform.SetParent(inactivePool);

				if(!itemPool.ContainsKey(gridCell.ItemNames[j]))
				{
					itemPool.Add(gridCell.ItemNames[j], new List<GameObject>());
				}

				itemPool[gridCell.ItemNames[j]].Add(gridCell.Items[j]);

				gridCell.Items.RemoveAt(j);
			}

			gridCell.Activated = false;
		}
	}

	/// <summary>
	/// Removes an item from the world. Put back into item pool.
	/// </summary>
	/// <param name="item">Item.</param>
	public void RemoveItemFromWorld(GameObject item)
	{
		Tuple<int, int> itemGridLocation = PointToGrid(new Vector2(item.transform.position.x, item.transform.position.z));

		if(itemGridLocation != null)
		{
			ItemPoolInfo gridCell = grid[itemGridLocation.X, itemGridLocation.Y];
			int index = gridCell.Items.IndexOf(item);

			item.SetActive(false);

			if(itemPool.ContainsKey(gridCell.ItemNames[index]))
			{
				itemPool[gridCell.ItemNames[index]].Add(item);
			}
			else
			{
				itemPool.Add(gridCell.ItemNames[index], new List<GameObject> {item});
			}

			gridCell.RemoveItemInfo(index);
		}
	}

	/// <summary>
	/// Adds an item's information to grid.
	/// </summary>
	/// <param name="location">Location.</param>
	/// <param name="item">Item.</param>
	/// <param name="activated">If set to <c>true</c> activated.</param>
	public Tuple<int, int> AddToGrid(Vector3 location, string item, bool activated)
	{
		Tuple<int, int> coord = PointToGrid(new Vector2(location.x, location.z));

		if(coord != null)
		{
			if(grid[coord.X, coord.Y] == null)
			{
				grid[coord.X, coord.Y] = new ItemPoolInfo(location, item, activated);
			}
			else
			{
				grid[coord.X, coord.Y].ItemNames.Add(item);
				grid[coord.X, coord.Y].Locations.Add(location);
			}

		}
		else
		{
			Debug.LogError("Point " + location + " is out of city bounds.");
		}

		return coord;
	}

	/// <summary>
	/// Adds an item that has just appeared in the world and was not previously accounted for.
	/// </summary>
	/// <param name="item">Item.</param>
	public void AddItemFromWorld(GameObject item)
	{
		Tuple<int, int> itemGridLocation = AddToGrid(item.transform.position, item.name, true);

		if(itemGridLocation != null)
		{
			grid[itemGridLocation.X, itemGridLocation.Y].Items.Add(item);

			item.transform.SetParent(activePool);
			item.SetActive(true);
		}
	}

	/// <summary>
	/// Adds the item immediately to the world.
	/// </summary>
	/// <param name="location">Location.</param>
	/// <param name="item">Item.</param>
	public void AddItemImmediate(Vector3 location, string item)
	{
		GameObject itemToAdd = getItemFromPool(item);
		itemToAdd.transform.position = location;

		AddItemFromWorld(itemToAdd);
	}

	/// <summary>
	/// Gets the desired item from the pool. If the pool has no free items of the desired type, then a new one is created.
	/// </summary>
	/// <returns>The item from the pool.</returns>
	/// <param name="itemName">Item name.</param>
	private GameObject getItemFromPool(string itemName)
	{
		GameObject poolItem = null;
		int index = 0;

		if(itemPool.ContainsKey(itemName))
		{
			if(itemPool[itemName].Count > 0)
			{
				index = itemPool[itemName].Count - 1;
				poolItem = itemPool[itemName][index];
				itemPool[itemName].RemoveAt(index);
			}
			else
			{
				/// TODO: vary amount generated
				poolItem = Game.Instance.WorldItemFactoryInstance.CreatePickUpInteractableItem(Game.Instance.ItemFactoryInstance.GetBaseItem(itemName), 1);
			}
		}

		poolItem.SetActive(true);

		return poolItem;
	}

	/// <summary>
	/// Adds gameobject to the pool.
	/// </summary>
	/// <param name="itemName">Item name.</param>
	/// <param name="item">Item.</param>
	public void AddItemToPool(string itemName, GameObject item)
	{
		if(!itemPool.ContainsKey(itemName))
		{
			itemPool.Add(itemName, new List<GameObject>());
		}

		itemPool[itemName].Add(item);
		item.transform.SetParent(inactivePool);
	}

	/// <summary>
	/// Gets the item pool.
	/// </summary>
	/// <returns>The item pool.</returns>
	public ItemPoolInfo[,] GetItemPool()
	{
		return grid;
	}
}