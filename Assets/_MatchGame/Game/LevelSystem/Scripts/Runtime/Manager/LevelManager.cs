using System;
using System.Collections.Generic;
using System.Linq;
using Abstractions.LevelSystem;
using Abstractions.SaveSystem;
using Zenject;

namespace Game.LevelSystem.Runtime
{
    public class LevelManager : ILevelManager, IInitializable, IDisposable
    {
        [Inject] private readonly SignalBus _signalBus;
        [Inject] private readonly LevelDatabase _database;
        [Inject] private readonly ISaveManager _saveManager;
        
        private int _currentLevelIndex;
        private ILevel _currentLevel;
        private readonly Queue<int> _rndQueue = new();
        
        public void Initialize()
        {
            _currentLevelIndex = _saveManager.Load<int>(SaveKeys.LevelIndex);
#if UNITY_EDITOR
            var testLevelName = UnityEditor.EditorPrefs.GetString("MatchGame.TestLevelName", string.Empty);
            if (!string.IsNullOrEmpty(testLevelName))
            {
                UnityEditor.EditorPrefs.DeleteKey("MatchGame.TestLevelName");
                var levels = _database.Levels;
                for (int i = 0; i < levels.Length; i++)
                {
                    if (levels[i].name == testLevelName)
                    {
                        _currentLevelIndex = i;
                        break;
                    }
                }
            }
#endif
            _signalBus.Subscribe<ICompleteLevelSignal>(OnLevelCompleted);

            LoadLevel();
        }

        public void Dispose()
        {
            _signalBus.TryUnsubscribe<ICompleteLevelSignal>(OnLevelCompleted);
        }
        
        public void LoadLevel()
        {
            UnityEngine.Debug.Log($"[LevelManager] LoadLevel called. Index: {_currentLevelIndex}");
            UnloadAndLoadLevel(GetCurrentLevel());
        }

        private void UnloadAndLoadLevel(ILevel level)
        {
            _currentLevel?.Unload();
            _currentLevel = level;
            _currentLevel.Load();
        }

        private ILevel GetCurrentLevel()
        {
            var lvls = _database.Levels;
            return lvls[_currentLevelIndex];
        }

        private void RefillRandomQueue()
        {
            _rndQueue.Clear();
            var indices = Enumerable.Range(0, _database.Levels.Length).OrderBy(_ => UnityEngine.Random.value).ToList();
            foreach (var i in indices)
            {
                _rndQueue.Enqueue(i);
            }
        }
        
        private void OnLevelCompleted(ICompleteLevelSignal signal)
        {
            var completeType = signal.CompleteType;
            var lvs = _database.Levels;
            var nextIndex = _currentLevelIndex;

            if (completeType == CompleteType.Win)
            {
                nextIndex++;
            }

            if (nextIndex < lvs.Length)
            {
                _currentLevelIndex = nextIndex;
            }
            else
            {
                if (_rndQueue.Count == 0)
                {
                    RefillRandomQueue();
                }

                _currentLevelIndex = _rndQueue.Dequeue();
            }
            
            _saveManager.Save(_currentLevelIndex, SaveKeys.LevelIndex);
        }
    }
}