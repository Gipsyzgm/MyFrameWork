using UnityEngine;

using System.Collections;

public class FPS : MonoBehaviour
{
    public static FPS Instance;


    private float time;

    private float updateTime = 0.5f;

    private float count = 0;

    private int frame = 0;

    public float fps = 0;

    public bool openDebug;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        Application.targetFrameRate = 60;

        time = updateTime;

        if(openDebug)
            Debug.unityLogger.logEnabled = openDebug;
    }

    // Update is called once per frame

    void Update()
    {

        Controller();

    }

    private void Controller()

    {

        time -= Time.deltaTime;

        if (time <= 0)

        {

            time = updateTime;

            // 每次加上 时间比例除以每帧的事件，得到每秒的频率

            count += Time.timeScale / Time.deltaTime;

            frame++;

            // 总帧数除以相加次数，得到当天帧率的平均值

            fps = (float)(count / frame);

        }

        if (frame > 60)
        {

            count = 0;

            frame = 0;

        }

    }

    void OnGUI()

    {
        GUIStyle text = new GUIStyle();
        text.fontSize = 30;
        text.normal.textColor = Color.white;
        GUI.Label(new Rect(300, 50, 300, 200), "FPS     " + fps.ToString(), text);

    }

}