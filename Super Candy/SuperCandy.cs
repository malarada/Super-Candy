using System;

using Exiled.API.Features;

namespace Super_Candy_Plugin
{
    public class SuperCandyPlugin : Plugin<Config>
    {
        public override string Name => "Super Candy Plugin";
        public override string Prefix => Name;
        public override string Author => "PyroCyclone";
        public override Version Version => new Version(1, 0, 0);
        public override Version RequiredExiledVersion => new Version(8, 11, 0);

        public override void OnEnabled()
        {
            CustomEvents.SubscribeEvents();

            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            CustomEvents.UnsubscribeEvents();

            base.OnDisabled();
        }
    }
}
