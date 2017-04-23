//This includes the Navigation header file
#include "Navigation.h"

//This initialises two instances of the Nodes class
Nodes nodes;
Nodes neighbour;

//This initialises two instances of the Arcs class
Arcs arcs;
Arcs route;

//This is the constructor for the Navigation class
Navigation::Navigation() :m_outFile("Output.txt")
{

}

//This is the destructor for the Navigation class
Navigation::~Navigation()
{
	
}

//This method builds the linked lists
bool Navigation::BuildNetwork(const string &fileNamePlaces, const string &fileNameLinks) const
{
	//These varibles hold the inputs from the Places file and the Linkes file
	fstream finPlaces(fileNamePlaces);
	fstream finLinks(fileNameLinks);

	//If either files hold no information the program exits
	if (finPlaces.fail() || finLinks.fail())
	{
		return false;
	}

	//If the Places file holds information
	if (finPlaces.is_open())
	{
		//These are the variables used to build a Node
		string name;
		string latitude;
		string longitude;
		string reference;

		//While there are Nodes to build
		while (getline(finPlaces, name, ','))
		{
			//This places variables split from Places by the comma delimiter into variables used to build a Node
			getline(finPlaces, reference, ',');
			getline(finPlaces, latitude, ',');
			getline(finPlaces, longitude);

			//This builds the Node
			nodes.AddNode(name, atoi(reference.c_str()), stof(latitude), stof(longitude));
		}

		//This closes the stream to the Places file
		finPlaces.close();
	}
	else
	{
		//If the file hold no information the program exits
		return false;
	}

	//If the Links file holds information
	if (finLinks.is_open())
	{
		//These are the variables used to build an Arc
		string name;
		string headReference;
		string tailReference;

		//While there are Arcs to build
		while (getline(finLinks, headReference, ','))
		{
			//This places variables split from Links by the comma delimiter into variables used to build an Arc
			getline(finLinks, tailReference, ',');
			getline(finLinks, name);

			//This builds the Arc
			arcs.AddArc(name, nodes.SearchForNode(atoi(headReference.c_str())), nodes.SearchForNode(atoi(tailReference.c_str())));
		}

		//This closes the stream to the Links file
		finLinks.close();

		//This returns that the build was successful
		return true;
	}

	//If the file hold no information the program exits
	return false;
}

//This method processes the commands to be performed upon the linked lists
bool Navigation::ProcessCommand(const string &commandString)
{
	//These variables are used to take the command entered and split them into strings used to determine which method to use
	string commandEntered = commandString;
	istringstream inString(commandEntered);
	string command;

	inString >> command;

	//If the command entered was "MaxDist" the MaxDist method is called
	if (command == "MaxDist")
	{
		return MaxDist(commandEntered);
	}

	//If the command entered was "MaxLink" the MaxLink method is called
	if (command == "MaxLink")
	{
		return MaxLink(commandEntered);
	}

	//If the command entered was "FindDist" the FindDist method is called
	if (command == "FindDist")
	{
		return FindDist(commandEntered);
	}

	//If the command entered was "FindNeighbour" the FindNeighbour method is called with permission to write to the output file
	if (command == "FindNeighbour")
	{
		return FindNeighbour(commandEntered, "", true);
	}

	//If the command entered was "Check" the Check method is called with permission to write to the output file
	if (command == "Check")
	{
		return Check(commandEntered, true);
	}

	//If the command entered was "FindRoute" the FindRoute method is called
	if (command == "FindRoute")
	{
		return FindRoute(commandEntered);
	}

	//If the command entered was not recognised the program exits
	return false;
}

