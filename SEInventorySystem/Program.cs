using Sandbox.ModAPI.Ingame;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using VRage;
using VRage.Game;
using VRage.Game.ModAPI.Ingame;

namespace IngameScript {
    partial class Program : MyGridProgram
    {
        // This file contains your actual script.
        //
        // You can either keep all your code here, or you can create separate
        // code files to make your program easier to navigate while coding.
        //
        // In order to add a new utility class, right-click on your project, 
        // select 'New' then 'Add Item...'. Now find the 'Space Engineers'
        // category under 'Visual C# Items' on the left hand side, and select
        // 'Utility Class' in the main area. Name it in the box below, and
        // press OK. This utility class will be merged in with your code when
        // deploying your final script.
        //
        // You can also simply create a new utility class manually, you don't
        // have to use the template if you don't want to. Just do so the first
        // time to see what a utility class looks like.
        // 
        // Go to:
        // https://github.com/malware-dev/MDK-SE/wiki/Quick-Introduction-to-Space-Engineers-Ingame-Scripts
        //
        // to learn more about ingame scripts.

        List<IMyCubeBlock> inventoriesBlocks = new List<IMyCubeBlock>();
        AssemblerGroup assemblersGroup = new AssemblerGroup();
        Dictionary<string, int> inventories = new Dictionary<string, int>();
        Dictionary<string, int> wantedValues = new Dictionary<string, int> {
            {"BulletproofGlass", 0 },
            {"Canvas", 0 },
            {"Computer", 0 },
            {"Construction", 0 },
            {"Detector", 0 },
            {"Display", 0 },
            {"Explosives", 0 },
            {"Girder", 0 },
            {"GravityGenerator", 0 },
            {"InteriorPlate", 0 },
            {"LargeTube", 0 },
            {"Medical", 0 },
            {"MetalGrid", 0 },
            {"Motor", 0 },
            {"PowerCell", 0 },
            {"RadioCommunication", 0 },
            {"Reactor", 0 },
            {"SmallTube", 0 },
            {"SolarCell", 0 },
            {"SteelPlate", 0 },
            {"Superconductor", 0 },
            {"Thrust", 0 },
            {"ZoneChip", 0 }
        };

        Dictionary<string, int> itemsStock = new Dictionary<string, int>();
        Dictionary<string, int> itemsQueue = new Dictionary<string, int>();

        public Program() {
            // The constructor, called only once every session and
            // always before any other method is called. Use it to
            // initialize your script. 
            //     
            // The constructor is optional and can be removed if not
            // needed.
            // 
            // It's recommended to set Runtime.UpdateFrequency 
            // here, which will allow your script to run itself without a 
            // timer block.

            Runtime.UpdateFrequency = UpdateFrequency.Update100;
        }

        public void Save() {
            // Called when the program needs to save its state. Use
            // this method to save your state to the Storage field
            // or some other means. 
            // 
            // This method is optional and can be removed if not
            // needed.
        }

        public void Main(string argument, UpdateType updateSource) {
            // The main entry point of the script, invoked every time
            // one of the programmable block's Run actions are invoked,
            // or the script updates itself. The updateSource argument
            // describes where the update came from. Be aware that the
            // updateSource is a  bitfield  and might contain more than 
            // one update type.
            // 
            // The method itself is required, but the arguments above
            // can be removed if not needed.

            Echo("AutoCrafter Started!");
            if (assemblersGroup.Assemblers.Count==0) LoadAssemblers();
            DetermineItemsStock();
            DetermineItemsQueue();
            foreach (var item in wantedValues)
            {
                if (item.Value > 0) {
                    Echo($"{item.Key}: w: {wantedValues[item.Key]}, s: {itemsStock.GetValueOrDefault(item.Key, -1)}, q: {itemsQueue.GetValueOrDefault(item.Key, -1)}");

                }

            }

            foreach (var item in wantedValues){
                var offset = item.Value - (itemsStock.GetValueOrDefault(item.Key, 0) + itemsQueue.GetValueOrDefault(item.Key, 0));
                if (offset > 0) {
                    Echo($"Quering {item.Key}");
                    AddItemToProductionQueue(item.Key, offset);
                }
                                  
            }
            ClearLists();
        }

        private void ClearLists() {
            inventoriesBlocks.Clear();
            inventories.Clear();
            itemsStock.Clear();
            itemsQueue.Clear();

        }

        private static bool IsComponent(MyItemType type) => type.TypeId.Contains("Component");

        private bool IsTracked(MyItemType type) => wantedValues.ContainsKey(GetItemName(type));

        private static string GetItemName(MyItemType type) {
            return (string)type.SubtypeId;
        }
        private void LoadAssemblers() {
            assemblersGroup.Assemblers.Clear();
            GridTerminalSystem.GetBlocksOfType(assemblersGroup.Assemblers, x => x.Mode == MyAssemblerMode.Assembly && x.CooperativeMode==true);
        }

        private bool AddItemToProductionQueue(string item,int amount) {
            return assemblersGroup.AddQueueItem(LabelConverter.GetBlueprintDefinitionByName(item), (MyFixedPoint)amount);
        }

        private void DetermineItemsStock() {
            GridTerminalSystem.GetBlocksOfType(inventoriesBlocks, x => x.HasInventory);
            List<MyInventoryItem> gridInventory = new List<MyInventoryItem>();
 
            //List<MyInventoryItem> items = new List<MyInventoryItem>();
            foreach (var block in inventoriesBlocks) {
                //items.Clear();
                //VRage.Game.ModAPI.Ingame.IMyInventory inventory = block.GetInventory();

                int invNum = block.InventoryCount;
                for (int i = 0; i < invNum; i++)
                {
                    block.GetInventory(i).GetItems(gridInventory);
                    //Echo($"InvSize: {block.GetInventory(i).ItemCount}, gridSize: {gridInventory.Count}");
                }
                //if (gridInventory == null || gridInventory.Count == 0) continue;
            }
            IEnumerable<MyInventoryItem> stocks = gridInventory.Where(x => IsComponent(x.Type) && IsTracked(x.Type));
            //Echo($"Filtered: {stocks.Count()}, InventorySlosts {gridInventory.Count}");
            foreach (var item in stocks) {
                var value = 0;
                itemsStock.TryGetValue(GetItemName(item.Type), out value);
                itemsStock[GetItemName(item.Type)] = value + (int)item.Amount;
            }
        }

        private void DetermineItemsQueue() {
            var queue = new List<MyProductionItem>();
            assemblersGroup.GetQueue(queue);

            if (queue == null || queue.Count == 0) return;

            foreach (var item in queue) {
                var name = LabelConverter.GetNameByDefinition(item.BlueprintId);
                if (wantedValues.ContainsKey(name)) {
                    var value = 0;
                    itemsQueue.TryGetValue(name, out value);
                    itemsQueue[name] = value + (int)item.Amount;
                }
                
            }
        }
    }
}
