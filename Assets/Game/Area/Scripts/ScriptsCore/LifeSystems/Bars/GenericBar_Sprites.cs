using UnityEngine.UI;

public class GenericBar_Sprites : GenericBar
{
    public Image sp;

    protected override void OnSetValue(float value)
    {
        var percent = (value * 100) / maxValue;
        if (porcentaje) porcentaje.text = !realvalue ? ((int)percent).ToString() + "%" : value + " / " + maxValue;
        sp.fillAmount = percent * scaler;
    }
}
