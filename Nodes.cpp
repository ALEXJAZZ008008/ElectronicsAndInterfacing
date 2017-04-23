//This includes the Nodes header file
#include "Nodes.h"

//This is the constructor for the Node list
Nodes::Nodes(void) : headNode(NULL), workNode(NULL)
{

}

//This is the destructor for the Node list
Nodes::~Nodes(void)
{
	//These delete the pointers to Node lists
	delete headNode;
	delete workNode;
}

//This is the copy constructor for the Node list
Nodes::Nodes(const Nodes & other) :headNode(other.headNode), workNode(other.workNode)
{

}

//This is the copy assignment constructor for the Node list
Nodes &Nodes::operator = (const Nodes &other)
{
	//This ensures there is something to copy
	if (this != &other)
	{
		//This creates a temporary instance of the thing to be copied
		Node *newheadNode = new Node;
		Node *newWorkNode = new Node;

		//This adds the thing to be copied into the temporary instance of itself
		newheadNode = other.headNode;
		newWorkNode = other.workNode;

		//This deletes the previous instance of the thing to be copied
		delete headNode;
		delete workNode;

		//This moves the new instance from temporary memory into none temporary memory
		headNode = newheadNode;
		workNode = newWorkNode;
	}

	//This returns the new variables
	return *this;
}

//This adds a new Node to the linked list
void Nodes::AddNode(const string &name, int reference, float latitude, float longitude)
{
	//If there is no Nodes in the linked list the first Node added becomes the head of the linked list
	if (headNode == NULL)
	{
		headNode = new Node(name, reference, latitude, longitude);
		headNode->SetNextNode(new Node("NULL", NULL, NULL, NULL));
		headNode->SetPreviousNode(new Node("NULL", NULL, NULL, NULL));
	}
	else
	{
		//This is a temporary Node used to search the linked list
		Node *addNode = headNode;

		//This cycles through the linked list to the last Node
		while (addNode->GetNextNode()->GetName() != "NULL")
		{
			addNode = addNode->GetNextNode();
		}

		//This is a temporary Node that holds the previous node
		Node *previousNode = addNode;

		//This addes the new Node
		addNode->SetNextNode(new Node(name, reference, latitude, longitude));
		addNode->GetNextNode()->SetNextNode(new Node("NULL", NULL, NULL, NULL));
		addNode->GetNextNode()->SetPreviousNode(previousNode);
	}
}

//This searches the linked list for a specific node
Node *Nodes::SearchForNode(int refrence) const
{
	//This is the result of the search
	Node *bufferNode = NULL;
	//This is a temporary Node used to search the linked list
	Node *searchNode = headNode;

	//This cycles through the linked list to the last Node
	while (searchNode->GetName() != "NULL")
	{
		//This checks to see if the current Node is the Node being searched for
		if (searchNode->GetReference() == refrence)
		{
			//This is the result of the search
			bufferNode = searchNode;
		}

		searchNode = searchNode->GetNextNode();
	}

	//This returns a result
	return bufferNode;
}

//This is the accessor for the first Node in the linked list
Node *Nodes::GetHeadNode(void) const
{
	return *&headNode;
}

//This is the mutator for the first Node in the linked list
void Nodes::SetHeadNode(Node *node)
{
	headNode = node;
}

//This is the accessor for a Node in the linked list
Node *Nodes::GetWorkNode(void) const
{
	return *&workNode;
}

//This is the mutator for a Node in the linked list
void Nodes::SetWorkNode(Node *node)
{
	workNode = node;
}