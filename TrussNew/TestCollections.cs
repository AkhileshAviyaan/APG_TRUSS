using AllEnums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Truss.Element;
using Truss.Loads;
using Truss.Supports;
using Truss;
using static Truss.Utility.MathHelp;
namespace TrussNew
{
	public class TestCollections
	{
		public void Test1()
		{
			TrussMain truss = new TrussMain();
			Node n1 = new Node(0, 40, "n1");
			Node n2 = new Node(40, 40, "n2");
			Node n3 = new Node(0, 0, "n3");
			truss.Nodes.AddRange([n1, n2, n3]);

			MaterialProperties mp = new MaterialProperties(10e6, 1.5, "MaterialProperties1");
			TrussElement e1 = new TrussElement(n1, n2, mp);
			TrussElement e2 = new TrussElement(n2, n3, mp);
			truss.TrussElement.AddRange([e1, e2]);
			n1.NodalSupport = new NodalSupport(ENodalSupport.Hinge);
			n3.NodalSupport = new NodalSupport(ENodalSupport.Hinge);

			n2.NodalLoad = new NodalLoad(500, 300);
			truss.Solve();
		}
		public void Test2()
		{
			TrussMain truss = new TrussMain();
			Node n1 = new Node(0, 0, "n1");
			Node n2 = new Node(0.5, 0, "n2");
			Node n3 = new Node(0.5, 0.4, "n3");
			Node n4 = new Node(0, 0.4, "n4");
			truss.Nodes.AddRange([n1, n2, n3,n4]);

			MaterialProperties mp = new MaterialProperties(210e6, 300e-6, "MaterialProperties1");
			TrussElement e1 = new TrussElement(n1, n2, mp);
			TrussElement e2 = new TrussElement(n2, n3, mp);
			TrussElement e3 = new TrussElement(n3, n4, mp);
			TrussElement e4 = new TrussElement(n3, n1, mp);
			truss.TrussElement.AddRange([e1, e2,e3,e4]);
			n1.NodalSupport = new NodalSupport(ENodalSupport.Hinge);
			n4.NodalSupport = new NodalSupport(ENodalSupport.Hinge);

			n2.NodalLoad = new NodalLoad(50, 0);
			n3.NodalLoad = new NodalLoad(0,-40);
			truss.Solve();
		}
		public void Test3()
		{
			TrussMain truss = new TrussMain();
			Node n1 = new Node(0, 0, "n1");
			Node n2 = new Node(0, 1, "n2");
			Node n3 = new Node(1, 1, "n3");
			truss.Nodes.AddRange([n1, n2, n3]);

			MaterialProperties mp1 = new MaterialProperties(210e6, 6e-4, "MaterialProperties1");
			MaterialProperties mp2 = new MaterialProperties(210e6, 6e-4*Sqrt(2), "MaterialProperties2");
			TrussElement e1 = new TrussElement(n1, n2, mp1);
			TrussElement e2 = new TrussElement(n2, n3, mp1);
			TrussElement e3 = new TrussElement(n3, n1, mp2);
			truss.TrussElement.AddRange([e1, e2, e3]);
			n1.NodalSupport = new NodalSupport(ENodalSupport.Hinge);
			n2.NodalSupport = new NodalSupport(ENodalSupport.RollarY);
			n3.NodalSupport = new NodalSupport(ENodalSupport.Rollar,135);

			n2.NodalLoad = new NodalLoad(1000, 0);
			truss.Solve();
		}
	}
}
