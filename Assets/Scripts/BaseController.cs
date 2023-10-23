using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PositionReset))]

abstract public class BaseController : MonoBehaviour
{
    protected PositionReset positionReset = null;
    // Start is called before the first frame update
    void Start()
    {
        if (TryGetComponent(out positionReset) == false)
        {
            Debug.Log("PositionResetが無かったので追加します");
            positionReset = GetComponent<PositionReset>();
        }
        //派生先の初期化
        Init();
        //実行時に自分自身をGameManagerに追加
        GameManager.Instance.Controllers.Add(this);
    }

    //abstract(継承先で定義しなくてはならない関数)
    public abstract void Init();

    public abstract void Restart();

    //virtual(継承先で定義しなくてもいい関数)
    public virtual void GameOver()
    {
        gameObject.SetActive(false);
    }
}
