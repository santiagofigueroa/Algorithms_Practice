using System;
using System.Collections.Generic;
using ContactManager.Filters;
using DataStructures;
using System.Linq;

namespace ContactManager
{
    public class ContactStore : IContactStore
    {

        // State level cache
        // HashTable<string, BinaryTree<Contact>> stateCache = new HashTable<string, BinaryTree<Contact>>();
        SortedList<Contact> contacts = new SortedList<Contact>();    

        int nextId = 1;

        //  As many data strucutures can implement IEnumerable interface .   
        public IEnumerable<Contact> Contacts
        {
            get
            {
                // We can do this as soreted list implements IEnum
                return contacts;
            }
        }

        public Contact Add(Contact contact)
        {
            int id = contact.ID.HasValue ? contact.ID.Value : nextId++;
            nextId = Math.Max(nextId, id + 1);

            Contact withId = Contact.CreateWithId(id, contact);

            Log.Verbose("Add: adding new contact with ID {0} ({1} {2})", withId.ID, withId.FirstName, withId.LastName);

            contacts.Add(withId);

            Log.Verbose("Add: complete ({0})", withId.ID);

            return withId;
        }

        public IEnumerable<Contact> Add(IEnumerable<Contact> contacts)
        {
            if (contacts == null)
            {
                Log.Error("Add: null contacts provided");
                throw new ArgumentNullException("contacts");
            }

            int beforeCount = this.contacts.Count;

            foreach (Contact c in contacts)
            {
                Add(c);
            }

            Log.Info("Added {0} contacts", this.contacts.Count - beforeCount);

            return Contacts;
        }

        public IEnumerable<Contact> Load(IEnumerable<Contact> newContacts)
        {
            nextId = 1;

            Add(newContacts);

            Log.Info("Loaded {0} contacts", contacts.Count);

            return contacts;
        }

        public bool Remove(ContactFieldFilter filter, out Contact removed)
        {
            Contact toRemove = Search(filter).FirstOrDefault();
            return Remove(toRemove, out removed);
        }

        public bool Remove(Contact contact, out Contact removed)
        {
            //  For compering objects is Null now.  
            if (contact.Equals(null)) {

                Log.Info("Remove Null contact provided" );
                throw new ArgumentNullException("");

            } else {

                if (contacts.Find(contact, out removed))
                {
                    contacts.Remove(removed);
                    Log.Info("Remove: removed contact {0} ({1} {2})", contact.ID.Value, contact.FirstName, contact.LastName);
                    return true; 

                }
            }

            Log.Warning("Remove: Contact not found.  No action taken.");
            removed = default;
            return false;
        }

        public IEnumerable<Contact> Search(ContactFieldFilter filter)
        {
            //Log.Verbose("Searching for contacts with filter: {0}", filter);

            return filter.Apply(this.Contacts);
        }
    }
}
