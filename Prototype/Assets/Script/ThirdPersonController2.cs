using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonController2 : MonoBehaviour
{
    public float movespeed = 3.0f;
    public Transform cam;

    public float turnSmoothTime = 0.1f;
    public float jumpHeight = 1f;


    float turnSmoothVelocity;
    //角色移動向量暫時變數
    Vector3 direction;
    Animator m_Animator;
    Rigidbody m_Rigidbody;
    float h;
    float v;
    bool isGrounded;
    bool isMoveing;

    // Start is called before the first frame update
    void Start()
    {
        m_Animator = GetComponent<Animator>();
        m_Rigidbody = GetComponent<Rigidbody>();
        //Rigidbody插值 = 移動更平滑？
        m_Rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
        cam = GameObject.Find("Main Camera").transform;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");
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
    }    
    private void OnCollisionEnter(Collision collision)
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
        float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
        //float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.transform.eulerAngles.y;

        Rotation(targetAngle);
        Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
        moveDir *= movespeed * Time.deltaTime;
        m_Rigidbody.MovePosition(m_Rigidbody.position + moveDir);
    }
    void Rotation(float targetAngle)
    {
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
        transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);
    }
    void Animation_Walk()
    {        
        //設置對應動畫狀態機的值
        m_Animator.SetBool("WalkSwitch", isMoveing);
    }
    void Jump(float jumpHeight)
    {
        m_Rigidbody.AddForce(Vector3.up * jumpHeight, ForceMode.Impulse);
    }
}
