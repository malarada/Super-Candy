
using Exiled.Events.Handlers;

namespace SuperCandy
{
    public class CustomEvents
    {
        public static void SubscribeEvents()
        {
            Player.Verified += EventHandler.PlayerVerified;
            Player.Spawning += EventHandler.PlayerSpawning;
            Player.Left += EventHandler.PlayerLeft;
            Player.TogglingNoClip += EventHandler.ToggleNoClip;
            Scp330.EatenScp330 += EventHandler.Eaten330;
        }

        public static void UnsubscribeEvents()
        {
            Player.Verified -= EventHandler.PlayerVerified;
            Player.Spawning -= EventHandler.PlayerSpawning;
            Player.Left -= EventHandler.PlayerLeft;
            Player.TogglingNoClip -= EventHandler.ToggleNoClip;
            Scp330.EatenScp330 -= EventHandler.Eaten330;
        }

        /*
        public static void SubscribeEvents()
        {
            Player.Verified += AltEventHandler.PlayerVerified;
            Player.Spawning += AltEventHandler.PlayerSpawning;
            Player.TogglingNoClip += AltEventHandler.TogglingNoClip;
            Player.ChangingMoveState += AltEventHandler.PlayerChangingMoveState;
            Player.ActivatingGenerator += AltEventHandler.PlayerActivatingGenerator;

            Scp330.EatenScp330 += AltEventHandler.EatenScp330;
        }

        public static void UnsubscribeEvents()
        {
            Player.Verified -= AltEventHandler.PlayerVerified;
            Player.Spawning -= AltEventHandler.PlayerSpawning;
            Player.TogglingNoClip -= AltEventHandler.TogglingNoClip;
            Player.ChangingMoveState -= AltEventHandler.PlayerChangingMoveState;
            Player.ActivatingGenerator -= AltEventHandler.PlayerActivatingGenerator;

            Scp330.EatenScp330 -= AltEventHandler.EatenScp330;
        }
        */
    }
}
