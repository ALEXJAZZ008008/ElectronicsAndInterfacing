//This includes the Arcs header file
#include "Arcs.h"

//This is the constructor for the Arc list
Arcs::Arcs(void) :headArc(NULL), workArc(NULL)
{

}

//This is the destructor for the Arc list
Arcs::~Arcs(void)
{
	//These delete the pointers to Arc lists
	delete headArc;
	delete workArc;
}

//This is the copy constructor for the Node list
Arcs::Arcs(const Arcs &other) :headArc(other.headArc), workArc(other.workArc)
{

}

//This is the copy assignment constructor for the Arc list
Arcs &Arcs::operator = (const Arcs &other)
{
	//This ensures there is something to copy
	if (this != &other)
	{
		//This creates a temporary instance of the thing to be copied
		Arc *newHeadArc = new Arc;
		Arc *newWorkArc = new Arc;

		//This adds the thing to be copied into the temporary instance of itself
		newHeadArc = other.headArc;
		newWorkArc = other.workArc;

		//This deletes the previous instance of the thing to be copied
		delete headArc;
		delete workArc;

		//This moves the new instance from temporary memory into none temporary memory
		headArc = newHeadArc;
		workArc = newWorkArc;
	}

	//This returns the new variables
	return *this;
}

//This adds a new Arc to the linked list
void Arcs::AddArc(const string &name, Node *headNode, Node *tailNode)
{
	//If there is no Arcs in the linked list the first Arc added becomes the head of the linked list
	if (headArc == NULL)
	{
		headArc = new Arc(name, headNode, tailNode);
		headArc->SetNextArc(new Arc("NULL", NULL, NULL));
	}
	else
	{
		//This is a temporary Arc used to search the linked list
		Arc *addArc = headArc;

		//This cycles through the linked list to the last Arc
		while (addArc->GetNextArc()->GetName() != "NULL")
		{
			addArc = addArc->GetNextArc();
		}

		//This addes the new Arc
		addArc->SetNextArc(new Arc(name, headNode, tailNode));
		addArc->GetNextArc()->SetNextArc(new Arc("NULL", NULL, NULL));
	}
}

//This removes an Arc from the linked list
void Arcs::RemoveArc(void) const
{
	//If there are no Arcs in the linked list no Arcs are removed
	if (headArc != NULL)
	{
		//This is a temporary Arc used to search the linked list
		Arc *deleteArc = headArc;
		
		//This cycles through the linked list to the last Arc
		while (deleteArc->GetName() != "NULL")
		{
			//This checks to see if the next Arc is the last Arc
			if (deleteArc->GetNextArc()->GetName() == "NULL")
			{
				//This deletes the current Arc
				deleteArc->SetName("NULL");
				deleteArc->SetNextArc(NULL);
				deleteArc->SetHeadNode(NULL);
				deleteArc->SetTailNode(NULL);

				//This exits the method
				break;
			}

			deleteArc = deleteArc->GetNextArc();
		}
	}
}

//This is the accessor for the first Arc in the linked list
Arc *Arcs::GetHeadArc(void) const
{
	return *&headArc;
}

//This is the mutator for the first Arc in the linked list
void Arcs::SetHeadArc(Arc *arc)
{
	headArc = arc;
}

//This is the accessor for a Arc in the linked list
Arc *Arcs::GetWorkArc(void) const
{
	return *&workArc;
}

//This is the mutator for a Arc in the linked list
void Arcs::SetWorkArc(Arc *arc)
{
	workArc = arc;
}