using System;
using Game;
using System.Collections.Generic;
using UnityEngine;

namespace Utils
{
    [CreateAssetMenu(fileName = "TowerContainer.asset", menuName = "CreateTowerContainer")]
    public class TowerContainer : ScriptableObject
    {
        public List<TowerInfo> towerAssets;


        private static TowerContainer instance;

        public static TowerContainer Instance
        {
            get { return instance ??= Resources.Load<TowerContainer>("TowerContainer"); }
        }

        // 获取塔的预制体的路径
        public string GetPrefabPath(TowerType type, Level level)
        {
            foreach (TowerInfo towerInfo in towerAssets)
            {
                if (towerInfo.type == type && towerInfo.level == level)
                    return towerInfo.prefabPath;
            }

            throw new ArgumentException($"未知的塔类型和等级：{type} - {level}");
        }

        // 获取塔的预制体的路径
        public TowerInfo GetTowerInfo(TowerType type, Level level)
        {
            foreach (TowerInfo towerInfo in towerAssets)
            {
                if (towerInfo.type == type && towerInfo.level == level)
                    return towerInfo;
            }

            throw new ArgumentException($"未知的塔类型和等级：{type} - {level}");
        }
    }
}