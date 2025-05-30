using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]

public class ZombieCtrl : MonoBehaviour
{
    //1.NavMeshAgent�� �̿� �ؼ�
    //2.Player�� ���� ���� �ȿ� ������ �����ϰ�
    //3.���� ���� �ȿ� ������ �����ϴ� ���� ������ �ִϸ��̼� ����
    //4.���������� ���ݹ����� ���Ϸ��� �Ÿ��� ���ؾ���, �÷��̾�� ������ ��ġ�� �˾ƾ���
    [SerializeField] private NavMeshAgent navi;
    [SerializeField] private Animator animator;
    [SerializeField] private Transform zombieTr;
    public float rotSpeed = 30f;
    public Transform playerTr;


    public float traceDist = 20.0f; // ���� ����
    public float attackDist = 3f; // ���� ����

    private readonly int hashAttack = Animator.StringToHash("IsAttack_B"); //�����Ҵ�� ���� ���ڿ��� �о ������ ��ȯ 
    private readonly int hashTrace = Animator.StringToHash("IsTrace_B");


    void Start()
    {
        navi = GetComponent<NavMeshAgent>();
        playerTr = GameObject.FindWithTag("Player").transform; // ���̾��Ű���� Player��� �±׸� ���� ������Ʈ�� Ʈ�������� ������ 
        animator = GetComponent<Animator>();
        zombieTr = GetComponent<Transform>();
    }

    
    void Update()
    {
        float dist = Vector3.Distance(zombieTr.position,playerTr.position);
        //float dist = (playerTr.position - zombieTr.position).magnitude; // ������ ũ��(�Ÿ�)�� ���� (Ÿ�� ��ġ - �ڱ��ڽ�)
        if (dist <= attackDist) // �������� ��
        {
            PlayerAttack();
        }
        else if(dist <= traceDist) // �������� ��
        {
            PlayerTrace();
        }
        else // ���ݵ� ������ �ƴҶ�
        {
            PlayerIdle();
        }
    }

    private void PlayerIdle()
    {
        animator.SetBool(hashTrace, false);
        navi.isStopped = true; // ���� ���� ���� �� ����
    }

    private void PlayerTrace()
    {
        animator.SetBool(hashAttack, false);
        animator.SetBool(hashTrace, true);
        navi.isStopped = false; // ���� ���� �ȿ� ������ �׺� ���� ���� 
        navi.destination = playerTr.position;
    }

    private void PlayerAttack()
    {
        animator.SetBool(hashAttack, true);
        navi.isStopped = true; // �������� �� �׺� ��������

        Vector3 attackTarget = (playerTr.position - zombieTr.position).normalized; // ���� ������ �Ÿ�
                                                                                   //Ÿ�� ��ġ - ���� ��ġ = ���� 
        Quaternion rot = Quaternion.LookRotation(attackTarget); // ���� �÷��̾ �ٶ󺸰� ȸ�� 
        zombieTr.rotation = Quaternion.Slerp(zombieTr.rotation, rot, Time.deltaTime * 10f);
        //��� ���� �Լ� //�ڱ��ڽ�ȸ����, Ÿ�� ȸ��, �ð���ŭ ȸ�� 
    }
}
