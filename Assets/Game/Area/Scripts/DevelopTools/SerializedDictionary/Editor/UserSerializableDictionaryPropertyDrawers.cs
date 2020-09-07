using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(DmgType_FloatDictionary))]
[CustomPropertyDrawer(typeof(EntityBase_CDListDictionary))]
[CustomPropertyDrawer(typeof(Float_TDListDictionary))]
public class AnySerializableDictionaryPropertyDrawer : SerializableDictionaryPropertyDrawer {}

[CustomPropertyDrawer(typeof(CombatDirectorStorage))]
[CustomPropertyDrawer(typeof(TotemDestruibleStorage))]
public class AnySerializableDictionaryStoragePropertyDrawer: SerializableDictionaryStoragePropertyDrawer {}
