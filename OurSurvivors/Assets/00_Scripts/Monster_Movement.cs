using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class Monster_Movement : MonoBehaviour
{
    public Transform target;
    public float speed = 3.0f;
    public float turnSpeed = 10f;
    public Rigidbody rb;

    private bool isSpawned = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Init(Transform player)
    {
        target = player;

        Rotate(GetDir(), false);
        StartCoroutine(SpawnStartCoroutine(transform.localScale));
    }

    IEnumerator SpawnStartCoroutine(Vector3 scaleEnd)
    {
        Vector3 ScaleStart = Vector3.zero;
        Vector3 ScaleEnd = scaleEnd;

        float duration = 0.5f;
        float timer = 0.0f;

        while (timer < duration)
        {
            float t = timer / duration;
            transform.localScale = Vector3.Lerp(ScaleStart, ScaleEnd, t);
            timer += Time.deltaTime;
            yield return null;
        }

        isSpawned = true;
    }

    private void FixedUpdate() // 물리기반
    {
        if (false == isSpawned)
            return;
        
        if (null == target)
            return;

        MoveAndRotate();
    }

    private void MoveAndRotate()
    {
        Vector3 dir = GetDir();
        float dist = dir.magnitude;
        Rotate(dir);
        if (0.01f < dist)
        {
            //transform.position += dir * speed * Time.deltaTime; 이러면 이동+충돌이라 떨림
            rb.MovePosition(rb.position + dir * speed * Time.fixedDeltaTime);
        }
    }

    Vector3 GetDir()
    {
        Vector3 dir = (target.position - transform.position).normalized;
        dir.y = 0f;
        return dir;
    }

    void Rotate(Vector3 dir, bool isLerp = true)
    {
        if (Vector3.zero != dir)
        {
            Quaternion targetRot = Quaternion.LookRotation(dir);
            if (isLerp)
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, turnSpeed * Time.deltaTime);
            else
                transform.rotation = targetRot;
        }
    }
}
