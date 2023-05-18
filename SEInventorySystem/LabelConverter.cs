using VRage.Game;

namespace IngameScript {
    partial class Program {
        public static class LabelConverter{

            public static MyDefinitionId getBlueprintDefinitionByName(string name) {

                string def = "MyObjectBuilder_BlueprintDefinition/";

                name = name.EndsWith("item") ? name.Replace("item", "") : name;
                name = name.EndsWith("OreToIngot") ? name.Replace("OreToIngot", "") : name;
                

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
        }
    }
}
