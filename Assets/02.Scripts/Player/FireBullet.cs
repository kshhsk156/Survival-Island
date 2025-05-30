using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//마우스 왼쪽 클릭으로 총알을 발사하는 스크립트 
//뭐가 필요? 1.FirePos 2.총알 발사 프리팹 3. 오디오 소스 클립
public class FireBullet : MonoBehaviour
{
    public Transform FirePos; // 총알 발사 위치
    public GameObject BulletPrefab; // 총알 프리팹
    private AudioSource source;
    public AudioClip fireSound;
    void Start()
    {
        source = GetComponent<AudioSource>();
    }

   
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Fire();
        }
    }
    void Fire()
    {
        Instantiate(BulletPrefab, FirePos.position, FirePos.rotation);
       
        source.PlayOneShot(fireSound,1.0f);
    }
}
