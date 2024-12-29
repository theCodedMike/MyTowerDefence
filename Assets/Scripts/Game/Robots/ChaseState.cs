using System.Collections.Generic;
using Game.Fsm;
using Game.Skeletons;
using UnityEngine;

namespace Game.Robots
{
    public class ChaseState : FsmState
    {
        private float chaseDistanceSqr;

        private float moveSpeed = 2f;

        public ChaseState(float chaseDistance, float attackDistance, FsmSystem fsm) : base(attackDistance, fsm)
        {
            stateId = StateId.RobotChase;
            chaseDistanceSqr = Mathf.Pow(chaseDistance, 2);
        }

        public override void CurrStateAction(GameObject robotObj)
        {
            List<Transform> aliveSkeletons = SpawnSkeleton.Instance.GetAliveSkeletons();
            if (aliveSkeletons.Count == 0)
                return;

            Vector3 robotPosition = robotObj.transform.position;
            robotPosition.y = 0;
            // ReSharper disable once ComplexConditionExpression
            Transform closest = aliveSkeletons.Find(
                skeleton => (skeleton.position - robotPosition).sqrMagnitude <= chaseDistanceSqr);
            if (closest is null)
            {
                Debug.LogWarning("追逐状态下，没有可追逐的僵尸...");
                return;
            }

            robotObj.transform.LookAt(closest);
            robotObj.transform.Translate(Vector3.forward * Time.deltaTime * moveSpeed);
        }

        public override void NextStateAction(GameObject robotObj)
        {
            List<Transform> aliveSkeletons = SpawnSkeleton.Instance.GetAliveSkeletons();
            if (aliveSkeletons.Count == 0)
                return;

            Vector3 robotPosition = robotObj.transform.position;
            robotPosition.y = 0;

            bool attack = false;
            bool patrol = true;
            // ReSharper disable once ComplexConditionExpression
            foreach (Transform skeleton in aliveSkeletons)
            {
                float sqrMagnitude = (skeleton.position - robotPosition).sqrMagnitude;

                if (sqrMagnitude <= attackDistanceSqr) // 只要有1只僵尸进入攻击距离，就进入攻击状态
                {
                    attack = true;
                    break;
                }

                if (sqrMagnitude <= chaseDistanceSqr) // 只要有1只僵尸在追逐距离内，就不转移到巡逻状态
                {
                    patrol = false;
                    break;
                }
            }

            if (attack)
                fsmSystem.DoTransition(Transition.RobotCatchUpSkeleton);
            if (patrol)
                fsmSystem.DoTransition(Transition.RobotLoseSkeleton);
        }
    }
}
