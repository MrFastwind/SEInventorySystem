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

            IMyInventory InputInventory { get; }
            IMyInventory OutputInventory { get; }

            bool IsProducing { get; }

            bool IsQueueEmpty { get; }

            void MoveQueueItemRequest(uint queueItemId, int targetIdx);

            bool CanUseBlueprint(MyDefinitionId blueprint);

            private IMyAssembler getEmptierAssembler() {
                int slotCount=0;
                IMyAssembler selected=null;
                var queue = new List<MyProductionItem>();

                foreach (var assembler in assemblers){
                    if (assembler.IsQueueEmpty) { return assembler; }

                    assembler.GetQueue(queue);

                    if (queue.Count < slotCount || selected == null) {
                        selected = assembler;
                        slotCount = queue.Count;
                    }

                }

                return selected;
            }

            void AddQueueItem(MyDefinitionId blueprint, MyFixedPoint amount) {
                
            }

            void AddQueueItem(MyDefinitionId blueprint, decimal amount);

            void AddQueueItem(MyDefinitionId blueprint, double amount);


            void InsertQueueItem(int idx, MyDefinitionId blueprint, MyFixedPoint amount);

            void InsertQueueItem(int idx, MyDefinitionId blueprint, decimal amount);

            void InsertQueueItem(int idx, MyDefinitionId blueprint, double amount);


            void RemoveQueueItem(int idx, MyFixedPoint amount);

            void RemoveQueueItem(int idx, decimal amount);

            void RemoveQueueItem(int idx, double amount);


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
