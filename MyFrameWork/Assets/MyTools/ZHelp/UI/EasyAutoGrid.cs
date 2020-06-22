/*
 *  项目名字：MyFrameWork
 *  创建时间：2019.1.14
 *  描述信息：自动布局Grid（适用单行）
 *  注意事项：自动布局的时候获取Item的anchoredPosition.x不是最终匹配的位置。如果在Start里做初始化。
 *  能获取到最终匹配的准确位置的时间大概在0.04S以后。
 *  注意：竖方向的布局，设置位置Y是和你传入的数值相反的。 
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using DG.Tweening;
[RequireComponent(typeof(ScrollRect))]

public class EasyAutoGrid : MonoBehaviour {

    public enum Direction
    {
        Horizontal,
        Vertical
    }
    public Direction dir = Direction.Horizontal; 
    List<RectTransform> trans = new List<RectTransform>();
    ScrollRect rect;
    GridLayoutGroup grid;
    Vector2 sizeDelta;
    /// <summary>
    /// 真实宽高
    /// </summary>
    Rect RectSize;
    int curIndex;
    bool m_IsDragging;
    float stepScale = 0.4f;
    public bool OpenScale;
    //单个Item距离中心点X最小
    float SpaceX;
    //单个Item距离中心点Y最小
    float SpaceY;
    System.Action<int> action;

    public void CallBack(int index)
    {
        Debug.LogError("Index:" + index);      
    }

    void Start () {
        Init(CallBack);   
        //Init();
	}
    //初始化调用这个传参数，到达中间的位置执行回调。
    public void Init(System.Action<int> _action = null)
    {
        rect = GetComponent<ScrollRect>();
        //Unrestricted 运动不受限制。内容可以永远移动。
        //Elastic 弹性运动。内容物可以暂时移出容器，但会被弹性拉回。
        //Clamped 夹紧运动。内容不能移出其容器。
        rect.movementType = ScrollRect.MovementType.Unrestricted;
        //获取真实宽高，不受锚点影响
        RectSize = rect.GetComponent<RectTransform>().rect;
        sizeDelta = new Vector2(RectSize.width, RectSize.height);
        if (rect.content.GetComponent<GridLayoutGroup>() == null)
        {
            Debug.LogError("Content缺少GridLayoutGroup组件");
            return;
        }
        else
        {
            grid = rect.content.GetComponent<GridLayoutGroup>();                   
        }
        //sizeDelta可以获取锚点距离左右的准确位置
        grid.GetComponent<RectTransform>().sizeDelta = sizeDelta;
        grid.childAlignment = TextAnchor.MiddleCenter;
        if (dir == Direction.Horizontal)
        {
            grid.startAxis = GridLayoutGroup.Axis.Horizontal;
            grid.constraint = GridLayoutGroup.Constraint.FixedRowCount;
            grid.constraintCount = 1;
            rect.horizontal = true;
            rect.vertical = false;
            //设置对应的格局匹配对应的锚点，通过设置锚点可以快速匹配Unity自带的锚点
            grid.GetComponent<RectTransform>().anchorMax = new Vector2(1,1);
            grid.GetComponent<RectTransform>().anchorMin = new Vector2(0,1);
        }
        else
        {
            grid.startAxis = GridLayoutGroup.Axis.Vertical;
            grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            grid.constraintCount = 1;
            rect.horizontal = false;
            rect.vertical = true;
            grid.GetComponent<RectTransform>().anchorMax = new Vector2(0, 1);
            grid.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0);

        }
        EventTriggerListener.Get(rect.gameObject).onDragStart = DragStart;
        EventTriggerListener.Get(rect.gameObject).onEndDrag = DragEnd;
        trans.Clear();
        for (int i = 0; i < grid.transform.childCount; i++)
        {
            trans.Add(grid.transform.GetChild(i).GetComponent<RectTransform>());
        }     
        SpaceX = grid.cellSize.x / 2;
        SpaceY = grid.cellSize.y / 2;
        action = _action;
    }
    void DragStart(GameObject o)
    {
        m_IsDragging = true;
    }
    void DragEnd(GameObject o)
    {
        m_IsDragging = false;
    }
    float time =0;
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            MoveToIndexHorizontal(0);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            MoveToIndexHorizontal(11);
        }
        time += Time.deltaTime;    
        if (time>0.1f)
        {
            if (dir == Direction.Horizontal)
            {
                UpdateScaleHorizontal();
            }
            else 
            {
                UpdateScaleVertical();
            }
        }      
    }
    //跳到某一个(配合Dotween)
    public void MoveToIndexHorizontal(int index)
    {
   
        float foundation = -trans[index].anchoredPosition.x + sizeDelta.x / 2;
        //grid.transform.DOLocalMoveX(foundation,0.5f);
        grid.transform.localPosition = new Vector3(foundation, grid.transform.localPosition.y);
    }
    public void MoveToIndexVertical(int index)
    {
        float foundation = -trans[index].anchoredPosition.y - sizeDelta.y / 2;
        grid.transform.localPosition = new Vector3(grid.transform.localPosition.x,foundation,0);
    }
    //==================横向的===============
    //边界
    void UpdateLitMaxHorizontal()
    {
        //Content的最大坐标和最小坐标
        float xMax = -trans[0].anchoredPosition.x + sizeDelta.x/2;
        float xMin = -trans[trans.Count - 1].anchoredPosition.x + sizeDelta.x / 2;
        //Content的位置
        float curX = grid.transform.localPosition.x;
        //如果大于最大或者小于最小设置为最大或最小
        if (curX > xMax || curX < xMin)
        {
            //速度
            rect.velocity = Vector2.zero;
            if (curX > xMax)
                grid.transform.localPosition = new Vector3(xMax, grid.transform.localPosition.y, 0);
            else
                grid.transform.localPosition = new Vector3(xMin, grid.transform.localPosition.y, 0);
        }
    }
    //缩放
    void UpdateScaleHorizontal()
    {
        if (trans.Count == 0) return;
        UpdateLitMaxHorizontal();
        //x值在这个点缩放是1中心点位置
        float m_foundation = -grid.transform.localPosition.x + sizeDelta.x/2;

        for (int i = 0; i < trans.Count; i++)
        {
            //锚点相对位置
            Vector3 vPos = trans[i].anchoredPosition;
            //Item距离中心点 距离越近x越小
            float x = Mathf.Abs(vPos.x - m_foundation);
            //Mathf.Pow(a,b) a的b次方小数的话，b>0 B越大越无限接近1
            float scale = Mathf.Pow((1 - stepScale), Mathf.Abs(x / sizeDelta.x));
            if (OpenScale)
                trans[i].transform.localScale = Vector3.one * scale;
            if (x < SpaceX)
            {
                if (curIndex != i)
                {
                    curIndex = i;
                    if (action != null)
                        action(curIndex);
                }
            }
        }
        if (!m_IsDragging)
        {
            if (Mathf.Abs(rect.velocity.x) < 200)
            {
                rect.velocity = Vector2.zero;
                float x = trans[curIndex].anchoredPosition.x;
                float distance = -x + m_foundation;
                if (distance > -5 && distance < 5) return;
                if (distance > 0)
                {
                    grid.transform.localPosition += new Vector3(Time.fixedDeltaTime * 300, 0, 0);
                }
                else
                {
                    grid.transform.localPosition -= new Vector3(Time.fixedDeltaTime * 300, 0, 0);
                }
            }
        }
    }
    //===============竖向的==============
    void UpdateLitMaxVertical()
    {
        float yMax = -trans[trans.Count - 1].anchoredPosition.y - sizeDelta.y / 2;
        float yMin = -trans[0].anchoredPosition.y - sizeDelta.y / 2;  
        float curY = grid.transform.localPosition.y; 
        if (curY > yMax || curY < yMin)
        {
            rect.velocity = Vector2.zero;
            if (curY > yMax)
                grid.transform.localPosition = new Vector3(grid.transform.localPosition.x, yMax, 0);
            else
                grid.transform.localPosition = new Vector3(grid.transform.localPosition.x, yMin, 0);
        }
    }
    void UpdateScaleVertical()
    {
        if (trans.Count == 0) return;
        UpdateLitMaxVertical();
        float m_foundation = -grid.transform.localPosition.y - sizeDelta.y / 2;
        for (int i = 0; i < trans.Count; i++)
        {
            //锚点相对位置
            Vector3 vPos = trans[i].anchoredPosition;
            //Item Y距离中心点
            float y = Mathf.Abs(vPos.y - m_foundation);

            float scale = Mathf.Pow((1 - stepScale), Mathf.Abs(y / sizeDelta.y));
            if(OpenScale)
                trans[i].transform.localScale = Vector3.one * scale;
            //当前那个在中间
            if (y <SpaceY)
            {
                if (curIndex != i)
                {
                    curIndex = i;
                    if (action != null)
                        action(curIndex);
                }
            }

        }
       
        if (!m_IsDragging)
        {
            if (Mathf.Abs(rect.velocity.y) < 200)
            {
                rect.velocity = Vector2.zero;

                float y = trans[curIndex].anchoredPosition.y;

                float distance = -y + m_foundation;

                if (distance > -5 && distance < 5) return;

                if (distance > 0)
                {
                    grid.transform.localPosition += new Vector3(0,Time.fixedDeltaTime * 300, 0);
                }
                else
                {
                    grid.transform.localPosition -= new Vector3(0,Time.fixedDeltaTime * 300, 0);
                }
            }
        }

    }

}
