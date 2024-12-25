using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;


namespace Game.Fsm
{
    public class AttackState : FsmState
    {
        // 当塔与僵尸的距离大于该距离时，塔变为静止
        private float attackDistance;
        // 塔的类型
        private TowerType type;

        private readonly float moveSpeed = 500;

        private float timer;

        public AttackState(FsmSystem fsm, TowerType type, float attackDistance) : base(fsm)
        {
            stateId = StateId.Attack;
            this.type = type;
            this.attackDistance = attackDistance;
        }

        /// <summary>
        /// 攻击状态下执行的逻辑
        /// </summary>
        /// <param name="towerObj"></param>
        public override void CurrStateAction(GameObject towerObj)
        {
            List<Transform> aliveSkeletons = SpawnSkeleton.Instance.GetAliveSkeletons();
            Vector3 towerPosition = towerObj.transform.position;
            float targetDistance = Mathf.Pow(attackDistance, 2);

            // ReSharper disable once ComplexConditionExpression
            Transform targetSkeleton =
                aliveSkeletons.Find(skeleton => (skeleton.position - towerPosition).sqrMagnitude <= targetDistance);
            if (targetSkeleton is null)
                return;

            towerObj.transform.LookAt(targetSkeleton);

            Vector3 dir = (targetSkeleton.position - towerPosition).normalized;

            timer += Time.deltaTime;
            if (timer >= 0.5f)
            {
                timer = 0;

                if (type == TowerType.LaserTower)
                {

                }
                else
                {
                    string prefabPath = type == TowerType.CannonTower ? "Prefabs/Bullets/CannonBullet" : "Prefabs/Bullets/KnifeBullet";
                    GameObject prefab = Resources.Load<GameObject>(prefabPath);
                    Vector3 firePosition = towerObj.GetComponent<Tower>().firePoint.position;

                    GameObject bulletObj = Object.Instantiate(prefab, firePosition, Quaternion.identity);
                    bulletObj.GetComponent<Bullet>().SetVelocity(dir);
                }
            }

        }

        /// <summary>
        /// 转移到下一状态(静止状态)的判断逻辑
        /// </summary>
        /// <param name="obj"></param>
        public override void NextStateAction(GameObject obj)
        {
            List<Transform> skeletons = SpawnSkeleton.Instance.GetAliveSkeletons();
            Vector3 towerPosition = obj.transform.position;
            float targetDistance = Mathf.Pow(attackDistance, 2);


            // ReSharper disable once ComplexConditionExpression
            if (skeletons.All(skeleton => (skeleton.position - towerPosition).sqrMagnitude > targetDistance))
            {
                fsmSystem.DoTransition(Transition.LoseSkeleton);
            }
        }
    }
}
