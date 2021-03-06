﻿///************************************************************************
//Project Lorule: A Dark Ages Server (http://darkages.creatorlink.net/index/)
//Copyright(C) 2018 TrippyInc Pty Ltd
//
//This program is free software: you can redistribute it and/or modify
//it under the terms of the GNU General Public License as published by
//the Free Software Foundation, either version 3 of the License, or
//(at your option) any later version.
//
//This program is distributed in the hope that it will be useful,
//but WITHOUT ANY WARRANTY; without even the implied warranty of
//MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
//GNU General Public License for more details.
//
//You should have received a copy of the GNU General Public License
//along with this program.If not, see<http://www.gnu.org/licenses/>.
//*************************************************************************/
using Darkages.Types;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Darkages.Network.Object
{
    [DataContract]
    public class SpriteCollection<T> : IEnumerable<T>
        where T: Sprite
    {
        public readonly List<T> Values = null;

        public SpriteCollection(IEnumerable<T> values)
        {
            Values = new List<T>(values);
        }

        public T Query(Predicate<T> predicate)
        {
            for (int i = Values.Count - 1; i >= 0; i--)
            {
                var subject = predicate(Values[i]);

                if (subject)
                {
                    return Values[i];
                }
            }

            return default(T);
        }

        public IEnumerable<T> QueryAll(Predicate<T> predicate)
        {
            for (int i = Values.Count - 1; i >= 0; i--)
            {
                var subject = predicate(Values[i]);

                if (subject)
                {
                    yield return Values[i];
                }
            }
        }

        public void Add(T obj)
        {
            Values.Add(obj);
        }

        public void Delete(T obj)
        {
            for (int i = Values.Count - 1; i >= 0; i--)
            {
                var subject   = (obj as Sprite);
                var predicate = (Values[i] as Sprite);

                if (subject == predicate)
                {
                    Values.RemoveAt(i);
                }
            }
        }

        public IEnumerator<T> GetEnumerator()   => Values.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    public sealed class ObjectService 
    {
        private readonly IDictionary<Type, object> _spriteCollections = new Dictionary<Type, object>
        {
            [typeof(Aisling)]  = new SpriteCollection<Aisling>(Enumerable.Empty<Aisling>()),
            [typeof(Monster)]  = new SpriteCollection<Monster>(Enumerable.Empty<Monster>()),
            [typeof(Mundane)]  = new SpriteCollection<Mundane>(Enumerable.Empty<Mundane>()),
            [typeof(Money)]    = new SpriteCollection<Money>(Enumerable.Empty<Money>()),
            [typeof(Item)]     = new SpriteCollection<Item>(Enumerable.Empty<Item>()),
        };

        public T Query<T>(Predicate<T> predicate) where T: Sprite
        {
            var obj = (SpriteCollection<T>)_spriteCollections[typeof(T)];
            var queryResult = obj.Query(predicate);
            {
                return queryResult;
            }
        }

        public IEnumerable<T> QueryAll<T>(Predicate<T> predicate) where T: Sprite
        {
            var obj = (SpriteCollection<T>)_spriteCollections[typeof(T)];
            var queryResult = obj.QueryAll(predicate);
            {
                return queryResult;
            }
        }

        public void RemoveAllGameObjects<T>(T[] objects) where T : Sprite
        {
            if (objects == null)
                return;

            for (uint i = 0; i < objects.Length; i++)
                RemoveGameObject(objects[i]);
        }

        public void AddGameObject<T>(T obj) where T : Sprite
        {
            var objCollection = (SpriteCollection<T>)_spriteCollections[typeof(T)];
            objCollection.Add(obj);
        }

        public void RemoveGameObject<T>(T obj) where T : Sprite
        {
            var objCollection = (SpriteCollection<T>)_spriteCollections[typeof(T)];
            objCollection.Delete(obj);
        }
    }
}