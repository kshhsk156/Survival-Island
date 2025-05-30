using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]

public class ZombieCtrl : MonoBehaviour
{
    //1.NavMeshAgent를 이용 해서
    //2.Player가 추적 범위 안에 들어오면 추적하고
    //3.공격 범위 안에 들어오면 공격하는 로직 구현과 애니메이션 연동
    //4.추적범위와 공격범위를 구하려면 거리를 구해야함, 플레이어와 좀비의 위치를 알아야함
    [SerializeField] private NavMeshAgent navi;
    [SerializeField] private Animator animator;
    [SerializeField] private Transform zombieTr;
    public float rotSpeed = 30f;
    public Transform playerTr;


    public float traceDist = 20.0f; // 추점 범위
    public float attackDist = 3f; // 공격 범위

    private readonly int hashAttack = Animator.StringToHash("IsAttack_B"); //동적할당과 동시 문자열을 읽어서 정수로 변환 
    private readonly int hashTrace = Animator.StringToHash("IsTrace_B");


    void Start()
    {
        navi = GetComponent<NavMeshAgent>();
        playerTr = GameObject.FindWithTag("Player").transform; // 하이어라키에서 Player라는 태그를 가진 오브젝트의 트랜스폼을 가져옴 
        animator = GetComponent<Animator>();
        zombieTr = GetComponent<Transform>();
    }

    
    void Update()
    {
        float dist = Vector3.Distance(zombieTr.position,playerTr.position);
        //float dist = (playerTr.position - zombieTr.position).magnitude; // 벡터의 크기(거리)를 구함 (타겟 위치 - 자기자신)
        if (dist <= attackDist) // 공격중일 때
        {
            PlayerAttack();
        }
        else if(dist <= traceDist) // 추적중일 때
        {
            PlayerTrace();
        }
        else // 공격도 추적도 아닐때
        {
            PlayerIdle();
        }
    }

    private void PlayerIdle()
    {
        animator.SetBool(hashTrace, false);
        navi.isStopped = true; // 추적 범위 밖일 때 정지
    }

    private void PlayerTrace()
    {
        animator.SetBool(hashAttack, false);
        animator.SetBool(hashTrace, true);
        navi.isStopped = false; // 추적 범위 안에 들어오면 네비 추적 시작 
        navi.destination = playerTr.position;
    }

    private void PlayerAttack()
    {
        animator.SetBool(hashAttack, true);
        navi.isStopped = true; // 공격중일 때 네비 추적정지

        Vector3 attackTarget = (playerTr.position - zombieTr.position).normalized; // 공격 대상과의 거리
                                                                                   //타겟 위치 - 좀비 위치 = 방향 
        Quaternion rot = Quaternion.LookRotation(attackTarget); // 좀비가 플레이어를 바라보게 회전 
        zombieTr.rotation = Quaternion.Slerp(zombieTr.rotation, rot, Time.deltaTime * 10f);
        //곡면 보간 함수 //자기자신회전값, 타겟 회전, 시간만큼 회전 
    }
}
