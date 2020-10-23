using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

namespace Bomberfox
{
    public static class GameManager
    {
        public enum Level
        {
            None = 0,
            Menu,
            InGame,
            GameOver,
            Shop,
        }

        public static readonly Dictionary<Level, string> LevelNames = new Dictionary<Level, string>()
        {
            {Level.Menu, "MainMenu"},
            {Level.InGame, "TestingGrounds"}
        };

        public static bool ChangeLevel(Level level)
        {
            if (LevelNames.ContainsKey(level))
            {
                string levelName = LevelNames[level];
                SceneManager.LoadScene(levelName);
                return true;
            }

            return false;
        }
    }
}
