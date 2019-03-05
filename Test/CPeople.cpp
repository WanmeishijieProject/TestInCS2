#include "pch.h"
#include "CPeople.h"

CPeople::CPeople()
{

}
CPeople::CPeople(double h)
{
	CPeople::Height = h;
}
bool CPeople::IsTaller(bool(*Func)(CPeople* p1, CPeople* p2),CPeople* P2)
{
	return Func(this, P2);
}

bool CPeople::operator>(CPeople& p2)
{
	return Height>p2.Height;
}
bool CPeople::operator<(CPeople& p2)
{
	return !(Height>p2.Height);
}

void CPeople::SetName(const wchar_t* Name)
{
	if (Name != this->pName)
	{
		//this->pName = Name;
		wmemset(pName, 0, 256);
		size_t len=wcslen(Name);
		wmemcpy(pName, Name, len);
		InfoModel* pModel = new InfoModel();

		pModel->Age = 34;
		wmemcpy(pModel->Name, Name, len);
		pModel->fc = Green;
		pModel->Height = 123;

		onNameChanged(this,pModel);
	}
}

wchar_t* CPeople::GetName()
{
	return this->pName;
}

CPeople::~CPeople()
{
}
