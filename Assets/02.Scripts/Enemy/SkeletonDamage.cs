using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class SkeletonDamage : MonoBehaviour
{
    private Rigidbody rb;
    private readonly string playertag = "Player";
    private readonly string jumpTag = "JUMPSUPORT";
    private readonly int hashJump = Animator.StringToHash("IsJump_T");
    private bool isJumping = false;
    private Animator anim;
    private NavMeshAgent agent;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
    }
    private void OnCollisionEnter(Collision col) // 콜백 함수 스스로 호출된다
    {
       if(col.gameObject.CompareTag(playertag))
        {
            this.rb.mass = 1000f;
            rb.isKinematic = true; // 물리 효과 해제
        }
    }
    private void OnCollisionExit(Collision col) // 콜백 함수 스스로 호출된다
    {
        if (col.gameObject.CompareTag(playertag))
        {
            this.rb.mass = 65f;
            rb.isKinematic = false; // 물리 효과를 준다
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(jumpTag) && isJumping == false)
        {
            isJumping = true;
            anim.SetTrigger(hashJump);
            agent.speed = 0.1f;
        }
    }
    void Update()
    {
        if (isJumping && agent.isOnOffMeshLink)
        {
            StartCoroutine(EnemyJump()); // 점프 코루틴 시작
        }
    }
    IEnumerator EnemyJump()
    {
        yield return new WaitForSeconds(1f); // 1초 대기
        isJumping = false;
        agent.speed = 3.5f;
    }
}
