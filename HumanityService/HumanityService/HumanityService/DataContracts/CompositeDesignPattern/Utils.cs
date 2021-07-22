using System;

namespace HumanityService.DataContracts.CompositeDesignPattern
{
    public class Utils
    {
        static public long UnixTimeSeconds()
        {
            return DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        }

        static public string CreateId()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
