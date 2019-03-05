// Test.cpp : 此文件包含 "main" 函数。程序执行将在此处开始并结束。
//

#include "pch.h"
#include <iostream>
#include "CPeople.h"
#include <Windows.h>
using namespace std;
typedef int(*CallBackFunc)(int x, int y);

int GetYourData(CallBackFunc fun, int x, int y)
{
	return fun(x, y);
}

int MyOwnFunc(int x, int y)
{
	return x * x + y * y;
}

bool Compare(CPeople* p1, CPeople* p2)
{
	return *p1 < *p2;
}


void OnNameChanged(CPeople* P, InfoModel* e)
{
	MessageBox(0, e->Name, L"", 0);
	if (e != NULL)
		delete e;
	e = NULL;
}
int main()
{
	int a = 3, b = 5;
	//cout<<GetYourData(MyOwnFunc,3, 5)<<endl;
	CPeople P1(8);
	CPeople P2(9);
	P1.onNameChanged = OnNameChanged;
	P2.onNameChanged = OnNameChanged;
	P1.SetName(L"ABCDEFG");
	P2.SetName(L"KLKJ");

	//cout<< P1.IsTaller(Compare,&P2)<<endl;
}







// 运行程序: Ctrl + F5 或调试 >“开始执行(不调试)”菜单
// 调试程序: F5 或调试 >“开始调试”菜单

// 入门提示: 
//   1. 使用解决方案资源管理器窗口添加/管理文件
//   2. 使用团队资源管理器窗口连接到源代码管理
//   3. 使用输出窗口查看生成输出和其他消息
//   4. 使用错误列表窗口查看错误
//   5. 转到“项目”>“添加新项”以创建新的代码文件，或转到“项目”>“添加现有项”以将现有代码文件添加到项目
//   6. 将来，若要再次打开此项目，请转到“文件”>“打开”>“项目”并选择 .sln 文件
