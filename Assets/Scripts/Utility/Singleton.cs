using UnityEngine;

public class singleton<T> : MonoBehaviour
    where T : MonoBehaviour
{
    private static T _instance = null;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                //インスペクタにあるかチェック、ある場合は取得して終了
                _instance = FindObjectOfType<T>();
                if (_instance == null)
                {
                    //インスペクタが無ければ作成する
                    _instance = new GameObject(typeof(T).ToString()).AddComponent<T>();
                    Debug.LogWarning("指定したシングルトンのオブジェクトが見つからなかったので作成 =" + typeof(T));
                }
            }
            return _instance;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
