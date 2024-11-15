using System.Data;
using Mono.Data.Sqlite;
using UnityEngine;

namespace SpeedTypingGame.Game.Persistence
{
    [AddComponentMenu("SpeedTypingGame/Game/Persistence/Database")]
    public class Database : PersistenceHandler
    {
        // Fields
        private static string _DataPath;
        private const string _CharacterDataTableName = "character_data";
        private const string _ExcerciseDataTableName = "excercise_data";

        private IDbConnection _connection;


        // Methods
        private void Awake()
        {
            _DataPath = $"URI=file:{Application.persistentDataPath}/Database/save.sqlite";
        }

        private new void Start()
        {
            Connect();
            Setup();

            Load();
        }

        private new void OnApplicationQuit()
        {
            Save();
            
            _connection.Close();
        }

        public override void Save()
        {
            
        }

        public override void Load()
        {

        }

        private void Connect()
        {
            _connection = new SqliteConnection(_DataPath);
            _connection.Open();
        }

        private void Setup()
        {
            IDbCommand characterDataTableCommand = _connection.CreateCommand();
            characterDataTableCommand.CommandText =
                $"CREATE TABLE IF NOT EXISTS {_CharacterDataTableName} " +
                $"(character TEXT PRIMARY KEY, correct_typings INT, incorrect_typings INT)";
            characterDataTableCommand.ExecuteReader();

            IDbCommand excerciseDataTableCommand = _connection.CreateCommand();
            excerciseDataTableCommand.CommandText =
                $"CREATE TABLE IF NOT EXISTS {_ExcerciseDataTableName} " +
                $"(timestamp INT PRIMARY KEY, duration FLOAT," +
                $"length INT, correct_characters INT, incorrect_characters INT)";
            excerciseDataTableCommand.ExecuteReader();
        }
    }
}