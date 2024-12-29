using System.Collections.Generic;
using Game.Skeletons;
using Game.Towers;
using UI;
using UnityEngine;
using Utils;

namespace Game
{
    // ReSharper disable once HollowTypeName
    public class GameManager : MonoBehaviour
    {
        private Camera mainCamera;

        private Dictionary<Vector3, Tower> towerBaseMap = new(16);

        private Vector3 currTowerBasePos;

        public static bool gameOver;

        private int gold; // 金币
        private int score; // 得分
        private int life; // 生命值
        private AudioSource audioSource;

        // Start is called before the first frame update
        private void Start()
        {
            gold = 100;
            score = 0;
            life = 10;
            mainCamera = Camera.main;
            UIManager.OnSelectTower = GenTower;
            UIManager.OnSelectMore = HandleMoreService;
            Skeleton.OnDeath = HandleSkeletonDeath;
            Skeleton.OnArriveEnd = HandleArriveEnd;
            audioSource = GetComponent<AudioSource>();
        }

        // Update is called once per frame
        private void Update()
        {
            if (gameOver)
            {
                Debug.Log("游戏结束...");
                return;
            }

            if (Input.GetMouseButtonDown(0))
            {
                Vector3 mousePosition = Input.mousePosition;
                Ray ray = mainCamera.ScreenPointToRay(mousePosition);
                if (Physics.Raycast(ray, out var hit))
                {
                    Transform basePos = hit.transform;
                    if (basePos.CompareTag("TowerBase"))
                    {
                        audioSource.Play();

                        currTowerBasePos = basePos.position;

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
            DoCreateTower(type, Level.Normal);
        }

        // 创建塔
        private void DoCreateTower(TowerType type, Level level, Transform oldTower = null)
        {
            TowerInfo towerInfo = TowerContainer.Instance.GetTowerInfo(type, level);

            if (gold < towerInfo.price)
            {
                Debug.LogWarning($"目前金币为{gold}，不足以购买塔...");
                return;
            }

            // ReSharper disable once ComplexConditionExpression
            if (level == Level.Upgraded && oldTower != null)
            {
                Destroy(oldTower.gameObject);
            }

            GameObject prefab = Resources.Load<GameObject>(towerInfo.prefabPath);
            Vector3 copyPos = currTowerBasePos;
            copyPos.y = 0;
            GameObject obj = Instantiate(prefab, copyPos, Quaternion.identity);
            Tower tower = obj.GetComponent<Tower>();
            tower.SetTowerInfo(towerInfo);

            towerBaseMap[currTowerBasePos] = tower; // 覆盖添加

            gold -= towerInfo.price;
            UIManager.Instance.UpdateGold(gold);
        }

        // 处理升级和出售
        private void HandleMoreService(MoreType type)
        {
            Tower tower = towerBaseMap[currTowerBasePos];
            int towerPrice = tower.price;

            if (type == MoreType.Upgrade) // 升级
            {
                DoCreateTower(tower.type, Level.Upgraded, tower.transform);
            }
            else if (type == MoreType.Sell) //出售
            {
                towerBaseMap.Remove(currTowerBasePos);
                Destroy(tower.transform.gameObject);

                gold += towerPrice;
                UIManager.Instance.UpdateGold(gold);
            }
        }


        // 处理僵尸死亡事件
        private void HandleSkeletonDeath(int score)
        {
            this.gold += score;
            this.score += score;

            UIManager.Instance.UpdateGold(this.gold);
            UIManager.Instance.UpdateScore(this.score);
        }

        // 处理僵尸到达终点事件
        private void HandleArriveEnd()
        {
            if (life == 0)
                return;

            life--;
            if (life <= 0)
            {
                life = 0;
                gameOver = true;
            }
            UIManager.Instance.UpdateLife(life);
        }
    }
}
