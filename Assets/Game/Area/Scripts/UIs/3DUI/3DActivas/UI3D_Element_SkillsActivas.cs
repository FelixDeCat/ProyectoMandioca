using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/*
 * que tienen las activas?
 * cooldown
 * tiempo de uso
 * Hover si te paras encima de informacion
 * - nombre
 * - descripcion lore
 * - descripcion tecnica
 * 
*/

public class UI3D_Element_SkillsActivas : UI3D_Element_WithFeedbacks
{
    [Header("UI3D_Element_SkillsActivas_Settings")]
    public ParticleSystem part_end_load;
    SkillInfo mySkillInfo;
    public Image img_to_fill;
    bool ocupied = false;
    public void Ocupy_place() => ocupied = true;
    public void Vacate_place() => ocupied = false;
    public bool IsOcupied() => ocupied;
    public void SetSkillInfo(SkillInfo skillInfo) => mySkillInfo = skillInfo;
    public void RemoveSkillInfo() => mySkillInfo = null;
    public void SetCooldown(float val) => img_to_fill.fillAmount = val;
    public void SkillLoaded() => part_end_load.Play();
    public void SetUnlocked() { Set_Interactable = true; }
    public void SetBlocked() { Set_Interactable = false; }
}
