// #######################################################################################
// ##                                                                                   ##
// ##  This main.cpp file WILL BE REPLACED during the marking procedure.                ##
// ##  Therefore all of your implementation code MUST be in the Navigation class,       ##
// ##  or any other new classes that you create and use inside of the Navigation class. ##
// ##                                                                                   ##
// #######################################################################################

#include "Navigation.h"
#include <iostream>
#include <iomanip>
#include <Windows.h>
using namespace std;

void main()
{
	const int NUMBER_OF_RUNS = 500;
	LARGE_INTEGER start, end, frequency;
	double timeTakenInSeconds;
	QueryPerformanceFrequency(&frequency);

	// Build Navigation
	QueryPerformanceCounter(&start);

	Navigation nav;
	if (nav.BuildNetwork("Places.csv", "Links.csv"))
	{
		QueryPerformanceCounter(&end);
		timeTakenInSeconds = (end.QuadPart - start.QuadPart) / (double)(frequency.QuadPart*NUMBER_OF_RUNS);
		cout << fixed << setprecision(10) << "BuildNetwork() - " << timeTakenInSeconds << " seconds" << endl;
	}
	else
	{
		cout << endl << "*** Error in BuildNetwork() ***" << endl;
	}

	// ***** Process Commands *****

	// MaxDist
	QueryPerformanceCounter(&start);
	if (nav.ProcessCommand("MaxDist"))
	{
		QueryPerformanceCounter(&end);
		timeTakenInSeconds = (end.QuadPart - start.QuadPart) / (double)(frequency.QuadPart*NUMBER_OF_RUNS);
		cout << fixed << setprecision(10) << "MaxDist - " << timeTakenInSeconds << " seconds" << endl;
	}
	else
	{
		cout << endl << "*** Error in MaxDist ***" << endl;
	}

	// MaxLink
	QueryPerformanceCounter(&start);
	if (nav.ProcessCommand("MaxLink"))
	{
		QueryPerformanceCounter(&end);
		timeTakenInSeconds = (end.QuadPart - start.QuadPart) / (double)(frequency.QuadPart*NUMBER_OF_RUNS);
		cout << fixed << setprecision(10) << "MaxLink - " << timeTakenInSeconds << " seconds" << endl;
	}
	else
	{
		cout << endl << "*** Error in MaxLink ***" << endl;
	}

	// FindDist
	QueryPerformanceCounter(&start);
	if (nav.ProcessCommand("FindDist 9361783 11391765"))
	{
		QueryPerformanceCounter(&end);
		timeTakenInSeconds = (end.QuadPart - start.QuadPart) / (double)(frequency.QuadPart*NUMBER_OF_RUNS);
		cout << fixed << setprecision(10) << "FindDist 9361783 11391765 - " << timeTakenInSeconds << " seconds" << endl;
	}
	else
	{
		cout << endl << "*** Error in FindDist 9361783 11391765 ***" << endl;
	}

	// FindNeighbour
	QueryPerformanceCounter(&start);
	if (nav.ProcessCommand("FindNeighbour 8611522"))
	{
		QueryPerformanceCounter(&end);
		timeTakenInSeconds = (end.QuadPart - start.QuadPart) / (double)(frequency.QuadPart*NUMBER_OF_RUNS);
		cout << fixed << setprecision(10) << "FindNeighbour 8611522 - " << timeTakenInSeconds << " seconds" << endl;
	}
	else
	{
		cout << endl << "*** Error in FindNeighbour 8611522 ***" << endl;
	}

	// Check Rail - valid route
	QueryPerformanceCounter(&start);
	if (nav.ProcessCommand("Check Rail 14601225 12321385 8611522 9361783"))
	{
		QueryPerformanceCounter(&end);
		timeTakenInSeconds = (end.QuadPart - start.QuadPart) / (double)(frequency.QuadPart*NUMBER_OF_RUNS);
		cout << fixed << setprecision(10) << "Check Rail 14601225 12321385 8611522 9361783 - " << timeTakenInSeconds << " seconds" << endl;
	}
	else
	{
		cout << endl << "*** Error in Check Rail 14601225 12321385 8611522 9361783 ***" << endl;
	}

	// Check Ship - FAIL
	QueryPerformanceCounter(&start);
	if (nav.ProcessCommand("Check Ship 14601225 12321385 8611522 9361783"))
	{
		QueryPerformanceCounter(&end);
		timeTakenInSeconds = (end.QuadPart - start.QuadPart) / (double)(frequency.QuadPart*NUMBER_OF_RUNS);
		cout << fixed << setprecision(10) << "Check Ship 14601225 12321385 8611522 9361783 - " << timeTakenInSeconds << " seconds" << endl;
	}
	else
	{
		cout << endl << "*** Error in Check Ship 14601225 12321385 8611522 9361783 ***" << endl;
	}

	// FindRoute Rail - valid route
	QueryPerformanceCounter(&start);
	if (nav.ProcessCommand("FindRoute Rail 9081958 15832241"))
	{
		QueryPerformanceCounter(&end);
		timeTakenInSeconds = (end.QuadPart - start.QuadPart) / (double)(frequency.QuadPart*NUMBER_OF_RUNS);
		cout << fixed << setprecision(10) << "FindRoute Rail 9081958 15832241 - " << timeTakenInSeconds << " seconds" << endl;
	}
	else
	{
		cout << endl << "*** Error in FindRoute Rail 9081958 15832241 ***" << endl;
	}

	// FindRoute Ship - FAIL
	QueryPerformanceCounter(&start);
	if (nav.ProcessCommand("FindRoute Ship 9081958 15832241"))
	{
		QueryPerformanceCounter(&end);
		timeTakenInSeconds = (end.QuadPart - start.QuadPart) / (double)(frequency.QuadPart*NUMBER_OF_RUNS);
		cout << fixed << setprecision(10) << "FindRoute Ship 9081958 15832241 - " << timeTakenInSeconds << " seconds" << endl;
	}
	else
	{
		cout << endl << "*** Error in FindRoute Ship 9081958 15832241 ***" << endl;
	}

	system("pause");
}