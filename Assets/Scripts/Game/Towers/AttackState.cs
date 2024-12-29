using System.Collections.Generic;
using System.Linq;
using Game.Bullets;
using Game.Fsm;
using Game.Skeletons;
using UnityEngine;
using Utils;

using MyBullet = Game.Bullets.Bullet;

namespace Game.Towers
{
    public class AttackState : FsmState
    {
        private float timer;
        private float fireSpeed = 1; // 生成子弹的速度(个/秒)
        private float displayPercentage = 0.2f; // 激光持续百分比
        private TowerType type; // 塔的类型
        private Level level; // 等级
        private GameObject bulletObj; // 子弹实例

        public AudioSource audioSource;
        public AudioClip laserClip;
        public AudioClip nonLaserClip;

        public AttackState(FsmSystem fsm, TowerType type, Level level, float attackDistance) : base(attackDistance, fsm)
        {
            stateId = StateId.TowerAttack;
            this.type = type;
            this.level = level;
        }


        /// <summary>
        /// 攻击状态下执行的逻辑
        /// </summary>
        /// <param name="towerObj"></param>
        public override void CurrStateAction(GameObject towerObj)
        {
            List<Transform> aliveSkeletons = SpawnSkeleton.Instance.GetAliveSkeletons();
            if (aliveSkeletons.Count == 0)
            {
                Debug.Log($"攻击状态下，目前没有僵尸...");
                return;
            }
            Vector3 towerPosition = towerObj.transform.position;
            // ReSharper disable once ComplexConditionExpression
            Transform targetSkeleton =
                aliveSkeletons.Find(skeleton => (skeleton.position - towerPosition).sqrMagnitude <= attackDistanceSqr);
            if (targetSkeleton is null)
            {
                Debug.Log($"攻击状态下，塔周围{(Mathf.Sqrt(attackDistanceSqr))}米内没有可攻击的僵尸...");
                return;
            }

            Transform bodyPoint = targetSkeleton.Find("BodyPoint");
            towerObj.transform.LookAt(bodyPoint);


            timer += Time.deltaTime;
            if (timer >= (1 / fireSpeed))
            {
                timer = 0;

                Vector3 firePosition = towerObj.transform.Find("FirePoint").position;
                TowerInfo towerInfo = TowerContainer.Instance.GetTowerInfo(type, level);
                GameObject bulletPrefab = Resources.Load<GameObject>(towerInfo.bulletPath);
                bulletObj = Object.Instantiate(bulletPrefab, firePosition, Quaternion.Euler(towerObj.transform.localEulerAngles));

                if (type == TowerType.LaserTower)
                {
                    audioSource.PlayOneShot(laserClip);

                    // 激光炮
                    Vector3 skeletonPosition = targetSkeleton.position;
                    skeletonPosition.y = 1f;
                    Vector3 direction = (skeletonPosition - firePosition).normalized;

                    bulletObj.transform.LookAt(skeletonPosition); // 这里控制渲染出来的激光的朝向

                    Laser laser = bulletObj.GetComponent<Laser>();
                    laser.Direction = direction; // 这里控制发出的探测射线的朝向
                    laser.Damage = towerInfo.damage;

                    laser.Render();
                }
                else
                {
                    audioSource.PlayOneShot(nonLaserClip);

                    // 非激光炮
                    Vector3 bodyPointPosition = bodyPoint.position;
                    bodyPointPosition.y = firePosition.y;
                    Vector3 direction = (bodyPointPosition - firePosition).normalized;

                    // ReSharper disable once ComplexConditionExpression
                    if (type == TowerType.KnifeTower && level == Level.Upgraded)
                    {
                        Vector3 firePosition2 = towerObj.transform.Find("FirePoint2").position;
                        GameObject bulletObj2 = Object.Instantiate(bulletPrefab, firePosition2, Quaternion.Euler(towerObj.transform.localEulerAngles));
                        MyBullet bullet2 = bulletObj2.GetComponent<MyBullet>();
                        bullet2.SetVelocity(direction);
                        bullet2.Damage = towerInfo.damage;
                    }

                    MyBullet bullet = bulletObj.GetComponent<MyBullet>();
                    bullet.SetVelocity(direction);
                    bullet.Damage = towerInfo.damage;
                }
            }

            // ReSharper disable once ComplexConditionExpression
            if (type == TowerType.LaserTower && (timer >= (displayPercentage / fireSpeed)) && bulletObj != null)
            {
                Object.Destroy(bulletObj);
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

            // ReSharper disable once ComplexConditionExpression
            if (skeletons.All(skeleton => (skeleton.position - towerPosition).sqrMagnitude > attackDistanceSqr))
            {
                fsmSystem.DoTransition(Transition.TowerLoseSkeleton);
            }
        }

        public override void DoAfterLeaveAction()
        {
            // ReSharper disable once ComplexConditionExpression
            if (type == TowerType.LaserTower && bulletObj != null)
                Object.Destroy(bulletObj);
        }
    }
}
