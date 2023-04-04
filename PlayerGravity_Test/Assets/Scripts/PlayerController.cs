using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // 移動速度
    public float moveSpeed = 15.0f;
    // 回転速度
    public float rotateSpeed = 5.0f;
    // ジャンプ力
    public float jumpPower;
    // 接地判定フラグ
    public bool isGround;
    
    // 回転する向き
    private int rotateDirection = 0;
    // プレイヤーのRigidbody
    [SerializeField]
    private Rigidbody rb = null;

    // Start is called before the first frame update
    void Start()
    {
        // Rigidbodyを設定
        rb = this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // ジャンプ処理
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
    /// ジャンプ処理
    /// </summary>
    public void Jump()
    {
        // 接地しているとき
        if(isGround)
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                isGround = false;
                // 上方向にJumpPower分の力を加える
                rb.AddForce(transform.up * jumpPower * 100);
            }
        }
    }

    /// <summary>
    /// 水平方向の回転処理
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

        // オブジェクトから見て垂直方向を軸とする情報を取得
        Quaternion objRotate = Quaternion.AngleAxis(rotateDirection * rotateSpeed, transform.up);
        // 現在の自分の回転の情報を取得
        Quaternion myRotate = this.transform.rotation;
        // 合成して、自分の回転に設定
        this.transform.rotation = objRotate * myRotate;
    }

    public void OnCollisionEnter(Collision collision)
    {
        // ”Planet”タグのオブジェクトに触れたら
        if(collision.gameObject.tag == "Planet")
        {
            isGround = true;
        }
    }
}
