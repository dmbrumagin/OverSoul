using UnityEngine;

namespace LevelManager
{
    [CreateAssetMenu(fileName = "New Level Setting", menuName = "LevelSetting/LevelSetting")]
    public class LevelSetting : ScriptableObject
    {
        public string levelName;
        public string songName;
        public int endOfLevel;
        public string itemLocation;
        public string monsterLocation;
        public string platformLocation;
    }
}