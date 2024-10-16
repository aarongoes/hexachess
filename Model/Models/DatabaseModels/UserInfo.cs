using System.Collections.Generic;

namespace Models
{
    public class UserInfo
    {
        public string name { get; set; }
        public int gameCount { get; set; }
        public Dictionary<string, int> gameModeCount { get; set; }

        public UserInfo()
        {
            
        }
        
        public UserInfo(string name, int gameCount, Dictionary<string, int> gameModeCount)
        {
            this.name = name;
            this.gameCount = gameCount;
            this.gameModeCount = gameModeCount;
        }
    }
}