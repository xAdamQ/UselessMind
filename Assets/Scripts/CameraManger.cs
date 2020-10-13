using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManger : MonoBehaviour
{
    const float MinOrthoSize = 5;
    const float VerticalPadding = .35f, HorizontalPadding = .1f;
    public const float ShakeTime = 1f;
    Hashtable ZoomHT, ShakeHT;
    Camera Cam;

    private void Awake()
    {
        ZoomHT = new Hashtable();
        ZoomHT.Add("from", 00);
        ZoomHT.Add("to", 00);
        ZoomHT.Add("time", .3f);
        ZoomHT.Add("onupdate", "SetOrthoSize");
        ZoomHT.Add("onupdatetarget", gameObject);

        ShakeHT = new Hashtable();
        ShakeHT.Add("amount", Vector3.one * .3f);
        ShakeHT.Add("time", ShakeTime);
        ShakeHT.Add("delay", BaseCard.SeeTime + BaseCard.FlipTime);

        Cam = GetComponent<Camera>();
        Game.I.LevelLoading += OnEveryLev;
    }

    /// <summary>
    /// calcuate reqiured ortographic size for x and y and set cam with the bigger
    /// </summary>
    void OnEveryLev()
    {
        AdaptToGrid();
        ZoomAnim();
    }

    public void SetOrthoSize(float val)
    {
        Cam.orthographicSize = val;
    }

    public void ShakeAnim()
    {
        iTween.ShakePosition(gameObject, ShakeHT);
    }

    void AdaptToGrid()
    {
        var vertical = new Period(-BaseCard.TotalSize.y / 2, (Level.Current.GridSize.y * BaseCard.TotalSize.y) - (BaseCard.TotalSize.y / 2));
        var horizontal = new Period(-BaseCard.TotalSize.x / 2, (Level.Current.GridSize.x * BaseCard.TotalSize.x) - (BaseCard.TotalSize.x / 2));
        var poz = new Vector3(horizontal.Start + (horizontal.Delta / 2f), vertical.Start + (vertical.Delta / 2f), -10);
        transform.position = poz;
    }

    void ZoomAnim()
    {
        var xSize = Level.Current.GridSize.x * BaseCard.TotalSize.x;
        xSize += xSize * HorizontalPadding;

        var ySize = Level.Current.GridSize.y * BaseCard.TotalSize.y;
        ySize += ySize * VerticalPadding;

        var ySizeToX = ySize * Cam.aspect;

        var size = xSize > ySizeToX ? xSize : ySizeToX;

        ZoomHT["from"] = Cam.orthographicSize;
        ZoomHT["to"] = size > MinOrthoSize ? size : MinOrthoSize;

        iTween.ValueTo(gameObject, ZoomHT);
    }
}
