using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//  namespace ?
//  �ش� �ڵ带 �׷�ȭ ��Ų��
//  1. �׷�ȭ�� �ڵ�� �׷쿡 �������� �ʴ� Ŭ���������� ����� �� ����
//  2. �׷쵵 �ڽ��� �ִ�
//  ����� ���� ��з�.�ߺз�.�Һз� ...

namespace KangJaeWook.SurvivalShooter.Characters
{
    public class PlayerMovement : MonoBehaviour
    {
        public float Speed = 6f;
        private Vector3 m_Movement;
        private Animator m_Animator;
        private Rigidbody m_Rigdbody;

        private void Awake()
        {
            m_Animator = GetComponent<Animator>();
            m_Rigdbody = GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            //  Input.GetAxis -> Smoothint filtering value (������ �ʴ����� ����� ��)
            //  GetAxisRaw �� ������ ���͸��� ������� �ʴ� ���� �ش�(-1, 0, 1)
            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");

            //  Movement
            //  normalized : �ش� ������ ũ�⸦ 1�� �����
            m_Movement.Set(h, 0, v);
            m_Movement = Speed * Time.deltaTime * m_Movement.normalized;

            m_Rigdbody.MovePosition(transform.position + m_Movement);

            //  Turn
            //  Ray ? ������ ��
            //  �������ε� �������� �߻��Ѵ�! (�������� ����)
            //  Cmaera.main.ScreenPointToRay(Vector3)
            //  �츮�� �����ִ� ȭ���� ���� ��ǥ�� ������ �������� ������ ���Ѵ�
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            //  Raycast : ������ ������ ������� �������� ���!
            //  �������� ù ��°�� ���� �����͸� �����´�
            //  1. Z �� ����(ī�޶� z�� �� ���� ����)���� �� ó�� �������� ���� �༮ �����͸� ����´�
            //  2. Collider �� �ִ� �༮�鸸 �´´�
            if (Physics.Raycast(ray, out RaycastHit hit, 100f, GameManager.FloorLayerMask))
            {
                //  RaycastHit ���� �������� ��ġ�� point ���� ������ �� �� �ִ�
                Vector3 point = hit.point - transform.position;
                point.y = 0;

                m_Rigdbody.MoveRotation(Quaternion.LookRotation(point));
            }

            //  Animation
            bool isWalking = h != 0f || v != 0f ;
            m_Animator.SetBool("IsWalking", isWalking);
        }
    }
}

