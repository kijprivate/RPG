using System;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;
using UnityEngine.AI;

[RequireComponent(typeof (ThirdPersonCharacter))]
[RequireComponent(typeof(AICharacterControl))]
[RequireComponent(typeof(NavMeshAgent))]
public class PlayerMovement : MonoBehaviour
{
    ThirdPersonCharacter thirdPersonCharacter=null;   // A reference to the ThirdPersonCharacter on the object
    CameraRaycaster cameraRaycaster=null;
    Vector3 currentDestination,clickPoint;
    AICharacterControl aiCharacterControl=null;
    GameObject walkTarget = null;

    bool isInDirectMode = false;

    [SerializeField] const int walkableLayerNumber = 8;
    [SerializeField] const int enemyLayerNumber = 9;

    private void Start()
    {
        cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
        thirdPersonCharacter = GetComponent<ThirdPersonCharacter>();
        currentDestination = transform.position;

        aiCharacterControl = GetComponent<AICharacterControl>();
        walkTarget = new GameObject("walkTarget");

        cameraRaycaster.notifyMouseClickObservers += ProcessMouseClick;
    }

    void ProcessMouseClick(RaycastHit raycastHit,int layerHit)
    {
        switch (layerHit)
        {
            case enemyLayerNumber:
                GameObject enemy = raycastHit.collider.gameObject;
                aiCharacterControl.SetTarget(enemy.transform);
                break;
            case walkableLayerNumber:
                walkTarget.transform.position = raycastHit.point;
                aiCharacterControl.SetTarget(walkTarget.transform);
                break;
            default:
                Debug.LogWarning("Dont knot how to handle");
                return;
        }
    }
    // Fixed update is called in sync with physics
    //private void FixedUpdate()
    //{
    //    if (Input.GetKeyDown(KeyCode.G))
    //    {
    //        isInDirectMode = !isInDirectMode;
    //        currentDestination = transform.position;
    //    }

    //    if (isInDirectMode)
    //    {
    //        ProcessDirectMovement();
    //        print("keybouard control");
    //    }
    //    else if (!isInDirectMode)
    //    {
    //        PrecessMouseMovement();
    //        print("mouse control");
    //    }
    //}

    private void ProcessDirectMovement() {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");


        Vector3 camForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
        Vector3 movement = v * camForward + h * Camera.main.transform.right;

        thirdPersonCharacter.Move(movement, false, false);
    }

    Vector3 ShortDestination(Vector3 destination, float shortening) {
        Vector3 reductionVector = (destination - transform.position).normalized * shortening;
        return destination - reductionVector;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawLine(transform.position, currentDestination);
        Gizmos.DrawSphere(currentDestination, 0.1f);
        Gizmos.DrawSphere(clickPoint, 0.15f);

       // Gizmos.DrawWireSphere(transform.position, attackMoveStopRadius);
    }
}

