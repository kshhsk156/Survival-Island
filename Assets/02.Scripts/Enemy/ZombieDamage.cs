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
    private readonly int hashHit = Animator.StringToHash("IsHit_T");
    private readonly int hashDie = Animator.StringToHash("IsDie_T");
    private readonly string bulletTag = "BULLET";
    private int hp;
    private int maxHp = 100; 
    private NavMeshAgent agent;
    public bool isDie = false;
    void Start()
    {
        hp = maxHp;
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
    }
    void Update()
    {
        if (isJumping && agent.isOnOffMeshLink)
        {
            StartCoroutine(EnemyJump()); // 점프 코루틴 시작
        }
    }
    private void OnCollisionEnter(Collision col) // 콜백 함수 스스로 호출된다
    {
       if(col.gameObject.CompareTag(playertag))
        {
            this.rb.mass = 1000f;
            rb.isKinematic = true; // 물리 효과 해제
          
        }
       else if(col.gameObject.CompareTag(bulletTag))
        {
            anim.SetTrigger(hashHit); 
            Destroy(col.gameObject); 
            hp -= 25;
            
            hp = Mathf.Clamp(hp,0, maxHp); // 체력을 0과 최대 체력 사이로 제한 
           
        }
       if(hp<=0)
        {
            Die();
        }
    }

    private void Die()
    {
        isDie = true;
        Destroy(gameObject, 10f);
        anim.SetTrigger(hashDie);   
        GetComponent<CapsuleCollider>().enabled = false;
        GetComponent<Rigidbody>().isKinematic = true;
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

    IEnumerator EnemyJump()
    {
        AnimatorClipInfo[] clipInfos = anim.GetCurrentAnimatorClipInfo(0); // 기본 인덱스 레이어 

        yield return new WaitForSeconds(clipInfos.Length); // 1초 대기
        isJumping = false;
        agent.speed = 3.5f;
    }
}
