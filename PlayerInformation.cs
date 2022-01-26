using System;
using RPG;
using ItemfindAnduse;
using System.Text.RegularExpressions;
using System.Collections.Generic;
namespace PlayerNamespace
{
    static class PlayerInformation
    {
        public static PlayCharacter SetUpCharacter()
        {
            Console.WriteLine("Creating New Character");
            GameplayLoop.IsCharaterMade = true;
            return new PlayCharacter();

        }
        public struct StatCatergorySections
        {
            public string StatName;
            public int StatAmount;
            public int StatLevelingUpBias;
        }
        public struct ItemInfoForInventory
        {
            public int ItemID;
            public int AmountOfItemsHolding;
            public string ItemName;
        }
        public class PlayCharacter
        {
            private int AmountOfLevelingBias = 10;

            public ItemInfoForInventory WeaponInUse = new ItemInfoForInventory();
            public StatCatergorySections[] PlayerStats = new StatCatergorySections[4];
            private readonly string[] StatName = { "Strength", "Defense", "Speed", "Health" };
            public int ThePlayerLevel = 1;
            public List<ItemInfoForInventory> PlayerInventory = new List<ItemInfoForInventory>();
            public string PlayerName;
            public int PlayerExpAmount = 0;
            public int NeedAmountOfExpToLevelUp = 1;
            public long Coins = 0;
            public void AccessingInvetory()
            {
                Console.Clear();
                string InventoryPage = Convert.ToString(PlayerInventory.Count / 9);
                int AmountofPages = InventoryPage[0];
                int CurrentPage = 0;
                Console.Clear();
                while (true)
                {
                    Console.WriteLine("Inventory");
                    Console.WriteLine("Use the arrow keys to change pages or put in a number to select a item");
                    for (int i = 0; i < 10; i++)
                    {
                        try
                        {
                            Console.WriteLine(i + ") " + PlayerInventory[i + (CurrentPage * 9)].ItemName + " Quantity " + PlayerInventory[i + (CurrentPage * 9)].AmountOfItemsHolding);
                        }
                        catch (System.Exception)
                        {
                            Console.WriteLine("Empty");

                        }
                    }
                    string KeyInputted = Console.ReadKey().Key.ToString();
                    if (Regex.IsMatch(KeyInputted, "(/w|^)(leftarrow)(/w|$)", RegexOptions.IgnoreCase))
                    {
                        if (CurrentPage > -1)
                        {
                            CurrentPage--;
                            Console.Clear();
                        }
                    }
                    if (Regex.IsMatch(KeyInputted, "(/w|^)(rightarrow)(/w|$)", RegexOptions.IgnoreCase))
                    {
                        {
                            if (CurrentPage <= AmountofPages)
                            {
                                CurrentPage++;
                                Console.Clear();

                            }
                        }
                    }
                    if (KeyInputted == "l" || KeyInputted == "L")
                    {
                        break;
                    }
                    if (Regex.IsMatch(KeyInputted, "[0-9]"))
                    {
                        Console.WriteLine("Are you sure that you want to use \n Y/N");
                        int YesOrNo = GameplayLoop.UserInputtedActionValue(2);
                        if (YesOrNo == 1)
                        {
                            int temp = Convert.ToInt32(KeyInputted);
                            ItemfindAnduse.ItemfindAndUse.ItemUser(PlayerInventory[temp], temp * CurrentPage);
                            break;
                        }
                        Console.Clear();
                    }



                }

            }
            public void WhatAreMyStats()
            {
                Console.Clear();
                Console.WriteLine("Stat Check");

                foreach (var StatType in GameplayLoop.UserCharater.PlayerStats)
                {
                    Console.WriteLine(StatType.StatName + " : have " + StatType.StatAmount + " points");
                }
                Console.WriteLine("Press any key to continue");
                Console.ReadKey();
            }
            public void UsingExpToLevelUp()
            {
                while (PlayerExpAmount >= NeedAmountOfExpToLevelUp)
                {
                    PlayerExpAmount -= NeedAmountOfExpToLevelUp;

                    IncreaseLevelOfStat();

                    NeedAmountOfExpToLevelUp += NeedAmountOfExpToLevelUp ;
                    if (PlayerExpAmount < NeedAmountOfExpToLevelUp)
                    {
                        Console.WriteLine("Level Up");
                        break;
                    }



                }

            }
            public static void IncreaseLevelOfStat()
            {
                RPG.GameplayLoop.UserCharater.ThePlayerLevel += 1;
                int counter = 0;
                foreach (var StatType in GameplayLoop.UserCharater.PlayerStats)
                {
                    GameplayLoop.UserCharater.PlayerStats[counter].StatAmount = StatType.StatLevelingUpBias + StatType.StatAmount + 1;
                    counter++;
                }
            }
            private int InputLevelingBias()
            {
                try
                {
                    int AmountOfBias = MainProgram.CheckForStringInInt(Console.ReadLine());
                    if (AmountOfBias > this.AmountOfLevelingBias)
                    {
                        Console.Clear();
                        Console.WriteLine("Put a Value under the amount left");
                        Console.WriteLine("you have " + this.AmountOfLevelingBias + "to use");
                        InputLevelingBias();
                    }
                    return AmountOfBias;
                }
                catch
                {
                    return InputLevelingBias();
                }

            }
            public PlayCharacter()
            {
                Console.WriteLine("What is your Name");
                PlayerName = Console.ReadLine();
                Console.Clear();
                for (int counter = 0; counter < PlayerStats.Length; counter++)
                {

                    PlayerStats[counter].StatName = StatName[counter];
                    PlayerStats[counter].StatAmount = 1;
                    if (AmountOfLevelingBias >= 0)
                    {

                        int TheBias = 1;
                        PlayerStats[counter].StatLevelingUpBias = 1;
                        if (AmountOfLevelingBias <= 1)
                        {
                            Console.WriteLine("Stats: " + PlayerStats[counter].StatName + " Has been set to 1");
                        }
                        if (AmountOfLevelingBias > 1)
                        {
                            Console.WriteLine("you have " + AmountOfLevelingBias + " point to use");
                            Console.WriteLine(PlayerStats[counter].StatName);
                            Console.WriteLine("How much leveling bias do you want to add to  " + PlayerStats[counter].StatName);
                            TheBias = InputLevelingBias();

                            AmountOfLevelingBias -= TheBias;
                            PlayerStats[counter].StatLevelingUpBias = TheBias;
                            Console.WriteLine("Stats: " + PlayerStats[counter].StatName + " Has been set to " + PlayerStats[counter].StatLevelingUpBias);

                        }
                    }


                    Console.Clear();

                }
                Console.WriteLine("Press any key to continue");
                Console.ReadKey();
            }


        }


    }
}