//This method calculates the max distance between two Nodes
bool Navigation::MaxDist(const string &commandEntered)
{
	//This is a temporary pointer to a list of Nodes
	Node *maxDistNode;

	//This holds the current longest distance
	float maxDistance = 0;

	//This holds the name of the Nodes that the max distance spans
	string headName, tailName;

	//This holds to condition of the method
	bool successful = false;

	//This sets the temporary pointer and the Work Node to the Head Node
	maxDistNode = nodes.GetHeadNode();
	nodes.SetWorkNode(maxDistNode);

	//This cycles through the linked list to the last Node
	while (maxDistNode->GetName() != "NULL")
	{
		//This cycles through the linked list to the last Node
		while (nodes.GetWorkNode()->GetName() != "NULL")
		{
			//If the distance between the temporary pointer and the Work Node is greater than the current max distance
			if (ArcLength(maxDistNode->GetLatitude(), maxDistNode->GetLongitude(), nodes.GetWorkNode()->GetLatitude(), nodes.GetWorkNode()->GetLongitude()) > maxDistance)
			{
				//This sets the consition of the method to successful
				successful = true;

				//This changes the max distance to the new max distance
				maxDistance = ArcLength(maxDistNode->GetLatitude(), maxDistNode->GetLongitude(), nodes.GetWorkNode()->GetLatitude(), nodes.GetWorkNode()->GetLongitude());

				//This records the Nodes that the max distance spans
				headName = maxDistNode->GetName();
				tailName = nodes.GetWorkNode()->GetName();
			}

			nodes.SetWorkNode(nodes.GetWorkNode()->GetNextNode());
		}

		maxDistNode = maxDistNode->GetNextNode();
		nodes.SetWorkNode(maxDistNode);
	}

	//This outputs the results to the output file
	m_outFile << commandEntered << endl;
	m_outFile << fixed;
	m_outFile << headName << "," << tailName << "," << setprecision(3) << maxDistance << endl;
	m_outFile << endl;

	//This returns the condition of the method
	return successful;
}

//This method to calculates the longest Arc
bool Navigation::MaxLink(const string &commandEntered)
{
	//This holds the current longest link
	float maxLink = 0;

	//This holds the reference of the Nodes that the max link spans
	int headReference = 0;
	int tailReference = 0;

	//This holds to condition of the method
	bool successful = false;

	//This sets the Work Arc to the Head Arc
	arcs.SetWorkArc(arcs.GetHeadArc());

	//This cycles through the linked list to the last Arc
	while (arcs.GetWorkArc()->GetName() != "NULL")
	{
		//If the link between the Head Node and the Tail Node is greater than the current max link
		if (ArcLength(arcs.GetWorkArc()->GetHeadNode()->GetLatitude(), arcs.GetWorkArc()->GetHeadNode()->GetLongitude(), arcs.GetWorkArc()->GetTailNode()->GetLatitude(), arcs.GetWorkArc()->GetTailNode()->GetLongitude()) > maxLink)
		{
			//This sets the consition of the method to successful
			successful = true;

			//This changes the max link to the new max link
			maxLink = ArcLength(arcs.GetWorkArc()->GetHeadNode()->GetLatitude(), arcs.GetWorkArc()->GetHeadNode()->GetLongitude(), arcs.GetWorkArc()->GetTailNode()->GetLatitude(), arcs.GetWorkArc()->GetTailNode()->GetLongitude());

			//This records the Nodes that the max link spans
			headReference = arcs.GetWorkArc()->GetHeadNode()->GetReference();
			tailReference = arcs.GetWorkArc()->GetTailNode()->GetReference();
		}

		arcs.SetWorkArc(arcs.GetWorkArc()->GetNextArc());
	}

	//This outputs the results to the output file
	m_outFile << commandEntered << endl;
	m_outFile << headReference << "," << tailReference << "," << setprecision(3) << maxLink << endl;
	m_outFile << endl;

	//This returns the condition of the method
	return successful;
}

//This method finds the distance between two Nodes
bool Navigation::FindDist(const string &commandEntered)
{
	//These hold different snippets of the command
	string methodCommand = commandEntered;
	string delimiter = " ";
	size_t splitPosition = 0;
	string splitString;

	//This holds the found distance
	float distance = 0;

	//This holds the reference of the Nodes that the distance spans
	int headReference = 0;
	int tailReference = 0;

	//This splits the command by blank space and places what is split into the relevant variables
	for (size_t i = 0; i < 3; i++)
	{
		splitPosition = methodCommand.find(delimiter);
		splitString = methodCommand.substr(0, splitPosition);

		if (i == 1)
		{
			headReference = atoi(methodCommand.c_str());
		}

		if (i == 2)
		{
			tailReference = atoi(methodCommand.c_str());
		}

		methodCommand.erase(0, splitPosition + delimiter.length());
	}

	//This sets the Work Node to the Node with the same reference as the head reference
	nodes.SetWorkNode(nodes.SearchForNode(headReference));

	//These set the relevant variables with the information attained from the head reference
	string headName = nodes.GetWorkNode()->GetName();

	float latitude = nodes.GetWorkNode()->GetLatitude();
	float longitude = nodes.GetWorkNode()->GetLongitude();

	//This sets the Work Node to the Node with the same reference as the head reference
	nodes.SetWorkNode(nodes.SearchForNode(tailReference));

	//These set the relevant variables with the information attained from the tail reference
	string tailName = nodes.GetWorkNode()->GetName();

	//If either variables hold no information the method returns false
	if (headName.empty() || tailName.empty())
	{
		return false;
	}

	//This gets the distance between the Head Node and the Tail Node
	distance = ArcLength(latitude, longitude, nodes.GetWorkNode()->GetLatitude(), nodes.GetWorkNode()->GetLongitude());

	//This outputs the results to the output file
	m_outFile << commandEntered << endl;
	m_outFile << headName << "," << tailName << "," << setprecision(3) << distance << endl;
	m_outFile << endl;

	return true;
}

