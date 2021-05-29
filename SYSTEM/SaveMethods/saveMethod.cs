using PixelCrushers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Player
{
    

    public class saveMethod : Saver
    {
        [SerializeField]
        public InventoryObject Inventory;
        [SerializeField]
        public InventoryObject Equipment;
        [SerializeField]
        public AbilityInventoryObject Abilities;

       // [Serializable]
       // public class MultiscenePositionData
       // {
       //     public List<ScenePositionData> positions = new List<ScenePositionData>();
       // }

      //  protected bool multiscene { get { return m_multiscene; } }
      //  [SerializeField]
     //   private bool m_multiscene = false;

      //  public string path;
      //  public string path2;
      //  public string path3;
      //  public void Save(InventoryObject Container, string savePath)
     //   {
            // string saveData = JsonUtility.ToJson(this, true);
            //  BinaryFormatter bf = new BinaryFormatter();
            //  FileStream file = File.Create(string.Concat(Application.persistentDataPath, savePath));
            //  PixelCrushers.SaveSystem.Serialize(file);
              //file.Close();
    //        IFormatter formatter = new BinaryFormatter(); //bf.Serialize
     //        Stream stream = new FileStream(string.Concat(Application.persistentDataPath, savePath), FileMode.Create, FileAccess.Write);
     //        formatter.Serialize(stream, Container);
    //         stream.Close();
    //    }
     //   public void Save(AbilityInventoryObject Container, string savePath)
     //   {
           // string saveData = JsonUtility.ToJson(this, true);
           // BinaryFormatter bf = new BinaryFormatter();
           // FileStream file = File.Create(string.Concat(Application.persistentDataPath, savePath));
           // PixelCrushers.SaveSystem.Serialize(file);
           // file.Close();
      //     IFormatter formatter = new BinaryFormatter(); //bf.Serialize
      //      Stream stream = new FileStream(string.Concat(Application.persistentDataPath, savePath), FileMode.Create, FileAccess.Write);
      //       formatter.Serialize(stream, Container);
      //       stream.Close();
      //  }
       // [ContextMenu("Load")]
      //  public void Load(InventoryObject Container, string savePath)
      //  {
        //    if (File.Exists(string.Concat(Application.persistentDataPath, savePath)))
        //    {
        //          BinaryFormatter bf = new BinaryFormatter();
        //          FileStream file = File.Open(string.Concat(Application.persistentDataPath, savePath), FileMode.Open);
        //        JsonUtility.FromJsonOverwrite(bf.Deserialize(file).ToString(),this);
                //PixelCrushers.SaveSystem.DeSerialize(file);
        //        file.Close();
                //IFormatter formatter = new BinaryFormatter();
               // Stream stream = new FileStream(string.Concat(Application.persistentDataPath, savePath), FileMode.Open, FileAccess.Read);
               // Inventory newContainer = (Inventory)formatter.Deserialize(stream);
               // for (int i = 0; i < Container.items.Length; i++)
               // {
               //     Container.items[i].UpdateSlot(newContainer.items[i].ID, newContainer.items[i].item, newContainer.items[i].amount);
               // }
               // stream.Close();
       //     }
     //   }
       // public void Load(AbilityInventoryObject Container, string savePath)
       // {
        //    if (File.Exists(string.Concat(Application.persistentDataPath, savePath)))
        //    {
           //     BinaryFormatter bf = new BinaryFormatter();
          ///      FileStream file = File.Open(string.Concat(Application.persistentDataPath, savePath), FileMode.Open);
          //      JsonUtility.FromJsonOverwrite(bf.Deserialize(file).ToString(),this);
                //PixelCrushers.SaveSystem.DeSerialize(file);
          //      file.Close();
                //IFormatter formatter = new BinaryFormatter();
                // Stream stream = new FileStream(string.Concat(Application.persistentDataPath, savePath), FileMode.Open, FileAccess.Read);
                // Inventory newContainer = (Inventory)formatter.Deserialize(stream);
                // for (int i = 0; i < Container.items.Length; i++)
                // {
                //     Container.items[i].UpdateSlot(newContainer.items[i].ID, newContainer.items[i].item, newContainer.items[i].amount);
                // }
                // stream.Close();
          //  }
      //  }
        // Start is called before the first frame update
      //  void Start()
      //  {
           // Save(Inventory, path);
           // Load(Inventory, path);
       // }

        // Update is called once per frame
       // void Update()
       // {
            //if (Input.GetKeyDown(KeyCode.J))
           // {
                
               // Save(Abilities, path2);
               // Save(Equipment, path3);
          //  }
           // if (Input.GetKeyDown(KeyCode.K))
          //  {
            //    Load(Inventory, path);
              //  Load(Abilities, path2);
               // Load(Equipment, path3);
           // }
       // }

        public override string RecordData()
        {
           // Save(Inventory, path);
            //throw new System.NotImplementedException();


            var currentScene = SceneManager.GetActiveScene().buildIndex;
            
                return SaveSystem.Serialize(Inventory);
            
        }

        public override void ApplyData(string s)
        {
            SaveSystem.Deserialize(s,Inventory);
        }
    }
}