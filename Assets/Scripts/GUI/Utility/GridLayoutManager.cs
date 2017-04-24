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
	[Tooltip("The panel whose dimensions will drive the size of the grid layout")]
	private RectTransform dimensionDrivingPanel;

	[SerializeField]
	[Tooltip("Scroll bar to take into account")]
	private RectTransform scrollBar;

	private float previousWidth;

	/// <summary>
	/// Awakens this instance.
	/// </summary>
	void Awake () 
	{
		SetGridSize();
		gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
		gridLayoutGroup.constraintCount = ElementsPerRow;
	}

	void Update()
	{
	}


	/// <summary>
	/// Sets the size of the grid cells.
	/// </summary>
	public void SetGridSize()
	{
		// actual free space of the grid is determined by the size of the container, the padding, and the total spacing between each element

		float scrollBarWidth = 0f;

		if(scrollBar != null)
		{
			scrollBarWidth = scrollBar.rect.width;
		}

		float xDim = Mathf.FloorToInt((dimensionDrivingPanel.rect.width - (gridLayoutGroup.padding.right + gridLayoutGroup.padding.left 
										+ gridLayoutGroup.spacing.x * (ElementsPerRow - 1) + scrollBarWidth)) / ElementsPerRow);
		float yDim;

		if(UseMaxRows)
		{
			yDim = Mathf.FloorToInt((dimensionDrivingPanel.rect.height - (gridLayoutGroup.padding.top + gridLayoutGroup.padding.bottom 
									+ gridLayoutGroup.spacing.y * (ElementsPerRow - 1))) / MaxRows);
		}
		else
		{
			yDim = xDim;
		}

		gridLayoutGroup.cellSize = new Vector2(xDim, yDim);
		previousWidth = dimensionDrivingPanel.rect.width;
	}

	public void CheckGridSize()
	{
		StartCoroutine(checkContentPanelSize());
	}
	/// <summary>
	/// Checks the size of the grid.
	/// </summary>
	private IEnumerator checkContentPanelSize()
	{
		yield return null;

		if(previousWidth != dimensionDrivingPanel.rect.width)
		{
			SetGridSize();
		}
	}
}
