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
    private readonly string bulletTag = "BULLET";
    private readonly int hashJump = Animator.StringToHash("IsJump_T");
    private readonly int hashHit = Animator.StringToHash("IsHit_T");
    private readonly int hashDie = Animator.StringToHash("IsDie_T");
    private bool isJumping = false;
    private Animator anim;
    private NavMeshAgent agent;
    private int hp;
    private int maxHp = 100;
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

            hp = Mathf.Clamp(hp, 0, maxHp);
        }
       if(hp <= 0)
        {
            Die();
        }
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
   

    void Die()
    {
        isDie = true;
        anim.SetTrigger(hashDie);
        Destroy(gameObject, 5f);
        GetComponent<Rigidbody>().isKinematic = false;
        GetComponent<CapsuleCollider>().enabled = false;
    }
    IEnumerator EnemyJump()
    {
        yield return new WaitForSeconds(1f); // 1�� ���
        isJumping = false;
        agent.speed = 3.5f;
    }
}
