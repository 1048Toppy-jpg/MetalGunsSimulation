using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    //hit時のパーティクル
    [SerializeField] private GameObject fireworksParticle = null;


    [SerializeField] private GameObject hitParticle = null;

    //hit時のSE
    [SerializeField] private AudioClip hitSE = null;
    private AudioSource audioSource = null;

    //hit時のフラグ
    private bool hitFlg = false;

    // Start is called before the first frame update
    void Start()
    {
        //SE
        if (gameObject.TryGetComponent(out audioSource) == false)
        {
            Debug.LogError("AudioManagerが見つかりませんでした。");
            audioSource = null;
        }else
        {
            audioSource.clip = hitSE;
        }
        hitFlg = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Hit(Vector3 hitposition)
    {
        if (!hitFlg)//ヒットしていない場合
        {
            hitFlg = true;//フラグをtrueにする
                          //クローンを生成
            var fireworksCloneParticle = Instantiate(fireworksParticle);
                fireworksCloneParticle.transform.position = transform.position;//座標を合わせる

            
        }

        var hitCloneParticle = Instantiate(hitParticle);
            hitCloneParticle.transform.position = hitposition;//座標を合わせる

        if (hitSE &&
                audioSource != null &&
                audioSource.clip != null)
            audioSource.Play();
    }

}
