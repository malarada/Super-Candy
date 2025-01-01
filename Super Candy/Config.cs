using Exiled.API.Interfaces;
using System.ComponentModel;

namespace SuperCandy
{
    public class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;
        public bool Debug { get; set; } = false;

        [Description("Should the player keep their powers after escaping. THIS FEATURE CURRENTLY DOESN'T WORK. NOT MY FAULT.")]
        public bool KeepPowersOnEscape { get; set; } = true;

        [Description("The activation type: ServerSpecific (for a server specific keybind) or NoClipKey (just using the noclip key)\nTHIS DOESN'T WORK RN")]
        public ActivationType ActivationType { get; set; } = ActivationType.NoClipKey;
    }

    public enum ActivationType
    {
        ServerSpecific,
        NoClipKey
    }
}
