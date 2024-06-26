using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Truss.Utility.MathHelp;
namespace Truss.Loads
{
	public class NodalLoad
	{
		public double Fx;
		public double Fy;
		public double F { get; set; }
		public double AngleFromHorizontal { get; set; }

		public NodalLoad() { }
		public NodalLoad(double fx, double fy)
		{
			Fx = fx;
			Fy = fy;
			F = Sqrt(Pow(fx, 2) + Pow(fy, 2));
			AngleFromHorizontal = Atan2(fx, fy);
		}

	}

}
