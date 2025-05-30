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
    private void OnCollisionEnter(Collision col) // �ݹ� �Լ� ������ ȣ��ȴ�
    {
       if(col.gameObject.CompareTag(playertag))
        {
            this.rb.mass = 1000f;
            rb.isKinematic = true; // ���� ȿ�� ����
          
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

    void Update()
    {
        if(isJumping && agent.isOnOffMeshLink)
        {
            StartCoroutine(EnemyJump()); // ���� �ڷ�ƾ ����
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
