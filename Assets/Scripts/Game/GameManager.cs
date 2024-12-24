using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    // ReSharper disable once HollowTypeName
    public class GameManager : MonoBehaviour
    {
        private Camera mainCamera;

        private Dictionary<Vector3, Transform> towerBaseMap = new(16);


        // Start is called before the first frame update
        void Start()
        {
            mainCamera = Camera.main;
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 mousePosition = Input.mousePosition;
                print($"MousePos: {mousePosition}");
                Ray ray = mainCamera.ScreenPointToRay(mousePosition);
                if (Physics.Raycast(ray, out var hit))
                {
                    Transform basePos = hit.transform;
                    print($"{basePos.position}");
                    if (basePos.CompareTag("TowerBase"))
                    {
                        print("恭喜你，你击中了塔基...");
                        if (towerBaseMap.ContainsKey(basePos.position))
                        {
                            print("Map中已存在该塔基");
                        }
                        else
                        {
                            print("已添加该塔基");
                            towerBaseMap.Add(basePos.position, basePos);
                        }
                    }
                }
            }
        }
    }
}
