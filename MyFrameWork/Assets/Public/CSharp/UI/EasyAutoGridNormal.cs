using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using DG.Tweening;



[RequireComponent(typeof(ScrollRect))]
public class EasyAutoGridNormal : MonoBehaviour {

    public enum Direction
    {
        Horizontal,
        Vertical
    }
    public Direction dir = Direction.Horizontal;
    public int fixedCount = 1;

    List<RectTransform> trans = new List<RectTransform>();
    ScrollRect rect;
    GridLayoutGroup grid;
    Vector2 sizeDelta;
    int curIndex;


    void Start()
    {
        //Init();//test
    }

    public void Init()
    {
        rect = GetComponent<ScrollRect>();

        rect.movementType = ScrollRect.MovementType.Elastic;
        sizeDelta = rect.GetComponent<RectTransform>().sizeDelta;
        grid = rect.content.GetComponent<GridLayoutGroup>();
        if (dir == Direction.Horizontal)
        {
            grid.childAlignment = TextAnchor.MiddleLeft;
            grid.startAxis = GridLayoutGroup.Axis.Vertical;
            grid.constraint = GridLayoutGroup.Constraint.FixedRowCount;
            grid.constraintCount = fixedCount;
            rect.horizontal = true;
            rect.vertical = false;
        }
        else
        {
            grid.childAlignment = TextAnchor.UpperCenter;
            grid.startAxis = GridLayoutGroup.Axis.Horizontal;
            grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            grid.constraintCount = fixedCount;
            rect.horizontal = false;
            rect.vertical = true;
        }
        trans.Clear();
        for (int i = 0; i < grid.transform.childCount; i++)
        {
            trans.Add(grid.transform.GetChild(i).GetComponent<RectTransform>());
        }

        if(dir == Direction.Horizontal)
        {
            float minX = sizeDelta.x;
            int column = trans.Count / fixedCount;
            if (trans.Count % fixedCount > 0)
            {
                column++;
            }
            float x = (grid.cellSize.x + grid.spacing.x) * column;
            if (minX > x)
                x = minX;
            grid.GetComponent<RectTransform>().sizeDelta = new Vector2(x, sizeDelta.y);
        }
        else
        {
            float minY = sizeDelta.y;
            int column = trans.Count / fixedCount;
            if(trans.Count % fixedCount > 0)
            {
                column++;
            }
            float y = (grid.cellSize.y + grid.spacing.y) * column;
            if (minY > y)
                y = minY;
            grid.GetComponent<RectTransform>().sizeDelta = new Vector2(sizeDelta.x, y);
        }

        //MoveToTop();
    }
    //跳到某一个 
    public void MoveToIndex(int index)
    {
        curIndex = index;

        if (dir == Direction.Horizontal)
        {
            float foundation = -trans[index].anchoredPosition.x + sizeDelta.x / 2;
            //grid.transform.DOLocalMoveX(foundation, 0.5f);
            //grid.transform.localPosition = new Vector3(foundation, grid.transform.localPosition.y);
        }
        else
        {
            float foundation = -trans[index].anchoredPosition.y + sizeDelta.y / 2;
            //grid.transform.DOLocalMoveY(foundation, 0.5f);
            //grid.transform.localPosition = new Vector3(grid.transform.localPosition.x, foundation);
        }

    }
    //跳到顶部
    public void MoveToTop()
    {
        if(dir == Direction.Horizontal)
        {
            grid.transform.localPosition = new Vector3((grid.GetComponent<RectTransform>().sizeDelta.x - sizeDelta.x) / 2.0f,0,0);
        }
        else
        {
            grid.transform.localPosition = new Vector3(0, -(grid.GetComponent<RectTransform>().sizeDelta.y - sizeDelta.y) / 2.0f, 0);
        }
    }
    //跳到底部
    public void MoveToDown()
    {
        float minY = sizeDelta.y / 2.0f;
        float y = (grid.cellSize.y + grid.spacing.y) * trans.Count;
        if (minY > y)
            y = minY;
        else
            y = y - minY;

        //grid.transform.localPosition = new Vector3(grid.transform.localPosition.x,y);
        //grid.transform.DOLocalMoveY(y,0.5f);

    }

}
