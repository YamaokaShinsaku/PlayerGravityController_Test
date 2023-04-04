using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // �ړ����x
    public float moveSpeed = 15.0f;
    // ��]���x
    public float rotateSpeed = 5.0f;
    // �W�����v��
    public float jumpPower;
    // �ڒn����t���O
    public bool isGround;
    
    // ��]�������
    private int rotateDirection = 0;
    // �v���C���[��Rigidbody
    [SerializeField]
    private Rigidbody rb = null;

    // Start is called before the first frame update
    void Start()
    {
        // Rigidbody��ݒ�
        rb = this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // �W�����v����
        Jump();
    }

    private void FixedUpdate()
    {
        HorizontalRotate();

        Vector3 moveDirection
            = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;

        rb.MovePosition(rb.position + transform.TransformDirection(moveDirection) * moveSpeed * Time.deltaTime);
    }

    /// <summary>
    /// �W�����v����
    /// </summary>
    public void Jump()
    {
        // �ڒn���Ă���Ƃ�
        if(isGround)
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                isGround = false;
                // �������JumpPower���̗͂�������
                rb.AddForce(transform.up * jumpPower * 100);
            }
        }
    }

    /// <summary>
    /// ���������̉�]����
    /// </summary>
    public void HorizontalRotate()
    {
        if(Input.GetKey(KeyCode.Q))
        {
            rotateDirection = -1;
        }
        else if (Input.GetKey(KeyCode.E))
        {
            rotateDirection = 1;
        }
        else
        {
            rotateDirection = 0;
        }

        // �I�u�W�F�N�g���猩�Đ������������Ƃ�������擾
        Quaternion objRotate = Quaternion.AngleAxis(rotateDirection * rotateSpeed, transform.up);
        // ���݂̎����̉�]�̏����擾
        Quaternion myRotate = this.transform.rotation;
        // �������āA�����̉�]�ɐݒ�
        this.transform.rotation = objRotate * myRotate;
    }

    public void OnCollisionEnter(Collision collision)
    {
        // �hPlanet�h�^�O�̃I�u�W�F�N�g�ɐG�ꂽ��
        if(collision.gameObject.tag == "Planet")
        {
            isGround = true;
        }
    }
}
