using System;
using TMPro;
using UnityEngine;

namespace UI
{
    // ReSharper disable once HollowTypeName
    public class UIManager : MonoBehaviour
    {
        [Header("金币")]
        public TMP_Text gold;
        [Header("分数")]
        public TMP_Text score;
        [Header("更多面板")]
        public Transform morePanel;
        [Header("选择炮塔面板")]
        public Transform selectPanel;


        public static UIManager Instance;


        private void Awake()
        {
            Instance = this;
        }

        // Start is called before the first frame update
        private void Start()
        {
            gold.text = "100";
            score.text = "0";
        }


        private void UpdateGold(int gold)
        {
            this.gold.text = $"{gold}";
        }

        private void UpdateScore(int score)
        {
            this.score.text = $"{score}";
        }


        /// <summary>
        /// 关闭更多面板
        /// </summary>
        public void CloseMorePanel()
        {
            morePanel.gameObject.SetActive(false);
            print("更多面板关闭");
        }

        /// <summary>
        /// 显示更多面板
        /// </summary>
        public void DisplayMorePanel(float left, float bottom, float right, float top)
        {
            if (selectPanel.gameObject.activeSelf)
                CloseSelectTowerPanel();

            RectTransform rect = morePanel.GetComponent<RectTransform>();
            rect.offsetMin = new Vector2(left, bottom);
            rect.offsetMax = new Vector2(right, top);
            morePanel.gameObject.SetActive(true);
        }
        /// <summary>
        /// 选择服务
        /// 0 -> 升级
        /// 1 -> 出售
        /// </summary>
        /// <param name="choice"></param>
        public void SelectChoice(int choice)
        {
            print($"{(choice == 0 ? "升级" : "出售")}");
        }



        /// <summary>
        /// 选择塔的类型
        /// 0 -> 激光炮塔
        /// 1 -> 飞刀炮塔
        /// 2 -> 大炮
        /// </summary>
        /// <param name="type">类型</param>
        public void SelectTower(int towerType)
        {
            print($"{(TowerType)towerType}...");
        }

        /// <summary>
        /// 关闭选择塔类型面板
        /// </summary>
        public void CloseSelectTowerPanel()
        {
            selectPanel.gameObject.SetActive(false);
            print("选择Tower面板关闭");
        }

        /// <summary>
        /// 显示选择塔类型面板
        /// </summary>
        public void DisplaySelectTowerPanel(float left, float bottom, float right, float top)
        {
            if (morePanel.gameObject.activeSelf)
                CloseMorePanel();

            RectTransform rect = selectPanel.GetComponent<RectTransform>();
            rect.offsetMin = new Vector2(left, bottom);
            rect.offsetMax = new Vector2(right, top);
            selectPanel.gameObject.SetActive(true);
        }
    }

    public enum TowerType
    {
        LaserTower,
        KnifeTower,
        CannonTower,
    }
}
