#include "pch.h"
#include "InfoModel.h"


InfoModel::InfoModel()
{
	this->Age=20;
	fc = Red;
	Height = 170;
	wmemset(Name, 0,NAME_LENGTH);
}


InfoModel::~InfoModel()
{

}
