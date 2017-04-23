//This includes the Node header file
#include "Node.h"

//This is the constructor for Node
Node::Node(void) :name(NULL), nextNode(NULL), previousNode(NULL), reference(NULL), latitude(NULL), longitude(NULL)
{

}

//This is the destructor for Node
Node::~Node(void)
{

}

//This is the copy constructor for Node
Node::Node(const Node &other) :name(other.name), nextNode(other.nextNode), previousNode(other.previousNode), reference(other.reference), latitude(other.latitude), longitude(other.longitude)
{
	
}

//This is the copy assignment constructor for Node
Node &Node::operator = (const Node &other)
{
	//This ensures there is something to copy
	if (this != &other)
	{
		//This creates a temporary instance of the thing to be copied
		string newName;
		Node *newNextNode = new Node;
		Node *newPreviousNode = new Node;
		int newReference;
		float newLatitude;
		float newLongitude;

		//This adds the thing to be copied into the temporary instance of itself
		newName = other.name;
		newNextNode = other.nextNode;
		newPreviousNode = other.previousNode;
		newReference = other.reference;
		newLatitude = other.latitude;
		newLongitude = other.longitude;

		//This deletes the previous instance of the thing to be copied
		delete &name;
		delete nextNode;
		delete previousNode;
		delete &reference;
		delete &latitude;
		delete &longitude;

		//This moves the new instance from temporary memory into none temporary memory
		name = newName;
		nextNode = newNextNode;
		previousNode = newPreviousNode;
		reference = newReference;
		latitude = newLatitude;
		longitude = newLongitude;
	}

	//This returns the new variables
	return *this;
}

//This creates a new Node
Node::Node(const string & name, int reference, float latitude, float longitude) : nextNode(NULL), previousNode(NULL)
{
	SetName(name);
	SetReference(reference);
	SetLatitude(latitude);
	SetLongitude(longitude);
}

//This is the accessor for the Nodes name
string Node::GetName(void) const
{
	return name;
}

//This is the mutator for the Nodes name
void Node::SetName(const string & string)
{
	name = string;
}

//This is the accessor for the next Node in the linked list
Node * Node::GetNextNode(void) const
{
	return nextNode;
}

//This is the mutator for the next Node in the linked list
void Node::SetNextNode(Node * node)
{
	nextNode = node;
}

//This is the accessor for the previous Node in the linked list
Node * Node::GetPreviousNode(void) const
{
	return previousNode;
}

//This is the mutator for the previous Node in the linked list
void Node::SetPreviousNode(Node * node)
{
	previousNode = node;
}

//This is the accessor for the Nodes reference
int Node::GetReference(void) const
{
	return reference;
}

//This is the mutator for the Nodes reference
void Node::SetReference(int intager)
{
	reference = intager;
}

//This is the accessor for the Nodes latitude
float Node::GetLatitude(void) const
{
	return latitude;
}

//This is the mutator for the Nodes latitude
void Node::SetLatitude(float floatingPoint)
{
	latitude = floatingPoint;
}

//This is the accessor for the Nodes longitude
float Node::GetLongitude(void) const
{
	return longitude;
}

//This is the mutator for the Nodes longitude
void Node::SetLongitude(float floatingPoint)
{
	longitude = floatingPoint;
}