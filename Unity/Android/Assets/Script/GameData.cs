using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using RDG;
// * testing data delete
using UnityEngine.SceneManagement;
public class GameData : MonoBehaviour
{
    void OnEnable()
    {
        GameMaster.onTransitionReady += DataSave;
        // 
        UIPrompt.onConfirm += DataProcess;
        // save on clear
        GameMaster.onClear += DoClear;
    }
    void OnDisable()
    {
        GameMaster.onTransitionReady -= DataSave;
        // 
        UIPrompt.onConfirm -= DataProcess;
        // save on clear
        GameMaster.onClear -= DoClear;
    }
    private void DataProcess(int typeButton)
    {
        // settings
        if (typeButton == 4) DataReset();
        // ads cleared
        else if (typeButton == 5 || typeButton == 6) DataDelete();
        // exit
        else if (typeButton == 7) Application.Quit();
    }
    public delegate void OnReset();
    public static event OnReset onReset;
    private void DataReset()
    {
        // 
        // print("data reset");
        // PlayerPrefs.SetInt("difficulty", 1);
        // PlayerPrefs.SetInt("theme", 0);
        // PlayerPrefs.SetInt("vibration", 0);
        // restore defaults
        Difficulty = DIFFICULTY;
        Theme = THEME;
        Vibrate = VIBRATION;
        // notify ui settings
        onReset?.Invoke();
    }
    // settings defaults
    private const int DIFFICULTY = 1;
    private const int THEME = 0;
    private const int VIBRATION = 1;
    public static int Difficulty
    {
        get { return PlayerPrefs.GetInt("difficulty", DIFFICULTY); }
        set { PlayerPrefs.SetInt("difficulty", value); }
    }
    public static int Theme
    {
        get { return PlayerPrefs.GetInt("theme", THEME); }
        set { PlayerPrefs.SetInt("theme", value); }
    }
    public static int Vibrate
    {
        get { return PlayerPrefs.GetInt("vibration", VIBRATION); }
        set { PlayerPrefs.SetInt("vibration", value); }
    }
    private static bool IsVibrate
    {
        get { return Vibrate == 1; }
    }
    public static int AdBanner
    {
        get { return PlayerPrefs.GetInt("adBanner", 0); }
        set { PlayerPrefs.SetInt("adBanner", value); }
    }
    public static int AdPopup
    {
        get { return PlayerPrefs.GetInt("adPopup", 0); }
        set { PlayerPrefs.SetInt("adPopup", value); }
    }
    public static int AdVideo
    {
        get { return PlayerPrefs.GetInt("adVideo", 0); }
        set { PlayerPrefs.SetInt("adVideo", value); }
    }
    // * testing vibration
    public enum IntensityVibrate
    {
        LOW,
        MEDIUM,
        HIGH
    }
    public static void DoVibrate(IntensityVibrate intensity)
    {
        // * disable for WebGL Build
        // if (IsVibrate)
        // {
        //     if (Application.platform == RuntimePlatform.Android)
        //     {
        //         switch (intensity)
        //         {
        //             case IntensityVibrate.LOW:
        //                 Vibration.Vibrate(4, 4);
        //                 break;
        //             case IntensityVibrate.MEDIUM:
        //                 Vibration.Vibrate(32, 32);
        //                 break;
        //             case IntensityVibrate.HIGH:
        //                 Vibration.Vibrate(64, 64);
        //                 break;
        //         }
        //     }
        //     else Handheld.Vibrate();
        //     // // * testing vibrate
        //     // print("vibrate " + intensity.ToString());
        // }
    }
    private void DataDelete()
    {
        // tropy/ad data
        DeletePrefs();
        // data exists
        if (File.Exists(_filePath))
        {
            // data remove
            File.Delete(_filePath);
            // meta remove
            if (File.Exists(_filePath + ".meta")) File.Delete(_filePath + ".meta");
        }
        // else Teleprompter.Register("Save Data not found");
        // * testing data delete ? fade out delay
        SceneManager.LoadScene(0);
        // 
        // print("Data Delete");
    }
    // // trophies
    // private int TrophyNinja
    // {
    //     get { return PlayerPrefs.GetInt("trophyNinja", 1); }
    //     set { PlayerPrefs.SetInt("trophyNinja", 1); }
    // }
    // * dev only
    private void DeletePrefs()
    {
        // // read tick count
        // PlayerPrefs.SetInt("ticksBest", 99999);
        // // 
        // PlayerPrefs.SetInt("difficulty", 0);
        // // trophies
        // PlayerPrefs.SetInt("trophySlayer", 0);
        // PlayerPrefs.SetInt("trophySlayerLock", 0);
        // PlayerPrefs.SetInt("trophyPlunderer", 0);
        // PlayerPrefs.SetInt("trophyPlundererLock", 0);
        // PlayerPrefs.DeleteAll();
        // read tick count
        _ticksBest = 99999;
        // // 
        // _difficulty = 0;
        // _theme = 0;
        // _vibration = 0;
        // trophies
        // - ninja
        // got seen
        if (!_trophies[0,0])
        {
            // give another chance
            _trophies[0,0] = true;
            _trophies[0,1] = false;
            // 
            PlayerPrefs.SetInt("trophyNinja", 1);
            PlayerPrefs.SetInt("trophyNinjaLock", 0);
        }
        // // - slayer
        // _trophies[1,0] = false;
        // _trophies[1,1] = false;
        // - pacifist
        // hurt someone
        if (!_trophies[2,0])
        {
            // give another chance
            _trophies[2,0] = true;
            _trophies[2,1] = false;
            // 
            PlayerPrefs.SetInt("trophyPacifist", 1);
            PlayerPrefs.SetInt("trophyPacifistLock", 0);
        }
        // // - plunderer
        // _trophies[3,0] = false;
        // _trophies[3,1] = false;
        // - ghost
        // got hurt
        if (!_trophies[4,0])
        {
            // give another chance
            _trophies[4,0] = true;
            _trophies[4,1] = false;
            // 
            PlayerPrefs.SetInt("trophyGhost", 1);
            PlayerPrefs.SetInt("trophyGhostLock", 0);
        }
        // - raider
        // took too long
        if (!_trophies[5,0])
        {
            // give another chance
            _trophies[5,0] = true;
            _trophies[5,1] = false;
            // 
            PlayerPrefs.SetInt("trophyRaider", 1);
            PlayerPrefs.SetInt("trophyRaiderLock", 0);
        }
        // ads
        PlayerPrefs.SetInt("adBanner", 0);
        PlayerPrefs.SetInt("adPopup", 0);
        PlayerPrefs.SetInt("adVideo", 0);
    }
    // * testing save/load ? use singletons
    // public Breakable _player;
    // ? set to default values
    private static Vector2Int _room;
    private static Vector2Int _direction;
    private static int _ticks;
    private static int _ticksBest;
    // private static int _difficulty;
    // private static int _theme;
    // private static int _vibration;
    // private Vector2Int _room;
    // [SerializeField] private Vector3 _playerPosition = new Vector3(1.5f, .5f, 2f);
    // [SerializeField] private Vector3 _playerRotation = new Vector3(0f, 0f, 0f);
    // [SerializeField] private int _playerHealth = 6;
    // private static Dictionary<string, EntityData> _entities = new Dictionary<string, EntityData>();
    private static Dictionary<string, EntityData> _entities;
    // [Serializable]
    protected enum EntityType
    {
        DEFAULT,
        BREAKABLE,
        CREATURE,
        INTERACT,
        REACT,
        REACT_SEQUENCE,
        // ITEM_HEAL,
        ITEM_CONSUME
    }
    public static Vector2Int Room
    {
        get { return _room; }
    }
    public static Vector2Int Direction
    {
        get { return _direction; }
    }
    public static int Ticks
    {
        get { return _ticks; }
    }
    public static int TicksBest
    {
        get { return _ticksBest; }
    }
    // public static int Difficulty
    // {
    //     get { return _difficulty; }
    // }
    // public static int Theme
    // {
    //     get { return _theme; }
    // }
    // public static int Vibration
    // {
    //     get { return _vibration; }
    // }
    // private void DataUpdate(Vector2Int room)
    // {
    //     // room
    //     _room = room;
    //     // player
    //     _playerPosition = _player.Position;
    //     _playerRotation = _player.Rotation;
    //     _playerHealth = _player.HealthInst;
    //     // breakable
    //     // item
    //     // mob
    //     // 
    //     StartCoroutine("DataSave");
    // }
    // IEnumerator DataSave()
    // {
    //     yield return null;
    // }
    // 
    // * testing trophy
    private static bool[,] _trophies;
    // ninja* - escape undetected, fail if even one mob detects player
    // slayer - kill all enemies, count up as mobs die and match to hard coded value
    // pacifist* - do no harm, attack action executed? mob hurt [vs breakable]
    // plunderer - collect all treasures, inventory check on clear
    // ghost* - take zero damage, player health drain
    // raider* - escape quickly, check ticks against hard coded value
    // * locks reset between runs to allow achieving all?
    public enum Trophy
    {
        NINJA, SLAYER, PACIFIST, PLUNDERER, GHOST, RAIDER
    }
    private static int _mobKills;
    private const int MOB_TOTAL = 52;
    private const int TICKS_MIN = 200;
    // called only when needed so can assume expected behaviour
    public static void SetTrophy(Trophy trophy)
    {
        // print("Tropy " + trophy.ToString());
        switch (trophy)
        {
            case Trophy.NINJA:
                // already locked
                if (_trophies[0,1]) return;
                // 
                _trophies[0,0] = false;
                // locked out for this run
                _trophies[0,1] = true;
                break;
            case Trophy.SLAYER:
                // already locked
                if (_trophies[1,1]) return;
                // 
                _mobKills++;
                if (_mobKills >= MOB_TOTAL)
                {
                    _trophies[1,0] = true;
                    _trophies[1,1] = true;
                }
                break;
            case Trophy.PACIFIST:
                // already locked
                if (_trophies[2,1]) return;
                // 
                _trophies[2,0] = false;
                // locked out for this run
                _trophies[2,1] = true;
                break;
            case Trophy.PLUNDERER:
                // already locked
                if (_trophies[3,1]) return;
                // 
                _trophies[3,0] = true;
                _trophies[3,1] = true;
                break;
            case Trophy.GHOST:
                // already locked
                if (_trophies[4,1]) return;
                // 
                _trophies[4,0] = false;
                // locked out for this run
                _trophies[4,1] = true;
                break;
            case Trophy.RAIDER:
                // already locked
                if (_trophies[5,1]) return;
                // 
                if (_ticks > TICKS_MIN)
                {
                    _trophies[5,0] = false;
                    // locked out for this run?
                    _trophies[5,1] = true;
                }
                break;
        }
    }
    public static bool IsTrophyNinja
    {
        get { return _trophies[0,0]; }
    }
    public static bool IsTrophySlayer
    {
        get { return _trophies[1,0]; }
    }
    public static bool IsTrophyPacifist
    {
        get { return _trophies[2,0]; }
    }
    public static bool IsTrophyPlunderer
    {
        get { return _trophies[3,0]; }
    }
    public static bool IsTrophyGhost
    {
        get { return _trophies[4,0]; }
    }
    public static bool IsTrophyRaider
    {
        get { return _trophies[5,0]; }
    }
    // 
    private string _filePath;
    private const string DATA_SEPARATOR = "#";
    void Awake()
    {
        _room = Vector2Int.zero;
        _direction = Vector2Int.down;
        _ticks = 0;
        _ticksBest = 99999;
        // 
        _entities = new Dictionary<string, EntityData>();
        // _filePath = Application.dataPath + "/data.json";
        _filePath = Application.persistentDataPath + "/data.json";
        _trophies = new bool[6,2];
        // // * testing trophy ? constant between saves
        // _trophies[0,0] = true;
        // _trophies[1,0] = false;
        // _trophies[2,0] = true;
        // _trophies[3,0] = false;
        // _trophies[4,0] = true;
        // _trophies[5,0] = true;
        // for (int i = 0; i < 6; i++) _trophies[i,1] = false;
        // print(_filePath);
        // initialize entity data
        DataLoad();
    }
    private void DataLoad()
    {
        // check if save file exists
        // take data from file and load into variables
        // // * testing with playerPrefs ? memory waste
        // _room = new Vector2Int(PlayerPrefs.GetInt("roomX", _room.x), PlayerPrefs.GetInt("roomY", _room.y));
        // cant get data since no entries at this time ? always reopen scene in case of load
        // foreach (KeyValuePair<string, EntityData> entity in _entities)
        // parse file and create entries from data contained ?
        // 
        // save file dependent data otherwise assume defaults
        if (File.Exists(_filePath))
        {
            // read text from file
            string load = File.ReadAllText(_filePath);
            // print("load: " + load);
            // split based on predefined separator ? no clue what's happening here
            string[] data = load.Split(new [] { DATA_SEPARATOR }, System.StringSplitOptions.None);
            // read room data
            _room.Set(int.Parse(data[0]), int.Parse(data[1]));
            // read transition direction
            _direction.Set(int.Parse(data[2]), int.Parse(data[3]));
            // read tick count
            _ticks = int.Parse(data[4]);
            // placeholder
            EntityData entity;
            // entity data
            for (int i = data.Length - 1; i > 4; i--)
            {
                // read entity data ? garbage
                // EntityData entity = JsonUtility.FromJson<EntityData>(data[i]);
                entity = JsonUtility.FromJson<EntityData>(data[i]);
                // FromJsonOverwrite(data[i], entity);
                // filter by entity type
                switch (entity.Type)
                {
                    case EntityType.DEFAULT:
                        // create new entry
                        // ? make a deep copy
                        EntityData entityDefault = JsonUtility.FromJson<EntityData>(data[i]);
                        _entities.Add(entityDefault.ID, entityDefault);
                        break;
                    case EntityType.BREAKABLE:
                        // create new entry
                        // _entities.Add(entity.ID, new DataBreakable(entity.ID, (entity as DataBreakable).Health));
                        // 
                        DataBreakable entityBreakable = JsonUtility.FromJson<DataBreakable>(data[i]);
                        _entities.Add(entityBreakable.ID, entityBreakable);
                        break;
                    case EntityType.CREATURE:
                        // create new entry
                        // _entities.Add(entity.ID, new DataCreature(entity.ID, (entity as DataCreature).Health, (entity as DataCreature).Position, (entity as DataCreature).Rotation));
                        // _entities.Add(entity.ID, JsonUtility.FromJson<DataCreature>(data[i]));
                        // 
                        DataCreature entityCreature = JsonUtility.FromJson<DataCreature>(data[i]);
                        _entities.Add(entityCreature.ID, entityCreature);
                        break;
                    case EntityType.INTERACT:
                        DataInteract entityInteract = JsonUtility.FromJson<DataInteract>(data[i]);
                        _entities.Add(entityInteract.ID, entityInteract);
                        break;
                    case EntityType.REACT:
                        DataReact entityReact = JsonUtility.FromJson<DataReact>(data[i]);
                        _entities.Add(entityReact.ID, entityReact);
                        break;
                    case EntityType.REACT_SEQUENCE:
                        DataReactSequence entityReactSequence = JsonUtility.FromJson<DataReactSequence>(data[i]);
                        _entities.Add(entityReactSequence.ID, entityReactSequence);
                        break;
                    // case EntityType.ITEM_HEAL:
                    //     DataItemHeal entityItemHeal = JsonUtility.FromJson<DataItemHeal>(data[i]);
                    //     _entities.Add(entityItemHeal.ID, entityItemHeal);
                    //     break;
                    case EntityType.ITEM_CONSUME:
                        DataItemConsume entityItemConsume = JsonUtility.FromJson<DataItemConsume>(data[i]);
                        _entities.Add(entityItemConsume.ID, entityItemConsume);
                        break;
                }
            }
            // print(_entities.Count);
            // print("Data Load: " + _room);
        }
        // read tick count
        _ticksBest = PlayerPrefs.GetInt("ticksBest", 99999);
        // // settings
        // _difficulty = PlayerPrefs.GetInt("difficulty", 0);
        // _theme = PlayerPrefs.GetInt("theme", 0);
        // _vibration = PlayerPrefs.GetInt("vibration", 0);
        // trophies
        _trophies[0,0] = PlayerPrefs.GetInt("trophyNinja", 1) == 1;
        _trophies[0,1] = PlayerPrefs.GetInt("trophyNinjaLock", 0) == 1;
        _trophies[1,0] = PlayerPrefs.GetInt("trophySlayer", 0) == 1;
        _trophies[1,1] = PlayerPrefs.GetInt("trophySlayerLock", 0) == 1;
        _trophies[2,0] = PlayerPrefs.GetInt("trophyPacifist", 1) == 1;
        _trophies[2,1] = PlayerPrefs.GetInt("trophyPacifistLock", 0) == 1;
        _trophies[3,0] = PlayerPrefs.GetInt("trophyPlunderer", 0) == 1;
        _trophies[3,1] = PlayerPrefs.GetInt("trophyPlundererLock", 0) == 1;
        _trophies[4,0] = PlayerPrefs.GetInt("trophyGhost", 1) == 1;
        _trophies[4,1] = PlayerPrefs.GetInt("trophyGhostLock", 0) == 1;
        _trophies[5,0] = PlayerPrefs.GetInt("trophyRaider", 1) == 1;
        _trophies[5,1] = PlayerPrefs.GetInt("trophyRaiderLock", 0) == 1;
        // // ads
        // AdBanner = PlayerPrefs.GetInt("adBanner", 0);
        // AdPopup = PlayerPrefs.GetInt("adPopup", 0);
        // AdVideo = PlayerPrefs.GetInt("adVideo", 0);
        // 
        // print("Data Load");
    }
    private void DataSave(Vector2Int room)
    {
        // prevent save in boss room(s)
        if (room.x > -2 && room.x < 2 && room.y > -10 && room.y < -6) return;
        // update direction
        _direction = room - _room;
        // update room
        _room = room;
        // update ticks
        _ticks = GameClock.Ticks;
        // // 
        // _difficulty = GameClock.Difficulty;
        // PlayerPrefs.SetInt("difficulty", _difficulty);
        // _theme = UISettings.Theme;
        // PlayerPrefs.SetInt("theme", _theme);
        // _vibration = UISettings.Vibration;
        // PlayerPrefs.SetInt("vibration", _vibration);
        // trophies
        PlayerPrefs.SetInt("trophyNinja", _trophies[0,0] ? 1 : 0);
        PlayerPrefs.SetInt("trophyNinjaLock", _trophies[0,1] ? 1 : 0);
        PlayerPrefs.SetInt("trophySlayer", _trophies[1,0] ? 1 : 0);
        PlayerPrefs.SetInt("trophySlayerLock", _trophies[1,1] ? 1 : 0);
        PlayerPrefs.SetInt("trophyPacifist", _trophies[2,0] ? 1 : 0);
        PlayerPrefs.SetInt("trophyPacifistLock", _trophies[2,1] ? 1 : 0);
        PlayerPrefs.SetInt("trophyPlunderer", _trophies[3,0] ? 1 : 0);
        PlayerPrefs.SetInt("trophyPlundererLock", _trophies[3,1] ? 1 : 0);
        PlayerPrefs.SetInt("trophyGhost", _trophies[4,0] ? 1 : 0);
        PlayerPrefs.SetInt("trophyGhostLock", _trophies[4,1] ? 1 : 0);
        PlayerPrefs.SetInt("trophyRaider", _trophies[5,0] ? 1 : 0);
        PlayerPrefs.SetInt("trophyRaiderLock", _trophies[5,1] ? 1 : 0);
        // check if save file exists
        // take data from variables and store into file
        // // * testing with playerPrefs
        // PlayerPrefs.SetInt("roomX", _room.x);
        // PlayerPrefs.SetInt("roomY", _room.y);
        // foreach (KeyValuePair<string, EntityData> entity in _entities)
        // {
        //     switch (entity.value.Type)
        //     {
        //         case EntityType.BREAKABLE:
        //             PlayerPrefs.SetInt(entity.key + "health", (entity.value as DataBreakable).Health);
        //             break;
        //     }
        // }
        // 
        string save = _room.x + DATA_SEPARATOR + _room.y + DATA_SEPARATOR + _direction.x + DATA_SEPARATOR + _direction.y + DATA_SEPARATOR + _ticks;
        foreach (EntityData data in _entities.Values)
            // // * testing readability
            // save += DATA_SEPARATOR + JsonUtility.ToJson(data, true);
            save += DATA_SEPARATOR + JsonUtility.ToJson(data);
        File.WriteAllText(_filePath, save);
        // print("save: " + save);
        // 
        // print("Data Save");
    }
    private void DoClear(Vector2Int room)
    {
        // 
        SetTrophy(Trophy.RAIDER);
        // 
        DataSave(room);
        // only counted for last room
        if (_ticks < _ticksBest)
        {
            _ticksBest = _ticks;
            PlayerPrefs.SetInt("ticksBest", _ticksBest);
        }
        // 
        // print("Do Clear");
    }
    #region Entity
    // [Serializable]
    protected class EntityData
    {
        [SerializeField] protected EntityType _type;
        public string _id;
        public Vector3 Position;
        public Vector3 Rotation;
        public bool IsActive;
        public string LootID;
        public EntityData(string id, Vector3 position, Vector3 rotation, bool isActive, string lootID)
        {
            _type = EntityType.DEFAULT;
            _id = id;
            Position = position;
            Rotation = rotation;
            IsActive = isActive;
            LootID = lootID;
        }
        // properties since these don't change
        public EntityType Type
        {
            get { return _type; }
        }
        public string ID
        {
            get { return _id; }
        }
    }
    // ? being called before awake as seen with null error on initialize entities dictionary inside awake
    public static bool DataLoadEntity(string id, out Vector3 position, out Vector3 rotation, out bool isActive, out string lootID)
    {
        // if (id == null) print(position);
        // entity data exists
        if (_entities.ContainsKey(id))
        {
            // print("Data Load: " + id + " @ " + Time.realtimeSinceStartup);
            // placeholder
            EntityData data;
            // get data ? try catch
            _entities.TryGetValue(id, out data);
            // parse data
            position = data.Position;
            rotation = data.Rotation;
            isActive = data.IsActive;
            lootID = data.LootID;
            // success
            return true;
        }
        // entity data does not exist
        position = Vector3.zero;
        rotation = Vector3.zero;
        isActive = true;
        lootID = "";
        // failure
        return false;
    }
    public static void DataSaveEntity(string id, Vector3 position, Vector3 rotation, bool isActive, string lootID)
    {
        // print("Data Save: " + id + " @ " + Time.realtimeSinceStartup);
        // placeholder
        EntityData data = null;
        // check if entity data exists
        if (_entities.ContainsKey(id))
        {
            // retrieve entity entry ? use try catch
            _entities.TryGetValue(id, out data);
            // update data
            data.Position = position;
            data.Rotation = rotation;
            data.IsActive = isActive;
            data.LootID = lootID;
        }
        // entity data not found
        else
        {
            // create new entry
            data = new EntityData(id, position, rotation, isActive, lootID);
            // store entity data
            _entities.Add(id, data);
        }
    }
    #endregion
    #region Breakable
    // [Serializable]
    private class DataBreakable : EntityData
    {
        public int Health;
        public DataBreakable(string id, Vector3 position, Vector3 rotation, bool isActive, string lootID, int health) : base(id, position, rotation, isActive, lootID)
        {
            _type = EntityType.BREAKABLE;
            Health = health;
        }
    }
    // ? being called before awake as seen with null error on initialize entities dictionary inside awake
    public static bool DataLoadBreakable(string id, out Vector3 position, out Vector3 rotation, out bool isActive, out string lootID, out int health)
    {
        // entity data exists
        if (_entities.ContainsKey(id))
        {
            // print("Data Load: " + id + " @ " + Time.realtimeSinceStartup);
            // placeholder
            EntityData data;
            // get data ? try catch
            _entities.TryGetValue(id, out data);
            // parse data
            position = data.Position;
            rotation = data.Rotation;
            isActive = data.IsActive;
            lootID = data.LootID;
            health = (data as DataBreakable).Health;
            // success
            return true;
        }
        // entity data does not exist
        position = Vector3.zero;
        rotation = Vector3.zero;
        isActive = true;
        lootID = "";
        health = 1;
        // failure
        return false;
    }
    public static void DataSaveBreakable(string id, Vector3 position, Vector3 rotation, bool isActive, string lootID, int health)
    {
        // print("Data Save: " + id + " @ " + Time.realtimeSinceStartup);
        // placeholder
        EntityData data = null;
        // check if entity data exists
        if (_entities.ContainsKey(id))
        {
            // retrieve entity entry ? use try catch
            _entities.TryGetValue(id, out data);
            // update data
            data.Position = position;
            data.Rotation = rotation;
            data.IsActive = isActive;
            data.LootID = lootID;
            (data as DataBreakable).Health = health;
        }
        // entity data not found
        else
        {
            // create new entry
            data = new DataBreakable(id, position, rotation, isActive, lootID, health);
            // store entity data
            _entities.Add(id, data);
        }
    }
    #endregion
    #region Creature
    // [Serializable]
    private class DataCreature : DataBreakable
    {
        public string[] Inventory;
        public DataCreature(string id, Vector3 position, Vector3 rotation, bool isActive, string lootID, int health, string[] inventory) : base(id, position, rotation, isActive, lootID, health)
        {
            _type = EntityType.CREATURE;
            Inventory = inventory;
        }
    }
    // ? being called before awake as seen with null error on initialize entities dictionary inside awake
    public static bool DataLoadCreature(string id, out Vector3 position, out Vector3 rotation, out bool isActive, out string lootID, out int health, out string[] inventory)
    {
        // entity data exists
        if (_entities.ContainsKey(id))
        {
            // print("Data Load: " + id + " @ " + Time.realtimeSinceStartup);
            // placeholder
            EntityData data;
            // get data ? try catch
            _entities.TryGetValue(id, out data);
            // parse data
            position = data.Position;
            rotation = data.Rotation;
            isActive = data.IsActive;
            lootID = data.LootID;
            health = (data as DataBreakable).Health;
            inventory = (data as DataCreature).Inventory;
            // success
            return true;
        }
        // entity data does not exist
        position = Vector3.zero;
        rotation = Vector3.zero;
        isActive = true;
        lootID = "";
        health = 1;
        inventory = null;
        // failure
        return false;
    }
    public static void DataSaveCreature(string id, Vector3 position, Vector3 rotation, bool isActive, string lootID, int health, string[] inventory)
    {
        // print("Data Save: " + id + " @ " + Time.realtimeSinceStartup);
        // placeholder
        EntityData data = null;
        // check if entity data exists
        if (_entities.ContainsKey(id))
        {
            // retrieve entity entry ? use try catch
            _entities.TryGetValue(id, out data);
            // update data
            data.Position = position;
            data.Rotation = rotation;
            data.IsActive = isActive;
            data.LootID = lootID;
            (data as DataBreakable).Health = health;
            (data as DataCreature).Inventory = inventory;
        }
        // entity data not found
        else
        {
            // create new entry
            data = new DataCreature(id, position, rotation, isActive, lootID, health, inventory);
            // store entity data
            _entities.Add(id, data);
        }
    }
    #endregion
    #region Interact
    // [Serializable]
    private class DataInteract : EntityData
    {
        public bool State;
        public bool Locked;
        public DataInteract(string id, Vector3 position, Vector3 rotation, bool isActive, string lootID, bool state, bool locked) : base(id, position, rotation, isActive, lootID)
        {
            _type = EntityType.INTERACT;
            State = state;
            Locked = locked;
        }
    }
    // ? being called before awake as seen with null error on initialize entities dictionary inside awake
    public static bool DataLoadInteract(string id, out Vector3 position, out Vector3 rotation, out bool isActive, out string lootID, out bool state, out bool locked)
    {
        // entity data exists
        if (_entities.ContainsKey(id))
        {
            // print("Data Load: " + id + " @ " + Time.realtimeSinceStartup);
            // placeholder
            EntityData data;
            // get data ? try catch
            _entities.TryGetValue(id, out data);
            // parse data
            position = data.Position;
            rotation = data.Rotation;
            isActive = data.IsActive;
            lootID = data.LootID;
            state = (data as DataInteract).State;
            locked = (data as DataInteract).Locked;
            // success
            return true;
        }
        // entity data does not exist
        position = Vector3.zero;
        rotation = Vector3.zero;
        isActive = true;
        lootID = "";
        state = true;
        locked = true;
        // failure
        return false;
    }
    public static void DataSaveInteract(string id, Vector3 position, Vector3 rotation, bool isActive, string lootID, bool state, bool locked)
    {
        // print("Data Save: " + id + " @ " + Time.realtimeSinceStartup);
        // placeholder
        EntityData data = null;
        // check if entity data exists
        if (_entities.ContainsKey(id))
        {
            // retrieve entity entry ? use try catch
            _entities.TryGetValue(id, out data);
            // update data
            data.Position = position;
            data.Rotation = rotation;
            data.IsActive = isActive;
            data.LootID = lootID;
            (data as DataInteract).State = state;
            (data as DataInteract).Locked = locked;
        }
        // entity data not found
        else
        {
            // create new entry
            data = new DataInteract(id, position, rotation, isActive, lootID, state, locked);
            // store entity data
            _entities.Add(id, data);
        }
    }
    #endregion
    #region React
    // [Serializable]
    private class DataReact : EntityData
    {
        public bool State;
        public bool[] Signal;
        public DataReact(string id, Vector3 position, Vector3 rotation, bool isActive, string lootID, bool state, bool[] signal) : base(id, position, rotation, isActive, lootID)
        {
            _type = EntityType.REACT;
            State = state;
            Signal = signal;
        }
    }
    // ? being called before awake as seen with null error on initialize entities dictionary inside awake
    public static bool DataLoadReact(string id, out Vector3 position, out Vector3 rotation, out bool isActive, out string lootID, out bool state, out bool[] signal)
    {
        // entity data exists
        if (_entities.ContainsKey(id))
        {
            // print("Data Load: " + id + " @ " + Time.realtimeSinceStartup);
            // print("Data Load: " + id + " React");
            // placeholder
            EntityData data;
            // get data ? try catch
            _entities.TryGetValue(id, out data);
            // parse data
            position = data.Position;
            rotation = data.Rotation;
            isActive = data.IsActive;
            lootID = data.LootID;
            state = (data as DataReact).State;
            signal = (data as DataReact).Signal;
            // success
            return true;
        }
        // entity data does not exist
        position = Vector3.zero;
        rotation = Vector3.zero;
        isActive = true;
        lootID = "";
        state = true;
        signal = null;
        // failure
        return false;
    }
    public static void DataSaveReact(string id, Vector3 position, Vector3 rotation, bool isActive, string lootID, bool state, bool[] signal)
    {
        // print("Data Save: " + id + " @ " + Time.realtimeSinceStartup);
        // print("Data Save: " + id + " React");
        // placeholder
        EntityData data = null;
        // check if entity data exists
        if (_entities.ContainsKey(id))
        {
            // retrieve entity entry ? use try catch
            _entities.TryGetValue(id, out data);
            // update data
            data.Position = position;
            data.Rotation = rotation;
            data.IsActive = isActive;
            data.LootID = lootID;
            (data as DataReact).State = state;
            (data as DataReact).Signal = signal;
        }
        // entity data not found
        else
        {
            // create new entry
            data = new DataReact(id, position, rotation, isActive, lootID, state, signal);
            // store entity data
            _entities.Add(id, data);
        }
    }
    #endregion
    #region ReactSequence
    // [Serializable]
    private class DataReactSequence : DataReact
    {
        public CachePing[] Cache;
        public int Next;
        public DataReactSequence(string id, Vector3 position, Vector3 rotation, bool isActive, string lootID, bool state, bool[] signal, CachePing[] cache, int next) : base(id, position, rotation, isActive, lootID, state, signal)
        {
            _type = EntityType.REACT_SEQUENCE;
            Cache = cache;
            Next = next;
        }
    }
    // ? being called before awake as seen with null error on initialize entities dictionary inside awake
    public static bool DataLoadReactSequence(string id, out Vector3 position, out Vector3 rotation, out bool isActive, out string lootID, out bool state, out bool[] signal, out CachePing[] cache, out int next)
    {
        // entity data exists
        if (_entities.ContainsKey(id))
        {
            // print("Data Load: " + id + " ReactSequence");
            // placeholder
            EntityData data;
            // get data ? try catch
            _entities.TryGetValue(id, out data);
            // parse data
            position = data.Position;
            rotation = data.Rotation;
            isActive = data.IsActive;
            lootID = data.LootID;
            state = (data as DataReact).State;
            signal = (data as DataReact).Signal;
            cache = (data as DataReactSequence).Cache;
            next = (data as DataReactSequence).Next;
            // success
            return true;
        }
        // entity data does not exist
        position = Vector3.zero;
        rotation = Vector3.zero;
        isActive = true;
        lootID = "";
        state = true;
        signal = null;
        cache = null;
        next = 0;
        // failure
        return false;
    }
    public static void DataSaveReactSequence(string id, Vector3 position, Vector3 rotation, bool isActive, string lootID, bool state, bool[] signal, CachePing[] cache, int next)
    {
        // print("Data Save: " + id + " ReactSequence");
        // placeholder
        EntityData data = null;
        // check if entity data exists
        if (_entities.ContainsKey(id))
        {
            // retrieve entity entry ? use try catch
            _entities.TryGetValue(id, out data);
            // update data
            data.Position = position;
            data.Rotation = rotation;
            data.IsActive = isActive;
            data.LootID = lootID;
            (data as DataReact).State = state;
            (data as DataReact).Signal = signal;
            (data as DataReactSequence).Cache = cache;
            (data as DataReactSequence).Next = next;
        }
        // entity data not found
        else
        {
            // create new entry
            data = new DataReactSequence(id, position, rotation, isActive, lootID, state, signal, cache, next);
            // store entity data
            _entities.Add(id, data);
        }
    }
    #endregion
    // #region ItemHeal
    // // [Serializable]
    // private class DataItemHeal : EntityData
    // {
    //     public int Count;
    //     public DataItemHeal(string id, Vector3 position, Vector3 rotation, bool isActive, string lootID, int count) : base(id, position, rotation, isActive, lootID)
    //     {
    //         _type = EntityType.ITEM_HEAL;
    //         Count = count;
    //     }
    // }
    // // ? being called before awake as seen with null error on initialize entities dictionary inside awake
    // public static bool DataLoadItemHeal(string id, out Vector3 position, out Vector3 rotation, out bool isActive, out string lootID, out int count)
    // {
    //     // entity data exists
    //     if (_entities.ContainsKey(id))
    //     {
    //         // print("Data Load: " + id + " @ " + Time.realtimeSinceStartup);
    //         // placeholder
    //         EntityData data;
    //         // get data ? try catch
    //         _entities.TryGetValue(id, out data);
    //         // parse data
    //         position = data.Position;
    //         rotation = data.Rotation;
    //         isActive = data.IsActive;
    //         lootID = data.LootID;
    //         count = (data as DataItemHeal).Count;
    //         // success
    //         return true;
    //     }
    //     // entity data does not exist
    //     position = Vector3.zero;
    //     rotation = Vector3.zero;
    //     isActive = true;
    //     lootID = "";
    //     count = 1;
    //     // failure
    //     return false;
    // }
    // public static void DataSaveItemHeal(string id, Vector3 position, Vector3 rotation, bool isActive, string lootID, int count)
    // {
    //     // print("Data Save: " + id + " @ " + Time.realtimeSinceStartup);
    //     // placeholder
    //     EntityData data = null;
    //     // check if entity data exists
    //     if (_entities.ContainsKey(id))
    //     {
    //         // retrieve entity entry ? use try catch
    //         _entities.TryGetValue(id, out data);
    //         // update data
    //         data.Position = position;
    //         data.Rotation = rotation;
    //         data.IsActive = isActive;
    //         data.LootID = lootID;
    //         (data as DataItemHeal).Count = count;
    //     }
    //     // entity data not found
    //     else
    //     {
    //         // create new entry
    //         data = new DataItemHeal(id, position, rotation, isActive, lootID, count);
    //         // store entity data
    //         _entities.Add(id, data);
    //     }
    // }
    // #endregion
    #region ItemConsume
    // [Serializable]
    private class DataItemConsume : EntityData
    {
        public int Count;
        public DataItemConsume(string id, Vector3 position, Vector3 rotation, bool isActive, string lootID, int count) : base(id, position, rotation, isActive, lootID)
        {
            _type = EntityType.ITEM_CONSUME;
            Count = count;
        }
    }
    // ? being called before awake as seen with null error on initialize entities dictionary inside awake
    public static bool DataLoadItemConsume(string id, out Vector3 position, out Vector3 rotation, out bool isActive, out string lootID, out int count)
    {
        // entity data exists
        if (_entities.ContainsKey(id))
        {
            // print("Data Load: " + id + " @ " + Time.realtimeSinceStartup);
            // placeholder
            EntityData data;
            // get data ? try catch
            _entities.TryGetValue(id, out data);
            // parse data
            position = data.Position;
            rotation = data.Rotation;
            isActive = data.IsActive;
            lootID = data.LootID;
            count = (data as DataItemConsume).Count;
            // success
            return true;
        }
        // entity data does not exist
        position = Vector3.zero;
        rotation = Vector3.zero;
        isActive = true;
        lootID = "";
        count = 1;
        // failure
        return false;
    }
    public static void DataSaveItemConsume(string id, Vector3 position, Vector3 rotation, bool isActive, string lootID, int count)
    {
        // print("Data Save: " + id + " @ " + Time.realtimeSinceStartup);
        // placeholder
        EntityData data = null;
        // check if entity data exists
        if (_entities.ContainsKey(id))
        {
            // retrieve entity entry ? use try catch
            _entities.TryGetValue(id, out data);
            // update data
            data.Position = position;
            data.Rotation = rotation;
            data.IsActive = isActive;
            data.LootID = lootID;
            (data as DataItemConsume).Count = count;
        }
        // entity data not found
        else
        {
            // create new entry
            data = new DataItemConsume(id, position, rotation, isActive, lootID, count);
            // store entity data
            _entities.Add(id, data);
        }
    }
    #endregion
}