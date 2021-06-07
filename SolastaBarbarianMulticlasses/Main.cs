using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using UnityModManagerNet;
using HarmonyLib;
using I2.Loc;
using SolastaModApi;
using SolastaMulticlassClassBuilder;
using System.Linq;
using System.Collections.Generic;

namespace SolastaBarbarianMulticlasses
{
    public class Main
    {
        [Conditional("DEBUG")]
        internal static void Log(string msg) => Logger.Log(msg);
        internal static void Error(Exception ex) => Logger?.Error(ex.ToString());
        internal static void Error(string msg) => Logger?.Error(msg);
        internal static UnityModManager.ModEntry.ModLogger Logger { get; private set; }

        internal static void LoadTranslations()
        {
            DirectoryInfo directoryInfo = new DirectoryInfo($@"{UnityModManager.modsPath}/SolastaBarbarianMulticlasses");
            FileInfo[] files = directoryInfo.GetFiles($"Translations-??.txt");

            foreach (var file in files)
            {
                var filename = $@"{UnityModManager.modsPath}/SolastaBarbarianMulticlasses/{file.Name}";
                var code = file.Name.Substring(13, 2);
                var languageSourceData = LocalizationManager.Sources[0];
                var languageIndex = languageSourceData.GetLanguageIndexFromCode(code);

                if (languageIndex < 0)
                    Main.Error($"language {code} not currently loaded.");
                else
                    using (var sr = new StreamReader(filename))
                    {
                        String line;
                        while ((line = sr.ReadLine()) != null)
                        {
                            var splitted = line.Split(new[] { '\t', ' ' }, 2);
                            var term = splitted[0];
                            var text = splitted[1];
                            languageSourceData.AddTerm(term).Languages[languageIndex] = text;
                        }
                    }
            }
        }

        internal static bool Load(UnityModManager.ModEntry modEntry)
        {
            try
            {
                Logger = modEntry.Logger;

                LoadTranslations();

                var harmony = new Harmony(modEntry.Info.Id);
                harmony.PatchAll(Assembly.GetExecutingAssembly());
            }
            catch (Exception ex)
            {
                Error(ex);
                throw;
            }

            return true;
        }

