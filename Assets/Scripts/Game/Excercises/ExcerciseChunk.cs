namespace SpeedTypingGame.Game.Excercises
{
    public class ExcerciseChunk
    {
        public enum ChunkType
        {
            Neutral,
            Correct,
            Wrong
        }

        // Fields
        private Excercise _excercise;

        public ChunkType Type;
        private int _startIndex;
        private int _endIndex;


        // Properties
        public bool IsNeutral => Type == ChunkType.Neutral;
        public bool IsCorrect => Type == ChunkType.Correct;
        public bool IsWrong => Type == ChunkType.Wrong;
        public int StartIndex => _startIndex;
        public int EndIndex => _endIndex;
        public string Text => _excercise.Text.Substring(_startIndex, _excercise.Length - _endIndex);


        // Methods
        public ExcerciseChunk(Excercise excercise, ChunkType type = ChunkType.Neutral)
        {
            _excercise = excercise;

            Type = type;
            _startIndex = excercise.CursorPosition;
            _endIndex = excercise.Length - 1;
        }
    }
}