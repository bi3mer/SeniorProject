using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GridLayoutManager : MonoBehaviour 
{
	[Tooltip("The number of elements that should appear in which row")]
	public int ElementsPerRow;

	[Tooltip("Is there be a maximum number of rows that the ui is locked to?")]
	public bool UseMaxRows;

	[Tooltip("Maximum number of rows")]
	public int MaxRows;

	[SerializeField]
	private GridLayoutGroup gridLayoutGroup;

	[SerializeField]
	private RectTransform contentPanel;

	/// <summary>
	/// Start this instance.
	/// </summary>
	void Start () 
	{
		SetGridSize();
	}

	/// <summary>
	/// Sets the size of the grid cells.
	/// </summary>
	public void SetGridSize()
	{
		// actual free space of the grid is determined by the size of the container, the padding, and the total spacing between each element
		float xDim = Mathf.FloorToInt((contentPanel.rect.width - (gridLayoutGroup.padding.right + gridLayoutGroup.padding.left 
										+ gridLayoutGroup.spacing.x * (ElementsPerRow - 1))) / ElementsPerRow);
		float yDim;

		if(UseMaxRows)
		{
			yDim = Mathf.FloorToInt((contentPanel.rect.height - (gridLayoutGroup.padding.top + gridLayoutGroup.padding.bottom 
									+ gridLayoutGroup.spacing.y * (ElementsPerRow - 1))) / MaxRows);
		}
		else
		{
			yDim = xDim;
		}

		gridLayoutGroup.cellSize = new Vector2(xDim, yDim);
	}
}
