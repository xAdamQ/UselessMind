using UnityEngine;

public class PackSelector : MonoBehaviour
{
    readonly string
        PackPath = "Prefs/Pack",
        PackGatePrefPath = "Prefs/PackGate";


    void Start()
    {
        var packGatePref = Resources.Load<GameObject>(PackGatePrefPath);

        var names = LanguageManger.I.GetValues("PackName");


        foreach (var name in names)
        {
            var newGate = Instantiate(packGatePref, transform);
            newGate.transform.GetChild(0).GetComponent<UnityEngine.UI.Text>().text
            = name;
        }

    }

}
