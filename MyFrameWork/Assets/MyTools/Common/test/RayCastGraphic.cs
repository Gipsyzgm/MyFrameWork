//======================================
//	Author: Matrix
//  Create Time：2018/5/10 20:50:48 
//  Function:
//======================================

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class RayCastGraphic : Singleton<RayCastGraphic>
{
    //protected const int kNoEventMaskSet = 1 << 10;
    //protected LayerMask m_BlockingMask = kNoEventMaskSet;

    private List<Graphic> m_RaycastResults = new List<Graphic>();

    public void Raycast(PointerEventData eventData, List<RaycastResult> resultAppendList, Canvas canvas,
        UnityEngine.Camera eventCamera, GraphicRaycaster raycaster, LayerMask m_BlockingMask)
    {
        if (canvas == null)
            return;

        // Convert to view space
        Vector2 pos;
        if (eventCamera == null)
            pos = new Vector2(eventData.position.x / Screen.width, eventData.position.y / Screen.height);
        else
            pos = eventCamera.ScreenToViewportPoint(eventData.position);

        // If it's outside the camera's viewport, do nothing
        if (pos.x < 0f || pos.x > 1f || pos.y < 0f || pos.y > 1f)
            return;

        float hitDistance = float.MaxValue;

        Ray ray = new Ray();

        if (eventCamera != null)
            ray = eventCamera.ScreenPointToRay(eventData.position);

        if (canvas.renderMode != RenderMode.ScreenSpaceOverlay &&
            raycaster.blockingObjects != GraphicRaycaster.BlockingObjects.None)
        {
            float dist = 100.0f;

            if (eventCamera != null)
                dist = eventCamera.farClipPlane - eventCamera.nearClipPlane;

            if (raycaster.blockingObjects == GraphicRaycaster.BlockingObjects.ThreeD ||
                raycaster.blockingObjects == GraphicRaycaster.BlockingObjects.All)
            {
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, dist, m_BlockingMask))
                {
                    hitDistance = hit.distance;
                }
            }

            if (raycaster.blockingObjects == GraphicRaycaster.BlockingObjects.TwoD ||
                raycaster.blockingObjects == GraphicRaycaster.BlockingObjects.All)
            {
                RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, dist, m_BlockingMask);

                if (hit.collider != null)
                {
                    hitDistance = hit.fraction * dist;
                }
            }
        }

        m_RaycastResults.Clear();
        Raycast(canvas, eventCamera, eventData.position, m_RaycastResults);

        for (var index = 0; index < m_RaycastResults.Count; index++)
        {
            var go = m_RaycastResults[index].gameObject;
            bool appendGraphic = true;

            if (raycaster.ignoreReversedGraphics)
            {
                if (eventCamera == null)
                {
                    // If we dont have a camera we know that we should always be facing forward
                    var dir = go.transform.rotation * Vector3.forward;
                    appendGraphic = Vector3.Dot(Vector3.forward, dir) > 0;
                }
                else
                {
                    // If we have a camera compare the direction against the cameras forward.
                    var cameraFoward = eventCamera.transform.rotation * Vector3.forward;
                    var dir = go.transform.rotation * Vector3.forward;
                    appendGraphic = Vector3.Dot(cameraFoward, dir) > 0;
                }
            }

            if (appendGraphic)
            {
                float distance = 0;

                if (eventCamera == null || canvas.renderMode == RenderMode.ScreenSpaceOverlay)
                    distance = 0;
                else
                {
                    Transform trans = go.transform;
                    Vector3 transForward = trans.forward;
                    // http://geomalgorithms.com/a06-_intersect-2.html
                    distance = (Vector3.Dot(transForward, trans.position - ray.origin) /
                                Vector3.Dot(transForward, ray.direction));

                    // Check to see if the go is behind the camera.
                    if (distance < 0)
                        continue;
                }

                if (distance >= hitDistance)
                    continue;

                var castResult = new RaycastResult
                {
                    gameObject = go,
                    distance = distance,
                    screenPosition = eventData.position,
                    index = resultAppendList.Count,
                    depth = m_RaycastResults[index].depth,
                    sortingLayer = canvas.sortingLayerID,
                    sortingOrder = canvas.sortingOrder
                };
                resultAppendList.Add(castResult);
            }
        }
    }

    [NonSerialized] static readonly List<Graphic> s_SortedGraphics = new List<Graphic>();

    private static void Raycast(Canvas canvas, UnityEngine.Camera eventCamera, Vector2 pointerPosition,
        List<Graphic> results)
    {
        // Debug.Log("ttt" + pointerPoision + ":::" + camera);
        // Necessary for the event system
        var foundGraphics = GraphicRegistry.GetGraphicsForCanvas(canvas);
        for (int i = 0; i < foundGraphics.Count; ++i)
        {
            Graphic graphic = foundGraphics[i];

            // -1 means it hasn't been processed by the canvas, which means it isn't actually drawn
            if (graphic.depth == -1 || !graphic.raycastTarget)
                continue;

            if (!RectTransformUtility.RectangleContainsScreenPoint(graphic.rectTransform, pointerPosition, eventCamera))
                continue;

            if (graphic.Raycast(pointerPosition, eventCamera))
            {
                s_SortedGraphics.Add(graphic);
            }
        }

        s_SortedGraphics.Sort((g1, g2) => g2.depth.CompareTo(g1.depth));
        //		StringBuilder cast = new StringBuilder();
        for (int i = 0; i < s_SortedGraphics.Count; ++i)
            results.Add(s_SortedGraphics[i]);
        //		Debug.Log (cast.ToString());

        s_SortedGraphics.Clear();
    }
}