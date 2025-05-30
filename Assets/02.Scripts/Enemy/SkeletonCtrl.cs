using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SkeletonCtrl : MonoBehaviour
{
    [SerializeField] private Transform playerTr;
    [SerializeField] private Transform skeletonTr;
    [SerializeField] private Animator animator;
    [SerializeField] private NavMeshAgent navi;
    [SerializeField] private float attackDis = 3f;
    [SerializeField] private float traceDis = 20.0f;
    [SerializeField] private SkeletonDamage S_damage;
    private readonly int hashAttack = Animator.StringToHash("IsAttack_B");
    private readonly int hashTrace = Animator.StringToHash("IsTrace_B");


    void Start()
    {
        skeletonTr = GetComponent<Transform>();
        animator = GetComponent<Animator>();
        navi = GetComponent<NavMeshAgent>();
        playerTr = GameObject.FindWithTag("Player").transform;
        S_damage = GetComponent<SkeletonDamage>();
    }

   
    void Update()
    {
        if ((S_damage.isDie))
        {
            return;
        }

       float dis =  Vector3.Distance(skeletonTr.position, playerTr.position);

        if(dis <= attackDis)
        {
            PlayerAttack();
        }
        else if(dis <= traceDis)
        {
            PlayerTrace();
        }
        else
        {
            PlayerIdle();
        }
    }

    private void PlayerIdle()
    {
        animator.SetBool(hashTrace, false);
        navi.isStopped = true;
    }

    private void PlayerTrace()
    {
        animator.SetBool(hashAttack, false);
        animator.SetBool(hashTrace, true);
        navi.isStopped = false;
        navi.destination = playerTr.position;
    }

    private void PlayerAttack()
    {
        animator.SetBool(hashAttack, true);
        navi.isStopped = true;

        Vector3 attackTarget = (playerTr.position - skeletonTr.position).normalized; // ¹æÇâ 

        Quaternion rot = Quaternion.LookRotation(attackTarget);
        skeletonTr.rotation = Quaternion.Slerp(skeletonTr.rotation, rot, Time.deltaTime * 10f);
    }
}
