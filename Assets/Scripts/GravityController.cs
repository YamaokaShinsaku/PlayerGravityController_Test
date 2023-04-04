using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityController : MonoBehaviour
{
    // プレイヤーのTransform
    [SerializeField]
    private Transform pTransform;
    // プレイヤーのRigidbody
    private Rigidbody pRb = null;
    // "Planet"タグがついているオブジェクトを格納する配列
    private GameObject[] planets;
    // 重力が軽くなる惑星
    private GameObject planet;
    // 重力の強さ
    public float gravity;
    // 惑星に対するプレイヤーの向き
    private Vector3 direction;
    // Rayが接触した惑星のポリゴンの法線
    private Vector3 normalVec = new Vector3(0, 0, 0);


    // Start is called before the first frame update
    void Start()
    {
        pRb = this.GetComponent<Rigidbody>();
        pRb.constraints = RigidbodyConstraints.FreezeRotation;
        pRb.useGravity = false;
        pTransform = this.transform;
    }

    // Update is called once per frame
    void Update()
    {
        Attract();
        RayTest();
    }

    /// <summary>
    /// 重力と回転の制御処理
    /// </summary>
    public void Attract()
    {
        // 重力の逆ベクトル
        Vector3 gravityUp = normalVec;
        // プレイヤーから見た上方向のベクトル
        Vector3 bodyUp = pTransform.up;
        // gravityUp方向に力を加える
        pTransform.GetComponent<Rigidbody>().AddForce(gravityUp * gravity);
        // 現在の姿勢からどれだけ回転させるかを計算し、格納する
        Quaternion targetRotation = Quaternion.FromToRotation(bodyUp, gravityUp) * pTransform.rotation;

        // 線形補完しながら回転させる
        pTransform.rotation = Quaternion.Lerp(pTransform.rotation, targetRotation, 120 * Time.deltaTime);
    }

    /// <summary>
    /// "Planet"タグを持つオブジェクトを返す
    /// </summary>
    /// <returns>プレイヤーに一番近いオブジェクトを返す</returns>
    public GameObject ChoosePlanet()
    {
        // "Planet"タグをもつオブジェクトを取得
        planets = GameObject.FindGameObjectsWithTag("Planet");

        double[] planetDistance = new double[planets.Length];

        for (int i = 0; i < planets.Length; i++)
        {
            planetDistance[i] = Vector3.Distance(this.transform.position, planets[i].transform.position);
        }

        int minIndex = 0;
        double minDistance = Mathf.Infinity;

        for (int j = 0; j < planets.Length; j++)
        {
            if (planetDistance[j] < minDistance)
            {
                minIndex = j;
            }
        }

        return planets[minIndex];
    }

    /// <summary>
    /// Rayがオブジェクト（惑星）に接触した際の処理
    /// </summary>
    public void RayTest()
    {
        // オブジェクト（惑星）を取得
        planet = ChoosePlanet();
        // プレイヤーから見たオブジェクト（惑星）の中心ベクトル
        direction = planet.transform.position - this.transform.position;
        // Rayを設定
        Ray ray = new Ray(this.transform.position, direction);

        // Rayが当たったオブジェクトの情報を取得
        RaycastHit hit;
        // Rayにオブジェクトが衝突したら
        if(Physics.Raycast(ray,out hit,Mathf.Infinity))
        {
            // Rayが当たったオブジェクトのタグが"Planet"の時
            if(hit.collider.tag == "Planet")
            {
                normalVec = hit.normal;
            }
        }
    }
}
