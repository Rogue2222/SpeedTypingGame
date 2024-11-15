using Newtonsoft.Json.Linq;

namespace SpeedTypingGame.Game.Persistence
{
    public interface IPersistable
    {
        public JToken ToJSON();

        public void FromJSON(JToken json);
    }
}