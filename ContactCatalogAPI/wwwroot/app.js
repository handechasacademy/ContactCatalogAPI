const API_URL = 'https://localhost:7008/api/contacts';

// Render contacts in the UI
function renderContacts(contacts) {
    const container = document.getElementById('contactList');
    container.innerHTML = '';

    if (contacts.length === 0) {
        container.innerHTML = '<p class="emptyState">No souls registered.</p>';
        return;
    }

    contacts.forEach(contact => {
        const contactCard = document.createElement('li');
        contactCard.innerHTML = `
            <span class="contactId">#${contact.id}</span>
            <span class="contactName">${contact.name}</span>
            <span class="contactEmail">${contact.email}</span>
            <div class="contactTags">${contact.tags.map(t => `<span class="tag">${t}</span>`).join('')}</div>
            <div class="contactActions">
                <button class="updateBtn" onclick="editContact(${contact.id})">Update</button>
                <button class="purgeBtn" onclick="deleteContact(${contact.id})">Purge</button>
            </div>
        `;
        container.appendChild(contactCard);
    });
}

// Load contacts from API

function loadContacts() {
    fetch(API_URL)
        .then(response => response.json())
            .then(data => renderContacts(data))
            .catch(error => console.error('Error loading contacts:', error));

}

// add contact
document.getElementById('addContactButton').addEventListener('click', () => {
    const name = document.getElementById('contactName').value.trim();
    const email = document.getElementById('contactEmail').value.trim();
    const tag = document.getElementById('contactTag').value.trim();

    if (!name || !email) {
        alert('Name and Email are required.');
        return;
    }
    
    fetch(API_URL, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ name, email, tag })
    })
        .then(response => {
            if (!response.ok) throw new Error('Failed to add contact');
            return response.json();
        })
        .then(() => {
            loadContacts();
            document.getElementById('contactName').value = '';
            document.getElementById('contactEmail').value = '';
            document.getElementById('contactTag').value = '';
        })
        .catch(error => console.error('Error adding contact:', error));
});

// Delete contact
function deleteContact(id) {
    fetch(`${API_URL}/${id}`, {
        method: 'DELETE'
    })
        .then(response => {
            if (!response.ok) throw new Error('Failed to delete contact');
            loadContacts();
        })
        .catch(error => console.error('Error deleting contact:', error));
}


// Update contact

let currentEditId = null;

function editContact(id) {
    currentEditId = id;
    document.getElementById('updateModal').classList.remove('hidden');
}

document.getElementById('saveUpdateBtn').addEventListener('click', () => {
    const newName = document.getElementById('updateName').value.trim();
    const newEmail = document.getElementById('updateEmail').value.trim();
    const tagToAdd = document.getElementById('updateTagAdd').value.trim();
    const tagToRemove = document.getElementById('updateTagRemove').value.trim();

    fetch(`${API_URL}/${currentEditId}`, {
        method: 'PUT',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ newName, newEmail, tagToAdd, tagToRemove })
    })
        .then(response => {
            if (!response.ok) throw new Error('Failed to update contact');
            loadContacts();
            document.getElementById('updateModal').classList.add('hidden');
        })
        .catch(error => console.error('Error updating contact:', error));
});

document.getElementById('cancelUpdateBtn').addEventListener('click', () => {
    document.getElementById('updateModal').classList.add('hidden');
});



// search contacts
document.getElementById('searchInput').addEventListener('input', (e) => {
    const query = e.target.value.trim();

    if (query === '') {
        loadContacts();
        return;
    }

    fetch(`${API_URL}/search?name=${query}`)
        .then(response => response.json())
        .then(data => renderContacts(data))
        .catch(error => console.error('Error searching contacts:', error));
});


// filter by tag
document.getElementById('filterTag').addEventListener('input', (e) => {
    const tag = e.target.value.trim();

    if (tag === '') {
        loadContacts();
        return;
    }

    fetch(`${API_URL}/filter?tag=${tag}`)
        .then(response => response.json())
        .then(data => renderContacts(data))
        .catch(error => console.error('Error filtering contacts:', error));
});


// ── Init ──
loadContacts();