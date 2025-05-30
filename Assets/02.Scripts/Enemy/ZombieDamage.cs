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
            StartCoroutine(EnemyJump()); // ���� �ڷ�ƾ ����
        }
    }
    private void OnCollisionEnter(Collision col) // �ݹ� �Լ� ������ ȣ��ȴ�
    {
       if(col.gameObject.CompareTag(playertag))
        {
            this.rb.mass = 1000f;
            rb.isKinematic = true; // ���� ȿ�� ����
          
        }
       else if(col.gameObject.CompareTag(bulletTag))
        {
            anim.SetTrigger(hashHit); 
            Destroy(col.gameObject); 
            hp -= 25;
            
            hp = Mathf.Clamp(hp,0, maxHp); // ü���� 0�� �ִ� ü�� ���̷� ���� 
           
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

    private void OnCollisionExit(Collision col) // �ݹ� �Լ� ������ ȣ��ȴ�
    {
        if (col.gameObject.CompareTag(playertag))
        {
            this.rb.mass = 65f;
            rb.isKinematic = false; // ���� ȿ���� �ش�
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
        AnimatorClipInfo[] clipInfos = anim.GetCurrentAnimatorClipInfo(0); // �⺻ �ε��� ���̾� 

        yield return new WaitForSeconds(clipInfos.Length); // 1�� ���
        isJumping = false;
        agent.speed = 3.5f;
    }
}
