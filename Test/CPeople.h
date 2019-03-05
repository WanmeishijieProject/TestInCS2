#pragma once
#include <string>
#include "InfoModel.h"
class CPeople
{
	typedef void(*OnNameChanged)(CPeople* Sender, InfoModel* e);
public:
	CPeople();
	CPeople(double h);
	double Height;
	bool IsTaller(bool(*Func)(CPeople* p1, CPeople* p2), CPeople* P1);
	bool operator >(CPeople& p2);
	bool operator <(CPeople& p2);
	void SetName(const wchar_t* Name);
	wchar_t* GetName();
	OnNameChanged onNameChanged;
	~CPeople();
private:
	wchar_t pName[256] = {0};
};

