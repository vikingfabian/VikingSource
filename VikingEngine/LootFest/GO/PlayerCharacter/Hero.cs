

namespace VikingEngine.LootFest.GO.PlayerCharacter
{
    class Hero : AbsHero
    {
       
         public Hero(Players.Player p)
            : base(p)
         {
             //NetworkShareObject();
         }
         public Hero(System.IO.BinaryReader r, Players.ClientPlayer parent)
             : base(r, parent)
         { }

         public override GameObjectType Type
         {
             get { return GameObjectType.Hero; }
         }

         public override void NetWriteUpdate(System.IO.BinaryWriter w)
         {
             base.NetWriteUpdate(w);
         }
        //public override GameObjectType PlayerCharacterType
        //{
        //    get { return GameObjectType.Hero; }
        //}
    }
    
}
