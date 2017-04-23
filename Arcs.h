//This ensures that this source file is included in the solution only once
#pragma once

//This includes the Arc header file
#include "Arc.h"

class Arcs
{
//Things from this point on are only accessible from within this class
private:

	//These are pointers to lists of Arcs
	Arc *headArc, *workArc;

//Things from this point on are accessible from outside this class
public:

	//These are the initialisers for the constructor, destructor, copy constructor and copy assignment constructor
	Arcs(void);
	~Arcs(void);
	Arcs(const Arcs &other);
	Arcs &operator = (const Arcs &other);

	//This is the initialiser for the method to add a new Arc to the linked list
	void AddArc(const string &name, Node *headNode, Node *tailNode);
	//This is the initialiser for the method to remove an Arc from the linked list
	void RemoveArc(void) const;

	//These are the initialisers for the accessors for the head Arc of the linked list
	Arc *GetHeadArc(void) const;
	void SetHeadArc(Arc *arc);

	//These are the initialisers for the accessors for the work Arc of the linked list
	Arc *GetWorkArc(void) const;
	void SetWorkArc(Arc *arc);
};