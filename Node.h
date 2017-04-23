//This ensures that this source file is included in the solution only once
#pragma once

//This includes the string library
#include <string>

//This includes the standard library
using namespace std;

class Node
{
//Things from this point on are only accessible from within this class
private:

	//This is the name of the Node
	string name;

	//These are the next and previous Node in the linked list
	Node *nextNode, *previousNode;

	//This is the reference of the Node
	int reference;

	//These reperesent the position of the Node
	float latitude, longitude;

//Things from this point on are accessible from outside this class
public:

	//These are the initialisers for the constructor, destructor, copy constructor and copy assignment constructor
	Node(void);
	~Node(void);
	Node(const Node &other);
	Node &operator = (const Node &other);

	//This is the initialiser for the method to create a new Node
	Node(const string & name, int reference, float latitude, float longitude);

	//These are the initialisers for the accessors for the Nodes name
	string GetName(void) const;
	void SetName(const string & string);

	//These are the initialisers for the accessors for the next Node in the linked list
	Node * GetNextNode(void) const;
	void SetNextNode(Node * node);

	//These are the initialisers for the accessors for the previous Node in the linked list
	Node * GetPreviousNode(void) const;
	void SetPreviousNode(Node * node);

	//These are the initialisers for the accessors for the Nodes reference
	int GetReference(void) const;
	void SetReference(int intager);

	//These are the initialisers for the accessors for the Nodes latitude
	float GetLatitude(void) const;
	void SetLatitude(float floatingPoint);

	//These are the initialisers for the accessors for the Nodes longitude
	float GetLongitude(void) const;
	void SetLongitude(float floatingPoint);
};