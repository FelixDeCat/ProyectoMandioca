using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DmgType_FloatDictionary : SerializableDictionary<Damagetype, float> { }

[Serializable]
public class CombatDirectorStorage : SerializableDictionary.Storage<List<EnemyBase>> { }

[Serializable]
public class EntityBase_CDListDictionary : SerializableDictionary<EntityBase, List<EnemyBase>, CombatDirectorStorage> { }
