using BepInEx;
using BepInEx.Logging;
using BepInEx.Configuration;
using HarmonyLib;
using Steamworks.Ugc;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using LethalLib;
using LethalLib.Modules;
using System;

namespace SM64Scrap

{
    [BepInPlugin(GUID, NAME, VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        const string GUID = "SM64Scrap";
        const string NAME = "SM64 Scrap";
        const string VERSION = "2.0.1";

        private ConfigEntry<int> configStarSpawnWeight;
        private ConfigEntry<LethalLib.Modules.Levels.LevelTypes> configStarMoonSpawns;
        private ConfigEntry<int> configCoinSpawnWeight;
        private ConfigEntry<LethalLib.Modules.Levels.LevelTypes> configCoinMoonSpawns;
        private ConfigEntry<int> configBlueCoinSpawnWeight;
        private ConfigEntry<LethalLib.Modules.Levels.LevelTypes> configBlueCoinMoonSpawns;
        private ConfigEntry<int> configRedCoinSpawnWeight;
        private ConfigEntry<LethalLib.Modules.Levels.LevelTypes> configRedCoinMoonSpawns;
        private ConfigEntry<int> configBlockSpawnWeight;
        private ConfigEntry<LethalLib.Modules.Levels.LevelTypes> configBlockMoonSpawns;
        private ConfigEntry<int> configMIPSSpawnWeight;
        private ConfigEntry<LethalLib.Modules.Levels.LevelTypes> configMIPSMoonSpawns;
        private ConfigEntry<int> configBobOmbSpawnWeight;
        private ConfigEntry<LethalLib.Modules.Levels.LevelTypes> configBobOmbMoonSpawns;
        private ConfigEntry<int> configOneUpSpawnWeight;
        private ConfigEntry<LethalLib.Modules.Levels.LevelTypes> configOneUpMoonSpawns;
        private ConfigEntry<int> configCapSpawnWeight;
        private ConfigEntry<LethalLib.Modules.Levels.LevelTypes> configCapMoonSpawns;
        private ConfigEntry<int> configBowserKeySpawnWeight;
        private ConfigEntry<LethalLib.Modules.Levels.LevelTypes> configBowserKeyMoonSpawns;
        private ConfigEntry<int> configShellSpawnWeight;
        private ConfigEntry<LethalLib.Modules.Levels.LevelTypes> configShellMoonSpawns;

        public static Plugin instance;

        void Awake()
        {
            // Star spawn config

            configStarSpawnWeight = Config.Bind("Star",
                           "Spawn Weight",
                           15,
                           "How many Power Stars will appear (higher value = more spawns)");

            configStarMoonSpawns = Config.Bind("Star",
                                                "Spawn Locations",
                                                Levels.LevelTypes.Vanilla,
                                                "Choose which moons the Power Star will spawn on");

            // Coin spawn config

            configCoinSpawnWeight = Config.Bind("Coin",
               "Spawn Weight",
               45,
               "How many Coins will appear (higher value = more spawns)");

            configCoinMoonSpawns = Config.Bind("Coin",
                                                "Spawn Locations",
                                                Levels.LevelTypes.Vanilla,
                                                "Choose which moons Coins will spawn on");

            // Blue Coin spawn config

            configBlueCoinSpawnWeight = Config.Bind("Blue Coin",
               "Spawn Weight",
               30,
               "How many Blue Coins will appear (higher value = more spawns)");

            configBlueCoinMoonSpawns = Config.Bind("Blue Coin",
                                    "Spawn Locations",
                                    Levels.LevelTypes.Vanilla,
                                    "Choose which moons Blue Coins will spawn on");

            // Red Coin spawn config

            configRedCoinSpawnWeight = Config.Bind("Red Coin",
               "Spawn Weight",
               35,
               "How many Red Coins will appear (higher value = more spawns)");

            configRedCoinMoonSpawns = Config.Bind("Red Coin",
                        "Spawn Locations",
                        Levels.LevelTypes.Vanilla,
                        "Choose which moons Red Coins will spawn on");

            // QBlock spawn config

            configBlockSpawnWeight = Config.Bind("Red ? Block",
               "Spawn Weight",
               25,
               "How many Red ? Blocks will appear (higher value = more spawns)");

            configBlockMoonSpawns = Config.Bind("Red ? Block",
                                                "Spawn Locations",
                                                Levels.LevelTypes.Vanilla,
                                                "Choose which moons Red ? Blocks will spawn on");

            // BobOmb spawn config

            configBobOmbSpawnWeight = Config.Bind("Bob-Omb",
               "Spawn Weight",
               20,
               "How many Bob-Ombs will appear (higher value = more spawns)");

            configBobOmbMoonSpawns = Config.Bind("Bob-Omb",
                                                "Spawn Locations",
                                                Levels.LevelTypes.Vanilla,
                                                "Choose which moons Bob-Ombs will spawn on");

            // Cap spawn config

            configCapSpawnWeight = Config.Bind("Marios Cap",
               "Spawn Weight",
               10,
               "How many of Mario's Cap will appear (higher value = more spawns)");

            configCapMoonSpawns = Config.Bind("Marios Cap",
                                                "Spawn Locations",
                                                Levels.LevelTypes.Vanilla,
                                                "Choose which moons Mario's Cap will spawn on");

            // MIPS spawn config

            configMIPSSpawnWeight = Config.Bind("MIPS",
               "Spawn Weight",
               5,
               "How many MIPS will appear (higher value = more spawns)");

            configMIPSMoonSpawns = Config.Bind("MIPS",
                                                "Spawn Locations",
                                                Levels.LevelTypes.Vanilla,
                                                "Choose which moons MIPS will spawn on");

            // Bowser Key spawn config

            configBowserKeySpawnWeight = Config.Bind("Bowser Key",
               "Spawn Weight",
               10,
               "How many Bowser Keys will appear (higher value = more spawns)");

            configBowserKeyMoonSpawns = Config.Bind("Bowser Key",
                                                "Spawn Locations",
                                                Levels.LevelTypes.Vanilla,
                                                "Choose which moons Bowser Keys will spawn on");

            // OneUp spawn config

            configOneUpSpawnWeight = Config.Bind("1-Up",
               "Spawn Weight",
               32,
               "How many 1-Up's will appear (higher value = more spawns)");

            configOneUpMoonSpawns = Config.Bind("1-Up",
                                                "Spawn Locations",
                                                Levels.LevelTypes.Vanilla,
                                                "Choose which moons 1-Up's will spawn on");

            // Shell spawn config

            configShellSpawnWeight = Config.Bind("Koopa Shell",
               "Spawn Weight",
               38,
               "How many Koopa Shells will appear (higher value = more spawns)");

            configShellMoonSpawns = Config.Bind("Koopa Shell",
                                                "Spawn Locations",
                                                Levels.LevelTypes.Vanilla,
                                                "Choose which moons Koopa Shells will spawn on");



            instance = this;

            string assetDir = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "sm64scrap");
            AssetBundle bundle = AssetBundle.LoadFromFile(assetDir);

            // Star scrap

            Item StarItem = bundle.LoadAsset<Item>("Assets/LethalCompany/Mods/Scrap/Star/StarItem.asset");
            LethalLib.Modules.NetworkPrefabs.RegisterNetworkPrefab(StarItem.spawnPrefab);
            LethalLib.Modules.Utilities.FixMixerGroups(StarItem.spawnPrefab);
            Items.RegisterScrap(StarItem, configStarSpawnWeight.Value, configStarMoonSpawns.Value);

            // Coin scrap

            Item CoinItem = bundle.LoadAsset<Item>("Assets/LethalCompany/Mods/Scrap/Coins/CoinItem.asset");
            LethalLib.Modules.NetworkPrefabs.RegisterNetworkPrefab(CoinItem.spawnPrefab);
            LethalLib.Modules.Utilities.FixMixerGroups(CoinItem.spawnPrefab);
            Items.RegisterScrap(CoinItem, configCoinSpawnWeight.Value, configCoinMoonSpawns.Value);

            // Red Coin Scrap

            Item RedCoinItem = bundle.LoadAsset<Item>("Assets/LethalCompany/Mods/Scrap/Coins/RedCoinItem.asset");
            LethalLib.Modules.NetworkPrefabs.RegisterNetworkPrefab(RedCoinItem.spawnPrefab);
            LethalLib.Modules.Utilities.FixMixerGroups(RedCoinItem.spawnPrefab);
            Items.RegisterScrap(RedCoinItem, configRedCoinSpawnWeight.Value, configRedCoinMoonSpawns.Value);

            // Blue Coin Scrap

            Item BlueCoinItem = bundle.LoadAsset<Item>("Assets/LethalCompany/Mods/Scrap/Coins/BlueCoinItem.asset");
            LethalLib.Modules.NetworkPrefabs.RegisterNetworkPrefab(BlueCoinItem.spawnPrefab);
            LethalLib.Modules.Utilities.FixMixerGroups(BlueCoinItem.spawnPrefab);
            Items.RegisterScrap(BlueCoinItem, configBlueCoinSpawnWeight.Value, configBlueCoinMoonSpawns.Value);

            // Koopa Shell Scrap

            Item ShellItem = bundle.LoadAsset<Item>("Assets/LethalCompany/Mods/Scrap/Shell/ShellItem.asset");
            LethalLib.Modules.NetworkPrefabs.RegisterNetworkPrefab(ShellItem.spawnPrefab);
            LethalLib.Modules.Utilities.FixMixerGroups(ShellItem.spawnPrefab);
            Items.RegisterScrap(ShellItem, configShellSpawnWeight.Value, configShellMoonSpawns.Value);

            // ? Block Scrap

            Item QBlockItem = bundle.LoadAsset<Item>("Assets/LethalCompany/Mods/Scrap/Q_Block/QBlockItem.asset");
            LethalLib.Modules.NetworkPrefabs.RegisterNetworkPrefab(QBlockItem.spawnPrefab);
            LethalLib.Modules.Utilities.FixMixerGroups(QBlockItem.spawnPrefab);
            Items.RegisterScrap(QBlockItem, configBlockSpawnWeight.Value, configBlockMoonSpawns.Value);

            // Bob-Omb Scrap

            Item BobOmbItem = bundle.LoadAsset<Item>("Assets/LethalCompany/Mods/Scrap/bob-omb/BobOmbItem.asset");
            LethalLib.Modules.NetworkPrefabs.RegisterNetworkPrefab(BobOmbItem.spawnPrefab);
            LethalLib.Modules.Utilities.FixMixerGroups(BobOmbItem.spawnPrefab);
            Items.RegisterScrap(BobOmbItem, configBobOmbSpawnWeight.Value, configBobOmbMoonSpawns.Value);

            // Cap Scrap

            Item CapItem = bundle.LoadAsset<Item>("Assets/LethalCompany/Mods/Scrap/Cap/CapItem.asset");
            LethalLib.Modules.NetworkPrefabs.RegisterNetworkPrefab(CapItem.spawnPrefab);
            LethalLib.Modules.Utilities.FixMixerGroups(CapItem.spawnPrefab);
            Items.RegisterScrap(CapItem, configCapSpawnWeight.Value, configCapMoonSpawns.Value);

            // MIPS Scrap

            Item MIPSItem = bundle.LoadAsset<Item>("Assets/LethalCompany/Mods/Scrap/MIPS/MIPSItem.asset");
            LethalLib.Modules.NetworkPrefabs.RegisterNetworkPrefab(MIPSItem.spawnPrefab);
            LethalLib.Modules.Utilities.FixMixerGroups(MIPSItem.spawnPrefab);
            Items.RegisterScrap(MIPSItem, configMIPSSpawnWeight.Value, configMIPSMoonSpawns.Value);

            // 1Up Scrap

            Item OneUpItem = bundle.LoadAsset<Item>("Assets/LethalCompany/Mods/Scrap/1Up/OneUpItem.asset");
            LethalLib.Modules.NetworkPrefabs.RegisterNetworkPrefab(OneUpItem.spawnPrefab);
            LethalLib.Modules.Utilities.FixMixerGroups(OneUpItem.spawnPrefab);
            Items.RegisterScrap(OneUpItem, configOneUpSpawnWeight.Value, configOneUpMoonSpawns.Value);
            // Bowser Key Scrap

            Item BowserKeyItem = bundle.LoadAsset<Item>("Assets/LethalCompany/Mods/Scrap/BowserKey/BowserKeyItem.asset");
            LethalLib.Modules.NetworkPrefabs.RegisterNetworkPrefab(BowserKeyItem.spawnPrefab);
            LethalLib.Modules.Utilities.FixMixerGroups(BowserKeyItem.spawnPrefab);
            Items.RegisterScrap(BowserKeyItem, configBowserKeySpawnWeight.Value, configBowserKeyMoonSpawns.Value);

            Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), GUID);
            Logger.LogInfo("Loaded SM64 Scrap");
        }
    }

}