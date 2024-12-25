using System.Collections.Generic;
using UnityEngine;

namespace Game.Fsm
{
    public class FsmSystem
    {
        private Dictionary<StateId, FsmState> idToStateMap = new(8);

        private StateId currStateId; // 当前所处的状态ID

        private FsmState currState;  // 当前所处的状态

        /// <summary>
        /// 添加状态
        /// </summary>
        /// <param name="state"></param>
        public void AddState(FsmState state)
        {
            if (state is null)
            {
                Debug.LogError("添加状态时，参数state不能为null...");
                return;
            }

            if (currState is null)
            {
                currState = state;
                currStateId = state.ID;
            }

            if (!idToStateMap.TryAdd(state.ID, state))
            {
                Debug.LogError($"状态{state.ID}已存在于Map，无法添加...");
                return;
            }
        }


        /// <summary>
        /// 删除状态
        /// </summary>
        /// <param name="stateId"></param>
        public void DeleteState(StateId stateId)
        {
            if (stateId == StateId.NoneStateId)
            {
                Debug.LogError($"状态ID({stateId})为None，无法删除...");
                return;
            }

            if (!idToStateMap.ContainsKey(stateId))
            {
                Debug.LogError($"{stateId}不存在于当前Map，无法删除...");
                return;
            }

            idToStateMap.Remove(stateId);
        }

        /// <summary>
        /// 状态转换
        /// </summary>
        /// <param name="trans"></param>
        public void DoTransition(Transition trans)
        {
            if (trans == Transition.NoneTransition)
            {
                Debug.LogError("执行状态转移时，参数trans为None...");
                return;
            }

            StateId stateId = currState.GetStateId(trans);
            if (stateId == StateId.NoneStateId)
            {
                Debug.LogError("待转移的状态ID为None...");
                return;
            }

            if (!idToStateMap.ContainsKey(stateId))
            {
                Debug.LogError($"状态机中未找到状态ID({stateId})，无法转换状态...");
                return;
            }

            FsmState state = idToStateMap[stateId];
            currState.DoAfterLeaveAction();
            currState = state;
            currStateId = stateId;
            currState.DoBeforeEnterAction();
        }

        /// <summary>
        /// 更新当前的状态
        /// </summary>
        /// <param name="obj"></param>
        public void UpdateState(GameObject obj)
        {
            currState.CurrStateAction(obj);
            currState.NextStateAction(obj);
        }
    }
}
