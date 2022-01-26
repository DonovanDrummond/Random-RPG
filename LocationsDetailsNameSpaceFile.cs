using System;
using RPG;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace LocationsDetailsNamespace
{

    public class OwnerNameAndEnemyPicker
    {

        public static string StoreOwnerNamePicker()
        {

            string[] Names = GameplayLoop.ItemEnemyShopOwnerInfo[2].Split("\n");
            string Name = Names[GameplayLoop.RNG.Next(1, Names.Length)];
            return Name.Split(",")[0];

        }
        public static List<dynamic> EnemyPicker()
        {
            string[] Enemys = GameplayLoop.ItemEnemyShopOwnerInfo[1].Split("\n");
            List<dynamic> info = new List<dynamic> { Enemys[GameplayLoop.RNG.Next(1, Enemys.Length)].Split(",") };
            return info;
        }


    }


    public struct ProductItems
    {
        public int ProductItemID;//this is for the item data in the CSV file
        public int StockItemID;
        public string ProductName;
        public int ProductPrice;
        public int ProductAmount;
    }

    public class Store
    {

        public string StoreOwnerName;
        public ProductItems[] StoreStock = new ProductItems[GameplayLoop.RNG.Next(1, 10)];
        public void StockingItems()
        {
            int counter = 0;
            foreach (var item in this.StoreStock)
            {
                int ItemIDNumber = GameplayLoop.RNG.Next(1, GameplayLoop.ItemEnemyShopOwnerInfo[0].Split("\n").Length);
                String[] Products = GameplayLoop.ItemEnemyShopOwnerInfo[0].Split("\n");
                this.StoreStock[counter].ProductItemID = ItemIDNumber;
                this.StoreStock[counter].StockItemID = counter;
                this.StoreStock[counter].ProductAmount = GameplayLoop.RNG.Next(1, 100);
                this.StoreStock[counter].ProductName = Products[ItemIDNumber].Split(",")[0];
                this.StoreStock[counter].ProductPrice = GameplayLoop.RNG.Next(1, 100*GameplayLoop.PlayerOnRound );
                counter++;
            }
        }

        public void UserMakingPurchase(ProductItems ItemBeingPurchase)
        {
            Console.WriteLine("The total amount of this product is:" + ItemBeingPurchase.ProductAmount);
            Console.WriteLine("Put in the amount that you are going to purchase");
            Console.WriteLine("If you do not want to buy anything just put 0");
            int AmountWantedToPurchase = MainProgram.CheckForStringInInt(Console.ReadLine());
            if (AmountWantedToPurchase != 0)
            {
                if (AmountWantedToPurchase * ItemBeingPurchase.ProductPrice > GameplayLoop.UserCharater.Coins)
                {
                    Console.Clear();

                    Console.WriteLine("You do not have enough coins to purchase that much");
                    UserMakingPurchase(ItemBeingPurchase);
                }
                if (AmountWantedToPurchase > ItemBeingPurchase.ProductAmount)
                {
                    Console.Clear();

                    Console.WriteLine("We do not have that much of that product");
                    UserMakingPurchase(ItemBeingPurchase);
                }
                else
                {
                    GameplayLoop.UserCharater.PlayerInventory.Add(new PlayerNamespace.PlayerInformation.ItemInfoForInventory
                    {
                        ItemName = ItemBeingPurchase.ProductName,
                        ItemID = ItemBeingPurchase.ProductItemID,
                        AmountOfItemsHolding = AmountWantedToPurchase,
                    });
                    GameplayLoop.UserCharater.Coins -= AmountWantedToPurchase * ItemBeingPurchase.ProductPrice;
                    StoreStock[ItemBeingPurchase.StockItemID].ProductAmount -= AmountWantedToPurchase;
                }

            }
            Console.Clear();

        }
        public Store()
        {
            StoreOwnerName = OwnerNameAndEnemyPicker.StoreOwnerNamePicker() + "'s";
            StockingItems();
            Console.Clear();

            while (true)
            {

                int ItemsNumber = 0;
                Console.WriteLine("You currently have " + GameplayLoop.UserCharater.Coins);
                Console.WriteLine("Hello This is " + StoreOwnerName + " Shop." + "\n Here is our selection of items");
                foreach (var item in this.StoreStock)
                {
                    if (item.ProductAmount <= 0)
                    {
                        Console.WriteLine("OUT OF STOCK");
                    }
                    else
                    {
                        Console.WriteLine(ItemsNumber + ") " + "Product Name:" + item.ProductName + " Quantity: " + item.ProductAmount + " Price: " + item.ProductPrice);//This will display all the items

                    }
                    ItemsNumber++;
                }
                Console.WriteLine("Press a number to select what item you want to buy or press L to leave");
                string CharaterInput = Console.ReadKey().Key.ToString();

                if (Regex.IsMatch(CharaterInput, "([l])", RegexOptions.IgnoreCase))
                {
                    break;
                }
                if (Regex.IsMatch(CharaterInput, "([0-" + (StoreStock.Length - 1) + "])"))
                {
                    Console.Clear();

                    UserMakingPurchase(StoreStock[Convert.ToInt32(CharaterInput)]);
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("Put in a number that matches the item or L to leave");
                }
            }
        }


    }
    public class Medical
    {
        public string DoctorName;
        public int PriceToHeal = GameplayLoop.RNG.Next(0, GameplayLoop.PlayerOnRound*2);
        public Medical()
        {
            int WillOrWillNotHeal = GameplayLoop.RNG.Next(0, 2);

            DoctorName = OwnerNameAndEnemyPicker.StoreOwnerNamePicker();
            Console.WriteLine("Hello This is " + DoctorName + " Clinic");

            Console.Clear();
            Console.WriteLine("The Fee to heal is " + PriceToHeal);
            Console.WriteLine("Would you like to heal");
            Console.WriteLine("Y/N");
            int DoctorChoiceToHeal = GameplayLoop.UserInputtedActionValue(2);

            if (DoctorChoiceToHeal == 1 && GameplayLoop.UserCharater.Coins >= PriceToHeal)
            {
                GameplayLoop.UserCharater.Coins -= PriceToHeal;
                if (WillOrWillNotHeal == 0)
                {
                    GameplayLoop.UserCharater.PlayerStats[4].StatAmount = Convert.ToInt32(GameplayLoop.UserCharater.PlayerStats[4].StatLevelingUpBias * GameplayLoop.UserCharater.ThePlayerLevel);
                    Console.WriteLine("You have been fully healed");
                }
                if (WillOrWillNotHeal == 2)
                {
                    Console.WriteLine("whoops I'm sure you will be fine");
                    int SomeHealOrHurting = GameplayLoop.RNG.Next(0, 1);
                    if (SomeHealOrHurting == 0)
                    {
                        if (GameplayLoop.UserCharater.PlayerStats[4].StatAmount > 1)
                        {
                            GameplayLoop.UserCharater.PlayerStats[4].StatAmount -= GameplayLoop.RNG.Next(1, GameplayLoop.UserCharater.PlayerStats[4].StatAmount - 1);
                        }
                    }
                    else
                    {
                        GameplayLoop.UserCharater.PlayerStats[4].StatAmount = GameplayLoop.RNG.Next(GameplayLoop.UserCharater.PlayerStats[4].StatAmount, Convert.ToInt32(GameplayLoop.UserCharater.PlayerStats[4].StatLevelingUpBias * GameplayLoop.UserCharater.ThePlayerLevel - 1));
                    }
                }
                else
                {
                    Console.WriteLine("Yeah sure you're better now");
                }
            }
            if (DoctorChoiceToHeal == 1 && GameplayLoop.UserCharater.Coins <= PriceToHeal)
            {
                Console.WriteLine("You do not have enough coin");

            }
            Console.ReadKey();

        }
    }
    public class Enemy
    {
        public List<dynamic> EnemyInfo = new List<dynamic>();
        public int level = 0;
        public int Attack = 0;
        public int Hp = 0;

        public Enemy()
        {
            EnemyInfo = OwnerNameAndEnemyPicker.EnemyPicker();
            int NormalEnemy = GameplayLoop.RNG.Next(0, 1);

            level = EnemylevelCaculater(Convert.ToInt32(EnemyInfo[0][1]), NormalEnemy);
            Attack = level * Convert.ToInt32(EnemyInfo[0][3]);
            Hp = level * Convert.ToInt32(EnemyInfo[0][2]);
        }
        public int EnemylevelCaculater(int MinEnemyLevelLimit, int NormalEnemy)
        {
            int LevelReturned = 1;
            if (NormalEnemy == 0)
            {
                LevelReturned = GameplayLoop.RNG.Next(MinEnemyLevelLimit, GameplayLoop.PlayerOnRound * 9999999);
            }
            if (MinEnemyLevelLimit < GameplayLoop.PlayerOnRound)
            {
                LevelReturned = GameplayLoop.RNG.Next(MinEnemyLevelLimit, GameplayLoop.PlayerOnRound);
            }
            else
            {
                LevelReturned = GameplayLoop.RNG.Next(GameplayLoop.PlayerOnRound, MinEnemyLevelLimit);

            }
            return LevelReturned;
        }


    }
    public class FreeMove
    {
        public FreeMove()
        {
            Console.Clear();
            int TheRoll = GameplayLoop.RNG.Next(0, GameplayLoop.PlayerOnRound*2);
            GameplayLoop.UserCharater.PlayerExpAmount += TheRoll;
            Console.WriteLine("You have received " + TheRoll + "EXP");
            TheRoll = GameplayLoop.RNG.Next(0, GameplayLoop.PlayerOnRound*2);
            GameplayLoop.UserCharater.Coins += TheRoll;
            Console.WriteLine("You have found " + TheRoll + "coins");
            Console.WriteLine("PRESS ANY KEY TO CONTINUE");
            Console.ReadKey();

        }
    }
}