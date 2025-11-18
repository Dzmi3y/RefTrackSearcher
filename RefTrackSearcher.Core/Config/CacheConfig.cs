namespace RefTrackSearcher.Core.Config;

public class CacheConfig
{
    public string Directory { get; set; } = string.Empty;
    public int Expiration { get; set; }
}