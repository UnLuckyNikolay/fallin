using Fallin.InventorySystem.Items;
using Fallin.Enums;

namespace Fallin.Characters.Enemies
{
    public class SmallRat : Enemy
    {
        public SmallRat(GameStateManager GSM) : 
        base(GSM, 
        new CharacterProperties{
            Level = 1,
            Name = "Small Rat",
            NameMap = "rt",
            NameColor = "gray",

            Strength = 2,
            Perception = 1,
            Endurance = 1,
            Charisma = 0,
            Intelligence = 3,
            Agility = 5,
            Luck = 4,

            HealthMultiplier = 0.5f,
            AttackMultiplier = 1
        }, 
        new EnemyProperties{
            SpriteAlive = 
                        """     _______     """ + "\n" +
                        """   ./_o?____\_/  """ + "\n" +
                        """      //  \\     """,

            SpriteDead = 
                        """                  """ + "\n" +
                        """   .__\\__//__    """ + "\n" +
                        """    \_*b____/ \   """,

            LootTable = new Dictionary<InventorySystem.Item, int>
            {
                { new PotionHealth(1), 50 }
            },

            ResourceTable = new Dictionary<ResourceType, int>
            {
                { ResourceType.Experience, 30 },
                { ResourceType.ExperienceBP, 100 },
                { ResourceType.Gold, 50 }
            }
        })
        {
        }

        public override void Move()
        {
            MoveRandomly();
        }
    }
}