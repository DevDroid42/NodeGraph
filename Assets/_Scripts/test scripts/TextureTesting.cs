using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextureTesting : MonoBehaviour
{
    Image img;

    EvaluableColorTable testTable;
    // Start is called before the first frame update
    void Start()
    {
        img = GetComponent<Image>();
        testTable = new EvaluableColorTable(60);
        int keyAmt = testTable.GetkeyAmt();
        for (int i = 0; i < keyAmt; i++)
        {
            testTable.SetKey(i, ColorOperations.HsvToRgb(i * (1f / keyAmt), 1, 1));
        }
    }

    float offset = 0;
    // Update is called once per frame
    void Update()
    {
        offset += 0.1f * Time.deltaTime;
        Texture2D tex = new Texture2D(100, 1, TextureFormat.ARGB32, false);
        for (int i = 0; i < tex.width; i++)
        {
            ColorVec col = testTable.EvaluateColor((float)i / tex.width + offset, 0, 0, 0);
            //Debug.Log(col);
            tex.SetPixel(i, 0, new Color(col.rx, col.gy, col.bz, col.aw));
        }
        tex.Apply();
        img.sprite = Sprite.Create(tex, new Rect(0, 0, 100, 1), new Vector2(0.5f, 0.5f));
    }
}
