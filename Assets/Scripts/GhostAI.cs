using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class GhostAI : MonoBehaviour
{
    public Transform[] spawnPoints; // ������ ���� ��ġ
    public Transform[] waypoints; // ������ ���� ��ġ
    public GameObject[] targetObjects; // Ÿ�� ������Ʈ��
    public Transform carryPosition; // �������� �� ���� ��ġ
    public float idleTimeMin = 10.0f; // ��� �ð� �ּҰ�
    public float idleTimeMax = 30.0f; // ��� �ð� �ִ밪
    public float moveTime = 5.0f; // �̵� �ð�
    public float runAwayTime = 3.0f; // ���� �ð�
    public Renderer ghostRenderer; // ������ ������

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
                    animator.SetBool("isRunning", false); // Idle ���·� ��ȯ
                    if (stateTimer <= 0)
                    {
                        currentState = State.Targeting;
                        stateTimer = 0; // Ÿ���� ���¿����� Ÿ�̸Ӱ� �ʿ� ����
                    }
                    break;

                case State.Targeting:
                    // Ÿ�� ������Ʈ ���� ����
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
                    // ���� ���� ��ġ ����
                    int spawnIndex = Random.Range(0, spawnPoints.Length);
                    transform.position = spawnPoints[spawnIndex].position;
                    ghostRenderer.enabled = true; // ���� Ȱ��ȭ
                    currentState = State.MoveToTargetObject;
                    break;

                case State.MoveToTargetObject:
                    // Ÿ�� ������Ʈ�� �̵�
                    if (currentTargetObject != null)
                    {
                        animator.SetBool("isRunning", true); // Run ���·� ��ȯ
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
                    // Ÿ�� ������Ʈ ���
                    if (currentTargetObject != null)
                    {
                        carriedObject = currentTargetObject;
                        carriedObject.transform.SetParent(this.transform);
                        carriedObject.transform.position = carryPosition != null ? carryPosition.position : transform.position + new Vector3(0, 1, 1); // ������ ��ġ �Ǵ� ���� �տ� ��ġ��Ŵ

                        // ������ ��ȣ�ۿ� ��Ȱ��ȭ
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
                        animator.SetBool("isRunning", true); // Run ���·� ��ȯ
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
                    // ���� ��������
                    if (carriedObject != null)
                    {
                        carriedObject.transform.SetParent(null);
                        // carriedObject.transform.position = transform.position + new Vector3(0, 0, 2); // ���� ��ġ �տ� ��������

                        // ������ ��ȣ�ۿ� Ȱ��ȭ
                        Rigidbody rb = carriedObject.GetComponent<Rigidbody>();
                        if (rb != null)
                        {
                            rb.isKinematic = false;
                        }

                        carriedObject = null;
                    }
                    // ���� ��Ȱ��ȭ ��� ������ �ʰ� ����
                    ghostRenderer.enabled = false;
                    stateTimer = Random.Range(idleTimeMin, idleTimeMax);
                    yield return new WaitForSeconds(stateTimer); // ��� �ð���ŭ ���
                    ghostRenderer.enabled = true;
                    currentState = State.Idle;
                    stateTimer = Random.Range(idleTimeMin, idleTimeMax);
                    break;
            }
        }
    }
}
