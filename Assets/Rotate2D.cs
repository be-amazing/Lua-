using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum LayoutType
{
    Horizontal = 0,
    Vertical = 1
}

public class Rotate2D : MonoBehaviour
{
    public LayoutType Layout = LayoutType.Horizontal;
    public ScrollRect rotate;

    public int VisibleCount = 5;
    public int Spacing = 100;
    public float Offset = 0f;
    public Vector2 Pivot = Vector2.zero;
    
    public List<float> CellScale;
    public GameObject[] Cells;
    public Vector3[] CellPosition;

    private void Start()
    {
        Cells = new GameObject[VisibleCount];
        CellPosition = new Vector3[VisibleCount];
        InitializeScrollRect();
    }
    
    private void InitializeScrollRect()
    {
        rotate.horizontal = Layout == LayoutType.Vertical;
        rotate.vertical = Layout == LayoutType.Vertical;
        
        GenerateScrollContentCell();

        // rotate.onValueChanged.AddListener(Rotate2dValueChanged);
    }
    
    private void Rotate2dValueChanged(Vector2 value)
    {
        print(value);
    }
    
    private void RotateAction(float dir)
    {
        for (int i = 0; i < VisibleCount; i++)
        {
            if (i == 0)
            {
                Cells[i].transform.DOLocalMove(Cells[dir > 0 ? i + 1 : VisibleCount-1].transform.localPosition, 2f);
                Cells[i].transform.DOScale(Cells[dir > 0 ? i + 1 : VisibleCount-1].transform.localScale, 2f);
            }
            else if (i == VisibleCount-1)
            {
                Cells[i].transform.DOLocalMove(Cells[dir > 0 ? 0 : i - 1].transform.localPosition, 2f);
                Cells[i].transform.DOScale(Cells[dir > 0 ? 0 : i - 1].transform.localScale, 2f);
            }
            else
            {
                Cells[i].transform.DOLocalMove(Cells[dir > 0 ? i + 1 : i - 1].transform.localPosition, 2f);
                Cells[i].transform.DOScale(Cells[dir > 0 ? i + 1 : i - 1].transform.localScale, 2f);
            }
        }
    }
    
    private void GenerateScrollContentCell()
    {
        int length = Mathf.Min(VisibleCount, CellScale.Count);
        for (int i = 0; i < length; i++)
        {
            GameObject slide = CreateSlideCell(GetSlideCellPrefab(),rotate.content.transform,GetCellLocalPosition(i),GetConstrainedScale(i),Pivot);
            Rotate2DCell rotate2DCell = slide.GetComponent<Rotate2DCell>();
            rotate2DCell.Number.text = i.ToString();
            rotate2DCell.MoveAction = RotateAction;
            Cells[i] = slide;
            CellPosition[i] = slide.transform.localPosition;
        }
    }

    /// <summary>
    /// 创建可滑动Cell
    /// </summary>
    private GameObject CreateSlideCell(GameObject original,Transform parent,Vector3 localPos,Vector3 localScale,Vector2 pivot)
    {
        GameObject cell = Instantiate(original,parent);
        cell.transform.localPosition = localPos;
        cell.transform.localScale = localScale;
        cell.GetComponent<RectTransform>().pivot = pivot;
        return cell;
    }

    private Vector3 GetCellLocalPosition(int i)
    {
        if (Layout == LayoutType.Horizontal)
        {
            return new Vector3(GetPositionByIndex(i), Offset, 0);
        }
        
        if (Layout == LayoutType.Vertical)
        {
            return new Vector3(Offset, GetPositionByIndex(i), 0);
        }
        
        return Vector3.one;
    }
    
    private float GetPositionByIndex(int index)
    {
        int length = Mathf.Min(VisibleCount, CellScale.Count);
        float offset = 0f;
        if (index == length / 2)
        {
            return offset;
        }

        int start = index > length / 2 ? length / 2 : index;
        int end = index > length / 2 ? index : length / 2;

        if (length % 2 == 0)
        {
            end--;
            offset = GetPositionOffset(index) / 2 + Spacing / 2;
        }
        else
        {
            start++;
            offset = GetPositionOffset(index) / 2 + GetPositionOffset(length / 2) / 2 + Spacing;
        }
        
        for (int i = start; i < end; i++)
        {
            offset = offset + Spacing + GetPositionOffset(i);
        }

        return index > length / 2 ? offset : -offset;
    }
    
    private float GetPositionOffset(int i)
    {
        float scale = CellScale[i];
        Rect rect = GetSlideCellPrefab().GetComponent<RectTransform>().rect;
        if (Layout == LayoutType.Horizontal)
        {
            return rect.width * scale;
        }
        
        if (Layout == LayoutType.Vertical)
        {
            return rect.height * scale;
        }

        return 0;
    }
    
    /// <summary>
    /// 获取x,y,z相同的Scale Value
    /// </summary>
    /// <param name="i"></param>
    /// <returns></returns>
    private Vector3 GetConstrainedScale(int i)
    {
        return new Vector3(CellScale[i], CellScale[i], CellScale[i]);
    }
    
    private GameObject GetSlideCellPrefab()
    {
        return Resources.Load<GameObject>("Prefabs/Cell");
    }
}
