using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(DmgType_FloatDictionary))]
[CustomPropertyDrawer(typeof(EntityBase_CDListDictionary))]
[CustomPropertyDrawer(typeof(Float_TDListDictionary))]
[CustomPropertyDrawer(typeof(Int_IntDictionary))]
[CustomPropertyDrawer(typeof(EnemyBase_IntDictionary))]
[CustomPropertyDrawer(typeof(LifePercent_EnemyBaseDictionary))]
public class AnySerializableDictionaryPropertyDrawer : SerializableDictionaryPropertyDrawer {}

[CustomPropertyDrawer(typeof(CombatDirectorStorage))]
[CustomPropertyDrawer(typeof(TotemDestruibleStorage))]
[CustomPropertyDrawer(typeof(EnemyStorage))]
public class AnySerializableDictionaryStoragePropertyDrawer: SerializableDictionaryStoragePropertyDrawer {}
