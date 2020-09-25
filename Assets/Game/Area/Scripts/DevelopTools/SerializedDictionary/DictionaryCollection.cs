using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DmgType_FloatDictionary : SerializableDictionary<Damagetype, float> { }

[Serializable]
public class CombatDirectorStorage : SerializableDictionary.Storage<List<CombatDirectorElement>> { }

[Serializable]
public class EntityBase_CDListDictionary : SerializableDictionary<EntityBase, List<CombatDirectorElement>, CombatDirectorStorage> { }

[Serializable]
public class TotemDestruibleStorage : SerializableDictionary.Storage<List<TotemDestruible>> { }

[Serializable]
public class Float_TDListDictionary : SerializableDictionary<float, List<TotemDestruible>, TotemDestruibleStorage> { }
