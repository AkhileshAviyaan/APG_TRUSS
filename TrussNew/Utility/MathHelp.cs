using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Truss.Utility
{
	internal static class MathHelp
	{
		public static double Cos(double x) => Math.Cos(x);
		public static double Sin(double x) => Math.Sin(x);
		public static double Tan(double x) => Math.Tan(x);
		public static double Atan(double x) => Math.Atan(x);
		public static double Atan2(double x, double y) => Math.Atan2(x, y);
		public static double Pow(double x, double y) => Math.Pow(x, y);
		public static double Sqrt(double x) => Math.Sqrt(x);
		public static double PI => Math.PI;
		public static double Abs(double x) => Math.Abs(x);
	}
}