using System.Collections.Generic;
using System.Linq;
using Game.Fsm;
using Game.Skeletons;
using UnityEngine;
using Utils;
using MyBullet = Game.Bullets.Bullet;

namespace Game.Robots
{
    public class AttackState : FsmState
    {
        private static readonly int Shoot = Animator.StringToHash("Shoot");

        private Animator animator;

        private int i = 0;

        public Transform[] bulletSpawnPoint;
        public GameObject bulletPrefab;
        public AudioSource audioSource;

        private float timer;
        private float fireSpeed = 2; // 个/秒

        // Start is called before the first frame update
        public AttackState(Animator animator, float attackDistance, FsmSystem fsm) : base(attackDistance, fsm)
        {
            stateId = StateId.RobotAttack;
            this.animator = animator;
        }

        public override void CurrStateAction(GameObject robotObj)
        {
            List<Transform> aliveSkeletons = SpawnSkeleton.Instance.GetAliveSkeletons();
            if (aliveSkeletons.Count == 0)
            {
                Debug.LogWarning("机器人攻击状态下，没有僵尸...");
                return;
            }

            Vector3 robotPosition = robotObj.transform.position;
            robotPosition.y = 0;

            // ReSharper disable once ComplexConditionExpression
            Transform targetSkeleton = aliveSkeletons.Find(
                skeleton => (skeleton.position - robotPosition).sqrMagnitude <= attackDistanceSqr);
            if (targetSkeleton is null)
            {
                Debug.LogWarning("机器人攻击状态下，没有可攻击的僵尸...");
                return;
            }

            Vector3 bodyPointPosition = targetSkeleton.Find("BodyPoint").position;

            // 使机器人面向僵尸
            robotObj.transform.forward = (bodyPointPosition - robotPosition).normalized;

            timer += Time.deltaTime;
            if (timer >= (1 / fireSpeed))
            {
                timer = 0;
                Fire();
            }
        }
        
        void Fire()
        {
            audioSource.Play();

            GameObject bulletObj = GameObjectPool.Instance.Get(bulletPrefab.name, bulletPrefab,
                bulletSpawnPoint[i].position, bulletSpawnPoint[i].rotation);
            //GameObject bulletObj = Object.Instantiate(bulletPrefab, bulletSpawnPoint[i].position, bulletSpawnPoint[i].rotation);
            MyBullet bullet = bulletObj.GetComponent<MyBullet>();
            bullet.PrefabName = bulletPrefab.name;
            bullet.SetVelocity(bulletSpawnPoint[i].forward);
            bullet.Damage = 30;

            i++;
            if (i >= 1) i = 0;
        }

        public override void NextStateAction(GameObject robotObj)
        {
            List<Transform> aliveSkeletons = SpawnSkeleton.Instance.GetAliveSkeletons();

            Vector3 robotPosition = robotObj.transform.position;
            robotPosition.y = 0;
            // ReSharper disable once ComplexConditionExpression
            if (aliveSkeletons.All(
                    skeleton => (skeleton.position - robotPosition).sqrMagnitude > attackDistanceSqr)
               )
            {
                fsmSystem.DoTransition(Transition.RobotLoseSkeleton);
            }
        }

        public override void DoBeforeEnterAction()
        {
            animator.SetBool(Shoot, true);
        }

        public override void DoAfterLeaveAction()
        {
            animator.SetBool(Shoot, false);
        }
    }
}
