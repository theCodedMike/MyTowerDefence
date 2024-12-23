using UnityEngine;
using UnityEngine.AI;

public class Skeleton : MonoBehaviour
{
    private Transform end;
    private NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        end = GameObject.FindWithTag("EndPoint").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (agent.enabled)
            agent.SetDestination(end.position);
    }
}
