using System;
using LocationsDetailsNamespace;
using PlayerNamespace;

namespace RPG
{
    class MainProgram
    {


        public static int CheckForStringInInt(string InputNumber)
        {
            int ReturnValue = 0;
            try
            {

                ReturnValue = Convert.ToInt32(InputNumber);
            }
            catch
            {

                Console.WriteLine("Please Put a Number");
                ReturnValue = CheckForStringInInt(Console.ReadLine());
            }
            return ReturnValue;
        }




        public static float CheckForStringInFloat(string InputNumber)
        {
            float ReturnValue = 0;
            try
            {

                ReturnValue = Convert.ToSingle(InputNumber);
            }
            catch (System.FormatException)
            {

                Console.WriteLine("Please Put a Number");
                ReturnValue = CheckForStringInFloat(Console.ReadLine());
            }
            return ReturnValue;
        }



        static void Main()
        {
            new GameplayLoop(System.IO.File.ReadAllText("data.csv").Split("Section")); //this will create Gameplay and send the data in  data.csv
        }
    }
    class GameplayLoop
    {
        public static PlayerInformation.PlayCharacter UserCharater = null;
        public static bool IsCharaterMade = false;
        public bool StopGameplayLoopBool = false;
        private static int[] PathingValues;
        public static bool SkipThisFight = false;
        public static Random RNG = new Random(); //this will be used for anything that needs a random number
        public static string[] ItemEnemyShopOwnerInfo = null;

        public static int PlayerOnRound = 1;


        public GameplayLoop(string[] FileTextinformation)
        {
            ItemEnemyShopOwnerInfo = FileTextinformation;
            PlayerOnRound = 1;

            while (StopGameplayLoopBool == false)
            {

                if (IsCharaterMade == false)
                {

                    UserCharater = PlayerInformation.SetUpCharacter();

                }
                Console.Clear();

                UserCharater.UsingExpToLevelUp();
                PathingValues = new int[] { RNG.Next(0, 4), RNG.Next(0, 4), RNG.Next(0, 4) };
                Console.WriteLine("level " + UserCharater.ThePlayerLevel + "\nCoins: " + UserCharater.Coins + "\nExp: " + UserCharater.PlayerExpAmount);
                Console.WriteLine("Exp Needed: " + UserCharater.NeedAmountOfExpToLevelUp);
                Console.WriteLine("Press P to See your Stats And Press H for Help");
                LocationPlacementDisplayer(PathingValues);
                PlayerChoosenLocation(UserInputtedActionValue(), PathingValues);

                PlayerOnRound++;
            }
        }


        public static int UserInputtedActionValue(int InputCatergory = 0)
        {
            string UserInputtedKeyValue = null;

            UserInputtedKeyValue = Console.ReadKey().Key.ToString();
            //GameplayMovement this will be used for choosing path
            if (InputCatergory == 0)
            {
                switch (UserInputtedKeyValue)
                {
                    case "W":
                        {
                            return 1;
                        }
                    case "UpArrow":
                        {
                            return 1;
                        }
                    case "D":
                        {
                            return 2;
                        }
                    case "RightArrow":
                        {
                            return 2;
                        }
                    case "A":
                        {
                            return 0;
                        }
                    case "LeftArrow":
                        {
                            return 0;
                        }
                    case "Escape":
                        {
                            Environment.Exit(0);
                            break;
                        }
                    case "P":
                        {


                            GameplayLoop.UserCharater.WhatAreMyStats();
                            Console.Clear();
                            Console.WriteLine("level " + UserCharater.ThePlayerLevel + "\n Coins: " + UserCharater.Coins + "\n Exp: " + UserCharater.PlayerExpAmount);
                            Console.WriteLine("Exp Needed: " + UserCharater.NeedAmountOfExpToLevelUp);
                            Console.WriteLine("Press P to See your Stats And Press H for Help");
                            GameplayLoop.LocationPlacementDisplayer(PathingValues);
                            return UserInputtedActionValue(InputCatergory);
                        }
                    case "I":
                        {

                            UserCharater.AccessingInvetory();
                            Console.Clear();
                            Console.WriteLine("level " + UserCharater.ThePlayerLevel + "\n Coins: " + UserCharater.Coins + "\n Exp: " + UserCharater.PlayerExpAmount);
                            Console.WriteLine("Exp Needed: " + UserCharater.NeedAmountOfExpToLevelUp);
                            Console.WriteLine("Press P to See your Stats And Press H for Help");
                            GameplayLoop.LocationPlacementDisplayer(PathingValues);
                            return UserInputtedActionValue(InputCatergory);
                        }
                    case "H":
                        {

                            Console.WriteLine("You move with arrows or AWD the direction you will move matchs the placement of the key \n Press P to see Stats \n press I to see what is in your Inventory\n Escape key will close the Game");
                            Console.ReadKey();
                            Console.Clear();
                            Console.WriteLine("level " + UserCharater.ThePlayerLevel + "\n Coins: " + UserCharater.Coins + "\n Exp: " + UserCharater.PlayerExpAmount);
                            Console.WriteLine("Exp Needed: " + UserCharater.NeedAmountOfExpToLevelUp);
                            Console.WriteLine("Press P to See your Stats And Press H for Help");
                            GameplayLoop.LocationPlacementDisplayer(PathingValues);
                            return UserInputtedActionValue(InputCatergory);

                        }


                }
            }
            //When player is fight an enemy
            if (InputCatergory == 1)
            {
                switch (UserInputtedKeyValue)
                {
                    case "G":
                        return 1;

                    case "H":
                        {
                            return 2;
                        }
                    case "l":
                        {

                            return 0;
                        }

                    case "I":
                        {
                            Console.Clear();
                            UserCharater.AccessingInvetory();
                            return UserInputtedActionValue(InputCatergory);
                        }
                }
            }
            //If there is a yes or No responds needed
            if (InputCatergory == 2)
            {
                switch (UserInputtedKeyValue)
                {
                    case "Y":
                        return 1;
                    case "N":
                        return 0;
                }
            }

            Console.WriteLine("///////////////////////////////// \n Put in a different character");
            return UserInputtedActionValue(InputCatergory);

        }



