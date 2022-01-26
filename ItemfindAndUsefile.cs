using System;
using LocationsDetailsNamespace;
using RPG;
namespace ItemfindAnduse
{
    class ItemfindAndUse
    {
        public static void ItemUser(PlayerNamespace.PlayerInformation.ItemInfoForInventory ChoosenItem, int ItemSpotInInvent)
        {
            string[] Products = GameplayLoop.ItemEnemyShopOwnerInfo[0].Split("\n");

            string[] item = Products[ChoosenItem.ItemID].Split(',');
            /*
            0 = nothing
            1 =skip enemy
            2 = skip path
            3 = damage
            4 = heal
            */
            int counter = 0;
            foreach (string ItemUses in item)
            {
                if (counter != 0)
                {
                    if (ItemUses == "TRUE")
                    {
                        switch (counter)
                        {
                            case 1:
                                {
                                    GameplayLoop.SkipThisFight = true;
                                    break;
                                }
                            case 2:
                                {
                                    new FreeMove();
                                    break;
                                }
                            case 3:
                                {
                                    GameplayLoop.UserCharater.WeaponInUse = ChoosenItem;
                                    break;
                                }
                            case 4:
                                {
                                    if (GameplayLoop.UserCharater.PlayerStats[4].StatAmount != GameplayLoop.UserCharater.PlayerStats[4].StatLevelingUpBias * GameplayLoop.UserCharater.ThePlayerLevel)
                                    {
                                        GameplayLoop.UserCharater.PlayerStats[4].StatAmount += Convert.ToInt32(item[6]);
                                        if (GameplayLoop.UserCharater.PlayerStats[4].StatLevelingUpBias * GameplayLoop.UserCharater.ThePlayerLevel < GameplayLoop.UserCharater.PlayerStats[4].StatAmount)
                                        {
                                            GameplayLoop.UserCharater.PlayerStats[4].StatAmount = Convert.ToInt32(GameplayLoop.UserCharater.PlayerStats[4].StatLevelingUpBias * GameplayLoop.UserCharater.ThePlayerLevel);

                                        }
                                        GameplayLoop.UserCharater.PlayerInventory.RemoveAt(ItemSpotInInvent);
                                    }

                                    break;
                                }


                        }
                    }
                }
                counter++;
            }

        }










    }
}