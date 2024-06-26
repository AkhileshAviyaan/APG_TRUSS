using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AllEnums;
namespace Truss.Supports
{
    public class NodalSupport
    {
        public double Ux { get; set; }
        public double Uy { get; set; }

        public bool UxRestrain { get; set; }
        public bool UyRestrain { get; set; }

        public double Rotation { get; set; }
        public ENodalSupport ENodalSupport { get; set; }
        public NodalSupport() { }
        public NodalSupport(ENodalSupport restrainCondition)
        {
            Rotation = 90;
            if (restrainCondition == ENodalSupport.Hinge || restrainCondition == ENodalSupport.Fixed)
            {
                UxRestrain = true;
                UyRestrain = true;
            }
            else if (restrainCondition == ENodalSupport.RollarX)
            {
                UxRestrain = true;
            }
            else if (restrainCondition == ENodalSupport.RollarY)
            {
                UyRestrain = true;
            }
        }
		public NodalSupport(ENodalSupport restrainCondition,double rotation)
		{
			if (restrainCondition == ENodalSupport.Hinge || restrainCondition == ENodalSupport.Fixed)
			{
				UxRestrain = true;
				UyRestrain = true;
                Rotation = 90;
			}
			else if (rotation==0)
			{
                ENodalSupport = ENodalSupport.RollarX ;
				UxRestrain = true;
                Rotation = 0;
			}
			else if (rotation==90)
			{
                ENodalSupport= ENodalSupport.RollarY ;
				UyRestrain = true;
                Rotation = 90;
			}
            else
            {
				UyRestrain = true;
				ENodalSupport = restrainCondition;
                Rotation=rotation;
            }
		}

	}

}



