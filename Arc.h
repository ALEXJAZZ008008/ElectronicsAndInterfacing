//This ensures that this source file is included in the solution only once
#pragma once

//This includes the Nodes header file
#include "Nodes.h"

class Arc
{
//Things from this point on are only accessible from within this class
private:

	//This is the name of the Arc
	string name;

	//This is the next Arc in the linked list
	Arc *nextArc;

	//These are the Nodes that the Arc links
	Node *headNode, *tailNode;

public:

	//These are the initialisers for the constructor, destructor, copy constructor and copy assignment constructor
	Arc(void);
	~Arc(void);
	Arc(const Arc &other);
	Arc &operator = (const Arc &other);

	//This is the initialiser for the method to create a new Arc
	Arc(const string &name, Node *headNode, Node *tailNode);

	//These are the initialisers for the accessors for the Arcs name
	string GetName(void) const;
	void SetName(const string &string);

	//These are the initialisers for the accessors for the next Arc in the linked list
	Arc *GetNextArc(void) const;
	void SetNextArc(Arc *arc);

	//These are the initialisers for the accessors for the start position Node of the Arc
	Node *GetHeadNode(void) const;
	void SetHeadNode(Node *node);

	//These are the initialisers for the accessors for the destination Node of the Arc
	Node *GetTailNode(void) const;
	void SetTailNode(Node *node);
};