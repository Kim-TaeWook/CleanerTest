using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;

public class PlayerSwap : MonoBehaviour
{
    public GameObject mainCamera;
    public GameObject player1FollowCamera;
    public GameObject player1Armature;
    public GameObject player2FollowCamera;
    public GameObject player2Armature;

    public float blendTime = 1.0f; // �ܺο��� ���� ������ �Ķ����

    private CinemachineVirtualCamera player1Camera;
    private CinemachineVirtualCamera player2Camera;

    private PlayerInput player1Input;
    private PlayerInput player2Input;

    private CinemachineBrain cinemachineBrain;

    public Transform player1HoldPosition;
    public Transform player1DropPosition;
    public Transform player2HoldPosition;
    public Transform player2DropPosition;

    void Start()
    {
        // CinemachineBrain ������Ʈ ã��
        cinemachineBrain = mainCamera.GetComponent<CinemachineBrain>();

        // �÷��̾� 1�� ������Ʈ ã��
        player1Camera = player1FollowCamera.GetComponent<CinemachineVirtualCamera>();
        player1Input = player1Armature.GetComponent<PlayerInput>();

        // �÷��̾� 2�� ������Ʈ ã��
        player2Camera = player2FollowCamera.GetComponent<CinemachineVirtualCamera>();
        player2Input = player2Armature.GetComponent<PlayerInput>();

        // �� �÷��̾��� �Է� ��Ȱ��ȭ
        player1Input.enabled = false;
        player2Input.enabled = false;

        // �÷��̾� 1 Ȱ��ȭ
        SwapToPlayer1();
    }

    void Update()
    {
        if (Keyboard.current.digit1Key.wasPressedThisFrame)
        {
            SwapToPlayer1();
        }
        else if (Keyboard.current.digit2Key.wasPressedThisFrame)
        {
            SwapToPlayer2();
        }
    }

    void SwapToPlayer1()
    {
        // �� �÷��̾��� �Է� ��Ȱ��ȭ
        player1Input.enabled = false;
        player2Input.enabled = false;

        // �÷��̾� 1 Ȱ��ȭ
        player1Input.enabled = true;

        // Cinemachine ī�޶� ��ȯ �ð� ����
        SetCameraBlendTime(blendTime);

        // Cinemachine ī�޶� ��ȯ
        player1Camera.Priority = 11;
        player2Camera.Priority = 9;

        // �÷��̾� 1�� holdPosition�� dropPosition ����
        SelectionManager.Instance.SetPlayerPositions(player1HoldPosition, player1DropPosition);
    }

    void SwapToPlayer2()
    {
        // �� �÷��̾��� �Է� ��Ȱ��ȭ
        player1Input.enabled = false;
        player2Input.enabled = false;

        // �÷��̾� 2 Ȱ��ȭ
        player2Input.enabled = true;

        // Cinemachine ī�޶� ��ȯ �ð� ����
        SetCameraBlendTime(blendTime);

        // Cinemachine ī�޶� ��ȯ
        player1Camera.Priority = 9;
        player2Camera.Priority = 11;

        // �÷��̾� 2�� holdPosition�� dropPosition ����
        SelectionManager.Instance.SetPlayerPositions(player2HoldPosition, player2DropPosition);
    }

    void SetCameraBlendTime(float time)
    {
        // CinemachineBrain�� �⺻ ���� �ð��� ����
        cinemachineBrain.m_DefaultBlend.m_Time = time;
    }
}
