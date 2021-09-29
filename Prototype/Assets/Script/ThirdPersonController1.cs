using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonController1 : MonoBehaviour
{
    public float movespeed = 3.0f;
    public float turnspeed = 20f;
    public float jumpHeight = 1f;

    //方向向量
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
        //Rigidbody插值 = 移動更平滑？
        m_Rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
    }
        // Update is called once per frame
    void FixedUpdate()
    {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");
        //設定方向向量的值
        //direction.Set(h, 0f, v);
        //向量標準化（變為單位向量）
        //direction.Normalize();
        //設定方向向量的值，並將該向量標準化（變為單位向量）
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
        //位移向量 = 單位向量 x 速度 x 單位時間
        Vector3 moveVector = direction * movespeed * Time.deltaTime;
        //根據位移向量，角色移動到對應位置
        m_Rigidbody.MovePosition(m_Rigidbody.position + moveVector);
        Animation_Walk();
    }
    void Roration()
    {
        //獲取轉向向量
        Vector3 targetDirection = new Vector3(h, 0f, v);
        float singleStep = turnspeed * Time.deltaTime;
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);
        transform.rotation = Quaternion.LookRotation(newDirection);
        ////畫一條紅線顯示目前角色朝向
        Debug.DrawRay(transform.position, newDirection, Color.red);
    }    
    void Animation_Walk()
    {
        //isMoveing = h != 0 || v != 0;
        //設置對應動畫狀態機的值
        m_Animator.SetBool("WalkSwitch", isMoveing);
    }
    void Jump(float jumpHeight)
    {
        m_Rigidbody.AddForce(Vector3.up * jumpHeight, ForceMode.Impulse);
    }
}
