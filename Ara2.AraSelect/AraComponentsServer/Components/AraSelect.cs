// Copyright (c) 2010-2016, Rafael Leonel Pontani. All rights reserved.
// For licensing, see LICENSE.md or http://www.araframework.com.br/license
// This file is part of AraFramework project details visit http://www.arafrework.com.br
// AraFramework - Rafael Leonel Pontani, 2016-4-14
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Ara2.Components.Select;
using Ara2.Dev;

namespace Ara2.Components
{
    [Serializable]
    [AraDevComponent(vConteiner:false,vResizable:false)]
    public class AraSelect : AraComponentVisualAnchorConteiner, IAraDev
    {
        public AraSelect(IAraContainerClient ConteinerFather)
            : base(AraObjectClienteServer.Create(ConteinerFather, "select"), ConteinerFather, "AraSelect")
        {
            List = new AraSelectList(this);

            Click = new AraComponentEvent<EventHandler>(this, "Click");
            Focus = new AraComponentEvent<EventHandler>(this, "Focus");
            LostFocus = new AraComponentEvent<EventHandler>(this, "LostFocus");
            Change = new AraComponentEvent<EventHandler>(this, "Change");

            KeyDown = new AraComponentEvent<Key_delegate>(this, "KeyDown");
            KeyUp = new AraComponentEvent<Key_delegate>(this, "KeyUp");
            KeyPress = new AraComponentEvent<Key_delegate>(this, "KeyPress");

            this.EventInternal += AraSelect_EventInternal;
            this.SetProperty += SetValueJS;

            this.MinWidth = 10;
            this.MinHeight = 20;
            this.Width = 110;
            this.Height = 17;
        }

        public override void LoadJS()
        {
            Tick vTick = Tick.GetTick();
            vTick.Session.AddJs("Ara2/Components/AraSelect/AraSelect.js");
        }

        public AraSelectList List ;

        public void AraSelect_EventInternal(String vFunction)
        {
            Tick vTick = Tick.GetTick();
            int vKey;
            switch (vFunction.ToUpper())
            {
                case "FOCUS":
                    Focus.InvokeEvent(this, new EventArgs());
                    break;
                case "LOSTFOCUS":
                    LostFocus.InvokeEvent(this, new EventArgs());
                    break;
                case "KEYDOWN":
                    vKey = Convert.ToInt16(vTick.Page.Request["KeyDown"]);
                    KeyDown.InvokeEvent(this, vKey);
                    break;
                case "KEYUP":
                    vKey = Convert.ToInt16(vTick.Page.Request["KeyUp"]);
                    KeyDown.InvokeEvent(this, vKey);
                    break;
                case "KEYPRESS":
                    vKey = Convert.ToInt16(vTick.Page.Request["KeyPress"]);
                    KeyDown.InvokeEvent(this, vKey);
                    break;
                case "CHANGE":
                    Change.InvokeEvent(this, new EventArgs());
                    break;
                case "CLICK":
                    Click.InvokeEvent(this, new EventArgs());
                    break;
            }
        }

        public void SetValueJS(String vCommand, dynamic vValue)
        {
            Tick vTick = Tick.GetTick();

            switch (vCommand.ToUpper())
            {
                case "GETVALUE()":
                    try
                    {
                        if (!Multiple)
                        {
                            if (vValue != "" && List[Convert.ToInt32(vValue)] !=null)
                                _Text = List[Convert.ToInt32(vValue)].Key;
                            else
                                _Text = "";
                            _TextArray = null;
                        }
                        else
                        {
                            _Text = null;
                            List<string> TmpTextArray = new List<string>();

                            foreach (string vTmp in Json.DynamicJson.Parse(vValue))
                            {
                                TmpTextArray.Add(List[Convert.ToInt32(vTmp)].Key);
                            }
                            _TextArray = TmpTextArray.ToArray();
                        }
                    }
                    catch { }
                    break;
            }



        }

        private string _Width = "";
        private bool _Visible = true;
        string _Color_Focus = "";
        string _Color_LostFocus = "";
        string _ToolTip = "";

        #region Events
            public delegate void Key_delegate(AraSelect Object, int vKey);
        [AraDevEvent]
        public AraComponentEvent<EventHandler> Click;
        [AraDevEvent]
        public AraComponentEvent<EventHandler> Focus;
        [AraDevEvent]
        public AraComponentEvent<EventHandler> LostFocus;
        [AraDevEvent]
        public AraComponentEvent<EventHandler> Change;

        [AraDevEvent]
        public AraComponentEvent<Key_delegate> KeyDown;
        [AraDevEvent]
        public AraComponentEvent<Key_delegate> KeyUp;
        [AraDevEvent]
        public AraComponentEvent<Key_delegate> KeyPress;
        #endregion

        // Eventos Fim
        
        [AraDevProperty("")]
        public string Color_Focus
        {
            get { return _Color_Focus; }
            set
            {
                _Color_Focus = value;
                this.TickScriptCall();
                Tick.GetTick().Script.Send(" vObj.Color_Focus = '" + AraTools.StringToStringJS(_Color_Focus) + "'; \n");
            }
        }

