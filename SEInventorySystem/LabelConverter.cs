using VRage.Game;

namespace IngameScript {
    partial class Program {
        public static class LabelConverter{

            public static MyDefinitionId GetBlueprintDefinitionByName(string name) {

                string def = "MyObjectBuilder_BlueprintDefinition/";

                name = name.EndsWith("item") ? name.Replace("item", "") : name;

                switch (name) {
                    case "ArtilleryShell":
                        def += "LargeCalibreAmmo";
                        break;
                    case "AssaultCannonShell":
                        def += "MediumCalibreAmmo";
                        break;
                    case "AutocannonMagazine":
                        def += "AutocannonClip";
                        break;
                    // Magazines
                    case "NATO_25x184mm":
                        def += "Magazine";
                        break;

                    // Ores
                    case "Cobalt":
                    case "Gold":
                    case "Iron":
                    case "Magnesium":
                    case "Nickel":
                    case "Platinum":
                    case "Silicon":
                    case "Silver":
                    case "Uranium":

                        def += "OreToIngot";
                        break;
                        

                    // where we just add "component"
                    case "Construction":
                    case "Computer":
                    case "Motor":
                    case "Detector":
                    case "Explosives":
                    case "Girder":
                    case "GravityGenerator":
                    case "Medical":
                    case "RadioCommunication":
                    case "Reactor":
                    case "Thrust":
                        
                        def += name + "Component";
                        break;
                    // nothing to change

                    default:
                        def += name;
                        break;
                }
                return MyDefinitionId.Parse(def);

                //Oxygen => IceToOxygen
                //Hydrogen => IceToHydrogen

            }

            public static string GetNameByDefinition(MyDefinitionId def) {

                var name = def.SubtypeId.ToString();

                //name = name.EndsWith("item") ? name.Replace("item", "") : name;
                // Ores
                if (name.EndsWith("OreToIngot")) return name.Replace("OreToIngot", "") ;
                if (name.EndsWith("Component")) return name.Replace("Component", "");

                // handle tools
                if (def.TypeId.ToString() == "MyObjectBuilder_PhysicalGunObject") {
                    if (name.Contains("AngleGrinder") ||
                        name.Contains("HandDrill") ||
                        name.Contains("Welder") ||
                        name.Contains("Rifle") ||
                        name.Contains("Launcher") ||
                        name.Contains("Pistol")) return name + "Item";
                }


                switch (name) {
                    case "LargeCalibreAmmo":
                        return "ArtilleryShell";
                    case "MediumCalibreAmmo":
                        return "AssaultCannonShell";
                    case "AutocannonClip":
                        return "AutocannonMagazine";
                    // Magazines
                    case "Magazine":
                        return "NATO_25x184mm";
                    // nothing to change
                    default:
                        return name;
                }
            }
        }
    }
}
