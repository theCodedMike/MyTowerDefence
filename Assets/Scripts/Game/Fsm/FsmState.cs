using System.Collections.Generic;
using Game.Towers;
using UnityEngine;

namespace Game.Fsm
{
    public abstract class FsmState
    {
        // 攻击距离的平方
        protected float attackDistanceSqr;
        // 状态ID
        protected StateId stateId;
        public StateId ID => stateId;

        protected Dictionary<Transition, StateId> transitionToStateIdMap = new(8);

        protected FsmSystem fsmSystem;

        protected FsmState(float attackDistance, FsmSystem fsm)
        {
            this.attackDistanceSqr = Mathf.Pow(attackDistance, 2);
            this.fsmSystem = fsm;
        }

        public void AddTransition(Transition trans, StateId id)
        {
            if (trans == Transition.NoneTransition)
            {
                Debug.LogError("添加转移条件时，转移条件不能为None...");
                return;
            }

            if (id == StateId.NoneStateId)
            {
                Debug.LogError("添加转移条件时，状态ID不能为None...");
                return;
            }

            if (!transitionToStateIdMap.TryAdd(trans, id))
            {
                Debug.LogError($"{trans}已存在，不能重复添加...");
            }
        }

        public void DeleteTransition(Transition trans)
        {
            if (trans == Transition.NoneTransition)
            {
                Debug.LogError("删除转移条件时，转移条件不能为null...");
                return;
            }

            if (!transitionToStateIdMap.ContainsKey(trans))
            {
                Debug.LogError("删除转移条件时，转移条件不存在...");
                return;
            }

            transitionToStateIdMap.Remove(trans);
        }

        public StateId GetStateId(Transition trans) => transitionToStateIdMap.ContainsKey(trans)
            ? transitionToStateIdMap[trans]
            : StateId.NoneStateId;



        /// <summary>
        /// 转移到此状态前要执行的逻辑
        /// </summary>
        public virtual void DoBeforeEnterAction() { }

        /// <summary>
        /// 离开此状态后要执行的逻辑
        /// </summary>
        public virtual void DoAfterLeaveAction() { }

        /// <summary>
        /// 处于本状态时要执行的逻辑
        /// </summary>
        /// <param name="gameObject"></param>
        public abstract void CurrStateAction(GameObject gameObject);

        /// <summary>
        /// 转换到下一状态需要的条件
        /// </summary>
        /// <param name="gameObject"></param>
        public abstract void NextStateAction(GameObject gameObject);
    }
}