//This method finds all the Nodes that are connected to one Node by an Arc
bool Navigation::FindNeighbour(const string &commandEntered, const string &checkName, bool write)
{
	//This is a temporary pointer to a list of Arcs
	Arc *findNeighbourArc;

	//These hold different snippets of the command
	string methodCommand = commandEntered;
	string delimiter = " ";
	size_t splitPosition = 0;
	string splitString;

	//This is a reference to the original Node
	int reference;

	//This holds to condition of the method
	bool successful = false;

	//This splits the command by blank space and places what is split into the relevant variables
	for (size_t i = 0; i < 2; i++)
	{
		splitPosition = methodCommand.find(delimiter);
		splitString = methodCommand.substr(0, splitPosition);

		if (i == 1)
		{
			reference = atoi(methodCommand.c_str());
		}

		methodCommand.erase(0, splitPosition + delimiter.length());
	}

	//If it is required the relevant information is written to the output file
	if (write)
	{
		m_outFile << commandEntered << endl;
	}

	//This sets the temporary pointer to the Head Node
	findNeighbourArc = arcs.GetHeadArc();

	//This cycles through the linked list to the last Arc
	while (findNeighbourArc->GetName() != "NULL")
	{
		//If the Head Node of the list is the Node being searched for
		if (findNeighbourArc->GetHeadNode()->GetReference() == reference)
		{
			//If it is required the relevant information is written to the output file and the condition of the method is set to successful
			if (write)
			{
				successful = true;

				m_outFile << findNeighbourArc->GetTailNode()->GetReference() << endl;
			}
			else
			{
				//This is used in FindRoute

				//This constructs a command to be used in Check
				string checkCommandEntered = "NULL " + checkName + " " + methodCommand + " " + to_string(findNeighbourArc->GetTailNode()->GetReference());

				//If the node has not been added previously and it is of a type that is relevant
				if (neighbour.SearchForNode(findNeighbourArc->GetTailNode()->GetReference()) == NULL && Check(checkCommandEntered, false))
				{
					//This addes Nodes and Arcs to the relevant lists for FindRoute
					arcs.SetWorkArc(findNeighbourArc);

					neighbour.AddNode(arcs.GetWorkArc()->GetTailNode()->GetName(), arcs.GetWorkArc()->GetTailNode()->GetReference(), arcs.GetWorkArc()->GetTailNode()->GetLatitude(), arcs.GetWorkArc()->GetTailNode()->GetLongitude());

					neighbour.SetWorkNode(nodes.SearchForNode(arcs.GetWorkArc()->GetTailNode()->GetReference()));

					route.AddArc(checkName, nodes.SearchForNode(atoi(methodCommand.c_str())), neighbour.GetWorkNode());
					
					//This returns to FindRoute
					return true;
				}
			}
		}
		else
		{
			//If the Tail Node of the list is the Node being searched for
			if (findNeighbourArc->GetTailNode()->GetReference() == reference)
			{
				//If it is required the relevant information is written to the output file and the condition of the method is set to successful
				if (write)
				{
					successful = true;

					m_outFile << findNeighbourArc->GetHeadNode()->GetReference() << endl;
				}
				else
				{
					//This constructs a command to be used in Check
					string checkCommandEntered = "NULL " + checkName + " " + methodCommand + " " + to_string(findNeighbourArc->GetHeadNode()->GetReference());

					//If the node has not been added previously and it is of a type that is relevant
					if (neighbour.SearchForNode(findNeighbourArc->GetHeadNode()->GetReference()) == NULL && Check(checkCommandEntered, false))
					{
						//This addes Nodes and Arcs to the relevant lists for FindRoute
						arcs.SetWorkArc(findNeighbourArc);

						neighbour.AddNode(arcs.GetWorkArc()->GetHeadNode()->GetName(), arcs.GetWorkArc()->GetHeadNode()->GetReference(), arcs.GetWorkArc()->GetHeadNode()->GetLatitude(), arcs.GetWorkArc()->GetHeadNode()->GetLongitude());

						neighbour.SetWorkNode(nodes.SearchForNode(arcs.GetWorkArc()->GetHeadNode()->GetReference()));

						route.AddArc(checkName, nodes.SearchForNode(atoi(methodCommand.c_str())), neighbour.GetWorkNode());
						
						//This returns to FindRoute
						return true;
					}
				}
			}
		}

		findNeighbourArc = findNeighbourArc->GetNextArc();
	}

	//If it is required the relevant information is written to the output file
	if (write)
	{
		m_outFile << endl;
	}

	//This returns the condition of the method
	return successful;
}

