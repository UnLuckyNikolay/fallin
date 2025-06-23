using Fallin.InventorySystem.Items;
using Fallin.Enums;
using static Utilities.ConsoleHelper;

namespace Fallin.Characters.Enemies
{
    public class BigRat : Enemy
    {
        public BigRat(GameStateManager GSM) : 
        base(GSM,
        new CharacterProperties{
            Level = 5,
            Name = "Big Rat",
            NameMap = "RT",
            NameColor = Color.DarkRed,

            Strength = 4,
            Perception = 2,
            Endurance = 3,
            Charisma = 0,
            Intelligence = 2,
            Agility = 0,
            Luck = 5,

            HealthMultiplier = 1.5f,
            DamageMultiplier = 0.7f

        }, new EnemyProperties{
            SpriteAlive = 
"""           _     __,..-----.._               ':-.     """ + "\n" +
"""         _/_)_.-`             '-.              `\\    """ + "\n" +
"""   \|.-'`   /o)                  '.              ||   """ + "\n" +
"""   /.   _o  ,                      \            .'/   """ + "\n" +
"""   \,_|\__,/                _..-    \       _.-'.'    """ + "\n" +
"""      | \ \      \         /         `----'`_.-'      """ + "\n" +
"""         _/;--._\ )        |   _\.__/`-----'          """ + "\n" +
"""       (((-'  __//`'-..__..-\       )                 """ + "\n" +
"""            (((-'       __// ''--. /                  """ + "\n" +
"""                      (((-'    __//                   """ + "\n" +
"""                             (((-'                    """,

            SpriteDead = 
"""                                                               """ + "\n" +
"""                                                               """ + "\n" +
"""                  _,..-----.._                                 """ + "\n" +
"""          _    .-'            '-.                              """ + "\n" +
"""        _/_)_./                  '.                            """ + "\n" +
"""  \|.-'`   /o)                     \                           """ + "\n" +
"""  /.   _x  ,                _..-    \                          """ + "\n" +
"""  \,_|\__,/      \         /         `----..__                 """ + "\n" +
"""     | \ \/;--._\ )        |   _\.__/`----..__''--._           """ + "\n" +
"""       (((-' (((-'`'-..___.-\..__   )         ''--. \          """ + "\n" +
"""                      (((-'/  (((-'/               \|          """,

            LootTable = new Dictionary<InventorySystem.Item, int>
            {
                { new PotionHealth(2), 100 }
            },

            ResourceTable = new Dictionary<ResourceType, int>
            {
                { ResourceType.Experience, 75 },
                { ResourceType.ExperienceBP, 300 },
                { ResourceType.Gold, 100 }
            }
        })
        {
        }

        public override void Move()
        {
            MoveRandomly();
        }

        public override void Attack(Character target)
        {
            AttackCharged(target);
        }
    }
}