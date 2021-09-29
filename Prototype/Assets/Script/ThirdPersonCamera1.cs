using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera1 : MonoBehaviour
{
    public Transform Target;
    public float smoothspeed = 2.0f;
    public Vector3 offset = new Vector3(0, 0, -3);

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Vector.Lerp在兩點之間做插值
        this.transform.position = Vector3.Lerp(this.transform.position, Target.position + offset, smoothspeed * Time.deltaTime);
    }
}