//These methods check if an Arc can be traveled via a certain transport method
bool Navigation::Check(const string &commandEntered, bool write)
{
	//These hold different snippets of the command
	string methodCommand = commandEntered;
	string name;
	string delimiter = " ";
	size_t splitPosition = 0;
	string splitString;
	string previousSplitString;

	size_t i = 0;

	//If it is required the relevant information is written to the output file
	if (write)
	{
		m_outFile << commandEntered << endl;
	}

	//While there are still places to split this splits the command by blank space and places what is split into the relevant variables
	while (splitPosition != -1)
	{
		splitPosition = methodCommand.find(delimiter);
		splitString = methodCommand.substr(0, splitPosition);

		if (i == 1)
		{
			name = splitString;
		}

		if (i > 2 && atoi(splitString.c_str()) != NULL)
		{
			//If the transport type is Foot this calls the method CheckFoot
			if (name == "Foot")
			{
				if (!CheckFoot(splitString, previousSplitString, write))
				{
					return false;
				}
			}

			//If the transport type is Foot this calls the method CheckBike
			if (name == "Bike")
			{
				if (!CheckBike(splitString, previousSplitString, write))
				{
					return false;
				}
			}

			//If the transport type is Foot this calls the method CheckCar
			if (name == "Car")
			{
				if (!CheckCar(splitString, previousSplitString, write))
				{
					return false;
				}
			}

			//If the transport type is Foot this calls the method CheckBus
			if (name == "Bus")
			{
				if (!CheckBus(splitString, previousSplitString, write))
				{
					return false;
				}
			}

			//If the transport type is Foot this calls the method CheckRail
			if (name == "Rail")
			{
				if (!CheckRail(splitString, previousSplitString, write))
				{
					return false;
				}
			}

			//If the transport type is Foot this calls the method CheckShip
			if (name == "Ship")
			{
				if (!CheckShip(splitString, previousSplitString, write))
				{
					return false;
				}
			}
		}

		previousSplitString = splitString;

		methodCommand.erase(0, splitPosition + delimiter.length());

		i++;
	}

	//If it is required the relevant information is written to the output file
	if (write)
	{
		m_outFile << endl;
	}

	return true;
}

bool Navigation::CheckFoot(const string &splitString, const string &previousSplitString, bool write)
{
	//This holds to condition of the method
	bool successful = false;

	//This sets the Work Arc to the Head Node
	arcs.SetWorkArc(arcs.GetHeadArc());

	//This cycles through the linked list to the last Arc
	while (arcs.GetWorkArc()->GetName() != "NULL")
	{
		//If the Head Node equals the Head Node and the Tail Node equals the Tail Node or the Head Node equals the Tail Node and the Tail Node equals the Head Node
		if ((arcs.GetWorkArc()->GetHeadNode()->GetReference() == atoi(splitString.c_str()) && arcs.GetWorkArc()->GetTailNode()->GetReference() == atoi(previousSplitString.c_str())) || (arcs.GetWorkArc()->GetHeadNode()->GetReference() == atoi(previousSplitString.c_str()) && arcs.GetWorkArc()->GetTailNode()->GetReference() == atoi(splitString.c_str())))
		{
			//This sets the consition of the method to successful
			successful = true;

			break;
		}

		arcs.SetWorkArc(arcs.GetWorkArc()->GetNextArc());
	}

	//If the condition of the method is successful
	if (successful)
	{
		//If it is required the relevant information is written to the output file
		if (write)
		{
			m_outFile << previousSplitString << "," << splitString << "," << "PASS" << endl;
		}
	}
	else
	{
		//If it is required the relevant information is written to the output file
		if (write)
		{
			m_outFile << previousSplitString << "," << splitString << "," << "FAIL" << endl;
			m_outFile << endl;
		}
	}

	//This returns the condition of the method
	return successful;
}

