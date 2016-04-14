// Copyright (c) 2010-2016, Rafael Leonel Pontani. All rights reserved.
// For licensing, see LICENSE.md or http://www.araframework.com.br/license
// This file is part of AraFramework project details visit http://www.arafrework.com.br
// AraFramework - Rafael Leonel Pontani, 2016-4-14
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ara2.Components.Select
{
    [Serializable]
    public class AraSelectList
    {
        List<AraSelectListItem> MyList = new List<AraSelectListItem>();

        AraSelect MySelect;
        public AraSelectList(AraSelect vSelect)
        {
            MySelect = vSelect;
        }

        public AraSelectListItem this[string key]
        {
            get
            {
                return MyList.Find(a => a.Key == key);
            }
        }

        public AraSelectListItem this[int index]
        {
            get
            {
                return MyList.Find(a => a.index == index);
            }
        }

        public AraSelectListItem GetByPosition(int vPosition)
        {
            return MyList[vPosition];
        }


        public void Add(object key, object value)
        {
            this.Add(Convert.ToString(key), Convert.ToString(value));
        }

        public void Add(string key, string value)
        {
            this.Add(key, value, false);
        }

        int vNewCodItem = 0;
        public void Add(string key, string value, bool vSelect)
        {
            MyList.Add(new AraSelectListItem(MySelect, key, value, vSelect, vNewCodItem));
            vNewCodItem++;
        }

        public int Count
        {
            get
            {
                return MyList.Count;
            }
        }

        public void Clear()
        {
            MyList = new List<AraSelectListItem>();

            MySelect.TickScriptCall();
            Tick.GetTick().Script.Send(" vObj.Clear(); \n");
        }

        public bool Contains(string key)
        {
            return this[key] != null;
        }

        public void Remove(string key)
        {
            if (MySelect.Text == key)
                MySelect.Text = "";

            int vIndex = this[key].index;
            MyList.Remove(this[key]);

            
            MySelect.TickScriptCall();
            Tick.GetTick().Script.Send(" vObj.Remove(" + vIndex + "); \n");
        }

        public AraSelectListItem[] ToArray()
        {
            return MyList.ToArray();
        }

    }
}
