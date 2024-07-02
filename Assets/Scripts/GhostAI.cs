using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class GhostAI : MonoBehaviour
{
    public Transform[] spawnPoints; // 유령의 스폰 위치
    public Transform[] waypoints; // 도망갈 랜덤 위치
    public GameObject[] targetObjects; // 타겟 오브젝트들
    public Transform carryPosition; // 아이템을 들 때의 위치
    public float idleTimeMin = 10.0f; // 대기 시간 최소값
    public float idleTimeMax = 30.0f; // 대기 시간 최대값
    public float moveTime = 5.0f; // 이동 시간
    public float runAwayTime = 3.0f; // 도망 시간
    public Renderer ghostRenderer; // 유령의 렌더러

    private NavMeshAgent agent;
    private GameObject currentTargetObject;
    private GameObject carriedObject;
    private Animator animator;

    public enum State { Idle, Targeting, Spawn, MoveToTargetObject, PickUp, MoveToRandomLocation, RunAway, End }
    [SerializeField]
    private State currentState;
    private float stateTimer;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        currentState = State.Idle;
        stateTimer = Random.Range(idleTimeMin, idleTimeMax);
        StartCoroutine(GhostFSM());
    }

    IEnumerator GhostFSM()
    {
        while (true)
        {
            yield return null;

            stateTimer -= Time.deltaTime;

            switch (currentState)
            {
                case State.Idle:
                    animator.SetBool("isRunning", false); // Idle 상태로 전환
                    if (stateTimer <= 0)
                    {
                        currentState = State.Targeting;
                        stateTimer = 0; // 타겟팅 상태에서는 타이머가 필요 없음
                    }
                    break;

                case State.Targeting:
                    // 타겟 오브젝트 랜덤 선택
                    if (targetObjects.Length > 0)
                    {
                        int randomIndex = Random.Range(0, targetObjects.Length);
                        currentTargetObject = targetObjects[randomIndex];
                        currentState = State.Spawn;
                    }
                    else
                    {
                        currentState = State.Idle;
                        stateTimer = Random.Range(idleTimeMin, idleTimeMax);
                    }
                    break;

                case State.Spawn:
                    // 유령 스폰 위치 설정
                    int spawnIndex = Random.Range(0, spawnPoints.Length);
                    transform.position = spawnPoints[spawnIndex].position;
                    ghostRenderer.enabled = true; // 유령 활성화
                    currentState = State.MoveToTargetObject;
                    break;

                case State.MoveToTargetObject:
                    // 타겟 오브젝트로 이동
                    if (currentTargetObject != null)
                    {
                        animator.SetBool("isRunning", true); // Run 상태로 전환
                        agent.SetDestination(currentTargetObject.transform.position);
                        if (Vector3.Distance(transform.position, currentTargetObject.transform.position) < 2)
                        {
                            currentState = State.PickUp;
                        }
                    }
                    else
                    {
                        currentState = State.Idle;
                        stateTimer = Random.Range(idleTimeMin, idleTimeMax);
                    }
                    break;

                case State.PickUp:
                    // 타겟 오브젝트 들기
                    if (currentTargetObject != null)
                    {
                        carriedObject = currentTargetObject;
                        carriedObject.transform.SetParent(this.transform);
                        carriedObject.transform.position = carryPosition != null ? carryPosition.position : transform.position + new Vector3(0, 1, 1); // 설정된 위치 또는 유령 앞에 위치시킴

                        // 물리적 상호작용 비활성화
                        Rigidbody rb = carriedObject.GetComponent<Rigidbody>();
                        if (rb != null)
                        {
                            rb.isKinematic = true;
                        }

                        currentState = State.MoveToRandomLocation;
                    }
                    else
                    {
                        currentState = State.Idle;
                        stateTimer = Random.Range(idleTimeMin, idleTimeMax);
                    }
                    break;

                case State.MoveToRandomLocation:
                    if (waypoints.Length > 0)
                    {
                        animator.SetBool("isRunning", true); // Run 상태로 전환
                        int randomIndex = Random.Range(0, waypoints.Length);
                        agent.SetDestination(waypoints[randomIndex].position);
                        currentState = State.RunAway;
                        stateTimer = runAwayTime;
                    }
                    break;

                case State.RunAway:
                    if (stateTimer <= 0 || Vector3.Distance(transform.position, agent.destination) < 2)
                    {
                        currentState = State.End;
                    }
                    break;

                case State.End:
                    // 물건 내려놓기
                    if (carriedObject != null)
                    {
                        carriedObject.transform.SetParent(null);
                        // carriedObject.transform.position = transform.position + new Vector3(0, 0, 2); // 현재 위치 앞에 내려놓기

                        // 물리적 상호작용 활성화
                        Rigidbody rb = carriedObject.GetComponent<Rigidbody>();
                        if (rb != null)
                        {
                            rb.isKinematic = false;
                        }

                        carriedObject = null;
                    }
                    // 유령 비활성화 대신 보이지 않게 설정
                    ghostRenderer.enabled = false;
                    stateTimer = Random.Range(idleTimeMin, idleTimeMax);
                    yield return new WaitForSeconds(stateTimer); // 대기 시간만큼 대기
                    ghostRenderer.enabled = true;
                    currentState = State.Idle;
                    stateTimer = Random.Range(idleTimeMin, idleTimeMax);
                    break;
            }
        }
    }
}
