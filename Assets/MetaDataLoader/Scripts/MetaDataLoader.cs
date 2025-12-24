using System;
using System.Collections.Generic;
using System.IO;
using OrbitalSnake.PowerUp;
using OrbitalSnake.Projectiles;
using UnityEditor;
using UnityEngine;

namespace OrbitalSnake.Meta
{
    [CreateAssetMenu(fileName = "MetaLoader", menuName = "ScriptableObjects/Meta/MetaData")]
    public class MetaDataLoader : ScriptableObject
    {
        public string MetaFileName;

        [SerializeField] private ProjectileSpawnConfig ProjectileSpawnConfig;

        [SerializeField] private ProjectileConfig FireballConfig;
        [SerializeField] private ProjectileConfig SpikeBallConfig;
        [SerializeField] private ProjectileConfig SnowballConfig;

        [SerializeField] private EarthConfig EarthConfig;
        [SerializeField] private CameraShakeConfig CameraShakeConfig;

        [SerializeField] private FreezePowerUp FreezePowerUp;
        [SerializeField] private BoundaryConfig BoundaryConfig;

        [SerializeField] private SnakeConfig SnakeConfig;

        public Dictionary<string, Action<string[]>> _metaDataParseActions = new();
        public Dictionary<string, List<string[]>> _ParsedCsv = new();

        public void TestLoading()
        {
            ClearAll();
            Init();
            LoadData();
        }

        public void LoadAllData()
        {
            ClearAll();
            Init();
            LoadData();
            if (Debug.isDebugBuild)
            {
                Debug.Log("[MetaData Loader]Data Loaded");
            }
        }

        private void LoadData()
        {
            TextAsset textAsset = null;
            textAsset = Resources.Load(MetaFileName) as TextAsset;
            if (textAsset != null)
            {
                //Debug.Log("Is not Null");
                LoadDataUsingCSV(textAsset.text);
            }
        }

        private void ClearAll()
        {
            _metaDataParseActions.Clear();
            _ParsedCsv.Clear();
        }

        private void LoadDataUsingCSV(string data)
        {
            Debug.Log(data);
            var tableCount = 0;
            var lastTableName = "";
            var reader = new StringReader(data);
            if (reader == null)
            {
                Debug.Log("metaData data not readable");
                return;
            }
            else
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    var elements = line.Split(',');
                    var sectionId = elements[0];
                    var skippedHeader = false;
                    string dataLine;

                    while ((dataLine = reader.ReadLine()) != null)
                    {
                        var dataElements = dataLine.Split(',');
                        if (dataElements[0] == "[END]")
                        {
                            break;
                        }

                        if (!skippedHeader)
                        {
                            skippedHeader = true;
                            continue;
                        }

                        if (_metaDataParseActions.ContainsKey(sectionId))
                        {
                            if (!string.Equals(lastTableName, sectionId))
                            {
                                lastTableName = sectionId;
                                tableCount++;
                            }

                            _metaDataParseActions[sectionId].Invoke(dataElements);
                        }

                        if (!_ParsedCsv.ContainsKey(sectionId))
                        {
                            _ParsedCsv.Add(sectionId, new List<string[]>());
                        }

                        List<string[]> container = _ParsedCsv[sectionId];
                        container.Add(dataElements);
                    }
                }

                reader.Close();
            }

