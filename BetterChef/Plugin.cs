namespace StormTweaks {
    [BepInPlugin("ror2.BetterChef", "BetterChef", "1.0.0")]
    public class Main : BaseUnityPlugin {
        public static ConfigFile config;

        public static BepInEx.Logging.ManualLogSource ModLogger;

        public void Awake() {
            // set logger
            ModLogger = Logger;
            config = this.Config;
            
            CHEF.Init();
        }

        public static T Bind<T>(string sec, string name, string desc, T val) {
            return config.Bind<T>(sec, name, val, desc).Value;
        }
    }
}