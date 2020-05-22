using UnityEngine.UI;

public class GraphicContainer
{
    Image background;
    Image front;

    public GraphicContainer(Image _background, Image _front)
    {
        background = _background;
        front = _front;
    }

    public void TurnOn() { front.enabled = true; }
    public void TurnOff() { front.enabled = false; }
}
