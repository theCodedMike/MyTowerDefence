using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;


namespace Game.Fsm
{
    public class AttackState : FsmState
    {
        public LineRenderer lineRenderer; // 激光射线渲染器
        public LayerMask mask; // 层遮罩
        
        
        private Ray shootRay;
        private float timer;
        private float fireSpeed = 1; // 生成子弹的速度(个/秒)
        private float displayPercentage = 0.6f; // 激光持续百分比


        public AttackState(FsmSystem fsm, TowerType type, float attackDistance) : base(type, attackDistance, fsm)
        {
            stateId = StateId.Attack;
        }


        /// <summary>
        /// 攻击状态下执行的逻辑
        /// </summary>
        /// <param name="towerObj"></param>
        public override void CurrStateAction(GameObject towerObj)
        {
            List<Transform> aliveSkeletons = SpawnSkeleton.Instance.GetAliveSkeletons();
            Vector3 towerPosition = towerObj.transform.position;
            Vector3 firePosition = towerObj.GetComponent<Tower>().firePoint.position;
            float targetDistance = Mathf.Pow(attackDistance, 2);

            // ReSharper disable once ComplexConditionExpression
            Transform targetSkeleton =
                aliveSkeletons.Find(skeleton => (skeleton.position - towerPosition).sqrMagnitude <= targetDistance);
            if (targetSkeleton is null)
            {
                Debug.Log("目前没有可攻击的僵尸...");
                return;
            }

            towerObj.transform.LookAt(targetSkeleton);

            timer += Time.deltaTime;
            if (timer >= (1 / fireSpeed))
            {
                timer = 0;

                if (type == TowerType.LaserTower)
                {
                    // 激光炮
                    lineRenderer.enabled = true;
                    shootRay.origin = firePosition;
                    Vector3 bodyPointPosition = targetSkeleton.Find("BodyPoint").position;
                    shootRay.direction = (bodyPointPosition - firePosition).normalized;

                    lineRenderer.positionCount = 2;
                    lineRenderer.SetPosition(0, firePosition);
                    lineRenderer.SetPosition(1, bodyPointPosition);

                    if (Physics.Raycast(shootRay, out var hit, Mathf.Infinity, mask))
                    {
                        //endPoint = hit.point;
                        if (hit.transform.CompareTag("Skeleton"))
                        {
                            hit.transform.GetComponent<Skeleton>().Damage();
                        }
                    }
                }
                else
                {
                    // 非激光炮
                    Vector3 dir = (targetSkeleton.position - towerPosition).normalized;
                    string prefabPath = type == TowerType.CannonTower ? "Prefabs/Bullets/CannonBullet" : "Prefabs/Bullets/KnifeBullet";
                    GameObject prefab = Resources.Load<GameObject>(prefabPath);
                    
                    GameObject bulletObj = Object.Instantiate(prefab, firePosition, Quaternion.Euler(towerObj.transform.localEulerAngles));
                    bulletObj.GetComponent<Bullet>().SetVelocity(dir);
                }
            }

            if (timer >= (displayPercentage / fireSpeed))
            {
                lineRenderer.enabled = false;
            }

        }

        /// <summary>
        /// 转移到下一状态(静止状态)的判断逻辑
        /// </summary>
        /// <param name="towerObj"></param>
        public override void NextStateAction(GameObject towerObj)
        {
            List<Transform> skeletons = SpawnSkeleton.Instance.GetAliveSkeletons();
            Vector3 towerPosition = towerObj.transform.position;
            float targetDistance = Mathf.Pow(attackDistance, 2);

            // ReSharper disable once ComplexConditionExpression
            if (skeletons.All(skeleton => (skeleton.position - towerPosition).sqrMagnitude > targetDistance))
            {
                fsmSystem.DoTransition(Transition.LoseSkeleton);
            }
        }

        public override void DoAfterLeaveAction()
        {
            if (lineRenderer.enabled)
                lineRenderer.enabled = false;
        }
    }
}
