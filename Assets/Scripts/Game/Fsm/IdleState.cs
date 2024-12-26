using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;

namespace Game.Fsm
{
    public class IdleState : FsmState
    {
        public IdleState(FsmSystem fsm, TowerType type, float attackDistance) : base(type, attackDistance, fsm)
        {
            stateId = StateId.Idle;
        }

        /// <summary>
        /// 静止状态下执行的逻辑
        /// </summary>
        /// <param name="towerObj"></param>
        public override void CurrStateAction(GameObject towerObj)
        {
            towerObj.transform.Rotate(Vector3.up, 1);
        }

        /// <summary>
        /// 转移到下一状态(攻击状态)的判断逻辑
        /// </summary>
        /// <param name="towerObj"></param>
        public override void NextStateAction(GameObject towerObj)
        {
            List<Transform> skeletons = SpawnSkeleton.Instance.GetAliveSkeletons();
            if (skeletons.Count == 0)
                return;

            Vector3 towerPosition = towerObj.transform.position;
            float targetDistance = Mathf.Pow(attackDistance, 2);

            // ReSharper disable once ComplexConditionExpression
            if (skeletons.Any(skeleton => (skeleton.position - towerPosition).sqrMagnitude <= targetDistance))
            {
                fsmSystem.DoTransition(Transition.SeeSkeleton);
            }
        }
    }
}
