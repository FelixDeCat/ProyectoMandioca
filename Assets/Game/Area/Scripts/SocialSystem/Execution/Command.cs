using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Command : MonoBehaviour
{
    public static Command instance;
    private void Awake() => instance = this;

    CommandBranch master_branch = new CommandBranch("master", true);

    void ConfigureDirection(Action<string> dir, params string[] posibles_commands) => master_branch.AddLeaf(dir, posibles_commands);
    void ConfigureDirection(CommandBranch branch) => master_branch.AddBranch(branch);
    void ExecuteCommand(string cmd) => master_branch.ExecuteCommand(cmd);

    public static void AddLeaf(Action<string> direction, params string[] masterLines) => Command.instance.ConfigureDirection(direction, masterLines);
    public static void AddBranch(CommandBranch subbranch) => Command.instance.ConfigureDirection(subbranch);
    public static void Execute(string cmd) => Command.instance.ExecuteCommand(cmd);

    /////////////////////////////////////////////////////////////////
    /// E X A M P L E
    /////////////////////////////////////////////////////////////////
    ///
    private void Start()
    {
        //Command
        //    .AddBranch(new CommandBranch("Item")
        //        .AddLeaf(Add, "Add")
        //        .AddLeaf(Remove, "Remove")
        //        .AddLeaf(Equip, "Equip")
        //        .AddBranch(
        //        new CommandBranch("Print")
        //            .AddLeaf(PrintNegro, "Negro")
        //            .AddLeaf(PrintBlanco, "Blanco")));


        
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B)) Command.Execute("Item_Add_Potadesalud");
        if (Input.GetKeyDown(KeyCode.N)) Command.Execute("Item_Remove_Potadesalud");
        if (Input.GetKeyDown(KeyCode.M)) Command.Execute("Item_Equip_Potadesalud");
        if (Input.GetKeyDown(KeyCode.G)) Command.Execute("Item_Print_Negro_Potadesalud");
        if (Input.GetKeyDown(KeyCode.H)) Command.Execute("Item_Print_Blanco_Potadesalud");
    }
    void Add(string item) => Debug.Log("Add: Recibí: " + item);
    void Remove(string item) => Debug.Log("Remove: Recibí: " + item);
    void Equip(string item) => Debug.Log("Equip: Recibí: " + item);
    void PrintNegro(string item) => Debug.Log("Print negro: " + item);
    void PrintBlanco(string item) => Debug.Log("Print blanco: " + item);

}
