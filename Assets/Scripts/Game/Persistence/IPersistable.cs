using Newtonsoft.Json.Linq;

namespace SpeedTypingGame.Game.Persistence
{
    /// <summary>
    /// Describes an interface for objects that need to be persisted using JSON conversion.
    /// </summary>
    public interface IPersistable
    {
        JToken ToJSON();

        void FromJSON(JToken json);
    }
}