bool Navigation::CheckBike(const string &splitString, const string &previousSplitString, bool write)
{
	//This holds to condition of the method
	bool successful = false;

	//This sets the Work Arc to the Head Node
	arcs.SetWorkArc(arcs.GetHeadArc());

	//This cycles through the linked list to the last Arc
	while (arcs.GetWorkArc()->GetName() != "NULL")
	{
		//If the method of transport is a relevant method and the Head Node equals the Head Node and the Tail Node equals the Tail Node or the Head Node equals the Tail Node and the Tail Node equals the Head Node
		if (arcs.GetWorkArc()->GetName() != "Foot" && ((arcs.GetWorkArc()->GetHeadNode()->GetReference() == atoi(splitString.c_str()) && arcs.GetWorkArc()->GetTailNode()->GetReference() == atoi(previousSplitString.c_str())) || (arcs.GetWorkArc()->GetHeadNode()->GetReference() == atoi(previousSplitString.c_str()) && arcs.GetWorkArc()->GetTailNode()->GetReference() == atoi(splitString.c_str()))))
		{
			//This sets the consition of the method to successful
			successful = true;

			break;
		}

		arcs.SetWorkArc(arcs.GetWorkArc()->GetNextArc());
	}

	//If the condition of the method is successful
	if (successful)
	{
		//If it is required the relevant information is written to the output file
		if (write)
		{
			m_outFile << previousSplitString << "," << splitString << "," << "PASS" << endl;
		}
	}
	else
	{
		//If it is required the relevant information is written to the output file
		if (write)
		{
			m_outFile << previousSplitString << "," << splitString << "," << "FAIL" << endl;
			m_outFile << endl;
		}
	}

	//This returns the condition of the method
	return successful;
}

bool Navigation::CheckCar(const string &splitString, const string &previousSplitString, bool write)
{
	//This holds to condition of the method
	bool successful = false;

	//This sets the Work Arc to the Head Node
	arcs.SetWorkArc(arcs.GetHeadArc());

	//This cycles through the linked list to the last Arc
	while (arcs.GetWorkArc()->GetName() != "NULL")
	{
		//If the method of transport is a relevant method and the Head Node equals the Head Node and the Tail Node equals the Tail Node or the Head Node equals the Tail Node and the Tail Node equals the Head Node
		if ((arcs.GetWorkArc()->GetName() != "Foot" && arcs.GetWorkArc()->GetName() != "Bike") && ((arcs.GetWorkArc()->GetHeadNode()->GetReference() == atoi(splitString.c_str()) && arcs.GetWorkArc()->GetTailNode()->GetReference() == atoi(previousSplitString.c_str())) || (arcs.GetWorkArc()->GetHeadNode()->GetReference() == atoi(previousSplitString.c_str()) && arcs.GetWorkArc()->GetTailNode()->GetReference() == atoi(splitString.c_str()))))
		{
			//This sets the consition of the method to successful
			successful = true;

			break;
		}

		arcs.SetWorkArc(arcs.GetWorkArc()->GetNextArc());
	}

	//If the condition of the method is successful
	if (successful)
	{
		//If it is required the relevant information is written to the output file
		if (write)
		{
			m_outFile << previousSplitString << "," << splitString << "," << "PASS" << endl;
		}
	}
	else
	{
		//If it is required the relevant information is written to the output file
		if (write)
		{
			m_outFile << previousSplitString << "," << splitString << "," << "FAIL" << endl;
			m_outFile << endl;
		}
	}

	//This returns the condition of the method
	return successful;
}

