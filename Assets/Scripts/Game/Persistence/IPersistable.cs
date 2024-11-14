using Newtonsoft.Json.Linq;

namespace SpeedTypingGame.Game.Persistence
{
    public interface IPersistable
    {
        public JObject ToJSON();

        public void FromJSON(JObject json);
    }
}