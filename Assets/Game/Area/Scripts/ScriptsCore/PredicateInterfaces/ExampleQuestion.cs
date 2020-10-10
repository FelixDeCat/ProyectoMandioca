using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleQuestion : MonoBehaviour, IQuestionable
{
    public Question Question { get => new Question(Gate.AND, QuestionConnection.Connect(this, Answers)); }
    [SerializeField] Component[] Answers;

    void Start()
    {
        Debug.Log("la respuesta a la pregunta es: " + Question.Ask());
    }
}
