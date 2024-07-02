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

    public float blendTime = 1.0f; // 외부에서 조절 가능한 파라미터

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
        // CinemachineBrain 컴포넌트 찾기
        cinemachineBrain = mainCamera.GetComponent<CinemachineBrain>();

        // 플레이어 1의 컴포넌트 찾기
        player1Camera = player1FollowCamera.GetComponent<CinemachineVirtualCamera>();
        player1Input = player1Armature.GetComponent<PlayerInput>();

        // 플레이어 2의 컴포넌트 찾기
        player2Camera = player2FollowCamera.GetComponent<CinemachineVirtualCamera>();
        player2Input = player2Armature.GetComponent<PlayerInput>();

        // 두 플레이어의 입력 비활성화
        player1Input.enabled = false;
        player2Input.enabled = false;

        // 플레이어 1 활성화
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
        // 두 플레이어의 입력 비활성화
        player1Input.enabled = false;
        player2Input.enabled = false;

        // 플레이어 1 활성화
        player1Input.enabled = true;

        // Cinemachine 카메라 전환 시간 설정
        SetCameraBlendTime(blendTime);

        // Cinemachine 카메라 전환
        player1Camera.Priority = 11;
        player2Camera.Priority = 9;

        // 플레이어 1의 holdPosition과 dropPosition 설정
        SelectionManager.Instance.SetPlayerPositions(player1HoldPosition, player1DropPosition);
    }

    void SwapToPlayer2()
    {
        // 두 플레이어의 입력 비활성화
        player1Input.enabled = false;
        player2Input.enabled = false;

        // 플레이어 2 활성화
        player2Input.enabled = true;

        // Cinemachine 카메라 전환 시간 설정
        SetCameraBlendTime(blendTime);

        // Cinemachine 카메라 전환
        player1Camera.Priority = 9;
        player2Camera.Priority = 11;

        // 플레이어 2의 holdPosition과 dropPosition 설정
        SelectionManager.Instance.SetPlayerPositions(player2HoldPosition, player2DropPosition);
    }

    void SetCameraBlendTime(float time)
    {
        // CinemachineBrain의 기본 블렌드 시간을 설정
        cinemachineBrain.m_DefaultBlend.m_Time = time;
    }
}
