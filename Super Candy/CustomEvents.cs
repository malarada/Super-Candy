using Exiled.Events.Handlers;

namespace Super_Candy_Plugin
{
    public class CustomEvents
    {
        public static void SubscribeEvents()
        {
            Player.Verified += EventHandler.PlayerVerified;
            Player.Spawning += EventHandler.PlayerSpawning;
            Player.TogglingNoClip += EventHandler.TogglingNoClip;
            Player.ChangingMoveState += EventHandler.PlayerChangingMoveState;
            Player.ActivatingGenerator += EventHandler.PlayerActivatingGenerator;

            Scp330.EatenScp330 += EventHandler.EatenScp330;
        }

        public static void UnsubscribeEvents()
        {
            Player.Verified -= EventHandler.PlayerVerified;
            Player.Spawning -= EventHandler.PlayerSpawning;
            Player.TogglingNoClip -= EventHandler.TogglingNoClip;
            Player.ChangingMoveState -= EventHandler.PlayerChangingMoveState;
            Player.ActivatingGenerator -= EventHandler.PlayerActivatingGenerator;

            Scp330.EatenScp330 -= EventHandler.EatenScp330;
        }
    }
}
