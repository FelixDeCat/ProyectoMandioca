using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Console : MonoBehaviour
{
    bool open;
    //public List<Graphic> auxs;
    public GameObject go;

    public Text message;
    public InputField inputf;
    public Dictionary<string, Func<string, bool>> commands = new Dictionary<string, Func<string, bool>>();
    public static Console instance;
    public void CreateCommands(string command, Func<string, bool> function) { commands.Add(command, function); }
    public void ExcecuteCommand(string command, string param)
    {
        if (commands.ContainsKey(command))
        {
            if (!commands[command].Invoke(param)) message.text += "\n-> Parametro incorrecto, prueba con \"" + command + " help\"";
        }
        else message.text += "\n-> Este comando no existe en la consola, prueba con el comando \"help\" o \"?\" "; 
    }



    private void Awake()
    {
        instance = this;

        CreateCommands("console", ConsoleCommands);
        CreateCommands("help", HelpConsole);
        CreateCommands("exit", Quit);
        CreateCommands("?", HelpConsole);
    }
    private void Start()
    {
        open = false;
        go.SetActive(false);
    }

    public void Message(string s)
    {
        message.text += "\n-> " + s;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Plus) || Input.GetKeyDown(KeyCode.Pipe) || Input.GetKeyDown(KeyCode.Tilde) || Input.GetKeyDown(KeyCode.Alpha0) || Input.GetKeyDown(KeyCode.Asterisk))
        {
            if (!open)
            {
                open = true;
                go.SetActive(true);
                inputf.Select();
            }
            else
            {
                open = false;
                go.SetActive(false);
                
            }
        }
    }

    public void InputOnEnter(string s)
    {

        inputf.text = "";

        string[] aux = s.Split(' ');
        if (aux.Length < 2) { ExcecuteCommand(aux[0],"");  return; }
        ExcecuteCommand(aux[0], aux[1]);
    }

    public bool Quit(string s)
    {
        Application.Quit();
        return true;
    }

    public bool HelpConsole(string s)
    {
        message.text += "\n┌─────────────────┐\n";
        message.text += "\n│ COMMANDS        │\n";
        message.text += "\n└─────────────────┘\n";
        foreach (var v in commands)
        {
            message.text += "-> "+ v.Key +"\n";
        }
        message.text += "--------------------------------------------------\n";
        return true;
    }

    public bool ConsoleCommands(string s)
    {
        if (s == "clear")
        {
            message.text = "";
            return true;
        }

        if (s == "help")
        {
            message.text += "\n------------------------------------------------------\n" +
                "-> clear: Limpia la consola \n" +
                "-> help: Muestra los comandos de consola \n" +
                "-> Color: Pinta el texto de la consola... \n" +
             "<Comandos de colores, respetar Mayusculas>\n" +
             "--->Color.White\n" +
             "--->Color.Yellow\n" +
             "--->Color.Red\n" +
             "--->Color.Magenta\n" +
             "--->Color.Blue\n" +
             "--->Color.Green\n" +
             "--->Color.Cyan\n" +
             "--->Color.Gray\n" +
             "--->Color.Grey\n" +
             "------------------------------------------------------";
            return true;
        }

        switch (s)
        {
            case "Color": message.color = Color.white; return true;
            case "Color.White": message.color = Color.white; return true;
            case "Color.Yellow": message.color = Color.yellow; return true;
            case "Color.Red": message.color = Color.red; return true;
            case "Color.Magenta": message.color = Color.magenta; return true;
            case "Color.Blue": message.color = Color.blue; return true;
            case "Color.Green": message.color = Color.green; return true;
            case "Color.Cyan": message.color = Color.cyan; return true;
            case "Color.Gray": message.color = Color.gray; return true;
            case "Color.Grey": message.color = Color.gray; return true;
        }

        return false;
    }
}
