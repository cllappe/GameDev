using System;
using UnityEngine;

//namespace PixelCrushers
//{
//    public class HealthSaver : Saver
//    {
//        [Serializable]
//        public class DestructibleData
//        {
//            public bool destroyed = false;
//            public Vector3 position;
//        }
//        private DestructibleData m_data = new DestructibleData();

//        public override string RecordData()
//        {
//            return SaveSystem.Serialize(m_data);
//        }

//        public override void ApplyData(string s)
//        {
//            var data = SaveSystem.Deserialize<DestructibleData>(s, m_data);
//            if (data == null) return;
//            m_data = data;


//        }
//    }
//}
