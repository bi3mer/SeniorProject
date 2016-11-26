public abstract class EquipableCategory : ItemCategory 
{
	protected string toolType;
	protected const string equipActionName = "Equip";

	public void Equip()
	{
		Game.Instance.PlayerInstance.EquippedTool = toolType;
	}
}
