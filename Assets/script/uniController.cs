using UnityEngine;
using System.Collections;

public class uniController : MonoBehaviour {

    float speed = 5.0f;
    float jumpSpeed = 8.0f;
    float gravity = 20.0f;
    float rotateSpeed = 10f;

    private Vector3 moveDirection = Vector3.zero;
    private CharacterController controller;

    // Use this for initialization
    void Start () {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        //CameraAxisControl();
        //jumpControl();
        //attachMove();
        //attachRotation();

        if (Input.GetKey("up"))
        {
            transform.position += transform.forward * 0.5f;
        }

        if (Input.GetKey("right"))
        {
            transform.Rotate(0, 10, 0);
        }
        if (Input.GetKey("left"))
        {
            transform.Rotate(0, -10, 0);
        }

    }

    //標準的なコントロール
    void NormalControl()
    {
        if (controller.isGrounded)
        {
            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            moveDirection = transform.TransformDirection(moveDirection);    //?必要ない気もする
            moveDirection *= speed;
        }
    }


    //カメラ軸に沿った移動コントロール
    void CameraAxisControl()
    {
        if (controller.isGrounded)
        {
            Vector3 forward = Camera.main.transform.TransformDirection(Vector3.forward);
            Vector3 right = Camera.main.transform.TransformDirection(Vector3.right);

            moveDirection = Input.GetAxis("Horizontal") * right + Input.GetAxis("Vertical") * forward;
            moveDirection *= speed;
        }
    }

    //標準的なジャンプコントロール
    void jumpControl()
    {
        if (Input.GetButton("Jump"))
            moveDirection.y = jumpSpeed;
    }


    //移動処理 
    void attachMove()
    {
        moveDirection.y -= gravity * Time.deltaTime;
        controller.Move(moveDirection * Time.deltaTime);
    }

    //キャラクターを進行方向へ向ける処理 
    void attachRotation()
    {
        var moveDirectionYzero = -moveDirection;
        moveDirectionYzero.y = 0;

        //ベクトルの２乗の長さを返しそれが0.001以上なら方向を変える（０に近い数字なら方向を変えない） 
        if (moveDirectionYzero.sqrMagnitude > 0.001)
        {

            //２点の角度をなだらかに繋げながら回転していく処理（stepがその変化するスピード） 
            float step = rotateSpeed * Time.deltaTime;
            Vector3 newDir = Vector3.RotateTowards(transform.forward, moveDirectionYzero, step, 0f);

            transform.rotation = Quaternion.LookRotation(newDir);
        }
    }
}
