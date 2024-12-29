using System;
using Game;
using Game.Towers;

namespace Utils
{
    [Serializable]
    public class TowerInfo
    {
        public TowerType type;

        public Level level;

        public string prefabPath; // 预制体路径

        public int price; // 价钱

        public string bulletPath; // 子弹预制体的路径

        public int damage; // 子弹伤害值


        public override string ToString()
        {
            return $"type: {type}, level: {level}, prefabPath: {prefabPath}, price: {price}, bulletPath: {bulletPath}, damage: {damage}";
        }
    }

    public enum Level
    {
        Normal, // 普通版的
        Upgraded, // 升级版的
    }
}
