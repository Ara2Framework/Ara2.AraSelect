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
    public class AraSelectListItem
    {
        public string Key;
        public string Caption;
        public bool Select;
        public int index = 0;
        AraSelect MySelect;

        public AraSelectListItem(AraSelect vMySelect, string vKey, string vCaption, bool vSelect, int vIndex)
        {
            index = vIndex;
            MySelect = vMySelect;
            Key = vKey;
            Caption = vCaption;
            Select = vSelect;

            
            MySelect.TickScriptCall();
            Tick.GetTick().Script.Send(" vObj.ListAdd('" + index + "','" + AraTools.StringToStringJS(Caption) + "'," + (Select == true ? "true" : "false") + "); \n");
        }

    }
}
