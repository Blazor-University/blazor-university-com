namespace TheFreezeTeam.Com;

public static class PathUtilities
{
  public static string CleanPath(string path)
  {
      var result = new string(path.Where(c => (!char.IsPunctuation(c)) || c == '-').ToArray());
      result = result.Replace(" ","-").ToLower();
      return result;
  }
}
