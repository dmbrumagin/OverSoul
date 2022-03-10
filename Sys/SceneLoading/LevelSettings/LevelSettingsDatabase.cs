using System.Collections.Generic;
using UnityEngine;

namespace LevelManager
{
    public class LevelSettingsDatabase : MonoBehaviour
    {
        public LevelSetting[] levelSettings;
        public Dictionary<string, LevelSetting> GetLevel = new Dictionary<string, LevelSetting>();
        string level;

        private void Awake()
        {
            for (int i = 0; i < levelSettings.Length; i++)
            {
                level = levelSettings[i].levelName;
                GetLevel.Add(level, levelSettings[i]);
            }
        }

    }
}