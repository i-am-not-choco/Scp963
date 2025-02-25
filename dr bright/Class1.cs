

using Exiled.API.Features;
using CustomItem = Exiled.CustomItems.API.Features.CustomItem;

namespace dr_bright
{ 
    public class Class1 : Plugin<Config>
    { 
        public static Class1 Instance { get; private set; }
        


        public override void OnEnabled()
        { 
            Instance = this;
            CustomItem.RegisterItems();
            base.OnEnabled();
        }
        

        public override void OnDisabled()
        {
            Instance = null;
            CustomItem.UnregisterItems();
            base.OnDisabled();
        }
        
        
    }
    
    
}