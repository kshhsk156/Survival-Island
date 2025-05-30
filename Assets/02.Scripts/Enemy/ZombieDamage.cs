using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class ZombieDamage : MonoBehaviour
{
    private Rigidbody rb;
    private readonly string playertag = "Player";
    private readonly string jumpTag = "JUMPSUPORT";
    private Animator anim;
    private bool isJumping = false;
    private readonly int hashJump = Animator.StringToHash("IsJump_T");
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
        if(isJumping && agent.isOnOffMeshLink)
        {
            StartCoroutine(EnemyJump()); // 점프 코루틴 시작
        }
    }
    IEnumerator EnemyJump()
    {
        AnimatorClipInfo[] clipInfos = anim.GetCurrentAnimatorClipInfo(0); // 기본 인덱스 레이어 

        yield return new WaitForSeconds(clipInfos.Length); // 1초 대기
        isJumping = false;
        agent.speed = 3.5f;
    }
}
