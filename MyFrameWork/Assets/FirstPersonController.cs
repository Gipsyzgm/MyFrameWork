using UnityEngine;
using System.Collections;
public class FirstPersonController : MonoBehaviour {
    public float speed = 5f;
    public float height = 5f;
    public float XSensitivity = 2f;
    public float smoothTime = 5f;

    private Rigidbody myRigidBody;
    private Quaternion m_CharacterTargetRot;
    void Awake() {
        myRigidBody = this.GetComponent<Rigidbody>();
        m_CharacterTargetRot = transform.localRotation;
    }
    void Update()
    {

        if (Input.GetKey(KeyCode.Space))
        {



        }
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        float s = 0f;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            s = height;
    }
    myRigidBody.MovePosition(transform.position + new Vector3(h, s, v) * speed * Time.deltaTime);
        float yRot = Input.GetAxis("Mouse X") * XSensitivity;
        m_CharacterTargetRot *= Quaternion.Euler(0f, yRot, 0f);
        transform.localRotation = Quaternion.Slerp(transform.localRotation, m_CharacterTargetRot,
                                                   smoothTime * Time.deltaTime);
    }
}