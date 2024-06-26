using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AllEnums;
using static Truss.Utility.MathHelp;
using Truss.Element;
using Truss.Utility;
using static System.Formats.Asn1.AsnWriter;
namespace Truss.Element
{
	public class TrussElement
	{
		public Node StartNode { get; set; }
		public Node EndNode { get; set; }
		public MaterialProperties MaterialProp { get; set; }
		public AllSinCosThetaComputation CS { get; set; }
		public double Length => Sqrt(Pow(StartNode.X - EndNode.X, 2) + Pow(StartNode.Y - EndNode.Y, 2));
		public double InternalForce { get; set; }
		public double InternalStrain { get; set; }
		public double InternalStress { get; set; }
		private double slope => Atan2((EndNode.Y - StartNode.Y) , (EndNode.X - StartNode.X));
		public LoadDirection loadDirection { get; set; }
		public TrussElement(Node n1, Node n2, MaterialProperties mp)
		{
			StartNode = n1;
			EndNode = n2;
			MaterialProp = mp;
			loadDirection = LoadDirection.GlobalDirection;
			LocalStiffnessMatrix = new Matrix(4, 4);
		}
		public Matrix LocalStiffnessMatrix { get; set; }
		public void GetLocalStiffnessMatrix()
		{
			var mp = this.MaterialProp;

			CS = new AllSinCosThetaComputation(this.slope);

			double firstExp = mp.Area * mp.E / Length;

			LocalStiffnessMatrix.Data = new double[,] { {CS.c2,CS.cs,-CS.c2,-CS.cs },
														{CS.cs,CS.s2, -CS.cs,-CS.s2 },
														{-CS.c2,-CS.cs,CS.c2,CS.cs},
														{-CS.cs,-CS.s2,CS.cs,CS.s2 } };
			LocalStiffnessMatrix = firstExp * LocalStiffnessMatrix;
		}
	}
	public class AllSinCosThetaComputation
	{


		public double c;
		public double s;
		public double c2;
		public double s2;
		public double cs;
		public AllSinCosThetaComputation() { }
		public AllSinCosThetaComputation(double slope)
		{
			c = Pow(Cos(slope), 1);
			s = Pow(Sin(slope), 1);
			c2 = Pow(Cos(slope), 2);
			s2 = Pow(Sin(slope), 2);
			cs = Cos(slope) * Sin(slope);
		}
	}
}