        [AraDevProperty("")]
        public string Color_LostFocus
        {
            get { return _Color_LostFocus; }
            set
            {
                _Color_LostFocus = value;
                this.TickScriptCall();
                Tick.GetTick().Script.Send(" vObj.Color_LostFocus = '" + AraTools.StringToStringJS(_Color_LostFocus) + "'; \n");
            }
        }

        [AraDevProperty("")]
        public string ToolTip
        {
            get { return _ToolTip; }
            set
            {
                _ToolTip = value;
                this.TickScriptCall();
                Tick.GetTick().Script.Send(" vObj.SetToolTip('" + AraTools.StringToStringJS(_ToolTip) + "'); \n");
            }
        }

        public void Clear()
        {
            List.Clear();
        }

        int _Size;
        [AraDevProperty(0)]
        public int Size
        {
            get {
               return _Size;    
            }
            set
            {
                _Size=value;

                this.TickScriptCall();
                Tick.GetTick().Script.Send(" vObj.SetSize(" + _Size + "); \n");
            }
        }

        
        bool _Multiple=false;

        [AraDevProperty(false)]
        public bool Multiple
        {
            get {
               return _Multiple;    
            }
            set
            {
                _Multiple=value;

                this.TickScriptCall();
                Tick.GetTick().Script.Send(" vObj.SetMultiple(" + (_Multiple == true ? "true" : "false") + "); \n");
            }
        }

        public void SetFocus()
        {
            this.TickScriptCall();
            Tick.GetTick().Script.Send(" vObj.SetFocus(); \n");
        }

        private string _Text = "";
        [AraDevProperty("")]
        public string Text
        {
            set
            {
                if (!Multiple)
                {


                    if (value!="")
                        if (List[value] == null)
                            throw new Exception("Erro no Set Text do objecto '" + this.Name + "' pois o valor '" + value + "' não se encontra na lista.");



                    this.TickScriptCall();
                    if (value!="")
                        Tick.GetTick().Script.Send(" vObj.SetValue('" + AraTools.StringToStringJS(List[value].index.ToString()) + "'); \n");
                    else
                        Tick.GetTick().Script.Send(" vObj.SetValue(''); \n");

                    string TmpValue = value;
                    string TmpText = _Text;
                    _Text = value;
                    if (TmpText != TmpValue)
                    {
                        try
                        {
                            if (this.Change.InvokeEvent != null)
                                if (this.Change.InvokeEvent.GetInvocationList().Length > 0)
                                    this.Change.InvokeEvent(this, new EventArgs());
                        }
                        catch (Exception err)
                        {
                            throw new Exception("On error event AraSelect.Change.\n " + err.Message);
                        }
                    }


                }
            }
            get {
                if (!Multiple)
                    return _Text;
                else
                    return null;
            }
        }
        string[] _TextArray;

        public string[] TextMultiple
        {
            set
            {
                if (Multiple)
                {
                    _TextArray = value;
                    string vTmpC = "[";
                    foreach (string Tmp in _TextArray)
                        vTmpC += "'" + AraTools.StringToStringJS(Tmp) + "',";
                    vTmpC = vTmpC.Substring(0, vTmpC.Length - 1);
                    vTmpC += "]";
                    this.TickScriptCall();
                    Tick.GetTick().Script.Send(" vObj.SetSelects(" + vTmpC + "); \n");
                }
            }
            get
            {
                if (Multiple)
                    return _TextArray;
                else
                    return null;
            }
        }

        private bool _Enable = true;
        [AraDevProperty(true)]
        public bool Enabled
        {
            get { return _Enable; }
            set
            {
                _Enable = value;
                this.TickScriptCall();
                Tick.GetTick().Script.Send(" vObj.SetEnable(" + (_Enable == true ? "true" : "false") + "); \n");
            }
        }
        /*
        private bool _Readonly = false;
        public bool Readonly
        {
            get { return _Readonly; }
            set
            {
                _Readonly = value;
                AraClient Client = AraClients.GetClient();
                this.Call();
                Client.Script.Send(" vObj.SetReadonly(" + (_Readonly == true ? "true" : "false") + "); \n");
            }
        }
        */



        #region Ara2Dev
        private string _Name = "";
        [AraDevProperty("")]
        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }

        private AraEvent<DStartEditPropertys> _StartEditPropertys = null;
        public AraEvent<DStartEditPropertys> StartEditPropertys
        {
            get
            {
                if (_StartEditPropertys == null)
                {
                    _StartEditPropertys = new AraEvent<DStartEditPropertys>();
                    this.Click += this_ClickEdit;
                }

                return _StartEditPropertys;
            }
            set
            {
                _StartEditPropertys = value;
            }
        }
        private void this_ClickEdit(object sender, EventArgs e)
        {
            if (_StartEditPropertys.InvokeEvent != null)
                _StartEditPropertys.InvokeEvent(this);
        }

        private AraEvent<DStartEditPropertys> _ChangeProperty = new AraEvent<DStartEditPropertys>();
        public AraEvent<DStartEditPropertys> ChangeProperty
        {
            get
            {
                return _ChangeProperty;
            }
            set
            {
                _ChangeProperty = value;
            }
        }

        #endregion

    }
}
