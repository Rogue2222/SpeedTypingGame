using Newtonsoft.Json.Linq;

namespace SpeedTypingGame.Game.Persistence
{
    /// <summary>
    /// Describes an interface for objects that need to be persisted using JSON conversion.
    /// </summary>
    public interface IPersistable
    {
        /// <summary>
        /// Converts the implementing object into a JSON format that sufficiently describes it.
        /// </summary>
        /// <returns>The proper JSON representation made of this object's values.</returns>
        JToken ToJSON();

        /// <summary>
        /// Deconvert a sufficient JSON format describing this object to set its values.
        /// </summary>
        /// <param name="json">The proper JSON representation of this object's values.</param>
        void FromJSON(JToken json);
    }
}