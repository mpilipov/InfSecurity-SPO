

#include "pch.h"
#include <iostream>

using namespace std;
void binary(unsigned int u);
int main()
{
	binary(45615161);
	return 0;
}
void binary(unsigned int u)
{
	int t;
	int f = 0;
	int m = (u % 10)+1;
	for (t = 128; t > 0; t = t / m)
		if (u & t)
		{
			f = f + t * m;
		}
		else
			f = f + t;
	cout << f;
}


