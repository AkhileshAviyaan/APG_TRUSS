using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using Truss.Utility;
using Truss.Element;
using static Truss.Utility.MathHelp;
using AllEnums;

namespace Truss.Element
{
	public class TrussMain
	{
		public List<Node> Nodes { get; set; }
		public List<TrussElement> TrussElement { get; set; }
		public Matrix F { get; set; }
		public Matrix K { get; set; }
		public Matrix T { get; set; }
		public Matrix U { get; set; }
		public Matrix ReactionMatrix { get; set; }
		public Matrix InternalForce { get; set; }
		int NodeCount { get; set; }

		public TrussMain()
		{
			Nodes = new List<Node>();
			TrussElement = new List<TrussElement>();
		}
		public void Solve()
		{
			NodeCount = Nodes.Count;
			ReactionMatrixUpdate();
			DisplacementMatrixUpdate();
			AssembleGlobalStiffness();
			CalculateDisplacement();
			OtherComputation();
		}
		void ReactionMatrixUpdate()
		{
			ReactionMatrix = new Matrix(NodeCount * 2, 1);
			for (int i = 0; i < NodeCount; i++)
			{
				Nodes[i].Id = i;
				if (Nodes[i].NodalLoad is not null)
				{
					ReactionMatrix.Data[i * 2, 0] = Nodes[i].NodalLoad.Fx;
					ReactionMatrix.Data[i * 2 + 1, 0] = Nodes[i].NodalLoad.Fy;
				}
			}
		}
		void DisplacementMatrixUpdate()
		{
			U = new Matrix(NodeCount * 2, 1);

			for (int i = 0; i < NodeCount; i++)
			{

				if(Nodes[i].NodalSupport is not null)
				{
					U.Data[i * 2, 0] = Nodes[i].NodalSupport.Ux;
					U.Data[i * 2 + 1, 0] = Nodes[i].NodalSupport.Uy;
				}
			}
		}
		void AssembleGlobalStiffness()
		{
			K = new Matrix(NodeCount * 2, NodeCount * 2);
			foreach (var element in TrussElement)
			{
				element.GetLocalStiffnessMatrix();
				Matrix k = element.LocalStiffnessMatrix;

				List<int> ids = [element.StartNode.Id * 2, element.StartNode.Id * 2 + 1, element.EndNode.Id * 2, element.EndNode.Id * 2 + 1];
				for (int i = 0; i < ids.Count; i++)
				{
					for (int j = 0; j < ids.Count; j++)
					{
						K.Data[ids[i], ids[j]] += k.Data[i, j];
					}
				}
			}
			TransformationStiffnessMatrix();
		}
		void TransformationStiffnessMatrix()
		{
			T = new Matrix(NodeCount * 2, NodeCount * 2);
			for (int i = 0; i < NodeCount; i++)
			{
				if (Nodes[i].NodalSupport is not null)
				{
					double O =PI*( Nodes[i].NodalSupport.Rotation - 90)/180;
					T.Data[2 * i, 2 * i] = Cos(O);
					T.Data[2 * i, 2 * i + 1] = Sin(O);
					T.Data[2 * i + 1, 2 * i] = -Sin(O);
					T.Data[2 * i + 1, 2 * i + 1] = Cos(O);
				}
				else
				{
					T.Data[2 * i, 2 * i] = 1;
					T.Data[2 * i + 1, 2 * i + 1] = 1;
				}
			}
			K = T * K * T.Transpose;
		}
		void CalculateDisplacement()
		{
			List<int> ids = [];

			//Get Id of not restrained node for reduced matrix
			foreach (var node in Nodes)
			{
				if (node.NodalSupport is not null)
				{
					if ((node.NodalSupport.UxRestrain is false)) ids.Add(node.Id * 2);
					if ((node.NodalSupport.UyRestrain is false)) ids.Add(node.Id * 2 + 1);
				}
				else
				{
					ids.Add(node.Id * 2);
					ids.Add(node.Id * 2 + 1);
				}
				if ((node.NodalDisplacement is not null))
				{
					if (!(ids.Contains(node.Id * 2) || ids.Contains(node.Id * 2 + 1)))
					{
						ids.Add(node.Id * 2);
						ids.Add(node.Id * 2 + 1);
					}
				}
				else
				{
					node.NodalDisplacement = new NodalDisplacement();
				}
			}
			Matrix rk = new Matrix(ids.Count, ids.Count);
			for (int i = 0; i < ids.Count; i++)
			{
				for (int j = 0; j < ids.Count; j++)
				{
					rk.Data[i, j] = K.Data[ids[i], ids[j]];
				}
			}
			Matrix irk = rk.Inverse;

			Matrix rf = new Matrix(ids.Count, 1);
			for (int i = 0; i < ids.Count; i++)
			{
				rf.Data[i, 0] = ReactionMatrix.Data[ids[i], 0];
			}

			Matrix u = irk * rf;
			for (int i = 0; i < ids.Count; i++)
			{
				U.Data[ids[i], 0] = u.Data[i, 0];
				if (ids[i] % 2 == 0)
				{
					Nodes[ids[i] / 2].NodalDisplacement.dx = u.Data[i, 0];
				}
				else
				{
					Nodes[(ids[i] - 1) / 2].NodalDisplacement.dy = u.Data[i, 0];
				}
			}
		}
		void OtherComputation()
		{
			ReactionMatrix = K * U - ReactionMatrix;

			InternalForce = new Matrix(TrussElement.Count, 1);
			int i = 0;
			foreach (var m in TrussElement)
			{
				double firstExp = m.MaterialProp.E * m.MaterialProp.Area / m.Length;
				double secondExp = (-m.CS.c * m.StartNode.NodalDisplacement.dx +
											-m.CS.s * m.StartNode.NodalDisplacement.dy +
											m.CS.c * m.EndNode.NodalDisplacement.dx +
											m.CS.s * m.EndNode.NodalDisplacement.dy);
				InternalForce.Data[i, 0] = m.InternalForce = firstExp * secondExp;
				i++;
			}

		}

	}
}