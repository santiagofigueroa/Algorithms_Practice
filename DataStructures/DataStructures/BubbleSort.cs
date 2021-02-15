using System;
using System.Collections.Generic;
using System.Text;

namespace DataStructures
{
	public static class BubbleSort
	{

		public static int[] bubblesort(int [] arr,  int n )
		{	
			for (int pass = n -1; pass >= 0;  pass--)
			{
				for (int i = 0; i <= pass -1; i++)
				{
					if(arr[i] > arr[i + 1])
					{
						var temp = arr[i];
						arr[i] = arr[i + 1];
						arr[i + 1] = temp;
						Console.WriteLine(arr[i]);
					}
				}	


			}
			return arr;

		} 

	}
}