        public void PlayerChoosenLocation(int PlayerChoiceValue, int[] RNGLocationNumber)
        {

            switch (RNGLocationNumber[PlayerChoiceValue])
            {
                case 0:
                    new FreeMove();
                    break;

                case 1:
                    new Store();
                    break;

                case 2:
                    new Medical();
                    break;

                case 3:
                    BattlingWithEnemy();
                    break;

            }

        }

        public void BattlingWithEnemy()
        {
            Enemy TheEnemy = new Enemy();
            Console.Clear();
            Console.WriteLine("You are in a fight");

            while (true)
            {
                int ChanceToEscape = RNG.Next(0, 1);
                Console.WriteLine("To do a Light Hit use G if you would like to do a Heavy Hit use H ");
                Console.WriteLine("Level: " + TheEnemy.level + "\n" + TheEnemy.EnemyInfo[0][0] + "\n" + "Health: " + TheEnemy.Hp + "\n" + "Attack: " + TheEnemy.Attack);

                Console.WriteLine("\n" + "You are level " + UserCharater.ThePlayerLevel);
                foreach (var temp in UserCharater.PlayerStats)
                {
                    Console.WriteLine(temp.StatName + ": " + temp.StatAmount);
                }
                if (ChanceToEscape == 1)
                {
                    Console.WriteLine("You have a chance to escape press L to runn.");
                }
                bool PlayerMissAttack = false;
                int UserInputForAttackingAndEscape = UserInputtedActionValue(1);
                Console.Clear();
                int HeldItemDamge = 0;
                int hitchancevalve = RNG.Next(0, 100);
                int Enemyhitchancevalve = RNG.Next(0, 100);
                int CritHitOrNot = RNG.Next(0, 1);
                float CritHitDamage = 1;
                if (ChanceToEscape == 1 && UserInputForAttackingAndEscape == 0)
                {
                    Console.WriteLine("You have escaped \n PRESS ANY KEY TO CONTINUE");
                    Console.ReadKey();
                    break;
                }
                if (ChanceToEscape == 0 && UserInputForAttackingAndEscape == 0)
                {

                    Console.WriteLine("You try to run but couldn't");
                    break;
                }


                try
                {
                    HeldItemDamge = Convert.ToInt32(ItemEnemyShopOwnerInfo[0].Split("\n")[UserCharater.WeaponInUse.ItemID].Split(",")[4]);
                }
                catch
                { }
                if (SkipThisFight == true)
                {
                    SkipThisFight = false;
                    break;
                }


                if (CritHitOrNot == 1)
                {
                    CritHitDamage = 1.5f;
                }
                if (UserInputForAttackingAndEscape == 1)
                {
                    if (hitchancevalve >= 80)
                    {
                        PlayerMissAttack = true;
                        Console.WriteLine("YOU MISS");
                    }

                }
                if (UserInputForAttackingAndEscape == 2)
                {
                    if (hitchancevalve >= 45)
                    {
                        PlayerMissAttack = true;
                        Console.WriteLine("YOU MISS");

                    }
                }

                if (PlayerMissAttack == false)
                {
                    TheEnemy.Hp -= Convert.ToInt32(((UserCharater.PlayerStats[0].StatAmount * UserInputForAttackingAndEscape) + HeldItemDamge) * CritHitDamage);
                }
                if (TheEnemy.Hp <= 0)
                {
                    UserCharater.PlayerExpAmount += 3 * TheEnemy.level / ~UserCharater.ThePlayerLevel;
                    Console.WriteLine("Congratulation " + UserCharater.PlayerName + "\n You have beat a " + TheEnemy.EnemyInfo[0][0]);
                    Console.WriteLine("Press any key to continue");
                    Console.ReadKey();
                    break;
                }
                if (Enemyhitchancevalve >= 50)
                {
                    GameplayLoop.UserCharater.PlayerStats[3].StatAmount -= (TheEnemy.Attack - UserCharater.PlayerStats[1].StatAmount);
                }

                if (UserCharater.PlayerStats[3].StatAmount <= 0)
                {
                    Console.WriteLine("You have died ");
                    IsCharaterMade = false;
                    break;
                }
            }
        }
        public static void LocationPlacementDisplayer(int[] PathingValues)
        {

            foreach (int Path in PathingValues)
            {
                Console.Write("| ");
                switch (Path)
                {
                    case 0:
                        Console.Write("Free Space");
                        break;

                    case 1:
                        Console.Write("Merchant");

                        break;

                    case 2:
                        Console.Write("Medical");
                        break;

                    case 3:
                        Console.Write("Enemy");
                        break;
                }
                Console.Write(" |");

            }
        }



    }






}


