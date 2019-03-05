#pragma once
#include <string>

#define NAME_LENGTH 256

enum FaverateColor
{
	Red,
	Green,
	Yellow,
};

class InfoModel
{

public:
	InfoModel();
	wchar_t Name[NAME_LENGTH];
	int Age;
	double Height;
	FaverateColor fc;
	~InfoModel();

};

