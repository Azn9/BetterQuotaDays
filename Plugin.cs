using BepInEx;
using BepInEx.Logging;
using BetterQuotaDays.Patches;
using HarmonyLib;

namespace BetterQuotaDays
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        public static ManualLogSource LoggerInstance;
        
        private void Awake()
        {
            var harmony = new Harmony(PluginInfo.PLUGIN_GUID);
            harmony.PatchAll(typeof(TimeOfDayPatch));
            
            Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
            LoggerInstance = Logger;
        }
    }
}

namespace BetterQuotaDays.Patches
{
    [HarmonyPatch(typeof(TimeOfDay))]
    internal class TimeOfDayPatch
    {
        
        [HarmonyPatch("SetNewProfitQuota")]
        [HarmonyPostfix]
        private static void Postfix(TimeOfDay __instance)
        {
            var days = 4 + __instance.profitQuota / 1000;
            __instance.timeUntilDeadline = __instance.totalTime * days;
            __instance.quotaVariables.deadlineDaysAmount = days;
            
            Plugin.LoggerInstance.LogInfo($"New deadline: {days} days");
        }
        
        [HarmonyPatch("SyncNewProfitQuotaClientRpc")]
        [HarmonyPostfix]
        private static void Postfix2(TimeOfDay __instance)
        {
            var days = 4 + __instance.profitQuota / 1000;
            __instance.timeUntilDeadline = __instance.totalTime * days;
            __instance.quotaVariables.deadlineDaysAmount = days;
            
            Plugin.LoggerInstance.LogInfo($"New deadline: {days} days");
        }
        
    }
}
