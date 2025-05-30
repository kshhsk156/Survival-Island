using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//���콺 ���� Ŭ������ �Ѿ��� �߻��ϴ� ��ũ��Ʈ 
//���� �ʿ�? 1.FirePos 2.�Ѿ� �߻� ������ 3. ����� �ҽ� Ŭ��
public class FireBullet : MonoBehaviour
{
    public Transform FirePos; // �Ѿ� �߻� ��ġ
    public GameObject BulletPrefab; // �Ѿ� ������
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
