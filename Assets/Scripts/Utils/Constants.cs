using System.Numerics;

public static class Constants
{
	public const string SIMON_ID = "simon";
	public const string REACTION_SPEED_ID = "reactionSpeed";
	public const string MEMORY_ID = "memory";
	public const string JIGSAW_ID = "jigsaw";
	public const string KEYSEQUENCE_ID = "keysequnce";

	public static Vector3 BlueLightColorColor => new Vector3(0.8196079f, 1f, 1f);

	public static Vector3 GreenLightColor => new Vector3(0.72f, 1f, 0.85f);

	public static Vector3 RedLightColor => new Vector3(1f, 0.5f, 0.5f);

	public const string GreenEmissionMaterialPath = "Materials/Emissive - Green";

	public const string OrangeEmissionMaterialPath = "Materials/Emissive - Orange";

	public const string BlueEmissionMaterialPath = "Materials/Emissive - Blue";

	public const string RedEmissionMaterialPath = "Materials/Emissive - Red";

	public const string StationMaterialPath = "Materials/StationBase";
}
