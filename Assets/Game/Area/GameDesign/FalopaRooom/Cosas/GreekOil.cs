using Tools.EventClasses;
using UnityEngine;

public class GreekOil : MonoBehaviour
{
    [SerializeField] EventCounterPredicate predicate = new EventCounterPredicate();
    [SerializeField] float duration = 60;
    CharacterHead character;
    public bool MyPredicate() => Main.instance.GetChar().CanUseEffect();
    private void Start() => predicate.Invoke(MyPredicate);
    public void OnEquip() => character = Main.instance.GetChar();
    public void FireInTheSword() => character.TurnOnGreekOilEffect(duration);

}
