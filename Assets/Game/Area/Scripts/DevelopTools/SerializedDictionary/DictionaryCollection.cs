using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DmgType_FloatDictionary : SerializableDictionary<Damagetype, float> { }

[Serializable]
public class CombatDirectorStorage : SerializableDictionary.Storage<List<CombatDirectorElement>> { }

[Serializable]
public class EnemyStorage : SerializableDictionary.Storage<EnemyBase[]> { }

[Serializable]
public class EntityBase_CDListDictionary : SerializableDictionary<Transform, List<CombatDirectorElement>, CombatDirectorStorage> { }

[Serializable]
public class LifePercent_EnemyBaseDictionary : SerializableDictionary<LifePercent, EnemyBase[], EnemyStorage> { }

[Serializable]
public class EnemyBase_IntDictionary : SerializableDictionary<EnemyBase, int> { }

[Serializable]
public class TotemDestruibleStorage : SerializableDictionary.Storage<List<TotemDestruible>> { }

[Serializable]
public class Float_TDListDictionary : SerializableDictionary<float, List<TotemDestruible>, TotemDestruibleStorage> { }

[Serializable]
public class Int_IntDictionary : SerializableDictionary<int, int> { }

[System.Serializable]
public class LifePercent
{
    [Range(0, 1)] public float minPercent = 0;
    [Range(0, 1)] public float maxPercent = 1;
}
