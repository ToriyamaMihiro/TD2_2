using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TestTools;

public class BossCounterParticle : MonoBehaviour
{
    //transform�Ŗ��t���[���擾����ƕ��ׂ��|����ׁA�ʂɎQ�Ƃ�ێ��B
    Transform tf;

    //-1.0f�Ŏ��v���A1.0f�Ŕ����v���B
    float direction = -1.0f;

    //�ړ����x�Ƃ������ړ��p�x�B
    float moveSpeed;

    //�v���C���[��ǔ����鑬�x�̃��[�g(�傫��������)�B
    float followRate = 0.1f;

    //�ǔ�����|�C���g�̃v���C���[����̋���(�܂菬�������A�ߕt��)�B
    float followTargetDistance;

    //�J�E���^�[�ɂȂ����Ƃ��ǂꂾ�����̈ʒu���痣����
    float distancePlus;

    float distance;

    int destroyTime;

    // Start is called before the first frame update
    void Start()
    {
        tf = transform;

        followTargetDistance = Random.Range(0.1f, 0.8f);
        moveSpeed = Random.Range(2f, 4.5f);
        distancePlus = Random.Range(1f, 3f);
        distance = followTargetDistance;
    }

    // Update is called once per frame
    void Update()
    {

        BossAttackAction boss;
        GameObject obj = GameObject.Find("Boss");
        boss = obj.GetComponent<BossAttackAction>();

        //�J�E���^�[�ɂȂ����炩���̋����Ƒ��������ɂȂ�܂ł��񂾂񋗗�����������
        if (boss.isCounter && followTargetDistance <= distance + distancePlus)
        {
            followTargetDistance = followTargetDistance + 0.1f;
        }

        //�v���C���[�����̋����Œǔ��B
        tf.position = Vector3.Lerp(tf.position, boss.transform.position + (tf.position - boss.transform.position).normalized * followTargetDistance, followRate);
        //�v���C���[�𒆐S�ɉ~�^���B
        tf.RotateAround(boss.transform.position, Vector3.forward, direction * moveSpeed);

        destroyTime += 1;
        if (destroyTime >= boss.counterMaxTime + 40)
        {
            boss.isCounter = false;
            Destroy(this.gameObject);
        }

    }
}
