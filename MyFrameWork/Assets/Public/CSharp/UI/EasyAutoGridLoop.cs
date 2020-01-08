using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class EasyAutoGridLoop : MonoBehaviour {

    public List<Sprite> itemsLoop;
    List<ItemLoop> loopImages = new List<ItemLoop>();
    Vector2 sizeDelta;
    int cacheCount = 3;
    Vector3 firstInputPos;                     //第一次手指的位置
    Vector3 curInputPos;                       //实时更新的手指的位置
    bool isDragging;                           //正在滑动
    bool isLeft;                               //向左滑动


    int curIndex = 0;          //当前的下标 012


	void Start () {
        Init();
    }
	
	void Update () {
        UpdateLit();
    }

    //初始化
    public void Init()
    {
        if (itemsLoop.Count < 3) return;

        //滑动
        gameObject.GetComponent<Image>().color = new Color(0, 0, 0, 1 / 255f);
        sizeDelta = GetComponent<RectTransform>().sizeDelta;
        GetComponent<RectTransform>().sizeDelta = sizeDelta;
        EventTriggerListener.Get(gameObject).onDragStart = DragStart;
        EventTriggerListener.Get(gameObject).onDrag = Dragging;
        EventTriggerListener.Get(gameObject).onEndDrag = DragEnd;

        //位置
        for (int i = 0; i < cacheCount; i++)
        {
            Image ima = new GameObject("Item").AddComponent<Image>();
            ima.color = Color.white;
            ima.transform.SetParent(transform);
            ima.transform.localScale = Vector3.one;
            ima.GetComponent<RectTransform>().sizeDelta = sizeDelta;
            ima.transform.localPosition = new Vector3(-sizeDelta.x + sizeDelta.x * i, 0, 0);
            ItemLoop item = ima.gameObject.AddComponent<ItemLoop>();
            item.ima = ima;
            item.btn = item.gameObject.AddComponent<Button>();
            item.btn.onClick.AddListener(delegate ()
            {
                ClickImage(item.gameObject);
            });

            loopImages.Add(item);
        }
        loopImages[0].ima.sprite = itemsLoop[itemsLoop.Count - 1];
        loopImages[1].ima.sprite = itemsLoop[0];
        loopImages[2].ima.sprite = itemsLoop[1];
        loopImages[0].imageIndex = itemsLoop.Count - 1;
        loopImages[1].imageIndex = 0;
        loopImages[2].imageIndex = 1;
    }
    //拽完了更新
    void UpdateLit()
    {
        if (!isDragging)
        {
            float maxPosX = sizeDelta.x;

            for (int i = 0; i < cacheCount; i++)
            {

                if (loopImages[2].transform.localPosition.x >= maxPosX)
                {
                    loopImages[i].transform.localPosition -= new Vector3(Time.deltaTime * 200, 0, 0);
                }
                else
                {
                    loopImages[i].transform.localPosition += new Vector3(Time.deltaTime * 200, 0, 0);
                }
            }

            if (Mathf.Abs(Mathf.Abs(loopImages[2].transform.localPosition.x) - sizeDelta.x) <= Time.deltaTime * 200)
            {
                for (int j = 0; j < cacheCount; j++)
                {
                    loopImages[j].transform.localPosition = new Vector3(-sizeDelta.x + sizeDelta.x * j, 0, 0);
                }
                isDragging = true;
            }

        }
    }

    void DragStart(GameObject o)
    {
        isDragging = true;
        firstInputPos = Input.mousePosition;
        curInputPos = Input.mousePosition;
    }
    void Dragging(GameObject o)
    {
        Vector3 addPos = Input.mousePosition - curInputPos;
        curInputPos = Input.mousePosition;

        for(int i=0;i<cacheCount;i++)
        {
            loopImages[i].transform.localPosition += new Vector3(addPos.x, 0, 0);
        }

        //最左边的越界就调到右边去
        float maxPosX = sizeDelta.x * 1.5f;
        bool needChangeLeft = loopImages[0].transform.localPosition.x < -maxPosX;
        if(needChangeLeft)
        {
            //记录
            curIndex++;
            if (curIndex > 2)
                curIndex = 0;
            //调位置
            loopImages[0].transform.localPosition = loopImages[2].transform.localPosition + new Vector3(sizeDelta.x, 0, 0);
            //刷新数据
            int index = loopImages[2].imageIndex + 1;
            if (index > itemsLoop.Count - 1)
                index = 0;
            loopImages[0].imageIndex = index;
            loopImages[0].ima.sprite = itemsLoop[index];
            Debug.Log(index);
            //调整List下标
            ItemLoop item0 = loopImages[0];
            ItemLoop item1 = loopImages[1];
            ItemLoop item2 = loopImages[2];
            loopImages[2] = item0;
            loopImages[1] = item2;
            loopImages[0] = item1;


            Debug.Log("curIndex = " + curIndex);
        }
        //最右边的越界就跳到左边去
        bool needChangeRight = loopImages[2].transform.localPosition.x > maxPosX;
        if(needChangeRight)
        {
            curIndex--;
            if (curIndex < 0)
                curIndex = 2;

            loopImages[2].transform.localPosition = loopImages[0].transform.localPosition - new Vector3(sizeDelta.x, 0, 0);

            int index = loopImages[0].imageIndex - 1;
            if (index < 0)
                index = itemsLoop.Count - 1;
            loopImages[2].imageIndex = index;
            loopImages[2].ima.sprite = itemsLoop[index];

            ItemLoop item0 = loopImages[0];
            ItemLoop item1 = loopImages[1];
            ItemLoop item2 = loopImages[2];
            loopImages[0] = item2;
            loopImages[1] = item0;
            loopImages[2] = item1;

            Debug.Log("curIndex = " + curIndex);
        }
    }
    void DragEnd(GameObject o)
    {
        //确定玩家这次是左滑还是右滑
        isLeft = Input.mousePosition.x - firstInputPos.x < 0 ? true : false;

        isDragging = false;
    }

    //点击图片
    void ClickImage(GameObject o)
    {
        ItemLoop item = o.GetComponent<ItemLoop>();

        Debug.Log(item.imageIndex);
    }
}
