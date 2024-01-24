using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RoguelikePlayer : MonoBehaviour
{
    [SerializeField] private AudioManagerSO audioManagerSO;
    [SerializeField] private QuestManager questManager;
    [SerializeField] [Range(0.1f,20f)] private float moveSpeed = 5f;
    [SerializeField] [Range(1f,10f)] private float actionRadius = 5f;
    [SerializeField] private ParticleSystem whistleParticleSystem;
    [SerializeField] private int npcLayer;
    [SerializeField] private Animator minimapAnimator;
    [SerializeField] private Camera minimapCamera;
    private RoguelikeInputactions roguelikeInputActions;
    private InputAction movement;
    private Animator playerAnimator;
    private bool isWalking = false;
    private bool isMinimapOpen = false;
    private bool isTouching = false;
    [SerializeField] private float marginRatio = 0.01f;
    private float touchTopMargin;
    private float touchBottomMargin;
    private float touchLeftMargin;
    private float touchRightMargin;
    private DialogManager dialogManager;

    private void Awake() {
        roguelikeInputActions = new RoguelikeInputactions();
        playerAnimator = GetComponent<Animator>();
        dialogManager = FindObjectOfType<DialogManager>();
    }

    private void OnEnable() {
        roguelikeInputActions.Player.Whistle.performed += Whistle;
        roguelikeInputActions.Player.Minimap.performed += ShowMinimap;
        roguelikeInputActions.Player.Minimap.canceled += HideMinimap;
        roguelikeInputActions.Player.MoveTouch.performed += IsTouching;
        roguelikeInputActions.Player.MoveTouch.canceled += IsNotTouching;
        roguelikeInputActions.Player.TouchMinimap.performed += ShowMinimap;
        roguelikeInputActions.UI.Accept.performed += Accept;
        roguelikeInputActions.UI.Disable();
        roguelikeInputActions.Player.Enable();
    }

    private void Accept(InputAction.CallbackContext obj)
    {
        dialogManager.NextDialog();
    }

    private void IsNotTouching(InputAction.CallbackContext obj)
    {
        isTouching = false;
    }

    private void IsTouching(InputAction.CallbackContext obj)
    {
        isTouching = true;
    }

    private void HideMinimap(InputAction.CallbackContext obj)
    {
        minimapCamera.enabled = false;
        isMinimapOpen = false;
        minimapAnimator.SetBool("isOpen", false);
    }

    private void ShowMinimap(InputAction.CallbackContext obj)
    {
        minimapCamera.enabled = true;
        isMinimapOpen = true;
        minimapAnimator.SetBool("isOpen", true);
    }

    public void DisableInputActions()
    {
        roguelikeInputActions.Player.Disable();
    }
    public void EnableInputActions()
    {
        roguelikeInputActions.Player.Enable();
    }

    public void EnableUIInputActions()
    {
        roguelikeInputActions.UI.Enable();
    }
    public void DisableUIInputActions()
    {
        roguelikeInputActions.UI.Disable();
    }


    private void Whistle(InputAction.CallbackContext obj)
    {
        if(isMinimapOpen)
        {
            HideMinimap(obj);
        }
        WhistleSFX();
        EndOrStartMission();
    }

    private void EndOrStartMission()
    {
        var colliders = Physics2D.OverlapCircleAll(transform.position, actionRadius, 1 << npcLayer);

        foreach(var collider in colliders)
        {
            var missionEnder = collider.GetComponent<MissionEnder>();
            var missionStarter = collider.GetComponent<MissionStarter>();
            if(missionEnder != null)
            {
                if(missionEnder.GetMissionCount() > 0)
                {
                    questManager.EndMission(missionEnder.DequeueMission());
                    return;
                }
            }
            if(missionStarter != null)
            {
                if(missionStarter.GetMissionCount() > 0)
                {
                    questManager.StartMission(missionStarter.DequeueMission());
                    return;
                }
            }
        }
    }

    private void WhistleSFX()
    {
        PlaySound("Whistle");
        PlayWhistleVFX();
    }

    private void PlayWhistleVFX()
    {
        Instantiate(whistleParticleSystem, transform.position, Quaternion.identity);
    }

    private void OnDisable() {
        roguelikeInputActions.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {
        touchTopMargin = Screen.height * (1 - marginRatio);
        touchRightMargin = Screen.width * (1 - marginRatio);
        touchBottomMargin = Screen.height * (marginRatio);
        touchLeftMargin = Screen.width * (marginRatio);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        MoveKeyBoard();
        MoveTouch();
    }

    private void MoveKeyBoard()
    {
        var direction = roguelikeInputActions.Player.Move.ReadValue<Vector2>();
        Move(direction);
    }

    private void MoveTouch()
    {
        var touchPosition = roguelikeInputActions.Player.TouchPosition.ReadValue<Vector2>();
        if(isTouching)
        {
            if(IsTouchInsideMargins(touchPosition))
            {
                var touchWorldPoint = Camera.main.ScreenToWorldPoint(new Vector3(touchPosition.x, touchPosition.y, 0));
                var screenCenterPoint = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width/2, Screen.height/2, 0));
                var direction = (new Vector2(touchWorldPoint.x, touchWorldPoint.y) - new Vector2(screenCenterPoint.x, screenCenterPoint.y)).normalized;
                Debug.Log("direction");
                Debug.Log(direction);
                Move(direction);
            }
        }
    }

    private bool IsTouchInsideMargins(Vector2 touchPosition)
    {
        if(touchPosition.x > touchRightMargin) return false;
        if(touchPosition.x < touchLeftMargin) return false;
        if(touchPosition.y > touchTopMargin) return false;
        if(touchPosition.y < touchBottomMargin) return false;
        return true;
    }

    private void Move(Vector2 direction)
    {
        if(direction.magnitude > 0)
        {
            transform.position += new Vector3(direction.x,direction.y,0) * moveSpeed * Time.deltaTime;
            if(!isWalking)
            {
                isWalking = true;
                playerAnimator.SetBool("isWalking",isWalking);
            }
        }
        else
        {
            if(isWalking)
            {
                isWalking = false;
                playerAnimator.SetBool("isWalking",isWalking);
            }
        }
    }

    public void PlaySound(string soundName)
    {
        audioManagerSO.Play(soundName, transform.position);
    }
    
    void OnDrawGizmosSelected()
    {
        // Display the explosion radius when selected
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, actionRadius);
    }

}
