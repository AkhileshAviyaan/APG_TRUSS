using Truss.Loads;
using System.Runtime.InteropServices;
using Truss.Supports;

namespace Truss.Element
{
    public class Node
	{
		public double X { get; set; }
		public double Y { get; set; }
		public int Id {  get; set; }	
		public string Label { get; set; }
		public NodalSupport NodalSupport { get; set; }
		public NodalLoad NodalLoad { get; set; }
		public ReactionForce ReactionForce { get; set; }
		public NodalDisplacement NodalDisplacement { get; set; }
		public bool ReactionUnkonwn { get; set; }
		public bool IsFree
		{
			get
			{
				if (NodalSupport == null)
				{
					return true;
				}
				else
				{
					if (NodalSupport.UxRestrain is true || NodalSupport.UyRestrain is true)
					{
						return false;
					}
				}
				return false;
			}
		}
		public Node()
		{
			ReactionForce=new ReactionForce();
		}
		public Node(double x, double y, string label):this()
		{
			X = x;
			Y = y;
			Label = label;
		}
	}

}
public class ReactionForce()
{
	public double Fx { get; set; }
	public double Fy { get; set; }
}
public class NodalDisplacement()
{
	public double dx { get; set; }
	public double dy { get; set; }
}