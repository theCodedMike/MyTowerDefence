using System;
using Game;

namespace Utils
{
    [Serializable]
    public class TowerInfo
    {
        public TowerType type;

        public Level level;

        public string prefabPath; // 预制体路径

        public int price; // 价钱

        public int damage; // 炮弹伤害值
    }

    public enum Level
    {
        Normal, // 普通版的
        Upgraded, // 升级版的
    }
}
