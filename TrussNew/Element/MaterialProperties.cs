using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Truss.Element
{
	public class MaterialProperties
	{
		public double E { get; set; }
		public string Label { get; set; }
		public MaterialProperties() { }
		public MaterialProperties(double e, double area, string label)
		{
			E = e;
			Area = area;
			Label =label;
		}
		public double Area { get; set; }
	}
}
