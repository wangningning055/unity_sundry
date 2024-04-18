public enum SourceType
{
	None = -1,
	UI = 0,
	Prefab = 1,
	Material = 2,
	Image = 3
}
public class GetPathBySourceType
{
	public static string getDirectorName(SourceType type)
	{
		return type.ToString();
	}
	public static SourceType getType(string dirName)
	{
		if(dirName == "Prefab")
			return  SourceType.Prefab;
		if(dirName == "UI")
			return SourceType.UI ;
		if(dirName == "Image")
			return  SourceType.Image;
		if(dirName == "Material")
			return  SourceType.Material;
		return SourceType.None;
	}
	public static string getSuffixName(SourceType type)
	{
		if(type == SourceType.Prefab ||type == SourceType.UI)
			return ".prefab";
		if(type == SourceType.Image)
			return ".jped";
		if(type == SourceType.Material)
			return ".mat";
		return "";
	}
	public static string GetPath(string sourceName, SourceType sourceType)
	{
		return "Assets/Raw/" + getDirectorName(sourceType) + "/" + sourceName + getSuffixName(sourceType);
	}
}