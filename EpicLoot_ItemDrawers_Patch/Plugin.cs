using System.Reflection;
using System.Runtime.CompilerServices;
using BepInEx;
using EpicLoot;
using EpicLoot.Crafting;
using HarmonyLib;

namespace EpicLoot_ItemDrawers_Patch
{
    [BepInPlugin("mods.kyo.epicloot_itemdrawers_patch", "EpicLoot ItemDrawers Patch", "1.0.0")]
    [BepInDependency("randyknapp.mods.epicloot")]
    [BepInDependency("mkz.itemdrawers")]
    public class Plugin : BaseUnityPlugin
    {
        public static Plugin Instance;

        private Harmony _harmony;
        
        void Awake()
        {
            Instance = this;
            _harmony = Harmony.CreateAndPatchAll(typeof(DrawerContainer_Patch));
        }

        void OnDestroy()
        {
            Instance = null;
            _harmony.UnpatchSelf();
        }
        
        [HarmonyPatch(typeof(DrawerContainer), "UpdateVisuals")]
        private class DrawerContainer_Patch
        {
            static void Postfix(DrawerContainer __instance)
            {
                if (typeof(DrawerContainer).GetField("_item", BindingFlags.NonPublic | BindingFlags.Instance)?.GetValue(__instance) is ItemDrop.ItemData item && __instance._image != null)
                {
                    if (item.IsMagicCraftingMaterial())
                    {
                        var rarity = item.GetCraftingMaterialRarity();
                        __instance._image.sprite = item.m_shared.m_icons[EpicLoot.EpicLoot.GetRarityIconIndex(rarity)];
                    }
                }
            }
        }

        public static void LogInfo(object value)
        {
            Instance.Logger.LogInfo(value);
        }
    }
}