        internal static void ModEntryPoint()
         {
            CharacterClassDefinition barbarianClass = DatabaseRepository.GetDatabase<CharacterClassDefinition>().GetAllElements()?.FirstOrDefault(c => string.Equals(c.Name, "AHBarbarianClass")); //String matching yay! :( oh well.
            CharacterSubclassDefinition bearSubclass = DatabaseRepository.GetDatabase<CharacterSubclassDefinition>().GetAllElements()?.FirstOrDefault(c => string.Equals(c.Name, "AHBarbarianSubclassPathOfTheBear"));
            CharacterSubclassDefinition frenzySubclass = DatabaseRepository.GetDatabase<CharacterSubclassDefinition>().GetAllElements()?.FirstOrDefault(c => string.Equals(c.Name, "AHBarbarianSubclassPathOfFrenzy"));
            CharacterSubclassDefinition reaverSubclass = DatabaseRepository.GetDatabase<CharacterSubclassDefinition>().GetAllElements()?.FirstOrDefault(c => string.Equals(c.Name, "AHBarbarianSubclassPathOfTheReaver"));

            if (barbarianClass != null && bearSubclass != null && frenzySubclass != null && reaverSubclass != null)
            {
                //Barb5/Rogue5
                MultiClassBuilder.BuildAndAddNewMultiClassToDB(barbarianClass, bearSubclass, new List<Tuple<CharacterClassDefinition, CharacterSubclassDefinition, int>>()
                {
                    new Tuple<CharacterClassDefinition, CharacterSubclassDefinition, int>(barbarianClass, bearSubclass, 4),
                    new Tuple<CharacterClassDefinition, CharacterSubclassDefinition, int>(DatabaseHelper.CharacterClassDefinitions.Rogue, DatabaseHelper.CharacterSubclassDefinitions.RoguishThief, 5),
                },
                "BearBarb5/ThiefRogue5");

                //Barb5/Rogue5
                MultiClassBuilder.BuildAndAddNewMultiClassToDB(barbarianClass, frenzySubclass, new List<Tuple<CharacterClassDefinition, CharacterSubclassDefinition, int>>()
                {
                    new Tuple<CharacterClassDefinition, CharacterSubclassDefinition, int>(barbarianClass, frenzySubclass, 4),
                    new Tuple<CharacterClassDefinition, CharacterSubclassDefinition, int>(DatabaseHelper.CharacterClassDefinitions.Rogue, DatabaseHelper.CharacterSubclassDefinitions.RoguishThief, 5),
                },
                "FrenzyBarb5/ThiefRogue5");

                //Barb5/Rogue5
                MultiClassBuilder.BuildAndAddNewMultiClassToDB(barbarianClass, reaverSubclass, new List<Tuple<CharacterClassDefinition, CharacterSubclassDefinition, int>>()
                {
                    new Tuple<CharacterClassDefinition, CharacterSubclassDefinition, int>(barbarianClass, reaverSubclass, 4),
                    new Tuple<CharacterClassDefinition, CharacterSubclassDefinition, int>(DatabaseHelper.CharacterClassDefinitions.Rogue, DatabaseHelper.CharacterSubclassDefinitions.RoguishThief, 5),
                },
                "ReaverBarb5/ThiefRogue5");
                
                //Barb7/Rogue3
                MultiClassBuilder.BuildAndAddNewMultiClassToDB(barbarianClass, reaverSubclass, new List<Tuple<CharacterClassDefinition, CharacterSubclassDefinition, int>>()
                {
                    new Tuple<CharacterClassDefinition, CharacterSubclassDefinition, int>(barbarianClass, reaverSubclass, 6),
                    new Tuple<CharacterClassDefinition, CharacterSubclassDefinition, int>(DatabaseHelper.CharacterClassDefinitions.Rogue, DatabaseHelper.CharacterSubclassDefinitions.RoguishThief, 3),
                },
                "ReaverBarb7/ThiefRogue3");

                //Barb6/Fighter4
                MultiClassBuilder.BuildAndAddNewMultiClassToDB(barbarianClass, bearSubclass, new List<Tuple<CharacterClassDefinition, CharacterSubclassDefinition, int>>()
                {
                    new Tuple<CharacterClassDefinition, CharacterSubclassDefinition, int>(barbarianClass, bearSubclass, 5),
                    new Tuple<CharacterClassDefinition, CharacterSubclassDefinition, int>(DatabaseHelper.CharacterClassDefinitions.Fighter, DatabaseHelper.CharacterSubclassDefinitions.MartialMountaineer, 4),
                },
                "BearBarb6/MountaineerFighter4");

                //Barb6/Fighter4
                MultiClassBuilder.BuildAndAddNewMultiClassToDB(barbarianClass, frenzySubclass, new List<Tuple<CharacterClassDefinition, CharacterSubclassDefinition, int>>()
                {
                    new Tuple<CharacterClassDefinition, CharacterSubclassDefinition, int>(barbarianClass, frenzySubclass, 5),
                    new Tuple<CharacterClassDefinition, CharacterSubclassDefinition, int>(DatabaseHelper.CharacterClassDefinitions.Fighter, DatabaseHelper.CharacterSubclassDefinitions.MartialMountaineer, 4),
                },
                "FrenzyBarb6/MountaineerFighter4");

                //Barb6/Fighter4
                MultiClassBuilder.BuildAndAddNewMultiClassToDB(barbarianClass, reaverSubclass, new List<Tuple<CharacterClassDefinition, CharacterSubclassDefinition, int>>()
                {
                    new Tuple<CharacterClassDefinition, CharacterSubclassDefinition, int>(barbarianClass, reaverSubclass, 5),
                    new Tuple<CharacterClassDefinition, CharacterSubclassDefinition, int>(DatabaseHelper.CharacterClassDefinitions.Fighter, DatabaseHelper.CharacterSubclassDefinitions.MartialMountaineer, 4),
                },
                "ReaverBarb6/MountaineerFighter4");

                //Fighter TacticianSubclass
                CharacterSubclassDefinition tacticianSubclass = DatabaseRepository.GetDatabase<CharacterSubclassDefinition>().GetAllElements().FirstOrDefault(c => string.Equals(c.Name, "GambitResourcePool")); //Good old terrible name that was a copy paste error.
                if(tacticianSubclass != null)
                {
                    //Barb6/Fighter4
                    MultiClassBuilder.BuildAndAddNewMultiClassToDB(barbarianClass, reaverSubclass, new List<Tuple<CharacterClassDefinition, CharacterSubclassDefinition, int>>()
                    {
                        new Tuple<CharacterClassDefinition, CharacterSubclassDefinition, int>(barbarianClass, reaverSubclass, 5),
                        new Tuple<CharacterClassDefinition, CharacterSubclassDefinition, int>(DatabaseHelper.CharacterClassDefinitions.Fighter, tacticianSubclass, 4),
                    },
                    "ReaverBarb6/TacticianFighter4");
                }
            }
        }
    }
}