bool Navigation::CheckBus(const string &splitString, const string &previousSplitString, bool write)
{
	//This holds to condition of the method
	bool successful = false;

	//This sets the Work Arc to the Head Node
	arcs.SetWorkArc(arcs.GetHeadArc());

	//This cycles through the linked list to the last Arc
	while (arcs.GetWorkArc()->GetName() != "NULL")
	{
		//If the method of transport is a relevant method and the Head Node equals the Head Node and the Tail Node equals the Tail Node or the Head Node equals the Tail Node and the Tail Node equals the Head Node
		if ((arcs.GetWorkArc()->GetName() != "Foot" && arcs.GetWorkArc()->GetName() != "Bike" && arcs.GetWorkArc()->GetName() != "Car") && ((arcs.GetWorkArc()->GetHeadNode()->GetReference() == atoi(splitString.c_str()) && arcs.GetWorkArc()->GetTailNode()->GetReference() == atoi(previousSplitString.c_str())) || (arcs.GetWorkArc()->GetHeadNode()->GetReference() == atoi(previousSplitString.c_str()) && arcs.GetWorkArc()->GetTailNode()->GetReference() == atoi(splitString.c_str()))))
		{
			successful = true;

			break;
		}

		arcs.SetWorkArc(arcs.GetWorkArc()->GetNextArc());
	}

	//If the condition of the method is successful
	if (successful)
	{
		//If it is required the relevant information is written to the output file
		if (write)
		{
			m_outFile << previousSplitString << "," << splitString << "," << "PASS" << endl;
		}
	}
	else
	{
		//If it is required the relevant information is written to the output file
		if (write)
		{
			m_outFile << previousSplitString << "," << splitString << "," << "FAIL" << endl;
			m_outFile << endl;
		}
	}

	//This returns the condition of the method
	return successful;
}

bool Navigation::CheckRail(const string &splitString, const string &previousSplitString, bool write)
{
	//This holds to condition of the method
	bool successful = false;

	//This sets the Work Arc to the Head Node
	arcs.SetWorkArc(arcs.GetHeadArc());

	//This cycles through the linked list to the last Arc
	while (arcs.GetWorkArc()->GetName() != "NULL")
	{
		//If the method of transport is a relevant method and the Head Node equals the Head Node and the Tail Node equals the Tail Node or the Head Node equals the Tail Node and the Tail Node equals the Head Node
		if ((arcs.GetWorkArc()->GetName() != "Foot" && arcs.GetWorkArc()->GetName() != "Bike" && arcs.GetWorkArc()->GetName() != "Car" && arcs.GetWorkArc()->GetName() != "Bus" && arcs.GetWorkArc()->GetName() != "Ship") && ((arcs.GetWorkArc()->GetHeadNode()->GetReference() == atoi(splitString.c_str()) && arcs.GetWorkArc()->GetTailNode()->GetReference() == atoi(previousSplitString.c_str())) || (arcs.GetWorkArc()->GetHeadNode()->GetReference() == atoi(previousSplitString.c_str()) && arcs.GetWorkArc()->GetTailNode()->GetReference() == atoi(splitString.c_str()))))
		{
			successful = true;

			break;
		}

		arcs.SetWorkArc(arcs.GetWorkArc()->GetNextArc());
	}

	//If the condition of the method is successful
	if (successful)
	{
		//If it is required the relevant information is written to the output file
		if (write)
		{
			m_outFile << previousSplitString << "," << splitString << "," << "PASS" << endl;
		}
	}
	else
	{
		//If it is required the relevant information is written to the output file
		if (write)
		{
			m_outFile << previousSplitString << "," << splitString << "," << "FAIL" << endl;
			m_outFile << endl;
		}
	}

	//This returns the condition of the method
	return successful;
}

bool Navigation::CheckShip(const string &splitString, const string &previousSplitString, bool write)
{
	//This holds to condition of the method
	bool successful = false;

	//This sets the Work Arc to the Head Node
	arcs.SetWorkArc(arcs.GetHeadArc());

	//This cycles through the linked list to the last Arc
	while (arcs.GetWorkArc()->GetName() != "NULL")
	{
		//If the method of transport is a relevant method and the Head Node equals the Head Node and the Tail Node equals the Tail Node or the Head Node equals the Tail Node and the Tail Node equals the Head Node
		if ((arcs.GetWorkArc()->GetName() != "Foot" && arcs.GetWorkArc()->GetName() != "Bike" && arcs.GetWorkArc()->GetName() != "Car" && arcs.GetWorkArc()->GetName() != "Bus" && arcs.GetWorkArc()->GetName() != "Rail") && ((arcs.GetWorkArc()->GetHeadNode()->GetReference() == atoi(splitString.c_str()) && arcs.GetWorkArc()->GetTailNode()->GetReference() == atoi(previousSplitString.c_str())) || (arcs.GetWorkArc()->GetHeadNode()->GetReference() == atoi(previousSplitString.c_str()) && arcs.GetWorkArc()->GetTailNode()->GetReference() == atoi(splitString.c_str()))))
		{
			successful = true;

			break;
		}

		arcs.SetWorkArc(arcs.GetWorkArc()->GetNextArc());
	}

	//If the condition of the method is successful
	if (successful)
	{
		//If it is required the relevant information is written to the output file
		if (write)
		{
			m_outFile << previousSplitString << "," << splitString << "," << "PASS" << endl;
		}
	}
	else
	{
		//If it is required the relevant information is written to the output file
		if (write)
		{
			m_outFile << previousSplitString << "," << splitString << "," << "FAIL" << endl;
			m_outFile << endl;
		}
	}

	//This returns the condition of the method
	return successful;
}

