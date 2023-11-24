using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;

public abstract class PanelSystemTemplateType : Templates
{
    
    public abstract int panelId { get; set; }
        
    
    //public int panelId = templateId;

    
    public PanelSystemTemplateType()
    {
     
    //panelId = base.templateId;

    }
        
    
}
