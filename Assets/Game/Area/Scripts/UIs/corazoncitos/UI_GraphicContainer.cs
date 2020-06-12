using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using ToolsMandioca.Extensions;

public class UI_GraphicContainer : FrontendStatBase
{
    public Sprite backContainerSprite;
    public Sprite frontContainerSprite;

    float timer;
    float time_to_anim = 0.5f;
    bool anim;
    bool go;
    Vector3 original;
    Vector3 scaled;
    float quantanim = 0.1f;

    [Range(1, 100)]
    public int size = 50;

    public bool invert;

    List<GraphicContainer> containers = new List<GraphicContainer>();

    private void Awake()
    {
        original = transform.localScale;
        scaled = new Vector3(transform.localScale.x+ quantanim, transform.localScale.y + quantanim, transform.localScale.z + quantanim);
    }


    public override void OnValueChange(int value, int max =100, bool _anim = false)
    {
        if (_anim)
        {
            this.anim = true;
            go = true;
            timer = 0;
        }
        while (containers.Count < max) containers.Add(CreateContainer());

        for (int i = 0; i < containers.Count; i++)
        {
            int physical_position = i + 1;

            if (invert)
            {
                var invertedindex = containers.Count - (i+1);

                if (physical_position <= value) containers[invertedindex].TurnOn();
                else containers[invertedindex].TurnOff();
            }
            else
            {
                if (physical_position <= value) containers[i].TurnOn();
                else containers[i].TurnOff();
            }

            
        }
    }

    private void Update()
    {
        if (anim)
        {
            if (timer < time_to_anim)
            {
                timer = timer + 1 * Time.deltaTime;
                transform.localScale = go ? Vector3.Lerp(original, scaled, timer): Vector3.Lerp(scaled, original, timer);
            }
            else
            {
                if (go) go = false;
                else { anim = false; go = true; }
                timer = 0;
            }
        }
    }

    GraphicContainer CreateContainer()
    {
        var rparent = this.transform.gameObject.CreateDefaultSubObject<RectTransform>("Container");
        rparent.sizeDelta = new Vector2(size, size);

        var back = rparent.gameObject.CreateDefaultSubObject<Image>("back");
        var front = rparent.gameObject.CreateDefaultSubObject<Image>("front");

        back.Stretch();
        front.Stretch();

        back.sprite = backContainerSprite;
        front.sprite = frontContainerSprite;

        back.preserveAspect = true;
        front.preserveAspect = true;

        return new GraphicContainer(back, front);
    }

    public override void OnValueChangeWithDelay(int value, float delay, int max = 100, bool anim = false){}
}
