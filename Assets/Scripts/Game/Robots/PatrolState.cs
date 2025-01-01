using System.Collections.Generic;
using System.Linq;
using Game.Fsm;
using Game.Skeletons;
using UnityEngine;
using Utils;

namespace Game.Robots
{
    public class PatrolState : FsmState
    {
        private string robotName;

        private List<Transform> robotPath;

        private int idx = 0;
        private bool reverse = false;

        private float moveSpeed = 2f;

        // Start is called before the first frame update
        public PatrolState(string objName, float attackDistance, FsmSystem fsm) : base(attackDistance, fsm)
        {
            stateId = StateId.RobotPatrol;
            robotName = objName;
            robotPath = Env.Instance.GetChildTransform($"{robotName}Path").GetComponentsInChildren<Transform>().Skip(1).ToList();
        }

        public override void CurrStateAction(GameObject robotObj)
        {
            robotObj.transform.LookAt(robotPath[idx]);
            robotObj.transform.Translate(Vector3.forward * (Time.deltaTime * moveSpeed));

            if (idx == 0)
                reverse = false;
            else if (idx == robotPath.Count - 1)
                reverse = true;
            if ((robotObj.transform.position - robotPath[idx].position).sqrMagnitude <= 0.5)
            {
                if (reverse)
                    idx--;
                else
                    idx++;
            }
        }

        public override void NextStateAction(GameObject robotObj)
        {
            List<Transform> aliveSkeletons = SpawnSkeleton.Instance.GetAliveSkeletons();
            if (aliveSkeletons.Count == 0)
                return;

            Vector3 robotPosition = robotObj.transform.position;
            robotPosition.y = 0;
            // ReSharper disable once ComplexConditionExpression
            if (aliveSkeletons.Any(
                    skeleton => (skeleton.position - robotPosition).sqrMagnitude <= attackDistanceSqr)
                )
            {
                fsmSystem.DoTransition(Transition.RobotSeeSkeleton);
            }
        }
    }
}
