﻿// Copyright (c) Pixel Crushers. All rights reserved.

using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace PixelCrushers
{

    /// <summary>
    /// Implementation of DataSerializer that uses BinaryFormatter.
    /// </summary>
    [AddComponentMenu("")] // Use wrapper instead.
    public class BinaryDataSerializer : DataSerializer
    {

        protected virtual void AddSurrogateSelectors(SurrogateSelector surrogateSelector)
        {
            surrogateSelector.AddSurrogate(typeof(Vector3), new StreamingContext(StreamingContextStates.All), new Vector3SerializationSurrogate());
            surrogateSelector.AddSurrogate(typeof(Quaternion), new StreamingContext(StreamingContextStates.All), new QuaternionSerializationSurrogate());
        }

        protected virtual BinaryFormatter CreateBinaryFormatter()
        {
            var binaryFormatter = new BinaryFormatter();
            var surrogateSelector = new SurrogateSelector();
            AddSurrogateSelectors(surrogateSelector);
            binaryFormatter.SurrogateSelector = surrogateSelector;
            return binaryFormatter;
        }

        public override string Serialize(object data)
        {
            if (data == null || !data.GetType().IsSerializable) return string.Empty;
            using (var stream = new MemoryStream())
            {
                var binaryFormatter = CreateBinaryFormatter();
                binaryFormatter.Serialize(stream, data);
                return Convert.ToBase64String(stream.ToArray());
            }
        }

        public override T Deserialize<T>(string s, T data = default(T))
        {
            if (string.IsNullOrEmpty(s)) return default(T);
            var bytes = Convert.FromBase64String(s);
            using (var stream = new MemoryStream(bytes))
            {
                var binaryFormatter = CreateBinaryFormatter();
                return (T) binaryFormatter.Deserialize(stream);
            }
        }

    }

}
