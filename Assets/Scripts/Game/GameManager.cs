using System;
using System.Collections.Generic;
using UI;
using UnityEngine;

namespace Game
{
    // ReSharper disable once HollowTypeName
    public class GameManager : MonoBehaviour
    {
        private Camera mainCamera;

        private Dictionary<Vector3, Tower> towerBaseMap = new(16);

        private Vector3 currTowerBasePos;


        private int gold; // 金币

        private int score; // 得分

        // Start is called before the first frame update
        private void Start()
        {
            gold = 100;
            score = 0;
            mainCamera = Camera.main;
            UIManager.OnSelectTower = GenTower;
            UIManager.OnSelectMore = HandleMoreService;
        }

        // Update is called once per frame
        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 mousePosition = Input.mousePosition;
                Ray ray = mainCamera.ScreenPointToRay(mousePosition);
                if (Physics.Raycast(ray, out var hit))
                {
                    Transform basePos = hit.transform;
                    if (basePos.CompareTag("TowerBase"))
                    {
                        currTowerBasePos = basePos.position;
                        print($"恭喜你，你击中了塔基..., {currTowerBasePos}");

                        if (towerBaseMap.ContainsKey(currTowerBasePos))
                        {
                            UIManager.Instance.DisplayMorePanel(currTowerBasePos);
                        }
                        else
                        {
                            UIManager.Instance.DisplaySelectTowerPanel(currTowerBasePos);
                        }
                    }
                }
            }
        }

        // 创建各种类型的塔
        private void GenTower(TowerType type)
        {
            if (gold < 30)
            {
                Debug.LogError($"目前金币为{gold}，不足以购买塔...");
                return;
            }

            string prefabPath = type switch
            {
                TowerType.LaserTower => "Prefabs/Towers/LaserTower",
                TowerType.CannonTower => "Prefabs/Towers/CannonTower",
                TowerType.KnifeTower => "Prefabs/Towers/KnifeTower",
                _ => throw new ArgumentException($"未知的塔类型：{type}"),
            };

            GameObject prefab = Resources.Load<GameObject>(prefabPath);
            GameObject obj = Instantiate(prefab, currTowerBasePos, Quaternion.identity);
            Tower tower = obj.GetComponent<Tower>();
            tower.SetTowerType(type);

            towerBaseMap.Add(currTowerBasePos, tower);

            gold -= 30;
            UIManager.Instance.UpdateGold(gold);
        }

        // 处理升级和出售
        private void HandleMoreService(MoreType type)
        {
            print($"{type}");
        }
    }
}
