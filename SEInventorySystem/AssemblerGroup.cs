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

            ICollection<IMyAssembler> assemblers;

            public AssemblerGroup(ICollection<IMyAssembler> assemblers) {
                this.assemblers = assemblers;
            }

            bool CanUseBlueprint(MyDefinitionId blueprint) {
                return assemblers.Any(x => x.CanUseBlueprint(blueprint));
             }

            private bool addToEmptierAssembler(MyDefinitionId blueprint, MyFixedPoint ammount) {
                int slotCount=0;
                IMyAssembler selected=null;
                var queue = new List<MyProductionItem>();

                foreach (var item in assemblers){
                    if (!item.CanUseBlueprint(blueprint)) continue;
                    if (item.IsQueueEmpty) {
                        selected.AddQueueItem(blueprint, ammount);
                        return true;
                    }

                    item.GetQueue(queue);
                    if ((queue.Capacity != queue.Count && queue.Count < slotCount) || selected == null) {
                        selected = item;
                        slotCount = queue.Count;
                    }
                }
                if(selected != null) {
                    selected.AddQueueItem(blueprint, ammount);
                    return true;
                }
                return false;
            }

            bool AddQueueItem(MyDefinitionId blueprint, MyFixedPoint amount) {
                return addToEmptierAssembler(blueprint, amount);
            }

            void AddQueueItem(MyDefinitionId blueprint, decimal amount) => AddQueueItem(blueprint,amount);

            void AddQueueItem(MyDefinitionId blueprint, double amount) => AddQueueItem(blueprint, amount);

            void ClearQueue() {
                foreach (var assembler in assemblers){
                    assembler.ClearQueue();
                }
            }

            void GetQueue(List<MyProductionItem> items) {
                foreach (var assembler in assemblers) {
                    assembler.GetQueue(items);                
                }
            }
        }
    }
}
