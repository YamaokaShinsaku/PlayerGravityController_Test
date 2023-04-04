using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityController : MonoBehaviour
{
    // �v���C���[��Transform
    [SerializeField]
    private Transform pTransform;
    // �v���C���[��Rigidbody
    private Rigidbody pRb = null;
    // "Planet"�^�O�����Ă���I�u�W�F�N�g���i�[����z��
    private GameObject[] planets;
    // �d�͂��y���Ȃ�f��
    private GameObject planet;
    // �d�͂̋���
    public float gravity;
    // �f���ɑ΂���v���C���[�̌���
    private Vector3 direction;
    // Ray���ڐG�����f���̃|���S���̖@��
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
    /// �d�͂Ɖ�]�̐��䏈��
    /// </summary>
    public void Attract()
    {
        // �d�͂̋t�x�N�g��
        Vector3 gravityUp = normalVec;
        // �v���C���[���猩��������̃x�N�g��
        Vector3 bodyUp = pTransform.up;
        // gravityUp�����ɗ͂�������
        pTransform.GetComponent<Rigidbody>().AddForce(gravityUp * gravity);
        // ���݂̎p������ǂꂾ����]�����邩���v�Z���A�i�[����
        Quaternion targetRotation = Quaternion.FromToRotation(bodyUp, gravityUp) * pTransform.rotation;

        // ���`�⊮���Ȃ����]������
        pTransform.rotation = Quaternion.Lerp(pTransform.rotation, targetRotation, 120 * Time.deltaTime);
    }

    /// <summary>
    /// "Planet"�^�O�����I�u�W�F�N�g��Ԃ�
    /// </summary>
    /// <returns>�v���C���[�Ɉ�ԋ߂��I�u�W�F�N�g��Ԃ�</returns>
    public GameObject ChoosePlanet()
    {
        // "Planet"�^�O�����I�u�W�F�N�g���擾
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
    /// Ray���I�u�W�F�N�g�i�f���j�ɐڐG�����ۂ̏���
    /// </summary>
    public void RayTest()
    {
        // �I�u�W�F�N�g�i�f���j���擾
        planet = ChoosePlanet();
        // �v���C���[���猩���I�u�W�F�N�g�i�f���j�̒��S�x�N�g��
        direction = planet.transform.position - this.transform.position;
        // Ray��ݒ�
        Ray ray = new Ray(this.transform.position, direction);

        // Ray�����������I�u�W�F�N�g�̏����擾
        RaycastHit hit;
        // Ray�ɃI�u�W�F�N�g���Փ˂�����
        if(Physics.Raycast(ray,out hit,Mathf.Infinity))
        {
            // Ray�����������I�u�W�F�N�g�̃^�O��"Planet"�̎�
            if(hit.collider.tag == "Planet")
            {
                normalVec = hit.normal;
            }
        }
    }
}
