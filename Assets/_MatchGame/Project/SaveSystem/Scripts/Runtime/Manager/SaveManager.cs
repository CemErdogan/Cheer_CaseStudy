using System;
using Abstractions.SaveSystem;
using UnityEngine;
using Zenject;

namespace Project.SaveSystem.Runtime
{
    public class SaveManager : ISaveManager, IInitializable, IDisposable
    {
        [Inject] private readonly SignalBus _signalBus;
        
        public void Initialize()
        {
            _signalBus.Subscribe<ISaveSignal>(OnSave);
        }

        public void Dispose()
        {
            _signalBus.Unsubscribe<ISaveSignal>(OnSave);
        }
        
        public void Save<T>(T data, string key)
        {
            try
            {
                string json;
                if (typeof(T).IsPrimitive || typeof(T) == typeof(string))
                {
                    json = Convert.ToString(data, System.Globalization.CultureInfo.InvariantCulture);
                }
                else
                {
                    json = JsonUtility.ToJson(data);
                }
                
                PlayerPrefs.SetString(key, json);
                PlayerPrefs.Save();
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to save data with key '{key}': {e.Message}");
            }
            finally
            {
                Debug.Log($"Data with key '{key}' saved successfully.");
            }
        }

        public T Load<T>(string key)
        {
            try
            {
                if (!PlayerPrefs.HasKey(key)) return default;
                
                var json  = PlayerPrefs.GetString(key);

                if (typeof(T).IsPrimitive || typeof(T) == typeof(string))
                {
                    return (T)Convert.ChangeType(json, typeof(T), System.Globalization.CultureInfo.InvariantCulture);
                }
                
                return JsonUtility.FromJson<T>(json);
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to load data with key '{key}': {e.Message}");
                return default;
            }
            finally
            {
                Debug.Log($"Data with key '{key}' saved successfully.");
            }
        }
        
        private void OnSave(ISaveSignal saveSignal)
        {
            Save(saveSignal.Data, saveSignal.Key);
        }
    }
}