using Sandbox.Game.EntityComponents;
using Sandbox.ModAPI.Ingame;
using Sandbox.ModAPI.Interfaces;
using SpaceEngineers.Game.ModAPI.Ingame;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using VRage;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.GUI.TextPanel;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.ModAPI.Ingame.Utilities;
using VRage.Game.ObjectBuilders.Definitions;
using VRageMath;

namespace IngameScript {
    partial class Program {
        public class AssemblerGroup {
            List<IMyAssembler> assemblers;

            private List<MyProductionItem> queue = new List<MyProductionItem>();

            public List<IMyAssembler> Assemblers {
                get {
                    return assemblers;
                }

                set {
                    assemblers = value;
                }
            }

            public AssemblerGroup(List<IMyAssembler> assemblers) {
                if(assemblers==null) assemblers=new List<IMyAssembler>();
                this.Assemblers = assemblers;
            }

            public AssemblerGroup() {
                this.Assemblers = new List<IMyAssembler>();
            }

            bool CanUseBlueprint(MyDefinitionId blueprint) {
                return Assemblers.Any(x => x.CanUseBlueprint(blueprint));
             }

            private bool addToEmptierAssembler(MyDefinitionId blueprint, MyFixedPoint ammount) {
                int slotCount=0;
                IMyAssembler selected=null;

                foreach (var item in assemblers){
                    if (item==null || !item.CanUseBlueprint(blueprint)) continue;
                    if (item.IsQueueEmpty) {
                        item.AddQueueItem(blueprint, ammount);
                        return true;
                    }
                    item.GetQueue(queue);
                    if ((50 > queue.Count && queue.Count < slotCount) || selected == null) {
                        selected = item;
                        slotCount = queue.Count;
                    }
                    queue.Clear();
                }
                if(selected != null) {
                    selected.AddQueueItem(blueprint, ammount);
                    return true;
                }
                return false;
            }

            public bool AddQueueItem(MyDefinitionId blueprint, MyFixedPoint amount) {
                return addToEmptierAssembler(blueprint, amount);
            }

            public void ClearQueue() {
                foreach (var assembler in Assemblers){
                    assembler.ClearQueue();
                }
            }

            public void GetQueue(List<MyProductionItem> items) {
                var queue = new List<MyProductionItem>();
                foreach (var assembler in assemblers) {
                    assembler.GetQueue(queue);
                    items.AddList(queue);
                }
            }
        }
    }
}
