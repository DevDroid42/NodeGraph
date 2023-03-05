using nodeSys2;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ColorTableEditor : EditorBase
{
    Property prop;
    Image img;
    Texture2D tex;
    public int resolution = 128;
    //create one colorVec array to avoid building and tearing down arrays
    private ColorVec[] colors;
    // Start is called before the first frame update
    void Start()
    {
        img = GetComponent<Image>();
        tex = new Texture2D(resolution, 1, TextureFormat.ARGB32, false);
        colors = new ColorVec[resolution];
    }

    // Update is called once per frame
    private float time;
    void Update()
    {
        if (time > 0.040)
        {
            time = 0;
            UpdateTexture();
        }
        time += Time.deltaTime;
    }

    public void UpdateTexture()
    {
        switch (prop.GetData())
        {
            case Evaluable data:
                {
                    if (img.sprite != null)
                    {
                        Destroy(img.sprite);
                    }
                    img.color = new Color(1f, 1f, 1f);

                    System.Diagnostics.Stopwatch stopWatch = new System.Diagnostics.Stopwatch();

                    //stopWatch.Start();
                    for (int i = 0; i < 1; i++)
                    {
                        UpdateTexture(data);
                    }
                    //stopWatch.Stop();
                    //Debug.Log("Threaded Speed: " + stopWatch.ElapsedMilliseconds);

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

    private void UpdateTexture(Evaluable data)
    {
        BatchEvaluation.EvaluateColorRangeByRef(colors, data);
        for (int i = 0; i < tex.width; i++)
        {
            ColorVec col = colors[i];
            //Debug.Log(col);
            tex.SetPixel(i, 0, new Color(col.rx, col.gy, col.bz, col.aw));
        }
    }

    public override void Setup(Property prop)
    {
        base.Setup(prop);
        this.prop = prop;
    }

    private void OnDestroy()
    {
        Destroy(tex);
        if (img != null)
        {
            if (img.sprite != null)
            {
                Destroy(img.sprite);
            }
        }
    }

}