//This method finds a route between two nodes
bool Navigation::FindRoute(const string &commandEntered)
{
	//These hold different snippets of the command
	string methodCommand = commandEntered;
	string delimiter = " ";
	string splitString, checkName;

	int splitPosition = 0;
	int headReference, tailReference;

	m_outFile << commandEntered << endl;

	//This splits the command by blank space and places what is split into the relevant variables
	for (size_t i = 0; i < 3; i++)
	{
		splitPosition = methodCommand.find(delimiter);
		splitString = methodCommand.substr(0, splitPosition);

		if (i == 1)
		{
			checkName = splitString;
		}

		if (i == 2)
		{
			tailReference = atoi(splitString.c_str());
		}

		methodCommand.erase(0, splitPosition + delimiter.length());
	}

	headReference = atoi(methodCommand.c_str());

	//This adds the Head Node to the neighbour linked list
	neighbour.AddNode(nodes.SearchForNode(headReference)->GetName(), nodes.SearchForNode(headReference)->GetReference(), nodes.SearchForNode(headReference)->GetLatitude(), nodes.SearchForNode(headReference)->GetLongitude());

	//This sets the Work Node to the Head Node
	neighbour.SetWorkNode(neighbour.GetHeadNode());

	//If the Head Node equals the Tail Node this exits the method
	if (neighbour.GetHeadNode()->GetReference() == tailReference)
	{
		m_outFile << neighbour.GetHeadNode()->GetReference() << endl;
		m_outFile << endl;

		return true;
	}
	else
	{
		//This cycles through the linked list to the last Node
		while (neighbour.GetWorkNode()->GetName() != "NULL")
		{
			//If the current Node equals the Tail Node
			if (neighbour.GetWorkNode()->GetReference() == tailReference)
			{
				//This sets the Work Arc to the Head Node
				route.SetWorkArc(route.GetHeadArc()->GetNextArc());

				//This holds the output string
				string output = "";

				//This cycles through the linked list to the last Arc
				while (route.GetWorkArc()->GetName() != "NULL")
				{
					//If the current Node equals the Tail Node
					if (route.GetWorkArc()->GetTailNode()->GetReference() == tailReference)
					{
						//Adds the current Node to the output string
						output = output + to_string(route.GetWorkArc()->GetHeadNode()->GetReference());;

						break;
					}
					else
					{
						//Adds the current Node to the output string
						output = output + to_string(route.GetWorkArc()->GetHeadNode()->GetReference()) + " ";

						route.SetWorkArc(route.GetWorkArc()->GetNextArc());
					}
				}
				
				m_outFile << to_string(tailReference) << endl;

				//While there are still places to split this splits the command by blank space and places what is split into the relevant variables
				while (splitPosition > 0)
				{
					splitPosition = output.find_last_of(delimiter);

					if (splitPosition > 0)
					{
						splitString = output.substr(splitPosition, output.length());

						splitString = splitString.substr(1, splitString.length());

						m_outFile << splitString << endl;

						output.erase(splitPosition, output.length());
					}
				}
				
				//If the output does not equal the Head Node add the Head Node to the output file
				if (output == to_string(headReference))
				{
					m_outFile << output << endl;
					m_outFile << endl;
				}
				else
				{
					m_outFile << output << endl;
					m_outFile << headReference << endl;
					m_outFile << endl;
				}
				
				return true;
			}

			//This constructs a command to be used in FindNeighbour
			string findNeighbourCommandEntered = "NULL " + to_string(neighbour.GetWorkNode()->GetReference());

			//If there are no more routes to take from the current Node move back one and delete the current Arc
			if (!FindNeighbour(findNeighbourCommandEntered, checkName, false))
			{
				neighbour.SetWorkNode(neighbour.GetWorkNode()->GetPreviousNode());

				route.RemoveArc();
			}
		}
	}

	m_outFile << "FAIL" << endl;
	m_outFile << endl;

	return false;
}

