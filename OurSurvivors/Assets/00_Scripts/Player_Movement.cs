using Unity.Mathematics;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Player_Movement : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public float rotateSpeed = 10.0f;
    public float camSpeed = 2.0f;
    public Vector3 cameraDir = Vector3.zero;
    public float detectionRadius = 10.0f;
    public LayerMask monsterLayer;

    private Transform target;
    private Camera camera;
    private Vector3 moveDir;
    private CharacterController controller;
    private Animator animator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        camera = Camera.main;
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        Move();
        Rotate();
        CameraMove();
    }

    private void Move()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        moveDir = new Vector3(h, 0, v).normalized;  // 항상1 고정. hv모두1이면 루트2
        controller.SimpleMove(moveDir * moveSpeed); // == move * Time.deltaTime
    }

    private void Rotate()
    {
        target = GetNearestMonster();
        if (null != target)
        {
            Vector3 dirToMonster = (target.position - transform.position);
            dirToMonster.y = 0f;
            RotateToQuaternion(dirToMonster);
        }        
        else if (0.01f < moveDir.sqrMagnitude) // magnitude(상대적으로 느림), sqrMagnitude(빠름) - 벡터의 길이 반환
        {
            RotateToQuaternion(moveDir);
        }
    }

    void RotateToQuaternion(Vector3 dir)
    {
        Quaternion targetRot = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotateSpeed * Time.deltaTime); // 곡선 st~en까지 t로
    }

    Transform GetNearestMonster()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, detectionRadius, monsterLayer);
        Transform nearest = null;
        float minDist = Mathf.Infinity;

        foreach(Collider col in hits)
        {
            float dist = Vector3.Distance(transform.position, col.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                nearest = col.transform;
            }
        }
        return nearest;
    }

    private void CameraMove()
    {
        camera.transform.position = Vector3.Lerp(
            camera.transform.position,
            transform.position + cameraDir,
            camSpeed * Time.deltaTime
            ); // 직선  st~en까지 t로
    }
}
