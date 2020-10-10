using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Answer_ReturnTrue : MonoBehaviour, IAnswerable
{
    public Answer Answer => new Answer(Gate.AND, () => true);
}