// ###################################################################################
// ##                                                                               ##
// ##  The following methods are used to calculate the distance between two nodes.  ##
// ##  DO NOT edit either of these methods!                                         ##
// ##                                                                               ##
// ###################################################################################

// **** DO NOT EDIT THIS METHOD ****
float Navigation::ArcLength(float startNorth, float startEast, float endNorth, float endEast) const
{
	float UTMNorthingStart;
	float UTMEastingStart;
	float UTMNorthingEnd;
	float UTMEastingEnd;

	LLtoUTM(startNorth, startEast, UTMNorthingStart, UTMEastingStart);
	LLtoUTM(endNorth, endEast, UTMNorthingEnd, UTMEastingEnd);

	float dNorth = UTMNorthingEnd - UTMNorthingStart;
	float dEast = UTMEastingEnd - UTMEastingStart;

	return sqrt(dNorth*dNorth + dEast*dEast) * 0.001f;
}

// **** DO NOT EDIT THIS METHOD ****
void Navigation::LLtoUTM(const float Lat, const float Long, float &UTMNorthing, float &UTMEasting) const
{
	//converts lat/long to UTM coords.  Equations from USGS Bulletin 1532 
	//East Longitudes are positive, West longitudes are negative. 
	//North latitudes are positive, South latitudes are negative
	//Lat and Long are in decimal degrees
	//Original code by Chuck Gantz- chuck.gantz@globalstar.com (http://www.gpsy.com/gpsinfo/geotoutm/)
	//Adapted by Darren McKie

	const float PI = 3.14159265f, FOURTHPI = PI / 4.0f, DEG2RAD = PI / 180.0f;

	float a = 6378137;
	float eccSquared = 0.00669438f;
	float k0 = 0.9996f;

	float LongOrigin;
	float eccPrimeSquared;
	float N, T, C, A, M;

	//Make sure the longitude is between -180.00 .. 179.9
	float LongTemp = (Long + 180) - int((Long + 180) / 360) * 360 - 180; // -180.00 .. 179.9;

	float LatRad = Lat*DEG2RAD;
	float LongRad = LongTemp*DEG2RAD;
	float LongOriginRad;
	int   ZoneNumber = 30;

	LongOrigin = (ZoneNumber - 1) * 6.0f - 180 + 3;  //+3 puts origin in middle of zone
	LongOriginRad = LongOrigin * DEG2RAD;

	eccPrimeSquared = (eccSquared) / (1 - eccSquared);

	N = a / sqrt(1 - eccSquared*sin(LatRad)*sin(LatRad));
	T = tan(LatRad)*tan(LatRad);
	C = eccPrimeSquared*cos(LatRad)*cos(LatRad);
	A = cos(LatRad)*(LongRad - LongOriginRad);

	M = a*((1 - eccSquared / 4 - 3 * eccSquared*eccSquared / 64 - 5 * eccSquared*eccSquared*eccSquared / 256)*LatRad
		- (3 * eccSquared / 8 + 3 * eccSquared*eccSquared / 32 + 45 * eccSquared*eccSquared*eccSquared / 1024)*sin(2 * LatRad)
		+ (15 * eccSquared*eccSquared / 256 + 45 * eccSquared*eccSquared*eccSquared / 1024)*sin(4 * LatRad)
		- (35 * eccSquared*eccSquared*eccSquared / 3072)*sin(6 * LatRad));

	UTMEasting = (k0*N*(A + (1 - T + C)*A*A*A / 6
		+ (5 - 18 * T + T*T + 72 * C - 58 * eccPrimeSquared)*A*A*A*A*A / 120)
		+ 500000.0f);

	UTMNorthing = (k0*(M + N*tan(LatRad)*(A*A / 2 + (5 - T + 9 * C + 4 * C*C)*A*A*A*A / 24
		+ (61 - 58 * T + T*T + 600 * C - 330 * eccPrimeSquared)*A*A*A*A*A*A / 720)));
	if (Lat < 0)
		UTMNorthing += 10000000.0f; //10000000 meter offset for southern hemisphere
}
