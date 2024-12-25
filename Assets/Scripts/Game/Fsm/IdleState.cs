using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;

namespace Game.Fsm
{
    public class IdleState : FsmState
    {
        // 当塔与僵尸的距离小于等于该距离时，塔开始攻击
        private float attackDistance;
        // 塔的类型
        private TowerType type;

        public IdleState(FsmSystem fsm, TowerType type, float attackDistance) : base(fsm)
        {
            stateId = StateId.Idle;
            this.type = type;
            this.attackDistance = attackDistance;
        }

        /// <summary>
        /// 静止状态下执行的逻辑
        /// </summary>
        /// <param name="obj"></param>
        public override void CurrStateAction(GameObject obj)
        {
            obj.transform.Rotate(Vector3.up, 1);
        }

        /// <summary>
        /// 转移到下一状态(攻击状态)的判断逻辑
        /// </summary>
        /// <param name="obj"></param>
        public override void NextStateAction(GameObject obj)
        {
            List<Transform> skeletons = SpawnSkeleton.Instance.GetAliveSkeletons();
            if (skeletons.Count == 0)
                return;

            Vector3 towerPosition = obj.transform.position;
            float targetDistance = Mathf.Pow(attackDistance, 2);

            // ReSharper disable once ComplexConditionExpression
            if (skeletons.Any(skeleton => (skeleton.position - towerPosition).sqrMagnitude <= targetDistance))
            {
                fsmSystem.DoTransition(Transition.SeeSkeleton);
            }
        }
    }
}