            tableCount = _metaDataParseActions.Count;
        }

        public void Init()
        {
            _ParsedCsv = new Dictionary<string, List<string[]>>();
            _metaDataParseActions = new Dictionary<string, Action<string[]>>()
            {
                { "[ProjectileSpawnConfig]", ParseProjectileSpawnConfig },
                { "[ProjectileData]", ParseProjectileData },
                { "[PlanetData]", ParsePlanetData },
                { "[CameraShakeConfig]", ParseCameraShake },
                { "[FreezePowerUp]", ParseFreezePowerUp },
                { "[BoundaryConfig]", ParseBoundaryConfig },
                { "[SnakeConfig]", ParseSnakeConfig }
            };
        }

        private void ParseProjectileSpawnConfig(string[] elements)
        {
            if (elements == null)
            {
                if (Debug.isDebugBuild)
                {
                    Debug.LogError("elements is null");
                }

                return;
            }

            ProjectileSpawnConfig.SpawnDelay = ParseFloat(elements[0]);
            ProjectileSpawnConfig.SpawnRadiusMultiplier = ParseFloat(elements[1]);
        }

        private void ParseProjectileData(string[] elements)
        {
            if (elements == null)
            {
                if (Debug.isDebugBuild)
                {
                    Debug.LogError("elements is null");
                }

                return;
            }

            if (ParseEnum<ProjectileType>(elements[0]) == ProjectileType.Fireball)
            {
                ProjectileSpawnConfig.GetProjectileSpawnData(ProjectileType.Fireball)
                    .UpdateSpawnProbability(ParseFloat(elements[1]));
                FireballConfig.Speed = ParseFloat(elements[2]);
                FireballConfig.Damage = ParseFloat(elements[3]);
            }
            else if (ParseEnum<ProjectileType>(elements[0]) == ProjectileType.SpikeBall)
            {
                ProjectileSpawnConfig.GetProjectileSpawnData(ProjectileType.SpikeBall)
                    .UpdateSpawnProbability(ParseFloat(elements[1]));
                SpikeBallConfig.Speed = ParseFloat(elements[2]);
                SpikeBallConfig.Damage = ParseFloat(elements[3]);
            }
            else if (ParseEnum<ProjectileType>(elements[0]) == ProjectileType.Snowball)
            {
                ProjectileSpawnConfig.GetProjectileSpawnData(ProjectileType.Snowball)
                    .UpdateSpawnProbability(ParseFloat(elements[1]));
                SnowballConfig.Speed = ParseFloat(elements[2]);
                SnowballConfig.Damage = ParseFloat(elements[3]);
            }
        }

        private void ParsePlanetData(string[] elements)
        {
            if (elements == null)
            {
                if (Debug.isDebugBuild)
                {
                    Debug.LogError("elements is null");
                }

                return;
            }

            EarthConfig.StartingHealth = ParseFloat(elements[0]);
            EarthConfig.Radius = ParseFloat(elements[1]);
        }

        private void ParseCameraShake(string[] elements)
        {
            if (elements == null)
            {
                if (Debug.isDebugBuild)
                {
                    Debug.LogError("elements is null");
                }

                return;
            }

            CameraShakeConfig.Duration = ParseFloat(elements[0]);
            CameraShakeConfig.Strength = ParseFloat(elements[1]);
            CameraShakeConfig.Vibratio = ParseInt(elements[2]);
            CameraShakeConfig.Randomness = ParseInt(elements[3]);
            CameraShakeConfig.Snapping = ParseBool(elements[4]);
            CameraShakeConfig.FadeOut = ParseBool(elements[5]);
        }

        private void ParseFreezePowerUp(string[] elements)
        {
            if (elements == null)
            {
                if (Debug.isDebugBuild)
                {
                    Debug.LogError("elements is null");
                }

                return;
            }

            FreezePowerUp.SetPowerUpDuration(ParseFloat(elements[0]));
        }

        private void ParseBoundaryConfig(string[] elements)
        {
            if (elements == null)
            {
                if (Debug.isDebugBuild)
                {
                    Debug.LogError("elements is null");
                }

                return;
            }

            BoundaryConfig.InnerRadiusMultplier = ParseFloat(elements[0]);
            BoundaryConfig.CenterPoint = ParseVector2(elements[1]);
            BoundaryConfig.padding = ParseFloat(elements[2]);
        }

        private void ParseSnakeConfig(string[] elements)
        {
            if (elements == null)
            {
                if (Debug.isDebugBuild)
                {
                    Debug.LogError("elements is null");
                }

                return;
            }

            SnakeConfig.MoveSpeed = ParseFloat(elements[0]);
            SnakeConfig.SteerSpeed = ParseFloat(elements[1]);
            SnakeConfig.SteerLerpSpeed = ParseFloat(elements[2]);
            SnakeConfig.gap = ParseInt(elements[3]);
            SnakeConfig.initialBodySize = ParseInt(elements[4]);
        }

        // Helper methods for parsing
        private int ParseInt(string value) => int.TryParse(value, out var result) ? result : 0;
        private float ParseFloat(string value) => float.TryParse(value, out var result) ? result : 0f;

        private bool ParseBool(string value) => bool.TryParse(value, out var result) && result;

        private Vector2 ParseVector2(string value)
        {
            value = value.Trim('(', ')', '"');
            string[] parts = value.Split(',');
            if (parts.Length == 2)
            {
                return new Vector2(ParseFloat(parts[0]), ParseFloat(parts[1]));
            }

            return Vector2.zero;
        }

        private Vector3 ParseVector3(string value)
        {
            value = value.Trim('(', ')', '"');
            string[] parts = value.Split(',');
            if (parts.Length == 3)
            {
                return new Vector3(ParseFloat(parts[0]), ParseFloat(parts[1]), ParseFloat(parts[2]));
            }

            return Vector3.zero;
        }

        private T ParseEnum<T>(string value) where T : struct =>
            Enum.TryParse<T>(value, true, out var result) ? result : default;
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(MetaDataLoader))]
    public class MetaDataLoaderEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            MetaDataLoader system = (MetaDataLoader)target;

            if (GUILayout.Button("LoadMetaData"))
            {
                system.LoadAllData();
            }
        }
    }
#endif
}