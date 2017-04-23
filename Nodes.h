//This ensures that this source file is included in the solution only once
#pragma once

//This includes the Node header file
#include "Node.h"

class Nodes
{
//Things from this point on are only accessible from within this class
private:

	//These are pointers to lists of Nodes
	Node *headNode, *workNode;

//Things from this point on are accessible from outside this class
public:

	//These are the initialisers for the constructor, destructor, copy constructor and copy assignment constructor
	Nodes(void);
	~Nodes(void);
	Nodes(const Nodes &other);
	Nodes &operator = (const Nodes &other);

	//This is the initialiser for the method to add a new Node to the linked list
	void AddNode(const string &name, int reference, float latitude, float longitude);

	//This is the initialiser for the method to search fo a Node in the linked list
	Node *SearchForNode(int refrence) const;

	//These are the initialisers for the accessors for the head Node of the linked list
	Node *GetHeadNode(void) const;
	void SetHeadNode(Node *node);

	//These are the initialisers for the accessors for the work Node of the linked list
	Node * GetWorkNode(void) const;
	void SetWorkNode(Node *node);
};