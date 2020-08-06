using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;

public class NodeEditor : EditorWindow
{
    
    private Vector2 _startMousePosition;
    private List<BaseNode> _allNodes;
    private BaseNode selected;
    
    private string currentName;

    public float toolbarHeight = 100;
    public GUIStyle labelStyle;
    public GUIStyle wrapTextFieldStyle;
    
    //Connect settings
    private bool isConnecting;
    private OptionButton currenteOptionButtonSelected;
    private BaseNode currentInitialConectionNode;
    float conectionSpace = 0;
    
    //paneo
    private bool _panning;
    public Vector2 offset;
    private Vector2 _prevOffset;
    private Vector2 _originalMousePos;
    public Rect graphRect;

    //nodos
    private int dialogueNodeWidth = 225;
    private int dialogueNodeHeight = 200;
    private int portalNodeWidth = 80;
    private int portalNodeHeight = 80;
    
    //Dialogue Tree
    public DialogueTree currentDialogueTree;
    private bool loadedTree = false;
    
    [MenuItem("CustomTools/Node Editor")]
    public static void ShowWindow()
    {
        var myWindow = GetWindow<NodeEditor>();
        myWindow._allNodes = new List<BaseNode>();
        
        myWindow.labelStyle = new GUIStyle();
        myWindow.labelStyle.fontSize = 20;
        myWindow.labelStyle.alignment = TextAnchor.MiddleCenter;
        
        myWindow.wrapTextFieldStyle = new GUIStyle(EditorStyles.textField);
        myWindow.wrapTextFieldStyle.wordWrap = true;
        
        myWindow.offset = new Vector2(0, myWindow.toolbarHeight);
        myWindow.graphRect = new Rect(0, myWindow.toolbarHeight,10000000, 10000000 );
    }

    private void OnGUI()
    {
        
        //Dibujo la toolbox
        DrawToolBar();
        //Veo que hace el mouse
        CheckMouseInput(Event.current);
        //Dibujo la mesa de trabajo
        DrawWorkBench();
        
    }

    void DrawWorkBench()
    {
        //Si no hay un arbol de dialogos puesto aca, no pasa nada
        if (currentDialogueTree == null)
        {
            loadedTree = false;
            return;
        }

        //pongo esta flag para que solo se cargue una vez todos los nodos
        if (!loadedTree)
        {
            currentName = currentDialogueTree.treeName;
            _allNodes = new List<BaseNode>();
            for (int i = 0; i < currentDialogueTree.dialogueNodes.Count; i++)
            {
                LoadNode(currentDialogueTree.editorNodePos[i].pos,
                        currentDialogueTree.dialogueNodes[i].dialogues, currentDialogueTree.dialogueNodes[i].conected, currentDialogueTree.editorNodePos[i].nodeType);
            }
        
            loadedTree = true;
        }

        //Cambio el color de la mesa de trabajo
        graphRect.x = offset.x;
        graphRect.y = offset.y;
        EditorGUI.DrawRect(new Rect(0, toolbarHeight, position.width, position.height - toolbarHeight), Color.grey );
       // EditorGUI.DrawRect(graphRect, Color.grey );
        
        GUI.BeginGroup(graphRect);
        {
            BeginWindows();
            {
                var originalBG = GUI.backgroundColor;
                
                for (int i = 0; i < _allNodes.Count; i++)
                {
                    if(_allNodes[i].GetType() == typeof(EditorPortalNode))
                        continue;
                    
                    foreach (var connected in _allNodes[i].connected)
                    {
                        if (connected.destination >= 0) //conecto los nodos con sus opciones con Beziers
                        {
                            if (_allNodes[i].closed)
                            {
                                var startPosCompress = _allNodes[i].myRect.position + new Vector2(_allNodes[i].myRect.width / 2, _allNodes[i].myRect.height / 2) ;
                                var endPosCompress = _allNodes[connected.destination].myRect.center - new Vector2(_allNodes[connected.destination].myRect.width / 2, 0) ;
                                
                                Handles.DrawBezier(startPosCompress, endPosCompress, startPosCompress + new Vector2(200, 0),
                                    endPosCompress + new Vector2(-100, 0),
                                    Color.blue, EditorGUIUtility.whiteTexture, 2);
                            }
                            else
                            {
                                var startPosExpanded = connected.myRect.center + new Vector2(connected.myRect.width / 2, 0) + _allNodes[i].myRect.position;
                                var endPosExpanded = _allNodes[connected.destination].myRect.center - new Vector2(_allNodes[connected.destination].myRect.width / 2, 0) ;
                                
                                Handles.DrawBezier(startPosExpanded, endPosExpanded, startPosExpanded + new Vector2(200, 0),
                                    endPosExpanded + new Vector2(-100, 0),
                                    Color.blue, EditorGUIUtility.whiteTexture, 2);
                            }
                        }
                    }
                }

                for (int i = 0; i < _allNodes.Count; i++)
                {
                    if (_allNodes[i] == selected)
                    {
                        GUI.backgroundColor = Color.green;
                    }

                                        
                    //Le doy su ID al nodo
                    _allNodes[i].id = i;
                    
                    _allNodes[i].myRect = GUI.Window(i,_allNodes[i].myRect, DrawNode, _allNodes[i].name);
                    GUI.backgroundColor = originalBG;
                    
                }
            }
            EndWindows();
        }
        GUI.EndGroup();
        
    }

