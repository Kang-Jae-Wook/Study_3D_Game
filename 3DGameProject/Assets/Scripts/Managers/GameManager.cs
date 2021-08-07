using UnityEngine;

//  Singleton
//  이 객체는 반드시 단 하나인 디자인 패턴
class GameManager : MonoBehaviour
{
    private static GameManager m_Instance = null;
    public static GameManager Get
    {
        get
        {
            if (m_Instance == null) m_Instance = FindObjectOfType<GameManager>();
            return m_Instance;
        }
    }

    public static int FloorLayerMask { get; private set; } = 0;

    private void Start()
    {
        //  LayerMast ?
        //  Bitmask ??
        //  1byte = 8 bit
        //  bit ? 0,1
        //  int -> 4byte -> 32bit
        //  시프트 연산자 (비트 연산자)
        //  1 << 0 = 0;
        //  1 << 1 = 2;
        //  1 << 2 = 4;
        //  1 << 3 = 8;
        //  LayerMask ?
        //  유니티에서 tag 와 layer 가 있는데, layer 의 이름을 찾아서 값을 가지고 온다
        FloorLayerMask = 1 << LayerMask.NameToLayer("Floor");

        DontDestroyOnLoad(this);
    }

}
