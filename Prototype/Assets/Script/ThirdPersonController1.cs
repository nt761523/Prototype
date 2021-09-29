using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonController1 : MonoBehaviour
{
    public float movespeed = 3.0f;
    public float turnspeed = 20f;
    public float jumpHeight = 1f;

    //��V�V�q
    Vector3 direction;
    Animator m_Animator;
    Rigidbody m_Rigidbody;
    float h;
    float v;
    bool isGrounded;
    bool isMoveing;
    void Start()
    {
        m_Animator = GetComponent<Animator>();
        m_Rigidbody = GetComponent<Rigidbody>();
        //Rigidbody���� = ���ʧ󥭷ơH
        m_Rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
    }
        // Update is called once per frame
    void FixedUpdate()
    {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");
        //�]�w��V�V�q����
        //direction.Set(h, 0f, v);
        //�V�q�зǤơ]�ܬ����V�q�^
        //direction.Normalize();
        //�]�w��V�V�q���ȡA�ñN�ӦV�q�зǤơ]�ܬ����V�q�^
        direction = new Vector3(h, 0f, v).normalized;
        isMoveing = h != 0 || v != 0;
        if (isMoveing && this.isGrounded)
        {
            Move();
        }
        Animation_Walk();

        if (Input.GetKeyDown("space") && this.isGrounded)
        {
            Jump(jumpHeight);
        }
    }    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Plane")
        {
            this.isGrounded = true;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.name == "Plane")
        {
            this.isGrounded = false;
        }
    }
    void Move()
    {
        Roration();
        //�첾�V�q = ���V�q x �t�� x ���ɶ�
        Vector3 moveVector = direction * movespeed * Time.deltaTime;
        //�ھڦ첾�V�q�A���Ⲿ�ʨ������m
        m_Rigidbody.MovePosition(m_Rigidbody.position + moveVector);
        Animation_Walk();
    }
    void Roration()
    {
        //�����V�V�q
        Vector3 targetDirection = new Vector3(h, 0f, v);
        float singleStep = turnspeed * Time.deltaTime;
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);
        transform.rotation = Quaternion.LookRotation(newDirection);
        ////�e�@�����u��ܥثe����¦V
        Debug.DrawRay(transform.position, newDirection, Color.red);
    }    
    void Animation_Walk()
    {
        //isMoveing = h != 0 || v != 0;
        //�]�m�����ʵe���A������
        m_Animator.SetBool("WalkSwitch", isMoveing);
    }
    void Jump(float jumpHeight)
    {
        m_Rigidbody.AddForce(Vector3.up * jumpHeight, ForceMode.Impulse);
    }
}
