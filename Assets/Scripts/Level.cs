using UnityEngine;

[System.Serializable, CreateAssetMenu(fileName = "someLevel", menuName = "Mine/Level")]
public class Level : ScriptableObject
{
    public static Level Current;

    public static float MessageReadTime = 2f;

    public int CloneCount = 2;
    public Vector2Int GridSize;
    ///<summary>
    /// 0 - cards matched by id only
    /// 1 - color is a must
    /// 2 - shape
    ///</summary> 
    public int MatchingLevel = 0;
    public int whiteCount, jokerCount, monsterCount, coupleCount, MeteorCount, ExtraFlipsCount, FreezeCount;
    public float Time = -1;
    public int Flips = -1;
    public GravityDir GravityDir = GravityDir.NON;
    public WallpaperProfile Wallpaper;
    public string StartMessage;
    public bool NoMoney = false;

    public static int AllCount = 40;
    public static void DebugLevelsReport()
    {
        var allLev = Resources.LoadAll<Level>("Levels/0");
        System.Array.Sort(allLev, (o, o2) => int.Parse(o.name).CompareTo(int.Parse(o2.name)));
        var debugText = "";
        int i = 0;
        foreach (var lev in allLev)
        {
            debugText += i.ToString() + '\n';
            debugText += lev.GridSize.ToString() + '\n';

            if (lev.whiteCount != 0) debugText += "whiteCount: " + lev.whiteCount + '\n';
            if (lev.jokerCount != 0) debugText += "jokerCount: " + lev.jokerCount + '\n';
            if (lev.monsterCount != 0) debugText += "monsterCount: " + lev.monsterCount + '\n';
            if (lev.coupleCount != 0) debugText += "coupleCount: " + lev.coupleCount + '\n';
            if (lev.MeteorCount != 0) debugText += "MeteorCount: " + lev.MeteorCount + '\n';
            if (lev.ExtraFlipsCount != 0) debugText += "ExtraFlipsCount: " + lev.ExtraFlipsCount + '\n';
            if (lev.FreezeCount != 0) debugText += "FreezeCount: " + lev.FreezeCount + '\n';
            debugText += '\n';

            if (lev.CloneCount != 2) debugText += "CloneCount: " + lev.CloneCount + '\n';
            if (lev.MatchingLevel != 0) debugText += "MatchingLevel: " + lev.MatchingLevel + '\n';
            debugText += '\n';

            if (lev.Time != -1) debugText += "Time: " + lev.Time + '\n';
            if (lev.Flips != -1) debugText += "Flips: " + lev.Flips + '\n';
            debugText += '\n';

            if (lev.GravityDir != GravityDir.NON) debugText += "GravityDir: " + lev.GravityDir + '\n';
            //debugText += "Wallpaper: " + lev.Wallpaper.name + '\n';


            debugText += "\n\n";
            i++;
        }

        Debug.Log(debugText);
    }

    #region try

    //public static Level[][] All { get; private set; }
    //void FillRaw()
    //{
    //    var packs = Resources.LoadAll<TextAsset>("Levels");
    //    All = new Level[packs.Length][];
    //    for (int p = 0; p < packs.Length; p++)
    //    {
    //        var pack = packs[p].text.Split('\n');
    //        for (int l = 0; l < pack.Length; l++)
    //        {
    //            All[p][l] = SerializeLevel(pack[l]);
    //        }
    //    }

    //    Level SerializeLevel(string raw)
    //    {
    //        var props = raw.Split(' ');
    //        var newLev = new Level();
    //        for (int i = 0; i < props.Length; i++)
    //        {
    //            var parts = props[i].Split('=', '.');
    //            if (parts.Length == 3)
    //            {
    //                newLev.GridSize = new Vector2Int(int.Parse(parts[1]), int.Parse(parts[2]));
    //            }
    //            else if (parts.Length == 2)
    //            {
    //                var val = int.Parse(parts[1]);
    //                switch (props[0])
    //                {
    //                    case "flips":
    //                        Flips = val;
    //                        break;

    //                    case "time":
    //                        Time = val;
    //                        break;

    //                    case "joker":
    //                        jokerCount = val;
    //                        break;

    //                    case "gravity":
    //                        GravityDir = (GravityDir)val;
    //                        break;

    //                    case "monster":
    //                        monsterCount = val;
    //                        break;

    //                    case "freeze":
    //                        FreezeCount = val;
    //                        break;
    //                }
    //            }
    //            else if (parts.Length == 1)
    //            {
    //                switch (parts[0])
    //                {
    //                    case "nomoney":
    //                        break;
    //                    case "_-_-_-_-_-_":
    //                        break;
    //                }
    //            }

    //        }
    //        return newLev;
    //    }

    //}

    #endregion

}

public enum GravityDir { NON, Down, Up }