    void DrawToolBar()
    {
        //Header (es la toolbox de arriba)
        EditorGUILayout.BeginVertical(GUILayout.Height(toolbarHeight));
        {
            string title = "My dialogue editor";
            if (currentDialogueTree != null)
            {
                title += $" currently editing: {currentDialogueTree.treeName}";
            }
            
            EditorGUILayout.LabelField(title, labelStyle);//titulo

            if (currentDialogueTree == null)
            {
                //lugar donde se pone el current tree
                currentDialogueTree = (DialogueTree)EditorGUILayout.ObjectField(currentDialogueTree, typeof(DialogueTree), false, GUILayout.Width(100));
                
                
                currentName = EditorGUILayout.TextField("DialogueTreeName", currentName, GUILayout.Width(400));

                if (currentName == "" || currentName == null)
                {
                    EditorGUILayout.HelpBox("Give a name to the tree to be able to create one", MessageType.Warning);
                }
                else
                {
                    if (GUILayout.Button("Create new dialogue tree", GUILayout.Width(200), GUILayout.Height(25)))
                    {
                        currentDialogueTree = CreateInstance<DialogueTree>();
                        currentDialogueTree.treeName = currentName;
                        AssetDatabase.CreateAsset(currentDialogueTree, "Assets/Game/Area/Scripts/Dialogue/" + currentName + ".asset");
                        currentName = "";
                    }    
                }
            }
            else
            {    //Se guarda el dialogueTree
                if (GUILayout.Button("Save Dialogue Tree", GUILayout.Width(200), GUILayout.Height(25)))
                {
                    SaveDialogueTree();
                }
            }


            //Botones de la toolbar
            EditorGUILayout.BeginHorizontal();
            {
                
                
                if (GUILayout.Button("Crear Dialogue Nodo", GUILayout.Width(140), GUILayout.Height(30)))
                {
                    AddDialogueNode();
                }
                if (GUILayout.Button("Crear Portal Nodo", GUILayout.Width(140), GUILayout.Height(30)))
                {
                    AddPortalNode();
                }
                if (GUILayout.Button("Borrar Nodo", GUILayout.Width(100), GUILayout.Height(30)))
                {
                    DeleteNode();
                }
                
                if (GUILayout.Button("Clean Workbench", GUILayout.Width(200), GUILayout.Height(25)))
                {
                    CleanWorkbench();
                }
            }
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndVertical();
        
        Repaint();
    }

    void CleanWorkbench()
    {
        currentDialogueTree = null;
        _allNodes.Clear();
        Repaint();
    }
    
    private void SaveDialogueTree()
    {
        currentDialogueTree.dialogueNodes = new List<DialogueNode>();
        currentDialogueTree.editorNodePos = new List<EditorNodeDATA>();

        //currentDialogueTree.treeName = currentName;
        
        foreach (BaseNode node in _allNodes)
        {
            var nodeDATA = new EditorNodeDATA();
            nodeDATA.id = node.id;
            nodeDATA.pos = node.myRect.position;
            nodeDATA.nodeType = node.nType.ToString();
            
            currentDialogueTree.editorNodePos.Add(nodeDATA); //guardo la posicion de las ventanas de los nodos
            
            List<Choice> newChoices = new List<Choice>();
            
            for (int i = 0; i < node.connected.Count; i++)
            {
                Choice nChoice = new Choice(node.connected[i].text, node.connected[i].destination);
                newChoices.Add(nChoice);
            }
            
            DialogueNode dn = new DialogueNode(node.id, node.dialogues, newChoices);
            currentDialogueTree.dialogueNodes.Add(dn);
        }
        EditorUtility.SetDirty(currentDialogueTree);

    }

    private void DeleteNode()
    {
        if (selected == null)
            return;

        //Tengo que sacar al nodo borrado de todas las conexiones con otros nodos
        for (int i = 0; i < _allNodes.Count; i++)
        {
            for (int j = 0; j < _allNodes[i].connected.Count; j++)
            {
                if (_allNodes[i].connected[j].destination == selected.id)
                    _allNodes[i].connected.Remove(_allNodes[i].connected[j]);
            }
        }
        //Lo saco del total de nodos
        _allNodes.Remove(selected);
        
        SaveDialogueTree();
    }

    // // // // // // // // // // // // // // // // // // // // // // // // // // // // // // // /
    //CHEQUEA MOUSE INPUT
    private void CheckMouseInput(Event e)
    {
        if (!graphRect.Contains(e.mousePosition) || !(focusedWindow == this) || !(mouseOverWindow == this))
            return;
        
        
        //Node conection
        if (isConnecting && currentInitialConectionNode != null && selected != currentInitialConectionNode)
        {
            isConnecting = false;
            
            for (int i = 0; i < _allNodes.Count; i++)
            {
                if (selected == _allNodes[i])
                {
                    currenteOptionButtonSelected.destination = _allNodes[i].id;
                    SaveDialogueTree();
                    ResetConections();
                }
            }
            Repaint();
            return;
        }

        //Panning
        if (e.button == 1 && e.type == EventType.MouseDown)
        {
            _panning = true;
            _prevOffset = new Vector2(graphRect.x, graphRect.y);
            _originalMousePos = e.mousePosition;
        }else if(e.button == 1 && e.type == EventType.MouseUp)
        {
            _panning = false;
        }

        if (_panning)
        {
            var newX = _prevOffset.x + e.mousePosition.x - _originalMousePos.x;
            offset.x = newX > 0 ? 0 : newX;
            
            var newY = _prevOffset.y + e.mousePosition.y - _originalMousePos.y;
            offset.y = newY > toolbarHeight ? toolbarHeight : newY;
            
            Repaint();
        }

        
        //node selection
        BaseNode overNode = null;
        for (int i = 0; i < _allNodes.Count; i++)
        {
            _allNodes[i].CheckMouse(e, offset);
            if (_allNodes[i].OverNode)
                overNode = _allNodes[i];
        }

        var prevSel = selected;
        if (e.button == 0 && e.type == EventType.MouseDown)
        {
            if (overNode != null)
                selected = overNode;
            else
            {
                selected = null;
            }
            
            if(prevSel != selected)
                Repaint();
        }
        
        
    }

    // // // // // // // // // // // // // // // // // // // // // // // // // // // // // // // /
    //Este arma un nodo vacio
    void AddDialogueNode()
    {
        _allNodes.Add(new EditorDialogueNode(-offset.x,-offset.y + toolbarHeight, dialogueNodeWidth, dialogueNodeHeight, currentName, NodeType.Dialogue));
        currentName = "";
    }
    
    void AddPortalNode()
    {
        _allNodes.Add(new EditorPortalNode(-offset.x,-offset.y + toolbarHeight, portalNodeWidth, portalNodeHeight, currentName, NodeType.Portal));
        currentName = "";
    }
    
    // // // // // // // // // // // // // // // // // // // // // // // // // // // // // // // /
    //Lo uso para cargar los nodos de un arbol
    void LoadNode(Vector2 rectPos, List<string> dialogues, List<Choice> conections, string nodeType)
    {
        BaseNode baseNode; 
            //Se fija si es un portal o uno de dialogo normal
        if (nodeType == NodeType.Dialogue.ToString())
        {
            baseNode = new EditorDialogueNode(rectPos.x, rectPos.y, dialogueNodeWidth, dialogueNodeHeight, currentName,NodeType.Dialogue);
        }
        else
        {
            var node = new EditorPortalNode(rectPos.x, rectPos.y, portalNodeWidth, portalNodeHeight, currentName,NodeType.Portal);
            
            if(node.connected.Any()) node.portalDestination = conections[0].conection.ToString();
            
            baseNode = node;

        }
        

        for (int i = 0; i < dialogues.Count; i++)
        {
            baseNode.dialogues.Add(dialogues[i]);
        }

        for (int i = 0; i < conections.Count; i++)
        {
            OptionButton oButton = new OptionButton();
            oButton.destination = conections[i].conection;
            oButton.text = conections[i].text;
            baseNode.connected.Add(oButton);
        }
        _allNodes.Add(baseNode);
        Repaint();
    }
    
    // // // // // // // // // // // // // // // // // // // // // // // // // // // // // // // /
    //      DIBUJO CADA NODO       // 
    void DrawNode(int id)
    {
        if (_allNodes[id].GetType() == typeof(EditorDialogueNode))
        {
            DrawEditorDialogueNode(id);
        }
        else
        {
            DrawPortalNode(id);
        }
        
        //Arrastrar nodo
        if (!_panning)
        {
            GUI.DragWindow();

            if (!_allNodes[id].OverNode) return;
        
            //Clamp los valores asi no se va de la ventana
            if (_allNodes[id].myRect.x < 0)
                _allNodes[id].myRect.x = 0;
        
            if (_allNodes[id].myRect.y < toolbarHeight - offset.y)
                _allNodes[id].myRect.y = toolbarHeight - offset.y;

        }
    }

    void DrawPortalNode(int id)
    {
        if (_allNodes[id].GetType() == typeof(EditorPortalNode))
        {
            var currentNode = (EditorPortalNode) _allNodes[id];
            var aux = currentNode
                .portalDestination; //me lo guardo para chequear si hubo un cambio en el nodo donde se conecta el portal
            //Le pone el titulo
            EditorGUILayout.BeginHorizontal();
            {
                GUILayout.Label("PORTAL");
                GUILayout.Label(_allNodes[id].id.ToString());
            }
            EditorGUILayout.EndHorizontal();

            //Aca es donde se dice a que nodo va a ir el portal

            currentNode.portalDestination =
                EditorGUILayout.TextField(currentNode.portalDestination, GUILayout.Width(50));

            if (currentNode.portalDestination != aux)
            {
                _allNodes[id].connected.Clear();
                OptionButton newChoice = new OptionButton();
                if (currentNode.portalDestination != null)
                    newChoice.destination = int.Parse(currentNode.portalDestination);
                _allNodes[id].connected.Add(newChoice);
            }
        }
    }
    
    private void DrawEditorDialogueNode(int id)
    {
        //Le pone el titulo
        EditorGUILayout.BeginVertical();
        {
            _allNodes[id].closed = GUILayout.Toggle(_allNodes[id].closed, "Open/close");
            GUILayout.Label($"ID {_allNodes[id].id}");
        }
        EditorGUILayout.EndVertical();
       
        
        //Boton que crea una nueva caja de dialogo
        if (GUILayout.Button("New dialogue", GUILayout.Width(90), GUILayout.Height(20)))
        {
            string newDialogue = "";
            _allNodes[id].dialogues.Add(newDialogue);
            
        }

        //Ve la cantidad de dialogos que hay en el nodo y crea texto para cada uno
        EditorGUILayout.BeginVertical();
        {
            for (int i = 0; i < _allNodes[id].dialogues.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Dialogo " + i);
                _allNodes[id].dialogues[i] = EditorGUILayout.TextField(_allNodes[id].dialogues[i], wrapTextFieldStyle, GUILayout.Height(50));

                if (GUILayout.Button("X", GUILayout.Width(25), GUILayout.Height(25)))
                {
                    _allNodes[id].dialogues.Remove(_allNodes[id].dialogues[i]);
                    SaveDialogueTree();
                }
                
                EditorGUILayout.EndHorizontal();
            }
        }
        EditorGUILayout.EndVertical();
        
        //Boton que crea una nueva conection
        if (GUILayout.Button("New conection", GUILayout.Width(100), GUILayout.Height(20)))
        {
            OptionButton newChoice = new OptionButton();
            newChoice.destination = -1;
            _allNodes[id].connected.Add(newChoice);
            
        }
        
        
        
        //Ve la cantidad de conections que hay en el nodo y crea texto y boton para cada una
        EditorGUILayout.BeginVertical();
        {
            for (int i = 0; i < _allNodes[id].connected.Count; i++)
            {
                OptionButton currentOptionButton = _allNodes[id].connected[i]; //referencio la opcion
                currentOptionButton.id = i; //le doy un id
                
                EditorGUILayout.BeginHorizontal();
                    GUILayout.Label("Conection"); //titulo
                    currentOptionButton.text = EditorGUILayout.TextField(currentOptionButton.text, //cuadro de texto de la opcion
                                                        wrapTextFieldStyle, GUILayout.Height(20));

                    bool pressedButton = GUILayout.Button("Connect"); // boton para conectar
                    
                    if (Event.current.type == EventType.Repaint)
                    {
                        currentOptionButton.myRect = GUILayoutUtility.GetLastRect();//guardo el rect del boton
                        conectionSpace = currentOptionButton.myRect.height;
                    }
                    if (pressedButton)
                    {
                        if (currenteOptionButtonSelected != null)//si aprete conect y ya habia apretado en otro, cancelo.
                        {
                            ResetConections();
                            return;
                        }

                        //me guardo las settings de esta conexion
                        currentInitialConectionNode = _allNodes[id];
                        currenteOptionButtonSelected = currentOptionButton; 
                        isConnecting = true;
                        Repaint();    
                    }

                    if (GUILayout.Button("X"))
                    {
                        _allNodes[id].connected.Remove(_allNodes[id].connected[i]);
                    }
                EditorGUILayout.EndHorizontal();
            }
        }
        EditorGUILayout.EndVertical();
        

        //Borra todas las conections
        if (GUILayout.Button("Break conections"))
        {
            _allNodes[id].connected = new List<OptionButton>();
            Repaint();
            SaveDialogueTree();
        }

        //abre o cierra dependiendo del boolean
        if (_allNodes[id].closed)
        {
            _allNodes[id].myRect.width = 100;
            _allNodes[id].myRect.height = 40;
        }
        else
        {
            _allNodes[id].myRect.width = dialogueNodeWidth;
            _allNodes[id].myRect.height = dialogueNodeHeight + _allNodes[id].dialogues.Count * 50 + + _allNodes[id].connected.Count * conectionSpace;
            
        }
    }
    
    private void ResetConections()
    {
        isConnecting = false;
        currenteOptionButtonSelected = null;
        currentInitialConectionNode = null;
    }
}
