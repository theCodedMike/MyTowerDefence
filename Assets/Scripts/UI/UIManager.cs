using System;
using System.Collections.Generic;
using Game.Towers;
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
        [Header("生命值")]
        public TMP_Text life;
        [Header("更多面板")]
        public Transform morePanel;
        [Header("选择炮塔面板")]
        public Transform selectPanel;

        // 在选择塔的面板处选中某个类型的塔的事件
        public static Action<TowerType> OnSelectTower;
        // 在更多面板处选中某个服务的事件
        public static Action<MoreType> OnSelectMore;


        public static UIManager Instance;

        private static Dictionary<Vector3, Vector3> towerPosToPanelPosMap;

        private AudioSource audioSource;

        static UIManager()
        {
            // ReSharper disable once ComplexConditionExpression
            towerPosToPanelPosMap = new(16)
            {
                { new Vector3(-12.0f, -1.0f, -7.0f), new Vector3(390.98f, 616.91f, 0f) },
                { new Vector3(-9.0f, -1.0f, -19.25f), new Vector3(127.67f, 419.03f, 0f) },
                { new Vector3(3.0f, -1.0f, -15.75f), new Vector3(355.87f, 271.42f, 0f) },
                { new Vector3(6.0f, -1.0f, -7.0f), new Vector3(605.62f, 359.99f, 0f) },
                { new Vector3(-3.0f, -1.0f, 8.75f), new Vector3(699.77f, 608.14f, 0f) },
                { new Vector3(-18.0f, -1.0f, 0f), new Vector3(429.28f, 673.57f, 0f) },
                { new Vector3(-18.0f, -1.0f, 21.0f), new Vector3(672.64f, 678.35f, 0f) },
                { new Vector3(-3.0f, -1.0f, 12.25f), new Vector3(989.42f, 678.35f, 0f) },
                { new Vector3(18.0f, -1.0f, 7.0f), new Vector3(1353.27f, 359.99f, 0f) }
            };
        }


        private void Awake()
        {
            Instance = this;
        }

        // Start is called before the first frame update
        private void Start()
        {
            gold.text = "100";
            score.text = "0";
            life.text = "10/10";
            audioSource = GetComponent<AudioSource>();
        }


        public void UpdateGold(int gold)
        {
            this.gold.text = $"{gold}";
        }

        public void UpdateScore(int score)
        {
            this.score.text = $"{score}";
        }

        public void UpdateLife(int life)
        {
            this.life.text = $"{life}/10";
        }

        /// <summary>
        /// 关闭更多面板
        /// </summary>
        public void CloseMorePanel()
        {
            PlayPressBtn();

            morePanel.gameObject.SetActive(false);
        }

        /// <summary>
        /// 显示更多面板
        /// </summary>
        public void DisplayMorePanel(Vector3 towerBasePos)
        {
            if (selectPanel.gameObject.activeSelf)
                CloseSelectTowerPanel();

            SetPanelPos(towerBasePos, morePanel);
        }
        /// <summary>
        /// 选择服务
        /// 0 -> 升级
        /// 1 -> 出售
        /// </summary>
        /// <param name="choice"></param>
        public void SelectChoice(int choice)
        {
            CloseMorePanel();
            OnSelectMore((MoreType)choice);
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
            CloseSelectTowerPanel();
            OnSelectTower((TowerType)towerType);
        }

        /// <summary>
        /// 关闭选择塔类型面板
        /// </summary>
        public void CloseSelectTowerPanel()
        {
            PlayPressBtn();

            selectPanel.gameObject.SetActive(false);
        }

        /// <summary>
        /// 显示选择塔类型面板
        /// </summary>
        public void DisplaySelectTowerPanel(Vector3 towerBasePos)
        {
            if (morePanel.gameObject.activeSelf)
                CloseMorePanel();

            SetPanelPos(towerBasePos, selectPanel);
        }


        // 处理Panel的位置
        private void SetPanelPos(Vector3 towerBasePos, Transform panel)
        {
            if (!towerPosToPanelPosMap.ContainsKey(towerBasePos))
            {
                Debug.LogError($"塔基位置错误：{towerBasePos}");
                return;
            }

            RectTransform rect = panel.GetComponent<RectTransform>();
            rect.position = towerPosToPanelPosMap[towerBasePos];

            panel.gameObject.SetActive(true);
        }

        // 播放按钮音效
        private void PlayPressBtn()
        {
            audioSource.Play();
        }
    }
}
