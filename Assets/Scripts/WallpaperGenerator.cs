using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class WallpaperGenerator : MonoBehaviour
{
    public static WallpaperGenerator I;

    public Sprite TransetionSprite, ShiftSprite;

    public WallpaperProfile CurrentProfile;

    private void Awake()
    {
        I = this;
    }

    private void Start()
    {
        Game.I.LevelLoaded += OnNewLevel;
        GO();
    }

    float MixTime;
    public void GO()
    {
        MixTime = CurrentProfile.MixTime;

        StopAllCoroutines();

        switch (CurrentProfile.WallpaperType)
        {

            case WallpaperType.Transetion:
                StartCoroutine(ColorTransetion());
                break;

            case WallpaperType.Shift:
                StartCoroutine(ShiftColors());
                break;

        }
    }

    public IEnumerator ColorTransetion()
    {
        GetComponent<Image>().sprite = TransetionSprite;
        var texture2D = GetComponent<Image>().sprite.texture;
        var resolution = TransetionSprite.texture.height;

        //var frames = MixTime / Time.fixedDeltaTime;
        var frames = MixTime / CurrentProfile.UpdateRate;

        var rate = 1f / frames;
        var palette = CurrentProfile.Palette;

        for (int h = 0; h < resolution; h++)
        {
            texture2D.SetPixel(0, h, palette[0]);
        }
        texture2D.Apply();


        // start with first color
        var paletteCounter = 0;
        while (true)
        {
            var upColorInd = paletteCounter % palette.Length;
            var downColorInd = (paletteCounter + 1) % palette.Length;
            paletteCounter++;

            var upColor = palette[upColorInd];
            var downColor = palette[downColorInd];

            for (int i = 0; i < frames; i++)
            {
                for (int e = resolution - 1; e > 0; e--)
                {
                    texture2D.SetPixel(0, e, texture2D.GetPixel(0, e - 1));
                }// shift colors up

                var current = Color.Lerp(upColor, downColor, i * rate);

                texture2D.SetPixel(0, 0, current);

                texture2D.Apply();

                yield return new WaitForSeconds(CurrentProfile.UpdateRate);
            }

        }
    }

    public IEnumerator ShiftColors()
    {
        var AnimeHT = new Hashtable();
        AnimeHT.Add("from", Color.white);
        AnimeHT.Add("to", Color.black);
        AnimeHT.Add("time", MixTime);
        AnimeHT.Add("onupdatetarget", gameObject);
        AnimeHT.Add("onupdate", "ChangeColor");
        AnimeHT.Add("easetype", iTween.EaseType.linear);

        GetComponent<Image>().sprite = ShiftSprite; //deafelt on image is transetion

        var palette = CurrentProfile.Palette;
        var paletteCounter = 0;

        while (true)
        {
            var UpColorInd = paletteCounter % (palette.Length);
            var DownColorInd = (paletteCounter + 1) % (palette.Length);
            paletteCounter++;

            var up = palette[UpColorInd];
            var down = palette[DownColorInd];

            AnimeHT["from"] = up;
            AnimeHT["to"] = down;

            iTween.ValueTo(gameObject, AnimeHT);

            yield return new WaitForSeconds(MixTime);
        }
    }

    public void ChangeColor(Color value)
    {
        GetComponent<Image>().color = value;
    }

    void OnNewLevel()
    {
        if (Level.Current.Wallpaper != CurrentProfile)
        {
            CurrentProfile = Level.Current.Wallpaper;
            GO();
        }
    }


    //[SerializeField] float Ratio;
    //public IEnumerator StaticGradient()
    //{
    //    GetComponent<Image>().sprite = TransetionSprite;
    //    var texture2D = GetComponent<Image>().sprite.texture;
    //    var resolution = TransetionSprite.texture.height;

    //    var frames = MixTime / Time.fixedDeltaTime;

    //    var rate = 1f / frames;
    //    var palette = CurrentProfile.Palette;

    //    var paletteCounter = 0;
    //    while (true)
    //    {
    //        var startColorInd = paletteCounter % palette.Length;
    //        var endColorInd = (paletteCounter + 1) % palette.Length;
    //        paletteCounter++;

    //        var startColor = palette[startColorInd];
    //        var endColor = palette[endColorInd];

    //        for (int i = 0; i < frames; i++)
    //        {

    //        }
    //    }
    //}
}
