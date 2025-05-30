using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 1.�ڱ��ڽ� ������ z������ ���� �̵��Ѵ�.
public class BulletCtrl : MonoBehaviour
{
    public float speed = 1500f;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * speed);
        Destroy(this.gameObject, 3f);
    }
}
