//This includes the Arc header file
#include "Arc.h"

//This is the constructor for Arc
Arc::Arc(void) :name(NULL), nextArc(NULL), headNode(NULL), tailNode(NULL)
{

}

//This is the destructor for Arc
Arc::~Arc(void)
{
	
}

//This is the copy constructor for Arc
Arc::Arc(const Arc &other) :name(other.name), nextArc(other.nextArc), headNode(other.headNode), tailNode(other.tailNode)
{

}

//This is the copy assignment constructor for Arc
Arc &Arc::operator = (const Arc &other)
{
	//This ensures there is something to copy
	if (this != &other)
	{
		//This creates a temporary instance of the thing to be copied
		string newName;
		Arc *newNextArc = new Arc;
		Node *newHeadNode = new Node;
		Node *newTailNode = new Node;

		//This adds the thing to be copied into the temporary instance of itself
		newName = other.name;
		newNextArc = other.nextArc;
		newHeadNode = other.headNode;
		newTailNode = other.tailNode;

		//This deletes the previous instance of the thing to be copied
		delete &name;
		delete nextArc;
		delete headNode;
		delete tailNode;

		//This moves the new instance from temporary memory into none temporary memory
		name = newName;
		nextArc = newNextArc;
		headNode = newHeadNode;
		tailNode = newTailNode;
	}

	//This returns the new variables
	return *this;
}

//This creates a new Arc
Arc::Arc(const string &name, Node *headNode, Node *tailNode) : nextArc(NULL)
{
	SetName(name);
	SetHeadNode(headNode);
	SetTailNode(tailNode);
}

//This is the accessor for the Arcs name
string Arc::GetName(void) const
{
	return name;
}

//This is the mutator for the Arcs name
void Arc::SetName(const string &string)
{
	name = string;
}

//This is the accessor for the next Arc in the linked list
Arc *Arc::GetNextArc(void) const
{
	return *&nextArc;
}

//This is the mutator for the next Arc in the linked list
void Arc::SetNextArc(Arc *arc)
{
	nextArc = arc;
}

//This is the accessor for the start position Node of the Arc
Node *Arc::GetHeadNode(void) const
{
	return *&headNode;
}

//This is the mutator for the start position Node of the Arc
void Arc::SetHeadNode(Node *node)
{
	headNode = node;
}

//This is the accessor for the destination Node of the Arc
Node *Arc::GetTailNode(void) const
{
	return *&tailNode;
}

//This is the mutator for the destination Node of the Arc
void Arc::SetTailNode(Node *node)
{
	tailNode = node;
}