//This ensures that this source file is included in the solution only once
#pragma once

//This includes the Arcs header file
#include "Arcs.h"
//These includes the fstream library, the sstream library, the iostream library and the iomanip library
#include <fstream>
#include <sstream>
#include <iostream>
#include <iomanip>

class Navigation
{
//Things from this point on are only accessible from within this class
private:

	//This is the initialiser for one of the methods given by the tutor
	void LLtoUTM(const float Lat, const float Long, float &UTMNorthing, float &UTMEasting) const;

	//This is the output file
	ofstream m_outFile;

public:

	//These are the initialisers for the constructor and destructor
	Navigation();
	~Navigation();

	//This is the initialiser for the method to build the linked lists
	bool BuildNetwork(const string &fileNamePlaces, const string &fileNameLinks) const;
	//This is the initialiser for the method to process the commands to be performed upon the linked lists
	bool ProcessCommand(const string &commandString);

	//This is the initialiser for the method to calculate the max distance between two Nodes
	bool MaxDist(const string &commandEntered);
	//This is the initialiser for the method to calculate the longest Arc
	bool MaxLink(const string &commandEntered);

	//This is the initialiser for the method to find the distance between two Nodes
	bool FindDist(const string &commandEntered);
	//This is the initialiser for the method to find all the Nodes that are connected to one Node by an Arc
	bool FindNeighbour(const string &commandEntered, const string &checkName, bool write);

	//These are the initialisers for the methods to check if an Arc can be traveled via a certain transport method
	bool Check(const string &commandEntered, bool write);
	bool CheckFoot(const string &splitString, const string &previousSplitString, bool write);
	bool CheckBike(const string &splitString, const string &previousSplitString, bool write);
	bool CheckCar(const string &splitString, const string &previousSplitString, bool write);
	bool CheckBus(const string &splitString, const string &previousSplitString, bool write);
	bool CheckRail(const string &splitString, const string &previousSplitString, bool write);
	bool CheckShip(const string &splitString, const string &previousSplitString, bool write);

	//This is the initialiser for the method to find a route between two nodes
	bool FindRoute(const string &commandEntered);

	//This is the initialiser for one of the methods given by the tutor
	float ArcLength(float startNorth, float startEast, float endNorth, float endEast) const;
};

