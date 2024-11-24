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
        private Exercise _exercise;

        public ChunkType Type;
        private int _startIndex;
        private int _endIndex;


        // Properties
        public bool IsNeutral => Type == ChunkType.Neutral;
        public bool IsCorrect => Type == ChunkType.Correct;
        public bool IsWrong => Type == ChunkType.Wrong;
        public int StartIndex => _startIndex;
        public int EndIndex => _endIndex;
        public string Text => _exercise.Text.Substring(_startIndex, _exercise.Length - _endIndex);


        // Methods
        public ExcerciseChunk(Exercise exercise, ChunkType type = ChunkType.Neutral)
        {
            _exercise = exercise;

            Type = type;
            _startIndex = exercise.CursorPosition;
            _endIndex = exercise.Length - 1;
        }
    }
}