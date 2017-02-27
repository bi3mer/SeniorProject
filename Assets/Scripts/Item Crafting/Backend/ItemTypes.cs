public class ItemTypes 
{
	/// <summary>
	/// Rope like objects.
	/// </summary>
	public static string Rope = "rope";

	/// <summary>
	/// Hooked objects.
	/// </summary>
	public static string Hook = "hook";

	/// <summary>
	/// Hard, sharp objects.
	/// </summary>
	public static string Sharp = "sharp";

	/// <summary>
	/// Long, thing objects.
	/// </summary>
	public static string Rod = "rod";

	/// <summary>
	/// Objects that can hold other objects.
	/// </summary>
	public static string Container = "container";

	/// <summary>
	/// Edible objects.
	/// </summary>
	public static string Edible = "edible";

	/// <summary>
	/// Objects that can be equipped.
	/// </summary>
	public static string Equipable = "equipable";

	/// <summary>
	/// Objects that can start a fire.
	/// </summary>
	public static string Igniter = "igniter";

	/// <summary>
	/// Objects that can fuel a fire.
	/// </summary>
	public static string Fuel = "fuel";

	/// <summary>
	/// Objects that can heal.
	/// </summary>
	public static string Medicinal = "medicinal";

	/// <summary>
	/// Advanced sharp objects that are tools.
	/// </summary>
	public static string Blade = "blade";

	/// <summary>
	/// Objects that are cloth.
	/// </summary>
	public static string Cloth = "cloth";

	/// <summary>
	/// Basic solid objects that can be used as raw materials for carving, as base materials, etc.
	/// Like wooden blocks that can be carved into handles or idols. 
	/// </summary>
	public static string BaseSolid = "base solid";

	public static string[] Types = new string[]{Rope, Hook, Sharp, Rod, Container, Edible, Equipable, Igniter, Fuel, Medicinal, Blade, Cloth, BaseSolid};
}
