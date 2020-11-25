using nodeSys2;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ColorTableEditor : EditorBase
{
    Property propCache;
    Image img;
    Texture2D tex;
    public int resolution = 256;
    // Start is called before the first frame update
    void Start()
    {
        img = GetComponent<Image>();
        tex = new Texture2D(resolution, 1, TextureFormat.ARGB32, false);
    }

    // Update is called once per frame
    void Update()
    {
        switch (propCache.GetData())
        {
            case EvaluableColorTable testTable:
                {
                    if(img.sprite != null)
                    {
                        Destroy(img.sprite);
                    }
                    img.color = new Color(1f, 1f, 1f);

                    for (int i = 0; i < tex.width; i++)
                    {
                        ColorVec col = testTable.EvaluateColor((float)i / tex.width, 0, 0, 0);
                        //Debug.Log(col);
                        tex.SetPixel(i, 0, new Color(col.rx, col.gy, col.bz, col.aw));
                    }
                    tex.Apply();
                    img.sprite = Sprite.Create(tex, new Rect(0, 0, resolution, 1), new Vector2(0.5f, 0.5f));
                    break;
                }
            default:
                Debug.LogWarning("ColorTableEditor received a property with invalid data.");
                img.color = new Color(0f, 0f, 0f);
                break;
        }

    }

    public override void Setup(Property prop)
    {
        base.Setup(prop);
        propCache = prop;
    }

    private void OnDestroy()
    {
        Destroy(tex);
        Destroy(img.sprite);
    }

}
